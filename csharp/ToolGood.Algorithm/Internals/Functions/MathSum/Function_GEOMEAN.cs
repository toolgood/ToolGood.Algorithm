using System;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_GEOMEAN : Function_N
    {
        public Function_GEOMEAN(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args = new List<Operand>(); foreach (var item in funcs) { var aa = item.Evaluate(work, tempParameter); if (aa.IsError) { return aa; } args.Add(aa); }

            var list = new List<double>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter is error!", "GeoMean"); }
            if (list.Count == 0) { return Operand.Error("Function '{0}' parameter is error!", "GeoMean"); }
            double product = 1.0;
            foreach (var num in list) {
                if (num <= 0) {
                    return Operand.Error("Function '{0}' parameter is error!", "GeoMean");
                }
                product *= (double)num;
            }
            double geoMean = Math.Pow(product, 1.0 / list.Count);
            return Operand.Create(geoMean);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "GeoMean");
        }
    }

}
