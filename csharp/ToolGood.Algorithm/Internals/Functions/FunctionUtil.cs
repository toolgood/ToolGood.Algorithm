using System;
using System.Collections.Generic;
using System.Linq;
using ToolGood.Algorithm.Internals.Visitors;

namespace ToolGood.Algorithm.Internals.Functions
{
	internal class FunctionUtil
	{
		public static readonly DateTime StartDateUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		#region ConvertTo

		/// <summary>
		/// 验证并转换参数为文本
		/// </summary>
		/// <param name="arg"></param>
		/// <param name="functionName"></param>
		/// <param name="paramIndex"></param>
		/// <returns></returns>
		public static Operand ConvertToText(Operand arg, string functionName, int paramIndex)
		{
			return arg.ToText("Function '{0}' parameter {1} is error!", functionName, paramIndex);
		}

		/// <summary>
		/// 验证并转换参数为布尔值
		/// </summary>
		/// <param name="arg"></param>
		/// <param name="functionName"></param>
		/// <param name="paramIndex"></param>
		/// <returns></returns>
		public static Operand ConvertToBoolean(Operand arg, string functionName, int paramIndex)
		{
			return arg.ToBoolean("Function '{0}' parameter {1} is error!", functionName, paramIndex);
		}

		/// <summary>
		/// 验证并转换参数为数字
		/// </summary>
		/// <param name="arg"></param>
		/// <param name="functionName"></param>
		/// <param name="paramIndex"></param>
		/// <returns></returns>
		public static Operand ConvertToNumber(Operand arg, string functionName, int paramIndex)
		{
			return arg.ToNumber("Function '{0}' parameter {1} is error!", functionName, paramIndex);
		}

		/// <summary>
		/// 验证并转换参数为数组
		/// </summary>
		/// <param name="arg"></param>
		/// <param name="functionName"></param>
		/// <param name="paramIndex"></param>
		/// <returns></returns>
		public static Operand ConvertToArray(Operand arg, string functionName, int paramIndex)
		{
			return arg.ToArray("Function '{0}' parameter {1} is error!", functionName, paramIndex);
		}

		/// <summary>
		/// 验证并转换参数为日期
		/// </summary>
		/// <param name="arg"></param>
		/// <param name="functionName"></param>
		/// <param name="paramIndex"></param>
		/// <returns></returns>
		public static Operand ConvertToDate(Operand arg, string functionName, int paramIndex)
		{
			return arg.ToMyDate("Function '{0}' parameter {1} is error!", functionName, paramIndex);
		}

		/// <summary>
		/// 获取字符串比较选项
		/// </summary>
		/// <param name="ignoreCase"></param>
		/// <returns></returns>
		public static StringComparison GetStringComparison(bool ignoreCase)
		{
			return ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
		}

		#endregion Parameter Validation

		private static bool F_base_GetList<T>(List<Operand> args, List<T> list, Func<Operand, Operand> converter, Func<Operand, T> valueGetter)
		{
			foreach(var item in args) {
				if(item.IsArray) {
					var o = F_base_GetList(item.ArrayValue, list, converter, valueGetter);
					if(o == false) { return false; }
				} else if(item.IsJson) {
					var i = item.ToArray(null);
					if(i.IsError) { return false; }
					var o = F_base_GetList(i.ArrayValue, list, converter, valueGetter);
					if(o == false) { return false; }
				} else {
					var converted = converter(item);
					if(converted.IsError) { return false; }
					list.Add(valueGetter(converted));
				}
			}
			return true;
		}

		private static bool F_base_GetList<T>(Operand args, List<T> list, Func<Operand, Operand> converter, Func<Operand, T> valueGetter)
		{
			if(args.IsError) { return false; }
			if(args.IsArray) {
				var o = F_base_GetList(args.ArrayValue, list, converter, valueGetter);
				if(o == false) { return false; }
			} else if(args.IsJson) {
				var i = args.ToArray(null);
				if(i.IsError) { return false; }
				var o = F_base_GetList(i.ArrayValue, list, converter, valueGetter);
				if(o == false) { return false; }
			} else {
				var converted = converter(args);
				if(converted.IsError) { return false; }
				list.Add(valueGetter(converted));
			}
			return true;
		}

