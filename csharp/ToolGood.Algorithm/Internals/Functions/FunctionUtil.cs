using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ToolGood.Algorithm.LitJson;

namespace ToolGood.Algorithm.Internals.Functions
{
	/// <summary>
	/// SUMIF/COUNTIF/AVERAGEIF 的比较运算符。
	/// 由 ParseSumIfMatch 在解析条件字符串时一次性确定，
	/// 之后逐行比较只在循环外做一次 switch 分发，行内是裸 decimal 比较。
	/// </summary>
	internal enum CompareOperator
	{
		Lt,
		Le,
		Gt,
		Ge,
		Eq,
		Ne,
	}

	internal static class FunctionUtil
	{
		public static readonly DateTime StartDateUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		private static readonly ConcurrentDictionary<string, Regex> _regexCache = new ConcurrentDictionary<string, Regex>();
		private const int MaxRegexCacheSize = 128;
		private static readonly TimeSpan RegexTimeout = TimeSpan.FromSeconds(1);

#if NET8_0_OR_GREATER
		// net8+ 使用编译版正则，热路径匹配显著更快；缓存有淘汰上限，编译成本可控
		private const RegexOptions DefaultRegexOptions = RegexOptions.Compiled;
#else
		private const RegexOptions DefaultRegexOptions = RegexOptions.None;
#endif

		public static StringComparison GetStringComparison(bool ignoreCase)
		{
			return ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
		}

		/// <summary>
		/// 获取（并缓存）指定模式的正则表达式，带 1 秒超时以防御 ReDoS
		/// </summary>
		/// <exception cref="ArgumentException">模式串非法</exception>
		public static Regex GetRegex(string pattern)
		{
			if(_regexCache.TryGetValue(pattern, out var regex)) { return regex; }
			regex = new Regex(pattern, DefaultRegexOptions, RegexTimeout);
			if(_regexCache.Count >= MaxRegexCacheSize) {
				// 淘汰一半而不是全部清空：pattern 基数稍大时全清会让命中率反复塌缩
				foreach(var key in _regexCache.Keys) {
					if(_regexCache.Count < MaxRegexCacheSize / 2) { break; }
					_regexCache.TryRemove(key, out _);
				}
			}
			_regexCache[pattern] = regex;
			return regex;
		}

		/// <summary>
		/// 将 JsonData 的标量值与文本进行等值比较（全部使用固定区域性/序数比较）
		/// </summary>
		public static bool JsonValueEquals(JsonData v, string text)
		{
			if(v == null) { return false; }
			if(v.IsString) { return v.StringValue == text; }
			if(v.IsDouble) { return v.NumberValue.ToString(CultureInfo.InvariantCulture) == text; }
			if(v.IsBoolean) { return v.BooleanValue.ToString().Equals(text, StringComparison.OrdinalIgnoreCase); }
			return false;
		}

		/// <summary>
		/// 将 Operand 的标量值与文本进行等值比较（全部使用固定区域性/序数比较）
		/// </summary>
		public static bool OperandValueEquals(Operand op, string text)
		{
			if(op == null) { return false; }
			if(op.IsText) { return op.TextValue == text; }
			if(op.IsNumber) { return op.NumberValue.ToString(CultureInfo.InvariantCulture) == text; }
			if(op.IsBoolean) { return op.BooleanValue.ToString().Equals(text, StringComparison.OrdinalIgnoreCase); }
			return false;
		}

		private static int EstimateCount(List<Operand> args)
		{
			int count = 0;
			for(int i = 0; i < args.Count; i++) {
				var item = args[i];
				if(item.IsArray) {
					count += item.ArrayValue.Count;
				} else if(item.IsJson) {
					count += item.JsonValue.Count;
				} else {
					count++;
				}
			}
			return count;
		}

