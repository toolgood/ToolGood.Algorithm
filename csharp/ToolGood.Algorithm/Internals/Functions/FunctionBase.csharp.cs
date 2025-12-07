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
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function 'UrlEncode' parameter error!"); if (args1.IsError) { return args1; } }
            var s = args1.TextValue;
            var r = System.Web.HttpUtility.UrlEncode(s);
            return Operand.Create(r);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "UrlEncode");
        }
    }

    internal class Function_URLDECODE : Function_1
    {
        public Function_URLDECODE(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function 'UrlDecode' parameter error!"); if (args1.IsError) { return args1; } }
            var s = args1.TextValue;
            var r = System.Web.HttpUtility.UrlDecode(s);
            return Operand.Create(r);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "UrlDecode");
        }
    }

    internal class Function_HTMLENCODE : Function_1
    {
        public Function_HTMLENCODE(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function 'HtmlEncode' parameter error!"); if (args1.IsError) { return args1; } }
            var s = args1.TextValue;
            var r = System.Web.HttpUtility.HtmlEncode(s);
            return Operand.Create(r);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "HtmlEncode");
        }
    }

    internal class Function_HTMLDECODE : Function_1
    {
        public Function_HTMLDECODE(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function 'HtmlDecode' parameter error!"); if (args1.IsError) { return args1; } }
            var s = args1.TextValue;
            var r = System.Web.HttpUtility.HtmlDecode(s);
            return Operand.Create(r);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "HtmlDecode");
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
            foreach (var item in funcs) { var a = item.Calculate(work).ToText("Function 'Base64ToText' parameter {0} is error!", index++); if (a.IsError) { return a; } args.Add(a); }
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
            return Operand.Error("Function Base64ToText is error!");
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Base64ToText");
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
            foreach (var item in funcs) { var a = item.Calculate(work).ToText("Function 'Base64urlToText' parameter {0} is error!", index++); if (a.IsError) { return a; } args.Add(a); }
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
            return Operand.Error("Function Base64urlToText is error!");
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Base64urlToText");
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
            foreach (var item in funcs) { var a = item.Calculate(work).ToText("Function 'TextToBase64' parameter {0} is error!", index++); if (a.IsError) { return a; } args.Add(a); }
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
            return Operand.Error("Function TextToBase64 is error!");
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "TextToBase64");
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
            foreach (var item in funcs) { var a = item.Calculate(work).ToText("Function 'TextToBase64url' parameter {0} is error!", index++); if (a.IsError) { return a; } args.Add(a); }
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
            return Operand.Error("Function TextToBase64url is error!");
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "TextToBase64url");
        }
    }

    internal class Function_REGEX : Function_2
    {
        public Function_REGEX(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work).ToText("Function 'Regex' parameter {0} is error!",1);
            if (args1.IsError) { return args1; }
            var args2 = func2.Calculate(work).ToText("Function 'Regex' parameter {0} is error!",2);
            if (args2.IsError) { return args2; }

            var b = Regex.Match(args1.TextValue, args2.TextValue);
            if (b.Success == false) {
                return Operand.Error("Function Regex is error!");
            }
            return Operand.Create(b.Value);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Regex");
        }
    }

    internal class Function_REGEXREPALCE : Function_3
    {
        public Function_REGEXREPALCE(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function 'RegexReplace' parameter {0} is error!", 1); if (args1.IsError) return args1; }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function 'RegexReplace' parameter {0} is error!", 2); if (args2.IsError) return args2; }
            var args3 = func3.Calculate(work); if (args3.Type != OperandType.TEXT) { args3 = args3.ToText("Function 'RegexReplace' parameter {0} is error!", 3); if (args3.IsError) return args3; }
            var b = Regex.Replace(args1.TextValue, args2.TextValue, args3.TextValue);
            return Operand.Create(b);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "RegexReplace");
        }
    }

    internal class Function_ISREGEX : Function_2
    {
        public Function_ISREGEX(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function 'IsRegex' parameter {0} is error!", 1); if (args1.IsError) return args1; }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function 'IsRegex' parameter {0} is error!", 2); if (args2.IsError) return args2; }
            var b = Regex.IsMatch(args1.TextValue, args2.TextValue);
            return Operand.Create(b);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "IsRegex");
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
            foreach (var item in funcs) { var a = item.Calculate(work).ToText("Function 'MD5' parameter {0} is error!", index++); if (a.IsError) { return a; } args.Add(a); }
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
            AddFunction(stringBuilder, "MD5");
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
            foreach (var item in funcs) { var a = item.Calculate(work).ToText("Function 'SHA1' parameter {0} is error!", index++); if (a.IsError) { return a; } args.Add(a); }
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
            AddFunction(stringBuilder, "SHA1");
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
            foreach (var item in funcs) { var a = item.Calculate(work).ToText("Function 'SHA256' parameter {0} is error!", index++); if (a.IsError) { return a; } args.Add(a); }
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
            AddFunction(stringBuilder, "SHA256");
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
            foreach (var item in funcs) { var a = item.Calculate(work).ToText("Function 'SHA512' parameter {0} is error!", index++); if (a.IsError) { return a; } args.Add(a); }
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
            AddFunction(stringBuilder, "SHA512");
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
            foreach (var item in funcs) { var a = item.Calculate(work).ToText("Function 'CRC32' parameter {0} is error!", index++); if (a.IsError) { return a; } args.Add(a); }
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
            AddFunction(stringBuilder, "CRC32");
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
            foreach (var item in funcs) { var a = item.Calculate(work).ToText("Function 'HmacMD5' parameter {0} is error!", index++); if (a.IsError) { return a; } args.Add(a); }
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
                return Operand.Error("Function HmacMD5 is error!" + ex.Message);
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "HmacMD5");
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
            foreach (var item in funcs) { var a = item.Calculate(work).ToText("Function 'HmacSHA1' parameter {0} is error!", index++); if (a.IsError) { return a; } args.Add(a); }
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
                return Operand.Error("Function HmacSHA1 is error!" + ex.Message);
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "HmacSHA1");
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
            foreach (var item in funcs) { var a = item.Calculate(work).ToText("Function 'HmacSHA256' parameter {0} is error!", index++); if (a.IsError) { return a; } args.Add(a); }
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
                return Operand.Error("Function HmacSHA256 is error!" + ex.Message);
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "HmacSHA256");
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
            foreach (var item in funcs) { var a = item.Calculate(work).ToText("Function 'HmacSHA512' parameter {0} is error!", index++); if (a.IsError) { return a; } args.Add(a); }
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
                return Operand.Error("Function HmacSHA512 is error!" + ex.Message);
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "HmacSHA512");
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
            foreach (var item in funcs) { var a = item.Calculate(work).ToText("Function 'TrimStart' parameter {0} is error!", index++); if (a.IsError) { return a; } args.Add(a); }
            if (args.Count == 1) {
                return Operand.Create(args[0].TextValue.TrimStart());
            } else {
                char[] trimChars = args[1].TextValue.ToCharArray();
                return Operand.Create(args[0].TextValue.TrimStart(trimChars));
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "TrimStart");
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
            foreach (var item in funcs) { var a = item.Calculate(work).ToText("Function 'TrimEnd' parameter {0} is error!", index++); if (a.IsError) { return a; } args.Add(a); }
            if (args.Count == 1) {
                return Operand.Create(args[0].TextValue.TrimEnd());
            } else {
                char[] trimChars = args[1].TextValue.ToCharArray();
                return Operand.Create(args[0].TextValue.TrimEnd(trimChars));
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "TrimEnd");
        }
    }

    internal class Function_INDEXOF : Function_N
    {
        public Function_INDEXOF(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = funcs[0].Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function 'IndexOf' parameter {0} is error!",1); if (args1.IsError) { return args1; } }
            var args2 = funcs[1].Calculate(work); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function 'IndexOf' parameter {0} is error!",2); if (args2.IsError) { return args2; } }
            var text = args1.TextValue;
            if (funcs.Length == 2) {
                return Operand.Create(text.AsSpan().IndexOf(args2.TextValue) + work.ExcelIndex);
            }
            var args3 = funcs[2].Calculate(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function 'IndexOf' parameter {0} is error!",3); if (args3.IsError) { return args3; } }
            if (funcs.Length == 3) {
                return Operand.Create(text.AsSpan(args3.IntValue).IndexOf(args2.TextValue) + args3.IntValue + work.ExcelIndex);
            }
            var args4 = funcs[3].Calculate(work); if (args4.Type != OperandType.NUMBER) { args4 = args4.ToNumber("Function 'IndexOf' parameter {0} is error!",4); if (args4.IsError) { return args4; } }
            return Operand.Create(text.IndexOf(args2.TextValue, args3.IntValue, args4.IntValue) + work.ExcelIndex);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "IndexOf");
        }
    }

    internal class Function_LASTINDEXOF : Function_N
    {
        public Function_LASTINDEXOF(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = funcs[0].Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function 'LastIndexOf' parameter {0} is error!",1); if (args1.IsError) { return args1; } }
            var args2 = funcs[1].Calculate(work); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function 'LastIndexOf' parameter {0} is error!",2); if (args2.IsError) { return args2; } }
            var text = args1.TextValue;
            if (funcs.Length == 2) {
                return Operand.Create(text.AsSpan().LastIndexOf(args2.TextValue) + work.ExcelIndex);
            }
            var args3 = funcs[2].Calculate(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function 'LastIndexOf' parameter {0} is error!",3); if (args3.IsError) { return args3; } }
            if (funcs.Length == 3) {
                return Operand.Create(text.AsSpan(0, args3.IntValue).LastIndexOf(args2.TextValue) + work.ExcelIndex);
            }
            var args4 = funcs[3].Calculate(work); if (args4.Type != OperandType.NUMBER) { args4 = args4.ToNumber("Function 'LastIndexOf' parameter {0} is error!",4); if (args4.IsError) { return args4; } }
            return Operand.Create(text.LastIndexOf(args2.TextValue, args3.IntValue, args4.IntValue) + work.ExcelIndex);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "LastIndexOf");
        }
    }

    internal class Function_SPLIT : Function_2
    {
        public Function_SPLIT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function 'Split' parameter {0} is error!",1); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function 'Split' parameter {0} is error!",2); if (args2.IsError) { return args2; } }
            return Operand.Create(args1.TextValue.Split(args2.TextValue.ToArray()));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Split");
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
                if (o == false) return Operand.Error("Function 'Join' parameter {0} is error!",1);

                var args2 = args[1].ToText("Function 'Join' parameter {0} is error!",2);
                if (args2.IsError) { return args2; }

                return Operand.Create(string.Join(args2.TextValue, list));
            } else {
                args1 = args1.ToText("Function 'Join' parameter {0} is error!",1);
                if (args1.IsError) { return args1; }

                List<string> list = new List<string>();
                for (int i = 1; i < args.Count; i++) {
                    var o = FunctionUtil.F_base_GetList(args[i], list);
                    if (o == false) return Operand.Error("Function 'Join' parameter {0} is error!",i+1);
                }
                return Operand.Create(string.Join(args1.TextValue, list));
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Join");
        }
    }

    internal class Function_SUBSTRING : Function_N
    {
        public Function_SUBSTRING(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = funcs[0].Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function 'Substring' parameter {0} is error!",1); if (args1.IsError) { return args1; } }
            var args2 = funcs[1].Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function 'Substring' parameter {0} is error!",2); if (args2.IsError) { return args2; } }

            var text = args1.TextValue;
            if (funcs.Length == 2) {
                return Operand.Create(text.AsSpan(args2.IntValue - work.ExcelIndex).ToString());
            }
            var args3 = funcs[2].Calculate(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function 'Substring' parameter {0} is error!",3); if (args3.IsError) { return args3; } }
            return Operand.Create(text.AsSpan(args2.IntValue - work.ExcelIndex, args3.IntValue).ToString());
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Substring");
        }
    }

    internal class Function_STARTSWITH : Function_N
    {
        public Function_STARTSWITH(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = funcs[0].Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function 'StartsWith' parameter {0} is error!",1); if (args1.IsError) { return args1; } }
            var args2 = funcs[1].Calculate(work); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function 'StartsWith' parameter {0} is error!",2); if (args2.IsError) { return args2; } }

            var text = args1.TextValue;
            if (funcs.Length == 2) {
                return Operand.Create(text.AsSpan().StartsWith(args2.TextValue.AsSpan()));
            }
            var args3 = funcs[2].Calculate(work); if (args3.Type != OperandType.BOOLEAN) { args3 = args3.ToBoolean("Function 'StartsWith' parameter {0} is error!",3); if (args3.IsError) { return args3; } }
            return Operand.Create(text.AsSpan().StartsWith(args2.TextValue.AsSpan(), args3.BooleanValue ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "StartsWith");
        }
    }

    internal class Function_ENDSWITH : Function_N
    {
        public Function_ENDSWITH(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = funcs[0].Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function 'EndsWith' parameter {0} is error!",1); if (args1.IsError) { return args1; } }
            var args2 = funcs[1].Calculate(work); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function 'EndsWith' parameter {0} is error!",2); if (args2.IsError) { return args2; } }
            var text = args1.TextValue;
            if (funcs.Length == 2) {
                return Operand.Create(text.AsSpan().EndsWith(args2.TextValue.AsSpan()));
            }
            var args3 = funcs[2].Calculate(work); if (args3.Type != OperandType.BOOLEAN) { args3 = args3.ToBoolean("Function 'EndsWith' parameter {0} is error!",3); if (args3.IsError) { return args3; } }
            return Operand.Create(text.AsSpan().EndsWith(args2.TextValue.AsSpan(), args3.BooleanValue ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "EndsWith");
        }
    }

    internal class Function_REMOVESTART : Function_N
    {
        public Function_REMOVESTART(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = funcs[0].Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function 'RemoveStart' parameter {0} is error!",1); if (args1.IsError) { return args1; } }
            var args2 = funcs[1].Calculate(work); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function 'RemoveStart' parameter {0} is error!",2); if (args2.IsError) { return args2; } }
            StringComparison comparison = StringComparison.Ordinal;
            if (funcs.Length == 3) {
                var args3 = funcs[2].Calculate(work); if (args3.Type != OperandType.BOOLEAN) { args3 = args3.ToBoolean("Function 'RemoveStart' parameter {0} is error!",3); if (args3.IsError) { return args3; } }
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
            AddFunction(stringBuilder, "RemoveStart");
        }
    }

    internal class Function_REMOVEEND : Function_N
    {
        public Function_REMOVEEND(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = funcs[0].Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function 'RemoveEnd' parameter {0} is error!",1); if (args1.IsError) { return args1; } }
            var args2 = funcs[1].Calculate(work); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function 'RemoveEnd' parameter {0} is error!",2); if (args2.IsError) { return args2; } }
            StringComparison comparison = StringComparison.Ordinal;
            if (funcs.Length == 3) {
                var args3 = funcs[2].Calculate(work); if (args3.Type != OperandType.BOOLEAN) { args3 = args3.ToBoolean("Function 'RemoveEnd' parameter {0} is error!",3); if (args3.IsError) { return args3; } }
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
            AddFunction(stringBuilder, "RemoveEnd");
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
            args1 = args1.ToText("Function 'Json' parameter is error!");
            if (args1.IsError) { return args1; }
            var txt = args1.TextValue;
            if ((txt.StartsWith('{') && txt.EndsWith('}')) || (txt.StartsWith('[') && txt.EndsWith(']'))) {
                try {
                    var json = JsonMapper.ToObject(txt);
                    return Operand.Create(json);
                } catch (Exception) { }
            }
            return Operand.Error("Function 'Json' parameter is error!");
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Json");
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
            var args2 = func2.Calculate(work).ToText("Function 'Has' parameter {0} is error!",2); if (args2.IsError) { return args2; }

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
                            if (v.BooleanValue.ToString().Equals(args2.TextValue, StringComparison.CurrentCultureIgnoreCase)) { return Operand.True; }
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
            return Operand.Error("Function 'Has' parameter {0} is error!",1);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Has");
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
            var args2 = func2.Calculate(work).ToText("Function 'HasValue' parameter {0} is error!",2); if (args2.IsError) { return args2; }

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
                            if (v.BooleanValue.ToString().Equals(args2.TextValue, StringComparison.CurrentCultureIgnoreCase)) { return Operand.True; }
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
                            if (v.BooleanValue.ToString().Equals(args2.TextValue, StringComparison.CurrentCultureIgnoreCase)) { return Operand.True; }
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
            return Operand.Error("Function 'HasValue' parameter {0} is error!",1);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "HasValue");
        }
    }


    #endregion csharp
}
