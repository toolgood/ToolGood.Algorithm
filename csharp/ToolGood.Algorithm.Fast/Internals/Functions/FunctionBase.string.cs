using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions
{
    #region string

    internal class Function_ASC : Function_1
    {
        public Function_ASC(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function ASC parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(F_base_ToDBC(args1.TextValue));
        }

        private static String F_base_ToDBC(String input)
        {
            StringBuilder sb = new StringBuilder(input);
            for (int i = 0; i < input.Length; i++) {
                var c = input[i];
                if (c == 12288) {
                    sb[i] = (char)32;
                    continue;
                } else if (c > 65280 && c < 65375) {
                    sb[i] = (char)(c - 65248);
                }
            }
            return sb.ToString();
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("ASC(");
            func1.ToString(stringBuilder, false);
            stringBuilder.Append(')');
        }
    }

    internal class Function_JIS : Function_1
    {
        public Function_JIS(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function JIS parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(F_base_ToSBC(args1.TextValue));
        }

        private static String F_base_ToSBC(String input)
        {
            StringBuilder sb = new StringBuilder(input);
            for (int i = 0; i < input.Length; i++) {
                var c = input[i];
                if (c == ' ') {
                    sb[i] = (char)12288;
                } else if (c < 127) {
                    sb[i] = (char)(c + 65248);
                }
            }
            return sb.ToString();
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("JIS(");
            func1.ToString(stringBuilder, false);
            stringBuilder.Append(')');
        }
    }

    internal class Function_CHAR : Function_1
    {
        public Function_CHAR(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function CHAR parameter is error!"); if (args1.IsError) { return args1; } }
            char c = (char)(int)args1.NumberValue;
            return Operand.Create(c.ToString());
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("CHAR(");
            func1.ToString(stringBuilder, false);
            stringBuilder.Append(')');
        }
    }

    internal class Function_CLEAN : Function_1
    {
        public Function_CLEAN(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function CLEAN parameter is error!"); if (args1.IsError) { return args1; } }
            var t = args1.TextValue;
            StringBuilder sb = new StringBuilder(t.Length);
            for (int i = 0; i < t.Length; i++) {
                var c = t[i];
                if (c != '\f' && c != '\n' && c != '\r' && c != '\t' && c != '\v') {
                    sb.Append(c);
                }
            }
            return Operand.Create(sb.ToString());
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("CLEAN(");
            func1.ToString(stringBuilder, false);
            stringBuilder.Append(')');
        }
    }

    internal class Function_CODE : Function_1
    {
        public Function_CODE(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function CODE parameter is error!"); if (args1.IsError) { return args1; } }
            if (string.IsNullOrEmpty(args1.TextValue)) {
                return Operand.Error("Function CODE parameter is error!");
            }
            char c = args1.TextValue[0];
            return Operand.Create((decimal)(int)c);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("CODE(");
            func1.ToString(stringBuilder, false);
            stringBuilder.Append(')');
        }
    }

    internal class Function_CONCATENATE : Function_N
    {
        public Function_CONCATENATE(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < funcs.Length; i++) {
                var a = funcs[i].Calculate(work); if (a.Type != OperandType.TEXT) { a = a.ToText($"Function CONCATENATE parameter {i + 1} is error!"); if (a.IsError) { return a; } }
                sb.Append(a.TextValue);
            }
            return Operand.Create(sb.ToString());
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("CONCATENATE(");
            for (int i = 0; i < funcs.Length; i++) {
                if (i > 0) {
                    stringBuilder.Append(", ");
                }
                funcs[i].ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_EXACT : Function_2
    {
        public Function_EXACT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function EXACT parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function EXACT parameter 2 is error!"); if (args2.IsError) { return args2; } }
            return Operand.Create(args1.TextValue == args2.TextValue);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("EXACT(");
            func1.ToString(stringBuilder, false);
            if (func2 != null) {
                stringBuilder.Append(", ");
                func2.ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_FIND : Function_3
    {
        public Function_FIND(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function FIND parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function FIND parameter 2 is error!"); if (args2.IsError) { return args2; } }
            if (func3 == null) {
                var p = args2.TextValue.AsSpan().IndexOf(args1.TextValue) + work.ExcelIndex;
                return Operand.Create(p);
            }
            var count = func3.Calculate(work).ToNumber("Function FIND parameter 3 is error!"); if (count.IsError) { return count; }
            var p2 = args2.TextValue.AsSpan(count.IntValue).IndexOf(args1.TextValue) + count.IntValue + work.ExcelIndex;
            return Operand.Create(p2);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("EXACT(");
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

    internal class Function_LEFT : Function_2
    {
        public Function_LEFT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function LEFT parameter 1 is error!"); if (args1.IsError) { return args1; } }
            if (func2 == null) {
                return Operand.Create(args1.TextValue[0].ToString());
            }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function LEFT parameter 2 is error!"); if (args2.IsError) { return args2; } }
            return Operand.Create(args1.TextValue.AsSpan(0, args2.IntValue).ToString());
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("LEFT(");
            func1.ToString(stringBuilder, false);
            if (func2 != null) {
                stringBuilder.Append(", ");
                func2.ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_LEN : Function_1
    {
        public Function_LEN(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function LEN parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create((decimal)args1.TextValue.Length);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("LEN(");
            func1.ToString(stringBuilder, false);
            stringBuilder.Append(')');
        }
    }

    internal class Function_LOWER : Function_1
    {
        public Function_LOWER(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function LOWER parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(args1.TextValue.ToLower());
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("LOWER(");
            func1.ToString(stringBuilder, false);
            stringBuilder.Append(')');
        }
    }

    internal class Function_MID : Function_3
    {
        public Function_MID(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function MID parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function MID parameter 2 is error!"); if (args2.IsError) { return args2; } }
            var args3 = func3.Calculate(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function MID parameter 3 is error!"); if (args3.IsError) { return args3; } }
            return Operand.Create(args1.TextValue.AsSpan(args2.IntValue - work.ExcelIndex, args3.IntValue).ToString());
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("MID(");
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

    internal class Function_PROPER : Function_1
    {
        public Function_PROPER(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function PROPER parameter is error!"); if (args1.IsError) { return args1; } }

            var text = args1.TextValue;
            StringBuilder sb = new StringBuilder(text);
            bool isFirst = true;
            for (int i = 0; i < text.Length; i++) {
                var t = text[i];
                if (t == ' ' || t == '\r' || t == '\n' || t == '\t' || t == '.') {
                    isFirst = true;
                } else if (isFirst) {
                    sb[i] = char.ToUpper(t);
                    isFirst = false;
                }
            }
            return Operand.Create(sb.ToString());
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("PROPER(");
            func1.ToString(stringBuilder, false);
            stringBuilder.Append(')');
        }
    }

    internal class Function_REPLACE : Function_4
    {
        public Function_REPLACE(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) : base(func1, func2, func3, func4)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function REPLACE parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var oldtext = args1.TextValue;
            if (func4 == null) {
                var args22 = func2.Calculate(work); if (args22.Type != OperandType.TEXT) { args22 = args22.ToText("Function REPLACE parameter 2 is error!"); if (args22.IsError) { return args22; } }
                var args32 = func3.Calculate(work); if (args32.Type != OperandType.TEXT) { args32 = args32.ToText("Function REPLACE parameter 3 is error!"); if (args32.IsError) { return args32; } }

                var old = args22.TextValue;
                var newstr = args32.TextValue;
                return Operand.Create(oldtext.Replace(old, newstr));
            }

            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function REPLACE parameter 2 is error!"); if (args2.IsError) { return args2; } }
            var args3 = func3.Calculate(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function REPLACE parameter 3 is error!"); if (args3.IsError) { return args3; } }
            var args4 = func4.Calculate(work); if (args4.Type != OperandType.TEXT) { args4 = args4.ToText("Function REPLACE parameter 4 is error!"); if (args4.IsError) { return args4; } }

            var start = args2.IntValue - work.ExcelIndex;
            var length = args3.IntValue;
            var newtext = args4.TextValue;

            StringBuilder sb = new StringBuilder(oldtext.Length + newtext.Length);
            for (int i = 0; i < oldtext.Length; i++) {
                if (i < start) {
                    sb.Append(oldtext[i]);
                } else if (i == start) {
                    sb.Append(newtext);
                } else if (i >= start + length) {
                    sb.Append(oldtext[i]);
                }
            }
            return Operand.Create(sb.ToString());
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
                    if (func4 != null) {
                        stringBuilder.Append(", ");
                        func4.ToString(stringBuilder, false);
                    }
                }
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_REPT : Function_2
    {
        public Function_REPT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function REPT parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function REPT parameter 2 is error!"); if (args2.IsError) { return args2; } }

            var newtext = args1.TextValue;
            var length = args2.IntValue;
            StringBuilder sb = new StringBuilder(newtext.Length * length);
            for (int i = 0; i < length; i++) {
                sb.Append(newtext);
            }
            return Operand.Create(sb.ToString());
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("REPT(");
            func1.ToString(stringBuilder, false);
            if (func2 != null) {
                stringBuilder.Append(", ");
                func2.ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_RIGHT : Function_2
    {
        public Function_RIGHT(FunctionBase func1) : base(func1, null)
        {
        }

        public Function_RIGHT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function RIGHT parameter 1 is error!"); if (args1.IsError) { return args1; } }

            if (func2 == null) {
                return Operand.Create(args1.TextValue[args1.TextValue.Length - 1].ToString());
            }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function RIGHT parameter 2 is error!"); if (args2.IsError) { return args2; } }
            return Operand.Create(args1.TextValue.AsSpan(args1.TextValue.Length - args2.IntValue, args2.IntValue).ToString());
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("RIGHT(");
            func1.ToString(stringBuilder, false);
            if (func2 != null) {
                stringBuilder.Append(", ");
                func2.ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_RMB : Function_1
    {
        public Function_RMB(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function RMB parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(F_base_ToChineseRMB(args1.NumberValue));
        }

        private static string F_base_ToChineseRMB(decimal x)
        {
            string s = x.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A", CultureInfo.InvariantCulture);
            string d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}", RegexOptions.Compiled);
            return Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString(), RegexOptions.Compiled);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("RMB(");
            func1.ToString(stringBuilder, false);
            stringBuilder.Append(')');
        }
    }

    internal class Function_SEARCH : Function_3
    {
        public Function_SEARCH(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function SEARCH parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function SEARCH parameter 2 is error!"); if (args2.IsError) { return args2; } }

            if (func3 == null) {
                var p = args2.TextValue.AsSpan().IndexOf(args1.TextValue, StringComparison.OrdinalIgnoreCase) + work.ExcelIndex;
                return Operand.Create(p);
            }
            var args3 = func3.Calculate(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function SEARCH parameter 3 is error!"); if (args3.IsError) { return args3; } }
            var p2 = args2.TextValue.AsSpan(args3.IntValue).IndexOf(args1.TextValue, StringComparison.OrdinalIgnoreCase) + args3.IntValue + work.ExcelIndex;
            return Operand.Create(p2);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("SEARCH(");
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

    internal class Function_SUBSTITUTE : Function_4
    {
        public Function_SUBSTITUTE(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) : base(func1, func2, func3, func4)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function SUBSTITUTE parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function SUBSTITUTE parameter 2 is error!"); if (args2.IsError) { return args2; } }
            var args3 = func3.Calculate(work); if (args3.Type != OperandType.TEXT) { args3 = args3.ToText("Function SUBSTITUTE parameter 3 is error!"); if (args3.IsError) { return args3; } }
            if (func4 == null) {
                return Operand.Create(args1.TextValue.Replace(args2.TextValue, args3.TextValue));
            }
            var args4 = func4.Calculate(work); if (args4.Type != OperandType.NUMBER) { args4 = args4.ToNumber("Function SUBSTITUTE parameter 4 is error!"); if (args4.IsError) { return args4; } }
            string text = args1.TextValue;
            string oldtext = args2.TextValue;
            string newtext = args3.TextValue;
            int index = args4.IntValue;

            int index2 = 0;
            StringBuilder sb = new StringBuilder(text.Length + newtext.Length);
            for (int i = 0; i < text.Length; i++) {
                bool b = true;
                for (int j = 0; j < oldtext.Length; j++) {
                    var t = text[i + j];
                    var t2 = oldtext[j];
                    if (t != t2) {
                        b = false;
                        break;
                    }
                }
                if (b) {
                    index2++;
                }
                if (b && index2 == index) {
                    sb.Append(newtext);
                    i += oldtext.Length - 1;
                } else {
                    sb.Append(text[i]);
                }
            }
            return Operand.Create(sb.ToString());
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("SUBSTITUTE(");
            func1.ToString(stringBuilder, false);
            if (func2 != null) {
                stringBuilder.Append(", ");
                func2.ToString(stringBuilder, false);
                if (func3 != null) {
                    stringBuilder.Append(", ");
                    func3.ToString(stringBuilder, false);
                    if (func4 != null) {
                        stringBuilder.Append(", ");
                        func4.ToString(stringBuilder, false);
                    }
                }
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_T : Function_1
    {
        public Function_T(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work);
            if (args1.Type == OperandType.TEXT) {
                return args1;
            }
            return Operand.Create("");
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("T(");
            func1.ToString(stringBuilder, false);
            stringBuilder.Append(')');
        }
    }

    internal class Function_TEXT : Function_2
    {
        public Function_TEXT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsError) { return args1; }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function TEXT parameter 2 is error!"); if (args2.IsError) { return args2; } }

            if (args1.Type == OperandType.TEXT) {
                return args1;
            } else if (args1.Type == OperandType.BOOLEAN) {
                return Operand.Create(args1.BooleanValue ? "TRUE" : "FALSE");
            } else if (args1.Type == OperandType.NUMBER) {
                return Operand.Create(args1.NumberValue.ToString(args2.TextValue, CultureInfo.InvariantCulture));
            } else if (args1.Type == OperandType.DATE) {
                return Operand.Create(args1.DateValue.ToString(args2.TextValue));
            }
            args1 = args1.ToText("Function TEXT parameter 1 is error!"); if (args1.IsError) { return args1; }
            return Operand.Create(args1.TextValue.ToString());
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("TEXT(");
            func1.ToString(stringBuilder, false);
            if (func2 != null) {
                stringBuilder.Append(", ");
                func2.ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }

    }

    internal class Function_TRIM : Function_1
    {
        public Function_TRIM(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function TRIM parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(args1.TextValue.AsSpan().Trim().ToString());
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("TRIM(");
            func1.ToString(stringBuilder, false);
            stringBuilder.Append(')');
        }
    }

    internal class Function_UPPER : Function_1
    {
        public Function_UPPER(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function UPPER parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(args1.TextValue.ToUpper());
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("UPPER(");
            func1.ToString(stringBuilder, false);
            stringBuilder.Append(')');
        }
    }

    internal class Function_VALUE : Function_1
    {
        public Function_VALUE(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work);
            if (args1.Type == OperandType.NUMBER) { return args1; }
            if (args1.Type == OperandType.BOOLEAN) { return args1.BooleanValue ? Operand.One : Operand.Zero; }
            if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function VALUE parameter is error!"); if (args1.IsError) { return args1; } }

            if (decimal.TryParse(args1.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                return Operand.Create(d);
            }
            return Operand.Error("Function VALUE parameter is error!");
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("VALUE(");
            func1.ToString(stringBuilder, false);
            stringBuilder.Append(')');
        }
    }

    #endregion string
}