		/// <summary>
		/// 展平参数列表为 Operand 列表。countHint 为调用方已知的元素总数
		/// （如单数组参数的 ArrayValue.Count），传入后可跳过 EstimateCount 扫描；
		/// 传 -1 时自行估算。
		/// </summary>
		public static bool FlattenToList(List<Operand> args, List<Operand> list, int countHint = -1)
		{
			list.Capacity = Math.Max(list.Capacity, countHint >= 0 ? countHint : EstimateCount(args));
			// 快速路径 1：遇数组/Json 之前的顶层标量直接入 list，不分配 Queue
			int i = 0;
			for(; i < args.Count; i++) {
				var item = args[i];
				if(item.IsArray || item.IsJson) break;
				list.Add(item);
			}
			if(i >= args.Count) { return true; }
			// 快速路径 2：首个嵌套项恰为最后一个参数时，直接展开该数组的标量元素，仍不分配 Queue
			if(i == args.Count - 1 && args[i].IsArray) {
				var array = args[i].ArrayValue;
				int j = 0;
				for(; j < array.Count; j++) {
					var el = array[j];
					if(el.IsArray || el.IsJson) break;
					list.Add(el);
				}
				if(j >= array.Count) { return true; }
				var q2 = new Queue<Operand>(array.Count - j);
				for(; j < array.Count; j++) { q2.Enqueue(array[j]); }
				return FlattenQueue(q2, list);
			}
			// 通用路径：从首个嵌套项起入队做 BFS 展开，输出顺序与从左到右的参数顺序一致
			var queue = new Queue<Operand>(args.Count - i);
			for(; i < args.Count; i++) { queue.Enqueue(args[i]); }
			return FlattenQueue(queue, list);
		}

		/// <summary>
		/// 展平参数列表为 decimal 列表。countHint 见 <see cref="FlattenToList(List{Operand}, List{Operand}, int)"/>。
		/// </summary>
		public static bool FlattenToList(List<Operand> args, List<decimal> list, int countHint = -1)
		{
			list.Capacity = Math.Max(list.Capacity, countHint >= 0 ? countHint : EstimateCount(args));
			// 快速路径 1：顶层标量直接转换入 list。
			// SUMIF/COUNTIF/AVERAGEIF 的热点场景 args 为纯标量列表，此处零额外分配。
			// 顺序语义与原实现一致——原实现以全部参数初始化队列做 BFS 展开，
			// 先直填首个嵌套项之前的标量，剩余部分入队后展开次序不变。
			int i = 0;
			for(; i < args.Count; i++) {
				var item = args[i];
				if(item.IsArray || item.IsJson) break;
				if(item.IsNumber) {
					list.Add(item.NumberValue);
				} else {
					var converted = item.ToNumber(null);
					if(converted.IsError) return false;
					list.Add(converted.NumberValue);
				}
			}
			if(i >= args.Count) { return true; }
			// 快速路径 2：首个嵌套项恰为最后一个参数（COUNT/MEDIAN/PRODUCT 的 sum(arr) 形态），
			// 直接展开该数组的标量元素，仍不分配 Queue
			if(i == args.Count - 1 && args[i].IsArray) {
				var array = args[i].ArrayValue;
				int j = 0;
				for(; j < array.Count; j++) {
					var el = array[j];
					if(el.IsArray || el.IsJson) break;
					if(el.IsNumber) {
						list.Add(el.NumberValue);
					} else {
						var converted = el.ToNumber(null);
						if(converted.IsError) return false;
						list.Add(converted.NumberValue);
					}
				}
				if(j >= array.Count) { return true; }
				// 数组内仍有嵌套：剩余元素入队。BFS 会先排完该数组顶层剩余元素再展开嵌套子元素，
				// 此处入队次序（嵌套项在前、剩余标量在后）经队列展开后与之一致。
				var q2 = new Queue<Operand>(array.Count - j);
				for(; j < array.Count; j++) { q2.Enqueue(array[j]); }
				return FlattenQueue(q2, list);
			}
			// 通用路径：从首个嵌套项起入队做 BFS 展开，输出顺序与从左到右的参数顺序一致
			var queue = new Queue<Operand>(args.Count - i);
			for(; i < args.Count; i++) { queue.Enqueue(args[i]); }
			return FlattenQueue(queue, list);
		}

		/// <summary>
		/// 队列 BFS 展开（FlattenToList 的通用路径）。单独拆出并禁止内联：
		/// 快速路径保持小巧，本方法的代码生成则与原队列实现一致。
		/// </summary>
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static bool FlattenQueue(Queue<Operand> queue, List<decimal> list)
		{
			while(queue.Count > 0) {
				var item = queue.Dequeue();
				if(item.IsArray) {
					var array = item.ArrayValue;
					for(int j = 0; j < array.Count; j++) queue.Enqueue(array[j]);
				} else if(item.IsJson) {
					var arr = item.ToArray(null);
					if(arr.IsError) return false;
					var array = arr.ArrayValue;
					for(int j = 0; j < array.Count; j++) queue.Enqueue(array[j]);
				} else if(item.IsNumber) {
					list.Add(item.NumberValue);
				} else {
					var converted = item.ToNumber(null);
					if(converted.IsError) return false;
					list.Add(converted.NumberValue);
				}
			}
			return true;
		}

