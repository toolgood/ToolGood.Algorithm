using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ToolGood.Algorithm.Internals.Visitors;

namespace ToolGood.Algorithm.Internals.Functions
{
	internal class FunctionUtil
	{
		public static readonly DateTime StartDateUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		/// <summary>
		/// 获取字符串比较选项
		/// </summary>
		/// <param name="ignoreCase"></param>
		/// <returns></returns>
		public static StringComparison GetStringComparison(bool ignoreCase)
		{
			return ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
		}

		private static bool FlattenToListCore<T>(List<Operand> args, List<T> list, Func<Operand, Operand> converter, Func<Operand, T> valueGetter)
		{
			var queue = new Queue<Operand>(args.Count);
			for(int i = 0; i < args.Count; i++) {
				queue.Enqueue(args[i]);
			}

			while(queue.Count > 0) {
				var item = queue.Dequeue();

				if(item.IsArray) {
					var array = item.ArrayValue;
					for(int i = 0; i < array.Count; i++) {
						queue.Enqueue(array[i]);
					}
				} else if(item.IsJson) {
					var i = item.ToArray(null);
					if(i.IsError) { return false; }
					var array = i.ArrayValue;
					for(int j = 0; j < array.Count; j++) {
						queue.Enqueue(array[j]);
					}
				} else {
					var converted = converter(item);
					if(converted.IsError) { return false; }
					list.Add(valueGetter(converted));
				}
			}
			return true;
		}

		private static bool FlattenToListCore<T>(Operand args, List<T> list, Func<Operand, Operand> converter, Func<Operand, T> valueGetter)
		{
			if(args.IsError) { return false; }
			if(args.IsArray) {
				return FlattenToListCore(args.ArrayValue, list, converter, valueGetter);
			} else if(args.IsJson) {
				var i = args.ToArray(null);
				if(i.IsError) { return false; }
				return FlattenToListCore(i.ArrayValue, list, converter, valueGetter);
			} else {
				var converted = converter(args);
				if(converted.IsError) { return false; }
				list.Add(valueGetter(converted));
			}
			return true;
		}

		public static bool FlattenToList(List<Operand> args, List<Operand> list)
		{
			return FlattenToListCore(args, list, obj => obj, obj => obj);
		}

		public static bool FlattenToList(List<Operand> args, List<decimal> list)
		{
			var queue = new Queue<Operand>(args.Count);
			for(int i = 0; i < args.Count; i++) {
				queue.Enqueue(args[i]);
			}

			while(queue.Count > 0) {
				var item = queue.Dequeue();

				if(item.IsArray) {
					var array = item.ArrayValue;
					for(int i = 0; i < array.Count; i++) {
						queue.Enqueue(array[i]);
					}
				} else if(item.IsJson) {
					var i = item.ToArray(null);
					if(i.IsError) { return false; }
					var array = i.ArrayValue;
					for(int j = 0; j < array.Count; j++) {
						queue.Enqueue(array[j]);
					}
				} else {
					if(item.IsNumber) {
						list.Add(item.NumberValue);
					} else {
						var converted = item.ToNumber(null);
						if(converted.IsError) { return false; }
						list.Add(converted.NumberValue);
					}
				}
			}
			return true;
		}

		public static bool FlattenToList(Operand args, List<decimal> list)
		{
			if(args.IsError) { return false; }
			if(args.IsArray) {
				return FlattenToList(args.ArrayValue, list);
			} else if(args.IsJson) {
				var i = args.ToArray(null);
				if(i.IsError) { return false; }
				return FlattenToList(i.ArrayValue, list);
			} else {
				if(args.IsNumber) {
					list.Add(args.NumberValue);
				} else {
					var converted = args.ToNumber(null);
					if(converted.IsError) { return false; }
					list.Add(converted.NumberValue);
				}
			}
			return true;
		}

		public static bool FlattenToList(Operand args, List<string> list)
		{
			return FlattenToListCore(args, list, obj => obj.ToText(null), obj => obj.TextValue);
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
			for(int i = 0; i < dbs.Count; i++) {
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
			for(int i = 0; i < dbs.Count; i++) {
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

		public static int GetGcd(List<decimal> list)
		{
			if(list.Count < 2) return list.Count == 1 ? (int)list[0] : 1;
			
			int min1 = (int)list[0], min2 = (int)list[1];
			if(min1 > min2) { var t = min1; min1 = min2; min2 = t; }
			
			for(int i = 2; i < list.Count; i++) {
				int val = (int)list[i];
				if(val < min1) { min2 = min1; min1 = val; }
				else if(val < min2) { min2 = val; }
			}
			
			var g = GetGcd(min2, min1);
			for(int i = 0; i < list.Count; i++) {
				int val = (int)list[i];
				if(val != min1 && val != min2) {
					g = GetGcd(val > g ? val : g, val > g ? g : val);
				}
			}
			return g;
		}

		public static int GetGcd(int a, int b)
		{
			if(b == 1) { return 1; }
			if(b == 0) { return a; }
			return GetGcd(b, a % b);
		}

		public static int GetLcm(List<decimal> list)
		{
			if(list.Count == 0) return 1;
			
			int a = 0;
			bool foundFirst = false;
			
			for(int i = 0; i < list.Count; i++) {
				int val = (int)list[i];
				if(val <= 1) continue;
				
				if(!foundFirst) {
					a = val;
					foundFirst = true;
					continue;
				}
				
				int b = val;
				int g = b > a ? GetGcd(b, a) : GetGcd(a, b);
				a = a / g * b;
			}
			return foundFirst ? a : 1;
		}

		public static int GetFactorial(int a)
		{
			if(a <= 0) { return 1; }
			int r = 1;
			for(int i = a; i > 0; i--) {
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

		public static int Compare(decimal t1, decimal t2)
		{
			var b = t1 - t2;
			if(b == 0) {
				return 0;
			} else if(b > 0) {
				return 1;
			}
			return -1;
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
	}
}