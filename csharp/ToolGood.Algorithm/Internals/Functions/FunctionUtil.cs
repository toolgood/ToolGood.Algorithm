using System;
using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
using ToolGood.Algorithm.Internals.Visitors;

namespace ToolGood.Algorithm.Internals.Functions
{
	internal class FunctionUtil
	{
		public static readonly DateTime StartDateUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static StringComparison GetStringComparison(bool ignoreCase)
		{
			return ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
		}

		private static int EstimateCount(List<Operand> args)
		{
			int count = 0;
			for(int i = 0; i < args.Count; i++) {
				var item = args[i];
				if(item.IsArray) {
					count += item.ArrayValue.Count;
				} else if(item.IsJson) {
					count += 8;
				} else {
					count++;
				}
			}
			return count;
		}

		private static int EstimateCount(Operand args)
		{
			if(args.IsArray) return args.ArrayValue.Count;
			if(args.IsJson) return 8;
			return 1;
		}

		public static bool FlattenToList(List<Operand> args, List<Operand> list)
		{
			list.Capacity = Math.Max(list.Capacity, EstimateCount(args));
			var stack = ArrayPool<Operand>.Shared.Rent(Math.Max(16, args.Count));
			int stackIndex = 0;
			
			for(int i = 0; i < args.Count; i++) {
				stack[stackIndex++] = args[i];
			}

			while(stackIndex > 0) {
				var item = stack[--stackIndex];

				if(item.IsArray) {
					var array = item.ArrayValue;
					if(stackIndex + array.Count >= stack.Length) {
						var newStack = ArrayPool<Operand>.Shared.Rent(stack.Length * 2 + array.Count);
						Array.Copy(stack, 0, newStack, 0, stackIndex);
						ArrayPool<Operand>.Shared.Return(stack);
						stack = newStack;
					}
					for(int i = array.Count - 1; i >= 0; i--) {
						stack[stackIndex++] = array[i];
					}
				} else if(item.IsJson) {
					var i = item.ToArray(null);
					if(i.IsError) {
						ArrayPool<Operand>.Shared.Return(stack);
						return false;
					}
					var array = i.ArrayValue;
					if(stackIndex + array.Count >= stack.Length) {
						var newStack = ArrayPool<Operand>.Shared.Rent(stack.Length * 2 + array.Count);
						Array.Copy(stack, 0, newStack, 0, stackIndex);
						ArrayPool<Operand>.Shared.Return(stack);
						stack = newStack;
					}
					for(int j = array.Count - 1; j >= 0; j--) {
						stack[stackIndex++] = array[j];
					}
				} else {
					list.Add(item);
				}
			}
			ArrayPool<Operand>.Shared.Return(stack);
			return true;
		}

		public static bool FlattenToList(List<Operand> args, List<decimal> list)
		{
			list.Capacity = Math.Max(list.Capacity, EstimateCount(args));
			var stack = ArrayPool<Operand>.Shared.Rent(Math.Max(16, args.Count));
			int stackIndex = 0;
			
			for(int i = 0; i < args.Count; i++) {
				stack[stackIndex++] = args[i];
			}

			while(stackIndex > 0) {
				var item = stack[--stackIndex];

				if(item.IsArray) {
					var array = item.ArrayValue;
					if(stackIndex + array.Count >= stack.Length) {
						var newStack = ArrayPool<Operand>.Shared.Rent(stack.Length * 2 + array.Count);
						Array.Copy(stack, 0, newStack, 0, stackIndex);
						ArrayPool<Operand>.Shared.Return(stack);
						stack = newStack;
					}
					for(int i = array.Count - 1; i >= 0; i--) {
						stack[stackIndex++] = array[i];
					}
				} else if(item.IsJson) {
					var i = item.ToArray(null);
					if(i.IsError) {
						ArrayPool<Operand>.Shared.Return(stack);
						return false;
					}
					var array = i.ArrayValue;
					if(stackIndex + array.Count >= stack.Length) {
						var newStack = ArrayPool<Operand>.Shared.Rent(stack.Length * 2 + array.Count);
						Array.Copy(stack, 0, newStack, 0, stackIndex);
						ArrayPool<Operand>.Shared.Return(stack);
						stack = newStack;
					}
					for(int j = array.Count - 1; j >= 0; j--) {
						stack[stackIndex++] = array[j];
					}
				} else {
					if(item.IsNumber) {
						list.Add(item.NumberValue);
					} else {
						var converted = item.ToNumber(null);
						if(converted.IsError) {
							ArrayPool<Operand>.Shared.Return(stack);
							return false;
						}
						list.Add(converted.NumberValue);
					}
				}
			}
			ArrayPool<Operand>.Shared.Return(stack);
			return true;
		}

