using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_PERCENTRANK : Function_3
    {
		public Function_PERCENTRANK(FunctionBase[] funcs) : base(funcs)
		{
		}

        public override string Name => "PercentRank";

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

            var k = args2.NumberValue;
            var v = ExcelFunctions.PercentRank(list.ToArray(), k);
            var d = 3;
            if (func3 != null) {
                var args3 = GetNumber_3(engine, tempParameter);
                if (args3.IsErrorOrNone) { return args3; }
                d = args3.IntValue;
                if (d < 0) {
                    return ParameterError(3);
                }
            }
            return Operand.Create(Math.Round(v, d, MidpointRounding.AwayFromZero));
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
			func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func3 != null) {
				func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			}
		}
	}

}
