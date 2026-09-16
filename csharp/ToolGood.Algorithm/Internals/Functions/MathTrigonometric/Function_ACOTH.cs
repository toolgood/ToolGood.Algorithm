using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathTrigonometric
{
	internal sealed class Function_ACOTH : Function_1
	{
		public Function_ACOTH(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 1) {
				throw new ArgumentException($"Function '{Name}' requires exactly 1 parameter.");
			}
		}

		public override string Name => "Acoth";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }
			var d = args1.NumberValue;
			// 不能写成 Math.Abs(d) <= 1:decimal.MinValue 取绝对值会溢出抛异常
			if(d >= -1 && d <= 1) {
				return NumError();
			}
			// acoth(d)=atanh(1/d),用倒数形式可避免大 d 时 (d+1)/(d-1) 饱和为 1 导致结果恒为 0
			return Operand.Create(MathEx.Atanh(1m / d));
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}
}
