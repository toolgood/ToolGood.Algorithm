using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.LitJson;

namespace ToolGood.Algorithm.Internals.Functions
{
    #region csharp

    internal class Function_URLENCODE : Function_1
    {
        public Function_URLENCODE(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function URLENCODE parameter error!"); if (args1.IsError) { return args1; } }
            var s = args1.TextValue;
            var r = System.Web.HttpUtility.UrlEncode(s);
            return Operand.Create(r);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("URLENCODE(");
            func1.ToString(stringBuilder, false);
            stringBuilder.Append(')');
        }
    }

    internal class Function_URLDECODE : Function_1
    {
        public Function_URLDECODE(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function URLDECODE parameter error!"); if (args1.IsError) { return args1; } }
            var s = args1.TextValue;
            var r = System.Web.HttpUtility.UrlDecode(s);
            return Operand.Create(r);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("URLDECODE(");
            func1.ToString(stringBuilder, false);
            stringBuilder.Append(')');
        }
    }

    internal class Function_HTMLENCODE : Function_1
    {
        public Function_HTMLENCODE(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function HTMLENCODE parameter error!"); if (args1.IsError) { return args1; } }
            var s = args1.TextValue;
            var r = System.Web.HttpUtility.HtmlEncode(s);
            return Operand.Create(r);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("HTMLENCODE(");
            func1.ToString(stringBuilder, false);
            stringBuilder.Append(')');
        }
    }

    internal class Function_HTMLDECODE : Function_1
    {
        public Function_HTMLDECODE(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function HTMLDECODE parameter error!"); if (args1.IsError) { return args1; } }
            var s = args1.TextValue;
            var r = System.Web.HttpUtility.HtmlDecode(s);
            return Operand.Create(r);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("HTMLDECODE(");
            func1.ToString(stringBuilder, false);
            stringBuilder.Append(')');
        }
    }