		public static bool FlattenToList(Operand args, List<decimal> list)
		{
			if(args.IsError) { return false; }
			if(args.IsArray) {
				var array = args.ArrayValue;
				list.Capacity = Math.Max(list.Capacity, array.Count);
				var stack = ArrayPool<Operand>.Shared.Rent(Math.Max(16, array.Count));
				int stackIndex = 0;
				
				for(int i = array.Count - 1; i >= 0; i--) {
					stack[stackIndex++] = array[i];
				}

				while(stackIndex > 0) {
					var item = stack[--stackIndex];

					if(item.IsArray) {
						var subArray = item.ArrayValue;
						if(stackIndex + subArray.Count >= stack.Length) {
							var newStack = ArrayPool<Operand>.Shared.Rent(stack.Length * 2 + subArray.Count);
							Array.Copy(stack, 0, newStack, 0, stackIndex);
							ArrayPool<Operand>.Shared.Return(stack);
							stack = newStack;
						}
						for(int i = subArray.Count - 1; i >= 0; i--) {
							stack[stackIndex++] = subArray[i];
						}
					} else if(item.IsJson) {
						var i = item.ToArray(null);
						if(i.IsError) {
							ArrayPool<Operand>.Shared.Return(stack);
							return false;
						}
						var subArray = i.ArrayValue;
						if(stackIndex + subArray.Count >= stack.Length) {
							var newStack = ArrayPool<Operand>.Shared.Rent(stack.Length * 2 + subArray.Count);
							Array.Copy(stack, 0, newStack, 0, stackIndex);
							ArrayPool<Operand>.Shared.Return(stack);
							stack = newStack;
						}
						for(int j = subArray.Count - 1; j >= 0; j--) {
							stack[stackIndex++] = subArray[j];
						}
					} else {
						if(item.IsNumber) {
							list.Add(item.NumberValue);
						} else {
							var converted = item.ToNumber(null);
							if(converted.IsError) {
								ArrayPool<Operand>.Shared.Return(stack);
								return false;
							}
							list.Add(converted.NumberValue);
						}
					}
				}
				ArrayPool<Operand>.Shared.Return(stack);
				return true;
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
			if(list.Count == 0) return 1;
			
			int g = (int)list[0];
			for(int i = 1; i < list.Count; i++) {
				g = GetGcd(g, (int)list[i]);
				if(g == 1) break;
			}
			return g;
		}

		public static int GetGcd(int a, int b)
		{
			while(b != 0) {
				int t = b;
				b = a % b;
				a = t;
			}
			return a;
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
			return QuickSelectCore(list, 0, list.Count - 1, targetIndex);
		}

		private static decimal QuickSelectCore(List<decimal> list, int left, int right, int k)
		{
			while(left < right) {
				int pivotIndex = Partition(list, left, right);
				if(k == pivotIndex) {
					return list[k];
				} else if(k < pivotIndex) {
					right = pivotIndex - 1;
				} else {
					left = pivotIndex + 1;
				}
			}
			return list[left];
		}

		private static int Partition(List<decimal> list, int left, int right)
		{
			decimal pivot = list[right];
			int i = left;
			for(int j = left; j < right; j++) {
				if(list[j] <= pivot) {
					Swap(list, i, j);
					i++;
				}
			}
			Swap(list, i, right);
			return i;
		}

		private static void Swap(List<decimal> list, int i, int j)
		{
			if(i != j) {
				decimal temp = list[i];
				list[i] = list[j];
				list[j] = temp;
			}
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