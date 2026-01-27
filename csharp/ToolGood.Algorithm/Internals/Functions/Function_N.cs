using System.Text;

namespace ToolGood.Algorithm.Internals.Functions
{
	internal abstract class Function_N : FunctionBase
	{
		protected FunctionBase[] funcs;

		protected Function_N(FunctionBase[] funcs)
		{
			this.funcs = funcs;
		}

		protected void AddFunction(StringBuilder stringBuilder, string functionName)
		{
			stringBuilder.Append(functionName);
			stringBuilder.Append('(');
			for(int i = 0; i < funcs.Length; i++) {
				if(i > 0) {
					stringBuilder.Append(", ");
				}
				funcs[i].ToString(stringBuilder, false);
			}
			stringBuilder.Append(')');
		}
	}


}