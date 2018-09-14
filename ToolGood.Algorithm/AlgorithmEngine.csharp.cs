using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ToolGood.Algorithm
{
    partial class AlgorithmEngine
    {
        private void AddCSharp()
        {
            addFunc("UrlDecode", UrlDecode);
            addFunc("UrlEncode", UrlEncode);
            addFunc("HtmlEncode", HtmlEncode);
            addFunc("HtmlDecode", HtmlDecode);
            addFunc("Base64ToText", Base64ToText);
            addFunc("TextToBase64", TextToBase64);
            addFunc("TextToBase64Url", TextToBase64Url);
            addFunc("Regex", Regex);
            addFunc("RegexRepalce", RegexRepalce);
            addFunc("IsRegex", IsRegex);
            addFunc("IsMatch", IsRegex);
            addFunc("Guid", Guid);
            addFunc("Md5", Md5);
            addFunc("Sha1", Sha1);
            addFunc("Sha256", Sha256);
            addFunc("Sha512", Sha512);
            addFunc("Crc8", Crc8);
            addFunc("Crc16", Crc16);
            addFunc("Crc32", Crc32);
            addFunc("HmacMd5", HmacMd5);
            addFunc("HmacSha1", HmacSha1);
            addFunc("HmacSha256", HmacSha256);
            addFunc("HmacSha512", HmacSha512);

            addFunc("TrimStart", TrimStart);
            addFunc("LTrim", TrimStart);

            addFunc("TrimEnd", TrimEnd);
            addFunc("RTrim", TrimEnd);

            addFunc("IndexOf", IndexOf);
            addFunc("LastIndexOf", LastIndexOf);
            addFunc("Split", Split);

            addFunc("Substring", Substring);
            addFunc("StartsWith", StartsWith);
            addFunc("EndsWith", EndsWith);
            addFunc("IsNullOrEmpty", IsNullOrEmpty);
            addFunc("IsNullOrWhiteSpace", IsNullOrWhiteSpace);

            addFunc("ToUpper", UPPER); //将文本转换为大写形式 
            addFunc("ToLower", LOWER); //将文本参数转换为数字 
        }

        private Operand UrlDecode(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("UrlDecode中参数不足", new List<Operand>());

            return new Operand(OperandType.STRING, HttpUtility.UrlDecode(arg[0].StringValue));
        }
        private Operand UrlEncode(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("UrlEncode中参数不足", new List<Operand>());
            return new Operand(OperandType.STRING, HttpUtility.UrlEncode(arg[0].StringValue));
        }
        private Operand HtmlEncode(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("HtmlEncode中参数不足", new List<Operand>());
            return new Operand(OperandType.STRING, HttpUtility.HtmlEncode(arg[0].StringValue));
        }
        private Operand HtmlDecode(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("HtmlDecode中参数不足", new List<Operand>());
            return new Operand(OperandType.STRING, HttpUtility.HtmlDecode(arg[0].StringValue));
        }
        private Operand Base64ToText(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("Base64ToText中参数不足", new List<Operand>());
            var text = arg[0].StringValue;
            var bytes = Base64.FromBase64ForUrlString(text);
            Encoding encoding;
            if (arg.Count == 1) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(arg[1].StringValue);
            }
            var t = encoding.GetString(bytes);
            return new Operand(OperandType.STRING, t);
        }
        private Operand TextToBase64(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("TextToBase64中参数不足", new List<Operand>());
            var text = arg[0].StringValue;
            Encoding encoding;
            if (arg.Count == 2) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(arg[1].StringValue);
            }
            var bytes = encoding.GetBytes(text);
            var t = Base64.ToBase64String(bytes);
            return new Operand(OperandType.STRING, t);
        }
        private Operand TextToBase64Url(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("TextToBase64Url中参数不足", new List<Operand>());
            var text = arg[0].StringValue;
            Encoding encoding;
            if (arg.Count == 2) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(arg[1].StringValue);
            }
            var bytes = encoding.GetBytes(text);
            var t = Base64.ToBase64ForUrlString(bytes);
            return new Operand(OperandType.STRING, t);
        }

        private Operand Regex(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("Regex中参数不足", new List<Operand>());
            if (arg.Count==2) {
                var b = System.Text.RegularExpressions.Regex.Match(arg[0].StringValue, arg[1].StringValue);
                if (b.Success==false) {
                    return throwError("Regex匹配失败", new List<Operand>());
                }
                return new Operand(OperandType.STRING, b.Value);
            } else if (arg.Count == 3) {
                var ms = System.Text.RegularExpressions.Regex.Matches(arg[0].StringValue, arg[1].StringValue);
                if (ms.Count <= arg[2].IntValue) {
                    return throwError("Regex匹配Index长度错误", new List<Operand>());
                }
                return new Operand(OperandType.STRING, ms[arg[2].IntValue].Value);
   
            } else  {
                var ms = System.Text.RegularExpressions.Regex.Matches(arg[0].StringValue, arg[1].StringValue);
                if (ms.Count <= arg[2].IntValue) {
                    return throwError("Regex匹配Index长度错误", new List<Operand>());
                }
                return new Operand(OperandType.STRING, ms[arg[2].IntValue].Groups[arg[3].IntValue].Value);
            }
        }
        private Operand RegexRepalce(List<Operand> arg)
        {
            if (arg.Count < 3) return throwError("RegexRepalce中参数不足", new List<Operand>());
            var b = System.Text.RegularExpressions.Regex.Replace(arg[0].StringValue, arg[1].StringValue, arg[2].StringValue);
            return new Operand(OperandType.STRING, b);
        }
        private Operand IsRegex(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("IsRegex中参数不足", new List<Operand>());
            var b = System.Text.RegularExpressions.Regex.IsMatch(arg[0].StringValue, arg[1].StringValue);
            return new Operand(OperandType.BOOLEAN, b);
        }

        private Operand Guid(List<Operand> arg)
        {
            return new Operand(OperandType.STRING, System.Guid.NewGuid().ToString());
        }
        private Operand Md5(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("Md5中参数不足", new List<Operand>());
            var text = arg[0].StringValue;
            Encoding encoding;
            if (arg.Count == 2) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(arg[1].StringValue);
            }
            var t = Hash.GetMd5String(encoding.GetBytes(text));
            return new Operand(OperandType.STRING, t);
        }
        private Operand Sha1(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("Sha1中参数不足", new List<Operand>());
            var text = arg[0].StringValue;
            Encoding encoding;
            if (arg.Count == 2) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(arg[1].StringValue);
            }
            var t = Hash.GetSha1Bytes(encoding.GetBytes(text));
            return new Operand(OperandType.STRING, t);
        }
        private Operand Sha256(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("Sha256中参数不足", new List<Operand>());
            var text = arg[0].StringValue;
            Encoding encoding;
            if (arg.Count == 2) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(arg[1].StringValue);
            }
            var t = Hash.GetSha256Bytes(encoding.GetBytes(text));
            return new Operand(OperandType.STRING, t);
        }
        private Operand Sha512(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("Sha512中参数不足", new List<Operand>());
            var text = arg[0].StringValue;
            Encoding encoding;
            if (arg.Count == 2) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(arg[1].StringValue);
            }
            var t = Hash.GetSha512String(encoding.GetBytes(text));
            return new Operand(OperandType.STRING, t);
        }
        private Operand Crc8(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("Crc8中参数不足", new List<Operand>());
            var text = arg[0].StringValue;
            Encoding encoding;
            if (arg.Count == 1) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(arg[1].StringValue);
            }
            var t = Hash.GetCrc8String(encoding.GetBytes(text));
            return new Operand(OperandType.STRING, t);
        }
        private Operand Crc16(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("Crc16中参数不足", new List<Operand>());
            var text = arg[0].StringValue;
            Encoding encoding;
            if (arg.Count == 1) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(arg[1].StringValue);
            }
            var t = Hash.GetCrc16String(encoding.GetBytes(text));
            return new Operand(OperandType.STRING, t);
        }
        private Operand Crc32(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("Crc32中参数不足", new List<Operand>());
            var text = arg[0].StringValue;
            Encoding encoding;
            if (arg.Count == 1) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(arg[1].StringValue);
            }
            var t = Hash.GetCrc32String(encoding.GetBytes(text));
            return new Operand(OperandType.STRING, t);
        }

        private Operand HmacMd5(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("HmacMd5中参数不足", new List<Operand>());
            var text = arg[0].StringValue;
            Encoding encoding;
            if (arg.Count == 3) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(arg[2].StringValue);
            }
            var t = Hash.GetHmacMd5String(encoding.GetBytes(text), arg[1].StringValue);
            return new Operand(OperandType.STRING, t);
        }
        private Operand HmacSha1(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("HmacSha1中参数不足", new List<Operand>());
            var text = arg[0].StringValue;
            Encoding encoding;
            if (arg.Count == 3) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(arg[2].StringValue);
            }
            var t = Hash.GetHmacSha1String(encoding.GetBytes(text), arg[1].StringValue);
            return new Operand(OperandType.STRING, t);
        }
        private Operand HmacSha256(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("HmacSha256中参数不足", new List<Operand>());
            var text = arg[0].StringValue;
            Encoding encoding;
            if (arg.Count == 3) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(arg[2].StringValue);
            }
            var t = Hash.GetHmacSha256String(encoding.GetBytes(text), arg[1].StringValue);
            return new Operand(OperandType.STRING, t);
        }
        private Operand HmacSha512(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("HmacSha512中参数不足", new List<Operand>());
            var text = arg[0].StringValue;
            Encoding encoding;
            if (arg.Count == 3) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(arg[2].StringValue);
            }
            var t = Hash.GetHmacSha512String(encoding.GetBytes(text), arg[1].StringValue);
            return new Operand(OperandType.STRING, t);
        }

        private Operand TrimStart(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("TrimStart中参数不足", new List<Operand>());
            var text = arg[0].StringValue;
            return new Operand(OperandType.STRING, text.TrimStart());
        }
        private Operand TrimEnd(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("TrimEnd中参数不足", new List<Operand>());
            var text = arg[0].StringValue;
            
            return new Operand(OperandType.STRING, text.TrimEnd());
        }

        private Operand IndexOf(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("IndexOf中参数不足", new List<Operand>());
            var text = arg[0].StringValue;
            if (arg.Count == 2) {
                return new Operand(OperandType.NUMBER, text.IndexOf(arg[1].StringValue));
            } else if (arg.Count == 3) {
                return new Operand(OperandType.NUMBER, text.IndexOf(arg[1].StringValue, arg[2].IntValue));
            } else {
                return new Operand(OperandType.NUMBER, text.IndexOf(arg[1].StringValue, arg[2].IntValue, arg[3].IntValue));
            }
        }
        private Operand LastIndexOf(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("IndexOf中参数不足", new List<Operand>());
            var text = arg[0].StringValue;
            if (arg.Count == 2) {
                return new Operand(OperandType.NUMBER, text.LastIndexOf(arg[1].StringValue));
            } else if (arg.Count == 3) {
                return new Operand(OperandType.NUMBER, text.LastIndexOf(arg[1].StringValue, arg[2].IntValue));
            } else {
                return new Operand(OperandType.NUMBER, text.LastIndexOf(arg[1].StringValue, arg[2].IntValue, arg[3].IntValue));
            }
        }
        private Operand Split(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("IndexOf中参数不足", new List<Operand>());
            var text = arg[0].StringValue;
                return new Operand(OperandType.ARRARY, text.Split(arg[1].StringValue.ToArray()));
        }

        private Operand Substring(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("Substring中参数不足", new List<Operand>());
            var text = arg[0].StringValue;
            if (arg.Count == 2) {
                return new Operand(OperandType.NUMBER, text.Substring(arg[1].IntValue));
            } else {
                return new Operand(OperandType.NUMBER, text.Substring(arg[1].IntValue, arg[2].IntValue));
            }
        }
        private Operand StartsWith(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("StartsWith中参数不足", new List<Operand>());
            var text = arg[0].StringValue;
            if (arg.Count == 2) {
                return new Operand(OperandType.NUMBER, text.StartsWith(arg[1].StringValue));
            } else {
                return new Operand(OperandType.NUMBER, text.StartsWith(arg[1].StringValue, arg[2].BooleanValue? StringComparison.CurrentCultureIgnoreCase: StringComparison.CurrentCulture));
            }
        }
        private Operand EndsWith(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("EndsWith中参数不足", new List<Operand>());
            var text = arg[0].StringValue;
            if (arg.Count == 2) {
                return new Operand(OperandType.NUMBER, text.EndsWith(arg[1].StringValue));
            } else {
                return new Operand(OperandType.NUMBER, text.EndsWith(arg[1].StringValue, arg[2].BooleanValue ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));
            }
        }
        private Operand IsNullOrEmpty(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("IsNullOrEmpty中参数不足", new List<Operand>());
            return new Operand(OperandType.STRING, string.IsNullOrEmpty(arg[0].StringValue));
        }
        private Operand IsNullOrWhiteSpace(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("IsNullOrWhiteSpace中参数不足", new List<Operand>());
            return new Operand(OperandType.STRING, string.IsNullOrWhiteSpace(arg[0].StringValue));
        }

    }
}