		public static bool F_base_GetList(List<Operand> args, List<decimal> list)
		{
			return F_base_GetList(args, list, 
				obj => obj.IsNumber ? obj : obj.ToNumber(null),
				obj => obj.NumberValue);
		}

		public static bool F_base_GetList(List<Operand> args, List<double> list)
		{
			return F_base_GetList(args, list, 
				obj => obj.IsNumber ? obj : obj.ToNumber(null),
				obj => obj.DoubleValue);
		}

		public static bool F_base_GetList(Operand args, List<decimal> list)
		{
			return F_base_GetList(args, list, 
				obj => obj.IsNumber ? obj : obj.ToNumber(null),
				obj => obj.NumberValue);
		}

		public static bool F_base_GetList(Operand args, List<double> list)
		{
			return F_base_GetList(args, list, 
				obj => obj.IsNumber ? obj : obj.ToNumber(null),
				obj => obj.DoubleValue);
		}

		public static bool F_base_GetList(Operand args, List<string> list)
		{
			return F_base_GetList(args, list, 
				obj => obj.ToText(null),
				obj => obj.TextValue);
		}

		public static bool F_base_GetList(List<Operand> args, List<string> list)
		{
			return F_base_GetList(args, list, 
				obj => obj.ToText(null),
				obj => obj.TextValue);
		}

		public static int F_base_countif(List<decimal> dbs, decimal d)
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

		public static int F_base_countif(List<decimal> dbs, string s, decimal d)
		{
			int count = 0;
			for(int i = 0; i < dbs.Count; i++) {
				var item = dbs[i];
				if(F_base_compare(item, d, s)) {
					count++;
				}
			}
			return count;
		}

		public static decimal F_base_sumif(List<decimal> dbs, decimal d, List<decimal> sumdbs)
		{
			decimal sum = 0;
			for(int i = 0; i < dbs.Count; i++) {
				var item = dbs[i];
				if(item == d) {
					sum += sumdbs[i];
				}
				//if (Math.Round(item, 10, MidpointRounding.AwayFromZero) == d) {
				//	sum += item;
				//}
			}
			return sum;
		}

		public static decimal F_base_sumif(List<decimal> dbs, string s, decimal d, List<decimal> sumdbs)
		{
			decimal sum = 0;
			for(int i = 0; i < dbs.Count; i++) {
				if(F_base_compare(dbs[i], d, s)) {
					sum += sumdbs[i];
				}
			}
			return sum;
		}

		public static bool F_base_compare(decimal a, decimal b, string ss)
		{
			if(CharUtil.Equals(ss, '<')) {
				return a < b;
				//return Math.Round(a - b, 12, MidpointRounding.AwayFromZero) < 0;
			} else if(CharUtil.Equals(ss, "<=")) {
				return a <= b;
				//return Math.Round(a - b, 12, MidpointRounding.AwayFromZero) <= 0;
			} else if(CharUtil.Equals(ss, '>')) {
				return a > b;
				//return Math.Round(a - b, 12, MidpointRounding.AwayFromZero) > 0;
			} else if(CharUtil.Equals(ss, ">=")) {
				return (a >= b);
				//return Math.Round(a - b, 12, MidpointRounding.AwayFromZero) >= 0;
			} else if(CharUtil.Equals(ss, "=", "==", "===")) {
				return a == b;
				//return Math.Round(a - b, 12, MidpointRounding.AwayFromZero) == 0;
			}
			return a != b;
			//return Math.Round(a - b, 12, MidpointRounding.AwayFromZero) != 0;
		}

		public static int F_base_gcd(List<decimal> list)
		{
			list = list.OrderBy(q => q).ToList();
			var g = F_base_gcd((int)list[1], (int)list[0]);
			for(int i = 2; i < list.Count; i++) {
				g = F_base_gcd((int)list[i], g);
			}
			return g;
		}

