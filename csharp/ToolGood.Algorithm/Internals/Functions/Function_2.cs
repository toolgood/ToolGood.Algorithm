using System.Text;

namespace ToolGood.Algorithm.Internals.Functions
{
	internal abstract class Function_2 : FunctionBase
	{
		protected FunctionBase func1;
		protected FunctionBase func2;

		public Function_2(FunctionBase func1, FunctionBase func2)
		{
			this.func1 = func1;
			this.func2 = func2;
		}

		protected void AddFunction(StringBuilder stringBuilder, string functionName)
		{
			stringBuilder.Append(functionName);
			stringBuilder.Append('(');
			func1.ToString(stringBuilder, false);
			if(func2 != null) {
				stringBuilder.Append(", ");
				func2.ToString(stringBuilder, false);
			}
			stringBuilder.Append(')');
		}
	}


}