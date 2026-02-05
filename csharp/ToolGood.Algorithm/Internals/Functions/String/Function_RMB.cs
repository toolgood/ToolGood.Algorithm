using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal class Function_RMB : Function_1
	{
		public Function_RMB(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "Rmb";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(work, tempParameter);
			if (args1.IsError) { return args1; }
			return Operand.Create(F_base_ToChineseRMB(args1.NumberValue));
		}

		private static readonly Regex Regex1 = new Regex(@"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", RegexOptions.Compiled);
		private static readonly Regex Regex2 = new Regex(".", RegexOptions.Compiled);

		private static string F_base_ToChineseRMB(decimal x)
		{
			string s = x.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A", CultureInfo.InvariantCulture);
			string d = Regex1.Replace(s, "${b}${z}");
			return Regex2.Replace(d, m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString());
		}

	}

}
