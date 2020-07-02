using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using ToolGood.Algorithm.LitJson;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm
{
    class MathVisitor : AbstractParseTreeVisitor<Operand>, ImathVisitor<Operand>
    {
        public event Func<string, Operand> GetParameter;
        public int excelIndex;

        #region base

        public Operand VisitProg([NotNull] mathParser.ProgContext context)
        {
            return this.Visit(context.expr());
        }

        public Operand VisitMulDiv_fun([NotNull] mathParser.MulDiv_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];

            if (context.op.Type == mathLexer.MUL) {
                if (secondValue.Type == OperandType.BOOLEAN) {
                    if (secondValue.BooleanValue)
                        return firstValue;
                    else
                        return Operand.Create(0);
                } else if (firstValue.Type == OperandType.BOOLEAN) {
                    if (firstValue.BooleanValue)
                        return secondValue;
                    else
                        return Operand.Create(0);
                }
                if (firstValue.Type == OperandType.STRING) {
                    var a = firstValue.ToDate();
                    if (a.IsError == false) firstValue = a;
                }
                if (firstValue.Type == OperandType.DATE) {
                    secondValue = secondValue.ToNumber();
                    if (secondValue.IsError) { return secondValue; }
                    return Operand.Create((Date)(firstValue.DateValue * secondValue.NumberValue));
                }

                if (secondValue.Type == OperandType.STRING) {
                    var a = secondValue.ToDate();
                    if (a.IsError == false) secondValue = a;
                }
                if (secondValue.Type == OperandType.DATE) {
                    firstValue = firstValue.ToNumber();
                    if (firstValue.IsError) { return firstValue; }
                    return Operand.Create((Date)(secondValue.DateValue * firstValue.NumberValue));
                }

                firstValue = firstValue.ToNumber();
                if (firstValue.IsError) { return firstValue; }
                secondValue = secondValue.ToNumber();
                if (secondValue.IsError) { return secondValue; }
                return Operand.Create(firstValue.NumberValue * secondValue.NumberValue);
            } else if (context.op.Type == mathLexer.DIV) {
                secondValue = secondValue.ToNumber("Div fun right value");
                if (secondValue.NumberValue == 0) {
                    return Operand.Error("无法除0");
                }
                if (firstValue.Type == OperandType.STRING) {
                    var a = firstValue.ToDate();
                    if (a.IsError == false) firstValue = a;
                }
                if (firstValue.Type == OperandType.DATE) {
                    return Operand.Create(firstValue.DateValue / secondValue.NumberValue);
                }

                firstValue = firstValue.ToNumber();
                if (firstValue.IsError) { return firstValue; }
                secondValue = secondValue.ToNumber();
                if (secondValue.IsError) { return secondValue; }
                return Operand.Create(firstValue.NumberValue / secondValue.NumberValue);
            } else if (context.op.Type == mathLexer.MOD_2) {
                firstValue = firstValue.ToNumber("% fun right value");
                if (firstValue.IsError) { return firstValue; }
                secondValue = secondValue.ToNumber("% fun right value");
                if (secondValue.IsError) { return secondValue; }
                if (secondValue.NumberValue == 0) {
                    return Operand.Error("无法除0");
                }
                return Operand.Create(firstValue.NumberValue % secondValue.NumberValue);
            }
            return Operand.Error("VisitMulDiv_fun出错了");
        }

        public Operand VisitAddSub_fun([NotNull] mathParser.AddSub_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];

            if (context.op.Type == mathLexer.MERGE) {
                return Operand.Create(firstValue.ToString("").StringValue + secondValue.ToString("").StringValue);
            }
            if (context.op.Type == mathLexer.ADD) {
                if (firstValue.Type == OperandType.STRING) {
                    var a = firstValue.ToDate();
                    if (a.IsError == false) firstValue = a;
                }
                if (secondValue.Type == OperandType.STRING) {
                    var a = secondValue.ToDate();
                    if (a.IsError == false) secondValue = a;
                }
                if (firstValue.Type == OperandType.DATE && secondValue.Type == OperandType.DATE) {
                    return Operand.Create(firstValue.DateValue + secondValue.DateValue);
                } else if (firstValue.Type == OperandType.DATE) {
                    secondValue = secondValue.ToNumber();
                    if (secondValue.IsError) { return secondValue; }
                    return Operand.Create(firstValue.DateValue + secondValue.NumberValue);
                } else if (secondValue.Type == OperandType.DATE) {
                    firstValue = firstValue.ToNumber();
                    if (firstValue.IsError) { return firstValue; }
                    return Operand.Create(secondValue.DateValue + firstValue.NumberValue);
                }
                firstValue = firstValue.ToNumber("");
                if (firstValue.IsError) { return firstValue; }
                secondValue = secondValue.ToNumber();
                if (secondValue.IsError) { return secondValue; }
                return Operand.Create(firstValue.NumberValue + secondValue.NumberValue);
            } else if (context.op.Type == mathLexer.SUB) {
                if (firstValue.Type == OperandType.STRING) {
                    var a = firstValue.ToDate();
                    if (a.IsError == false) firstValue = a;
                }
                if (secondValue.Type == OperandType.STRING) {
                    var a = secondValue.ToDate();
                    if (a.IsError == false) secondValue = a;
                }
                if (firstValue.Type == OperandType.DATE && secondValue.Type == OperandType.DATE) {
                    return Operand.Create(firstValue.DateValue - secondValue.DateValue);
                } else if (firstValue.Type == OperandType.DATE) {
                    secondValue = secondValue.ToNumber();
                    if (secondValue.IsError) { return secondValue; }
                    return Operand.Create(firstValue.DateValue - secondValue.NumberValue);
                } else if (secondValue.Type == OperandType.DATE) {
                    firstValue = firstValue.ToNumber();
                    if (firstValue.IsError) { return firstValue; }
                    return Operand.Create(secondValue.DateValue - firstValue.NumberValue);
                }
                firstValue = firstValue.ToNumber("");
                if (firstValue.IsError) { return firstValue; }
                secondValue = secondValue.ToNumber();
                if (secondValue.IsError) { return secondValue; }
                return Operand.Create(firstValue.NumberValue - secondValue.NumberValue);
            }
            throw new NotImplementedException();
        }

        public Operand VisitJudge_fun([NotNull] mathParser.Judge_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];
            int type = context.op.Type;

            int r;
            if (firstValue.Type == secondValue.Type) {
                if (firstValue.Type == OperandType.STRING || firstValue.Type == OperandType.JSON) {
                    r = compare(firstValue.ToString("").StringValue, secondValue.ToString("").StringValue);
                } else if (firstValue.Type == OperandType.ARRARY) {
                    return Operand.Error("两个类型无法比较");
                } else {
                    r = compare(firstValue.ToNumber("").NumberValue, secondValue.ToNumber("").NumberValue);
                }
            } else if ((firstValue.Type == OperandType.DATE && secondValue.Type == OperandType.STRING) || (secondValue.Type == OperandType.DATE && firstValue.Type == OperandType.STRING)
                 || (firstValue.Type == OperandType.NUMBER && secondValue.Type == OperandType.STRING) || (secondValue.Type == OperandType.NUMBER && firstValue.Type == OperandType.STRING)
                  ) {
                r = compare(firstValue.ToString().StringValue, secondValue.ToString().StringValue);
            } else if ((firstValue.Type == OperandType.BOOLEAN && secondValue.Type == OperandType.STRING) || (secondValue.Type == OperandType.BOOLEAN && firstValue.Type == OperandType.STRING)) {
                r = compare2(firstValue.ToString().StringValue, secondValue.ToString().StringValue);
            } else if (firstValue.Type == OperandType.STRING || secondValue.Type == OperandType.STRING
                  || firstValue.Type == OperandType.JSON || secondValue.Type == OperandType.JSON
                  || firstValue.Type == OperandType.ARRARY || secondValue.Type == OperandType.ARRARY
                  ) {
                return Operand.Error("两个类型无法比较");
            } else {
                r = compare(firstValue.ToNumber("").NumberValue, secondValue.ToNumber("").NumberValue);
            }
            if (type == mathLexer.LT) {
                return Operand.Create(r == -1);
            } else if (type == mathLexer.LE) {
                return Operand.Create(r <= 0);
            } else if (type == mathLexer.GT) {
                return Operand.Create(r == 1);
            } else if (type == mathLexer.GE) {
                return Operand.Create(r >= 0);
            } else if (type == mathLexer.ET) {
                return Operand.Create(r == 0);
            }
            return Operand.Create(r != 0);
        }

        private int compare(double t1, double t2)
        {
            t1 = Math.Round(t1, 12);
            t2 = Math.Round(t2, 12);
            if (t1 == t2) {
                return 0;
            } else if (t1 > t2) {
                return 1;
            }
            return -1;
        }
        private int compare(string t1, string t2)
        {
            if (t1 == t2) {
                return 0;
            }
            List<string> ts = new List<string>() { t1, t2 };
            ts = ts.OrderBy(q => q).ToList();
            if (t1 == ts[1]) {
                return 1;
            }
            return -1;
        }
        private int compare2(string t1, string t2)
        {
            if (t1.Equals(t2, StringComparison.OrdinalIgnoreCase)) {
                return 0;
            }
            List<string> ts = new List<string>() { t1, t2 };
            ts = ts.OrderBy(q => q).ToList();
            if (t1 == ts[1]) {
                return 1;
            }
            return -1;
        }

        public Operand VisitArray_fun([NotNull] mathParser.Array_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }
            return Operand.Create(args);
        }
        public Operand VisitBracket_fun([NotNull] mathParser.Bracket_funContext context)
        {
            return this.Visit(context.expr());
        }

        public Operand VisitNUM_fun([NotNull] mathParser.NUM_funContext context)
        {
            var sub = context.SUB()?.GetText() ?? "";
            var t = context.NUM().GetText();
            var d = double.Parse(sub + t, CultureInfo.GetCultureInfo("en-US"));
            return Operand.Create(d);
        }
        public Operand VisitSTRING_fun([NotNull] mathParser.STRING_funContext context)
        {
            var opd = context.STRING().GetText();
            StringBuilder sb = new StringBuilder();
            int index = 1;
            while (index < opd.Length - 1) {
                var c = opd[index++];
                if (c == '\\') {
                    var c2 = opd[index++];
                    if (c2 == 'n') sb.Append('\n');
                    else if (c2 == 'r') sb.Append('\r');
                    else if (c2 == 't') sb.Append('\t');
                    else if (c2 == '0') sb.Append('\0');
                    else if (c2 == 'v') sb.Append('\v');
                    else if (c2 == 'a') sb.Append('\a');
                    else if (c2 == 'b') sb.Append('\b');
                    else if (c2 == 'f') sb.Append('\f');
                    else sb.Append(opd[index++]);
                } else {
                    sb.Append(c);
                }
            }
            return Operand.Create(sb.ToString());
        }
        public Operand VisitPARAMETER_fun([NotNull] mathParser.PARAMETER_funContext context)
        {
            var p = this.Visit(context.parameter()).ToString();
            if (p.IsError) return p;

            if (GetParameter != null) {
                return GetParameter(p.StringValue);
            }
            return Operand.Error("");
        }

        #endregion

        #region flow
        public Operand VisitIF_fun([NotNull] mathParser.IF_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var b = args[0].ToBoolean("");
            if (b.IsError) { return b; }

            if (b.BooleanValue) {
                if (args.Count > 1) {
                    return args[1];
                }
                return Operand.True;
            }
            if (args.Count == 3) {
                return args[2];
            }
            return Operand.False;
        }
        public Operand VisitIFERROR_fun([NotNull] mathParser.IFERROR_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); args.Add(a); }

            if (args[0].IsError) {
                return args[1];
            }
            if (args.Count == 3) {
                return args[2];
            }
            return Operand.False;

        }
        public Operand VisitIFNUMBER_fun([NotNull] mathParser.IFNUMBER_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            if (args[0].Type == OperandType.NUMBER) {
                return args[1];
            }
            if (args.Count == 3) {
                return args[2];
            }
            return Operand.False;
        }
        public Operand VisitIFTEXT_fun([NotNull] mathParser.IFTEXT_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            if (args[0].Type == OperandType.STRING) {
                if (args.Count > 1) {
                    return args[1];
                }
                return Operand.True;
            }
            if (args.Count == 3) return args[2];
            return Operand.False;
        }
        public Operand VisitISNUMBER_fun([NotNull] mathParser.ISNUMBER_funContext context)
        {
            var firstValue = this.Visit(context.expr());
            if (firstValue.IsError) { return firstValue; }

            if (firstValue.Type == OperandType.NUMBER) {
                return Operand.True;
            }
            return Operand.False;
        }
        public Operand VisitISTEXT_fun([NotNull] mathParser.ISTEXT_funContext context)
        {
            var firstValue = this.Visit(context.expr());
            if (firstValue.IsError) { return firstValue; }

            if (firstValue.Type == OperandType.STRING) {
                return Operand.True;
            }
            return Operand.False;
        }
        public Operand VisitISERROR_fun([NotNull] mathParser.ISERROR_funContext context)
        {
            var firstValue = this.Visit(context.expr());
            if (firstValue.Type == OperandType.ERROR) {
                return Operand.True;
            }
            return Operand.False;
        }
        public Operand VisitAND_fun([NotNull] mathParser.AND_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) {
                var a = this.Visit(item).ToBoolean();
                if (a.IsError) { return a; }
                args.Add(a);
            }

            var b = true;
            foreach (var item in args) {
                if (item.BooleanValue == false) {
                    b = false;
                    break;
                }
            }
            return Operand.Create(b);
        }
        public Operand VisitOR_fun([NotNull] mathParser.OR_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) {
                var a = this.Visit(item).ToBoolean();
                if (a.IsError) { return a; }
                args.Add(a);
            }

            var b = false;
            foreach (var item in args) {
                if (item.BooleanValue == true) {
                    b = true;
                    break;
                }
            }
            return Operand.Create(b);
        }
        public Operand VisitNOT_fun([NotNull] mathParser.NOT_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToBoolean();
            if (firstValue.IsError) { return firstValue; }
            return Operand.Create(!firstValue.BooleanValue);
        }
        public Operand VisitTRUE_fun([NotNull] mathParser.TRUE_funContext context)
        {
            return Operand.True;
        }
        public Operand VisitFALSE_fun([NotNull] mathParser.FALSE_funContext context)
        {
            return Operand.False;
        }
        #endregion

        #region math

        #region base

        public Operand VisitE_fun([NotNull] mathParser.E_funContext context)
        {
            return Operand.Create(Math.E);
        }
        public Operand VisitPI_fun([NotNull] mathParser.PI_funContext context)
        {
            return Operand.Create(Math.PI);
        }

        public Operand VisitABS_fun([NotNull] mathParser.ABS_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("ABS left value");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(Math.Abs(firstValue.NumberValue));
        }
        public Operand VisitQUOTIENT_fun([NotNull] mathParser.QUOTIENT_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber(); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];

            if (secondValue.NumberValue == 0) {
                return Operand.Error("div 0 error!");
            }
            return Operand.Create((double)(int)(firstValue.NumberValue / secondValue.NumberValue));
        }
        public Operand VisitMOD_fun([NotNull] mathParser.MOD_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber(); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];

            if (secondValue.NumberValue == 0) {
                return Operand.Error("div 0 error!");
            }
            return Operand.Create((int)(firstValue.NumberValue % secondValue.NumberValue));

        }
        public Operand VisitSIGN_fun([NotNull] mathParser.SIGN_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Sign func");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(Math.Sign(firstValue.NumberValue));
        }
        public Operand VisitSQRT_fun([NotNull] mathParser.SQRT_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("ABS left value");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(Math.Sqrt(firstValue.NumberValue));
        }
        public Operand VisitTRUNC_fun([NotNull] mathParser.TRUNC_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("TRUNC func left value");
            if (firstValue.IsError) { return firstValue; }
            return Operand.Create((int)(firstValue.NumberValue));
        }
        public Operand VisitINT_fun([NotNull] mathParser.INT_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("INT func left value");
            if (firstValue.IsError) { return firstValue; }
            return Operand.Create((int)(firstValue.NumberValue));
        }
        public Operand VisitGCD_fun([NotNull] mathParser.GCD_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            List<int> list = new List<int>();
            foreach (var item in args) {
                if (item.Type == OperandType.ARRARY) {
                    foreach (var it in item.ArrayValue) {
                        var a = it.ToNumber();
                        if (a.IsError) { return a; }
                        list.Add(a.IntValue);
                    }
                } else {
                    var a = item.ToNumber();
                    if (a.IsError) { return a; }
                    list.Add(a.IntValue);
                }
            }
            return Operand.Create(F_base_gcd(list));
        }
        public Operand VisitLCM_fun([NotNull] mathParser.LCM_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            List<int> list = new List<int>();
            foreach (var item in args) {
                if (item.Type == OperandType.ARRARY) {
                    foreach (var it in item.ArrayValue) {
                        var a = it.ToNumber();
                        if (a.IsError) { return a; }
                        list.Add(a.IntValue);
                    }
                } else {
                    var a = item.ToNumber();
                    if (a.IsError) { return a; }
                    list.Add(a.IntValue);
                }

            }
            return Operand.Create(F_base_lgm(list));
        }
        public Operand VisitCOMBIN_fun([NotNull] mathParser.COMBIN_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber(); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];

            var total = firstValue.IntValue;
            var count = secondValue.IntValue;
            double sum = 1;
            double sum2 = 1;
            for (int i = 0; i < count; i++) {
                sum *= (total - i);
                sum2 *= (i + 1);
            }
            return Operand.Create(sum / sum2);
        }
        public Operand VisitPERMUT_fun([NotNull] mathParser.PERMUT_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber(); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];

            var total = firstValue.IntValue;
            var count = secondValue.IntValue;

            double sum = 1;
            for (int i = 0; i < count; i++) {
                sum *= (total - i);
            }
            return Operand.Create(sum);
        }

        private int F_base_gcd(List<int> list)
        {
            list = list.OrderBy(q => q).ToList();
            var g = F_base_gcd(list[1], list[0]);
            for (int i = 2; i < list.Count; i++) {
                g = F_base_gcd(list[i], g);
            }
            return g;
        }
        private int F_base_gcd(int a, int b)
        {
            if (b == 1) { return 1; }
            if (b == 0) { return a; }
            return F_base_gcd(b, a % b);
        }
        private int F_base_lgm(List<int> list)
        {
            list = list.OrderBy(q => q).ToList();
            list.RemoveAll(q => q <= 1);

            var a = list[0];
            for (int i = 1; i < list.Count; i++) {
                var b = list[i];
                int g = b > a ? F_base_gcd(b, a) : F_base_gcd(a, b);
                a = a / g * b;
            }
            return a;
        }
        #endregion

        #region trigonometric functions
        public Operand VisitDEGREES_fun([NotNull] mathParser.DEGREES_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }

            var z = firstValue.NumberValue;
            var r = (z / Math.PI * 180);
            return Operand.Create(r);
        }
        public Operand VisitRADIANS_fun([NotNull] mathParser.RADIANS_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("RADIANS left value");
            if (firstValue.IsError) { return firstValue; }

            var r = firstValue.NumberValue / 180 * Math.PI;
            return Operand.Create(r);
        }
        public Operand VisitCOS_fun([NotNull] mathParser.COS_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("RADIANS left value");
            if (firstValue.IsError) { return firstValue; }
            return Operand.Create(Math.Cos(firstValue.NumberValue));
        }
        public Operand VisitCOSH_fun([NotNull] mathParser.COSH_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(Math.Cosh(firstValue.NumberValue));
        }
        public Operand VisitSIN_fun([NotNull] mathParser.SIN_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }
            return Operand.Create(Math.Sin(firstValue.NumberValue));
        }
        public Operand VisitSINH_fun([NotNull] mathParser.SINH_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }
            return Operand.Create(Math.Sinh(firstValue.NumberValue));
        }
        public Operand VisitTAN_fun([NotNull] mathParser.TAN_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }
            return Operand.Create(Math.Tan(firstValue.NumberValue));
        }
        public Operand VisitTANH_fun([NotNull] mathParser.TANH_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }
            return Operand.Create(Math.Tanh(firstValue.NumberValue));
        }
        public Operand VisitACOS_fun([NotNull] mathParser.ACOS_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }
            return Operand.Create(Math.Acos(firstValue.NumberValue));
        }
        public Operand VisitACOSH_fun([NotNull] mathParser.ACOSH_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }

