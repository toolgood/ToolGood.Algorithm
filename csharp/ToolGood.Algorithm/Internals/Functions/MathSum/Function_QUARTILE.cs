using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{

    internal class Function_QUARTILE : Function_2
    {
		public Function_QUARTILE(FunctionBase[] funcs) : base(funcs)
		{
		}

		

        public override string Name => "Quartile";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetArray_1(work, tempParameter);
            if (args1.IsError) { return args1; }

            var args2 = GetNumber_2(work, tempParameter);
            if (args2.IsError) { return args2; }

            var list = new List<double>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return ParameterError(1); }

            var quant = args2.IntValue;
            if (quant < 0 || quant > 4) {
                return ParameterError(2);
            }
            return Operand.Create(ExcelFunctions.Quartile(list.Select(q => (double)q).ToArray(), quant));
        }

    }

}
