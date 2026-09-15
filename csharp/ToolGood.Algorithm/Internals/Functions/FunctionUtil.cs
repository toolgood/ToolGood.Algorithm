using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using ToolGood.Algorithm.Internals.Visitors;
using ToolGood.Algorithm.LitJson;

namespace ToolGood.Algorithm.Internals.Functions
{
	internal static class FunctionUtil
	{
		public static readonly DateTime StartDateUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		private static readonly ConcurrentDictionary<string, Regex> _regexCache = new ConcurrentDictionary<string, Regex>();
		private const int MaxRegexCacheSize = 128;
		private static readonly TimeSpan RegexTimeout = TimeSpan.FromSeconds(1);

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
			regex = new Regex(pattern, RegexOptions.None, RegexTimeout);
			if(_regexCache.Count >= MaxRegexCacheSize) {
				_regexCache.Clear();
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

		public static bool FlattenToList(List<Operand> args, List<Operand> list)
		{
			list.Capacity = Math.Max(list.Capacity, EstimateCount(args));
			// 使用队列做顺序展开，保证输出顺序与参数从左到右的顺序一致
			var queue = new Queue<Operand>(args);
			while(queue.Count > 0) {
				var item = queue.Dequeue();
				if(item.IsArray) {
					var array = item.ArrayValue;
					for(int i = 0; i < array.Count; i++) queue.Enqueue(array[i]);
				} else if(item.IsJson) {
					var i = item.ToArray(null);
					if(i.IsError) return false;
					var array = i.ArrayValue;
					for(int j = 0; j < array.Count; j++) queue.Enqueue(array[j]);
				} else {
					list.Add(item);
				}
			}
			return true;
		}

		public static bool FlattenToList(List<Operand> args, List<decimal> list)
		{
			list.Capacity = Math.Max(list.Capacity, EstimateCount(args));
			// 使用队列做顺序展开，保证输出顺序与参数从左到右的顺序一致
			var queue = new Queue<Operand>(args);
			while(queue.Count > 0) {
				var item = queue.Dequeue();
				if(item.IsArray) {
					var array = item.ArrayValue;
					for(int i = 0; i < array.Count; i++) queue.Enqueue(array[i]);
				} else if(item.IsJson) {
					var i = item.ToArray(null);
					if(i.IsError) return false;
					var array = i.ArrayValue;
					for(int j = 0; j < array.Count; j++) queue.Enqueue(array[j]);
				} else {
					if(item.IsNumber) {
						list.Add(item.NumberValue);
					} else {
						var converted = item.ToNumber(null);
						if(converted.IsError) return false;
						list.Add(converted.NumberValue);
					}
				}
			}
			return true;
		}

		public static bool FlattenToList(Operand args, List<decimal> list)
		{
			if(args.IsError) return false;
			if(args.IsArray) return FlattenToList(args.ArrayValue, list);
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

		public static int GetCountIf(List<decimal> dbs, string s, decimal d)
		{
			int count = 0;
			for(int i = 0; i < dbs.Count; i++) {
				var item = dbs[i];
				if(CompareValues(item, d, s)) {
					count++;
				}
			}
			return count;
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

		public static decimal GetSumIf(List<decimal> dbs, string s, decimal d, List<decimal> sumdbs)
		{
			decimal sum = 0;
			int n = Math.Min(dbs.Count, sumdbs.Count);
			for(int i = 0; i < n; i++) {
				if(CompareValues(dbs[i], d, s)) {
					sum += sumdbs[i];
				}
			}
			return sum;
		}

		public static bool CompareValues(decimal a, decimal b, string ss)
		{
			if(CharUtil.Equals(ss, '<')) {
				return a < b;
			} else if(CharUtil.Equals(ss, "<=")) {
				return a <= b;
			} else if(CharUtil.Equals(ss, '>')) {
				return a > b;
			} else if(CharUtil.Equals(ss, ">=")) {
				return (a >= b);
			} else if(CharUtil.Equals(ss, "=", "==", "===")) {
				return a == b;
			}
			return a != b;
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

		public static Tuple<string, decimal> ParseSumIfMatch(string s)
		{
			if(s.Length == 0) { return null; }
			var span = s.AsSpan();
			var c = span[0];
			if(c == '>' || c == '＞') {
				if(span.Length > 1 && (span[1] == '=' || span[1] == '＝')) {
					if(decimal.TryParse(span.Slice(2).Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
						return Tuple.Create(">=", d);
					}
				} else if(decimal.TryParse(span.Slice(1).Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
					return Tuple.Create(">", d);
				}
			} else if(c == '<' || c == '＜') {
				if(span.Length > 1 && (span[1] == '=' || span[1] == '＝')) {
					if(decimal.TryParse(span.Slice(2).Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
						return Tuple.Create("<=", d);
					}
				} else if(span.Length > 1 && (span[1] == '>' || span[1] == '＞')) {
					if(decimal.TryParse(span.Slice(2).Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
						return Tuple.Create("!=", d);
					}
				} else if(decimal.TryParse(span.Slice(1).Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
					return Tuple.Create("<", d);
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
					return Tuple.Create("=", d);
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
					return Tuple.Create("!=", d);
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