using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{

    internal sealed class Function_QUARTILE : Function_2
    {
		public Function_QUARTILE(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 2) {
				throw new ArgumentException($"Function '{Name}' requires exactly 2 parameters.");
			}
		}

        public override string Name => "Quartile";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetArray_1(engine, tempParameter);
            if (args1.IsErrorOrNone) { return args1; }

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone) { return args2; }

            var list = new List<decimal>();
            var o = FunctionUtil.FlattenToList(args1, list);
            if (o == false) { return ParameterError(1); }
            if (list.Count == 0) { return ParameterError(1); }

            var quant = args2.IntValue;
            if (quant < 0 || quant > 4) {
                return ParameterError(2);
            }
            return Operand.Create(ExcelFunctions.Quartile(list.ToArray(), quant));
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
			func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}

}
