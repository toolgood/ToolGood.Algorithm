using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using ToolGood.Algorithm.LitJson;

namespace ToolGood.Algorithm
{
    partial class AlgorithmEngine
    {
        private void AddCSharp()
        {
            addFunc("UrlDecode", F_UrlDecode);
            addFunc("UrlEncode", F_UrlEncode);
            addFunc("HtmlEncode", F_HtmlEncode);
            addFunc("HtmlDecode", F_HtmlDecode);
            addFunc("Base64ToText", F_Base64ToText);
            addFunc("Base64UrlToText", F_Base64UrlToText);
            addFunc("TextToBase64", F_TextToBase64);
            addFunc("TextToBase64Url", F_TextToBase64Url);
            addFunc("Regex", F_Regex);
            addFunc("RegexRepalce", F_RegexRepalce);
            addFunc("IsRegex", F_IsRegex);
            addFunc("IsMatch", F_IsRegex);
            addFunc("Guid", F_Guid);
            addFunc("Md5", F_Md5);
            addFunc("Sha1", F_Sha1);
            addFunc("Sha256", F_Sha256);
            addFunc("Sha512", F_Sha512);
            addFunc("Crc8", F_Crc8);
            addFunc("Crc16", F_Crc16);
            addFunc("Crc32", F_Crc32);
            addFunc("HmacMd5", F_HmacMd5);
            addFunc("HmacSha1", F_HmacSha1);
            addFunc("HmacSha256", F_HmacSha256);
            addFunc("HmacSha512", F_HmacSha512);

            addFunc("TrimStart", F_TrimStart);
            addFunc("LTrim", F_TrimStart);

            addFunc("TrimEnd", F_TrimEnd);
            addFunc("RTrim", F_TrimEnd);

            addFunc("IndexOf", F_IndexOf);
            addFunc("LastIndexOf", F_LastIndexOf);
            addFunc("Split", F_Split);
            addFunc("Join", F_Join);



            addFunc("Substring", F_Substring);
            addFunc("StartsWith", F_StartsWith);
            addFunc("EndsWith", F_EndsWith);
            addFunc("IsNullOrEmpty", F_IsNullOrEmpty);
            addFunc("IsNullOrWhiteSpace", F_IsNullOrWhiteSpace);

            addFunc("RemoveStart", F_RemoveStart);
            addFunc("RemoveEnd", F_RemoveEnd);
            addFunc("RemoveBoth", F_RemoveBoth);


            addFunc("ToUpper", F_Upper); //将文本转换为大写形式 
            addFunc("ToLower", F_Lower); //将文本参数转换为数字 

            addFunc("P", F_P);
            addFunc("param", F_P);
            addFunc("json", F_Json);
            addFunc("Tryjson", F_TryJson);
        }
        private Operand F_TryJson(List<Operand> arg)
        {
            CheckArgsCount("TryJson", arg, new OperandType[][] { new OperandType[] { OperandType.STRING } });
            var match = Regex.Match(arg[0].StringValue, @"\{.*\}|\[.*\]");
            if (match.Success) {
                var json = JsonMapper.ToObject(match.Value);
                return new Operand(OperandType.JSON, json);
            }
            return new Operand(OperandType.ERROR, "无法转Json格式！");
        }

        private Operand F_Json(List<Operand> arg)
        {
            CheckArgsCount("Json", arg, new OperandType[][] { new OperandType[] { OperandType.STRING } });
            var json = JsonMapper.ToObject(arg[0].StringValue);
            return new Operand(OperandType.JSON, json);
        }


        private Operand F_P(List<Operand> arg)
        {
            CheckArgsCount("P", arg, new OperandType[][] { new OperandType[] { OperandType.STRING } });
            if (arg[0].StringValue.StartsWith("[")) {
                return GetParameter(new Operand(OperandType.ANY, arg[0].StringValue));
            }
            return GetParameter(new Operand(OperandType.ANY, "[" + arg[0].StringValue + "]"));
        }