		public static int F_base_gcd(int a, int b)
		{
			if(b == 1) { return 1; }
			if(b == 0) { return a; }
			return F_base_gcd(b, a % b);
		}

		public static int F_base_lgm(List<decimal> list)
		{
			list = list.OrderBy(q => q).ToList();
			list.RemoveAll(q => q <= 1);

			int a = (int)list[0];
			for(int i = 1; i < list.Count; i++) {
				int b = (int)list[i];
				int g = b > a ? F_base_gcd(b, a) : F_base_gcd(a, b);
				a = a / g * b;
			}
			return a;
		}

		public static int F_base_Factorial(int a)
		{
			if(a <= 0) { return 1; }
			int r = 1;
			for(int i = a; i > 0; i--) {
				r *= i;
			}
			return r;
		}

		public static Tuple<string, decimal> sumifMatch(string s)
		{
			var c = s[0];
			if(c == '>' || c == '＞') {
				if(s.Length > 1 && (s[1] == '=' || s[1] == '＝')) {
					if(decimal.TryParse(s.AsSpan(2).Trim(), out decimal d)) {
						return Tuple.Create(">=", d);
					}
				} else if(decimal.TryParse(s.AsSpan(1).Trim(), out decimal d)) {
					return Tuple.Create(">", d);
				}
			} else if(c == '<' || c == '＜') {
				if(s.Length > 1 && (s[1] == '=' || s[1] == '＝')) {
					if(decimal.TryParse(s.AsSpan(2).Trim(), out decimal d)) {
						return Tuple.Create("<=", d);
					}
				} else if(s.Length > 1 && (s[1] == '>' || s[1] == '＞')) {
					if(decimal.TryParse(s.AsSpan(2).Trim(), out decimal d)) {
						return Tuple.Create("!=", d);
					}
				} else if(decimal.TryParse(s.AsSpan(1).Trim(), out decimal d)) {
					return Tuple.Create("<", d);
				}
			} else if(c == '=' || c == '＝') {
				var index = 1;
				if(s.Length > 1 && (s[1] == '=' || s[1] == '＝')) {
					index = 2;
					if(s.Length > 2 && (s[2] == '=' || s[2] == '＝')) {
						index = 3;
					}
				}
				if(decimal.TryParse(s.AsSpan(index).Trim(), out decimal d)) {
					return Tuple.Create("=", d);
				}
			} else if(c == '!' || c == '！') {
				var index = 1;
				if(s.Length > 1 && (s[1] == '=' || s[1] == '＝')) {
					index = 2;
					if(s.Length > 2 && (s[2] == '=' || s[2] == '＝')) {
						index = 3;
					}
				}
				if(decimal.TryParse(s.AsSpan(index).Trim(), out decimal d)) {
					return Tuple.Create("!=", d);
				}
			}
			return null;
		}

		public static int Compare(decimal t1, decimal t2)
		{
			var b = t1 - t2;
			//var b = Math.Round(t1 - t2, 12, MidpointRounding.AwayFromZero);
			if(b == 0) {
				return 0;
			} else if(b > 0) {
				return 1;
			}
			return -1;
		}

		public static bool TryParseBoolean(string TextValue, out bool boolValue)
		{
			if(TextValue.Equals("true", StringComparison.OrdinalIgnoreCase)) {
				boolValue = true;
				return true;
			}
			if(TextValue.Equals("false", StringComparison.OrdinalIgnoreCase)) {
				boolValue = false;
				return true;
			}
			if(TextValue.Equals("yes", StringComparison.OrdinalIgnoreCase)) {
				boolValue = true; 
				return true;
			}
			if(TextValue.Equals("no", StringComparison.OrdinalIgnoreCase)) {
				boolValue = false; 
				return true;
			}
			if(TextValue.Equals("1") || TextValue.Equals("是") || TextValue.Equals("有")) {
				boolValue = true;
				return true;
			}
			if(TextValue.Equals("0") || TextValue.Equals("否") || TextValue.Equals("不是") || TextValue.Equals("无") || TextValue.Equals("没有")) {
				boolValue = false;
				return true;
			}
			boolValue = false;
			return false;
		}
	}
}