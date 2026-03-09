using System;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Operator
{

	internal sealed class Function_AND : Function_2
	{
		public Function_AND(FunctionBase[] funcs) : base(funcs)
		{
		}

		public Function_AND(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override string Name => "And";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			// ���� && and || or �� excel��  AND(x,y) OR(x,y) ������
			// ��excel�� AND(x,y) OR(x,y) �ȱ�����
			// �ڳ����У�&& and  ��true ֱ�ӷ���true �Ͳ�������һ���᲻�ᱨ��
			// �ڳ����У�|| or  ��false ֱ�ӷ���false �Ͳ�������һ���᲻�ᱨ��
			var args1 = GetBoolean_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }
			if(args1.BooleanValue == false) {
				var args2 = GetBoolean_2(engine, tempParameter);
				if(args2.IsErrorOrNone) { return args2; }
				return Operand.False;
			}
			return GetBoolean_2(engine, tempParameter);
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			if(addBrackets) stringBuilder.Append('(');
			func1.ToString(stringBuilder, false);
			stringBuilder.Append(" && ");
			func2.ToString(stringBuilder, false);
			if(addBrackets) stringBuilder.Append(')');
		}
		public override OperandType GetResultType()
		{
			return OperandType.BOOLEAN;
		}
	}


}