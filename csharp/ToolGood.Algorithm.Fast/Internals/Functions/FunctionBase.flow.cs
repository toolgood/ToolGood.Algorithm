using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions
{
    #region flow

    internal class Function_IF : Function_3
    {
        public Function_IF(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.BOOLEAN) { args1 = args1.ToBoolean("Function IF first parameter is error!"); if (args1.IsError) { return args1; } }
            if (args1.BooleanValue) return func2.Calculate(work);
            if (func3 == null) { return Operand.False; }
            return func3.Calculate(work);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("IF(");
            func1.ToString(stringBuilder, false);
            if (func2 != null) {
                stringBuilder.Append(", ");
                func2.ToString(stringBuilder, false);
                if (func3 != null) {
                    stringBuilder.Append(", ");
                    func3.ToString(stringBuilder, false);
                }
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_IFERROR : Function_3
    {
        public Function_IFERROR(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work);
            if (args1.IsError) { return func2.Calculate(work); }
            return func3.Calculate(work);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("IFERROR(");
            func1.ToString(stringBuilder, false);
            if (func2 != null) {
                stringBuilder.Append(", ");
                func2.ToString(stringBuilder, false);
                if (func3 != null) {
                    stringBuilder.Append(", ");
                    func3.ToString(stringBuilder, false);
                }
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_ISNUMBER : Function_1
    {
        public Function_ISNUMBER(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work);
            if (args1.Type == OperandType.NUMBER) { return Operand.True; }
            return Operand.False;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("ISNUMBER(");
            func1.ToString(stringBuilder, false);
            stringBuilder.Append(')');
        }
    }

    internal class Function_ISTEXT : Function_1
    {
        public Function_ISTEXT(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work);
            if (args1.Type == OperandType.TEXT) { return Operand.True; }
            return Operand.False;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("ISTEXT(");
            func1.ToString(stringBuilder, false);
            stringBuilder.Append(')');
        }
    }

    internal class Function_ISERROR : Function_2
    {
        public Function_ISERROR(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work);
            if (func2 != null) {
                if (args1.IsError) { return func2.Calculate(work); }
                return args1;
            }
            if (args1.IsError) { return Operand.True; }
            return Operand.False;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("ISERROR(");
            func1.ToString(stringBuilder, false);
            if (func2 != null) {
                stringBuilder.Append(", ");
                func2.ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_ISNULL : Function_2
    {
        public Function_ISNULL(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work);
            if (func2 != null) {
                if (args1.IsNull) { return func2.Calculate(work); }
                return args1;
            }
            if (args1.IsNull) { return Operand.True; }
            return Operand.False;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("ISNULL(");
            func1.ToString(stringBuilder, false);
            if (func2 != null) {
                stringBuilder.Append(", ");
                func2.ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_ISNULLORERROR : Function_2
    {
        public Function_ISNULLORERROR(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work);
            if (func2 != null) {
                if (args1.IsNull || args1.IsError) { return func2.Calculate(work); }
                return args1;
            }
            if (args1.IsNull || args1.IsError) { return Operand.True; }
            return Operand.False;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("ISNULLORERROR(");
            func1.ToString(stringBuilder, false);
            if (func2 != null) {
                stringBuilder.Append(", ");
                func2.ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_ISEVEN : Function_1
    {
        public Function_ISEVEN(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work);
            if (args1.Type == OperandType.NUMBER) {
                if (args1.IntValue % 2 == 0) { return Operand.True; }
            }
            return Operand.False;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("ISEVEN(");
            func1.ToString(stringBuilder, false);
            stringBuilder.Append(')');
        }
    }

    internal class Function_ISODD : Function_1
    {
        public Function_ISODD(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work);
            if (args1.Type == OperandType.NUMBER) {
                if (args1.IntValue % 2 == 1) { return Operand.True; }
            }
            return Operand.False;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("ISODD(");
            func1.ToString(stringBuilder, false);
            stringBuilder.Append(')');
        }
    }

    internal class Function_ISLOGICAL : Function_1
    {
        public Function_ISLOGICAL(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work);
            if (args1.Type == OperandType.BOOLEAN) { return Operand.True; }
            return Operand.False;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("ISLOGICAL(");
            func1.ToString(stringBuilder, false);
            stringBuilder.Append(')');
        }
    }

    internal class Function_ISNONTEXT : Function_1
    {
        public Function_ISNONTEXT(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work);
            if (args1.Type != OperandType.TEXT) { return Operand.True; }
            return Operand.False;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("ISNONTEXT(");
            func1.ToString(stringBuilder, false);
            stringBuilder.Append(')');
        }
    }

    internal class Function_NOT : Function_1
    {
        public Function_NOT(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work);
            if (args1.IsError) { return args1; }
            return args1.BooleanValue ? Operand.False : Operand.True;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("NOT(");
            func1.ToString(stringBuilder, false);
            stringBuilder.Append(')');
        }
    }

    #endregion flow
}
