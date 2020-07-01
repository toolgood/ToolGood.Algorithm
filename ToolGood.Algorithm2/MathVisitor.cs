using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm
{
    class MathVisitor : AbstractParseTreeVisitor<Operand>, ImathVisitor<Operand>
    {
        public event Func<string, Operand> GetParameter;
        public int excelIndex;

        #region base
        private Operand ThrowError(string errMsg, params Operand[] ops)
        {
            foreach (var item in ops) {
                if (item.IsError) {
                    return item;
                }
            }
            return Operand.Error(errMsg);
        }

        public Operand VisitProg([NotNull] mathParser.ProgContext context)
        {
            return this.Visit(context.expr());
        }

        public Operand VisitMulDiv_fun([NotNull] mathParser.MulDiv_funContext context)
        {
            var firstValue = this.Visit(context.expr(0));
            if (firstValue.IsError) { return firstValue; }
            var secondValue = this.Visit(context.expr(1));
            if (secondValue.IsError) { return secondValue; }

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
                    return Operand.Create((Date)(firstValue.DateValue * secondValue.ToNumber().NumberValue));
                }

                if (secondValue.Type == OperandType.STRING) {
                    var a = secondValue.ToDate();
                    if (a.IsError == false) secondValue = a;
                }
                if (secondValue.Type == OperandType.DATE) {
                    firstValue = firstValue.ToNumber();
                    if (firstValue.IsError) { return firstValue; }
                    return Operand.Create((Date)(firstValue.ToNumber().NumberValue * secondValue.DateValue));
                }

                firstValue = firstValue.ToNumber();
                if (firstValue.IsError) { return firstValue; }
                secondValue = secondValue.ToNumber();
                if (secondValue.IsError) { return secondValue; }
                return Operand.Create(firstValue.NumberValue * secondValue.NumberValue);
            } else if (context.op.Type == mathLexer.DIV) {
                secondValue = secondValue.ToNumber("Div fun right value");
                if (secondValue.NumberValue == 0) {
                    return ThrowError("无法除0");
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
                    return ThrowError("无法除0");
                }
                return Operand.Create(firstValue.NumberValue % secondValue.NumberValue);
            }
            return ThrowError("VisitMulDiv_fun出错了");
        }

        public Operand VisitAddSub_fun([NotNull] mathParser.AddSub_funContext context)
        {
            var firstValue = this.Visit(context.expr(0));
            if (firstValue.IsError) { return firstValue; }
            var secondValue = this.Visit(context.expr(1));
            if (secondValue.IsError) { return secondValue; }

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
            var firstValue = this.Visit(context.expr(0));
            if (firstValue.IsError) { return firstValue; }
            var secondValue = this.Visit(context.expr(1));
            if (secondValue.IsError) { return secondValue; }
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
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }
            return Operand.Create(arg);
        }
        public Operand VisitBracket_fun([NotNull] mathParser.Bracket_funContext context)
        {
            return this.Visit(context.expr());
        }

        public Operand VisitNUM_fun([NotNull] mathParser.NUM_funContext context)
        {
            var t = context.NUM().GetText();
            var d = double.Parse(t, CultureInfo.GetCultureInfo("en-US"));
            return Operand.Create(d);
        }
        public Operand VisitSTRING_fun([NotNull] mathParser.STRING_funContext context)
        {
            var t = context.STRING().GetText();
            t = t.Substring(1, t.Length - 2);
            return Operand.Create(t);
        }
        public Operand VisitPARAMETER_fun([NotNull] mathParser.PARAMETER_funContext context)
        {
            return this.Visit(context.parameter());
            //var t = context.PARAMETER().GetText();
            //t = t.Substring(1, t.Length - 2);
            //if (GetParameter != null) {
            //    return GetParameter(t);
            //}
            //return Operand.Error(t);
        }

        #endregion

        #region flow
        public Operand VisitIF_fun([NotNull] mathParser.IF_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            var b = arg[0].ToBoolean("");
            if (b.IsError) { return b; }

            if (b.BooleanValue) {
                if (arg.Count > 1) {
                    return arg[1];
                }
                return Operand.True;
            }
            if (arg.Count == 3) {
                return arg[2];
            }
            return Operand.False;
        }
        public Operand VisitIFERROR_fun([NotNull] mathParser.IFERROR_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            if (arg[0].IsError) {
                if (arg.Count > 1) {
                    return arg[1];
                }
                return Operand.True;
            }
            if (arg.Count == 3) {
                return arg[2];
            }
            return Operand.False;

        }
        public Operand VisitIFNUMBER_fun([NotNull] mathParser.IFNUMBER_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            if (arg[0].Type == OperandType.NUMBER) {
                return Operand.True;
            } else if (arg[0].Type == OperandType.STRING) {
                if (double.TryParse(arg[0].StringValue, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"), out _)) {
                    return Operand.True;
                }
            }
            if (arg.Count == 3) {
                return arg[2];
            }
            return Operand.False;
        }
        public Operand VisitIFTEXT_fun([NotNull] mathParser.IFTEXT_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            if (arg[0].Type == OperandType.STRING) {
                if (arg.Count > 1) {
                    return arg[1];
                }
                return Operand.True;
            }
            if (arg.Count == 3) return arg[2];
            return Operand.False;
        }
        public Operand VisitISNUMBER_fun([NotNull] mathParser.ISNUMBER_funContext context)
        {
            var firstValue = this.Visit(context.expr());
            if (firstValue.IsError) { return firstValue; }

            if (firstValue.Type == OperandType.NUMBER) {
                return Operand.True;
            } else if (firstValue.Type == OperandType.STRING) {
                if (double.TryParse(firstValue.StringValue, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"), out _)) {
                    return Operand.True;
                }
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
            var arg = new List<Operand>();
            foreach (var item in context.expr()) {
                var a = this.Visit(item).ToBoolean();
                if (a.IsError) { return a; }
                arg.Add(a);
            }

            var b = true;
            foreach (var item in arg) {
                if (item.BooleanValue == false) {
                    b = false;
                    break;
                }
            }
            return Operand.Create(b);
        }
        public Operand VisitOR_fun([NotNull] mathParser.OR_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) {
                var a = this.Visit(item).ToBoolean();
                if (a.IsError) { return a; }
                arg.Add(a);
            }

            var b = false;
            foreach (var item in arg) {
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
            var firstValue = this.Visit(context.expr(0)).ToNumber("ABS firstValue");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = this.Visit(context.expr(1)).ToNumber("ABS secondValue");
            if (secondValue.IsError) { return secondValue; }

            if (secondValue.NumberValue == 0) {
                return ThrowError("div 0 error!");
            }
            return Operand.Create((double)(int)(firstValue.NumberValue / secondValue.NumberValue));
        }
        public Operand VisitMOD_fun([NotNull] mathParser.MOD_funContext context)
        {
            var firstValue = this.Visit(context.expr(0)).ToNumber("Mod fun left value");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = this.Visit(context.expr(1)).ToNumber("Mod fun right value");
            if (secondValue.IsError) { return secondValue; }

            if (secondValue.NumberValue == 0) {
                return ThrowError("div 0 error!");
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

            throw new NotImplementedException();
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
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            List<int> list = new List<int>();
            foreach (var item in arg) {
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
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            List<int> list = new List<int>();
            foreach (var item in arg) {
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
            var firstValue = this.Visit(context.expr(0)).ToNumber();
            if (firstValue.IsError) { return firstValue; }
            var secondValue = this.Visit(context.expr(1)).ToNumber();
            if (secondValue.IsError) { return secondValue; }

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
            var firstValue = this.Visit(context.expr(0)).ToNumber();
            if (firstValue.IsError) { return firstValue; }
            var secondValue = this.Visit(context.expr(1)).ToNumber();
            if (secondValue.IsError) { return secondValue; }

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

            var z = firstValue.NumberValue;
            if (z < 1) {
                return ThrowError("acosh中参数小于1");
            }
            var r = Math.Log(z + Math.Pow(z * z - 1, 0.5));
            return Operand.Create(r);
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
                return ThrowError("acosh中参数小于1");
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
            var firstValue = this.Visit(context.expr(0)).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = this.Visit(context.expr(1)).ToNumber("DEGREES left value");
            if (secondValue.IsError) { return secondValue; }

            return Operand.Create(Math.Atan2(firstValue.NumberValue, secondValue.NumberValue));
        }

        #endregion

        #region rounding

        public Operand VisitROUND_fun([NotNull] mathParser.ROUND_funContext context)
        {
            var firstValue = this.Visit(context.expr(0)).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = this.Visit(context.expr(1)).ToNumber("DEGREES left value");
            if (secondValue.IsError) { return secondValue; }

            return Operand.Create((double)Math.Round((decimal)firstValue.NumberValue, secondValue.IntValue, MidpointRounding.AwayFromZero));
        }
        public Operand VisitROUNDDOWN_fun([NotNull] mathParser.ROUNDDOWN_funContext context)
        {

            var firstValue = this.Visit(context.expr(0)).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = this.Visit(context.expr(1)).ToNumber("DEGREES left value");
            if (secondValue.IsError) { return secondValue; }

            var a = Math.Pow(10, secondValue.IntValue);
            var b = firstValue.NumberValue;

            b = ((double)(int)(b * a)) / a;
            return Operand.Create(b);
        }
        public Operand VisitROUNDUP_fun([NotNull] mathParser.ROUNDUP_funContext context)
        {
            var firstValue = this.Visit(context.expr(0)).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = this.Visit(context.expr(1)).ToNumber("DEGREES left value");
            if (secondValue.IsError) { return secondValue; }

            var a = Math.Pow(10, secondValue.IntValue);
            var b = firstValue.NumberValue;

            var t = (Math.Ceiling(Math.Abs(b) * a)) / a;
            if (b > 0) return Operand.Create(t);
            return Operand.Create(-t);
        }
        public Operand VisitCEILING_fun([NotNull] mathParser.CEILING_funContext context)
        {
            var firstValue = this.Visit(context.expr(0)).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = this.Visit(context.expr(1)).ToNumber("DEGREES left value");
            if (secondValue.IsError) { return secondValue; }

            var a = firstValue.NumberValue;
            var b = secondValue.NumberValue;
            var d = Math.Ceiling(a / b) * b;
            return Operand.Create(d);
        }
        public Operand VisitFLOOR_fun([NotNull] mathParser.FLOOR_funContext context)
        {

            var firstValue = this.Visit(context.expr(0)).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = this.Visit(context.expr(1)).ToNumber("DEGREES left value");
            if (secondValue.IsError) { return secondValue; }

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
            var firstValue = this.Visit(context.expr(0)).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = this.Visit(context.expr(1)).ToNumber("DEGREES left value");
            if (secondValue.IsError) { return secondValue; }

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
            var firstValue = this.Visit(context.expr(0)).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = this.Visit(context.expr(1)).ToNumber("DEGREES left value");
            if (secondValue.IsError) { return secondValue; }

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
                return ThrowError("fact中参数小于0");
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
                return ThrowError("factdouble中参数小于0");
            }
            double d = 1;
            for (int i = z; i > 0; i -= 2) {
                d *= i;
            }
            return Operand.Create(d);
        }
        public Operand VisitPOWER_fun([NotNull] mathParser.POWER_funContext context)
        {
            var firstValue = this.Visit(context.expr(0)).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = this.Visit(context.expr(1)).ToNumber("DEGREES left value");
            if (secondValue.IsError) { return secondValue; }

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
            var arg = new List<Operand>();
            foreach (var item in context.expr()) {
                var a = this.Visit(item).ToNumber();
                if (a.IsError) { return a; }
                arg.Add(a);
            }

            if (arg.Count > 1) {
                return Operand.Create(Math.Log(arg[0].NumberValue, arg[1].NumberValue));
            }
            return Operand.Create(Math.Log(arg[0].NumberValue, 10));
        }
        public Operand VisitLOG10_fun([NotNull] mathParser.LOG10_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("DEGREES left value");
            if (firstValue.IsError) { return firstValue; }
            return Operand.Create(Math.Log(firstValue.NumberValue, 10));
        }
        public Operand VisitMULTINOMIAL_fun([NotNull] mathParser.MULTINOMIAL_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) {
                var a = this.Visit(item);
                arg.Add(a);
            }

            int sum = 0;
            int n = 1;
            foreach (var item in arg) {
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
            var arg = new List<Operand>();
            foreach (var item in context.expr()) {
                var a = this.Visit(item);
                arg.Add(a);
            }
            double d = 1;
            for (int i = 0; i < arg.Count; i++) {
                var item = arg[i];
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
            var arg = new List<Operand>();
            foreach (var item in context.expr()) {
                var a = this.Visit(item);
                arg.Add(a);
            }
            double d = 1;
            for (int i = 0; i < arg.Count; i++) {
                var item = arg[i];
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
            t = System.Text.RegularExpressions.Regex.Replace(t, @"[\f\n\r\t\v]", "");
            return Operand.Create(t);
        }
        public Operand VisitCODE_fun([NotNull] mathParser.CODE_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToString("CODE left value");
            if (firstValue.IsError) { return firstValue; }

            var t = firstValue.StringValue;
            if (t.Length == 0) {
                return ThrowError("字符串长度为0");
            }
            return Operand.Create((double)(int)(char)t[0]);
        }
        public Operand VisitCONCATENATE_fun([NotNull] mathParser.CONCATENATE_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) {
                var a = this.Visit(item).ToNumber("CONCATENATE ");
                if (a.IsError) { return a; }
                arg.Add(a);
            }

            StringBuilder sb = new StringBuilder();
            foreach (var item in arg) {
                sb.Append(item.StringValue);
            }
            return Operand.Create(sb.ToString());
        }
        public Operand VisitEXACT_fun([NotNull] mathParser.EXACT_funContext context)
        {
            var firstValue = this.Visit(context.expr(0)).ToString("EXACT left value");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = this.Visit(context.expr(1)).ToString("EXACT left value");
            if (secondValue.IsError) { return secondValue; }

            return Operand.Create(firstValue.StringValue == secondValue.StringValue);
        }
        public Operand VisitFIND_fun([NotNull] mathParser.FIND_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            var firstValue = arg[0].ToString("FIND left value");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = arg[1].ToString("FIND left value");
            if (secondValue.IsError) { return secondValue; }

            if (arg.Count == 2) {
                var p = secondValue.StringValue.IndexOf(firstValue.StringValue) + excelIndex;
                return Operand.Create(p);
            }
            var count = arg[2].ToNumber();
            if (count.IsError) { return count; }

            var p2 = secondValue.StringValue.IndexOf(firstValue.StringValue, count.IntValue) + excelIndex;
            return Operand.Create(p2);
        }
        public Operand VisitFIXED_fun([NotNull] mathParser.FIXED_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            var num = 2;
            if (arg.Count > 1) {
                arg[1] = arg[1].ToNumber();
                if (arg[1].IsError) { return arg[1]; }
                num = arg[1].IntValue;
            }
            arg[0] = arg[0].ToNumber();
            if (arg[0].IsError) { return arg[0]; }

            var s = Math.Round(arg[0].NumberValue, num);
            var no = false;
            if (arg.Count == 3) {
                arg[2] = arg[2].ToNumber();
                if (arg[2].IsError) { return arg[2]; }
                no = arg[2].BooleanValue;
            }
            if (no == false) {
                return Operand.Create(s.ToString("N" + num));
            }
            return Operand.Create(s.ToString());
        }
        public Operand VisitLEFT_fun([NotNull] mathParser.LEFT_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            var firstValue = arg[0].ToString("FIND left value");
            if (firstValue.IsError) { return firstValue; }

            if (arg.Count == 1) {
                return Operand.Create(firstValue.StringValue[0].ToString());
            }
            var secondValue = arg[1].ToNumber("FIND left value");
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
            var firstValue = this.Visit(context.expr(0)).ToString("MID left value");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = this.Visit(context.expr(1)).ToNumber("MID left value");
            if (secondValue.IsError) { return secondValue; }
            var thirdValue = this.Visit(context.expr(2)).ToNumber("MID left value");
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
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            arg[0] = arg[0].ToString("");
            if (arg.Count == 3) {
                arg[1] = arg[1].ToString("");
                arg[2] = arg[2].ToString("");
                var srcText = arg[0].StringValue;
                var old = arg[1].StringValue;
                var newstr = arg[2].StringValue;
                return Operand.Create(srcText.Replace(old, newstr));
            }

            var oldtext = arg[0].StringValue;
            arg[1] = arg[1].ToNumber("");
            arg[2] = arg[2].ToNumber("");

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
            return Operand.Create(sb.ToString());
        }
        public Operand VisitREPT_fun([NotNull] mathParser.REPT_funContext context)
        {
            var firstValue = this.Visit(context.expr(0)).ToString("MID left value");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = this.Visit(context.expr(1)).ToNumber("MID left value");
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
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            var firstValue = arg[0].ToString("FIND left value");
            if (firstValue.IsError) { return firstValue; }

            if (arg.Count == 1) {
                return Operand.Create(firstValue.StringValue[firstValue.StringValue.Length - 1].ToString());
            }
            var secondValue = arg[1].ToNumber("FIND left value");
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
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            arg[0] = arg[0].ToString();
            arg[1] = arg[1].ToString();
            if (arg.Count == 2) {
                var p = arg[1].StringValue.IndexOf(arg[0].StringValue, StringComparison.OrdinalIgnoreCase) + excelIndex;
                return Operand.Create(p);
            }
            arg[2] = arg[2].ToNumber();
            var p2 = arg[1].StringValue.IndexOf(arg[0].StringValue, arg[2].IntValue, StringComparison.OrdinalIgnoreCase) + excelIndex;
            return Operand.Create(p2);
        }
        public Operand VisitSUBSTITUTE_fun([NotNull] mathParser.SUBSTITUTE_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }
            arg[0] = arg[0].ToString();
            arg[1] = arg[1].ToString();
            arg[2] = arg[2].ToString();
            if (arg.Count == 3) {
                return Operand.Create(arg[0].StringValue.Replace(arg[1].StringValue, arg[2].StringValue));
            }

            arg[3] = arg[3].ToNumber();

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
            return Operand.Create(sb.ToString());
        }
        public Operand VisitT_fun([NotNull] mathParser.T_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToString("T left value");
            return firstValue;
        }
        public Operand VisitTEXT_fun([NotNull] mathParser.TEXT_funContext context)
        {
            var firstValue = this.Visit(context.expr(0));
            if (firstValue.IsError) { return firstValue; }
            var secondValue = this.Visit(context.expr(1)).ToString("MID left value");
            if (secondValue.IsError) { return secondValue; }

            var f = secondValue.StringValue;
            var a = firstValue;
            if (a.Type == OperandType.STRING) {
                return a;
            } else if (a.Type == OperandType.BOOLEAN) {
                return Operand.Create(a.BooleanValue.ToString());
            } else if (a.Type == OperandType.NUMBER) {
                return Operand.Create(a.NumberValue.ToString(f));
            } else if (a.Type == OperandType.DATE) {
                return Operand.Create(a.DateValue.ToString(f));
            }
            return Operand.Create(a.StringValue.ToString());
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

            if (double.TryParse(firstValue.StringValue, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"), out double d)) {
                return Operand.Create(d);
            }
            return ThrowError("无法转成数字");
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
            string d = System.Text.RegularExpressions.Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            return System.Text.RegularExpressions.Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString());
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
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToNumber(); if (a.IsError) { return a; } arg.Add(a); }

            Date d;
            if (arg.Count == 3) {
                d = new Date(arg[0].IntValue, arg[1].IntValue, arg[2].IntValue, 0, 0, 0);
            } else if (arg.Count == 4) {
                d = new Date(arg[0].IntValue, arg[1].IntValue, arg[2].IntValue, arg[3].IntValue, 0, 0);
            } else if (arg.Count == 5) {
                d = new Date(arg[0].IntValue, arg[1].IntValue, arg[2].IntValue, arg[3].IntValue, arg[4].IntValue, 0);
            } else {
                d = new Date(arg[0].IntValue, arg[1].IntValue, arg[2].IntValue, arg[3].IntValue, arg[4].IntValue, arg[5].IntValue);
            }
            return Operand.Create(d);
        }
        public Operand VisitTIME_fun([NotNull] mathParser.TIME_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToNumber(); if (a.IsError) { return a; } arg.Add(a); }

            Date d;
            if (arg.Count == 3) {
                d = new Date(0, 0, 0, arg[0].IntValue, arg[1].IntValue, arg[2].IntValue);
            } else {
                d = new Date(0, 0, 0, arg[0].IntValue, arg[1].IntValue, 0);
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
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            arg[0] = arg[0].ToDate();
            if (arg[0].IsError) { return arg[0]; }

            var type = 1;
            if (arg.Count == 2) {
                arg[1] = arg[1].ToNumber();
                if (arg[1].IsError) { return arg[1]; }
                type = arg[1].IntValue;
            }

            var t = ((DateTime)arg[0].DateValue).DayOfWeek;
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
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            arg[0] = arg[0].ToDate();
            if (arg[0].IsError) { return arg[0]; }
            arg[1] = arg[1].ToDate();
            if (arg[1].IsError) { return arg[1]; }
            arg[2] = arg[2].ToString();
            if (arg[2].IsError) { return arg[2]; }

            var startDate = (DateTime)arg[0].DateValue;
            var endDate = (DateTime)arg[1].DateValue;
            var t = arg[2].StringValue.ToLower();

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
            return ThrowError("DATE比较类型错误");
        }
        public Operand VisitDAYS360_fun([NotNull] mathParser.DAYS360_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            arg[0] = arg[0].ToDate();
            if (arg[0].IsError) { return arg[0]; }
            arg[1] = arg[1].ToDate();
            if (arg[1].IsError) { return arg[1]; }

            var startDate = (DateTime)arg[0].DateValue;
            var endDate = (DateTime)arg[1].DateValue;

            var method = false;
            if (arg.Count == 3) {
                arg[2] = arg[2].ToDate();
                if (arg[2].IsError) { return arg[2]; }
                method = arg[2].BooleanValue;
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
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            arg[0] = arg[0].ToDate();
            if (arg[0].IsError) { return arg[0]; }
            arg[1] = arg[1].ToNumber();
            if (arg[1].IsError) { return arg[1]; }

            return Operand.Create((Date)(((DateTime)arg[0].DateValue).AddMonths(arg[1].IntValue)));
        }
        public Operand VisitEOMONTH_fun([NotNull] mathParser.EOMONTH_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            arg[0] = arg[0].ToDate();
            if (arg[0].IsError) { return arg[0]; }
            arg[1] = arg[1].ToNumber();
            if (arg[1].IsError) { return arg[1]; }

            var dt = ((DateTime)arg[0].DateValue).AddMonths(arg[1].IntValue + 1);
            dt = new DateTime(dt.Year, dt.Month, 1).AddDays(-1);
            return Operand.Create(dt);
        }
        public Operand VisitNETWORKDAYS_fun([NotNull] mathParser.NETWORKDAYS_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToDate(); if (a.IsError) { return a; } arg.Add(a); }

            var startDate = (DateTime)arg[0].DateValue;
            var endDate = (DateTime)arg[1].DateValue;

            List<DateTime> list = new List<DateTime>();
            for (int i = 2; i < arg.Count; i++) {
                list.Add(arg[i].DateValue);
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
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            arg[0] = arg[0].ToDate();
            if (arg[0].IsError) { return arg[0]; }
            arg[1] = arg[1].ToNumber();
            if (arg[1].IsError) { return arg[1]; }


            var startDate = (DateTime)arg[0].DateValue;
            var days = arg[1].IntValue;
            List<DateTime> list = new List<DateTime>();
            for (int i = 2; i < arg.Count; i++) {
                arg[i] = arg[i].ToDate();
                if (arg[i].IsError) { return arg[i]; }
                list.Add(arg[i].DateValue);
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
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            arg[0] = arg[0].ToDate();
            if (arg[0].IsError) { return arg[0]; }

            var startDate = (DateTime)arg[0].DateValue;

            var days = startDate.DayOfYear + (int)(new DateTime(startDate.Year, 1, 1).DayOfWeek);
            if (arg.Count == 2) {
                arg[1] = arg[1].ToNumber();
                if (arg[1].IsError) { return arg[1]; }
                if (arg[1].IntValue == 2) {
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
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToNumber(); if (a.IsError) { return a; } arg.Add(a); }

            var max = double.MinValue;

            foreach (var item in arg) {
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
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToNumber(); if (a.IsError) { return a; } arg.Add(a); }

            List<int> list = new List<int>();
            foreach (var item in arg) {
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
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToNumber(); if (a.IsError) { return a; } arg.Add(a); }

            var min = double.MaxValue;

            foreach (var item in arg) {
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
            var firstValue = this.Visit(context.expr(0)).ToArray();
            if (firstValue.IsError) { return firstValue; }
            var secondValue = this.Visit(context.expr(1)).ToNumber("MID left value");
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
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item).ToNumber(); if (a.IsError) { return a; } arg.Add(a); }

            Dictionary<double, int> dict = new Dictionary<double, int>();
            foreach (var item in arg) {
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
            var firstValue = this.Visit(context.expr(0)).ToArray();
            if (firstValue.IsError) { return firstValue; }
            var secondValue = this.Visit(context.expr(1)).ToNumber("MID left value");
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
            var firstValue = this.Visit(context.expr(0)).ToArray();
            if (firstValue.IsError) { return firstValue; }
            var secondValue = this.Visit(context.expr(1)).ToNumber("MID left value");
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
            var firstValue = this.Visit(context.expr(0)).ToArray();
            if (firstValue.IsError) { return firstValue; }
            var secondValue = this.Visit(context.expr(1)).ToNumber("MID left value");
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
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            var firstValue = arg[0].ToArray();
            if (firstValue.IsError) { return firstValue; }
            var secondValue = arg[1].ToNumber("MID left value");
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
            if (arg.Count == 3) {
                var thirdValue = this.Visit(context.expr(2)).ToNumber("MID left value");
                if (thirdValue.IsError) { return thirdValue; }
                d = thirdValue.IntValue;
            }
            return Operand.Create(Math.Round(v, d));
        }
        public Operand VisitAVERAGE_fun([NotNull] mathParser.AVERAGE_funContext context)
        {
            var firstValue = this.Visit(context.expr(0)).ToArray();
            if (firstValue.IsError) { return firstValue; }

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
            return Operand.Create(list.Average());
        }
        public Operand VisitAVERAGEIF_fun([NotNull] mathParser.AVERAGEIF_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            List<double> list = arg[0].GetNumberList();
            List<double> sumdbs = list;
            if (arg.Count == 3) sumdbs = arg[2].GetNumberList();

            double sum;
            int count;
            if (arg[1].Type == OperandType.NUMBER) {
                count = F_base_countif(list, arg[1].NumberValue);
                sum = count * arg[1].NumberValue;
            } else {
                if (double.TryParse(arg[1].StringValue.Trim(), NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"), out double d)) {
                    count = F_base_countif(list, arg[1].NumberValue);
                    sum = F_base_sumif(list, "=" + arg[1].StringValue.Trim(), sumdbs);
                } else {
                    count = F_base_countif(list, arg[1].StringValue.Trim());
                    sum = F_base_sumif(list, arg[1].StringValue.Trim(), sumdbs);
                }
            }
            return Operand.Create(sum / count);
        }
        public Operand VisitGEOMEAN_fun([NotNull] mathParser.GEOMEAN_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            if (arg.Count == 1) return arg[0];
            var dbs = F_base_GetList(arg);
            double sum = 1;
            foreach (var db in dbs) {
                sum *= db;
            }
            return Operand.Create(Math.Pow(sum, 1.0 / dbs.Count));
        }
        public Operand VisitHARMEAN_fun([NotNull] mathParser.HARMEAN_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            if (arg.Count == 1) return arg[0];
            var dbs = F_base_GetList(arg);
            double sum = 0;
            foreach (var db in dbs) {
                sum += 1 / db;
            }
            return Operand.Create(dbs.Count / sum);
        }
        public Operand VisitCOUNT_fun([NotNull] mathParser.COUNT_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            List<double> list = F_base_GetList(arg);
            return Operand.Create(list.Count);
        }
        public Operand VisitCOUNTIF_fun([NotNull] mathParser.COUNTIF_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            var dbs = arg[0].GetNumberList();
            int count = 0;
            if (arg[1].Type == OperandType.NUMBER) {
                count = F_base_countif(dbs, arg[1].NumberValue);
            } else {
                if (double.TryParse(arg[1].StringValue.Trim(), NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"), out double d)) {
                    count = F_base_countif(dbs, arg[1].NumberValue);
                } else {
                    count = F_base_countif(dbs, arg[1].StringValue.Trim());
                }
            }
            return Operand.Create(count);
        }
        public Operand VisitSUM_fun([NotNull] mathParser.SUM_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            if (arg.Count == 1) return arg[0];
            var dbs = F_base_GetList(arg);
            double sum = 0;
            foreach (var db in dbs) {
                sum += db;
            }
            return Operand.Create(sum);
        }
        public Operand VisitSUMIF_fun([NotNull] mathParser.SUMIF_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            var dbs = arg[0].GetNumberList();
            var sumdbs = dbs;
            if (arg.Count == 3) sumdbs = arg[2].GetNumberList();
            double sum;
            if (arg[1].Type == OperandType.NUMBER) {
                sum = F_base_countif(dbs, arg[1].NumberValue) * arg[1].NumberValue;
            } else {
                if (double.TryParse(arg[1].StringValue.Trim(), NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"), out _)) {
                    sum = F_base_sumif(dbs, "=" + arg[1].StringValue.Trim(), sumdbs);
                } else {
                    sum = F_base_sumif(dbs, arg[1].StringValue.Trim(), sumdbs);
                }
            }
            return Operand.Create(sum);
        }
        public Operand VisitAVEDEV_fun([NotNull] mathParser.AVEDEV_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            List<double> list = F_base_GetList(arg);
            double avg = list.Average();
            double sum = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += Math.Abs(list[i] - avg);
            }
            return Operand.Create(sum / list.Count);
        }
        public Operand VisitSTDEV_fun([NotNull] mathParser.STDEV_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }

            List<double> list = F_base_GetList(arg);
            double avg = list.Average();
            double sum = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += (list[i] - avg) * (list[i] - avg);
            }
            return Operand.Create(Math.Sqrt(sum / (list.Count - 1)));
        }
        public Operand VisitSTDEVP_fun([NotNull] mathParser.STDEVP_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { var a = this.Visit(item); if (a.IsError) { return a; } arg.Add(a); }


            List<double> list = F_base_GetList(arg);
            double sum = 0;
            double avg = list.Average();

            for (int i = 0; i < list.Count; i++) {
                sum += (list[i] - avg) * (list[i] - avg);
            }
            return Operand.Create(Math.Sqrt(sum / (list.Count)));
        }
        public Operand VisitDEVSQ_fun([NotNull] mathParser.DEVSQ_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitVAR_fun([NotNull] mathParser.VAR_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitVARP_fun([NotNull] mathParser.VARP_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitNORMDIST_fun([NotNull] mathParser.NORMDIST_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitNORMINV_fun([NotNull] mathParser.NORMINV_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitNORMSDIST_fun([NotNull] mathParser.NORMSDIST_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitNORMSINV_fun([NotNull] mathParser.NORMSINV_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitBETADIST_fun([NotNull] mathParser.BETADIST_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitBETAINV_fun([NotNull] mathParser.BETAINV_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitBINOMDIST_fun([NotNull] mathParser.BINOMDIST_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitEXPONDIST_fun([NotNull] mathParser.EXPONDIST_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitFDIST_fun([NotNull] mathParser.FDIST_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitFINV_fun([NotNull] mathParser.FINV_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitFISHER_fun([NotNull] mathParser.FISHER_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitFISHERINV_fun([NotNull] mathParser.FISHERINV_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitGAMMADIST_fun([NotNull] mathParser.GAMMADIST_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitGAMMAINV_fun([NotNull] mathParser.GAMMAINV_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitGAMMALN_fun([NotNull] mathParser.GAMMALN_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitHYPGEOMDIST_fun([NotNull] mathParser.HYPGEOMDIST_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitLOGINV_fun([NotNull] mathParser.LOGINV_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitLOGNORMDIST_fun([NotNull] mathParser.LOGNORMDIST_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitNEGBINOMDIST_fun([NotNull] mathParser.NEGBINOMDIST_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitPOISSON_fun([NotNull] mathParser.POISSON_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitTDIST_fun([NotNull] mathParser.TDIST_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitTINV_fun([NotNull] mathParser.TINV_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitWEIBULL_fun([NotNull] mathParser.WEIBULL_funContext context)
        {
            throw new NotImplementedException();
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
        private List<double> F_base_GetList(List<Operand> arg)
        {
            List<double> list = new List<double>();
            foreach (var item in arg) {
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
            throw new NotImplementedException();
        }
        public Operand VisitURLDECODE_fun([NotNull] mathParser.URLDECODE_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitHTMLENCODE_fun([NotNull] mathParser.HTMLENCODE_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitHTMLDECODE_fun([NotNull] mathParser.HTMLDECODE_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitBASE64TOTEXT_fun([NotNull] mathParser.BASE64TOTEXT_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitBASE64URLTOTEXT_fun([NotNull] mathParser.BASE64URLTOTEXT_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitTEXTTOBASE64_fun([NotNull] mathParser.TEXTTOBASE64_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitTEXTTOBASE64URL_fun([NotNull] mathParser.TEXTTOBASE64URL_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitREGEX_fun([NotNull] mathParser.REGEX_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitREGEXREPALCE_fun([NotNull] mathParser.REGEXREPALCE_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitISREGEX_fun([NotNull] mathParser.ISREGEX_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitISMATCH_fun([NotNull] mathParser.ISMATCH_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitGUID_fun([NotNull] mathParser.GUID_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitMD5_fun([NotNull] mathParser.MD5_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitSHA1_fun([NotNull] mathParser.SHA1_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitSHA256_fun([NotNull] mathParser.SHA256_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitSHA512_fun([NotNull] mathParser.SHA512_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitCRC8_fun([NotNull] mathParser.CRC8_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitCRC16_fun([NotNull] mathParser.CRC16_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitCRC32_fun([NotNull] mathParser.CRC32_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitHMACMD5_fun([NotNull] mathParser.HMACMD5_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitHMACSHA1_fun([NotNull] mathParser.HMACSHA1_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitHMACSHA256_fun([NotNull] mathParser.HMACSHA256_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitHMACSHA512_fun([NotNull] mathParser.HMACSHA512_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitTRIMSTART_fun([NotNull] mathParser.TRIMSTART_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitLTRIM_fun([NotNull] mathParser.LTRIM_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitTRIMEND_fun([NotNull] mathParser.TRIMEND_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitRTRIM_fun([NotNull] mathParser.RTRIM_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitINDEXOF_fun([NotNull] mathParser.INDEXOF_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitLASTINDEXOF_fun([NotNull] mathParser.LASTINDEXOF_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitSPLIT_fun([NotNull] mathParser.SPLIT_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitJOIN_fun([NotNull] mathParser.JOIN_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitSUBSTRING_fun([NotNull] mathParser.SUBSTRING_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitSTARTSWITH_fun([NotNull] mathParser.STARTSWITH_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitENDSWITH_fun([NotNull] mathParser.ENDSWITH_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitISNULLOREMPTY_fun([NotNull] mathParser.ISNULLOREMPTY_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitISNULLORWHITESPACE_fun([NotNull] mathParser.ISNULLORWHITESPACE_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitTOUPPER_fun([NotNull] mathParser.TOUPPER_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitTOLOWER_fun([NotNull] mathParser.TOLOWER_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitREMOVESTART_fun([NotNull] mathParser.REMOVESTART_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitREMOVEEND_fun([NotNull] mathParser.REMOVEEND_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitREMOVEBOTH_fun([NotNull] mathParser.REMOVEBOTH_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitJSON_fun([NotNull] mathParser.JSON_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitTRYJSON_fun([NotNull] mathParser.TRYJSON_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitP_fun([NotNull] mathParser.P_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitGetJsonValue_fun([NotNull] mathParser.GetJsonValue_funContext context)
        {

            throw new NotImplementedException();
        }

        public Operand VisitParameter([NotNull] mathParser.ParameterContext context)
        {

            throw new NotImplementedException();
        }

        #endregion

    }
}
