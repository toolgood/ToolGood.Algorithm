using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ToolGood.Algorithm
{
    partial class AlgorithmEngine
    {
        private void AddStringFunction()
        {
            addFunc("ASC", ASC); //将字符串内的全角（双字节）英文字母或片假名更改为半角（单字节）字符
            addFunc("jis", jis); //将字符串中的半角（单字节）英文字符或片假名更改为全角（双字节）字符。
            addFunc("WIDECHAR", jis); //将字符串中的半角（单字节）英文字符或片假名更改为全角（双字节）字符。



            addFunc("CHAR", CHAR); //返回由代码数字指定的字符
            addFunc("CLEAN", CLEAN); //删除文本中所有打印不出的字符
            addFunc("CODE", CODE); //返回文本字符串中第一个字符的数字代码
            addFunc("CONCATENATE", CONCATENATE); //将若干文本项合并到一个文本项中
            addFunc("EXACT", EXACT); //检查两个文本值是否完全相同
            addFunc("FIND", FIND); //在一文本值内查找另一文本值（区分大小写） 
            addFunc("FIXED", FIXED); //将数字设置为具有固定小数位的文本格式
            addFunc("LEFT", LEFT); //返回文本值最左边的字符
            addFunc("LEN", LEN); //返回文本字符串中的字符个数
            addFunc("LOWER", LOWER); //将文本转换为小写形式
            addFunc("MID", MID); //从文本字符串中的指定位置起返回特定个数的字符
            addFunc("PROPER", PROPER); //将文本值中每一个单词的首字母设置为大写
            addFunc("REPLACE", REPLACE); //替换文本内的字符
            addFunc("REPT", REPT); //按给定次数重复文本
            addFunc("RIGHT", RIGHT); //返回文本值最右边的字符
            addFunc("RMB", RMB); //按 ￥(RMB)货币格式将数字转换为文本
            addFunc("SEARCH", SEARCH); //在一文本值中查找另一文本值（不区分大小写） 
            addFunc("SUBSTITUTE", SUBSTITUTE); //在文本字符串中以新文本替换旧文本 
            addFunc("T", T); //将参数转换为文本 
            addFunc("TEXT", TEXT); //设置数字的格式并将数字转换为文本 
            addFunc("TRIM", TRIM); //删除文本中的空格 
            addFunc("UPPER", UPPER); //将文本转换为大写形式 
            addFunc("VALUE", VALUE); //将文本参数转换为数字 

        }

        private Operand RMB(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("RMB中参数不足", new List<Operand>());
            return new Operand(OperandType.STRING, ToChineseRMB(arg[0].NumberValue));
        }

        private Operand jis(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("jis中参数不足", new List<Operand>());
            return new Operand(OperandType.STRING, ToSBC(arg[0].StringValue));
        }

        private Operand VALUE(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("VALUE中参数不足", new List<Operand>());
            double d;

            if (double.TryParse(arg[0].StringValue, out d)) {
                return new Operand(OperandType.STRING, d);
            }
            return throwError("无法转成数字", arg);
        }

        private Operand UPPER(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("TEXT中参数不足", new List<Operand>());
            return new Operand(OperandType.STRING, arg[0].StringValue.ToUpper());
        }

        private Operand TRIM(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("TEXT中参数不足", new List<Operand>());
            return new Operand(OperandType.STRING, arg[0].StringValue.Trim());
        }

        private Operand TEXT(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("TEXT中参数不足", new List<Operand>());
            var f = arg[1].StringValue;
            var a = arg[0];
            if (a.Type == OperandType.STRING) {
                return a;
            } else if (a.Type == OperandType.BOOLEAN) {
                return new Operand(OperandType.STRING, a.BooleanValue.ToString());
            } else if (a.Type == OperandType.NUMBER) {
                return new Operand(OperandType.STRING, a.NumberValue.ToString(f));
            } else if (a.Type == OperandType.DATE) {
                return new Operand(OperandType.STRING, a.DateValue.ToString(f));
            }
            return new Operand(OperandType.STRING, a.StringValue.ToString());
        }

        private Operand ASC(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("ASC中参数不足", new List<Operand>());
            return new Operand(OperandType.STRING, ToDBC(arg[0].StringValue));
        }

        private Operand T(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("T中参数不足", new List<Operand>());
            if (arg[0].Type== OperandType.NUMBER || arg[0].Type== OperandType.BOOLEAN) {
                return new Operand(OperandType.STRING, "");
            }
            return new Operand(OperandType.STRING, arg[0].StringValue);
        }

        private Operand SUBSTITUTE(List<Operand> arg)
        {
            if (arg.Count < 3) return throwError("SUBSTITUTE中参数不足", new List<Operand>());
            if (arg.Count == 3) {
                return new Operand(OperandType.STRING,
                    arg[0].StringValue.Replace(arg[1].StringValue, arg[2].StringValue)
                    );
            }
            string text = arg[0].StringValue;
            string oldtext = arg[1].StringValue;
            string newtext = arg[2].StringValue;
            int index = arg[3].IntValue;

            int index2 = 0;
            StringBuilder sb = new StringBuilder();
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
            return new Operand(OperandType.STRING, sb.ToString());
        }

        private Operand RIGHT(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("LEFT中参数不足", new List<Operand>());
            var t = arg[0].StringValue;
            if (arg.Count == 1) {
                return new Operand(OperandType.STRING, t[t.Length - 1].ToString());
            }
            return new Operand(OperandType.STRING, t.Substring(t.Length - arg[1].IntValue, arg[1].IntValue));
        }

        private Operand REPT(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("REPT中参数不足", new List<Operand>());
            var newtext = arg[0].StringValue;
            var length = arg[1].IntValue;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++) {
                sb.Append(newtext);
            }
            return new Operand(OperandType.STRING, sb.ToString());
        }

        private Operand REPLACE(List<Operand> arg)
        {
            if (arg.Count==3 && arg[1].Type == OperandType.STRING && arg[2].Type == OperandType.STRING) {
                var srcText = arg[0].StringValue;
                var old= arg[1].StringValue;
                var newstr = arg[2].StringValue;
                return new Operand(OperandType.STRING, srcText.Replace(old, newstr));
            }
            if (arg.Count < 4) return throwError("REPLACE中参数不足或不正确", new List<Operand>());
            var oldtext = arg[0].StringValue;
            var start = arg[1].IntValue - 1;
            var length = arg[2].IntValue;
            var newtext = arg[3].StringValue;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < oldtext.Length; i++) {
                if (i < start) {
                    sb.Append(oldtext[i]);
                } else if (i == start) {
                    sb.Append(newtext);
                } else if (i >= start + length) {
                    sb.Append(oldtext[i]);
                }
            }
            return new Operand(OperandType.STRING, sb.ToString());
        }

        private Operand PROPER(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("PROPER中参数不足", new List<Operand>());
            var text = arg[0].StringValue;
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
            return new Operand(OperandType.STRING, sb.ToString());
        }

        private Operand MID(List<Operand> arg)
        {
            if (arg.Count < 3) return throwError("MID中参数不足", new List<Operand>());
            return new Operand(OperandType.STRING, arg[0].StringValue.Substring(arg[1].IntValue - 1, arg[2].IntValue));
        }

        private Operand LOWER(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("LOWER中参数不足", new List<Operand>());
            return new Operand(OperandType.STRING, arg[0].StringValue.ToLower());
        }

        private Operand LEN(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("LEN中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, (double)arg[0].StringValue.Length);
        }

        private Operand LEFT(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("LEFT中参数不足", new List<Operand>());
            if (arg.Count == 1) {
                return new Operand(OperandType.STRING, arg[0].StringValue[0].ToString());
            }
            return new Operand(OperandType.STRING, arg[0].StringValue.Substring(0, arg[1].IntValue));
        }

        private Operand FIXED(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("FIXED中参数不足", new List<Operand>());
            var num = 2;
            if (arg.Count > 1) num = arg[1].IntValue;
            var s = Math.Round(arg[0].NumberValue, num);//.ToString();
            var no = false;
            if (arg.Count == 3) no = arg[2].BooleanValue;
            if (no == false) {
                return new Operand(OperandType.STRING, s.ToString("N" + num));
            }
            return new Operand(OperandType.STRING, s.ToString());
        }
        private Operand SEARCH(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("FIND中参数不足", new List<Operand>());

            if (arg.Count == 2) {
                var p = arg[1].StringValue.ToLower().IndexOf(arg[0].StringValue.ToLower()) + 1;
                return new Operand(OperandType.NUMBER, (double)p);
            }
            var p2 = arg[1].StringValue.ToLower().IndexOf(arg[0].StringValue.ToLower(), arg[2].IntValue) + 1;
            return new Operand(OperandType.NUMBER, (double)p2);
        }

        private Operand FIND(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("FIND中参数不足", new List<Operand>());

            if (arg.Count == 2) {
                var p = arg[1].StringValue.IndexOf(arg[0].StringValue) + 1;
                return new Operand(OperandType.NUMBER, (double)p);
            }
            var p2 = arg[1].StringValue.IndexOf(arg[0].StringValue, (int)arg[2].NumberValue) + 1;
            return new Operand(OperandType.NUMBER, (double)p2);
        }

        private Operand EXACT(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("EXACT中参数不足", new List<Operand>());

            return new Operand(OperandType.BOOLEAN, arg[0].StringValue == arg[1].StringValue);
        }

        private Operand CONCATENATE(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("CONCATENATE中参数不足", new List<Operand>());
            StringBuilder sb = new StringBuilder();
            foreach (var item in arg) {
                sb.Append(item.StringValue);
            }
            return new Operand(OperandType.STRING, sb.ToString());
        }

        private Operand CODE(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("CODE中参数不足", new List<Operand>());
            var t = arg[0].StringValue;
            if (t.Length == 0) {
                return throwError("字符串长度为0", arg);
            }
            return new Operand(OperandType.NUMBER, (double)(int)(char)t[0]);
        }

        private Operand CLEAN(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("CLEAN中参数不足", new List<Operand>());
            var t = arg[0].StringValue;
            t =System.Text.RegularExpressions.Regex.Replace(t, @"[\f\n\r\t\v]", "");
            return new Operand(OperandType.STRING, t);
        }

        private Operand CHAR(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("SUM中参数不足", new List<Operand>());
            char c = (char)(int)arg[0].NumberValue;
            return new Operand(OperandType.STRING, c.ToString());
        }





    }
}
