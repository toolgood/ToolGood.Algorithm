using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{

    internal class Function_QUARTILE : Function_2
    {
        public Function_QUARTILE(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            args1 = FunctionUtil.ConvertToArray(args1, "Quartile", 1);
            if (args1.IsError) { return args1; }

            var args2 = func2.Evaluate(work, tempParameter);
            args2 = FunctionUtil.ConvertToNumber(args2, "Quartile", 2);
            if (args2.IsError) { return args2; }

            var list = new List<double>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "Quartile", 1); }

            var quant = args2.IntValue;
            if (quant < 0 || quant > 4) {
                return Operand.Error("Function '{0}' parameter {1} is error!", "Quartile", 2);
            }
            return Operand.Create(ExcelFunctions.Quartile(list.Select(q => (double)q).ToArray(), quant));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Quartile");
        }
    }

}
