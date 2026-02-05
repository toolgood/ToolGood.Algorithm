using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.MathSum
{
	internal class Function_COVARIANCES : Function_2
    {
        public Function_COVARIANCES(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override string Name => "Covariances";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsError) { return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsError) { return args2; }

            var list1 = new List<decimal>();
            var list2 = new List<decimal>();

            var o1 = FunctionUtil.F_base_GetList(args1, list1);
            var o2 = FunctionUtil.F_base_GetList(args2, list2);
            if (o1 == false) { return ParameterError(1); }
            if (o2 == false) { return ParameterError(2); }
            if (list1.Count != list2.Count) { return Operand.Error("Function '{0}' parameter's count error!", "CovarIanceS"); }
            if (list1.Count == 1) { return Operand.Error("Function '{0}' parameter's count error!", "CovarIanceS"); }

            var avg1 = list1.Average();
            var avg2 = list2.Average();
            decimal sum = 0;
            for (int i = 0; i < list1.Count; i++) {
                sum += (list1[i] - avg1) * (list2[i] - avg2);
            }
            var val = sum / (list1.Count - 1);
            return Operand.Create(val);
        }

    }

    

}
