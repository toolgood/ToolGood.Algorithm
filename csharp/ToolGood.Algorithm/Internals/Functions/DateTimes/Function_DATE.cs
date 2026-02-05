using System;
using System.Text;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_DATE : Function_N
    {
        public Function_DATE(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override string Name => "Date";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = funcs[0].Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToNumber(args1, "Date", 1);
			if (args1.IsError) { return args1; }

			var args2 = funcs[1].Evaluate(work, tempParameter);
			args2 = FunctionUtil.ConvertToNumber(args2, "Date", 2);
			if (args2.IsError) { return args2; }

			var args3 = funcs[2].Evaluate(work, tempParameter);
			args3 = FunctionUtil.ConvertToNumber(args3, "Date", 3);
			if (args3.IsError) { return args3; }

            MyDate d;
            if (funcs.Length == 3) {
                d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, 0, 0, 0);
            } else if (funcs.Length == 4) {
                var args4 = funcs[3].Evaluate(work, tempParameter);
                args4 = FunctionUtil.ConvertToNumber(args4, "Date", 4);
                if (args4.IsError) { return args4; }

                d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, args4.IntValue, 0, 0);
            } else if (funcs.Length == 5) {
                var args4 = funcs[3].Evaluate(work, tempParameter);
                args4 = FunctionUtil.ConvertToNumber(args4, "Date", 4);
                if (args4.IsError) { return args4; }

                var args5 = funcs[4].Evaluate(work, tempParameter);
                args5 = FunctionUtil.ConvertToNumber(args5, "Date", 5);
                if (args5.IsError) { return args5; }

                d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, args4.IntValue, args5.IntValue, 0);
            } else {
                var args4 = funcs[3].Evaluate(work, tempParameter);
                args4 = FunctionUtil.ConvertToNumber(args4, "Date", 4);
                if (args4.IsError) { return args4; }

                var args5 = funcs[4].Evaluate(work, tempParameter);
                args5 = FunctionUtil.ConvertToNumber(args5, "Date", 5);
                if (args5.IsError) { return args5; }

                var args6 = funcs[5].Evaluate(work, tempParameter);
                args6 = FunctionUtil.ConvertToNumber(args6, "Date", 6);
                if (args6.IsError) { return args6; }

                d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, args4.IntValue, args5.IntValue, args6.IntValue);
            }
            return Operand.Create(d);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Date");
        }
    }

}
