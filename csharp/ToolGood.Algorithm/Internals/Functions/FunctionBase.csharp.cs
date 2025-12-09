using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ToolGood.Algorithm.LitJson;

namespace ToolGood.Algorithm.Internals.Functions
{
    #region csharp

    internal class Function_URLENCODE : Function_1
    {
        public Function_URLENCODE(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter is error!", "UrlEncode"); if (args1.IsError) { return args1; } }
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter is error!", "UrlDecode"); if (args1.IsError) { return args1; } }
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter is error!", "HtmlEncode"); if (args1.IsError) { return args1; } }
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter is error!", "HtmlDecode"); if (args1.IsError) { return args1; } }
            var s = args1.TextValue;
            var r = System.Web.HttpUtility.HtmlDecode(s);
            return Operand.Create(r);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "HtmlDecode");
        }
    }

    internal class Function_BASE64TOTEXT : Function_2
    {
        public Function_BASE64TOTEXT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Base64ToText", 1); if (args1.IsError) return args1; }
            try {
                Encoding encoding;
                if (func2 == null) {
                    encoding = Encoding.UTF8;
                } else {
                    var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "Base64ToText", 2); if (args2.IsError) return args2; }
                    encoding = Encoding.GetEncoding(args2.TextValue);
                }
                var t = encoding.GetString(Base64.FromBase64String(args1.TextValue));
                return Operand.Create(t);
            } catch (Exception) { }
            return Operand.Error("Function 'Base64ToText' is error!");
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Base64ToText");
        }
    }

    internal class Function_BASE64URLTOTEXT : Function_2
    {
        public Function_BASE64URLTOTEXT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Base64urlToText", 1); if (args1.IsError) return args1; }
            try {
                Encoding encoding;
                if (func2 == null) {
                    encoding = Encoding.UTF8;
                } else {
                    var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "Base64urlToText", 2); if (args2.IsError) return args2; }
                    encoding = Encoding.GetEncoding(args2.TextValue);
                }
                var t = encoding.GetString(Base64.FromBase64ForUrlString(args1.TextValue));
                return Operand.Create(t);
            } catch (Exception) { }
            return Operand.Error("Function 'Base64urlToText' is error!");
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Base64urlToText");
        }
    }

    internal class Function_TEXTTOBASE64 : Function_2
    {
        public Function_TEXTTOBASE64(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "TextToBase64", 1); if (args1.IsError) return args1; }
            try {
                Encoding encoding;
                if (func2 == null) {
                    encoding = Encoding.UTF8;
                } else {
                    var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "TextToBase64", 2); if (args2.IsError) return args2; }
                    encoding = Encoding.GetEncoding(args2.TextValue);
                }
                var bytes = encoding.GetBytes(args1.TextValue);
                var t = Base64.ToBase64String(bytes);
                return Operand.Create(t);
            } catch (Exception) { }
            return Operand.Error("Function 'TextToBase64' is error!");
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "TextToBase64");
        }
    }

    internal class Function_TEXTTOBASE64URL : Function_2
    {
        public Function_TEXTTOBASE64URL(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "TextToBase64", 1); if (args1.IsError) return args1; }
            try {
                Encoding encoding;
                if (func2 == null) {
                    encoding = Encoding.UTF8;
                } else {
                    var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "TextToBase64", 2); if (args2.IsError) return args2; }
                    encoding = Encoding.GetEncoding(args2.TextValue);
                }
                var bytes = encoding.GetBytes(args1.TextValue);
                var t = Base64.ToBase64ForUrlString(bytes);
                return Operand.Create(t);
            } catch (Exception) { }
            return Operand.Error("Function 'TextToBase64' is error!");
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work).ToText("Function '{0}' parameter {1} is error!", "Regex", 1);
            if (args1.IsError) { return args1; }
            var args2 = func2.Evaluate(work).ToText("Function '{0}' parameter {1} is error!", "Regex", 2);
            if (args2.IsError) { return args2; }

            var b = Regex.Match(args1.TextValue, args2.TextValue);
            if (b.Success == false) {
                return Operand.Error("Function 'Regex' is error!");
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "RegexReplace", 1); if (args1.IsError) return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "RegexReplace", 2); if (args2.IsError) return args2; }
            var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotText) { args3 = args3.ToText("Function '{0}' parameter {1} is error!", "RegexReplace", 3); if (args3.IsError) return args3; }
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "IsRegex", 1); if (args1.IsError) return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "IsRegex", 2); if (args2.IsError) return args2; }
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            return Operand.Create(System.Guid.NewGuid().ToString());
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("GUID()");
        }
    }

    internal class Function_MD5 : Function_2
    {
        public Function_MD5(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "MD5", 1); if (args1.IsError) return args1; }
            try {
                Encoding encoding;
                if (func2 == null) {
                    encoding = Encoding.UTF8;
                } else {
                    var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "MD5", 2); if (args2.IsError) return args2; }
                    encoding = Encoding.GetEncoding(args2.TextValue);
                }
                var t = Hash.GetMd5String(encoding.GetBytes(args1.TextValue));
                return Operand.Create(t);
            } catch (Exception ex) {
                return Operand.Error("Function 'MD5' is error!" + ex.Message);
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "MD5");
        }
    }

    internal class Function_SHA1 : Function_2
    {
        public Function_SHA1(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "SHA1", 1); if (args1.IsError) return args1; }
            try {
                Encoding encoding;
                if (func2 == null) {
                    encoding = Encoding.UTF8;
                } else {
                    var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "SHA1", 2); if (args2.IsError) return args2; }
                    encoding = Encoding.GetEncoding(args2.TextValue);
                }
                var t = Hash.GetSha1String(encoding.GetBytes(args1.TextValue));
                return Operand.Create(t);
            } catch (Exception ex) {
                return Operand.Error("Function 'SHA1' is error!" + ex.Message);
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "SHA1");
        }
    }

    internal class Function_SHA256 : Function_2
    {
        public Function_SHA256(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "SHA256", 1); if (args1.IsError) return args1; }
            try {
                Encoding encoding;
                if (func2 == null) {
                    encoding = Encoding.UTF8;
                } else {
                    var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "SHA256", 2); if (args2.IsError) return args2; }
                    encoding = Encoding.GetEncoding(args2.TextValue);
                }
                var t = Hash.GetSha256String(encoding.GetBytes(args1.TextValue));
                return Operand.Create(t);
            } catch (Exception ex) {
                return Operand.Error("Function 'SHA256' is error!" + ex.Message);
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "SHA256");
        }
    }

    internal class Function_SHA512 : Function_2
    {
        public Function_SHA512(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "SHA512", 1); if (args1.IsError) return args1; }
            try {
                Encoding encoding;
                if (func2 == null) {
                    encoding = Encoding.UTF8;
                } else {
                    var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "SHA512", 2); if (args2.IsError) return args2; }
                    encoding = Encoding.GetEncoding(args2.TextValue);
                }
                var t = Hash.GetSha512String(encoding.GetBytes(args1.TextValue));
                return Operand.Create(t);
            } catch (Exception ex) {
                return Operand.Error("Function 'SHA512' is error!" + ex.Message);
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "SHA512");
        }
    }

    internal class Function_CRC32 : Function_2
    {
        public Function_CRC32(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "CRC32", 1); if (args1.IsError) return args1; }
            try {
                Encoding encoding;
                if (func2 == null) {
                    encoding = Encoding.UTF8;
                } else {
                    var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "CRC32", 2); if (args2.IsError) return args2; }
                    encoding = Encoding.GetEncoding(args2.TextValue);
                }
                var t = Hash.GetCrc32String(encoding.GetBytes(args1.TextValue));
                return Operand.Create(t);
            } catch (Exception ex) {
                return Operand.Error("Function 'CRC32' is error!" + ex.Message);
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "CRC32");
        }
    }

    internal class Function_HMACMD5 : Function_3
    {
        public Function_HMACMD5(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "HmacMD5", 1); if (args1.IsError) return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "HmacMD5", 2); if (args2.IsError) return args2; }
            try {
                Encoding encoding;
                if (func3 == null) {
                    encoding = Encoding.UTF8;
                } else {
                    var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotText) { args3 = args3.ToText("Function '{0}' parameter {1} is error!", "HmacMD5", 3); if (args3.IsError) return args3; }
                    encoding = Encoding.GetEncoding(args3.TextValue);
                }
                var t = Hash.GetHmacMd5String(encoding.GetBytes(args1.TextValue), args2.TextValue);
                return Operand.Create(t);
            } catch (Exception ex) {
                return Operand.Error("Function 'HmacMD5' is error!" + ex.Message);
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "HmacMD5");
        }
    }

    internal class Function_HMACSHA1 : Function_3
    {
        public Function_HMACSHA1(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "HmacSHA1", 1); if (args1.IsError) return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "HmacSHA1", 2); if (args2.IsError) return args2; }
            try {
                Encoding encoding;
                if (func3 == null) {
                    encoding = Encoding.UTF8;
                } else {
                    var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotText) { args3 = args3.ToText("Function '{0}' parameter {1} is error!", "HmacSHA1", 3); if (args3.IsError) return args3; }
                    encoding = Encoding.GetEncoding(args3.TextValue);
                }
                var t = Hash.GetHmacSha1String(encoding.GetBytes(args1.TextValue), args2.TextValue);
                return Operand.Create(t);
            } catch (Exception ex) {
                return Operand.Error("Function 'HmacSHA1' is error!" + ex.Message);
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "HmacSHA1");
        }
    }

    internal class Function_HMACSHA256 : Function_3
    {
        public Function_HMACSHA256(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "HmacSHA256", 1); if (args1.IsError) return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "HmacSHA256", 2); if (args2.IsError) return args2; }
            try {
                Encoding encoding;
                if (func3 == null) {
                    encoding = Encoding.UTF8;
                } else {
                    var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotText) { args3 = args3.ToText("Function '{0}' parameter {1} is error!", "HmacSHA256", 3); if (args3.IsError) return args3; }
                    encoding = Encoding.GetEncoding(args3.TextValue);
                }
                var t = Hash.GetHmacSha256String(encoding.GetBytes(args1.TextValue), args2.TextValue);
                return Operand.Create(t);
            } catch (Exception ex) {
                return Operand.Error("Function 'HmacSHA256' is error!" + ex.Message);
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "HmacSHA256");
        }
    }

    internal class Function_HMACSHA512 : Function_3
    {
        public Function_HMACSHA512(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "HmacSHA512", 1); if (args1.IsError) return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "HmacSHA512", 2); if (args2.IsError) return args2; }
            try {
                Encoding encoding;
                if (func3 == null) {
                    encoding = Encoding.UTF8;
                } else {
                    var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotText) { args3 = args3.ToText("Function '{0}' parameter {1} is error!", "HmacSHA512", 3); if (args3.IsError) return args3; }
                    encoding = Encoding.GetEncoding(args3.TextValue);
                }
                var t = Hash.GetHmacSha512String(encoding.GetBytes(args1.TextValue), args2.TextValue);
                return Operand.Create(t);
            } catch (Exception ex) {
                return Operand.Error("Function 'HmacSHA512' is error!" + ex.Message);
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "HmacSHA512");
        }
    }

    internal class Function_TRIMSTART : Function_2
    {
        public Function_TRIMSTART(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "TrimStart", 1); if (args1.IsError) return args1; }
            if (func2 == null) {
                return Operand.Create(args1.TextValue.TrimStart());
            }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "TrimStart", 2); if (args2.IsError) return args2; }
            char[] trimChars = args2.TextValue.ToCharArray();
            return Operand.Create(args1.TextValue.TrimStart(trimChars));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "TrimStart");
        }
    }

    internal class Function_TRIMEND : Function_2
    {
        public Function_TRIMEND(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "TrimEnd", 1); if (args1.IsError) return args1; }
            if (func2 == null) {
                return Operand.Create(args1.TextValue.TrimEnd());
            }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "TrimEnd", 2); if (args2.IsError) return args2; }
            char[] trimChars = args2.TextValue.ToCharArray();
            return Operand.Create(args1.TextValue.TrimEnd(trimChars));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "TrimEnd");
        }
    }

    internal class Function_INDEXOF : Function_4
    {
        public Function_INDEXOF(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) : base(func1, func2, func3, func4)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "IndexOf", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "IndexOf", 2); if (args2.IsError) { return args2; } }
            var text = args1.TextValue;
            if (func3 == null) {
                return Operand.Create(text.AsSpan().IndexOf(args2.TextValue) + work.ExcelIndex);
            }
            var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotNumber) { args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "IndexOf", 3); if (args3.IsError) { return args3; } }
            if (func4 == null) {
                return Operand.Create(text.AsSpan(args3.IntValue).IndexOf(args2.TextValue) + args3.IntValue + work.ExcelIndex);
            }
            var args4 = func4.Evaluate(work, tempParameter); if (args4.IsNotNumber) { args4 = args4.ToNumber("Function '{0}' parameter {1} is error!", "IndexOf", 4); if (args4.IsError) { return args4; } }
            return Operand.Create(text.IndexOf(args2.TextValue, args3.IntValue, args4.IntValue) + work.ExcelIndex);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "IndexOf");
        }
    }

    internal class Function_LASTINDEXOF : Function_4
    {
        public Function_LASTINDEXOF(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) : base(func1, func2, func3, func4)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "LastIndexOf", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "LastIndexOf", 2); if (args2.IsError) { return args2; } }
            var text = args1.TextValue;
            if (func3 == null) {
                return Operand.Create(text.AsSpan().LastIndexOf(args2.TextValue) + work.ExcelIndex);
            }
            var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotNumber) { args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "LastIndexOf", 3); if (args3.IsError) { return args3; } }
            if (func4 == null) {
                return Operand.Create(text.AsSpan(0, args3.IntValue).LastIndexOf(args2.TextValue) + work.ExcelIndex);
            }
            var args4 = func4.Evaluate(work, tempParameter); if (args4.IsNotNumber) { args4 = args4.ToNumber("Function '{0}' parameter {1} is error!", "LastIndexOf", 4); if (args4.IsError) { return args4; } }
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Split", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "Split", 2); if (args2.IsError) { return args2; } }
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args = new List<Operand>();
            foreach (var item in funcs) { var aa = item.Evaluate(work, tempParameter); if (aa.IsError) { return aa; } args.Add(aa); }
            var args1 = args[0];
            if (args1.IsJson) {
                var o = args1.ToArray(null);
                if (o.IsError == false) {
                    args1 = o;
                }
            }
            if (args1.IsArray) {
                List<string> list = new List<string>();
                var o = FunctionUtil.F_base_GetList(args1, list);
                if (o == false) return Operand.Error("Function '{0}' parameter {1} is error!", "Join", 1);

                var args2 = args[1].ToText("Function '{0}' parameter {1} is error!", "Join", 2);
                if (args2.IsError) { return args2; }

                return Operand.Create(string.Join(args2.TextValue, list));
            } else {
                args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Join", 1);
                if (args1.IsError) { return args1; }

                List<string> list = new List<string>();
                for (int i = 1; i < args.Count; i++) {
                    var o = FunctionUtil.F_base_GetList(args[i], list);
                    if (o == false) return Operand.Error("Function '{0}' parameter {1} is error!", "Join", i + 1);
                }
                return Operand.Create(string.Join(args1.TextValue, list));
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Join");
        }
    }

    internal class Function_SUBSTRING : Function_3
    {
        public Function_SUBSTRING(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Substring", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Substring", 2); if (args2.IsError) { return args2; } }

            var text = args1.TextValue;
            if (func3 == null) {
                return Operand.Create(text.AsSpan(args2.IntValue - work.ExcelIndex).ToString());
            }
            var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotNumber) { args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "Substring", 3); if (args3.IsError) { return args3; } }
            return Operand.Create(text.AsSpan(args2.IntValue - work.ExcelIndex, args3.IntValue).ToString());
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Substring");
        }
    }

    internal class Function_STARTSWITH : Function_3
    {
        public Function_STARTSWITH(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "StartsWith", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "StartsWith", 2); if (args2.IsError) { return args2; } }

            var text = args1.TextValue;
            if (func3 == null) {
                return Operand.Create(text.AsSpan().StartsWith(args2.TextValue.AsSpan()));
            }
            var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotBoolean) { args3 = args3.ToBoolean("Function '{0}' parameter {1} is error!", "StartsWith", 3); if (args3.IsError) { return args3; } }
            return Operand.Create(text.AsSpan().StartsWith(args2.TextValue.AsSpan(), args3.BooleanValue ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "StartsWith");
        }
    }

    internal class Function_ENDSWITH : Function_3
    {
        public Function_ENDSWITH(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "EndsWith", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "EndsWith", 2); if (args2.IsError) { return args2; } }
            var text = args1.TextValue;
            if (func3 == null) {
                return Operand.Create(text.AsSpan().EndsWith(args2.TextValue.AsSpan()));
            }
            var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotBoolean) { args3 = args3.ToBoolean("Function '{0}' parameter {1} is error!", "EndsWith", 3); if (args3.IsError) { return args3; } }
            return Operand.Create(text.AsSpan().EndsWith(args2.TextValue.AsSpan(), args3.BooleanValue ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "EndsWith");
        }
    }

    internal class Function_REMOVESTART : Function_3
    {
        public Function_REMOVESTART(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "RemoveStart", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "RemoveStart", 2); if (args2.IsError) { return args2; } }
            StringComparison comparison = StringComparison.Ordinal;
            if (func3 != null) {
                var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotBoolean) { args3 = args3.ToBoolean("Function '{0}' parameter {1} is error!", "RemoveStart", 3); if (args3.IsError) { return args3; } }
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

    internal class Function_REMOVEEND : Function_3
    {
        public Function_REMOVEEND(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "RemoveEnd", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "RemoveEnd", 2); if (args2.IsError) { return args2; } }
            StringComparison comparison = StringComparison.Ordinal;
            if (func3 != null) {
                var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotBoolean) { args3 = args3.ToBoolean("Function '{0}' parameter {1} is error!", "RemoveEnd", 3); if (args3.IsError) { return args3; } }
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter);
            if (args1.IsJson) { return args1; }
            args1 = args1.ToText("Function '{0}' parameter is error!", "Json");
            if (args1.IsError) { return args1; }
            var txt = args1.TextValue;
            if ((txt.StartsWith('{') && txt.EndsWith('}')) || (txt.StartsWith('[') && txt.EndsWith(']'))) {
                try {
                    var json = JsonMapper.ToObject(txt);
                    return Operand.Create(json);
                } catch (Exception) { }
            }
            return Operand.Error("Function '{0}' parameter is error!", "Json");
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsError) { return args1; }
            var args2 = func2.Evaluate(work).ToText("Function '{0}' parameter {1} is error!", "Has", 2); if (args2.IsError) { return args2; }

            if (args1.IsArrayJson) {
                return Operand.Create(((OperandKeyValueList)args1).ContainsKey(args2));
            } else if (args1.IsJson) {
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
            } else if (args1.IsArray) {
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
            return Operand.Error("Function '{0}' parameter {1} is error!", "Has", 1);
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsError) { return args1; }
            var args2 = func2.Evaluate(work).ToText("Function '{0}' parameter {1} is error!", "HasValue", 2); if (args2.IsError) { return args2; }

            if (args1.IsArrayJson) {
                return Operand.Create(((OperandKeyValueList)args1).ContainsValue(args2));
            } else if (args1.IsJson) {
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
            } else if (args1.IsArray) {
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
            return Operand.Error("Function '{0}' parameter {1} is error!", "HasValue", 1);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "HasValue");
        }
    }


    #endregion csharp
}
