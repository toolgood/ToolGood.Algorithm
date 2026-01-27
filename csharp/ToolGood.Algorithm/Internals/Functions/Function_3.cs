using System.Text;

namespace ToolGood.Algorithm.Internals.Functions
{
	internal abstract class Function_3 : FunctionBase
	{
		protected FunctionBase func1;
		protected FunctionBase func2;
		protected FunctionBase func3;

		protected Function_3(FunctionBase func1, FunctionBase func2, FunctionBase func3)
		{
			this.func1 = func1;
			this.func2 = func2;
			this.func3 = func3;
		}
		protected void AddFunction(StringBuilder stringBuilder, string functionName)
		{
			stringBuilder.Append(functionName);
			stringBuilder.Append('(');
			func1.ToString(stringBuilder, false);
			if(func2 != null) {
				stringBuilder.Append(", ");
				func2.ToString(stringBuilder, false);
				if(func3 != null) {
					stringBuilder.Append(", ");
					func3.ToString(stringBuilder, false);
				}
			}
			stringBuilder.Append(')');
		}
	}


}