        private Operand F_UrlDecode(List<Operand> arg)
        {
            CheckArgsCount("UrlDecode", arg, new OperandType[][] { new OperandType[] { OperandType.STRING } });

            return new Operand(OperandType.STRING, HttpUtility.UrlDecode(arg[0].StringValue));
        }
        private Operand F_UrlEncode(List<Operand> arg)
        {
            CheckArgsCount("UrlEncode", arg, new OperandType[][] { new OperandType[] { OperandType.STRING } });

            return new Operand(OperandType.STRING, HttpUtility.UrlEncode(arg[0].StringValue));
        }
        private Operand F_HtmlEncode(List<Operand> arg)
        {
            CheckArgsCount("HtmlEncode", arg, new OperandType[][] { new OperandType[] { OperandType.STRING } });
            return new Operand(OperandType.STRING, HttpUtility.HtmlEncode(arg[0].StringValue));
        }
        private Operand F_HtmlDecode(List<Operand> arg)
        {
            CheckArgsCount("HtmlDecode", arg, new OperandType[][] { new OperandType[] { OperandType.STRING } });

            return new Operand(OperandType.STRING, HttpUtility.HtmlDecode(arg[0].StringValue));
        }
        private Operand F_Base64ToText(List<Operand> arg)
        {
            CheckArgsCount("Base64ToText", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING },
                new OperandType[] { OperandType.STRING, OperandType.STRING },
                });

