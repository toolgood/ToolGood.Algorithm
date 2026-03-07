using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathTransformation
{
	internal sealed class Function_OCT2DEC : Function_2
    {
		public Function_OCT2DEC(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "Oct2Dec";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetText_1(engine, tempParameter);
            if (args1.IsError) { return args1; }

            if (RegexHelper.OctRegex.IsMatch(args1.TextValue) == false) { return ParameterError(1); }
            var num = Convert.ToInt32(args1.TextValue, 8);
			if(func2 != null) {
				var args2 = GetNumber_2(engine, tempParameter);
				if(args2.IsError) { return args2; }
				var n = num.ToString();
				if(n.Length <= args2.IntValue) {
					return Operand.Create(n.ToString().PadLeft(args2.IntValue, '0'));
				}
				return ParameterError(2);
			}
			return Operand.Create(num);
        }
    }

    

}
