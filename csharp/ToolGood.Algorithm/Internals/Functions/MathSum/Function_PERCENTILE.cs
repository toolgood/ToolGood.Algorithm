using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_PERCENTILE : Function_2
    {
		public Function_PERCENTILE(FunctionBase[] funcs) : base(funcs)
		{
		}

		public Function_PERCENTILE(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override string Name => "Percentile";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetArray_1(work, tempParameter);
            if (args1.IsError) { return args1; }

            var args2 = GetNumber_2(work, tempParameter);
            if (args2.IsError) { return args2; }

            var list = new List<double>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return ParameterError(1); }
            var k = args2.DoubleValue;
            return Operand.Create(ExcelFunctions.Percentile(list.Select(q => (double)q).ToArray(), (double)k));
        }

    }

}
