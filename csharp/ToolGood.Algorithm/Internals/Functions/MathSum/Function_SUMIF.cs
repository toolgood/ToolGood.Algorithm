using System;
using System.Collections.Generic;
using System.Globalization;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_SUMIF : Function_3
    {
		public Function_SUMIF(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length < 2 || funcs.Length > 3) {
				throw new ArgumentException($"Function '{Name}' requires 2 to 3 parameters.");
			}
		}

        public override string Name => "SumIf";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetArray_1(engine, tempParameter); if (args1.IsErrorOrNone) { return args1; }
            var args2 = func2.Evaluate(engine, tempParameter); if (args2.IsErrorOrNone) { return args2; }

            var list = new List<decimal>();
            var o = FunctionUtil.FlattenToList(args1, list);
            if (o == false) { return ParameterError(1); }

            List<decimal> sumdbs;
            if (func3 != null) {
                var args3 = GetArray_3(engine, tempParameter); if (args3.IsErrorOrNone) { return args3; }
                sumdbs = new List<decimal>();
                var o2 = FunctionUtil.FlattenToList(args3, sumdbs);
                if (o2 == false) { return ParameterError(3); }
            } else {
                sumdbs = list;
            }

            decimal sum;
            if (args2.IsNumber) {
                sum = FunctionUtil.GetSumIf(list, args2.NumberValue, sumdbs);
            } else {
                var span = args2.TextValue.AsSpan().Trim();
                if (decimal.TryParse(span, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    sum = FunctionUtil.GetSumIf(list, d, sumdbs);
                } else {
                    var m2 = FunctionUtil.ParseSumIfMatch(args2.TextValue.Trim());
                    if (m2 != null) {
                        sum = FunctionUtil.GetSumIf(list, m2.Item1, m2.Item2, sumdbs);
                    } else {
                        return ParameterError(2);
                    }
                }
            }
            return Operand.Create(sum);
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
			func2.GetParameterTypes(noneEngine, result, OperandType.NONE);
			if(func3 != null) {
				func3.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
			}
		}
	}

}