#if NETSTANDARD2_1
            return Operand.Create(Math.Acosh(firstValue.NumberValue));
#else
            var z = firstValue.NumberValue;
            if (z < 1) {
                return Operand.Error("acosh中参数小于1");
            }
            var r = Math.Log(z + Math.Pow(z * z - 1, 0.5));

            return Operand.Create(r);
#endif
        }
        public Operand VisitASIN_fun([NotNull] mathParser.ASIN_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(Math.Asin(firstValue.NumberValue));
        }
        public Operand VisitASINH_fun([NotNull] mathParser.ASINH_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }
#if NETSTANDARD2_1
            return Operand.Create(Math.Asinh(firstValue.NumberValue));
#else
            var x = firstValue.NumberValue;
            var d = Math.Log(x + Math.Sqrt(x * x + 1));
            return Operand.Create(d);
#endif        
        }
        public Operand VisitATAN_fun([NotNull] mathParser.ATAN_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(Math.Atan(firstValue.NumberValue));
        }
        public Operand VisitATANH_fun([NotNull] mathParser.ATANH_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }
#if NETSTANDARD2_1
            return Operand.Create(Math.Atanh(firstValue.NumberValue));
#else
            var x = firstValue.NumberValue;
            var d = Math.Log((1 + x) / (1 - x)) / 2;
            return Operand.Create(d);
