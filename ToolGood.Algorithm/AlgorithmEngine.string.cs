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
            addFunc("ASC", Func_Asc); //将字符串内的全角（双字节）英文字母或片假名更改为半角（单字节）字符
            addFunc("jis", Func_Jis); //将字符串中的半角（单字节）英文字符或片假名更改为全角（双字节）字符。
            addFunc("WIDECHAR", Func_Jis); //将字符串中的半角（单字节）英文字符或片假名更改为全角（双字节）字符。



            addFunc("CHAR", Func_Char); //返回由代码数字指定的字符
            addFunc("CLEAN", Func_Clean); //删除文本中所有打印不出的字符
            addFunc("CODE", Func_Code); //返回文本字符串中第一个字符的数字代码
            addFunc("CONCATENATE", Func_Concatenate); //将若干文本项合并到一个文本项中
            addFunc("EXACT", Func_Exact); //检查两个文本值是否完全相同
            addFunc("FIND", Func_Find); //在一文本值内查找另一文本值（区分大小写） 
            addFunc("FIXED", Func_Fixed); //将数字设置为具有固定小数位的文本格式
            addFunc("LEFT", Func_Left); //返回文本值最左边的字符
            addFunc("LEN", Func_Len); //返回文本字符串中的字符个数
            addFunc("LOWER", Func_Lower); //将文本转换为小写形式
            addFunc("MID", Func_Mid); //从文本字符串中的指定位置起返回特定个数的字符
            addFunc("PROPER", Func_Proper); //将文本值中每一个单词的首字母设置为大写
            addFunc("REPLACE", Func_Replace); //替换文本内的字符
            addFunc("REPT", Func_Rept); //按给定次数重复文本
            addFunc("RIGHT", Func_Right); //返回文本值最右边的字符
            addFunc("RMB", Func_Rmb); //按 ￥(RMB)货币格式将数字转换为文本
            addFunc("SEARCH", Func_Search); //在一文本值中查找另一文本值（不区分大小写） 
            addFunc("SUBSTITUTE", Func_Substitute); //在文本字符串中以新文本替换旧文本 
            addFunc("T", Func_T); //将参数转换为文本 
            addFunc("TEXT", Func_Text); //设置数字的格式并将数字转换为文本 
            addFunc("TRIM", Func_Trim); //删除文本中的空格 
            addFunc("UPPER", Func_Upper); //将文本转换为大写形式 
            addFunc("VALUE", Func_Value); //将文本参数转换为数字 

        }

        private Operand Func_Rmb(List<Operand> arg)
        {
            CheckArgsCount("RMB", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER },
                 });

            return new Operand(OperandType.STRING, Func_base_ToChineseRMB(arg[0].NumberValue));
        }

        private Operand Func_Jis(List<Operand> arg)
        {
            CheckArgsCount("jis", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING },
                 });

            return new Operand(OperandType.STRING, Func_base_ToSBC(arg[0].StringValue));
        }

        private Operand Func_Value(List<Operand> arg)
        {
            CheckArgsCount("VALUE", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING },
                 });

            if (double.TryParse(arg[0].StringValue, out double d)) {
                return new Operand(OperandType.STRING, d);
            }
            return ThrowError("无法转成数字", arg);
        }

        private Operand Func_Upper(List<Operand> arg)
        {
            CheckArgsCount("TEXT", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING },
                 });

            return new Operand(OperandType.STRING, arg[0].StringValue.ToUpper());
        }

        private Operand Func_Trim(List<Operand> arg)
        {
            CheckArgsCount("Trim", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING },
                 });

            return new Operand(OperandType.STRING, arg[0].StringValue.Trim());
        }

        private Operand Func_Text(List<Operand> arg)
        {
            CheckArgsCount("Text", arg, new OperandType[][] {
                new OperandType[] { OperandType.ANY,OperandType.STRING },
                 });

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

        private Operand Func_Asc(List<Operand> arg)
        {
            CheckArgsCount("ASC", arg, new OperandType[][] {
                new OperandType[] {  OperandType.STRING },
                 });

             return new Operand(OperandType.STRING, Func_base_ToDBC(arg[0].StringValue));
        }

        private Operand Func_T(List<Operand> arg)
        {
            CheckArgsCount("ASC", arg, new OperandType[][] {
                new OperandType[] {  OperandType.ANY },
                 });

            if (arg[0].Type == OperandType.NUMBER || arg[0].Type == OperandType.BOOLEAN) {
                return new Operand(OperandType.STRING, "");
            }
            return new Operand(OperandType.STRING, arg[0].StringValue);
        }

        private Operand Func_Substitute(List<Operand> arg)
        {
            CheckArgsCount("SUBSTITUTE", arg, new OperandType[][] {
                new OperandType[] {  OperandType.STRING,OperandType.STRING,OperandType.STRING },
                new OperandType[] {  OperandType.STRING,OperandType.STRING,OperandType.STRING, OperandType.STRING },
                 });

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

        private Operand Func_Right(List<Operand> arg)
        {
            CheckArgsCount("Right", arg, new OperandType[][] {
                new OperandType[] {  OperandType.STRING },
                new OperandType[] {  OperandType.STRING, OperandType.NUMBER },
                 });

            var t = arg[0].StringValue;
            if (arg.Count == 1) {
                return new Operand(OperandType.STRING, t[t.Length - 1].ToString());
            }
            return new Operand(OperandType.STRING, t.Substring(t.Length - arg[1].IntValue, arg[1].IntValue));
        }

        private Operand Func_Rept(List<Operand> arg)
        {
            CheckArgsCount("REPT", arg, new OperandType[][] {
                new OperandType[] {  OperandType.STRING, OperandType.NUMBER },
                 });

            var newtext = arg[0].StringValue;
            var length = arg[1].IntValue;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++) {
                sb.Append(newtext);
            }
            return new Operand(OperandType.STRING, sb.ToString());
        }

        private Operand Func_Replace(List<Operand> arg)
        {
            CheckArgsCount("REPLACE", arg, new OperandType[][] {
                new OperandType[] {  OperandType.STRING, OperandType.STRING, OperandType.STRING },
                new OperandType[] {  OperandType.STRING, OperandType.NUMBER, OperandType.NUMBER, OperandType.STRING },
                 });

            if (arg.Count == 3  ) {
                var srcText = arg[0].StringValue;
                var old = arg[1].StringValue;
                var newstr = arg[2].StringValue;
                return new Operand(OperandType.STRING, srcText.Replace(old, newstr));
            }
            var oldtext = arg[0].StringValue;
            var start = arg[1].IntValue - excelIndex;
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

        private Operand Func_Proper(List<Operand> arg)
        {
            CheckArgsCount("PROPER", arg, new OperandType[][] {
                new OperandType[] {  OperandType.STRING  },
                 });

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

        private Operand Func_Mid(List<Operand> arg)
        {
            CheckArgsCount("MID", arg, new OperandType[][] {
                new OperandType[] {  OperandType.STRING , OperandType.NUMBER, OperandType.NUMBER },
                 });
            return new Operand(OperandType.STRING, arg[0].StringValue.Substring(arg[1].IntValue - excelIndex, arg[2].IntValue));
        }

        private Operand Func_Lower(List<Operand> arg)
        {
            CheckArgsCount("LOWER", arg, new OperandType[][] {
                new OperandType[] {  OperandType.STRING  },
                 });

            return new Operand(OperandType.STRING, arg[0].StringValue.ToLower());
        }

        private Operand Func_Len(List<Operand> arg)
        {
            CheckArgsCount("LEN", arg, new OperandType[][] {
                new OperandType[] {  OperandType.STRING  },
                 });

            return new Operand(OperandType.NUMBER, (double)arg[0].StringValue.Length);
        }

        private Operand Func_Left(List<Operand> arg)
        {
            CheckArgsCount("LEFT", arg, new OperandType[][] {
                new OperandType[] {  OperandType.STRING  },
                new OperandType[] {  OperandType.STRING, OperandType.NUMBER  },
                 });

            if (arg.Count == 1) {
                return new Operand(OperandType.STRING, arg[0].StringValue[0].ToString());
            }
            return new Operand(OperandType.STRING, arg[0].StringValue.Substring(0, arg[1].IntValue));
        }

        private Operand Func_Fixed(List<Operand> arg)
        {
            CheckArgsCount("Fixed", arg, new OperandType[][] {
                new OperandType[] {  OperandType.NUMBER },
                new OperandType[] {  OperandType.NUMBER, OperandType.NUMBER  },
                new OperandType[] {  OperandType.NUMBER, OperandType.NUMBER, OperandType.BOOLEAN  },
                 });

            if (arg.Count < 1) return ThrowError("FIXED 中参数不足", new List<Operand>());
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

        private Operand Func_Search(List<Operand> arg)
        {
            CheckArgsCount("Search", arg, new OperandType[][] {
                new OperandType[] {  OperandType.STRING, OperandType.STRING  },
                new OperandType[] {  OperandType.STRING, OperandType.STRING, OperandType.BOOLEAN  },
                 });

 
            if (arg.Count == 2) {
                var p = arg[1].StringValue.ToLower().IndexOf(arg[0].StringValue.ToLower()) + excelIndex;
                return new Operand(OperandType.NUMBER, (double)p);
            }
            var p2 = arg[1].StringValue.ToLower().IndexOf(arg[0].StringValue.ToLower(), arg[2].IntValue) + excelIndex;
            return new Operand(OperandType.NUMBER, (double)p2);
        }

        private Operand Func_Find(List<Operand> arg)
        {
            CheckArgsCount("FIND", arg, new OperandType[][] {
                new OperandType[] {  OperandType.STRING, OperandType.STRING  },
                new OperandType[] {  OperandType.STRING, OperandType.STRING, OperandType.BOOLEAN  },
                 });

            if (arg.Count < 2) return ThrowError("FIND 中参数不足", new List<Operand>());

            if (arg.Count == 2) {
                var p = arg[1].StringValue.IndexOf(arg[0].StringValue) + excelIndex;
                return new Operand(OperandType.NUMBER, (double)p);
            }
            var p2 = arg[1].StringValue.IndexOf(arg[0].StringValue, (int)arg[2].NumberValue) + excelIndex;
            return new Operand(OperandType.NUMBER, (double)p2);
        }

        private Operand Func_Exact(List<Operand> arg)
        {
            CheckArgsCount("EXACT", arg, new OperandType[][] {
                new OperandType[] {  OperandType.STRING, OperandType.STRING  },
                 });

            return new Operand(OperandType.BOOLEAN, arg[0].StringValue == arg[1].StringValue);
        }

        private Operand Func_Concatenate(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("CONCATENATE 中参数不足", new List<Operand>());
            StringBuilder sb = new StringBuilder();
            foreach (var item in arg) {
                sb.Append(item.StringValue);
            }
            return new Operand(OperandType.STRING, sb.ToString());
        }

        private Operand Func_Code(List<Operand> arg)
        {
            CheckArgsCount("CODE", arg, new OperandType[][] {
                new OperandType[] {  OperandType.STRING  },
                 });
            var t = arg[0].StringValue;
            if (t.Length == 0) {
                return ThrowError("字符串长度为0", arg);
            }
            return new Operand(OperandType.NUMBER, (double)(int)(char)t[0]);
        }

        private Operand Func_Clean(List<Operand> arg)
        {
            CheckArgsCount("CLEAN", arg, new OperandType[][] {
                new OperandType[] {  OperandType.STRING  },
                 });

            var t = arg[0].StringValue;
            t = System.Text.RegularExpressions.Regex.Replace(t, @"[\f\n\r\t\v]", "");
            return new Operand(OperandType.STRING, t);
        }

        private Operand Func_Char(List<Operand> arg)
        {
            CheckArgsCount("Char", arg, new OperandType[][] {
                new OperandType[] {  OperandType.STRING  },
                 });

            char c = (char)(int)arg[0].NumberValue;
            return new Operand(OperandType.STRING, c.ToString());
        }





    }
}
