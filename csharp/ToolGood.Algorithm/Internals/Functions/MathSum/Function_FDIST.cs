using System;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_FDIST : Function_3
    {
        public Function_FDIST(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "FDist", 1); if (args1.IsError) return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "FDist", 2); if (args2.IsError) return args2; }
            var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotNumber) { args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "FDist", 3); if (args3.IsError) return args3; }

            var x = args1.DoubleValue;
            var degreesFreedom = args2.IntValue;
            var degreesFreedom2 = args3.IntValue;
            if (degreesFreedom <= 0.0 || degreesFreedom2 <= 0.0) {
                return Operand.Error("Function '{0}' parameter is error!", "FDist");
            }
            return Operand.Create(ExcelFunctions.FDist((double)x, degreesFreedom, degreesFreedom2));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "FDist");
        }
    }

}