		/// <summary>
		/// 队列 BFS 展开（List&lt;Operand&gt; 重载的通用路径）。见 <see cref="FlattenQueue(Queue{Operand}, List{decimal})"/>。
		/// </summary>
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static bool FlattenQueue(Queue<Operand> queue, List<Operand> list)
		{
			while(queue.Count > 0) {
				var item = queue.Dequeue();
				if(item.IsArray) {
					var array = item.ArrayValue;
					for(int j = 0; j < array.Count; j++) queue.Enqueue(array[j]);
				} else if(item.IsJson) {
					var arr = item.ToArray(null);
					if(arr.IsError) return false;
					var array = arr.ArrayValue;
					for(int j = 0; j < array.Count; j++) queue.Enqueue(array[j]);
				} else {
					list.Add(item);
				}
			}
			return true;
		}

		public static bool FlattenToList(Operand args, List<decimal> list)
		{
			if(args.IsError) return false;
			if(args.IsArray) return FlattenToList(args.ArrayValue, list, args.ArrayValue.Count);
			if(args.IsJson) {
				var i = args.ToArray(null);
				if(i.IsError) return false;
				return FlattenToList(i.ArrayValue, list);
			}
			if(args.IsNumber) {
				list.Add(args.NumberValue);
			} else {
				var converted = args.ToNumber(null);
				if(converted.IsError) return false;
				list.Add(converted.NumberValue);
			}
			return true;
		}

		/// <summary>
		/// 单操作数展平成 Operand 列表。与 “FlattenToList(new[]{ args }, list)” 等价，
		/// 供 COUNT 等需要逐项保留 Operand（判断能否转数值）的函数免去参数中转列表。
		/// </summary>
		public static bool FlattenToList(Operand args, List<Operand> list)
		{
			if(args.IsError) { return false; }
			if(args.IsArray) {
				return FlattenToList(args.ArrayValue, list, args.ArrayValue.Count);
			}
			if(args.IsJson) {
				var i = args.ToArray(null);
				if(i.IsError) return false;
				return FlattenToList(i.ArrayValue, list);
			}
			list.Add(args);
			return true;
		}

		public static bool FlattenToList(Operand args, List<string> list)
		{
			if(args.IsError) { return false; }
			if(args.IsArray) {
				var array = args.ArrayValue;
				list.Capacity = Math.Max(list.Capacity, array.Count);
				for(int i = 0; i < array.Count; i++) {
					var item = array[i];
					if(item.IsArray || item.IsJson) {
						if(!FlattenToList(item, list)) return false;
					} else {
						var converted = item.ToText(null);
						if(converted.IsError) { return false; }
						list.Add(converted.TextValue);
					}
				}
			} else if(args.IsJson) {
				var i = args.ToArray(null);
				if(i.IsError) { return false; }
				return FlattenToList(i, list);
			} else {
				var converted = args.ToText(null);
				if(converted.IsError) { return false; }
				list.Add(converted.TextValue);
			}
			return true;
		}

		public static int GetCountIf(List<decimal> dbs, decimal d)
		{
			int count = 0;
			for(int i = 0; i < dbs.Count; i++) {
				var item = dbs[i];
				if(item == d) {
					count++;
				}
			}
			return count;
		}

		public static int GetCountIf(List<decimal> dbs, CompareOperator op, decimal d)
		{
			// 比较分支提升到循环外：每行只剩裸 decimal 比较（微基准与理论下限持平），
			// 且行内不再做 CharUtil.Equals 逐字符字符串比较。
			// 注意：switch 不能放进逐行循环——行内枚举分发实测比原字符串逐字符比较更慢
			// （10 万行：1.3ms vs 1.0ms），分发必须留在循环外（0.67ms，与裸比较下限持平）。
			switch(op) {
				case CompareOperator.Lt: {
					int count = 0;
					for(int i = 0; i < dbs.Count; i++) { if(dbs[i] < d) { count++; } }
					return count;
				}
				case CompareOperator.Le: {
					int count = 0;
					for(int i = 0; i < dbs.Count; i++) { if(dbs[i] <= d) { count++; } }
					return count;
				}
				case CompareOperator.Gt: {
					int count = 0;
					for(int i = 0; i < dbs.Count; i++) { if(dbs[i] > d) { count++; } }
					return count;
				}
				case CompareOperator.Ge: {
					int count = 0;
					for(int i = 0; i < dbs.Count; i++) { if(dbs[i] >= d) { count++; } }
					return count;
				}
				case CompareOperator.Eq: {
					int count = 0;
					for(int i = 0; i < dbs.Count; i++) { if(dbs[i] == d) { count++; } }
					return count;
				}
				default: {
					int count = 0;
					for(int i = 0; i < dbs.Count; i++) { if(dbs[i] != d) { count++; } }
					return count;
				}
			}
		}

