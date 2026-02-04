using System;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_HYPGEOMDIST : Function_4
    {
        public Function_HYPGEOMDIST(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) : base(func1, func2, func3, func4)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            args1 = FunctionUtil.ConvertToNumber(args1, "HypgeomDist", 1);
            if (args1.IsError) return args1;

            var args2 = func2.Evaluate(work, tempParameter);
            args2 = FunctionUtil.ConvertToNumber(args2, "HypgeomDist", 2);
            if (args2.IsError) return args2;

            var args3 = func3.Evaluate(work, tempParameter);
            args3 = FunctionUtil.ConvertToNumber(args3, "HypgeomDist", 3);
            if (args3.IsError) return args3;

            var args4 = func4.Evaluate(work, tempParameter);
            args4 = FunctionUtil.ConvertToNumber(args4, "HypgeomDist", 4);
            if (args4.IsError) return args4;

            int k = args1.IntValue;
            int draws = args2.IntValue;
            int success = args3.IntValue;
            int population = args4.IntValue;
            if (!(population >= 0 && success >= 0 && draws >= 0 && success <= population && draws <= population)) {
                return Operand.Error("Function '{0}' parameter is error!", "HypgeomDist");
            }
            return Operand.Create(ExcelFunctions.HypgeomDist(k, draws, success, population));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "HypgeomDist");
        }
    }

}
