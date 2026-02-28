using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal sealed class Function_PERCENTILE : Function_2
    {
		public Function_PERCENTILE(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "Percentile";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetArray_1(engine, tempParameter);
            if (args1.IsError) { return args1; }

            var args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsError) { return args2; }

            var list = new List<double>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return ParameterError(1); }
            var k = args2.DoubleValue;
            return Operand.Create(ExcelFunctions.Percentile(list.ToArray(), k));
        }

    }

}
