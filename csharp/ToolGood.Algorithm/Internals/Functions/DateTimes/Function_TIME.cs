using System;
using System.Text;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_TIME : Function_3
    {
        public Function_TIME(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override string Name => "Time";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToNumber(args1, "Time", 1);
			if (args1.IsError) { return args1; }

			var args2 = func2.Evaluate(work, tempParameter);
			args2 = FunctionUtil.ConvertToNumber(args2, "Time", 2);
			if (args2.IsError) { return args2; }

			MyDate d;
			if (func3 != null) {
				var args3 = func3.Evaluate(work, tempParameter);
				args3 = FunctionUtil.ConvertToNumber(args3, "Time", 3);
				if (args3.IsError) { return args3; }
				d = new MyDate(0, 0, 0, args1.IntValue, args2.IntValue, args3.IntValue);
			} else {
				d = new MyDate(0, 0, 0, args1.IntValue, args2.IntValue, 0);
			}
            return Operand.Create(d);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Time");
        }
    }

}
