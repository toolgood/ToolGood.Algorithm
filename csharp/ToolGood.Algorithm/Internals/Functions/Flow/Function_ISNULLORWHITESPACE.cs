using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Flow
{
    internal sealed class Function_ISNULLORWHITESPACE : Function_1
    {
        public Function_ISNULLORWHITESPACE(FunctionBase func1) : base(func1)
        {
        }

        public override string Name => "IsNullOrWhitespace";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(engine, tempParameter);
            if (args1.IsNull) { return Operand.True; }
            var textArg = GetText_1(engine, tempParameter);
			if (textArg.IsError) { return textArg; }
            return Operand.Create(string.IsNullOrWhiteSpace(textArg.TextValue));
        }

    } 
}
