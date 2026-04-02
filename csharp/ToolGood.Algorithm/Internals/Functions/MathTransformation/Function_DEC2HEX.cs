using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathTransformation
{
    internal sealed class Function_DEC2HEX : Function_2
    {
		public Function_DEC2HEX(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length < 1 || funcs.Length > 2) {
				throw new ArgumentException($"Function '{Name}' requires 1 to 2 parameters.");
			}
		}

        public override string Name => "Dec2Hex";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if(args1.IsErrorOrNone) { return args1; }
            var num = Convert.ToString(args1.IntValue, 16).ToUpperInvariant();
            if(func2 != null) {
                var args2 = GetNumber_2(engine, tempParameter);
                if(args2.IsErrorOrNone) { return args2; }
                if(args2.IntValue < 0) {
                    return ParameterError(2);
                }
                if(num.Length <= args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return ParameterError(2);
            }
            return Operand.Create(num);
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
