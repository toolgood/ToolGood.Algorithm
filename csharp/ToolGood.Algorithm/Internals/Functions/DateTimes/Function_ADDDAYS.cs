using System;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_ADDDAYS : Function_2
    {
		public Function_ADDDAYS(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "AddDays";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetDate_1(engine, tempParameter);
            if (args1.IsErrorOrNone) { return args1; }

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone) { return args2; }
            return Operand.Create(args1.DateValue.AddDays(args2.IntValue));
        }
		public override OperandType GetResultType()
		{
			return OperandType.DATE;
		}
	} 

}