		public static decimal GetSumIf(List<decimal> dbs, decimal d, List<decimal> sumdbs)
		{
			decimal sum = 0;
			int n = Math.Min(dbs.Count, sumdbs.Count);
			for(int i = 0; i < n; i++) {
				var item = dbs[i];
				if(item == d) {
					sum += sumdbs[i];
				}
			}
			return sum;
		}

		public static decimal GetSumIf(List<decimal> dbs, CompareOperator op, decimal d, List<decimal> sumdbs)
		{
			int n = Math.Min(dbs.Count, sumdbs.Count);
			switch(op) {
				case CompareOperator.Lt: {
					decimal sum = 0;
					for(int i = 0; i < n; i++) { if(dbs[i] < d) { sum += sumdbs[i]; } }
					return sum;
				}
				case CompareOperator.Le: {
					decimal sum = 0;
					for(int i = 0; i < n; i++) { if(dbs[i] <= d) { sum += sumdbs[i]; } }
					return sum;
				}
				case CompareOperator.Gt: {
					decimal sum = 0;
					for(int i = 0; i < n; i++) { if(dbs[i] > d) { sum += sumdbs[i]; } }
					return sum;
				}
				case CompareOperator.Ge: {
					decimal sum = 0;
					for(int i = 0; i < n; i++) { if(dbs[i] >= d) { sum += sumdbs[i]; } }
					return sum;
				}
				case CompareOperator.Eq: {
					decimal sum = 0;
					for(int i = 0; i < n; i++) { if(dbs[i] == d) { sum += sumdbs[i]; } }
					return sum;
				}
				default: {
					decimal sum = 0;
					for(int i = 0; i < n; i++) { if(dbs[i] != d) { sum += sumdbs[i]; } }
					return sum;
				}
			}
		}

		public static decimal GetGcd(List<decimal> list)
		{
			if(list.Count == 0) return 1;
			
			decimal g = Math.Truncate(list[0]);
			for(int i = 1; i < list.Count; i++) {
				g = GetGcd(g, Math.Truncate(list[i]));
				if(g == 1) break;
			}
			return g;
		}

		public static decimal GetGcd(decimal a, decimal b)
		{
			while(b != 0) {
				decimal t = b;
				b = a % b;
				a = t;
			}
			return a;
		}

		public static decimal GetLcm(List<decimal> list)
		{
			if(list.Count == 0) return 1;

			// 与 GetGcd 保持一致的归约方式：不跳过任何元素，
			// 含 0 时结果为 0（与 Excel 的 LCM 一致），全部为 1 时结果为 1
			decimal a = Math.Truncate(list[0]);
			for(int i = 1; i < list.Count; i++) {
				decimal b = Math.Truncate(list[i]);
				if(a == 0 || b == 0) { a = 0; continue; }
				var g = b > a ? GetGcd(b, a) : GetGcd(a, b);
				a = a / g * b;
			}
			return a;
		}

		public static decimal GetFactorial(decimal a)
		{
			if(a <= 0) { return 1; }
			a = Math.Truncate(a);
			decimal r = 1;
			for(decimal i = a; i > 0; i--) {
				r *= i;
			}
			return r;
		}

