using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions
{

    #region if iferror
    internal class Function_IF : Function_3
    {
        public Function_IF(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotBoolean) { args1 = args1.ToBoolean("Function '{0}' parameter {1} is error!", "If", 1); if (args1.IsError) { return args1; } }
            if (args1.BooleanValue) return func2.Evaluate(work, tempParameter);
            if (func3 == null) { return Operand.False; }
            return func3.Evaluate(work, tempParameter);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "If");
        }
    }

    internal class Function_IFERROR : Function_3
    {
        public Function_IFERROR(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            if (args1.IsError) { return func2.Evaluate(work, tempParameter); }
            return func3.Evaluate(work, tempParameter);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "IfError");
        }
    }

    #endregion

    #region isXXXX

    internal class Function_ISNUMBER : Function_1
    {
        public Function_ISNUMBER(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            if (args1.IsNumber) { return Operand.True; }
            return Operand.False;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "IsNumber");
        }
    }

    internal class Function_ISTEXT : Function_1
    {
        public Function_ISTEXT(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            if (args1.IsText) { return Operand.True; }
            return Operand.False;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "IsText");
        }
    }

    internal class Function_ISERROR : Function_2
    {
        public Function_ISERROR(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            if (func2 != null) {
                if (args1.IsError) { return func2.Evaluate(work, tempParameter); }
                return args1;
            }
            if (args1.IsError) { return Operand.True; }
            return Operand.False;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "IsError");
        }
    }

    internal class Function_ISNULL : Function_2
    {
        public Function_ISNULL(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            if (func2 != null) {
                if (args1.IsNull) { return func2.Evaluate(work, tempParameter); }
                if (args1.IsText && args1.TextValue == null) { return func2.Evaluate(work, tempParameter); }
                return args1;
            }
            if (args1.IsNull) { return Operand.True; }
            if (args1.IsText && args1.TextValue == null) { return Operand.True; }
            return Operand.False;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "IsNull");
        }
    }

    internal class Function_ISNULLORERROR : Function_2
    {
        public Function_ISNULLORERROR(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            if (func2 != null) {
                if (args1.IsNull || args1.IsError) { return func2.Evaluate(work, tempParameter); }
                if (args1.IsText && args1.TextValue == null) { return func2.Evaluate(work, tempParameter); }
                return args1;
            }
            if (args1.IsNull || args1.IsError) { return Operand.True; }
            if (args1.IsText && args1.TextValue == null) { return Operand.True; }
            return Operand.False;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "IsNullOrError");
        }
    }

    internal class Function_ISEVEN : Function_1
    {
        public Function_ISEVEN(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            if (args1.IsNumber) {
                if (args1.IntValue % 2 == 0) { return Operand.True; }
            }
            return Operand.False;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "IsEven");
        }
    }

    internal class Function_ISODD : Function_1
    {
        public Function_ISODD(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            if (args1.IsNumber) {
                if (args1.IntValue % 2 == 1) { return Operand.True; }
            }
            return Operand.False;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "IsOdd");
        }
    }

    internal class Function_ISLOGICAL : Function_1
    {
        public Function_ISLOGICAL(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            if (args1.IsBoolean) { return Operand.True; }
            return Operand.False;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "IsLogical");
        }
    }

    internal class Function_ISNONTEXT : Function_1
    {
        public Function_ISNONTEXT(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            if (args1.IsNotText) { return Operand.True; }
            return Operand.False;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "IsNontext");
        }
    }



    internal class Function_ISNULLOREMPTY : Function_1
    {
        public Function_ISNULLOREMPTY(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            if (args1.IsNull) { return Operand.True; }
            if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "IsNullOrEmpty", 1); if (args1.IsError) { return args1; } }
            return Operand.Create(string.IsNullOrEmpty(args1.TextValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "IsNullOrEmpty");
        }
    }

    internal class Function_ISNULLORWHITESPACE : Function_1
    {
        public Function_ISNULLORWHITESPACE(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            if (args1.IsNull) { return Operand.True; }
            if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "IsNullOrWhiteSpace", 1); if (args1.IsError) { return args1; } }
            return Operand.Create(string.IsNullOrWhiteSpace(args1.TextValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "IsNullOrWhiteSpace");
        }
    } 
    #endregion


    internal class Function_NOT : Function_1
    {
        public Function_NOT(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            if (args1.IsNotBoolean) { args1 = args1.ToBoolean("Function '{0}' parameter is error!", "Not"); if (args1.IsError) { return args1; } }
            return args1.BooleanValue ? Operand.False : Operand.True;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Not");
        }
    }
}
