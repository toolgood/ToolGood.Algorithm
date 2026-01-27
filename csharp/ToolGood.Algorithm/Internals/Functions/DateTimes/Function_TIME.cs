using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_TIME : Function_3
    {
        public Function_TIME(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Time", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Time", 2); if (args2.IsError) { return args2; } }

            MyDate d;
            if (func3 != null) {
                var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotNumber) { args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "Time", 3); if (args3.IsError) { return args3; } }
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