		public static Tuple<CompareOperator, decimal> ParseSumIfMatch(string s)
		{
			if(s.Length == 0) { return null; }
			var span = s.AsSpan();
			var c = span[0];
			if(c == '>' || c == '＞') {
				if(span.Length > 1 && (span[1] == '=' || span[1] == '＝')) {
					if(decimal.TryParse(span.Slice(2).Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
						return Tuple.Create(CompareOperator.Ge, d);
					}
				} else if(decimal.TryParse(span.Slice(1).Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
					return Tuple.Create(CompareOperator.Gt, d);
				}
			} else if(c == '<' || c == '＜') {
				if(span.Length > 1 && (span[1] == '=' || span[1] == '＝')) {
					if(decimal.TryParse(span.Slice(2).Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
						return Tuple.Create(CompareOperator.Le, d);
					}
				} else if(span.Length > 1 && (span[1] == '>' || span[1] == '＞')) {
					if(decimal.TryParse(span.Slice(2).Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
						return Tuple.Create(CompareOperator.Ne, d);
					}
				} else if(decimal.TryParse(span.Slice(1).Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
					return Tuple.Create(CompareOperator.Lt, d);
				}
			} else if(c == '=' || c == '＝') {
				var index = 1;
				if(span.Length > 1 && (span[1] == '=' || span[1] == '＝')) {
					index = 2;
					if(span.Length > 2 && (span[2] == '=' || span[2] == '＝')) {
						index = 3;
					}
				}
				if(decimal.TryParse(span.Slice(index).Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
					return Tuple.Create(CompareOperator.Eq, d);
				}
			} else if(c == '!' || c == '！') {
				var index = 1;
				if(span.Length > 1 && (span[1] == '=' || span[1] == '＝')) {
					index = 2;
					if(span.Length > 2 && (span[2] == '=' || span[2] == '＝')) {
						index = 3;
					}
				}
				if(decimal.TryParse(span.Slice(index).Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
					return Tuple.Create(CompareOperator.Ne, d);
				}
			}
			return null;
		}
	
		public static bool TryParseBoolean(string TextValue, out bool boolValue)
		{
			var span = TextValue.AsSpan();
			var len = span.Length;
			switch(len) {
				case 1: {
					var c = span[0];
					if(c == '1' || c == '是' || c == '有') { boolValue = true; return true; }
					if(c == '0' || c == '否' || c == '无') { boolValue = false; return true; }
					break;
				}
				case 2: {
					if(span.Equals("no", StringComparison.OrdinalIgnoreCase)) { boolValue = false; return true; }
					if(span.SequenceEqual("不是".AsSpan())) { boolValue = false; return true; }
					if(span.SequenceEqual("没有".AsSpan())) { boolValue = false; return true; }
					break;
				}
				case 3: {
					if(span.Equals("yes", StringComparison.OrdinalIgnoreCase)) { boolValue = true; return true; }
					break;
				}
				case 4: {
					if(span.Equals("true", StringComparison.OrdinalIgnoreCase)) { boolValue = true; return true; }
					break;
				}
				case 5: {
					if(span.Equals("false", StringComparison.OrdinalIgnoreCase)) { boolValue = false; return true; }
					break;
				}
			}
			boolValue = false;
			return false;
		}

		public static decimal QuickSelect(List<decimal> list, int k, bool largest)
		{
			if(list.Count == 1) return list[0];

			int targetIndex = largest ? list.Count - 1 - k : k;
			int left = 0, right = list.Count - 1;
			while(left < right) {
				int pivotIndex = SelectPivot(list, left, right);
				pivotIndex = Partition(list, left, right, pivotIndex);
				if(targetIndex == pivotIndex) {
					return list[targetIndex];
				} else if(targetIndex < pivotIndex) {
					right = pivotIndex - 1;
				} else {
					left = pivotIndex + 1;
				}
			}
			return list[left];
		}

		private static int SelectPivot(List<decimal> list, int left, int right)
		{
			int mid = left + (right - left) / 2;
			decimal a = list[left], b = list[mid], c = list[right];
			if(a < b) {
				if(b < c) return mid;
				if(a < c) return right;
				return left;
			}
			if(a < c) return left;
			if(b < c) return right;
			return mid;
		}

		private static int Partition(List<decimal> list, int left, int right, int pivotIndex)
		{
			decimal pivot = list[pivotIndex];
			(list[pivotIndex], list[right]) = (list[right], list[pivotIndex]);
			int storeIndex = left;
			for(int i = left; i < right; i++) {
				if(list[i] < pivot) {
					(list[storeIndex], list[i]) = (list[i], list[storeIndex]);
					storeIndex++;
				}
			}
			(list[storeIndex], list[right]) = (list[right], list[storeIndex]);
			return storeIndex;
		}

		public static int GetRank(List<decimal> values, decimal num, bool descending)
		{
			int rank = 1;
			int count = 0;
			for(int i = 0; i < values.Count; i++) {
				if(values[i] == num) {
					count++;
				} else if((descending && values[i] > num) || (!descending && values[i] < num)) {
					rank++;
				}
			}
			return count > 0 ? rank : 0;
		}
	}
}