            var text = arg[0].StringValue;
            var bytes = Base64.FromBase64String(text);
            Encoding encoding;
            if (arg.Count == 1) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(arg[1].StringValue);
            }
            var t = encoding.GetString(bytes);
            return new Operand(OperandType.STRING, t);
        }
        private Operand F_Base64UrlToText(List<Operand> arg)
        {
            CheckArgsCount("Base64UrlToText", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING },
                new OperandType[] { OperandType.STRING, OperandType.STRING },
                });

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

        private Operand F_TextToBase64(List<Operand> arg)
        {
            CheckArgsCount("TextToBase64", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING },
                new OperandType[] { OperandType.STRING, OperandType.STRING },
                });

            var text = arg[0].StringValue;
            Encoding encoding;
            if (arg.Count == 1) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(arg[1].StringValue);
            }
            var bytes = encoding.GetBytes(text);
            var t = Base64.ToBase64String(bytes);
            return new Operand(OperandType.STRING, t);
        }
        private Operand F_TextToBase64Url(List<Operand> arg)
        {
            CheckArgsCount("TextToBase64Url", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING },
                new OperandType[] { OperandType.STRING, OperandType.STRING },
                });

            var text = arg[0].StringValue;
            Encoding encoding;
            if (arg.Count == 1) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(arg[1].StringValue);
            }
            var bytes = encoding.GetBytes(text);
            var t = Base64.ToBase64ForUrlString(bytes);
            return new Operand(OperandType.STRING, t);
        }

        private Operand F_Regex(List<Operand> arg)
        {
            CheckArgsCount("Regex", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING, OperandType.STRING },
                new OperandType[] { OperandType.STRING, OperandType.STRING, OperandType.NUMBER },
                new OperandType[] { OperandType.STRING, OperandType.STRING,OperandType.NUMBER,OperandType.NUMBER },
                });

            if (arg.Count == 2) {
                var b = System.Text.RegularExpressions.Regex.Match(arg[0].StringValue, arg[1].StringValue);
                if (b.Success == false) {
                    return ThrowError("Regex匹配失败", new List<Operand>());
                }
                return new Operand(OperandType.STRING, b.Value);
            } else if (arg.Count == 3) {
                var ms = System.Text.RegularExpressions.Regex.Matches(arg[0].StringValue, arg[1].StringValue);
                if (ms.Count <= arg[2].IntValue - excelIndex) {
                    return ThrowError("Regex匹配Index长度错误", new List<Operand>());
                }
                return new Operand(OperandType.STRING, ms[arg[2].IntValue - excelIndex].Value);

            } else {
                var ms = System.Text.RegularExpressions.Regex.Matches(arg[0].StringValue, arg[1].StringValue);
                if (ms.Count <= arg[2].IntValue + excelIndex) {
                    return ThrowError("Regex匹配Index长度错误", new List<Operand>());
                }
                return new Operand(OperandType.STRING, ms[arg[2].IntValue - excelIndex].Groups[arg[3].IntValue].Value);
            }
        }
        private Operand F_RegexRepalce(List<Operand> arg)
        {
            CheckArgsCount("RegexRepalce", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING, OperandType.STRING, OperandType.STRING },
                });

            if (arg.Count < 3) return ThrowError("RegexRepalce 中参数不足", new List<Operand>());
            var b = System.Text.RegularExpressions.Regex.Replace(arg[0].StringValue, arg[1].StringValue, arg[2].StringValue);
            return new Operand(OperandType.STRING, b);
        }
        private Operand F_IsRegex(List<Operand> arg)
        {
            CheckArgsCount("IsRegex", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING, OperandType.STRING },
                });

            var b = System.Text.RegularExpressions.Regex.IsMatch(arg[0].StringValue, arg[1].StringValue);
            return new Operand(OperandType.BOOLEAN, b);
        }

        private Operand F_Guid(List<Operand> arg)
        {
            return new Operand(OperandType.STRING, System.Guid.NewGuid().ToString());
        }
        private Operand F_Md5(List<Operand> arg)
        {
            CheckArgsCount("Md5", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING },
                new OperandType[] { OperandType.STRING, OperandType.STRING },
                });

            var text = arg[0].StringValue;
            Encoding encoding;
            if (arg.Count == 1) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(arg[1].StringValue);
            }
            var t = Hash.GetMd5String(encoding.GetBytes(text));
            return new Operand(OperandType.STRING, t);
        }
        private Operand F_Sha1(List<Operand> arg)
        {
            CheckArgsCount("Sha1", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING },
                new OperandType[] { OperandType.STRING, OperandType.STRING },
                });
            var text = arg[0].StringValue;
            Encoding encoding;
            if (arg.Count == 1) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(arg[1].StringValue);
            }
            var t = Hash.GetSha1Bytes(encoding.GetBytes(text));
            return new Operand(OperandType.STRING, t);
        }
        private Operand F_Sha256(List<Operand> arg)
        {
            CheckArgsCount("Sha256", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING },
                new OperandType[] { OperandType.STRING, OperandType.STRING },
                });

            var text = arg[0].StringValue;
            Encoding encoding;
            if (arg.Count == 1) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(arg[1].StringValue);
            }
            var t = Hash.GetSha256Bytes(encoding.GetBytes(text));
            return new Operand(OperandType.STRING, t);
        }
        private Operand F_Sha512(List<Operand> arg)
        {
            CheckArgsCount("Sha512", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING },
                new OperandType[] { OperandType.STRING, OperandType.STRING },
                });
            var text = arg[0].StringValue;
            Encoding encoding;
            if (arg.Count == 1) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(arg[1].StringValue);
            }
            var t = Hash.GetSha512String(encoding.GetBytes(text));
            return new Operand(OperandType.STRING, t);
        }
        private Operand F_Crc8(List<Operand> arg)
        {
            CheckArgsCount("Crc8", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING },
                new OperandType[] { OperandType.STRING, OperandType.STRING },
                });

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
        private Operand F_Crc16(List<Operand> arg)
        {
            CheckArgsCount("Crc16", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING },
                new OperandType[] { OperandType.STRING, OperandType.STRING },
                });

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
        private Operand F_Crc32(List<Operand> arg)
        {
            CheckArgsCount("Crc32", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING },
                new OperandType[] { OperandType.STRING, OperandType.STRING },
                });

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

        private Operand F_HmacMd5(List<Operand> arg)
        {
            CheckArgsCount("HmacMd5", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING },
                new OperandType[] { OperandType.STRING, OperandType.STRING },
                });

            var text = arg[0].StringValue;
            Encoding encoding;
            if (arg.Count == 2) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(arg[2].StringValue);
            }
            var t = Hash.GetHmacMd5String(encoding.GetBytes(text), arg[1].StringValue);
            return new Operand(OperandType.STRING, t);
        }
        private Operand F_HmacSha1(List<Operand> arg)
        {
            CheckArgsCount("HmacSha1", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING },
                new OperandType[] { OperandType.STRING, OperandType.STRING },
                });

            var text = arg[0].StringValue;
            Encoding encoding;
            if (arg.Count == 2) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(arg[2].StringValue);
            }
            var t = Hash.GetHmacSha1String(encoding.GetBytes(text), arg[1].StringValue);
            return new Operand(OperandType.STRING, t);
        }
        private Operand F_HmacSha256(List<Operand> arg)
        {
            CheckArgsCount("HmacSha256", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING },
                new OperandType[] { OperandType.STRING, OperandType.STRING },
                });

            var text = arg[0].StringValue;
            Encoding encoding;
            if (arg.Count == 2) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(arg[2].StringValue);
            }
            var t = Hash.GetHmacSha256String(encoding.GetBytes(text), arg[1].StringValue);
            return new Operand(OperandType.STRING, t);
        }
        private Operand F_HmacSha512(List<Operand> arg)
        {
            CheckArgsCount("HmacSha512", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING },
                new OperandType[] { OperandType.STRING, OperandType.STRING },
                });

            var text = arg[0].StringValue;
            Encoding encoding;
            if (arg.Count == 2) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(arg[2].StringValue);
            }
            var t = Hash.GetHmacSha512String(encoding.GetBytes(text), arg[1].StringValue);
            return new Operand(OperandType.STRING, t);
        }

        private Operand F_TrimStart(List<Operand> arg)
        {
            CheckArgsCount("TrimStart", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING },
                new OperandType[] { OperandType.STRING, OperandType.STRING },
                });

            var text = arg[0].StringValue;
            if (arg.Count == 2) {
                return new Operand(OperandType.STRING, text.TrimStart(arg[1].StringValue.ToArray()));
            }
            return new Operand(OperandType.STRING, text.TrimStart());
        }
        private Operand F_TrimEnd(List<Operand> arg)
        {
            CheckArgsCount("TrimEnd", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING },
                new OperandType[] { OperandType.STRING, OperandType.STRING },
                });
            var text = arg[0].StringValue;
            if (arg.Count == 2) {
                return new Operand(OperandType.STRING, text.TrimEnd(arg[1].StringValue.ToArray()));
            }
            return new Operand(OperandType.STRING, text.TrimEnd());
        }

        private Operand F_IndexOf(List<Operand> arg)
        {
            CheckArgsCount("IndexOf", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING, OperandType.STRING },
                new OperandType[] { OperandType.STRING, OperandType.STRING, OperandType.NUMBER },
                new OperandType[] { OperandType.STRING, OperandType.STRING, OperandType.NUMBER, OperandType.NUMBER },
                });

            if (arg.Count < 2) return ThrowError("IndexOf 中参数不足", new List<Operand>());
            var text = arg[0].StringValue;
            if (arg.Count == 2) {
                return new Operand(OperandType.NUMBER, text.IndexOf(arg[1].StringValue) + excelIndex);
            } else if (arg.Count == 3) {
                return new Operand(OperandType.NUMBER, text.IndexOf(arg[1].StringValue, arg[2].IntValue) + excelIndex);
            } else {
                return new Operand(OperandType.NUMBER, text.IndexOf(arg[1].StringValue, arg[2].IntValue, arg[3].IntValue) + excelIndex);
            }
        }
        private Operand F_LastIndexOf(List<Operand> arg)
        {
            CheckArgsCount("LastIndexOf", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING, OperandType.STRING },
                new OperandType[] { OperandType.STRING, OperandType.STRING, OperandType.NUMBER },
                new OperandType[] { OperandType.STRING, OperandType.STRING, OperandType.NUMBER, OperandType.NUMBER },
                });

            var text = arg[0].StringValue;
            if (arg.Count == 2) {
                return new Operand(OperandType.NUMBER, text.LastIndexOf(arg[1].StringValue) + excelIndex);
            } else if (arg.Count == 3) {
                return new Operand(OperandType.NUMBER, text.LastIndexOf(arg[1].StringValue, arg[2].IntValue) + excelIndex);
            } else {
                return new Operand(OperandType.NUMBER, text.LastIndexOf(arg[1].StringValue, arg[2].IntValue, arg[3].IntValue) + excelIndex);
            }
        }
        private Operand F_Split(List<Operand> arg)
        {
            CheckArgsCount("Split", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING, OperandType.STRING },
                new OperandType[] { OperandType.STRING, OperandType.STRING, OperandType.NUMBER },
                });

            var text = arg[0].StringValue;
            if (arg.Count == 3) {
                return new Operand(OperandType.STRING, text.Split(arg[1].StringValue.ToArray())[arg[2].IntValue - excelIndex]);
            }
            return new Operand(OperandType.ARRARY, text.Split(arg[1].StringValue.ToArray()));
        }

        private Operand F_Join(List<Operand> arg)
        {
            if (arg.Count < 2) return ThrowError("Join 参数不足", new List<Operand>());
            var text = arg[0].StringValue;

            List<string> list = new List<string>();
            for (int i = 1; i < arg.Count; i++) {
                if (arg[i].Type == OperandType.ARRARY) {
                    var ls = arg[i].Value as IList;
                    foreach (var item in ls) {
                        list.Add(item?.ToString());
                    }
                } else {
                    list.Add(arg[i].StringValue);
                }
            }

            return new Operand(OperandType.STRING, string.Join(text, list));
        }


        private Operand F_Substring(List<Operand> arg)
        {
            CheckArgsCount("Substring", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING, OperandType.NUMBER },
                new OperandType[] { OperandType.STRING, OperandType.NUMBER, OperandType.NUMBER },
                });
            var text = arg[0].StringValue;
            if (arg.Count == 2) {
                return new Operand(OperandType.STRING, text.Substring(arg[1].IntValue - excelIndex));
            } else {
                return new Operand(OperandType.STRING, text.Substring(arg[1].IntValue - excelIndex, arg[2].IntValue));
            }
        }
        private Operand F_StartsWith(List<Operand> arg)
        {
            CheckArgsCount("StartsWith", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING, OperandType.STRING },
                new OperandType[] { OperandType.STRING, OperandType.STRING, OperandType.BOOLEAN },
                });
            var text = arg[0].StringValue;
            if (arg.Count == 2) {
                return new Operand(OperandType.BOOLEAN, text.StartsWith(arg[1].StringValue));
            } else {
                return new Operand(OperandType.BOOLEAN, text.StartsWith(arg[1].StringValue, arg[2].BooleanValue ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));
            }
        }
        private Operand F_EndsWith(List<Operand> arg)
        {
            CheckArgsCount("EndsWith", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING, OperandType.STRING },
                new OperandType[] { OperandType.STRING, OperandType.STRING, OperandType.BOOLEAN },
                });
            var text = arg[0].StringValue;
            if (arg.Count == 2) {
                return new Operand(OperandType.BOOLEAN, text.EndsWith(arg[1].StringValue));
            } else {
                return new Operand(OperandType.BOOLEAN, text.EndsWith(arg[1].StringValue, arg[2].BooleanValue ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));
            }
        }
        private Operand F_IsNullOrEmpty(List<Operand> arg)
        {
            CheckArgsCount("IsNullOrEmpty", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING },
                });
            return new Operand(OperandType.STRING, string.IsNullOrEmpty(arg[0].StringValue));
        }
        private Operand F_IsNullOrWhiteSpace(List<Operand> arg)
        {
            CheckArgsCount("IsNullOrWhiteSpace", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING },
                });
            return new Operand(OperandType.STRING, string.IsNullOrWhiteSpace(arg[0].StringValue));
        }

        private Operand F_RemoveStart(List<Operand> arg)
        {
            CheckArgsCount("RemoveStart", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING, OperandType.STRING },
                 });
            var text = arg[0].StringValue;
            if (text.StartsWith(arg[1].StringValue)) {
                text = text.Substring(arg[1].StringValue.Length);
            }
            return new Operand(OperandType.STRING, text);
        }
        private Operand F_RemoveEnd(List<Operand> arg)
        {
            CheckArgsCount("RemoveEnd", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING, OperandType.STRING },
                 });
            var text = arg[0].StringValue;
            if (text.EndsWith(arg[1].StringValue)) {
                text = text.Substring(0, text.Length - arg[1].StringValue.Length);
            }
            return new Operand(OperandType.STRING, text);
        }
        private Operand F_RemoveBoth(List<Operand> arg)
        {
            CheckArgsCount("RemoveBoth", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING, OperandType.STRING, OperandType.STRING  },
                new OperandType[] { OperandType.STRING, OperandType.STRING, OperandType.STRING, OperandType.BOOLEAN  },
                 });
            var text = arg[0].StringValue;
            if (arg.Count == 4) {
                if (arg[3].BooleanValue) {
                    if (text.StartsWith(arg[1].StringValue) && text.EndsWith(arg[2].StringValue)) {
                        text = text.Substring(arg[1].StringValue.Length);
                        text = text.Substring(0, text.Length - arg[2].StringValue.Length);
                    }
                    return new Operand(OperandType.STRING, text);
                }
            }
            if (text.StartsWith(arg[1].StringValue)) {
                text = text.Substring(arg[1].StringValue.Length);
            }
            if (text.EndsWith(arg[2].StringValue)) {
                text = text.Substring(0, text.Length - arg[2].StringValue.Length);
            }
            return new Operand(OperandType.STRING, text);
        }

    }
}
