using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

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
            foreach (var item in context.expr()) { arg.Add(this.Visit(item)); }
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
            foreach (var item in context.expr()) { arg.Add(this.Visit(item)); }

            var a = arg[0].ToBoolean("");
            if (a.IsError) { return a; }

            if (a.BooleanValue) {
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
            foreach (var item in context.expr()) { arg.Add(this.Visit(item)); }

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
            foreach (var item in context.expr()) { arg.Add(this.Visit(item)); }

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
            foreach (var item in context.expr()) { arg.Add(this.Visit(item)); }

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
            foreach (var item in context.expr()) { arg.Add(this.Visit(item)); }

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
            foreach (var item in context.expr()) { arg.Add(this.Visit(item)); }

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
            foreach (var item in context.expr()) { arg.Add(this.Visit(item)); }

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
            foreach (var item in context.expr()) { arg.Add(this.Visit(item)); }

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
            foreach (var item in context.expr()) { arg.Add(this.Visit(item)); }

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




            throw new NotImplementedException();
        }
        public Operand VisitREPLACE_fun([NotNull] mathParser.REPLACE_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitREPT_fun([NotNull] mathParser.REPT_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitRIGHT_fun([NotNull] mathParser.RIGHT_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitRMB_fun([NotNull] mathParser.RMB_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitSEARCH_fun([NotNull] mathParser.SEARCH_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitSUBSTITUTE_fun([NotNull] mathParser.SUBSTITUTE_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitT_fun([NotNull] mathParser.T_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitTEXT_fun([NotNull] mathParser.TEXT_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitTRIM_fun([NotNull] mathParser.TRIM_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitUPPER_fun([NotNull] mathParser.UPPER_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitVALUE_fun([NotNull] mathParser.VALUE_funContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 转成全角
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 转成半角
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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
        #endregion

        #region date time

        public Operand VisitDATEVALUE_fun([NotNull] mathParser.DATEVALUE_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitTIMEVALUE_fun([NotNull] mathParser.TIMEVALUE_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitDATE_fun([NotNull] mathParser.DATE_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitTIME_fun([NotNull] mathParser.TIME_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitNOW_fun([NotNull] mathParser.NOW_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitTODAY_fun([NotNull] mathParser.TODAY_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitYEAR_fun([NotNull] mathParser.YEAR_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitMONTH_fun([NotNull] mathParser.MONTH_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitDAY_fun([NotNull] mathParser.DAY_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitHOUR_fun([NotNull] mathParser.HOUR_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitMINUTE_fun([NotNull] mathParser.MINUTE_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitSECOND_fun([NotNull] mathParser.SECOND_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitWEEKDAY_fun([NotNull] mathParser.WEEKDAY_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitDATEDIF_fun([NotNull] mathParser.DATEDIF_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitDAYS360_fun([NotNull] mathParser.DAYS360_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitEDATE_fun([NotNull] mathParser.EDATE_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitEOMONTH_fun([NotNull] mathParser.EOMONTH_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitNETWORKDAYS_fun([NotNull] mathParser.NETWORKDAYS_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitWORKDAY_fun([NotNull] mathParser.WORKDAY_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitWEEKNUM_fun([NotNull] mathParser.WEEKNUM_funContext context)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region sum

        public Operand VisitMAX_fun([NotNull] mathParser.MAX_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitMEDIAN_fun([NotNull] mathParser.MEDIAN_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitMIN_fun([NotNull] mathParser.MIN_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitQUARTILE_fun([NotNull] mathParser.QUARTILE_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitMODE_fun([NotNull] mathParser.MODE_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitLARGE_fun([NotNull] mathParser.LARGE_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitSMALL_fun([NotNull] mathParser.SMALL_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitPERCENTILE_fun([NotNull] mathParser.PERCENTILE_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitPERCENTRANK_fun([NotNull] mathParser.PERCENTRANK_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitAVERAGE_fun([NotNull] mathParser.AVERAGE_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitAVERAGEIF_fun([NotNull] mathParser.AVERAGEIF_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitGEOMEAN_fun([NotNull] mathParser.GEOMEAN_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitHARMEAN_fun([NotNull] mathParser.HARMEAN_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitCOUNT_fun([NotNull] mathParser.COUNT_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitCOUNTIF_fun([NotNull] mathParser.COUNTIF_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitSUM_fun([NotNull] mathParser.SUM_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitSUMIF_fun([NotNull] mathParser.SUMIF_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitAVEDEV_fun([NotNull] mathParser.AVEDEV_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitSTDEV_fun([NotNull] mathParser.STDEV_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitSTDEVP_fun([NotNull] mathParser.STDEVP_funContext context)
        {
            throw new NotImplementedException();
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
