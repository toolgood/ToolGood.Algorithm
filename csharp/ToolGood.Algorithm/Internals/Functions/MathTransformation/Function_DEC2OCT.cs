using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathTransformation
{
	internal sealed class Function_DEC2OCT : Function_2
    {
		public Function_DEC2OCT(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length < 1 || funcs.Length > 2) {
				throw new ArgumentException($"Function '{Name}' requires 1 to 2 parameters.");
			}
		}

        public override string Name => "Dec2Oct";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetNumber_1(engine, tempParameter);
            if (args1.IsErrorOrNone) { return args1; }
            // Excel 范围:-536870912 ~ 536870911
            var numValue = args1.NumberValue;
            if (numValue < -536870912m || numValue > 536870911m) {
                return ParameterError(1);
            }
            var num = (int)numValue;
            if (num < 0) {
                // 负数:返回 10 位八进制补码,按 Excel 语义忽略 places
                return Operand.Create(Convert.ToString(num & 0x3FFFFFFF, 8).PadLeft(10, '0'));
            }
            var oct = Convert.ToString(num, 8);
            if (func2 != null) {
                var args2 = GetNumber_2(engine, tempParameter);
                if (args2.IsErrorOrNone) { return args2; }
                if (args2.IntValue < 0) {
                    return ParameterError(2);
                }
                if (oct.Length > args2.IntValue) {
                    return ParameterError(2);
                }
                return Operand.Create(oct.PadLeft(args2.IntValue, '0'));
            }
            return Operand.Create(oct);
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
