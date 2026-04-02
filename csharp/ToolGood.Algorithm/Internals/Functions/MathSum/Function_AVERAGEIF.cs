using System;
using System.Collections.Generic;
using System.Globalization;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_AVERAGEIF : Function_3
    {
		public Function_AVERAGEIF(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length < 2) {
				throw new ArgumentException($"Function '{Name}' requires at least 2 parameters.");
			}
		}

        public override string Name => "AverageIf";

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
            int count;
            if (args2.IsNumber) {
                count = FunctionUtil.GetCountIf(list, args2.NumberValue);
                sum = count * args2.NumberValue;
            } else {
                var span = args2.TextValue.AsSpan().Trim();
                if (decimal.TryParse(span, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    count = FunctionUtil.GetCountIf(list, d);
                    sum = FunctionUtil.GetSumIf(list, d, sumdbs);
                } else {
                    var m2 = FunctionUtil.ParseSumIfMatch(args2.TextValue.Trim());
                    if (m2 != null) {
                        count = FunctionUtil.GetCountIf(list, m2.Item1, m2.Item2);
                        sum = FunctionUtil.GetSumIf(list, m2.Item1, m2.Item2, sumdbs);
                    } else {
                        return ParameterError(2);
                    }
                }
            }
            if (count == 0) {
                return Div0Error();
            }
            return Operand.Create(sum / count);
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
