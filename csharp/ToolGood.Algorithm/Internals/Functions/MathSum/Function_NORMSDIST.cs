using System;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_NORMSDIST : Function_1
    {
        public Function_NORMSDIST(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "NormSDist"); if (args1.IsError) return args1; }
            var num = args1.DoubleValue;
            return Operand.Create(ExcelFunctions.NormSDist((double)num));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "NormSDist");
        }
    }

}
