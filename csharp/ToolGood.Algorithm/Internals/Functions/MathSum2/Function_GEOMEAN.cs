using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathSum2
{
	internal sealed class Function_GEOMEAN : Function_N
    {
        public Function_GEOMEAN(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override string Name => "GeoMean";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
			var args = new List<Operand>(funcs.Length); foreach(var item in funcs) { var aa = item.Evaluate(engine, tempParameter); if(aa.IsErrorOrNone) { return aa; } args.Add(aa); }


			var list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return ParameterError(1); }
            if (list.Count == 0) { return ParameterError(1); }
            decimal product = 1.0m;
            foreach (var num in list) {
                if (num <= 0) {
                    return ParameterError(1);
                }
                product *= num;
            }
			decimal geoMean = MathEx.Pow(product, 1.0m / list.Count);
            return Operand.Create(geoMean);
        }
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}
	}

}