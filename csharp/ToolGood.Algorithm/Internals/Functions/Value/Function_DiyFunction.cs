using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal sealed class Function_DiyFunction : Function_N
	{
		private readonly string funName;

		public Function_DiyFunction(string name, FunctionBase[] funcs) : base(funcs)
		{
			this.funName = name;
		}

		public override string Name => "DiyFunction";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args = new List<Operand>(funcs.Length);
			// 必须先拦截参数错误: 直接把错误操作数交给 ExecuteDiyFunction,
			// 自定义函数一旦访问 NumberValue/TextValue 等属性就会抛 NotImplementedException 逃逸出引擎
			var error = TryEvaluateAll(engine, tempParameter, args);
			if(error != null) { return error; }
			return engine.ExecuteDiyFunction(funName, args);
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, funName);
		}
		public override OperandType GetResultType()
		{
			return OperandType.NONE;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			for(int i = 0; i < funcs.Length; i++) {
				funcs[i].GetParameterTypes(noneEngine, result, OperandType.NONE);
			}
		}
	}

}
