using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_ADDSECONDS : Function_2
    {
		public Function_ADDSECONDS(FunctionBase[] funcs) : base(funcs)
		{
		}

        public override string Name => "AddSeconds";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetDate_1(engine, tempParameter);
            if (args1.IsErrorOrNone) { return args1; }

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone) { return args2; }
            var date = args1.DateValue.AddSeconds(args2.IntValue);
            return Operand.Create(date);
        }
		public override OperandType GetResultType()
		{
			return OperandType.DATE;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.DATE);
			func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}

}