    internal class Function_BASE64TOTEXT : Function_N
    {
        public Function_BASE64TOTEXT(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var a = item.Calculate(work).ToText($"Function BASE64TOTEXT parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }
            try {
                Encoding encoding;
                if (args.Count == 1) {
                    encoding = Encoding.UTF8;
                } else {
                    encoding = Encoding.GetEncoding(args[1].TextValue);
                }
                var t = encoding.GetString(Base64.FromBase64String(args[0].TextValue));
                return Operand.Create(t);
            } catch (Exception) { }
            return Operand.Error("Function BASE64TOTEXT is error!");
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("BASE64TOTEXT(");
            for (int i = 0; i < funcs.Length; i++) {
                if (i > 0) {
                    stringBuilder.Append(", ");
                }
                funcs[i].ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_BASE64URLTOTEXT : Function_N
    {
        public Function_BASE64URLTOTEXT(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var a = item.Calculate(work).ToText($"Function BASE64URLTOTEXT parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }
            try {
                Encoding encoding;
                if (args.Count == 1) {
                    encoding = Encoding.UTF8;
                } else {
                    encoding = Encoding.GetEncoding(args[1].TextValue);
                }
                var t = encoding.GetString(Base64.FromBase64ForUrlString(args[0].TextValue));
                return Operand.Create(t);
            } catch (Exception) { }
            return Operand.Error("Function BASE64URLTOTEXT is error!");
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("BASE64URLTOTEXT(");
            for (int i = 0; i < funcs.Length; i++) {
                if (i > 0) {
                    stringBuilder.Append(", ");
                }
                funcs[i].ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_TEXTTOBASE64 : Function_N
    {
        public Function_TEXTTOBASE64(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var a = item.Calculate(work).ToText($"Function TEXTTOBASE64 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }
            try {
                Encoding encoding;
                if (args.Count == 1) {
                    encoding = Encoding.UTF8;
                } else {
                    encoding = Encoding.GetEncoding(args[1].TextValue);
                }
                var bytes = encoding.GetBytes(args[0].TextValue);
                var t = Base64.ToBase64String(bytes);
                return Operand.Create(t);
            } catch (Exception) {
            }
            return Operand.Error("Function TEXTTOBASE64 is error!");
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("TEXTTOBASE64(");
            for (int i = 0; i < funcs.Length; i++) {
                if (i > 0) {
                    stringBuilder.Append(", ");
                }
                funcs[i].ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_TEXTTOBASE64URL : Function_N
    {
        public Function_TEXTTOBASE64URL(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var a = item.Calculate(work).ToText($"Function TEXTTOBASE64URL parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }
            try {
                Encoding encoding;
                if (args.Count == 1) {
                    encoding = Encoding.UTF8;
                } else {
                    encoding = Encoding.GetEncoding(args[1].TextValue);
                }
                var bytes = encoding.GetBytes(args[0].TextValue);
                var t = Base64.ToBase64ForUrlString(bytes);
                return Operand.Create(t);
            } catch (Exception) { }
            return Operand.Error("Function TEXTTOBASE64URL is error!");
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("TEXTTOBASE64URL(");
            for (int i = 0; i < funcs.Length; i++) {
                if (i > 0) {
                    stringBuilder.Append(", ");
                }
                funcs[i].ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_REGEX : Function_2
    {
        public Function_REGEX(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work).ToText("Function REGEX parameter 1 is error!");
            if (args1.IsError) { return args1; }
            var args2 = func2.Calculate(work).ToText("Function REGEX parameter 2 is error!");
            if (args2.IsError) { return args2; }

            var b = Regex.Match(args1.TextValue, args2.TextValue);
            if (b.Success == false) {
                return Operand.Error("Function REGEX is error!");
            }
            return Operand.Create(b.Value);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("REGEX(");
            func1.ToString(stringBuilder, false);
            if (func2 != null) {
                stringBuilder.Append(", ");
                func2.ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_REGEXREPALCE : Function_3
    {
        public Function_REGEXREPALCE(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function REGEXREPALCE parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function REGEXREPALCE parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = func3.Calculate(work); if (args3.Type != OperandType.TEXT) { args3 = args3.ToText("Function REGEXREPALCE parameter 3 error!"); if (args3.IsError) return args3; }
            var b = Regex.Replace(args1.TextValue, args2.TextValue, args3.TextValue);
            return Operand.Create(b);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("REGEXREPALCE(");
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

    internal class Function_ISREGEX : Function_2
    {
        public Function_ISREGEX(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function ISREGEX parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function ISREGEX parameter 2 error!"); if (args2.IsError) return args2; }
            var b = Regex.IsMatch(args1.TextValue, args2.TextValue);
            return Operand.Create(b);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("ISREGEX(");
            func1.ToString(stringBuilder, false);
            if (func2 != null) {
                stringBuilder.Append(", ");
                func2.ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_GUID : FunctionBase
    {
        public Function_GUID()
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            return Operand.Create(System.Guid.NewGuid().ToString());
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("GUID()");
        }
    }

    internal class Function_MD5 : Function_N
    {
        public Function_MD5(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var a = item.Calculate(work).ToText($"Function MD5 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }
            try {
                Encoding encoding;
                if (args.Count == 1) {
                    encoding = Encoding.UTF8;
                } else {
                    encoding = Encoding.GetEncoding(args[1].TextValue);
                }
                var t = Hash.GetMd5String(encoding.GetBytes(args[0].TextValue));
                return Operand.Create(t);
            } catch (Exception ex) {
                return Operand.Error("Function MD5 is error!" + ex.Message);
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("MD5(");
            for (int i = 0; i < funcs.Length; i++) {
                if (i > 0) {
                    stringBuilder.Append(", ");
                }
                funcs[i].ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_SHA1 : Function_N
    {
        public Function_SHA1(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var a = item.Calculate(work).ToText($"Function SHA1 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }
            try {
                Encoding encoding;
                if (args.Count == 1) {
                    encoding = Encoding.UTF8;
                } else {
                    encoding = Encoding.GetEncoding(args[1].TextValue);
                }
                var t = Hash.GetSha1String(encoding.GetBytes(args[0].TextValue));
                return Operand.Create(t);
            } catch (Exception ex) {
                return Operand.Error("Function SHA1 is error!" + ex.Message);
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("SHA1(");
            for (int i = 0; i < funcs.Length; i++) {
                if (i > 0) {
                    stringBuilder.Append(", ");
                }
                funcs[i].ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_SHA256 : Function_N
    {
        public Function_SHA256(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var a = item.Calculate(work).ToText($"Function SHA256 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }
            try {
                Encoding encoding;
                if (args.Count == 1) {
                    encoding = Encoding.UTF8;
                } else {
                    encoding = Encoding.GetEncoding(args[1].TextValue);
                }
                var t = Hash.GetSha256String(encoding.GetBytes(args[0].TextValue));
                return Operand.Create(t);
            } catch (Exception ex) {
                return Operand.Error("Function SHA256 is error!" + ex.Message);
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("SHA256(");
            for (int i = 0; i < funcs.Length; i++) {
                if (i > 0) {
                    stringBuilder.Append(", ");
                }
                funcs[i].ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_SHA512 : Function_N
    {
        public Function_SHA512(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var a = item.Calculate(work).ToText($"Function SHA512 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }
            try {
                Encoding encoding;
                if (args.Count == 1) {
                    encoding = Encoding.UTF8;
                } else {
                    encoding = Encoding.GetEncoding(args[1].TextValue);
                }
                var t = Hash.GetSha512String(encoding.GetBytes(args[0].TextValue));
                return Operand.Create(t);
            } catch (Exception ex) {
                return Operand.Error("Function SHA512 is error!" + ex.Message);
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("SHA512(");
            for (int i = 0; i < funcs.Length; i++) {
                if (i > 0) {
                    stringBuilder.Append(", ");
                }
                funcs[i].ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_CRC32 : Function_N
    {
        public Function_CRC32(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var a = item.Calculate(work).ToText($"Function CRC32 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }
            try {
                Encoding encoding;
                if (args.Count == 1) {
                    encoding = Encoding.UTF8;
                } else {
                    encoding = Encoding.GetEncoding(args[1].TextValue);
                }
                var t = Hash.GetCrc32String(encoding.GetBytes(args[0].TextValue));
                return Operand.Create(t);
            } catch (Exception ex) {
                return Operand.Error("Function CRC32 is error!" + ex.Message);
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("CRC32(");
            for (int i = 0; i < funcs.Length; i++) {
                if (i > 0) {
                    stringBuilder.Append(", ");
                }
                funcs[i].ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_HMACMD5 : Function_N
    {
        public Function_HMACMD5(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var a = item.Calculate(work).ToText($"Function HMACMD5 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }
            try {
                Encoding encoding;
                if (args.Count == 2) {
                    encoding = Encoding.UTF8;
                } else {
                    encoding = Encoding.GetEncoding(args[2].TextValue);
                }
                var t = Hash.GetHmacMd5String(encoding.GetBytes(args[0].TextValue), args[1].TextValue);
                return Operand.Create(t);
            } catch (Exception ex) {
                return Operand.Error("Function HMACMD5 is error!" + ex.Message);
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("HMACMD5(");
            for (int i = 0; i < funcs.Length; i++) {
                if (i > 0) {
                    stringBuilder.Append(", ");
                }
                funcs[i].ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_HMACSHA1 : Function_N
    {
        public Function_HMACSHA1(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var a = item.Calculate(work).ToText($"Function HMACSHA1 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }
            try {
                Encoding encoding;
                if (args.Count == 2) {
                    encoding = Encoding.UTF8;
                } else {
                    encoding = Encoding.GetEncoding(args[2].TextValue);
                }
                var t = Hash.GetHmacSha1String(encoding.GetBytes(args[0].TextValue), args[1].TextValue);
                return Operand.Create(t);
            } catch (Exception ex) {
                return Operand.Error("Function HMACSHA1 is error!" + ex.Message);
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("HMACSHA1(");
            for (int i = 0; i < funcs.Length; i++) {
                if (i > 0) {
                    stringBuilder.Append(", ");
                }
                funcs[i].ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_HMACSHA256 : Function_N
    {
        public Function_HMACSHA256(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var a = item.Calculate(work).ToText($"Function HMACSHA256 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }
            try {
                Encoding encoding;
                if (args.Count == 2) {
                    encoding = Encoding.UTF8;
                } else {
                    encoding = Encoding.GetEncoding(args[2].TextValue);
                }
                var t = Hash.GetHmacSha256String(encoding.GetBytes(args[0].TextValue), args[1].TextValue);
                return Operand.Create(t);
            } catch (Exception ex) {
                return Operand.Error("Function HMACSHA256 is error!" + ex.Message);
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("HMACSHA256(");
            for (int i = 0; i < funcs.Length; i++) {
                if (i > 0) {
                    stringBuilder.Append(", ");
                }
                funcs[i].ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_HMACSHA512 : Function_N
    {
        public Function_HMACSHA512(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var a = item.Calculate(work).ToText($"Function HMACSHA512 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }
            try {
                Encoding encoding;
                if (args.Count == 2) {
                    encoding = Encoding.UTF8;
                } else {
                    encoding = Encoding.GetEncoding(args[2].TextValue);
                }
                var t = Hash.GetHmacSha512String(encoding.GetBytes(args[0].TextValue), args[1].TextValue);
                return Operand.Create(t);
            } catch (Exception ex) {
                return Operand.Error("Function HMACSHA512 is error!" + ex.Message);
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("HMACSHA512(");
            for (int i = 0; i < funcs.Length; i++) {
                if (i > 0) {
                    stringBuilder.Append(", ");
                }
                funcs[i].ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_TRIMSTART : Function_N
    {
        public Function_TRIMSTART(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var a = item.Calculate(work).ToText($"Function TRIMSTART parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }
            if (args.Count == 1) {
                return Operand.Create(args[0].TextValue.TrimStart());
            } else {
                char[] trimChars = args[1].TextValue.ToCharArray();
                return Operand.Create(args[0].TextValue.TrimStart(trimChars));
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("TRIMSTART(");
            for (int i = 0; i < funcs.Length; i++) {
                if (i > 0) {
                    stringBuilder.Append(", ");
                }
                funcs[i].ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_TRIMEND : Function_N
    {
        public Function_TRIMEND(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var a = item.Calculate(work).ToText($"Function TRIMEND parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }
            if (args.Count == 1) {
                return Operand.Create(args[0].TextValue.TrimEnd());
            } else {
                char[] trimChars = args[1].TextValue.ToCharArray();
                return Operand.Create(args[0].TextValue.TrimEnd(trimChars));
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("TRIMEND(");
            for (int i = 0; i < funcs.Length; i++) {
                if (i > 0) {
                    stringBuilder.Append(", ");
                }
                funcs[i].ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_INDEXOF : Function_N
    {
        public Function_INDEXOF(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = funcs[0].Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function INDEXOF parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = funcs[1].Calculate(work); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function INDEXOF parameter 2 is error!"); if (args2.IsError) { return args2; } }
            var text = args1.TextValue;
            if (funcs.Length == 2) {
                return Operand.Create(text.AsSpan().IndexOf(args2.TextValue) + work.ExcelIndex);
            }
            var args3 = funcs[2].Calculate(work); if (args3.Type != OperandType.TEXT) { args3 = args3.ToText("Function INDEXOF parameter 3 is error!"); if (args3.IsError) { return args3; } }
            if (funcs.Length == 3) {
                return Operand.Create(text.AsSpan(args3.IntValue).IndexOf(args2.TextValue) + args3.IntValue + work.ExcelIndex);
            }
            var args4 = funcs[3].Calculate(work); if (args4.Type != OperandType.TEXT) { args4 = args4.ToText("Function INDEXOF parameter 4 is error!"); if (args4.IsError) { return args4; } }
            return Operand.Create(text.IndexOf(args2.TextValue, args3.IntValue, args4.IntValue) + work.ExcelIndex);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("INDEXOF(");
            for (int i = 0; i < funcs.Length; i++) {
                if (i > 0) {
                    stringBuilder.Append(", ");
                }
                funcs[i].ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_LASTINDEXOF : Function_N
    {
        public Function_LASTINDEXOF(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = funcs[0].Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function LASTINDEXOF parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = funcs[1].Calculate(work); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function LASTINDEXOF parameter 2 is error!"); if (args2.IsError) { return args2; } }
            var text = args1.TextValue;
            if (funcs.Length == 2) {
                return Operand.Create(text.AsSpan().LastIndexOf(args2.TextValue) + work.ExcelIndex);
            }
            var args3 = funcs[2].Calculate(work); if (args3.Type != OperandType.TEXT) { args3 = args3.ToText("Function LASTINDEXOF parameter 3 is error!"); if (args3.IsError) { return args3; } }
            if (funcs.Length == 3) {
                return Operand.Create(text.AsSpan(0, args3.IntValue).LastIndexOf(args2.TextValue) + work.ExcelIndex);
            }
            var args4 = funcs[3].Calculate(work); if (args4.Type != OperandType.TEXT) { args4 = args4.ToText("Function LASTINDEXOF parameter 4 is error!"); if (args4.IsError) { return args4; } }
            return Operand.Create(text.LastIndexOf(args2.TextValue, args3.IntValue, args4.IntValue) + work.ExcelIndex);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("LASTINDEXOF(");
            for (int i = 0; i < funcs.Length; i++) {
                if (i > 0) {
                    stringBuilder.Append(", ");
                }
                funcs[i].ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_SPLIT : Function_2
    {
        public Function_SPLIT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function SPLIT parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function SPLIT parameter 2 is error!"); if (args2.IsError) { return args2; } }
            return Operand.Create(args1.TextValue.Split(args2.TextValue.ToArray()));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("SPLIT(");
            func1.ToString(stringBuilder, false);
            if (func2 != null) {
                stringBuilder.Append(", ");
                func2.ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_JOIN : Function_N
    {
        public Function_JOIN(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>();
            foreach (var item in funcs) { var aa = item.Calculate(work); if (aa.IsError) { return aa; } args.Add(aa); }
            var args1 = args[0];
            if (args1.Type == OperandType.JSON) {
                var o = args1.ToArray(null);
                if (o.IsError == false) {
                    args1 = o;
                }
            }
            if (args1.Type == OperandType.ARRARY) {
                List<string> list = new List<string>();
                var o = FunctionUtil.F_base_GetList(args1, list);
                if (o == false) return Operand.Error("Function JOIN parameter 1 is error!");

                var args2 = args[1].ToText("Function JOIN parameter 2 is error!");
                if (args2.IsError) { return args2; }

                return Operand.Create(string.Join(args2.TextValue, list));
            } else {
                args1 = args1.ToText("Function JOIN parameter 1 is error!");
                if (args1.IsError) { return args1; }

                List<string> list = new List<string>();
                for (int i = 1; i < args.Count; i++) {
                    var o = FunctionUtil.F_base_GetList(args[i], list);
                    if (o == false) return Operand.Error($"Function JOIN parameter {i + 1} is error!");
                }
                return Operand.Create(string.Join(args1.TextValue, list));
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("JOIN(");
            for (int i = 0; i < funcs.Length; i++) {
                if (i > 0) {
                    stringBuilder.Append(", ");
                }
                funcs[i].ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_SUBSTRING : Function_N
    {
        public Function_SUBSTRING(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = funcs[0].Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function SUBSTRING parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = funcs[1].Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function SUBSTRING parameter 2 is error!"); if (args2.IsError) { return args2; } }

            var text = args1.TextValue;
            if (funcs.Length == 2) {
                return Operand.Create(text.AsSpan(args2.IntValue - work.ExcelIndex).ToString());
            }
            var args3 = funcs[2].Calculate(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function SUBSTRING parameter 3 is error!"); if (args3.IsError) { return args3; } }
            return Operand.Create(text.AsSpan(args2.IntValue - work.ExcelIndex, args3.IntValue).ToString());
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("SUBSTRING(");
            for (int i = 0; i < funcs.Length; i++) {
                if (i > 0) {
                    stringBuilder.Append(", ");
                }
                funcs[i].ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_STARTSWITH : Function_N
    {
        public Function_STARTSWITH(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = funcs[0].Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function STARTSWITH parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = funcs[1].Calculate(work); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function STARTSWITH parameter 2 is error!"); if (args2.IsError) { return args2; } }

            var text = args1.TextValue;
            if (funcs.Length == 2) {
                return Operand.Create(text.AsSpan().StartsWith(args2.TextValue.AsSpan()));
            }
            var args3 = funcs[2].Calculate(work); if (args3.Type != OperandType.BOOLEAN) { args3 = args3.ToBoolean("Function STARTSWITH parameter 3 is error!"); if (args3.IsError) { return args3; } }
            return Operand.Create(text.AsSpan().StartsWith(args2.TextValue.AsSpan(), args3.BooleanValue ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("STARTSWITH(");
            for (int i = 0; i < funcs.Length; i++) {
                if (i > 0) {
                    stringBuilder.Append(", ");
                }
                funcs[i].ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_ENDSWITH : Function_N
    {
        public Function_ENDSWITH(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = funcs[0].Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function ENDSWITH parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = funcs[1].Calculate(work); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function ENDSWITH parameter 2 is error!"); if (args2.IsError) { return args2; } }
            var text = args1.TextValue;
            if (funcs.Length == 2) {
                return Operand.Create(text.AsSpan().EndsWith(args2.TextValue.AsSpan()));
            }
            var args3 = funcs[2].Calculate(work); if (args3.Type != OperandType.BOOLEAN) { args3 = args3.ToBoolean("Function ENDSWITH parameter 3 is error!"); if (args3.IsError) { return args3; } }
            return Operand.Create(text.AsSpan().EndsWith(args2.TextValue.AsSpan(), args3.BooleanValue ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("ENDSWITH(");
            for (int i = 0; i < funcs.Length; i++) {
                if (i > 0) {
                    stringBuilder.Append(", ");
                }
                funcs[i].ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_ISNULLOREMPTY : Function_1
    {
        public Function_ISNULLOREMPTY(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function ISNULLOREMPTY parameter 1 is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(string.IsNullOrEmpty(args1.TextValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("ISNULLOREMPTY(");
            func1.ToString(stringBuilder, false);
            stringBuilder.Append(')');
        }
    }

    internal class Function_ISNULLORWHITESPACE : Function_1
    {
        public Function_ISNULLORWHITESPACE(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function ISNULLORWHITESPACE parameter 1 is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(string.IsNullOrWhiteSpace(args1.TextValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("ISNULLORWHITESPACE(");
            func1.ToString(stringBuilder, false);
            stringBuilder.Append(')');
        }
    }

    internal class Function_REMOVESTART : Function_N
    {
        public Function_REMOVESTART(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = funcs[0].Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function REMOVESTART parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = funcs[1].Calculate(work); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function REMOVESTART parameter 2 is error!"); if (args2.IsError) { return args2; } }
            StringComparison comparison = StringComparison.Ordinal;
            if (funcs.Length == 3) {
                var args3 = funcs[2].Calculate(work); if (args3.Type != OperandType.BOOLEAN) { args3 = args3.ToBoolean("Function REMOVESTART parameter 3 is error!"); if (args3.IsError) { return args3; } }
                if (args3.BooleanValue) {
                    comparison = StringComparison.OrdinalIgnoreCase;
                }
            }
            var text = args1.TextValue;
            if (text.StartsWith(args2.TextValue, comparison)) {
                return Operand.Create(text.AsSpan(args2.TextValue.Length).ToString());
            }
            return args1;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("REMOVESTART(");
            for (int i = 0; i < funcs.Length; i++) {
                if (i > 0) {
                    stringBuilder.Append(", ");
                }
                funcs[i].ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_REMOVEEND : Function_N
    {
        public Function_REMOVEEND(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = funcs[0].Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function REMOVEEND parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = funcs[1].Calculate(work); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function REMOVEEND parameter 2 is error!"); if (args2.IsError) { return args2; } }
            StringComparison comparison = StringComparison.Ordinal;
            if (funcs.Length == 3) {
                var args3 = funcs[2].Calculate(work); if (args3.Type != OperandType.BOOLEAN) { args3 = args3.ToBoolean("Function REMOVEEND parameter 3 is error!"); if (args3.IsError) { return args3; } }
                if (args3.BooleanValue) {
                    comparison = StringComparison.OrdinalIgnoreCase;
                }
            }
            var text = args1.TextValue;
            if (text.EndsWith(args2.TextValue, comparison)) {
                return Operand.Create(text.AsSpan(0, text.Length - args2.TextValue.Length).ToString());
            }
            return args1;
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("REMOVEEND(");
            for (int i = 0; i < funcs.Length; i++) {
                if (i > 0) {
                    stringBuilder.Append(", ");
                }
                funcs[i].ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_JSON : Function_1
    {
        public Function_JSON(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work);
            if (args1.Type == OperandType.JSON) { return args1; }
            args1 = args1.ToText("Function JSON parameter is error!");
            if (args1.IsError) { return args1; }
            var txt = args1.TextValue;
            if ((txt.StartsWith('{') && txt.EndsWith('}')) || (txt.StartsWith('[') && txt.EndsWith(']'))) {
                try {
                    var json = JsonMapper.ToObject(txt);
                    return Operand.Create(json);
                } catch (Exception) { }
            }
            return Operand.Error("Function JSON parameter is error!");
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("JSON(");
            func1.ToString(stringBuilder, false);
            stringBuilder.Append(')');
        }
    }

    internal class Function_HAS : Function_2
    {
        public Function_HAS(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsError) { return args1; }
            var args2 = func2.Calculate(work).ToText("Function HAS parameter 2 is error!"); if (args2.IsError) { return args2; }

            if (args1.Type == OperandType.ARRARYJSON) {
                return Operand.Create(((OperandKeyValueList)args1).ContainsKey(args2));
            } else if (args1.Type == OperandType.JSON) {
                var json = args1.JsonValue;
                if (json.IsArray) {
                    for (int i = 0; i < json.Count; i++) {
                        var v = json[i];
                        if (v.IsString) {
                            if (v.StringValue == args2.TextValue) { return Operand.True; }
                        } else if (v.IsDouble) {
                            if (v.NumberValue.ToString() == args2.TextValue) { return Operand.True; }
                        } else if (v.IsBoolean) {
                            if (v.BooleanValue.ToString().ToUpper() == args2.TextValue) { return Operand.True; }
                        }
                    }
                } else {
                    var v = json[args2.TextValue];
                    if (v != null) {
                        return Operand.True;
                    }
                }
                return Operand.False;
            } else if (args1.Type == OperandType.ARRARY) {
                var ar = ((OperandArray)args1);
                foreach (var item in ar.ArrayValue) {
                    var t = item.ToText();
                    if (t.IsError) { continue; }
                    if (t.TextValue == args2.TextValue) {
                        return Operand.True;
                    }
                }
                return Operand.False;
            }
            return Operand.Error("Function HAS parameter 1 is error!");
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("HAS(");
            func1.ToString(stringBuilder, false);
            if (func2 != null) {
                stringBuilder.Append(", ");
                func2.ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }

    internal class Function_HASVALUE : Function_2
    {
        public Function_HASVALUE(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.IsError) { return args1; }
            var args2 = func2.Calculate(work).ToText("Function HASVALUE parameter 2 is error!"); if (args2.IsError) { return args2; }

            if (args1.Type == OperandType.ARRARYJSON) {
                return Operand.Create(((OperandKeyValueList)args1).ContainsValue(args2));
            } else if (args1.Type == OperandType.JSON) {
                var json = args1.JsonValue;
                if (json.IsArray) {
                    for (int i = 0; i < json.Count; i++) {
                        var v = json[i];
                        if (v.IsString) {
                            if (v.StringValue == args2.TextValue) { return Operand.True; }
                        } else if (v.IsDouble) {
                            if (v.NumberValue.ToString() == args2.TextValue) { return Operand.True; }
                        } else if (v.IsBoolean) {
                            if (v.BooleanValue.ToString().ToUpper() == args2.TextValue) { return Operand.True; }
                        }
                    }
                } else {
                    foreach (var item in json.inst_object) {
                        var v = item.Value;
                        if (v.IsString) {
                            if (v.StringValue == args2.TextValue) { return Operand.True; }
                        } else if (v.IsDouble) {
                            if (v.NumberValue.ToString() == args2.TextValue) { return Operand.True; }
                        } else if (v.IsBoolean) {
                            if (v.BooleanValue.ToString().ToUpper() == args2.TextValue) { return Operand.True; }
                        }
                    }
                }
                return Operand.False;
            } else if (args1.Type == OperandType.ARRARY) {
                var ar = ((OperandArray)args1);
                foreach (var item in ar.ArrayValue) {
                    var t = item.ToText();
                    if (t.IsError) { continue; }
                    if (t.TextValue == args2.TextValue) {
                        return Operand.True;
                    }
                }
                return Operand.False;
            }
            return Operand.Error("Function HASVALUE parameter 1 is error!");
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("HASVALUE(");
            func1.ToString(stringBuilder, false);
            if (func2 != null) {
                stringBuilder.Append(", ");
                func2.ToString(stringBuilder, false);
            }
            stringBuilder.Append(')');
        }
    }


    #endregion csharp
}
