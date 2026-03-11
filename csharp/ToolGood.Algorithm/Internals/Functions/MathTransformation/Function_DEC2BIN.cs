using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.MathTransformation
{
	internal sealed class Function_DEC2BIN : Function_2
    {
		public Function_DEC2BIN(FunctionBase[] funcs) : base(funcs)
		{
		}

        public override string Name => "Dec2Bin";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsErrorOrNone) { return args1; }
            var num = args1.IntValue;
            if (num < -512 || num > 511) {
                return ParameterError(1);
            }
            var binaryStr = Convert.ToString(num, 2);
            if (func2 != null) {
                var args2 = GetNumber_2(engine, tempParameter);
                if (args2.IsErrorOrNone) { return args2; }
                if (binaryStr.Length > args2.IntValue) {
                    return ParameterError(2);
                }
                if (binaryStr.Length <= args2.IntValue) {
                    return Operand.Create(binaryStr.PadLeft(args2.IntValue, '0'));
                }
                return ParameterError(2);
            }
            return Operand.Create(binaryStr);
        }
		public override OperandType GetResultType()
		{
			return OperandType.TEXT;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func2 != null) {
				func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			}
		}
	}

}
