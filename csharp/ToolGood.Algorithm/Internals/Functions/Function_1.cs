using System.Text;

namespace ToolGood.Algorithm.Internals.Functions
{
	internal abstract class Function_1 : FunctionBase
	{
		protected FunctionBase func1;

		protected Function_1(FunctionBase func1)
		{
			this.func1 = func1;
		}
		protected void AddFunction(StringBuilder stringBuilder, string functionName)
		{
			stringBuilder.Append(functionName);
			stringBuilder.Append('(');
			func1.ToString(stringBuilder, false);
			stringBuilder.Append(')');
		}
	}


}