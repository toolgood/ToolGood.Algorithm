using System;
using System.Globalization;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal sealed class Function_FIXED : Function_3
    {
		public Function_FIXED(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "Fixed";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var num = 2;
			if (func2 != null) {
				var args2 = GetNumber_2(engine, tempParameter);
				if (args2.IsError) { return args2; }
				num = args2.IntValue;
			}
			var args1 = GetNumber_1(engine, tempParameter);
			if (args1.IsError) { return args1; }

			var s = Math.Round(args1.NumberValue, num, MidpointRounding.AwayFromZero);
			var no = false;
			if (func3 != null) {
				var args3 = GetBoolean_3(engine, tempParameter);
				if (args3.IsError) { return args3; }
				no = args3.BooleanValue;
			}
            if (no == false) {
                return Operand.Create(s.ToString('N' + num.ToString(), CultureInfo.InvariantCulture));
            }
            return Operand.Create(s.ToString(CultureInfo.InvariantCulture));
        }

    }

    

}
