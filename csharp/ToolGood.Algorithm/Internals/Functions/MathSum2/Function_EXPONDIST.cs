using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum2
{
	internal sealed class Function_EXPONDIST : Function_3
	{
		public Function_EXPONDIST(FunctionBase[] funcs) : base(funcs)
		{
		}



		public override string Name => "ExpOnDist";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(engine, tempParameter);
			if(args1.IsErrorOrNone) return args1;

			var args2 = GetNumber_2(engine, tempParameter);
			if(args2.IsErrorOrNone) return args2;

			var args3 = GetBoolean_3(engine, tempParameter);
			if(args3.IsErrorOrNone) return args3;

			var n1 = args1.NumberValue;
			var rate = args2.NumberValue;
			if(n1 < 0.0m) {
				return ParameterError(1);
			}
			if(rate <= 0.0m) {
				return ParameterError(2);
			}
			return Operand.Create(ExponDist(n1, rate, args3.BooleanValue));
		}

		public decimal ExponDist(decimal x, decimal rate, bool state)
		{
			if(state) {
				return CDF(rate, x);
			}
			return PDF(rate, x);
		}
		public decimal PDF(decimal rate, decimal x)
		{
			return x < 0.0m ? 0.0m : rate * MathEx.Exp(-rate * x);
		}

		public decimal CDF(decimal rate, decimal x)
		{
			return x < 0.0m ? 0.0m : 1.0m - MathEx.Exp(-rate * x);
		}

		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			func3.GetParameterTypes(noneEngine, result, OperandType.BOOLEAN);
		}
	}

}
