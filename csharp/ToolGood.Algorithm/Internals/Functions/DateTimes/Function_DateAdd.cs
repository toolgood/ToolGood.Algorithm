using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	/// <summary>
	/// 日期偏移类函数的公共实现。
	/// <para>
	/// ADDDAYS / ADDHOURS / ADDMINUTES / ADDMONTHS / ADDSECONDS / ADDYEARS / EDATE 七个类原本各自
	/// 复制了一份完全相同的求值逻辑（取日期、取整数偏移量、调用 AddXxx、捕获 ArgumentOutOfRangeException
	/// 与 OverflowException 转成参数错误），差异只有 Name 与具体调用的 AddXxx 方法。
	/// 这里收敛为一份，子类只声明名称与偏移运算。
	/// </para>
	/// <para>
	/// 子类类名保持不变，因此 Antlr 版与 Fast 版两张函数工厂表都不需要改动。
	/// </para>
	/// </summary>
	internal abstract class Function_DateAdd : Function_2
	{
		protected Function_DateAdd(FunctionBase[] funcs) : base(funcs)
		{
			if(funcs.Length != 2) {
				throw new ArgumentException($"Function '{Name}' requires exactly 2 parameters.");
			}
		}

		/// <summary>
		/// 对日期施加整数偏移量。越界由底层 AddXxx 抛出，由本类的求值方法统一转成参数错误。
		/// </summary>
		protected abstract MyDate Add(MyDate date, int offset);

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetDate_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }

			var args2 = GetNumber_2(engine, tempParameter);
			if(args2.IsErrorOrNone) { return args2; }

			try {
				return Operand.Create(Add(args1.DateValue, args2.IntValue));
			} catch(ArgumentOutOfRangeException) {
				// 偏移后超出 DateTime 可表示范围
				return ParameterError(2);
			} catch(OverflowException) {
				return ParameterError(2);
			}
		}

		public override OperandType GetResultType()
		{
			return OperandType.DATE;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.DATE);
			func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}
}
