using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathTransformation
{
	internal sealed class Function_BIN2OCT : Function_2
    {
		public Function_BIN2OCT(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length < 1 || funcs.Length > 2) {
				throw new ArgumentException($"Function '{Name}' requires 1 to 2 parameters.");
			}
		}

        public override string Name => "Bin2Oct";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetText_1(engine, tempParameter);
            if (args1.IsErrorOrNone) { return args1; }

            if(RegexHelper.IsBin(args1.TextValue) == false) { return ParameterError(1); }
            var text = args1.TextValue;
            if (text.Length > 10) { return ParameterError(1); }
            // 10 位二进制补码解析
            var bin = Convert.ToInt32(text, 2);
            if (bin >= 512) { bin -= 1024; }
            string oct;
            if (bin < 0) {
                // 负数:10 位八进制补码
                oct = Convert.ToString(bin & 0x3FFFFFFF, 8).PadLeft(10, '0');
            } else {
                oct = Convert.ToString(bin, 8);
            }
            if (func2 != null) {
                var args2 = GetNumber_2(engine, tempParameter);
                if (args2.IsErrorOrNone) { return args2; }
                if (args2.IntValue < 0 || args2.IntValue > 10) {
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
			func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
			if(func2 != null) {
				func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			}
		}
	}

}
