using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathBase
{
	internal class Function_FLOOR : Function_2
    {
        public Function_FLOOR(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToNumber(args1, "Floor", 1);
			if (args1.IsError) { return args1; }

			if (func2 == null)
				return Operand.Create(Math.Floor(args1.NumberValue));

			var args2 = func2.Evaluate(work, tempParameter);
			args2 = FunctionUtil.ConvertToNumber(args2, "Floor", 2);
			if (args2.IsError) { return args2; }
            var b = args2.NumberValue;
            if (b >= 1) { return Operand.Create(args1.IntValue); }
            if (b <= 0) { return Operand.Error("Function '{0}' parameter {1} is error!", "Floor", 2); }

            var a = args1.NumberValue;
            var d = Math.Floor(a / b) * b;
            return Operand.Create(d);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Floor");
        }
    }

    

}
