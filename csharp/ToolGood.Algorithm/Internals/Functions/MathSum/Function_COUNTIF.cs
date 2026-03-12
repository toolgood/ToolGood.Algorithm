using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_COUNTIF : Function_2
    {
		public Function_COUNTIF(FunctionBase[] funcs) : base(funcs)
		{
		}

        public override string Name => "CountIf";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetArray_1(engine, tempParameter);
            if (args1.IsErrorOrNone) { return args1; }

            var args2 = func2.Evaluate(engine, tempParameter);
            if (args2.IsErrorOrNone) { return args2; }

            var list = new List<decimal>();
            var o = FunctionUtil.FlattenToList(args1, list);
            if (o == false) { return ParameterError(1); }
            int count;
            if (args2.IsNumber) {
                count = FunctionUtil.GetCountIf(list, args2.NumberValue);
            } else {
                var span = args2.TextValue.AsSpan().Trim();
                if (decimal.TryParse(span, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    count = FunctionUtil.GetCountIf(list, d);
                } else {
                    var m2 = FunctionUtil.ParseSumIfMatch(args2.TextValue.Trim());
                    if (m2 != null) {
                        count = FunctionUtil.GetCountIf(list, m2.Item1, m2.Item2);
                    } else {
                        return ParameterError(2);
                    }
                }
            }
            return Operand.Create(count);
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
			func2.GetParameterTypes(noneEngine, result, OperandType.NONE);
		}
	}

}