#endif        
        }
        public Operand VisitATAN2_fun([NotNull] mathParser.ATAN2_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber(); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];

            return Operand.Create(Math.Atan2(secondValue.NumberValue, firstValue.NumberValue));
        }

        #endregion

        #region rounding

        public Operand VisitROUND_fun([NotNull] mathParser.ROUND_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber(); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];

            return Operand.Create((double)Math.Round((decimal)firstValue.NumberValue, secondValue.IntValue, MidpointRounding.AwayFromZero));
        }
        public Operand VisitROUNDDOWN_fun([NotNull] mathParser.ROUNDDOWN_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber(); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];

            var a = Math.Pow(10, secondValue.IntValue);
            var b = firstValue.NumberValue;

            b = ((double)(int)(b * a)) / a;
            return Operand.Create(b);
        }
        public Operand VisitROUNDUP_fun([NotNull] mathParser.ROUNDUP_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber(); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];

            var a = Math.Pow(10, secondValue.IntValue);
            var b = firstValue.NumberValue;

            var t = (Math.Ceiling(Math.Abs(b) * a)) / a;
            if (b > 0) return Operand.Create(t);
            return Operand.Create(-t);
        }
        public Operand VisitCEILING_fun([NotNull] mathParser.CEILING_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber(); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            if (args.Count == 1)
                return Operand.Create(Math.Ceiling(firstValue.NumberValue));

            var secondValue = args[1];
            var a = firstValue.NumberValue;
            var b = secondValue.NumberValue;
            var d = Math.Ceiling(a / b) * b;
            return Operand.Create(d);
        }
        public Operand VisitFLOOR_fun([NotNull] mathParser.FLOOR_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber(); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            if (args.Count == 1)
                return Operand.Create(Math.Floor(firstValue.NumberValue));

            var secondValue = args[1];
            var a = firstValue.NumberValue;
            var b = secondValue.NumberValue;
            var d = Math.Floor(a / b) * b;
            return Operand.Create(d);
        }
        public Operand VisitEVEN_fun([NotNull] mathParser.EVEN_funContext context)
        {

            var firstValue = this.Visit(context.expr()).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }

            var z = firstValue.NumberValue;
            if (z % 2 == 0) {
                return firstValue;
            }
            z = Math.Ceiling(z);
            if (z % 2 == 0) {
                return Operand.Create(z);
            }
            z = z + 1;
            return Operand.Create(z);
        }
        public Operand VisitODD_fun([NotNull] mathParser.ODD_funContext context)
        {

            var firstValue = this.Visit(context.expr()).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }
            var z = firstValue.NumberValue;
            if (z % 2 == 1) {
                return firstValue;
            }
            z = Math.Ceiling(z);
            if (z % 2 == 1) {
                return Operand.Create(z);
            }
            z = z + 1;
            return Operand.Create(z);
        }
        public Operand VisitMROUND_fun([NotNull] mathParser.MROUND_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber(); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];

            var a = secondValue.NumberValue;
            var b = firstValue.NumberValue;
            var r = Math.Round(b / a, 0) * a;
            return Operand.Create(r);
        }
        #endregion

        #region RAND
        public Operand VisitRAND_fun([NotNull] mathParser.RAND_funContext context)
        {
            var tick = DateTime.Now.Ticks;
            Random rand = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            return Operand.Create(rand.NextDouble());
        }
        public Operand VisitRANDBETWEEN_fun([NotNull] mathParser.RANDBETWEEN_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber(); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];

            var tick = DateTime.Now.Ticks;
            Random rand = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            return Operand.Create(rand.NextDouble() * (secondValue.NumberValue - firstValue.NumberValue) + firstValue.NumberValue);
        }
        #endregion

        #region  power logarithm factorial
        public Operand VisitFACT_fun([NotNull] mathParser.FACT_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }

            var z = firstValue.IntValue;
            if (z < 0) {
                return Operand.Error("fact中参数小于0");
            }
            double d = 1;
            for (int i = 1; i <= z; i++) {
                d *= i;
            }
            return Operand.Create(d);
        }
        public Operand VisitFACTDOUBLE_fun([NotNull] mathParser.FACTDOUBLE_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }

            var z = firstValue.IntValue;
            if (z < 0) {
                return Operand.Error("factdouble中参数小于0");
            }
            double d = 1;
            for (int i = z; i > 0; i -= 2) {
                d *= i;
            }
            return Operand.Create(d);
        }
        public Operand VisitPOWER_fun([NotNull] mathParser.POWER_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber(); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];

            return Operand.Create(Math.Pow(firstValue.NumberValue, secondValue.NumberValue));
        }
        public Operand VisitEXP_fun([NotNull] mathParser.EXP_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(Math.Exp(firstValue.NumberValue));
        }
        public Operand VisitLN_fun([NotNull] mathParser.LN_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }
            return Operand.Create(Math.Log(firstValue.NumberValue));
        }
        public Operand VisitLOG_fun([NotNull] mathParser.LOG_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) {
                var a = this.Visit(item).ToNumber();
                if (a.IsError) { return a; }
                args.Add(a);
            }

            if (args.Count > 1) {
                return Operand.Create(Math.Log(args[0].NumberValue, args[1].NumberValue));
            }
            return Operand.Create(Math.Log(args[0].NumberValue, 10));
        }
        public Operand VisitLOG10_fun([NotNull] mathParser.LOG10_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }
            return Operand.Create(Math.Log(firstValue.NumberValue, 10));
        }
        public Operand VisitMULTINOMIAL_fun([NotNull] mathParser.MULTINOMIAL_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) {
                var a = this.Visit(item);
                args.Add(a);
            }

            int sum = 0;
            int n = 1;
            foreach (var item in args) {
                if (item.Type == OperandType.ARRARY) {
                    foreach (var it in item.ArrayValue) {
                        var a = it.ToNumber();
                        if (a.IsError) { return a; }
                        n *= F_base_Factorial(a.IntValue);
                        sum += a.IntValue;
                    }
                } else {
                    var a = item.ToNumber();
                    if (a.IsError) { return a; }
                    n *= F_base_Factorial(a.IntValue);
                    sum += a.IntValue;
                }
            }
            var r = F_base_Factorial(sum) / n;
            return Operand.Create(r);
        }
        public Operand VisitPRODUCT_fun([NotNull] mathParser.PRODUCT_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) {
                var a = this.Visit(item);
                args.Add(a);
            }
            double d = 1;
            for (int i = 0; i < args.Count; i++) {
                var item = args[i];
                if (item.Type == OperandType.ARRARY) {
                    foreach (var it in item.ArrayValue) {
                        var a = it.ToNumber();
                        if (a.IsError) { return a; }
                        d *= a.NumberValue;
                    }
                } else {
                    var a = item.ToNumber();
                    if (a.IsError) { return a; }
                    d *= a.NumberValue;
                }
            }
            return Operand.Create(d);
        }
        public Operand VisitSQRTPI_fun([NotNull] mathParser.SQRTPI_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }
            return Operand.Create(Math.Sqrt(firstValue.NumberValue * Math.PI));
        }
        public Operand VisitSUMSQ_fun([NotNull] mathParser.SUMSQ_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) {
                var a = this.Visit(item);
                args.Add(a);
            }
            double d = 0;
            for (int i = 0; i < args.Count; i++) {
                var item = args[i];
                if (item.Type == OperandType.ARRARY) {
                    foreach (var it in item.ArrayValue) {
                        var a = it.ToNumber();
                        if (a.IsError) { return a; }
                        d += a.NumberValue * a.NumberValue;
                    }
                } else {
                    var a = item.ToNumber();
                    if (a.IsError) { return a; }
                    d += a.NumberValue * a.NumberValue;
                }
            }

            return Operand.Create(d);
        }



        private int F_base_Factorial(int a)
        {
            int r = 1;
            for (int i = a; i > 0; i--) {
                r *= i;
            }
            return r;
        }
        #endregion

        #endregion

        #region string

        public Operand VisitASC_fun([NotNull] mathParser.ASC_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToString("ASC left value");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(F_base_ToDBC(firstValue.StringValue));
        }
        public Operand VisitJIS_fun([NotNull] mathParser.JIS_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToString("JIS left value");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(F_base_ToSBC(firstValue.StringValue));
        }
        public Operand VisitCHAR_fun([NotNull] mathParser.CHAR_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("CHAR left value");
            if (firstValue.IsError) { return firstValue; }

            char c = (char)(int)firstValue.NumberValue;
            return Operand.Create(c.ToString());
        }
        public Operand VisitCLEAN_fun([NotNull] mathParser.CLEAN_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToString("CLEAN left value");
            if (firstValue.IsError) { return firstValue; }

            var t = firstValue.StringValue;
            t = Regex.Replace(t, @"[\f\n\r\t\v]", "");
            return Operand.Create(t);
        }
        public Operand VisitCODE_fun([NotNull] mathParser.CODE_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToString("CODE left value");
            if (firstValue.IsError) { return firstValue; }

            var t = firstValue.StringValue;
            if (t.Length == 0) {
                return Operand.Error("字符串长度为0");
            }
            return Operand.Create((double)(int)(char)t[0]);
        }
        public Operand VisitCONCATENATE_fun([NotNull] mathParser.CONCATENATE_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) {
                var a = this.Visit(item).ToString("CONCATENATE ");
                if (a.IsError) { return a; }
                args.Add(a);
            }

            StringBuilder sb = new StringBuilder();
            foreach (var item in args) {
                sb.Append(item.StringValue);
            }
            return Operand.Create(sb.ToString());
        }
        public Operand VisitEXACT_fun([NotNull] mathParser.EXACT_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber(); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];

            return Operand.Create(firstValue.StringValue == secondValue.StringValue);
        }
        public Operand VisitFIND_fun([NotNull] mathParser.FIND_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToString("FIND left value");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToString("FIND left value");
            if (secondValue.IsError) { return secondValue; }

            if (args.Count == 2) {
                var p = secondValue.StringValue.IndexOf(firstValue.StringValue) + excelIndex;
                return Operand.Create(p);
            }
            var count = args[2].ToNumber();
            if (count.IsError) { return count; }

            var p2 = secondValue.StringValue.IndexOf(firstValue.StringValue, count.IntValue) + excelIndex;
            return Operand.Create(p2);
        }
        public Operand VisitFIXED_fun([NotNull] mathParser.FIXED_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var num = 2;
            if (args.Count > 1) {
                args[1] = args[1].ToNumber();
                if (args[1].IsError) { return args[1]; }
                num = args[1].IntValue;
            }
            args[0] = args[0].ToNumber();
            if (args[0].IsError) { return args[0]; }

            var s = Math.Round(args[0].NumberValue, num);
            var no = false;
            if (args.Count == 3) {
                args[2] = args[2].ToBoolean();
                if (args[2].IsError) { return args[2]; }
                no = args[2].BooleanValue;
            }
            if (no == false) {
                return Operand.Create(s.ToString("N" + num));
            }
            return Operand.Create(s.ToString());
        }
        public Operand VisitLEFT_fun([NotNull] mathParser.LEFT_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToString("FIND left value");
            if (firstValue.IsError) { return firstValue; }

            if (args.Count == 1) {
                return Operand.Create(firstValue.StringValue[0].ToString());
            }
            var secondValue = args[1].ToNumber("FIND left value");
            if (secondValue.IsError) { return secondValue; }
            return Operand.Create(firstValue.StringValue.Substring(0, secondValue.IntValue));
        }
        public Operand VisitLEN_fun([NotNull] mathParser.LEN_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToString("LEN func");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(firstValue.StringValue.Length);
        }
        public Operand VisitLOWER_fun([NotNull] mathParser.LOWER_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToString("LOWER func");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(firstValue.StringValue.ToLower());
        }
        public Operand VisitMID_fun([NotNull] mathParser.MID_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToString("MID left value");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToNumber("MID left value");
            if (secondValue.IsError) { return secondValue; }
            var thirdValue = args[2].ToNumber("MID left value");
            if (thirdValue.IsError) { return thirdValue; }

            return Operand.Create(firstValue.StringValue.Substring(secondValue.IntValue - excelIndex, thirdValue.IntValue));
        }
        public Operand VisitPROPER_fun([NotNull] mathParser.PROPER_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToString("MID left value");
            if (firstValue.IsError) { return firstValue; }

            var text = firstValue.StringValue;
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
        public Operand VisitREPLACE_fun([NotNull] mathParser.REPLACE_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            args[0] = args[0].ToString("");
            if (args.Count == 3) {
                args[1] = args[1].ToString("");
                args[2] = args[2].ToString("");
                var srcText = args[0].StringValue;
                var old = args[1].StringValue;
                var newstr = args[2].StringValue;
                return Operand.Create(srcText.Replace(old, newstr));
            }

            var oldtext = args[0].StringValue;
            args[1] = args[1].ToNumber("");
            args[2] = args[2].ToNumber("");

            var start = args[1].IntValue - excelIndex;
            var length = args[2].IntValue;
            var newtext = args[3].StringValue;

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
            return Operand.Create(sb.ToString());
        }
        public Operand VisitREPT_fun([NotNull] mathParser.REPT_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToString("MID left value");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToNumber("MID left value");
            if (secondValue.IsError) { return secondValue; }

            var newtext = firstValue.StringValue;
            var length = secondValue.IntValue;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++) {
                sb.Append(newtext);
            }
            return Operand.Create(sb.ToString());
        }
        public Operand VisitRIGHT_fun([NotNull] mathParser.RIGHT_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToString("FIND left value");
            if (firstValue.IsError) { return firstValue; }

            if (args.Count == 1) {
                return Operand.Create(firstValue.StringValue[firstValue.StringValue.Length - 1].ToString());
            }
            var secondValue = args[1].ToNumber("FIND left value");
            if (secondValue.IsError) { return secondValue; }
            return Operand.Create(firstValue.StringValue.Substring(firstValue.StringValue.Length - secondValue.IntValue, secondValue.IntValue));
        }
        public Operand VisitRMB_fun([NotNull] mathParser.RMB_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("MID left value");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(F_base_ToChineseRMB(firstValue.NumberValue));
        }
        public Operand VisitSEARCH_fun([NotNull] mathParser.SEARCH_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            args[0] = args[0].ToString();
            args[1] = args[1].ToString();
            if (args.Count == 2) {
                var p = args[1].StringValue.IndexOf(args[0].StringValue, StringComparison.OrdinalIgnoreCase) + excelIndex;
                return Operand.Create(p);
            }
            args[2] = args[2].ToNumber();
            var p2 = args[1].StringValue.IndexOf(args[0].StringValue, args[2].IntValue, StringComparison.OrdinalIgnoreCase) + excelIndex;
            return Operand.Create(p2);
        }
        public Operand VisitSUBSTITUTE_fun([NotNull] mathParser.SUBSTITUTE_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }
            args[0] = args[0].ToString();
            args[1] = args[1].ToString();
            args[2] = args[2].ToString();
            if (args.Count == 3) {
                return Operand.Create(args[0].StringValue.Replace(args[1].StringValue, args[2].StringValue));
            }
            args[3] = args[3].ToNumber();
            if (args[3].IsError) { return args[3]; }

            string text = args[0].StringValue;
            string oldtext = args[1].StringValue;
            string newtext = args[2].StringValue;
            int index = args[3].IntValue;

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
            return Operand.Create(sb.ToString());
        }
        public Operand VisitT_fun([NotNull] mathParser.T_funContext context)
        {
            var firstValue = this.Visit(context.expr());
            if (firstValue.Type == OperandType.STRING) {
                return firstValue;
            }
            return Operand.Create("");
        }
        public Operand VisitTEXT_fun([NotNull] mathParser.TEXT_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToString("MID left value");
            if (secondValue.IsError) { return secondValue; }

            if (firstValue.Type == OperandType.STRING) {
                return firstValue;
            } else if (firstValue.Type == OperandType.BOOLEAN) {
                return Operand.Create(firstValue.BooleanValue.ToString());
            } else if (firstValue.Type == OperandType.NUMBER) {
                return Operand.Create(firstValue.NumberValue.ToString(secondValue.StringValue));
            } else if (firstValue.Type == OperandType.DATE) {
                return Operand.Create(firstValue.DateValue.ToString(secondValue.StringValue));
            }
            return Operand.Create(firstValue.StringValue.ToString());
        }
        public Operand VisitTRIM_fun([NotNull] mathParser.TRIM_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToString("MID left value");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(firstValue.StringValue.Trim());
        }
        public Operand VisitUPPER_fun([NotNull] mathParser.UPPER_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToString("UPPER func");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(firstValue.StringValue.ToUpper());
        }
        public Operand VisitVALUE_fun([NotNull] mathParser.VALUE_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToString("UPPER func");
            if (firstValue.IsError) { return firstValue; }

            if (double.TryParse(firstValue.StringValue, NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out double d)) {
                return Operand.Create(d);
            }
            return Operand.Error("无法转成数字");
        }

        private String F_base_ToSBC(String input)
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
        private String F_base_ToDBC(String input)
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
        private string F_base_ToChineseRMB(double x)
        {
            string s = x.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            string d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            return Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString());
        }
        #endregion

        #region date time

        public Operand VisitDATEVALUE_fun([NotNull] mathParser.DATEVALUE_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToString("DATEVALUE left value");
            if (firstValue.IsError) { return firstValue; }

            var d = DateTime.Parse(firstValue.StringValue).Date;
            return Operand.Create(d);
        }
        public Operand VisitTIMEVALUE_fun([NotNull] mathParser.TIMEVALUE_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToString("TIMEVALUE left value");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(new Date(TimeSpan.Parse(firstValue.StringValue)));
        }
        public Operand VisitDATE_fun([NotNull] mathParser.DATE_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToNumber(); if (a.IsError) { return a; } args.Add(a); }

            Date d;
            if (args.Count == 3) {
                d = new Date(args[0].IntValue, args[1].IntValue, args[2].IntValue, 0, 0, 0);
            } else if (args.Count == 4) {
                d = new Date(args[0].IntValue, args[1].IntValue, args[2].IntValue, args[3].IntValue, 0, 0);
            } else if (args.Count == 5) {
                d = new Date(args[0].IntValue, args[1].IntValue, args[2].IntValue, args[3].IntValue, args[4].IntValue, 0);
            } else {
                d = new Date(args[0].IntValue, args[1].IntValue, args[2].IntValue, args[3].IntValue, args[4].IntValue, args[5].IntValue);
            }
            return Operand.Create(d);
        }
        public Operand VisitTIME_fun([NotNull] mathParser.TIME_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToNumber(); if (a.IsError) { return a; } args.Add(a); }

            Date d;
            if (args.Count == 3) {
                d = new Date(0, 0, 0, args[0].IntValue, args[1].IntValue, args[2].IntValue);
            } else {
                d = new Date(0, 0, 0, args[0].IntValue, args[1].IntValue, 0);
            }
            return Operand.Create(d);
        }
        public Operand VisitNOW_fun([NotNull] mathParser.NOW_funContext context)
        {
            return Operand.Create(new Date(DateTime.Now));
        }
        public Operand VisitTODAY_fun([NotNull] mathParser.TODAY_funContext context)
        {
            return Operand.Create(new Date(DateTime.Today));
        }
        public Operand VisitYEAR_fun([NotNull] mathParser.YEAR_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToDate("YEAR left value");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(firstValue.DateValue.Year);
        }
        public Operand VisitMONTH_fun([NotNull] mathParser.MONTH_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToDate("YEAR left value");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(firstValue.DateValue.Month);
        }
        public Operand VisitDAY_fun([NotNull] mathParser.DAY_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToDate("YEAR left value");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(firstValue.DateValue.Day);
        }
        public Operand VisitHOUR_fun([NotNull] mathParser.HOUR_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToDate("YEAR left value");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(firstValue.DateValue.Hour);
        }
        public Operand VisitMINUTE_fun([NotNull] mathParser.MINUTE_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToDate("YEAR left value");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(firstValue.DateValue.Minute);
        }
        public Operand VisitSECOND_fun([NotNull] mathParser.SECOND_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToDate("YEAR left value");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(firstValue.DateValue.Second);
        }
        public Operand VisitWEEKDAY_fun([NotNull] mathParser.WEEKDAY_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            args[0] = args[0].ToDate();
            if (args[0].IsError) { return args[0]; }

            var type = 1;
            if (args.Count == 2) {
                args[1] = args[1].ToNumber();
                if (args[1].IsError) { return args[1]; }
                type = args[1].IntValue;
            }

            var t = ((DateTime)args[0].DateValue).DayOfWeek;
            if (type == 1) {
                return Operand.Create((double)(t + 1));
            } else if (type == 2) {
                if (t == 0) return Operand.Create(7d);
                return Operand.Create((double)t);
            }
            if (t == 0) {
                return Operand.Create(6d);
            }
            return Operand.Create((double)(t - 1));
        }
        public Operand VisitDATEDIF_fun([NotNull] mathParser.DATEDIF_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            args[0] = args[0].ToDate();
            if (args[0].IsError) { return args[0]; }
            args[1] = args[1].ToDate();
            if (args[1].IsError) { return args[1]; }
            args[2] = args[2].ToString();
            if (args[2].IsError) { return args[2]; }

            var startDate = (DateTime)args[0].DateValue;
            var endDate = (DateTime)args[1].DateValue;
            var t = args[2].StringValue.ToLower();

            if (t == "y") {
                #region y
                bool b = false;
                if (startDate.Month < endDate.Month) {
                    b = true;
                } else if (startDate.Month == endDate.Month) {
                    if (startDate.Day <= endDate.Day) b = true;
                }
                if (b) {
                    return Operand.Create((endDate.Year - startDate.Year));
                } else {
                    return Operand.Create((endDate.Year - startDate.Year - 1));
                }
                #endregion
            } else if (t == "m") {
                #region m
                bool b = false;
                if (startDate.Day <= endDate.Day) b = true;
                if (b) {
                    return Operand.Create((endDate.Year * 12 + endDate.Month - startDate.Year * 12 - startDate.Month));
                } else {
                    return Operand.Create((endDate.Year * 12 + endDate.Month - startDate.Year * 12 - startDate.Month - 1));
                }
                #endregion
            } else if (t == "d") {
                return Operand.Create((endDate - startDate).Days);
            } else if (t == "yd") {
                #region yd
                var day = endDate.DayOfYear - startDate.DayOfYear;
                if (endDate.Year > startDate.Year && day < 0) {
                    var days = new DateTime(startDate.Year, 12, 31).DayOfYear;
                    day = days + day;
                }
                return Operand.Create((day));
                #endregion
            } else if (t == "md") {
                #region md
                var mo = endDate.Day - startDate.Day;
                if (mo < 0) {
                    var days = new DateTime(startDate.Year, startDate.Month + 1, 1).AddDays(-1).Day;
                    mo += days;
                }
                return Operand.Create((mo));
                #endregion
            } else if (t == "ym") {
                #region ym
                var mo = endDate.Month - startDate.Month;
                if (endDate.Day < startDate.Day) mo = mo - 1;
                if (mo < 0) mo += 12;
                return Operand.Create((mo));
                #endregion
            }
            return Operand.Error("DATE比较类型错误");
        }
        public Operand VisitDAYS360_fun([NotNull] mathParser.DAYS360_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            args[0] = args[0].ToDate();
            if (args[0].IsError) { return args[0]; }
            args[1] = args[1].ToDate();
            if (args[1].IsError) { return args[1]; }

            var startDate = (DateTime)args[0].DateValue;
            var endDate = (DateTime)args[1].DateValue;

            var method = false;
            if (args.Count == 3) {
                args[2] = args[2].ToDate();
                if (args[2].IsError) { return args[2]; }
                method = args[2].BooleanValue;
            }
            var days = endDate.Year * 360 + (endDate.Month - 1) * 30
                        - startDate.Year * 360 - (startDate.Month - 1) * 30;
            if (method) {
                if (endDate.Day == 31) days += 30;
                if (startDate.Day == 31) days -= 30;
            } else {
                if (startDate.Day == new DateTime(startDate.Year, startDate.Month + 1, 1).AddDays(-1).Day) {
                    days -= 30;
                } else {
                    days -= startDate.Day;
                }
                if (endDate.Day == new DateTime(endDate.Year, endDate.Month + 1, 1).AddDays(-1).Day) {
                    if (startDate.Day < 30) {
                        days += 31;
                    } else {
                        days += 30;
                    }
                } else {
                    days += endDate.Day;
                }
            }
            return Operand.Create(days);
        }
        public Operand VisitEDATE_fun([NotNull] mathParser.EDATE_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            args[0] = args[0].ToDate();
            if (args[0].IsError) { return args[0]; }
            args[1] = args[1].ToNumber();
            if (args[1].IsError) { return args[1]; }

            return Operand.Create((Date)(((DateTime)args[0].DateValue).AddMonths(args[1].IntValue)));
        }
        public Operand VisitEOMONTH_fun([NotNull] mathParser.EOMONTH_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            args[0] = args[0].ToDate();
            if (args[0].IsError) { return args[0]; }
            args[1] = args[1].ToNumber();
            if (args[1].IsError) { return args[1]; }

            var dt = ((DateTime)args[0].DateValue).AddMonths(args[1].IntValue + 1);
            dt = new DateTime(dt.Year, dt.Month, 1).AddDays(-1);
            return Operand.Create(dt);
        }
        public Operand VisitNETWORKDAYS_fun([NotNull] mathParser.NETWORKDAYS_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToDate(); if (a.IsError) { return a; } args.Add(a); }

            var startDate = (DateTime)args[0].DateValue;
            var endDate = (DateTime)args[1].DateValue;

            List<DateTime> list = new List<DateTime>();
            for (int i = 2; i < args.Count; i++) {
                list.Add(args[i].DateValue);
            }

            var days = 0;
            while (startDate <= endDate) {
                if (startDate.DayOfWeek != DayOfWeek.Sunday && startDate.DayOfWeek != DayOfWeek.Saturday) {
                    if (list.Contains(startDate) == false) {
                        days++;
                    }
                }
                startDate = startDate.AddDays(1);
            }
            return Operand.Create(days);
        }
        public Operand VisitWORKDAY_fun([NotNull] mathParser.WORKDAY_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            args[0] = args[0].ToDate();
            if (args[0].IsError) { return args[0]; }
            args[1] = args[1].ToNumber();
            if (args[1].IsError) { return args[1]; }


            var startDate = (DateTime)args[0].DateValue;
            var days = args[1].IntValue;
            List<DateTime> list = new List<DateTime>();
            for (int i = 2; i < args.Count; i++) {
                args[i] = args[i].ToDate();
                if (args[i].IsError) { return args[i]; }
                list.Add(args[i].DateValue);
            }
            while (days > 0) {
                startDate = startDate.AddDays(1);
                if (startDate.DayOfWeek == DayOfWeek.Saturday) continue;
                if (startDate.DayOfWeek == DayOfWeek.Sunday) continue;
                if (list.Contains(startDate)) continue;
                days--;
            }
            return Operand.Create(startDate);
        }
        public Operand VisitWEEKNUM_fun([NotNull] mathParser.WEEKNUM_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            args[0] = args[0].ToDate();
            if (args[0].IsError) { return args[0]; }

            var startDate = (DateTime)args[0].DateValue;

            var days = startDate.DayOfYear + (int)(new DateTime(startDate.Year, 1, 1).DayOfWeek);
            if (args.Count == 2) {
                args[1] = args[1].ToNumber();
                if (args[1].IsError) { return args[1]; }
                if (args[1].IntValue == 2) {
                    days--;
                }
            }

            var week = Math.Ceiling(days / 7.0);
            return Operand.Create(week);
        }

        #endregion

        #region sum

        public Operand VisitMAX_fun([NotNull] mathParser.MAX_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToNumber(); if (a.IsError) { return a; } args.Add(a); }

            var max = double.MinValue;

            foreach (var item in args) {
                if (item.Type == OperandType.ARRARY) {
                    foreach (var it in item.ArrayValue) {
                        var a = it.ToNumber();
                        if (a.IsError) { return a; }
                        if (a.NumberValue > max) {
                            max = a.NumberValue;
                        }
                    }
                } else {
                    var a = item.ToNumber();
                    if (a.IsError) { return a; }
                    if (a.NumberValue > max) {
                        max = a.NumberValue;
                    }
                }
            }
            return Operand.Create(max);
        }
        public Operand VisitMEDIAN_fun([NotNull] mathParser.MEDIAN_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToNumber(); if (a.IsError) { return a; } args.Add(a); }

            List<int> list = new List<int>();
            foreach (var item in args) {
                if (item.Type == OperandType.ARRARY) {
                    foreach (var it in item.ArrayValue) {
                        var a = it.ToNumber();
                        if (a.IsError) { return a; }
                        list.Add(a.IntValue);
                    }
                } else {
                    var a = item.ToNumber();
                    if (a.IsError) { return a; }
                    list.Add(a.IntValue);
                }
            }

            list = list.OrderBy(q => q).ToList();
            return Operand.Create(list[list.Count / 2]);
        }
        public Operand VisitMIN_fun([NotNull] mathParser.MIN_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToNumber(); if (a.IsError) { return a; } args.Add(a); }

            var min = double.MaxValue;

            foreach (var item in args) {
                if (item.Type == OperandType.ARRARY) {
                    foreach (var it in item.ArrayValue) {
                        var a = it.ToNumber();
                        if (a.IsError) { return a; }
                        if (a.NumberValue < min) {
                            min = a.NumberValue;
                        }
                    }
                } else {
                    var a = item.ToNumber();
                    if (a.IsError) { return a; }
                    if (a.NumberValue < min) {
                        min = a.NumberValue;
                    }
                }
            }
            return Operand.Create(min);
        }
        public Operand VisitQUARTILE_fun([NotNull] mathParser.QUARTILE_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToArray();
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToNumber("MID left value");
            if (secondValue.IsError) { return secondValue; }

            List<double> list = new List<double>();
            foreach (var item in firstValue.ArrayValue) {
                if (item.Type == OperandType.ARRARY) {
                    foreach (var it in item.ArrayValue) {
                        var a = it.ToNumber();
                        if (a.IsError) { return a; }
                        list.Add(a.NumberValue);
                    }
                } else {
                    var a = item.ToNumber();
                    if (a.IsError) { return a; }
                    list.Add(a.NumberValue);

                }
            }

            var quant = secondValue.IntValue;
            return Operand.Create(ExcelFunctions.Quartile(list.ToArray(), quant));
        }
        public Operand VisitMODE_fun([NotNull] mathParser.MODE_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToNumber(); if (a.IsError) { return a; } args.Add(a); }

            Dictionary<double, int> dict = new Dictionary<double, int>();
            foreach (var item in args) {
                if (item.Type == OperandType.ARRARY) {
                    foreach (var it in item.ArrayValue) {
                        var a = it.ToNumber();
                        if (a.IsError) { return a; }
                        if (dict.ContainsKey(item.NumberValue)) {
                            dict[item.NumberValue] += 1;
                        } else {
                            dict[item.NumberValue] = 1;
                        }
                    }
                } else {
                    var a = item.ToNumber();
                    if (a.IsError) { return a; }
                    if (dict.ContainsKey(item.NumberValue)) {
                        dict[item.NumberValue] += 1;
                    } else {
                        dict[item.NumberValue] = 1;
                    }
                }
            }
            return Operand.Create(dict.OrderByDescending(q => q.Value).First().Key);
        }
        public Operand VisitLARGE_fun([NotNull] mathParser.LARGE_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToArray();
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToNumber("MID left value");
            if (secondValue.IsError) { return secondValue; }

            List<double> list = new List<double>();
            foreach (var item in firstValue.ArrayValue) {
                if (item.Type == OperandType.ARRARY) {
                    foreach (var it in item.ArrayValue) {
                        var a = it.ToNumber();
                        if (a.IsError) { return a; }
                        list.Add(a.NumberValue);
                    }
                } else {
                    var a = item.ToNumber();
                    if (a.IsError) { return a; }
                    list.Add(a.NumberValue);

                }
            }
            list = list.OrderByDescending(q => q).ToList();
            var quant = secondValue.IntValue;
            return Operand.Create(list[secondValue.IntValue - excelIndex]);
        }
        public Operand VisitSMALL_fun([NotNull] mathParser.SMALL_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToArray();
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToNumber("MID left value");
            if (secondValue.IsError) { return secondValue; }

            List<double> list = new List<double>();
            foreach (var item in firstValue.ArrayValue) {
                if (item.Type == OperandType.ARRARY) {
                    foreach (var it in item.ArrayValue) {
                        var a = it.ToNumber();
                        if (a.IsError) { return a; }
                        list.Add(a.NumberValue);
                    }
                } else {
                    var a = item.ToNumber();
                    if (a.IsError) { return a; }
                    list.Add(a.NumberValue);

                }
            }
            list = list.OrderBy(q => q).ToList();
            var quant = secondValue.IntValue;
            return Operand.Create(list[secondValue.IntValue - excelIndex]);
        }
        public Operand VisitPERCENTILE_fun([NotNull] mathParser.PERCENTILE_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToArray();
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToNumber("MID left value");
            if (secondValue.IsError) { return secondValue; }

            List<double> list = new List<double>();
            foreach (var item in firstValue.ArrayValue) {
                if (item.Type == OperandType.ARRARY) {
                    foreach (var it in item.ArrayValue) {
                        var a = it.ToNumber();
                        if (a.IsError) { return a; }
                        list.Add(a.NumberValue);
                    }
                } else {
                    var a = item.ToNumber();
                    if (a.IsError) { return a; }
                    list.Add(a.NumberValue);

                }
            }
            var k = secondValue.NumberValue;
            return Operand.Create(ExcelFunctions.Percentile(list.ToArray(), k));
        }
        public Operand VisitPERCENTRANK_fun([NotNull] mathParser.PERCENTRANK_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToArray();
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToNumber("MID left value");
            if (secondValue.IsError) { return secondValue; }

            List<double> list = new List<double>();
            foreach (var item in firstValue.ArrayValue) {
                if (item.Type == OperandType.ARRARY) {
                    foreach (var it in item.ArrayValue) {
                        var a = it.ToNumber();
                        if (a.IsError) { return a; }
                        list.Add(a.NumberValue);
                    }
                } else {
                    var a = item.ToNumber();
                    if (a.IsError) { return a; }
                    list.Add(a.NumberValue);
                }
            }

            var k = secondValue.NumberValue;
            var v = ExcelFunctions.PercentRank(list.ToArray(), k);
            var d = 3;
            if (args.Count == 3) {
                var thirdValue = args[2].ToNumber("MID left value");
                if (thirdValue.IsError) { return thirdValue; }
                d = thirdValue.IntValue;
            }
            return Operand.Create(Math.Round(v, d));
        }
        public Operand VisitAVERAGE_fun([NotNull] mathParser.AVERAGE_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            List<double> list = F_base_GetList(args);
            return Operand.Create(list.Average());
        }
        public Operand VisitAVERAGEIF_fun([NotNull] mathParser.AVERAGEIF_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            List<double> list = args[0].GetNumberList();
            List<double> sumdbs = list;
            if (args.Count == 3) sumdbs = args[2].GetNumberList();

            double sum;
            int count;
            if (args[1].Type == OperandType.NUMBER) {
                count = F_base_countif(list, args[1].NumberValue);
                sum = count * args[1].NumberValue;
            } else {
                if (double.TryParse(args[1].StringValue.Trim(), NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out double d)) {
                    count = F_base_countif(list, args[1].NumberValue);
                    sum = F_base_sumif(list, "=" + args[1].StringValue.Trim(), sumdbs);
                } else {
                    count = F_base_countif(list, args[1].StringValue.Trim());
                    sum = F_base_sumif(list, args[1].StringValue.Trim(), sumdbs);
                }
            }
            return Operand.Create(sum / count);
        }
        public Operand VisitGEOMEAN_fun([NotNull] mathParser.GEOMEAN_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            if (args.Count == 1) return args[0];
            var dbs = F_base_GetList(args);
            double sum = 1;
            foreach (var db in dbs) {
                sum *= db;
            }
            return Operand.Create(Math.Pow(sum, 1.0 / dbs.Count));
        }
        public Operand VisitHARMEAN_fun([NotNull] mathParser.HARMEAN_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            if (args.Count == 1) return args[0];
            var dbs = F_base_GetList(args);
            double sum = 0;
            foreach (var db in dbs) {
                sum += 1 / db;
            }
            return Operand.Create(dbs.Count / sum);
        }
        public Operand VisitCOUNT_fun([NotNull] mathParser.COUNT_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            List<double> list = F_base_GetList(args);
            return Operand.Create(list.Count);
        }
        public Operand VisitCOUNTIF_fun([NotNull] mathParser.COUNTIF_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var dbs = args[0].GetNumberList();
            int count = 0;
            if (args[1].Type == OperandType.NUMBER) {
                count = F_base_countif(dbs, args[1].NumberValue);
            } else {
                if (double.TryParse(args[1].StringValue.Trim(), NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out double d)) {
                    count = F_base_countif(dbs, args[1].NumberValue);
                } else {
                    count = F_base_countif(dbs, args[1].StringValue.Trim());
                }
            }
            return Operand.Create(count);
        }
        public Operand VisitSUM_fun([NotNull] mathParser.SUM_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            if (args.Count == 1) return args[0];
            var dbs = F_base_GetList(args);
            double sum = 0;
            foreach (var db in dbs) {
                sum += db;
            }
            return Operand.Create(sum);
        }
        public Operand VisitSUMIF_fun([NotNull] mathParser.SUMIF_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var dbs = args[0].GetNumberList();
            var sumdbs = dbs;
            if (args.Count == 3) sumdbs = args[2].GetNumberList();
            double sum;
            if (args[1].Type == OperandType.NUMBER) {
                sum = F_base_countif(dbs, args[1].NumberValue) * args[1].NumberValue;
            } else {
                if (double.TryParse(args[1].StringValue.Trim(), NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out _)) {
                    sum = F_base_sumif(dbs, "=" + args[1].StringValue.Trim(), sumdbs);
                } else {
                    sum = F_base_sumif(dbs, args[1].StringValue.Trim(), sumdbs);
                }
            }
            return Operand.Create(sum);
        }
        public Operand VisitAVEDEV_fun([NotNull] mathParser.AVEDEV_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            List<double> list = F_base_GetList(args);
            double avg = list.Average();
            double sum = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += Math.Abs(list[i] - avg);
            }
            return Operand.Create(sum / list.Count);
        }
        public Operand VisitSTDEV_fun([NotNull] mathParser.STDEV_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            List<double> list = F_base_GetList(args);
            double avg = list.Average();
            double sum = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += (list[i] - avg) * (list[i] - avg);
            }
            return Operand.Create(Math.Sqrt(sum / (list.Count - 1)));
        }
        public Operand VisitSTDEVP_fun([NotNull] mathParser.STDEVP_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }


            List<double> list = F_base_GetList(args);
            double sum = 0;
            double avg = list.Average();

            for (int i = 0; i < list.Count; i++) {
                sum += (list[i] - avg) * (list[i] - avg);
            }
            return Operand.Create(Math.Sqrt(sum / (list.Count)));
        }
        public Operand VisitDEVSQ_fun([NotNull] mathParser.DEVSQ_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            List<double> list = F_base_GetList(args);
            double avg = list.Average();
            double sum = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += (list[i] - avg) * (list[i] - avg);
            }
            return Operand.Create(sum);
        }
        public Operand VisitVAR_fun([NotNull] mathParser.VAR_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            List<double> list = F_base_GetList(args);
            double sum = 0;
            double sum2 = 0;
            //double avg = list.Average();
            for (int i = 0; i < list.Count; i++) {
                sum += list[i] * list[i];
                sum2 += list[i];
            }
            return Operand.Create((list.Count * sum - sum2 * sum2) / list.Count / (list.Count - 1));
        }
        public Operand VisitVARP_fun([NotNull] mathParser.VARP_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            List<double> list = F_base_GetList(args);

            double sum = 0;
            double avg = list.Average();
            for (int i = 0; i < list.Count; i++) {
                sum += (avg - list[i]) * (avg - list[i]);
            }
            return Operand.Create(sum / list.Count);
        }
        public Operand VisitNORMDIST_fun([NotNull] mathParser.NORMDIST_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            args[0] = args[0].ToNumber();
            if (args[0].IsError) return args[0];
            args[1] = args[1].ToNumber();
            if (args[1].IsError) return args[1];
            args[2] = args[2].ToNumber();
            if (args[2].IsError) return args[2];

            args[3] = args[3].ToBoolean();
            if (args[3].IsError) return args[3];

            var num = args[0].NumberValue;
            var avg = args[1].NumberValue;
            var STDEV = args[2].NumberValue;
            var b = args[3].BooleanValue;
            return Operand.Create(ExcelFunctions.NormDist(num, avg, STDEV, b));
        }
        public Operand VisitNORMINV_fun([NotNull] mathParser.NORMINV_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToNumber(); if (a.IsError) { return a; } args.Add(a); }

            var num = args[0].NumberValue;
            var avg = args[1].NumberValue;
            var STDEV = args[2].NumberValue;
            return Operand.Create(ExcelFunctions.NormInv(num, avg, STDEV));
        }
        public Operand VisitNORMSDIST_fun([NotNull] mathParser.NORMSDIST_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("");
            if (firstValue.IsError) { return firstValue; }

            var k = firstValue.NumberValue;
            return Operand.Create(ExcelFunctions.NormSDist(k));
        }
        public Operand VisitNORMSINV_fun([NotNull] mathParser.NORMSINV_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("");
            if (firstValue.IsError) { return firstValue; }

            var k = firstValue.NumberValue;
            return Operand.Create(ExcelFunctions.NormSInv(k));
        }
        public Operand VisitBETADIST_fun([NotNull] mathParser.BETADIST_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToNumber(); if (a.IsError) { return a; } args.Add(a); }

            var x = args[0].NumberValue;
            var alpha = args[1].NumberValue;
            var beta = args[2].NumberValue;
            return Operand.Create(ExcelFunctions.BetaDist(x, alpha, beta));
        }
        public Operand VisitBETAINV_fun([NotNull] mathParser.BETAINV_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToNumber(); if (a.IsError) { return a; } args.Add(a); }

            var probability = args[0].NumberValue;
            var alpha = args[1].NumberValue;
            var beta = args[2].NumberValue;
            return Operand.Create(ExcelFunctions.BetaInv(probability, alpha, beta));
        }
        public Operand VisitBINOMDIST_fun([NotNull] mathParser.BINOMDIST_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            args[0] = args[0].ToNumber();
            if (args[0].IsError) return args[0];
            args[1] = args[1].ToNumber();
            if (args[1].IsError) return args[1];
            args[2] = args[2].ToNumber();
            if (args[2].IsError) return args[2];

            args[3] = args[3].ToBoolean();
            if (args[3].IsError) return args[3];

            return Operand.Create(ExcelFunctions.BinomDist(args[0].IntValue, args[1].IntValue, args[2].NumberValue, args[3].BooleanValue));
        }
        public Operand VisitEXPONDIST_fun([NotNull] mathParser.EXPONDIST_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            args[0] = args[0].ToNumber();
            if (args[0].IsError) return args[0];
            args[1] = args[1].ToNumber();
            if (args[1].IsError) return args[1];
            args[2] = args[2].ToBoolean();
            if (args[2].IsError) return args[2];


            return Operand.Create(ExcelFunctions.ExponDist(args[0].NumberValue, args[1].NumberValue, args[2].BooleanValue));
        }
        public Operand VisitFDIST_fun([NotNull] mathParser.FDIST_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToNumber(); if (a.IsError) { return a; } args.Add(a); }

            var x = args[0].NumberValue;
            var degreesFreedom = args[1].IntValue;
            var degreesFreedom2 = args[2].IntValue;
            return Operand.Create(ExcelFunctions.FDist(x, degreesFreedom, degreesFreedom2));
        }
        public Operand VisitFINV_fun([NotNull] mathParser.FINV_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToNumber(); if (a.IsError) { return a; } args.Add(a); }


            var probability = args[0].NumberValue;
            var degreesFreedom = args[1].IntValue;
            var degreesFreedom2 = args[2].IntValue;
            return Operand.Create(ExcelFunctions.FInv(probability, degreesFreedom, degreesFreedom2));
        }
        public Operand VisitFISHER_fun([NotNull] mathParser.FISHER_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("");
            if (firstValue.IsError) { return firstValue; }

            var x = firstValue.NumberValue;
            var n = 0.5 * Math.Log((1 + x) / (1 - x));
            return Operand.Create(n);
        }
        public Operand VisitFISHERINV_fun([NotNull] mathParser.FISHERINV_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("");
            if (firstValue.IsError) { return firstValue; }

            var x = firstValue.NumberValue;
            var n = (Math.Exp(2 * x) - 1) / (Math.Exp(2 * x) + 1);
            return Operand.Create(n);
        }
        public Operand VisitGAMMADIST_fun([NotNull] mathParser.GAMMADIST_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            args[0] = args[0].ToNumber();
            if (args[0].IsError) return args[0];
            args[1] = args[1].ToNumber();
            if (args[1].IsError) return args[1];
            args[2] = args[2].ToNumber();
            if (args[2].IsError) return args[2];

            args[3] = args[3].ToBoolean();
            if (args[3].IsError) return args[3];


            var x = args[0].NumberValue;
            var alpha = args[1].NumberValue;
            var beta = args[2].NumberValue;
            var cumulative = args[3].BooleanValue;
            return Operand.Create(ExcelFunctions.GammaDist(x, alpha, beta, cumulative));
        }
        public Operand VisitGAMMAINV_fun([NotNull] mathParser.GAMMAINV_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToNumber(); if (a.IsError) { return a; } args.Add(a); }

            var probability = args[0].NumberValue;
            var alpha = args[1].NumberValue;
            var beta = args[2].NumberValue;
            return Operand.Create(ExcelFunctions.GammaInv(probability, alpha, beta));
        }
        public Operand VisitGAMMALN_fun([NotNull] mathParser.GAMMALN_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(ExcelFunctions.GAMMALN(firstValue.NumberValue));
        }
        public Operand VisitHYPGEOMDIST_fun([NotNull] mathParser.HYPGEOMDIST_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToNumber(); if (a.IsError) { return a; } args.Add(a); }

            return Operand.Create(ExcelFunctions.HypgeomDist(args[0].IntValue, args[1].IntValue, args[2].IntValue, args[3].IntValue));
        }
        public Operand VisitLOGINV_fun([NotNull] mathParser.LOGINV_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToNumber(); if (a.IsError) { return a; } args.Add(a); }

            return Operand.Create(ExcelFunctions.LogInv(args[0].NumberValue, args[1].NumberValue, args[2].NumberValue));
        }
        public Operand VisitLOGNORMDIST_fun([NotNull] mathParser.LOGNORMDIST_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToNumber(); if (a.IsError) { return a; } args.Add(a); }

            return Operand.Create(ExcelFunctions.LognormDist(args[0].NumberValue, args[1].NumberValue, args[2].NumberValue));
        }
        public Operand VisitNEGBINOMDIST_fun([NotNull] mathParser.NEGBINOMDIST_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToNumber(); if (a.IsError) { return a; } args.Add(a); }

            return Operand.Create(ExcelFunctions.NegbinomDist(args[0].IntValue, args[1].NumberValue, args[2].NumberValue));
        }
        public Operand VisitPOISSON_fun([NotNull] mathParser.POISSON_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            args[0] = args[0].ToNumber();
            if (args[0].IsError) return args[0];
            args[1] = args[1].ToNumber();
            if (args[1].IsError) return args[1];
            args[2] = args[2].ToBoolean();
            if (args[2].IsError) return args[2];

            return Operand.Create(ExcelFunctions.POISSON(args[0].IntValue, args[1].NumberValue, args[2].BooleanValue));
        }
        public Operand VisitTDIST_fun([NotNull] mathParser.TDIST_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToNumber(); if (a.IsError) { return a; } args.Add(a); }

            var x = args[0].NumberValue;
            var degreesFreedom = args[1].IntValue;
            var tails = args[2].IntValue;
            return Operand.Create(ExcelFunctions.TDist(x, degreesFreedom, tails));
        }
        public Operand VisitTINV_fun([NotNull] mathParser.TINV_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToNumber(); if (a.IsError) { return a; } args.Add(a); }

            var probability = args[0].NumberValue;
            var degreesFreedom = args[1].IntValue;
            return Operand.Create(ExcelFunctions.TInv(probability, degreesFreedom));
        }
        public Operand VisitWEIBULL_fun([NotNull] mathParser.WEIBULL_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            args[0] = args[0].ToNumber();
            if (args[0].IsError) return args[0];
            args[1] = args[1].ToNumber();
            if (args[1].IsError) return args[1];
            args[2] = args[2].ToNumber();
            if (args[2].IsError) return args[2];

            args[3] = args[3].ToBoolean();
            if (args[3].IsError) return args[3];

            return Operand.Create(ExcelFunctions.WEIBULL(args[0].NumberValue, args[1].NumberValue, args[2].NumberValue, args[3].BooleanValue));
        }

        private int F_base_countif(List<double> dbs, double d)
        {
            int count = 0;
            d = Math.Round(d, 12);
            foreach (var item in dbs) {
                if (Math.Round(item, 12) == d) {
                    count++;
                }
            }
            return count;
        }
        private int F_base_countif(List<double> dbs, string s)
        {
            Regex re = new Regex(@"(<|<=|>|>=|=|==|!=|<>) *([-+]?\d+(\.(\d+)?)?)");
            if (re.IsMatch(s) == false) {
                return 0;
            }
            var m = re.Match(s);
            var d = double.Parse(m.Groups[2].Value, CultureInfo.GetCultureInfo("en-US"));
            var ss = m.Groups[1].Value;
            int count = 0;

            foreach (var item in dbs) {
                if (F_base_compare(item, d, s)) {
                    count++;
                }
            }
            return count;
        }
        private double F_base_sumif(List<double> dbs, string s, List<double> sumdbs)
        {
            Regex re = new Regex(@"(<|<=|>|>=|=|==|!=|<>) *([-+]?\d+(\.(\d+)?)?)");
            if (re.IsMatch(s) == false) {
                return 0;
            }
            var m = re.Match(s);
            var d = double.Parse(m.Groups[2].Value, CultureInfo.GetCultureInfo("en-US"));
            var ss = m.Groups[1].Value;
            double sum = 0;

            for (int i = 0; i < dbs.Count; i++) {
                if (F_base_compare(dbs[i], d, s)) {
                    sum += sumdbs[i];
                }
            }
            return sum;
        }
        private bool F_base_compare(double a, double b, string ss)
        {
            if (ss == "<") {
                return a < b;
            } else if (ss == "<=") {
                return a <= b;
            } else if (ss == ">") {
                return a > b;
            } else if (ss == ">=") {
                return a >= b;
            } else if (ss == "=" || ss == "==") {
                return a == b;
            }
            return a != b;
        }
        private List<double> F_base_GetList(List<Operand> args)
        {
            List<double> list = new List<double>();
            foreach (var item in args) {
                if (item.Type == OperandType.NUMBER) {
                    list.Add(item.NumberValue);
                } else if (item.Type == OperandType.ARRARY) {
                    var ls = item.GetNumberList();
                    if (ls == null) continue;
                    foreach (var d in ls) {
                        list.Add(d);
                    }
                }
            }
            return list;
        }
        #endregion

        #region csharp

        public Operand VisitURLENCODE_fun([NotNull] mathParser.URLENCODE_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToString("");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(HttpUtility.UrlEncode(firstValue.StringValue));
        }
        public Operand VisitURLDECODE_fun([NotNull] mathParser.URLDECODE_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToString("");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(HttpUtility.UrlDecode(firstValue.StringValue));
        }
        public Operand VisitHTMLENCODE_fun([NotNull] mathParser.HTMLENCODE_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToString("");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(HttpUtility.HtmlEncode(firstValue.StringValue));
        }
        public Operand VisitHTMLDECODE_fun([NotNull] mathParser.HTMLDECODE_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToString("");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(HttpUtility.HtmlDecode(firstValue.StringValue));
        }
        public Operand VisitBASE64TOTEXT_fun([NotNull] mathParser.BASE64TOTEXT_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToString(""); if (a.IsError) { return a; } args.Add(a); }

            Encoding encoding;
            if (args.Count == 1) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(args[1].StringValue);
            }
            var t = encoding.GetString(Base64.FromBase64String(args[0].StringValue));
            return Operand.Create(t);
        }
        public Operand VisitBASE64URLTOTEXT_fun([NotNull] mathParser.BASE64URLTOTEXT_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToString(""); if (a.IsError) { return a; } args.Add(a); }

            Encoding encoding;
            if (args.Count == 1) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(args[1].StringValue);
            }
            var t = encoding.GetString(Base64.FromBase64ForUrlString(args[0].StringValue));
            return Operand.Create(t);
        }
        public Operand VisitTEXTTOBASE64_fun([NotNull] mathParser.TEXTTOBASE64_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToString(""); if (a.IsError) { return a; } args.Add(a); }

            Encoding encoding;
            if (args.Count == 1) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(args[1].StringValue);
            }
            var bytes = encoding.GetBytes(args[0].StringValue);
            var t = Base64.ToBase64String(bytes);
            return Operand.Create(t);
        }
        public Operand VisitTEXTTOBASE64URL_fun([NotNull] mathParser.TEXTTOBASE64URL_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToString(""); if (a.IsError) { return a; } args.Add(a); }

            Encoding encoding;
            if (args.Count == 1) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(args[1].StringValue);
            }
            var bytes = encoding.GetBytes(args[0].StringValue);
            var t = Base64.ToBase64ForUrlString(bytes);
            return Operand.Create(t);
        }
        public Operand VisitREGEX_fun([NotNull] mathParser.REGEX_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToString("");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToString("");
            if (secondValue.IsError) { return secondValue; }

            if (args.Count == 2) {
                var b = Regex.Match(args[0].StringValue, args[1].StringValue);
                if (b.Success == false) {
                    return Operand.Error("Regex匹配失败");
                }
                return Operand.Create(b.Value);
            } else if (args.Count == 3) {
                var ms = Regex.Matches(args[0].StringValue, args[1].StringValue);
                if (ms.Count <= args[2].IntValue - excelIndex) {
                    return Operand.Error("Regex匹配Index长度错误");
                }
                args[2] = args[2].ToNumber();
                if (args[2].IsError) { return args[2]; }
                return Operand.Create(ms[args[2].IntValue - excelIndex].Value);

            } else {
                var ms = Regex.Matches(args[0].StringValue, args[1].StringValue);
                if (ms.Count <= args[2].IntValue + excelIndex) {
                    return Operand.Error("Regex匹配Index长度错误");
                }
                args[2] = args[2].ToNumber();
                if (args[2].IsError) { return args[2]; }
                args[3] = args[3].ToNumber();
                if (args[3].IsError) { return args[3]; }
                return Operand.Create(ms[args[2].IntValue - excelIndex].Groups[args[3].IntValue].Value);
            }
        }
        public Operand VisitREGEXREPALCE_fun([NotNull] mathParser.REGEXREPALCE_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToString(""); if (a.IsError) { return a; } args.Add(a); }

            var b = Regex.Replace(args[0].StringValue, args[1].StringValue, args[2].StringValue);
            return Operand.Create(b);
        }
        public Operand VisitISREGEX_fun([NotNull] mathParser.ISREGEX_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToString(""); if (a.IsError) { return a; } args.Add(a); }


            var b = Regex.IsMatch(args[0].StringValue, args[1].StringValue);
            return Operand.Create(b);
        }
        public Operand VisitGUID_fun([NotNull] mathParser.GUID_funContext context)
        {
            return Operand.Create(System.Guid.NewGuid().ToString());
        }
        public Operand VisitMD5_fun([NotNull] mathParser.MD5_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToString(""); if (a.IsError) { return a; } args.Add(a); }

            Encoding encoding;
            if (args.Count == 1) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(args[1].StringValue);
            }
            var t = Hash.GetMd5String(encoding.GetBytes(args[0].StringValue));
            return Operand.Create(t);
        }
        public Operand VisitSHA1_fun([NotNull] mathParser.SHA1_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToString(""); if (a.IsError) { return a; } args.Add(a); }

            Encoding encoding;
            if (args.Count == 1) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(args[1].StringValue);
            }
            var t = Hash.GetSha1String(encoding.GetBytes(args[0].StringValue));
            return Operand.Create(t);
        }
        public Operand VisitSHA256_fun([NotNull] mathParser.SHA256_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToString(""); if (a.IsError) { return a; } args.Add(a); }

            Encoding encoding;
            if (args.Count == 1) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(args[1].StringValue);
            }
            var t = Hash.GetSha256String(encoding.GetBytes(args[0].StringValue));
            return Operand.Create(t);
        }
        public Operand VisitSHA512_fun([NotNull] mathParser.SHA512_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToString(""); if (a.IsError) { return a; } args.Add(a); }

            Encoding encoding;
            if (args.Count == 1) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(args[1].StringValue);
            }
            var t = Hash.GetSha512String(encoding.GetBytes(args[0].StringValue));
            return Operand.Create(t);
        }
        public Operand VisitCRC8_fun([NotNull] mathParser.CRC8_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToString(""); if (a.IsError) { return a; } args.Add(a); }

            Encoding encoding;
            if (args.Count == 1) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(args[1].StringValue);
            }
            var t = Hash.GetCrc8String(encoding.GetBytes(args[0].StringValue));
            return Operand.Create(t);
        }
        public Operand VisitCRC16_fun([NotNull] mathParser.CRC16_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToString(""); if (a.IsError) { return a; } args.Add(a); }

            Encoding encoding;
            if (args.Count == 1) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(args[1].StringValue);
            }
            var t = Hash.GetCrc16String(encoding.GetBytes(args[0].StringValue));
            return Operand.Create(t);
        }
        public Operand VisitCRC32_fun([NotNull] mathParser.CRC32_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToString(""); if (a.IsError) { return a; } args.Add(a); }

            Encoding encoding;
            if (args.Count == 1) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(args[1].StringValue);
            }
            var t = Hash.GetCrc32String(encoding.GetBytes(args[0].StringValue));
            return Operand.Create(t);
        }
        public Operand VisitHMACMD5_fun([NotNull] mathParser.HMACMD5_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToString(""); if (a.IsError) { return a; } args.Add(a); }

            Encoding encoding;
            if (args.Count == 2) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(args[2].StringValue);
            }
            var t = Hash.GetHmacMd5String(encoding.GetBytes(args[0].StringValue), args[1].StringValue);
            return Operand.Create(t);
        }
        public Operand VisitHMACSHA1_fun([NotNull] mathParser.HMACSHA1_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToString(""); if (a.IsError) { return a; } args.Add(a); }

            Encoding encoding;
            if (args.Count == 2) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(args[2].StringValue);
            }
            var t = Hash.GetHmacSha1String(encoding.GetBytes(args[0].StringValue), args[1].StringValue);
            return Operand.Create(t);
        }
        public Operand VisitHMACSHA256_fun([NotNull] mathParser.HMACSHA256_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToString(""); if (a.IsError) { return a; } args.Add(a); }

            Encoding encoding;
            if (args.Count == 2) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(args[2].StringValue);
            }
            var t = Hash.GetHmacSha256String(encoding.GetBytes(args[0].StringValue), args[1].StringValue);
            return Operand.Create(t);
        }
        public Operand VisitHMACSHA512_fun([NotNull] mathParser.HMACSHA512_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToString(""); if (a.IsError) { return a; } args.Add(a); }

            Encoding encoding;
            if (args.Count == 2) {
                encoding = Encoding.UTF8;
            } else {
                encoding = Encoding.GetEncoding(args[2].StringValue);
            }
            var t = Hash.GetHmacSha512String(encoding.GetBytes(args[0].StringValue), args[1].StringValue);
            return Operand.Create(t);
        }
        public Operand VisitTRIMSTART_fun([NotNull] mathParser.TRIMSTART_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToString(""); if (a.IsError) { return a; } args.Add(a); }

            var text = args[0].StringValue;
            if (args.Count == 2) {

                return Operand.Create(text.TrimStart(args[1].StringValue.ToArray()));
            }
            return Operand.Create(text.TrimStart());
        }

        public Operand VisitTRIMEND_fun([NotNull] mathParser.TRIMEND_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToString(""); if (a.IsError) { return a; } args.Add(a); }

            var text = args[0].StringValue;
            if (args.Count == 2) {

                return Operand.Create(text.TrimEnd(args[1].StringValue.ToArray()));
            }
            return Operand.Create(text.TrimEnd());
        }

        public Operand VisitINDEXOF_fun([NotNull] mathParser.INDEXOF_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            args[0] = args[0].ToString();
            if (args[0].IsError) { return args[0]; }
            args[1] = args[1].ToString();
            if (args[1].IsError) { return args[1]; }

            var text = args[0].StringValue;
            if (args.Count == 2) {
                return Operand.Create(text.IndexOf(args[1].StringValue) + excelIndex);
            }
            args[2] = args[2].ToString();
            if (args[2].IsError) { return args[2]; }
            if (args.Count == 3) {

                return Operand.Create(text.IndexOf(args[1].StringValue, args[2].IntValue) + excelIndex);
            }
            if (args[3].IsError) { return args[3]; }
            return Operand.Create(text.IndexOf(args[1].StringValue, args[2].IntValue, args[3].IntValue) + excelIndex);
        }
        public Operand VisitLASTINDEXOF_fun([NotNull] mathParser.LASTINDEXOF_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            args[0] = args[0].ToString();
            if (args[0].IsError) { return args[0]; }
            args[1] = args[1].ToString();
            if (args[1].IsError) { return args[1]; }

            var text = args[0].StringValue;
            if (args.Count == 2) {
                return Operand.Create(text.LastIndexOf(args[1].StringValue) + excelIndex);
            }
            args[2] = args[2].ToString();
            if (args[2].IsError) { return args[2]; }
            if (args.Count == 3) {

                return Operand.Create(text.LastIndexOf(args[1].StringValue, args[2].IntValue) + excelIndex);
            }
            if (args[3].IsError) { return args[3]; }
            return Operand.Create(text.LastIndexOf(args[1].StringValue, args[2].IntValue, args[3].IntValue) + excelIndex);
        }
        public Operand VisitSPLIT_fun([NotNull] mathParser.SPLIT_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToString(""); if (a.IsError) { return a; } args.Add(a); }

            return Operand.Create(args[0].StringValue.Split(args[1].StringValue.ToArray()));
        }
        public Operand VisitJOIN_fun([NotNull] mathParser.JOIN_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            if (args[0].Type == OperandType.ARRARY) {
                args[1] = args[1].ToString("");
                if (args[1].IsError) { return args[1]; }

                List<string> list = new List<string>();
                foreach (var item in args[0].ArrayValue) {
                    if (item.Type == OperandType.ARRARY) {
                        foreach (var it in item.ArrayValue) {
                            var a = it.ToString();
                            if (a.IsError) { return a; }
                            list.Add(a.StringValue);
                        }
                    } else {
                        var a = item.ToString();
                        if (a.IsError) { return a; }
                        list.Add(a.StringValue);
                    }
                }
                return Operand.Create(string.Join(args[1].StringValue, list));
            } else {
                args[0] = args[0].ToString("");
                if (args[0].IsError) { return args[0]; }

                List<string> list = new List<string>();
                for (int i = 1; i < args.Count; i++) {
                    var item = args[i];
                    if (item.Type == OperandType.ARRARY) {
                        foreach (var it in item.ArrayValue) {
                            var a = it.ToString();
                            if (a.IsError) { return a; }
                            list.Add(a.StringValue);
                        }
                    } else {
                        var a = item.ToString();
                        if (a.IsError) { return a; }
                        list.Add(a.StringValue);
                    }
                }
                return Operand.Create(string.Join(args[0].StringValue, list));
            }
        }
        public Operand VisitSUBSTRING_fun([NotNull] mathParser.SUBSTRING_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            args[0] = args[0].ToString();
            if (args[0].IsError) { return args[0]; }
            args[1] = args[1].ToNumber();
            if (args[1].IsError) { return args[1]; }

            var text = args[0].StringValue;
            if (args.Count == 2) {
                return Operand.Create(text.Substring(args[1].IntValue - excelIndex));
            }
            args[2] = args[2].ToNumber();
            if (args[2].IsError) { return args[2]; }
            return Operand.Create(text.Substring(args[1].IntValue - excelIndex, args[2].IntValue));
        }
        public Operand VisitSTARTSWITH_fun([NotNull] mathParser.STARTSWITH_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            args[0] = args[0].ToString();
            if (args[0].IsError) { return args[0]; }
            args[1] = args[1].ToString();
            if (args[1].IsError) { return args[1]; }

            var text = args[0].StringValue;
            if (args.Count == 2) {
                return Operand.Create(text.StartsWith(args[1].StringValue));
            }
            args[2] = args[2].ToBoolean();
            if (args[2].IsError) { return args[2]; }
            return Operand.Create(text.StartsWith(args[1].StringValue, args[2].BooleanValue ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));
        }
        public Operand VisitENDSWITH_fun([NotNull] mathParser.ENDSWITH_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            args[0] = args[0].ToString();
            if (args[0].IsError) { return args[0]; }
            args[1] = args[1].ToString();
            if (args[1].IsError) { return args[1]; }

            var text = args[0].StringValue;
            if (args.Count == 2) {
                return Operand.Create(text.EndsWith(args[1].StringValue));
            }
            args[2] = args[2].ToBoolean();
            if (args[2].IsError) { return args[2]; }
            return Operand.Create(text.EndsWith(args[1].StringValue, args[2].BooleanValue ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture));

        }
        public Operand VisitISNULLOREMPTY_fun([NotNull] mathParser.ISNULLOREMPTY_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToString("");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(string.IsNullOrEmpty(firstValue.StringValue));
        }
        public Operand VisitISNULLORWHITESPACE_fun([NotNull] mathParser.ISNULLORWHITESPACE_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToString("");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(string.IsNullOrWhiteSpace(firstValue.StringValue));
        }
        public Operand VisitTOUPPER_fun([NotNull] mathParser.TOUPPER_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToString("");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(firstValue.StringValue.ToUpper());
        }
        public Operand VisitTOLOWER_fun([NotNull] mathParser.TOLOWER_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToString("");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(firstValue.StringValue.ToLower());
        }
        public Operand VisitREMOVESTART_fun([NotNull] mathParser.REMOVESTART_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            args[0] = args[0].ToString();
            if (args[0].IsError) { return args[0]; }
            args[1] = args[1].ToString();
            if (args[1].IsError) { return args[1]; }

            var text = args[0].StringValue;
            if (args.Count == 2) {
                if (text.StartsWith(args[1].StringValue)) {
                    return Operand.Create(text.Substring(args[1].StringValue.Length));
                }
            } else {
                args[2] = args[2].ToBoolean();
                if (args[2].IsError) { return args[2]; }
                if (args[2].BooleanValue) {
                    if (text.StartsWith(args[1].StringValue, StringComparison.OrdinalIgnoreCase)) {
                        return Operand.Create(text.Substring(args[1].StringValue.Length));
                    }
                } else {
                    if (text.StartsWith(args[1].StringValue)) {
                        return Operand.Create(text.Substring(args[1].StringValue.Length));
                    }
                }

            }
            return args[0];
        }
        public Operand VisitREMOVEEND_fun([NotNull] mathParser.REMOVEEND_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            args[0] = args[0].ToString();
            if (args[0].IsError) { return args[0]; }
            args[1] = args[1].ToString();
            if (args[1].IsError) { return args[1]; }

            var text = args[0].StringValue;
            if (args.Count == 2) {
                if (text.EndsWith(args[1].StringValue)) {
                    return Operand.Create(text.Substring(0, text.Length - args[1].StringValue.Length));
                }
            } else {
                args[2] = args[2].ToBoolean();
                if (args[2].IsError) { return args[2]; }
                if (args[2].BooleanValue) {
                    if (text.EndsWith(args[1].StringValue, StringComparison.OrdinalIgnoreCase)) {
                        return Operand.Create(text.Substring(0, text.Length - args[1].StringValue.Length));
                    }
                } else {
                    if (text.EndsWith(args[1].StringValue)) {
                        return Operand.Create(text.Substring(0, text.Length - args[1].StringValue.Length));
                    }
                }

            }
            return args[0];
        }
        public Operand VisitJSON_fun([NotNull] mathParser.JSON_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToString("");
            if (firstValue.IsError) { return firstValue; }

            try {
                var json = JsonMapper.ToObject(firstValue.StringValue);
                return Operand.Create(json);
            } catch (Exception) { }
            return Operand.Error("无法转Json格式！");
        }

        public Operand VisitGetJsonValue_fun([NotNull] mathParser.GetJsonValue_funContext context)
        {
            var firstValue = this.Visit(context.expr());
            if (firstValue.IsError) { return firstValue; }

            var obj = firstValue;
            var op = this.Visit(context.parameter());
            if (obj.Type == OperandType.ARRARY) {
                op = op.ToNumber();
                if (op.IsError) { return op; }
                var index = op.IntValue - excelIndex;
                if (index < obj.ArrayValue.Count)
                    return obj.ArrayValue[index];
                return Operand.Error("");
            }
            if (obj.Type == OperandType.JSON) {
                var json = obj.JsonValue;
                if (json.IsArray) {
                    op = op.ToNumber();
                    if (op.IsError) { return op; }
                    var index = op.IntValue - excelIndex;
                    if (index < json.Count) {
                        var v = json[op.IntValue - excelIndex];
                        if (v.IsString) return Operand.Create(v.ToString());
                        if (v.IsBoolean) return Operand.Create(bool.Parse(v.ToString()));
                        if (v.IsDouble) return Operand.Create(double.Parse(v.ToString(), CultureInfo.GetCultureInfo("en-US")));
                        if (v.IsInt) return Operand.Create(double.Parse(v.ToString(), CultureInfo.GetCultureInfo("en-US")));
                        if (v.IsLong) return Operand.Create(double.Parse(v.ToString(), CultureInfo.GetCultureInfo("en-US")));
                        return Operand.Create(v);
                    }
                    return Operand.Error("");
                } else {
                    op = op.ToString("");
                    if (op.IsError) { return op; }
                    var v = json[op.StringValue];
                    if (v == null) return Operand.Create(v);
                    if (v.IsString) return Operand.Create(v.ToString());
                    if (v.IsBoolean) return Operand.Create(bool.Parse(v.ToString()));
                    if (v.IsDouble) return Operand.Create(double.Parse(v.ToString(), CultureInfo.GetCultureInfo("en-US")));
                    if (v.IsInt) return Operand.Create(double.Parse(v.ToString(), CultureInfo.GetCultureInfo("en-US")));
                    if (v.IsLong) return Operand.Create(double.Parse(v.ToString(), CultureInfo.GetCultureInfo("en-US")));
                    if (v.IsObject) return Operand.Create(v);
                    if (v.IsArray) return Operand.Create(v);
                    return Operand.Create(v);
                }
            }
            return Operand.Error(op + "操作无效！");
        }

        public Operand VisitParameter([NotNull] mathParser.ParameterContext context)
        {
            var expr = context.expr();
            if (expr != null) {
                return this.Visit(expr);
            }
            return Operand.Create(context.p.Text);
        }


        #endregion

    }
}
