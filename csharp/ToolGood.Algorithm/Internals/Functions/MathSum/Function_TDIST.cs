using System;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_TDIST : Function_3
    {
        public Function_TDIST(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "TDist", 1); if (args1.IsError) return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "TDist", 2); if (args2.IsError) return args2; }
            var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotNumber) { args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "TDist", 3); if (args3.IsError) return args3; }
            var x = args1.DoubleValue;
            var degreesFreedom = args2.IntValue;
            var tails = args3.IntValue;
            if (degreesFreedom <= 0.0m || tails < 1 || tails > 2) {
                return Operand.Error("Function '{0}' parameter is error!", "TDist");
            }
            return Operand.Create(ExcelFunctions.TDist((double)x, degreesFreedom, tails));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "TDist");
        }
    }

}
