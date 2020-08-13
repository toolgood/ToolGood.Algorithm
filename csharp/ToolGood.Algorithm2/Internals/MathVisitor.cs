using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Antlr4.Runtime.Tree;
using ToolGood.Algorithm.Internals;
using ToolGood.Algorithm.LitJson;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm
{
    class MathVisitor : AbstractParseTreeVisitor<Operand>, ImathVisitor<Operand>
    {
        private static readonly Regex sumifRegex = new Regex(@"(<|<=|>|>=|=|==|!=|<>) *([-+]?\d+(\.(\d+)?)?)", RegexOptions.Compiled);
        private static readonly Regex bit_2 = new Regex("^[01]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex bit_8 = new Regex("^[0-8]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex bit_16 = new Regex("^[0-9a-f]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex clearRegex = new Regex(@"[\f\n\r\t\v]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex numberRegex = new Regex(@"^-?(0|[1-9])\d*(\.\d+)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en-US");
        public event Func<string, Operand> GetParameter;
        public event Func<string, List<Operand>, Operand> DiyFunction;
        public int excelIndex;

        #region base

        public Operand VisitProg(mathParser.ProgContext context)
        {
            return VisitChildren(context);
        }

        public Operand VisitMulDiv_fun(mathParser.MulDiv_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];
            var t = context.op.Text;
            if (firstValue.Type == OperandType.STRING) {
                if (numberRegex.IsMatch(firstValue.TextValue)) {
                    var a = firstValue.ToNumber(null);
                    if (a.IsError == false) firstValue = a;
                } else {
                    var a = firstValue.ToDate(null);
                    if (a.IsError == false) firstValue = a;
                }
            }
            if (secondValue.Type == OperandType.STRING) {
                if (numberRegex.IsMatch(secondValue.TextValue)) {
                    var a = secondValue.ToNumber(null);
                    if (a.IsError == false) secondValue = a;
                } else {
                    var a = secondValue.ToDate(null);
                    if (a.IsError == false) secondValue = a;
                }
            }
            if (t == "*") {
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
                if (firstValue.Type == OperandType.DATE) {
                    secondValue = secondValue.ToNumber($"Function '{t}' parameter 2 is error!");
                    if (secondValue.IsError) { return secondValue; }
                    return Operand.Create((Date)(firstValue.DateValue * secondValue.NumberValue));
                }
                if (secondValue.Type == OperandType.DATE) {
                    firstValue = firstValue.ToNumber($"Function '{t}' parameter 1 is error!");
                    if (firstValue.IsError) { return firstValue; }
                    return Operand.Create((Date)(secondValue.DateValue * firstValue.NumberValue));
                }

                firstValue = firstValue.ToNumber($"Function '{t}' parameter 1 is error!");
                if (firstValue.IsError) { return firstValue; }
                secondValue = secondValue.ToNumber($"Function '{t}' parameter 2 is error!");
                if (secondValue.IsError) { return secondValue; }
                return Operand.Create(firstValue.NumberValue * secondValue.NumberValue);
            } else if (t == "/") {
                if (firstValue.Type == OperandType.DATE) {
                    return Operand.Create(firstValue.DateValue / secondValue.NumberValue);
                }

                firstValue = firstValue.ToNumber($"Function '{t}' parameter 1 is error!");
                if (firstValue.IsError) { return firstValue; }
                secondValue = secondValue.ToNumber("Div fun right value");
                if (secondValue.IsError) { return secondValue; }
                if (secondValue.NumberValue == 0) {
                    return Operand.Error($"Function '{t}' parameter 2 is error!");
                }
                return Operand.Create(firstValue.NumberValue / secondValue.NumberValue);
            } else if (t == "%") {
                firstValue = firstValue.ToNumber("% fun right value");
                if (firstValue.IsError) { return firstValue; }
                secondValue = secondValue.ToNumber("% fun right value");
                if (secondValue.IsError) { return secondValue; }
                if (secondValue.NumberValue == 0) {
                    return Operand.Error($"Div 0 is error!");
                }
                return Operand.Create(firstValue.NumberValue % secondValue.NumberValue);
            }
            return Operand.Error($"Function '{t}' parameter is error!");
        }

        public Operand VisitAddSub_fun(mathParser.AddSub_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];
            var t = context.op.Text;

            if (t == "&") {
                if (firstValue.IsNull && secondValue.IsNull) {
                    return firstValue;
                } else if (firstValue.IsNull) {
                    secondValue = secondValue.ToText($"Function '{t}' parameter 2 is error!");
                    return secondValue;
                } else if (secondValue.IsNull) {
                    firstValue = firstValue.ToText($"Function '{t}' parameter 1 is error!");
                    return firstValue;
                }

                firstValue = firstValue.ToText($"Function '{t}' parameter 1 is error!");
                if (firstValue.IsError) { return firstValue; }
                secondValue = secondValue.ToText($"Function '{t}' parameter 2 is error!");
                if (secondValue.IsError) { return secondValue; }
                return Operand.Create(firstValue.TextValue + secondValue.TextValue);
            }
            if (firstValue.Type == OperandType.STRING) {
                if (numberRegex.IsMatch(firstValue.TextValue)) {
                    var a = firstValue.ToNumber(null);
                    if (a.IsError == false) firstValue = a;
                } else {
                    var a = firstValue.ToDate(null);
                    if (a.IsError == false) firstValue = a;
                }
            }
            if (secondValue.Type == OperandType.STRING) {
                if (numberRegex.IsMatch(secondValue.TextValue)) {
                    var a = secondValue.ToNumber(null);
                    if (a.IsError == false) secondValue = a;
                } else {
                    var a = secondValue.ToDate(null);
                    if (a.IsError == false) secondValue = a;
                }
            }
            if (t == "+") {
                if (firstValue.Type == OperandType.DATE && secondValue.Type == OperandType.DATE) {
                    return Operand.Create(firstValue.DateValue + secondValue.DateValue);
                } else if (firstValue.Type == OperandType.DATE) {
                    secondValue = secondValue.ToNumber($"Function '{t}' parameter 2 is error!");
                    if (secondValue.IsError) { return secondValue; }
                    return Operand.Create(firstValue.DateValue + secondValue.NumberValue);
                } else if (secondValue.Type == OperandType.DATE) {
                    firstValue = firstValue.ToNumber($"Function '{t}' parameter 1 is error!");
                    if (firstValue.IsError) { return firstValue; }
                    return Operand.Create(secondValue.DateValue + firstValue.NumberValue);
                }
                firstValue = firstValue.ToNumber($"Function '{t}' parameter 1 is error!");
                if (firstValue.IsError) { return firstValue; }
                secondValue = secondValue.ToNumber($"Function '{t}' parameter 2 is error!");
                if (secondValue.IsError) { return secondValue; }
                return Operand.Create(firstValue.NumberValue + secondValue.NumberValue);
            } else if (t == "-") {
                if (firstValue.Type == OperandType.DATE && secondValue.Type == OperandType.DATE) {
                    return Operand.Create(firstValue.DateValue - secondValue.DateValue);
                } else if (firstValue.Type == OperandType.DATE) {
                    secondValue = secondValue.ToNumber($"Function '{t}' parameter 2 is error!");
                    if (secondValue.IsError) { return secondValue; }
                    return Operand.Create(firstValue.DateValue - secondValue.NumberValue);
                } else if (secondValue.Type == OperandType.DATE) {
                    firstValue = firstValue.ToNumber($"Function '{t}' parameter 1 is error!");
                    if (firstValue.IsError) { return firstValue; }
                    return Operand.Create(secondValue.DateValue - firstValue.NumberValue);
                }
                firstValue = firstValue.ToNumber(null);
                if (firstValue.IsError) { return firstValue; }
                secondValue = secondValue.ToNumber($"Function '{t}' parameter 2 is error!");
                if (secondValue.IsError) { return secondValue; }
                return Operand.Create(firstValue.NumberValue - secondValue.NumberValue);
            }
            return Operand.Error($"Function '{t}' parameter is error!");
        }

        public Operand VisitJudge_fun(mathParser.Judge_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];
            string type = context.op.Text;

            if (firstValue.IsNull) {
                if (secondValue.IsNull && (type == "==" || type == "=")) {
                    return Operand.True;
                } else if (secondValue.IsNull == false && (type == "<>" || type == "!=")) {
                    return Operand.True;
                }
                return Operand.False;
            } else if (secondValue.IsNull) {
                if (type == "==" || type == "=") {
                    return Operand.False;
                }
                return Operand.True;
            }

            int r;
            if (firstValue.Type == secondValue.Type) {
                if (firstValue.Type == OperandType.STRING || firstValue.Type == OperandType.JSON) {
                    firstValue = firstValue.ToText($"Function '{type}' parameter 1 is error!");
                    if (firstValue.IsError) { return firstValue; }
                    secondValue = secondValue.ToText($"Function '{type}' parameter 2 is error!");
                    if (secondValue.IsError) { return secondValue; }

                    r = string.CompareOrdinal(firstValue.TextValue, secondValue.TextValue);
                } else if (firstValue.Type == OperandType.ARRARY) {
                    return Operand.Error("两个类型无法比较");
                } else {
                    firstValue = firstValue.ToNumber($"Function '{type}' parameter 1 is error!");
                    if (firstValue.IsError) { return firstValue; }
                    secondValue = secondValue.ToNumber($"Function '{type}' parameter 2 is error!");
                    if (secondValue.IsError) { return secondValue; }
                    r = Compare(firstValue.NumberValue, secondValue.NumberValue);
                }
            } else if ((firstValue.Type == OperandType.DATE && secondValue.Type == OperandType.STRING) || (secondValue.Type == OperandType.DATE && firstValue.Type == OperandType.STRING)
                 || (firstValue.Type == OperandType.NUMBER && secondValue.Type == OperandType.STRING) || (secondValue.Type == OperandType.NUMBER && firstValue.Type == OperandType.STRING)
                  ) {
                firstValue = firstValue.ToText($"Function '{type}' parameter 1 is error!");
                if (firstValue.IsError) { return firstValue; }
                secondValue = secondValue.ToText($"Function '{type}' parameter 2 is error!");
                if (secondValue.IsError) { return secondValue; }

                r = string.CompareOrdinal(firstValue.TextValue, secondValue.TextValue);
            } else if ((firstValue.Type == OperandType.BOOLEAN && secondValue.Type == OperandType.STRING) || (secondValue.Type == OperandType.BOOLEAN && firstValue.Type == OperandType.STRING)) {
                firstValue = firstValue.ToText($"Function '{type}' parameter 1 is error!");
                if (firstValue.IsError) { return firstValue; }
                secondValue = secondValue.ToText($"Function '{type}' parameter 2 is error!");
                if (secondValue.IsError) { return secondValue; }

                r = string.Compare(firstValue.TextValue, secondValue.TextValue, true);
            } else if (firstValue.Type == OperandType.STRING || secondValue.Type == OperandType.STRING
                  || firstValue.Type == OperandType.JSON || secondValue.Type == OperandType.JSON
                  || firstValue.Type == OperandType.ARRARY || secondValue.Type == OperandType.ARRARY
                  ) {
                return Operand.Error("两个类型无法比较");
            } else {
                firstValue = firstValue.ToNumber($"Function '{type}' parameter 1 is error!");
                if (firstValue.IsError) { return firstValue; }
                secondValue = secondValue.ToNumber($"Function '{type}' parameter 2 is error!");
                if (secondValue.IsError) { return secondValue; }

                r = Compare(firstValue.NumberValue, secondValue.NumberValue);
            }
            if (type == "<") {
                return Operand.Create(r == -1);
            } else if (type == "<=") {
                return Operand.Create(r <= 0);
            } else if (type == ">") {
                return Operand.Create(r == 1);
            } else if (type == ">=") {
                return Operand.Create(r >= 0);
            } else if (type == "=" || type == "==") {
                return Operand.Create(r == 0);
            }
            return Operand.Create(r != 0);
        }

        private int Compare(double t1, double t2)
        {
            var b = Math.Round(t1 - t2, 12, MidpointRounding.AwayFromZero);
            if (b == 0) {
                return 0;
            } else if (b > 0) {
                return 1;
            }
            return -1;
        }

        public Operand VisitAndOr_fun(mathParser.AndOr_funContext context)
        {
            var t = context.op.Text;
            var args = new List<Operand>();
            var index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToBoolean($"Function '{t}' parameter {index++} is error!"); ; if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];
            if (t == "&&" || t.Equals("and", StringComparison.OrdinalIgnoreCase)) {
                if (firstValue.BooleanValue && secondValue.BooleanValue) {
                    return Operand.True;
                }
                return Operand.False;
            }
            if (firstValue.BooleanValue || secondValue.BooleanValue) {
                return Operand.True;
            }
            return Operand.False;
        }


        #endregion

        #region flow
        public Operand VisitIF_fun(mathParser.IF_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var b = args[0].ToBoolean("Function IF first parameter is error!");
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
        public Operand VisitIFERROR_fun(mathParser.IFERROR_funContext context)
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
        public Operand VisitISNUMBER_fun(mathParser.ISNUMBER_funContext context)
        {
            var firstValue = this.Visit(context.expr());
            if (firstValue.IsError) { return firstValue; }

            if (firstValue.Type == OperandType.NUMBER) {
                return Operand.True;
            }
            return Operand.False;
        }
        public Operand VisitISTEXT_fun(mathParser.ISTEXT_funContext context)
        {
            var firstValue = this.Visit(context.expr());
            if (firstValue.IsError) { return firstValue; }

            if (firstValue.Type == OperandType.STRING) {
                return Operand.True;
            }
            return Operand.False;
        }
        public Operand VisitISERROR_fun(mathParser.ISERROR_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); args.Add(aa); }
            if (args.Count == 2) {
                if (args[0].IsError) {
                    return args[1];
                }
                return args[0];
            }

            if (args[0].IsError) {
                return Operand.True;
            }
            return Operand.False;
        }

        public Operand VisitISNULL_fun(mathParser.ISNULL_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); args.Add(aa); }

            if (args.Count == 2) {
                if (args[0].IsNull) {
                    return args[1];
                }
                return args[0];
            }
            if (args[0].IsNull) {
                return Operand.True;
            }
            return Operand.False;
        }

        public Operand VisitISNULLORERROR_fun(mathParser.ISNULLORERROR_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); args.Add(aa); }

            if (args.Count == 2) {
                if (args[0].IsNull || args[0].IsError) {
                    return args[1];
                }
                return args[0];
            }
            if (args[0].IsNull || args[0].IsError) {
                return Operand.True;
            }
            return Operand.False;
        }

        public Operand VisitISEVEN_fun(mathParser.ISEVEN_funContext context)
        {
            var firstValue = this.Visit(context.expr());
            if (firstValue.Type == OperandType.NUMBER) {
                if (firstValue.IntValue % 2 == 0) {
                    return Operand.True;
                }
            }
            return Operand.False;
        }

        public Operand VisitISLOGICAL_fun(mathParser.ISLOGICAL_funContext context)
        {
            var firstValue = this.Visit(context.expr());
            if (firstValue.Type == OperandType.BOOLEAN) {
                return Operand.True;
            }
            return Operand.False;
        }

        public Operand VisitISODD_fun(mathParser.ISODD_funContext context)
        {
            var firstValue = this.Visit(context.expr());
            if (firstValue.Type == OperandType.NUMBER) {
                if (firstValue.IntValue % 2 == 1) {
                    return Operand.True;
                }
            }
            return Operand.False;
        }

        public Operand VisitISNONTEXT_fun(mathParser.ISNONTEXT_funContext context)
        {
            var firstValue = this.Visit(context.expr());
            if (firstValue.Type != OperandType.STRING) {
                return Operand.True;
            }
            return Operand.False;
        }

        public Operand VisitAND_fun(mathParser.AND_funContext context)
        {
            var args = new List<Operand>();
            var index = 1;
            foreach (var item in context.expr()) {
                var a = this.Visit(item).ToBoolean($"Function AND parameter {index++} is error!");
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
        public Operand VisitOR_fun(mathParser.OR_funContext context)
        {
            var args = new List<Operand>();
            var index = 1;
            foreach (var item in context.expr()) {
                var a = this.Visit(item).ToBoolean($"Function OR parameter {index++} is error!");
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
        public Operand VisitNOT_fun(mathParser.NOT_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToBoolean("Function NOT parameter is error!");
            if (firstValue.IsError) { return firstValue; }
            return Operand.Create(!firstValue.BooleanValue);
        }
        public Operand VisitTRUE_fun(mathParser.TRUE_funContext context)
        {
            return Operand.True;
        }
        public Operand VisitFALSE_fun(mathParser.FALSE_funContext context)
        {
            return Operand.False;
        }
        #endregion

        #region math

        #region base

        public Operand VisitE_fun(mathParser.E_funContext context)
        {
            return Operand.Create(Math.E);
        }
        public Operand VisitPI_fun(mathParser.PI_funContext context)
        {
            return Operand.Create(Math.PI);
        }

        public Operand VisitABS_fun(mathParser.ABS_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function ABS parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(Math.Abs(firstValue.NumberValue));
        }
        public Operand VisitQUOTIENT_fun(mathParser.QUOTIENT_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function QUOTIENT parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];

            if (secondValue.NumberValue == 0) {
                return Operand.Error("Function QUOTIENT div 0 error!");
            }
            return Operand.Create((double)(int)(firstValue.NumberValue / secondValue.NumberValue));
        }
        public Operand VisitMOD_fun(mathParser.MOD_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function MOD parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];

            if (secondValue.NumberValue == 0) {
                return Operand.Error("Function MOD div 0 error!");
            }
            return Operand.Create((int)(firstValue.NumberValue % secondValue.NumberValue));

        }
        public Operand VisitSIGN_fun(mathParser.SIGN_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function SIGN parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(Math.Sign(firstValue.NumberValue));
        }
        public Operand VisitSQRT_fun(mathParser.SQRT_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function SQRT parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(Math.Sqrt(firstValue.NumberValue));
        }
        public Operand VisitTRUNC_fun(mathParser.TRUNC_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function TRUNC parameter is error!");
            if (firstValue.IsError) { return firstValue; }
            return Operand.Create((int)(firstValue.NumberValue));
        }
        public Operand VisitINT_fun(mathParser.INT_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function INT parameter is error!");
            if (firstValue.IsError) { return firstValue; }
            return Operand.Create((int)(firstValue.NumberValue));
        }
        public Operand VisitGCD_fun(mathParser.GCD_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            List<double> list = new List<double>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function GCD parameter is error!"); }

            return Operand.Create(F_base_gcd(list));
        }
        public Operand VisitLCM_fun(mathParser.LCM_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            List<double> list = new List<double>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function GCD parameter is error!"); }

            return Operand.Create(F_base_lgm(list));
        }
        public Operand VisitCOMBIN_fun(mathParser.COMBIN_funContext context)
        {
            var args = new List<Operand>(); var index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function COMBIN parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

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
        public Operand VisitPERMUT_fun(mathParser.PERMUT_funContext context)
        {
            var args = new List<Operand>(); var index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function PERMUT parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

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

        private int F_base_gcd(List<double> list)
        {
            list = list.OrderBy(q => q).ToList();
            var g = F_base_gcd((int)list[1], (int)list[0]);
            for (int i = 2; i < list.Count; i++) {
                g = F_base_gcd((int)list[i], g);
            }
            return g;
        }
        private int F_base_gcd(int a, int b)
        {
            if (b == 1) { return 1; }
            if (b == 0) { return a; }
            return F_base_gcd(b, a % b);
        }
        private int F_base_lgm(List<double> list)
        {
            list = list.OrderBy(q => q).ToList();
            list.RemoveAll(q => q <= 1);

            int a = (int)list[0];
            for (int i = 1; i < list.Count; i++) {
                int b = (int)list[i];
                int g = b > a ? F_base_gcd(b, a) : F_base_gcd(a, b);
                a = a / g * b;
            }
            return a;
        }
        #endregion

        #region trigonometric functions
        public Operand VisitDEGREES_fun(mathParser.DEGREES_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function DEGREES parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            var z = firstValue.NumberValue;
            var r = (z / Math.PI * 180);
            return Operand.Create(r);
        }
        public Operand VisitRADIANS_fun(mathParser.RADIANS_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function RADIANS parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            var r = firstValue.NumberValue / 180 * Math.PI;
            return Operand.Create(r);
        }
        public Operand VisitCOS_fun(mathParser.COS_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function COS parameter is error!");
            if (firstValue.IsError) { return firstValue; }
            return Operand.Create(Math.Cos(firstValue.NumberValue));
        }
        public Operand VisitCOSH_fun(mathParser.COSH_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function COSH parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(Math.Cosh(firstValue.NumberValue));
        }
        public Operand VisitSIN_fun(mathParser.SIN_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function SIN parameter is error!");
            if (firstValue.IsError) { return firstValue; }
            return Operand.Create(Math.Sin(firstValue.NumberValue));
        }
        public Operand VisitSINH_fun(mathParser.SINH_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function SINH parameter is error!");
            if (firstValue.IsError) { return firstValue; }
            return Operand.Create(Math.Sinh(firstValue.NumberValue));
        }
        public Operand VisitTAN_fun(mathParser.TAN_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function TAN parameter is error!");
            if (firstValue.IsError) { return firstValue; }
            return Operand.Create(Math.Tan(firstValue.NumberValue));
        }
        public Operand VisitTANH_fun(mathParser.TANH_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function TANH parameter is error!");
            if (firstValue.IsError) { return firstValue; }
            return Operand.Create(Math.Tanh(firstValue.NumberValue));
        }
        public Operand VisitACOS_fun(mathParser.ACOS_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function ACOS parameter is error!");
            if (firstValue.IsError) { return firstValue; }
            var x = firstValue.NumberValue;
            if (x < -1 && x > 1) {
                return Operand.Error("Function ACOS parameter is error!");
            }
            return Operand.Create(Math.Acos(x));
        }
        public Operand VisitACOSH_fun(mathParser.ACOSH_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function ACOSH parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            var z = firstValue.NumberValue;
            if (z < 1) {
                return Operand.Error("Function ACOSH parameter is error!");
            }
#if NETSTANDARD2_1
            return Operand.Create(Math.Acosh(z));
#else
            var r = Math.Log(z + Math.Pow(z * z - 1, 0.5));
            return Operand.Create(r);
#endif
        }
        public Operand VisitASIN_fun(mathParser.ASIN_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function ASIN parameter is error!");
            if (firstValue.IsError) { return firstValue; }
            var x = firstValue.NumberValue;
            if (x < -1 && x > 1) {
                return Operand.Error("Function ASIN parameter is error!");
            }
            return Operand.Create(Math.Asin(x));
        }
        public Operand VisitASINH_fun(mathParser.ASINH_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function ASINH parameter is error!");
            if (firstValue.IsError) { return firstValue; }
#if NETSTANDARD2_1
            return Operand.Create(Math.Asinh(firstValue.NumberValue));
#else
            var x = firstValue.NumberValue;
            var d = Math.Log(x + Math.Sqrt(x * x + 1));
            return Operand.Create(d);
#endif        
        }
        public Operand VisitATAN_fun(mathParser.ATAN_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function ATAN parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(Math.Atan(firstValue.NumberValue));
        }
        public Operand VisitATANH_fun(mathParser.ATANH_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function ATANH parameter is error!");
            if (firstValue.IsError) { return firstValue; }
            var x = firstValue.NumberValue;
            if (x >= 1 || x <= -1) {
                return Operand.Error("Function ATANH parameter is error!");
            }
#if NETSTANDARD2_1
            return Operand.Create(Math.Atanh(x));
#else
            var d = Math.Log((1 + x) / (1 - x)) / 2;
            return Operand.Create(d);
#endif        
        }
        public Operand VisitATAN2_fun(mathParser.ATAN2_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function ATAN2 parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];

            return Operand.Create(Math.Atan2(secondValue.NumberValue, firstValue.NumberValue));
        }
        public Operand VisitFIXED_fun(mathParser.FIXED_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var num = 2;
            if (args.Count > 1) {
                var secondValue = args[1].ToNumber("Function FIXED parameter 2 is error!");
                if (secondValue.IsError) { return secondValue; }
                num = secondValue.IntValue;
            }
            var firstValue = args[0].ToNumber("Function FIXED parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }

            var s = Math.Round(firstValue.NumberValue, num, MidpointRounding.AwayFromZero);
            var no = false;
            if (args.Count == 3) {
                var thirdValue = args[2].ToBoolean("Function FIXED parameter 3 is error!");
                if (thirdValue.IsError) { return thirdValue; }
                no = thirdValue.BooleanValue;
            }
            if (no == false) {
                return Operand.Create(s.ToString("N" + num, cultureInfo));
            }
            return Operand.Create(s.ToString(cultureInfo));
        }

        #endregion

        #region transformation
        public Operand VisitBIN2OCT_fun(mathParser.BIN2OCT_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToText("Function BIN2OCT parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }

            if (bit_2.IsMatch(firstValue.TextValue) == false) { return Operand.Error("Function BIN2OCT parameter 1 is error!"); }
            var num = Convert.ToString(Convert.ToInt32(firstValue.TextValue, 2), 8);
            if (args.Count == 2) {
                var secondValue = args[1].ToNumber("Function BIN2OCT parameter 2 is error!");
                if (secondValue.IsError) { return secondValue; }
                if (num.Length > secondValue.IntValue) {
                    return Operand.Create(num.PadLeft(secondValue.IntValue, '0'));
                }
                return Operand.Error("Function BIN2OCT parameter 2 is error!");
            }
            return Operand.Create(num);
        }
        public Operand VisitBIN2DEC_fun(mathParser.BIN2DEC_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToText("Function BIN2DEC parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            if (bit_2.IsMatch(firstValue.TextValue) == false) { return Operand.Error("Function BIN2DEC parameter is error!"); }
            var num = Convert.ToInt32(firstValue.TextValue, 2);
            return Operand.Create(num);
        }
        public Operand VisitBIN2HEX_fun(mathParser.BIN2HEX_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToText("Function BIN2HEX parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }

            if (bit_2.IsMatch(firstValue.TextValue) == false) { return Operand.Error("Function BIN2HEX parameter 1 is error!"); }
            var num = Convert.ToString(Convert.ToInt32(firstValue.TextValue, 2), 16).ToUpper();
            if (args.Count == 2) {
                var secondValue = args[1].ToNumber("Function BIN2HEX parameter 2 is error!");
                if (secondValue.IsError) { return secondValue; }
                if (num.Length > secondValue.IntValue) {
                    return Operand.Create(num.PadLeft(secondValue.IntValue, '0'));
                }
                return Operand.Error("Function BIN2HEX parameter 2 is error!");
            }
            return Operand.Create(num);
        }

        public Operand VisitOCT2BIN_fun(mathParser.OCT2BIN_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToText("Function OCT2BIN parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }

            if (bit_8.IsMatch(firstValue.TextValue) == false) { return Operand.Error("Function OCT2BIN parameter 1 is error!"); }
            var num = Convert.ToString(Convert.ToInt32(firstValue.TextValue, 8), 2);
            if (args.Count == 2) {
                var secondValue = args[1].ToNumber("Function OCT2BIN parameter 2 is error!");
                if (secondValue.IsError) { return secondValue; }
                if (num.Length > secondValue.IntValue) {
                    return Operand.Create(num.PadLeft(secondValue.IntValue, '0'));
                }
                return Operand.Error("Function OCT2BIN parameter 2 is error!");
            }
            return Operand.Create(num);
        }
        public Operand VisitOCT2DEC_fun(mathParser.OCT2DEC_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToText("Function OCT2DEC parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            if (bit_8.IsMatch(firstValue.TextValue) == false) { return Operand.Error("Function OCT2DEC parameter is error!"); }
            var num = Convert.ToInt32(firstValue.TextValue, 8);
            return Operand.Create(num);
        }
        public Operand VisitOCT2HEX_fun(mathParser.OCT2HEX_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToText("Function OCT2HEX parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            if (bit_8.IsMatch(firstValue.TextValue) == false) { return Operand.Error("Function OCT2HEX parameter 1 is error!"); }
            var num = Convert.ToString(Convert.ToInt32(firstValue.TextValue, 8), 16).ToUpper();
            if (args.Count == 2) {
                var secondValue = args[1].ToNumber("Function OCT2HEX parameter 2 is error!");
                if (secondValue.IsError) { return secondValue; }
                if (num.Length > secondValue.IntValue) {
                    return Operand.Create(num.PadLeft(secondValue.IntValue, '0'));
                }
                return Operand.Error("Function OCT2HEX parameter 2 is error!");
            }
            return Operand.Create(num);
        }
        public Operand VisitDEC2BIN_fun(mathParser.DEC2BIN_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToNumber("Function DEC2BIN parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            var num = System.Convert.ToString(firstValue.IntValue, 2);
            if (args.Count == 2) {
                var secondValue = args[1].ToNumber("Function DEC2BIN parameter 2 is error!");
                if (secondValue.IsError) { return secondValue; }
                if (num.Length > secondValue.IntValue) {
                    return Operand.Create(num.PadLeft(secondValue.IntValue, '0'));
                }
                return Operand.Error("Function DEC2BIN parameter 2 is error!");
            }
            return Operand.Create(num);
        }
        public Operand VisitDEC2OCT_fun(mathParser.DEC2OCT_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToNumber("Function DEC2OCT parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            var num = System.Convert.ToString(firstValue.IntValue, 8);
            if (args.Count == 2) {
                var secondValue = args[1].ToNumber("Function DEC2OCT parameter 2 is error!");
                if (secondValue.IsError) { return secondValue; }
                if (num.Length > secondValue.IntValue) {
                    return Operand.Create(num.PadLeft(secondValue.IntValue, '0'));
                }
                return Operand.Error("Function DEC2OCT parameter 2 is error!");
            }
            return Operand.Create(num);
        }
        public Operand VisitDEC2HEX_fun(mathParser.DEC2HEX_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToNumber("Function DEC2HEX parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            var num = System.Convert.ToString(firstValue.IntValue, 16).ToUpper();
            if (args.Count == 2) {
                var secondValue = args[1].ToNumber("Function DEC2HEX parameter 2 is error!");
                if (secondValue.IsError) { return secondValue; }
                if (num.Length > secondValue.IntValue) {
                    return Operand.Create(num.PadLeft(secondValue.IntValue, '0'));
                }
                return Operand.Error("Function DEC2HEX parameter 2 is error!");
            }
            return Operand.Create(num);
        }

        public Operand VisitHEX2BIN_fun(mathParser.HEX2BIN_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToText("Function HEX2BIN parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            if (bit_16.IsMatch(firstValue.TextValue) == false) { return Operand.Error("Function HEX2BIN parameter 1 is error!"); }

            var num = Convert.ToString(Convert.ToInt32(firstValue.TextValue, 16), 2);
            if (args.Count == 2) {
                var secondValue = args[1].ToNumber("Function HEX2BIN parameter 2 is error!");
                if (secondValue.IsError) { return secondValue; }
                if (num.Length > secondValue.IntValue) {
                    return Operand.Create(num.PadLeft(secondValue.IntValue, '0'));
                }
                return Operand.Error("Function HEX2BIN parameter 2 is error!");
            }
            return Operand.Create(num);
        }
        public Operand VisitHEX2OCT_fun(mathParser.HEX2OCT_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToText("Function HEX2OCT parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            if (bit_16.IsMatch(firstValue.TextValue) == false) { return Operand.Error("Function HEX2OCT parameter 1 is error!"); }
            var num = Convert.ToString(Convert.ToInt32(firstValue.TextValue, 16), 8);
            if (args.Count == 2) {
                var secondValue = args[1].ToNumber("Function HEX2OCT parameter 2 is error!");
                if (secondValue.IsError) { return secondValue; }
                if (num.Length > secondValue.IntValue) {
                    return Operand.Create(num.PadLeft(secondValue.IntValue, '0'));
                }
                return Operand.Error("Function HEX2OCT parameter 2 is error!");
            }
            return Operand.Create(num);
        }
        public Operand VisitHEX2DEC_fun(mathParser.HEX2DEC_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToText("Function HEX2DEC parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            if (bit_16.IsMatch(firstValue.TextValue) == false) { return Operand.Error("Function HEX2DEC parameter is error!"); }
            var num = Convert.ToInt32(firstValue.TextValue, 16);
            return Operand.Create(num);
        }
        #endregion


        #region rounding

        public Operand VisitROUND_fun(mathParser.ROUND_funContext context)
        {
            var args = new List<Operand>(); var index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function ROUND parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];

            return Operand.Create((double)Math.Round((decimal)firstValue.NumberValue, secondValue.IntValue, MidpointRounding.AwayFromZero));
        }
        public Operand VisitROUNDDOWN_fun(mathParser.ROUNDDOWN_funContext context)
        {
            var args = new List<Operand>(); var index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function ROUNDDOWN parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];
            if (firstValue.NumberValue == 0.0) {
                return firstValue;
            }
            var a = Math.Pow(10, secondValue.IntValue);
            var b = firstValue.NumberValue;

            b = ((double)(int)(b * a)) / a;
            return Operand.Create(b);
        }
        public Operand VisitROUNDUP_fun(mathParser.ROUNDUP_funContext context)
        {
            var args = new List<Operand>(); var index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function ROUNDUP parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];
            if (firstValue.NumberValue == 0.0) {
                return firstValue;
            }
            var a = Math.Pow(10, secondValue.IntValue);
            var b = firstValue.NumberValue;

            var t = (Math.Ceiling(Math.Abs(b) * a)) / a;
            if (b > 0) return Operand.Create(t);
            return Operand.Create(-t);
        }
        public Operand VisitCEILING_fun(mathParser.CEILING_funContext context)
        {
            var args = new List<Operand>(); var index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function CEILING parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            if (args.Count == 1)
                return Operand.Create(Math.Ceiling(firstValue.NumberValue));

            var secondValue = args[1];
            var b = secondValue.NumberValue;
            if (b == 0) {
                return Operand.Create(0);
            }
            if (b < 0) {
                return Operand.Error("Function CEILING parameter 2 is error!");
            }
            var a = firstValue.NumberValue;
            var d = Math.Ceiling(a / b) * b;
            return Operand.Create(d);
        }
        public Operand VisitFLOOR_fun(mathParser.FLOOR_funContext context)
        {
            var args = new List<Operand>(); var index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function FLOOR parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            if (args.Count == 1)
                return Operand.Create(Math.Floor(firstValue.NumberValue));

            var secondValue = args[1];
            var b = secondValue.NumberValue;
            if (b >= 1) {
                return Operand.Create(firstValue.IntValue);
            }
            if (b <= 0) {
                return Operand.Error("Function FLOOR parameter 2 is error!");
            }
            var a = firstValue.NumberValue;
            var d = Math.Floor(a / b) * b;
            return Operand.Create(d);
        }
        public Operand VisitEVEN_fun(mathParser.EVEN_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function EVEN parameter is error!");
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
        public Operand VisitODD_fun(mathParser.ODD_funContext context)
        {

            var firstValue = this.Visit(context.expr()).ToNumber("Function ODD parameter is error!");
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
        public Operand VisitMROUND_fun(mathParser.MROUND_funContext context)
        {
            var args = new List<Operand>(); var index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function MROUND parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];

            var a = secondValue.NumberValue;
            if (a <= 0) {
                return Operand.Error("Function MROUND parameter 2 is error!");
            }
            var b = firstValue.NumberValue;
            var r = Math.Round(b / a, 0, MidpointRounding.AwayFromZero) * a;
            return Operand.Create(r);
        }
        #endregion

        #region RAND
        public Operand VisitRAND_fun(mathParser.RAND_funContext context)
        {
            var tick = DateTime.Now.Ticks;
            Random rand = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            return Operand.Create(rand.NextDouble());
        }
        public Operand VisitRANDBETWEEN_fun(mathParser.RANDBETWEEN_funContext context)
        {
            var args = new List<Operand>(); var index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function RANDBETWEEN parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];

            var tick = DateTime.Now.Ticks;
            Random rand = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            return Operand.Create(rand.NextDouble() * (secondValue.NumberValue - firstValue.NumberValue) + firstValue.NumberValue);
        }
        #endregion

        #region  power logarithm factorial
        public Operand VisitFACT_fun(mathParser.FACT_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function FACT parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            var z = firstValue.IntValue;
            if (z < 0) {
                return Operand.Error("Function FACT parameter is error!");
            }
            double d = 1;
            for (int i = 1; i <= z; i++) {
                d *= i;
            }
            return Operand.Create(d);
        }
        public Operand VisitFACTDOUBLE_fun(mathParser.FACTDOUBLE_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function FACTDOUBLE parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            var z = firstValue.IntValue;
            if (z < 0) {
                return Operand.Error("Function FACTDOUBLE parameter is error!");
            }
            double d = 1;
            for (int i = z; i > 0; i -= 2) {
                d *= i;
            }
            return Operand.Create(d);
        }
        public Operand VisitPOWER_fun(mathParser.POWER_funContext context)
        {
            var args = new List<Operand>(); var index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function POWER parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];

            return Operand.Create(Math.Pow(firstValue.NumberValue, secondValue.NumberValue));
        }
        public Operand VisitEXP_fun(mathParser.EXP_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function EXP parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(Math.Exp(firstValue.NumberValue));
        }
        public Operand VisitLN_fun(mathParser.LN_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function LN parameter is error!");
            if (firstValue.IsError) { return firstValue; }
            return Operand.Create(Math.Log(firstValue.NumberValue));
        }
        public Operand VisitLOG_fun(mathParser.LOG_funContext context)
        {
            var args = new List<Operand>(); var index = 1;
            foreach (var item in context.expr()) {
                var a = this.Visit(item).ToNumber($"Function LOG parameter {index++} is error!");
                if (a.IsError) { return a; }
                args.Add(a);
            }

            if (args.Count > 1) {
                return Operand.Create(Math.Log(args[0].NumberValue, args[1].NumberValue));
            }
            return Operand.Create(Math.Log(args[0].NumberValue, 10));
        }
        public Operand VisitLOG10_fun(mathParser.LOG10_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function LOG10 parameter is error!");
            if (firstValue.IsError) { return firstValue; }
            return Operand.Create(Math.Log(firstValue.NumberValue, 10));
        }
        public Operand VisitMULTINOMIAL_fun(mathParser.MULTINOMIAL_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            List<double> list = new List<double>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function MULTINOMIAL parameter is error!"); }

            int sum = 0;
            int n = 1;
            foreach (var a in list) {
                n *= F_base_Factorial((int)a);
                sum += (int)a;
            }

            var r = F_base_Factorial(sum) / n;
            return Operand.Create(r);
        }
        public Operand VisitPRODUCT_fun(mathParser.PRODUCT_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            List<double> list = new List<double>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function PRODUCT parameter is error!"); }

            double d = 1;
            foreach (var a in list) {
                d *= a;
            }

            return Operand.Create(d);
        }
        public Operand VisitSQRTPI_fun(mathParser.SQRTPI_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function SQRTPI parameter is error!");
            if (firstValue.IsError) { return firstValue; }
            return Operand.Create(Math.Sqrt(firstValue.NumberValue * Math.PI));
        }
        public Operand VisitSUMSQ_fun(mathParser.SUMSQ_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            List<double> list = new List<double>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function SUMSQ parameter is error!"); }

            double d = 0;
            foreach (var a in list) {
                d += a * a;
            }

            return Operand.Create(d);
        }
        private int F_base_Factorial(int a)
        {
            if (a == 0) {
                return 1;
            }
            int r = 1;
            for (int i = a; i > 0; i--) {
                r *= i;
            }
            return r;
        }
        #endregion

        #endregion

        #region string

        public Operand VisitASC_fun(mathParser.ASC_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToText("Function ASC parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(F_base_ToDBC(firstValue.TextValue));
        }
        public Operand VisitJIS_fun(mathParser.JIS_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToText("Function JIS parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(F_base_ToSBC(firstValue.TextValue));
        }
        public Operand VisitCHAR_fun(mathParser.CHAR_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function CHAR parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            char c = (char)(int)firstValue.NumberValue;
            return Operand.Create(c.ToString());
        }
        public Operand VisitCLEAN_fun(mathParser.CLEAN_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToText("Function CLEAN parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            var t = firstValue.TextValue;
            t = clearRegex.Replace(t, "");
            return Operand.Create(t);
        }
        public Operand VisitCODE_fun(mathParser.CODE_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToText("Function CODE parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            var t = firstValue.TextValue;
            if (t.Length == 0) {
                return Operand.Error("Function CODE parameter is error!");
            }
            return Operand.Create((double)(int)(char)t[0]);
        }
        public Operand VisitCONCATENATE_fun(mathParser.CONCATENATE_funContext context)
        {
            var args = new List<Operand>(); var index = 1;
            foreach (var item in context.expr()) {
                var a = this.Visit(item).ToText($"Function CONCATENATE parameter {index++} is error!");
                if (a.IsError) { return a; }
                args.Add(a);
            }

            StringBuilder sb = new StringBuilder();
            foreach (var item in args) {
                sb.Append(item.TextValue);
            }
            return Operand.Create(sb.ToString());
        }
        public Operand VisitEXACT_fun(mathParser.EXACT_funContext context)
        {
            var args = new List<Operand>(); var index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function EXACT parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            var secondValue = args[1];

            return Operand.Create(firstValue.TextValue == secondValue.TextValue);
        }
        public Operand VisitFIND_fun(mathParser.FIND_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToText("Function FIND parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToText("Function FIND parameter 2 is error!");
            if (secondValue.IsError) { return secondValue; }

            if (args.Count == 2) {
#if NETSTANDARD2_1
                var p = secondValue.TextValue.AsSpan().IndexOf(firstValue.TextValue) + excelIndex;
#else
                var p = secondValue.TextValue.IndexOf(firstValue.TextValue) + excelIndex;
#endif
                return Operand.Create(p);
            }
            var count = args[2].ToNumber("Function FIND parameter 3 is error!");
            if (count.IsError) { return count; }
#if NETSTANDARD2_1
            var p2 = secondValue.TextValue.AsSpan(count.IntValue).IndexOf(firstValue.TextValue) + count.IntValue + excelIndex;
#else
            var p2 = secondValue.TextValue.IndexOf(firstValue.TextValue, count.IntValue) + excelIndex;
#endif
            return Operand.Create(p2);
        }
        public Operand VisitLEFT_fun(mathParser.LEFT_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToText("Function LEFT parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }

            if (args.Count == 1) {
                return Operand.Create(firstValue.TextValue[0].ToString());
            }
            var secondValue = args[1].ToNumber("Function LEFT parameter 2 is error!");
            if (secondValue.IsError) { return secondValue; }
#if NETSTANDARD2_1
            return Operand.Create(firstValue.TextValue.AsSpan(0, secondValue.IntValue).ToString());
#else
            return Operand.Create(firstValue.TextValue.Substring(0, secondValue.IntValue));
#endif
        }
        public Operand VisitLEN_fun(mathParser.LEN_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToText("Function LEN parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(firstValue.TextValue.Length);
        }
        public Operand VisitLOWER_fun(mathParser.LOWER_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToText("Function LOWER/TOLOWER parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(firstValue.TextValue.ToLower());
        }
        public Operand VisitMID_fun(mathParser.MID_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToText("Function MID parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToNumber("Function MID parameter 2 is error!");
            if (secondValue.IsError) { return secondValue; }
            var thirdValue = args[2].ToNumber("Function MID parameter 3 is error!");
            if (thirdValue.IsError) { return thirdValue; }
#if NETSTANDARD2_1
            return Operand.Create(firstValue.TextValue.AsSpan(secondValue.IntValue - excelIndex, thirdValue.IntValue).ToString());
#else
            return Operand.Create(firstValue.TextValue.Substring(secondValue.IntValue - excelIndex, thirdValue.IntValue));
#endif
        }
        public Operand VisitPROPER_fun(mathParser.PROPER_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToText("Function PROPER parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            var text = firstValue.TextValue;
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
        public Operand VisitREPLACE_fun(mathParser.REPLACE_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToText("Function REPLACE parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            var oldtext = firstValue.TextValue;
            if (args.Count == 3) {
                var secondValue2 = args[1].ToText("Function REPLACE parameter 2 is error!");
                var thirdValue2 = args[2].ToText("Function REPLACE parameter 3 is error!");
                var old = secondValue2.TextValue;
                var newstr = thirdValue2.TextValue;
                return Operand.Create(oldtext.Replace(old, newstr));
            }


            var secondValue = args[1].ToNumber("Function REPLACE parameter 2 is error!");
            if (secondValue.IsError) { return secondValue; }
            var thirdValue = args[2].ToNumber("Function REPLACE parameter 3 is error!");
            if (thirdValue.IsError) { return thirdValue; }
            var fourthValue = args[3].ToText("Function REPLACE parameter 3 is error!");
            if (fourthValue.IsError) { return fourthValue; }

            var start = secondValue.IntValue - excelIndex;
            var length = thirdValue.IntValue;
            var newtext = fourthValue.TextValue;

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
        public Operand VisitREPT_fun(mathParser.REPT_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToText("Function REPT parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToNumber("Function REPT parameter 2 is error!");
            if (secondValue.IsError) { return secondValue; }

            var newtext = firstValue.TextValue;
            var length = secondValue.IntValue;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++) {
                sb.Append(newtext);
            }
            return Operand.Create(sb.ToString());
        }
        public Operand VisitRIGHT_fun(mathParser.RIGHT_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToText("Function RIGHT parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }

            if (args.Count == 1) {
                return Operand.Create(firstValue.TextValue[firstValue.TextValue.Length - 1].ToString());
            }
            var secondValue = args[1].ToNumber("Function RIGHT parameter 2 is error!");
            if (secondValue.IsError) { return secondValue; }
#if NETSTANDARD2_1
            return Operand.Create(firstValue.TextValue.AsSpan(firstValue.TextValue.Length - secondValue.IntValue, secondValue.IntValue).ToString());
#else
            return Operand.Create(firstValue.TextValue.Substring(firstValue.TextValue.Length - secondValue.IntValue, secondValue.IntValue));
#endif

        }
        public Operand VisitRMB_fun(mathParser.RMB_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function RMB parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(F_base_ToChineseRMB(firstValue.NumberValue));
        }
        public Operand VisitSEARCH_fun(mathParser.SEARCH_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToText("Function SEARCH parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToText("Function SEARCH parameter 2 is error!");
            if (secondValue.IsError) { return secondValue; }
            if (args.Count == 2) {
#if NETSTANDARD2_1
                var p = secondValue.TextValue.AsSpan().IndexOf(firstValue.TextValue, StringComparison.OrdinalIgnoreCase) + excelIndex;
#else
                var p = secondValue.TextValue.IndexOf(firstValue.TextValue, StringComparison.OrdinalIgnoreCase) + excelIndex;
#endif
                return Operand.Create(p);
            }
            var thirdValue = args[2].ToNumber("Function SEARCH parameter 3 is error!");
            if (thirdValue.IsError) { return thirdValue; }
#if NETSTANDARD2_1
            var p2 = secondValue.TextValue.AsSpan(thirdValue.IntValue).IndexOf(firstValue.TextValue, StringComparison.OrdinalIgnoreCase) + thirdValue.IntValue + excelIndex;
#else
            var p2 = secondValue.TextValue.IndexOf(firstValue.TextValue, thirdValue.IntValue, StringComparison.OrdinalIgnoreCase) + excelIndex;
#endif
            return Operand.Create(p2);
        }
        public Operand VisitSUBSTITUTE_fun(mathParser.SUBSTITUTE_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }
            var firstValue = args[0].ToText("Function SUBSTITUTE parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToText("Function SUBSTITUTE parameter 2 is error!");
            if (secondValue.IsError) { return secondValue; }
            var thirdValue = args[2].ToText("Function SUBSTITUTE parameter 3 is error!");
            if (thirdValue.IsError) { return thirdValue; }
            if (args.Count == 3) {
                return Operand.Create(firstValue.TextValue.Replace(secondValue.TextValue, thirdValue.TextValue));
            }
            var fourthValue = args[3].ToNumber("Function SUBSTITUTE parameter 4 is error!");
            if (fourthValue.IsError) { return fourthValue; }

            string text = firstValue.TextValue;
            string oldtext = secondValue.TextValue;
            string newtext = thirdValue.TextValue;
            int index = fourthValue.IntValue;

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
        public Operand VisitT_fun(mathParser.T_funContext context)
        {
            var firstValue = this.Visit(context.expr());
            if (firstValue.Type == OperandType.STRING) {
                return firstValue;
            }
            return Operand.Create("");
        }
        public Operand VisitTEXT_fun(mathParser.TEXT_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToText("Function TEXT parameter 2 is error!");
            if (secondValue.IsError) { return secondValue; }

            if (firstValue.Type == OperandType.STRING) {
                return firstValue;
            } else if (firstValue.Type == OperandType.BOOLEAN) {
                return Operand.Create(firstValue.BooleanValue ? "TRUE" : "FALSE");
            } else if (firstValue.Type == OperandType.NUMBER) {
                return Operand.Create(firstValue.NumberValue.ToString(secondValue.TextValue, cultureInfo));
            } else if (firstValue.Type == OperandType.DATE) {
                return Operand.Create(firstValue.DateValue.ToString(secondValue.TextValue));
            }
            firstValue = firstValue.ToText("Function TEXT parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            return Operand.Create(firstValue.TextValue.ToString());
        }
        public Operand VisitTRIM_fun(mathParser.TRIM_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToText("Function TRIM parameter is error!");
            if (firstValue.IsError) { return firstValue; }
#if NETSTANDARD2_1
            return Operand.Create(firstValue.TextValue.AsSpan().Trim().ToString());
#else
            return Operand.Create(firstValue.TextValue.Trim());
#endif
        }
        public Operand VisitUPPER_fun(mathParser.UPPER_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToText("Function UPPER/TOUPPER parameter is error!");
            if (firstValue.IsError) { return firstValue; }
            return Operand.Create(firstValue.TextValue.ToUpper());
        }
        public Operand VisitVALUE_fun(mathParser.VALUE_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToText("Function VALUE parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            if (double.TryParse(firstValue.TextValue, NumberStyles.Any, cultureInfo, out double d)) {
                return Operand.Create(d);
            }
            return Operand.Error("Function VALUE parameter is error!");
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
            string s = x.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A", cultureInfo);
            string d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}", RegexOptions.Compiled);
            return Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString(), RegexOptions.Compiled);
        }
        #endregion

        #region date time

        public Operand VisitDATEVALUE_fun(mathParser.DATEVALUE_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToText("Function DATEVALUE parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            if (DateTime.TryParse(firstValue.TextValue, cultureInfo, DateTimeStyles.None, out DateTime dt)) {
                return Operand.Create(dt);
            }
            return Operand.Error("Function DATEVALUE parameter is error!");
        }
        public Operand VisitTIMEVALUE_fun(mathParser.TIMEVALUE_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToText("Function TIMEVALUE parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            if (TimeSpan.TryParse(firstValue.TextValue, cultureInfo, out TimeSpan dt)) {
                return Operand.Create(dt);
            }
            return Operand.Error("Function TIMEVALUE parameter is error!");
        }
        public Operand VisitDATE_fun(mathParser.DATE_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function DATE parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

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
        public Operand VisitTIME_fun(mathParser.TIME_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function TIME parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            Date d;
            if (args.Count == 3) {
                d = new Date(0, 0, 0, args[0].IntValue, args[1].IntValue, args[2].IntValue);
            } else {
                d = new Date(0, 0, 0, args[0].IntValue, args[1].IntValue, 0);
            }
            return Operand.Create(d);
        }
        public Operand VisitNOW_fun(mathParser.NOW_funContext context)
        {
            return Operand.Create(new Date(DateTime.Now));
        }
        public Operand VisitTODAY_fun(mathParser.TODAY_funContext context)
        {
            return Operand.Create(new Date(DateTime.Today));
        }
        public Operand VisitYEAR_fun(mathParser.YEAR_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToDate("Function YEAR parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(firstValue.DateValue.Year);
        }
        public Operand VisitMONTH_fun(mathParser.MONTH_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToDate("Function MONTH parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(firstValue.DateValue.Month);
        }
        public Operand VisitDAY_fun(mathParser.DAY_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToDate("Function DAY parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(firstValue.DateValue.Day);
        }
        public Operand VisitHOUR_fun(mathParser.HOUR_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToDate("Function HOUR parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(firstValue.DateValue.Hour);
        }
        public Operand VisitMINUTE_fun(mathParser.MINUTE_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToDate("Function MINUTE parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(firstValue.DateValue.Minute);
        }
        public Operand VisitSECOND_fun(mathParser.SECOND_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToDate("Function SECOND parameter is error!");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(firstValue.DateValue.Second);
        }
        public Operand VisitWEEKDAY_fun(mathParser.WEEKDAY_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToDate("Function WEEKDAY parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }

            var type = 1;
            if (args.Count == 2) {
                var secondValue = args[1].ToNumber("Function WEEKDAY parameter 2 is error!");
                if (secondValue.IsError) { return secondValue; }
                type = secondValue.IntValue;
            }

            var t = ((DateTime)firstValue.DateValue).DayOfWeek;
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
        public Operand VisitDATEDIF_fun(mathParser.DATEDIF_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToDate("Function DATEDIF parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToDate("Function DATEDIF parameter 2 is error!");
            if (secondValue.IsError) { return secondValue; }
            var thirdValue = args[2].ToText("Function DATEDIF parameter 3 is error!");
            if (thirdValue.IsError) { return thirdValue; }

            var startDate = (DateTime)firstValue.DateValue;
            var endDate = (DateTime)secondValue.DateValue;
            var t = thirdValue.TextValue.ToLower();

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
            return Operand.Error("Function DATEDIF parameter 3 is error!");
        }
        public Operand VisitDAYS360_fun(mathParser.DAYS360_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToDate("Function DAYS360 parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToDate("Function DAYS360 parameter 2 is error!");
            if (secondValue.IsError) { return secondValue; }

            var startDate = (DateTime)firstValue.DateValue;
            var endDate = (DateTime)secondValue.DateValue;

            var method = false;
            if (args.Count == 3) {
                var thirdValue = args[2].ToDate("Function DAYS360 parameter 3 is error!");
                if (thirdValue.IsError) { return thirdValue; }
                method = thirdValue.BooleanValue;
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
        public Operand VisitEDATE_fun(mathParser.EDATE_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToDate("Function EDATE parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToNumber("Function EDATE parameter 2 is error!");
            if (secondValue.IsError) { return secondValue; }

            return Operand.Create((Date)(((DateTime)firstValue.DateValue).AddMonths(secondValue.IntValue)));
        }
        public Operand VisitEOMONTH_fun(mathParser.EOMONTH_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToDate("Function EOMONTH parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToNumber("Function EOMONTH parameter 2 is error!");
            if (secondValue.IsError) { return secondValue; }

            var dt = ((DateTime)firstValue.DateValue).AddMonths(secondValue.IntValue + 1);
            dt = new DateTime(dt.Year, dt.Month, 1).AddDays(-1);
            return Operand.Create(dt);
        }
        public Operand VisitNETWORKDAYS_fun(mathParser.NETWORKDAYS_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = this.Visit(item).ToDate($"Function NETWORKDAYS parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }

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
        public Operand VisitWORKDAY_fun(mathParser.WORKDAY_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToDate("Function WORKDAY parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToNumber("Function WORKDAY parameter 2 is error!");
            if (secondValue.IsError) { return secondValue; }


            var startDate = (DateTime)firstValue.DateValue;
            var days = secondValue.IntValue;
            List<DateTime> list = new List<DateTime>();
            for (int i = 2; i < args.Count; i++) {
                args[i] = args[i].ToDate($"Function WORKDAY parameter {i + 1} is error!");
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
        public Operand VisitWEEKNUM_fun(mathParser.WEEKNUM_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToDate("Function WEEKNUM parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }

            var startDate = (DateTime)firstValue.DateValue;

            var days = startDate.DayOfYear + (int)(new DateTime(startDate.Year, 1, 1).DayOfWeek);
            if (args.Count == 2) {
                var secondValue = args[1].ToNumber("Function WEEKNUM parameter 2 is error!");
                if (secondValue.IsError) { return secondValue; }
                if (secondValue.IntValue == 2) {
                    days--;
                }
            }

            var week = Math.Ceiling(days / 7.0);
            return Operand.Create(week);
        }

        #endregion

        #region sum

        public Operand VisitMAX_fun(mathParser.MAX_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function MAX parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            List<double> list = new List<double>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function MAX parameter error!"); }

            return Operand.Create(list.Max());
        }
        public Operand VisitMEDIAN_fun(mathParser.MEDIAN_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function MEDIAN parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            List<double> list = new List<double>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function MEDIAN parameter error!"); }

            list = list.OrderBy(q => q).ToList();
            return Operand.Create(list[list.Count / 2]);
        }
        public Operand VisitMIN_fun(mathParser.MIN_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function MIN parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            List<double> list = new List<double>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function MIN parameter error!"); }

            return Operand.Create(list.Min());
        }
        public Operand VisitQUARTILE_fun(mathParser.QUARTILE_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToArray("Function QUARTILE parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToNumber("Function QUARTILE parameter 2 is error!");
            if (secondValue.IsError) { return secondValue; }

            List<double> list = new List<double>();
            var o = F_base_GetList(firstValue, list);
            if (o == false) { return Operand.Error("Function QUARTILE parameter 1 error!"); }

            var quant = secondValue.IntValue;
            if (quant < 0 || quant > 4) {
                return Operand.Error("Function QUARTILE parameter 2 is error!");
            }
            return Operand.Create(ExcelFunctions.Quartile(list.ToArray(), quant));
        }
        public Operand VisitMODE_fun(mathParser.MODE_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function MODE parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            List<double> list = new List<double>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function MODE parameter error!"); }


            Dictionary<double, int> dict = new Dictionary<double, int>();
            foreach (var item in list) {
                if (dict.ContainsKey(item)) {
                    dict[item] += 1;
                } else {
                    dict[item] = 1;
                }
            }
            return Operand.Create(dict.OrderByDescending(q => q.Value).First().Key);
        }
        public Operand VisitLARGE_fun(mathParser.LARGE_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToArray("Function LARGE parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToNumber("Function LARGE parameter 2 is error!");
            if (secondValue.IsError) { return secondValue; }


            List<double> list = new List<double>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function LARGE parameter error!"); }


            list = list.OrderByDescending(q => q).ToList();
            var quant = secondValue.IntValue;
            return Operand.Create(list[secondValue.IntValue - excelIndex]);
        }
        public Operand VisitSMALL_fun(mathParser.SMALL_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToArray("Function SMALL parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToNumber("Function SMALL parameter 2 is error!");
            if (secondValue.IsError) { return secondValue; }

            List<double> list = new List<double>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function SMALL parameter error!"); }

            list = list.OrderBy(q => q).ToList();
            var quant = secondValue.IntValue;
            return Operand.Create(list[secondValue.IntValue - excelIndex]);
        }
        public Operand VisitPERCENTILE_fun(mathParser.PERCENTILE_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToArray("Function PERCENTILE parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToNumber("Function PERCENTILE parameter 2 is error!");
            if (secondValue.IsError) { return secondValue; }

            List<double> list = new List<double>();
            var o = F_base_GetList(firstValue, list);
            if (o == false) { return Operand.Error("Function PERCENTILE parameter error!"); }

            var k = secondValue.NumberValue;
            return Operand.Create(ExcelFunctions.Percentile(list.ToArray(), k));
        }
        public Operand VisitPERCENTRANK_fun(mathParser.PERCENTRANK_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToArray("Function PERCENTRANK parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToNumber("Function PERCENTRANK parameter 2 is error!");
            if (secondValue.IsError) { return secondValue; }

            List<double> list = new List<double>();
            var o = F_base_GetList(firstValue, list);
            if (o == false) { return Operand.Error("Function PERCENTRANK parameter error!"); }

            var k = secondValue.NumberValue;
            var v = ExcelFunctions.PercentRank(list.ToArray(), k);
            var d = 3;
            if (args.Count == 3) {
                var thirdValue = args[2].ToNumber("Function PERCENTRANK parameter 3 is error!");
                if (thirdValue.IsError) { return thirdValue; }
                d = thirdValue.IntValue;
            }
            return Operand.Create(Math.Round(v, d, MidpointRounding.AwayFromZero));
        }
        public Operand VisitAVERAGE_fun(mathParser.AVERAGE_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            List<double> list = new List<double>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function AVERAGE parameter error!"); }

            return Operand.Create(list.Average());
        }


        public Operand VisitAVERAGEIF_fun(mathParser.AVERAGEIF_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            List<double> list = new List<double>();
            var o = F_base_GetList(args[0], list);
            if (o == false) { return Operand.Error("Function AVERAGE parameter 1 error!"); }

            List<double> sumdbs;
            if (args.Count == 3) {
                sumdbs = new List<double>();
                var o2 = F_base_GetList(args[2], sumdbs);
                if (o2 == false) { return Operand.Error("Function AVERAGE parameter 3 error!"); }
            } else {
                sumdbs = list;
            }

            double sum;
            int count;
            if (args[1].Type == OperandType.NUMBER) {
                count = F_base_countif(list, args[1].NumberValue);
                sum = count * args[1].NumberValue;
            } else {
                if (double.TryParse(args[1].TextValue.Trim(), NumberStyles.Any, cultureInfo, out double d)) {
                    count = F_base_countif(list, d);
                    sum = F_base_sumif(list, "=" + args[1].TextValue.Trim(), sumdbs);
                } else {
                    var sunif = args[1].TextValue.Trim();
                    if (sumifRegex.IsMatch(sunif)) {
                        count = F_base_countif(list, sunif);
                        sum = F_base_sumif(list, sunif, sumdbs);
                    } else {
                        return Operand.Error("Function AVERAGE parameter 2 error!");
                    }
                }
            }
            return Operand.Create(sum / count);
        }
        public Operand VisitGEOMEAN_fun(mathParser.GEOMEAN_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            if (args.Count == 1) return args[0];

            List<double> list = new List<double>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function GEOMEAN parameter error!"); }

            double sum = 1;
            foreach (var db in list) {
                sum *= db;
            }
            return Operand.Create(Math.Pow(sum, 1.0 / list.Count));
        }
        public Operand VisitHARMEAN_fun(mathParser.HARMEAN_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            if (args.Count == 1) return args[0];

            List<double> list = new List<double>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function HARMEAN parameter error!"); }

            double sum = 0;
            foreach (var db in list) {
                if (db == 0) {
                    return Operand.Error("Function HARMEAN parameter error!");
                }
                sum += 1 / db;
            }
            return Operand.Create(list.Count / sum);
        }
        public Operand VisitCOUNT_fun(mathParser.COUNT_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            List<double> list = new List<double>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function COUNT parameter error!"); }

            return Operand.Create(list.Count);
        }
        public Operand VisitCOUNTIF_fun(mathParser.COUNTIF_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            List<double> list = new List<double>();
            var o = F_base_GetList(args[0], list);
            if (o == false) { return Operand.Error("Function COUNTIF parameter error!"); }

            int count;
            if (args[1].Type == OperandType.NUMBER) {
                count = F_base_countif(list, args[1].NumberValue);
            } else {
                if (double.TryParse(args[1].TextValue.Trim(), NumberStyles.Any, cultureInfo, out double d)) {
                    count = F_base_countif(list, d);
                } else {
                    var sunif = args[1].TextValue.Trim();
                    if (sumifRegex.IsMatch(sunif)) {
                        count = F_base_countif(list, sunif);
                    } else {
                        return Operand.Error("Function COUNTIF parameter 2 error!");
                    }
                }
            }
            return Operand.Create(count);
        }
        public Operand VisitSUM_fun(mathParser.SUM_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            if (args.Count == 1) return args[0];

            List<double> list = new List<double>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function SUM parameter error!"); }

            double sum = list.Sum();
            return Operand.Create(sum);
        }
        public Operand VisitSUMIF_fun(mathParser.SUMIF_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            List<double> list = new List<double>();
            var o = F_base_GetList(args[0], list);
            if (o == false) { return Operand.Error("Function SUMIF parameter 1 error!"); }

            List<double> sumdbs;
            if (args.Count == 3) {
                sumdbs = new List<double>();
                var o2 = F_base_GetList(args[2], sumdbs);
                if (o2 == false) { return Operand.Error("Function SUMIF parameter 3 error!"); }
            } else {
                sumdbs = list;
            }

            double sum;
            if (args[1].Type == OperandType.NUMBER) {
                sum = F_base_countif(list, args[1].NumberValue) * args[1].NumberValue;
            } else {
                if (double.TryParse(args[1].TextValue.Trim(), NumberStyles.Any, cultureInfo, out _)) {
                    sum = F_base_sumif(list, "=" + args[1].TextValue.Trim(), sumdbs);
                } else {
                    var sunif = args[1].TextValue.Trim();
                    if (sumifRegex.IsMatch(sunif)) {
                        sum = F_base_sumif(list, sunif, sumdbs);
                    } else {
                        return Operand.Error("Function SUMIF parameter 2 error!");
                    }
                }
            }
            return Operand.Create(sum);
        }
        public Operand VisitAVEDEV_fun(mathParser.AVEDEV_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            List<double> list = new List<double>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function AVEDEV parameter error!"); }

            double avg = list.Average();
            double sum = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += Math.Abs(list[i] - avg);
            }
            return Operand.Create(sum / list.Count);
        }

        public Operand VisitSTDEV_fun(mathParser.STDEV_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            if (args.Count == 1) {
                return Operand.Error("Function STDEV parameter only one error!");
            }
            List<double> list = new List<double>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function STDEV parameter error!"); }

            double avg = list.Average();
            double sum = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += (list[i] - avg) * (list[i] - avg);
            }
            return Operand.Create(Math.Sqrt(sum / (list.Count - 1)));
        }
        public Operand VisitSTDEVP_fun(mathParser.STDEVP_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            List<double> list = new List<double>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function STDEVP parameter error!"); }

            double sum = 0;
            double avg = list.Average();

            for (int i = 0; i < list.Count; i++) {
                sum += (list[i] - avg) * (list[i] - avg);
            }
            return Operand.Create(Math.Sqrt(sum / (list.Count)));
        }
        public Operand VisitDEVSQ_fun(mathParser.DEVSQ_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            List<double> list = new List<double>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function DEVSQ parameter error!"); }

            double avg = list.Average();
            double sum = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += (list[i] - avg) * (list[i] - avg);
            }
            return Operand.Create(sum);
        }
        public Operand VisitVAR_fun(mathParser.VAR_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            if (args.Count == 1) {
                return Operand.Error("Function VAR parameter only one error!");
            }
            List<double> list = new List<double>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function VAR parameter error!"); }

            double sum = 0;
            double sum2 = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += list[i] * list[i];
                sum2 += list[i];
            }
            return Operand.Create((list.Count * sum - sum2 * sum2) / list.Count / (list.Count - 1));
        }
        public Operand VisitVARP_fun(mathParser.VARP_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            List<double> list = new List<double>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function VARP parameter error!"); }

            double sum = 0;
            double avg = list.Average();
            for (int i = 0; i < list.Count; i++) {
                sum += (avg - list[i]) * (avg - list[i]);
            }
            return Operand.Create(sum / list.Count);
        }
        public Operand VisitNORMDIST_fun(mathParser.NORMDIST_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToNumber("Function NORMDIST parameter 1 error!");
            if (firstValue.IsError) return firstValue;
            var secondValue = args[1].ToNumber("Function NORMDIST parameter 2 error!");
            if (secondValue.IsError) return secondValue;
            var thirdValue = args[2].ToNumber("Function NORMDIST parameter 3 error!");
            if (thirdValue.IsError) return thirdValue;

            var fourthValue = args[3].ToBoolean("Function NORMDIST parameter 4 error!");
            if (fourthValue.IsError) return fourthValue;

            var num = firstValue.NumberValue;
            var avg = secondValue.NumberValue;
            var STDEV = thirdValue.NumberValue;
            var b = fourthValue.BooleanValue;
            return Operand.Create(ExcelFunctions.NormDist(num, avg, STDEV, b));
        }
        public Operand VisitNORMINV_fun(mathParser.NORMINV_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function NORMINV parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            var num = args[0].NumberValue;
            var avg = args[1].NumberValue;
            var STDEV = args[2].NumberValue;
            return Operand.Create(ExcelFunctions.NormInv(num, avg, STDEV));
        }
        public Operand VisitNORMSDIST_fun(mathParser.NORMSDIST_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function NORMSDIST parameter 1 error!");
            if (firstValue.IsError) { return firstValue; }

            var k = firstValue.NumberValue;
            return Operand.Create(ExcelFunctions.NormSDist(k));
        }
        public Operand VisitNORMSINV_fun(mathParser.NORMSINV_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function NORMSINV parameter 1 error!");
            if (firstValue.IsError) { return firstValue; }

            var k = firstValue.NumberValue;
            return Operand.Create(ExcelFunctions.NormSInv(k));
        }
        public Operand VisitBETADIST_fun(mathParser.BETADIST_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function BETADIST parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            var x = args[0].NumberValue;
            var alpha = args[1].NumberValue;
            var beta = args[2].NumberValue;

            if (alpha < 0.0 || beta < 0.0) {
                return Operand.Error("Function BETADIST parameter error!");
            }

            return Operand.Create(ExcelFunctions.BetaDist(x, alpha, beta));
        }
        public Operand VisitBETAINV_fun(mathParser.BETAINV_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function BETAINV parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            var probability = args[0].NumberValue;
            var alpha = args[1].NumberValue;
            var beta = args[2].NumberValue;
            if (alpha < 0.0 || beta < 0.0 || probability < 0.0 || probability > 1.0) {
                return Operand.Error("Function BETAINV parameter error!");
            }
            return Operand.Create(ExcelFunctions.BetaInv(probability, alpha, beta));
        }
        public Operand VisitBINOMDIST_fun(mathParser.BINOMDIST_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToNumber("Function BINOMDIST parameter 1 error!");
            if (firstValue.IsError) return firstValue;
            var secondValue = args[1].ToNumber("Function BINOMDIST parameter 2 error!");
            if (secondValue.IsError) return secondValue;
            var thirdValue = args[2].ToNumber("Function BINOMDIST parameter 3 error!");
            if (thirdValue.IsError) return thirdValue;

            var fourthValue = args[3].ToBoolean("Function BINOMDIST parameter 4 error!");
            if (fourthValue.IsError) return fourthValue;

            if (!(thirdValue.NumberValue >= 0.0 && thirdValue.NumberValue <= 1.0 && secondValue.NumberValue >= 0)) {
                return Operand.Error("Function BINOMDIST parameter error!");
            }
            return Operand.Create(ExcelFunctions.BinomDist(firstValue.IntValue, secondValue.IntValue, thirdValue.NumberValue, fourthValue.BooleanValue));
        }
        public Operand VisitEXPONDIST_fun(mathParser.EXPONDIST_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToNumber("Function EXPONDIST parameter 1 error!");
            if (firstValue.IsError) return firstValue;
            var secondValue = args[1].ToNumber("Function EXPONDIST parameter 2 error!");
            if (secondValue.IsError) return secondValue;
            var thirdValue = args[2].ToBoolean("Function EXPONDIST parameter 3 error!");
            if (thirdValue.IsError) return thirdValue;

            if (firstValue.NumberValue < 0.0) {
                return Operand.Error("Function EXPONDIST parameter error!");
            }

            return Operand.Create(ExcelFunctions.ExponDist(firstValue.NumberValue, secondValue.NumberValue, thirdValue.BooleanValue));
        }
        public Operand VisitFDIST_fun(mathParser.FDIST_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function FDIST parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            var x = args[0].NumberValue;
            var degreesFreedom = args[1].IntValue;
            var degreesFreedom2 = args[2].IntValue;
            if (degreesFreedom <= 0.0 || degreesFreedom2 <= 0.0) {
                return Operand.Error("Function FDIST parameter error!");
            }
            return Operand.Create(ExcelFunctions.FDist(x, degreesFreedom, degreesFreedom2));
        }
        public Operand VisitFINV_fun(mathParser.FINV_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function FINV parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            var probability = args[0].NumberValue;
            var degreesFreedom = args[1].IntValue;
            var degreesFreedom2 = args[2].IntValue;
            if (degreesFreedom <= 0.0 || degreesFreedom2 <= 0.0) {
                return Operand.Error("Function FINV parameter error!");
            }
            return Operand.Create(ExcelFunctions.FInv(probability, degreesFreedom, degreesFreedom2));
        }
        public Operand VisitFISHER_fun(mathParser.FISHER_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function FISHER parameter error!");
            if (firstValue.IsError) { return firstValue; }

            var x = firstValue.NumberValue;
            if (x >= 1 || x <= -1) {
                return Operand.Error("Function FISHER parameter error!");
            }
            var n = 0.5 * Math.Log((1 + x) / (1 - x));
            return Operand.Create(n);
        }
        public Operand VisitFISHERINV_fun(mathParser.FISHERINV_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function FISHERINV parameter error!");
            if (firstValue.IsError) { return firstValue; }

            var x = firstValue.NumberValue;
            var n = (Math.Exp(2 * x) - 1) / (Math.Exp(2 * x) + 1);
            return Operand.Create(n);
        }
        public Operand VisitGAMMADIST_fun(mathParser.GAMMADIST_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToNumber("Function GAMMADIST parameter 1 error!");
            if (firstValue.IsError) return firstValue;
            var secondValue = args[1].ToNumber("Function GAMMADIST parameter 2 error!");
            if (secondValue.IsError) return secondValue;
            var thirdValue = args[2].ToNumber("Function GAMMADIST parameter 3 error!");
            if (thirdValue.IsError) return thirdValue;

            var fourthValue = args[3].ToBoolean("Function GAMMADIST parameter 4 error!");
            if (fourthValue.IsError) return fourthValue;


            var x = firstValue.NumberValue;
            var alpha = secondValue.NumberValue;
            var beta = thirdValue.NumberValue;
            var cumulative = fourthValue.BooleanValue;
            if (alpha < 0.0 || beta < 0.0) {
                return Operand.Error("Function GAMMADIST parameter error!");
            }
            return Operand.Create(ExcelFunctions.GammaDist(x, alpha, beta, cumulative));
        }
        public Operand VisitGAMMAINV_fun(mathParser.GAMMAINV_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function GAMMAINV parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            var probability = args[0].NumberValue;
            var alpha = args[1].NumberValue;
            var beta = args[2].NumberValue;
            if (alpha < 0.0 || beta < 0.0 || probability < 0 || probability > 1.0) {
                return Operand.Error("Function GAMMAINV parameter error!");
            }
            return Operand.Create(ExcelFunctions.GammaInv(probability, alpha, beta));
        }
        public Operand VisitGAMMALN_fun(mathParser.GAMMALN_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToNumber("Function GAMMALN parameter error!");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(ExcelFunctions.GAMMALN(firstValue.NumberValue));
        }
        public Operand VisitHYPGEOMDIST_fun(mathParser.HYPGEOMDIST_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function HYPGEOMDIST parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            int k = args[0].IntValue;
            int draws = args[1].IntValue;
            int success = args[2].IntValue;
            int population = args[3].IntValue;
            if (!(population >= 0 && success >= 0 && draws >= 0 && success <= population && draws <= population)) {
                return Operand.Error("Function HYPGEOMDIST parameter error!");
            }
            return Operand.Create(ExcelFunctions.HypgeomDist(k, draws, success, population));
        }
        public Operand VisitLOGINV_fun(mathParser.LOGINV_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function LOGINV parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            if (args[2].NumberValue < 0.0) {
                return Operand.Error("Function LOGINV parameter error!");
            }
            return Operand.Create(ExcelFunctions.LogInv(args[0].NumberValue, args[1].NumberValue, args[2].NumberValue));
        }
        public Operand VisitLOGNORMDIST_fun(mathParser.LOGNORMDIST_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function LOGNORMDIST parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            if (args[2].NumberValue < 0.0) {
                return Operand.Error("Function LOGNORMDIST parameter error!");
            }
            return Operand.Create(ExcelFunctions.LognormDist(args[0].NumberValue, args[1].NumberValue, args[2].NumberValue));
        }
        public Operand VisitNEGBINOMDIST_fun(mathParser.NEGBINOMDIST_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function NEGBINOMDIST parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            int k = args[0].IntValue;
            double r = args[1].NumberValue;
            double p = args[2].NumberValue;

            if (!(r >= 0.0 && p >= 0.0 && p <= 1.0)) {
                return Operand.Error("Function NEGBINOMDIST parameter error!");
            }
            return Operand.Create(ExcelFunctions.NegbinomDist(k, r, p));
        }
        public Operand VisitPOISSON_fun(mathParser.POISSON_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToNumber("Function POISSON parameter 1 error!");
            if (firstValue.IsError) return firstValue;
            var secondValue = args[1].ToNumber("Function POISSON parameter 2 error!");
            if (secondValue.IsError) return secondValue;
            var thirdValue = args[2].ToBoolean("Function POISSON parameter 3 error!");
            if (thirdValue.IsError) return thirdValue;

            int k = firstValue.IntValue;
            double lambda = secondValue.NumberValue;
            bool state = thirdValue.BooleanValue;
            if (!(lambda > 0.0)) {
                return Operand.Error("Function POISSON parameter error!");
            }
            return Operand.Create(ExcelFunctions.POISSON(k, lambda, state));
        }
        public Operand VisitTDIST_fun(mathParser.TDIST_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function TDIST parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            var x = args[0].NumberValue;
            var degreesFreedom = args[1].IntValue;
            var tails = args[2].IntValue;
            if (degreesFreedom <= 0.0 || tails < 1 || tails > 2) {
                return Operand.Error("Function TDIST parameter error!");
            }
            return Operand.Create(ExcelFunctions.TDist(x, degreesFreedom, tails));
        }
        public Operand VisitTINV_fun(mathParser.TINV_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var aa = this.Visit(item).ToNumber($"Function TINV parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            var probability = args[0].NumberValue;
            var degreesFreedom = args[1].IntValue;
            if (degreesFreedom <= 0.0) {
                return Operand.Error("Function TINV parameter error!");
            }
            return Operand.Create(ExcelFunctions.TInv(probability, degreesFreedom));
        }
        public Operand VisitWEIBULL_fun(mathParser.WEIBULL_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToNumber("Function WEIBULL parameter 1 error!");
            if (firstValue.IsError) return firstValue;
            var secondValue = args[1].ToNumber("Function WEIBULL parameter 2 error!");
            if (secondValue.IsError) return secondValue;
            var thirdValue = args[2].ToNumber("Function WEIBULL parameter 3 error!");
            if (thirdValue.IsError) return thirdValue;

            var fourthValue = args[3].ToBoolean("Function WEIBULL parameter 4 error!");
            if (fourthValue.IsError) return fourthValue;

            var x = firstValue.NumberValue;
            var shape = secondValue.NumberValue;
            var scale = thirdValue.NumberValue;
            var state = fourthValue.BooleanValue;
            if (shape <= 0.0 || scale <= 0.0) {
                return Operand.Error("Function WEIBULL parameter error!");
            }

            return Operand.Create(ExcelFunctions.WEIBULL(x, shape, scale, state));
        }

        private int F_base_countif(List<double> dbs, double d)
        {
            int count = 0;
            d = Math.Round(d, 12, MidpointRounding.AwayFromZero);
            foreach (var item in dbs) {
                if (Math.Round(item, 12, MidpointRounding.AwayFromZero) == d) {
                    count++;
                }
            }
            return count;
        }
        private int F_base_countif(List<double> dbs, string s)
        {
            var m = sumifRegex.Match(s);
            var d = double.Parse(m.Groups[2].Value, NumberStyles.Any, cultureInfo);
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
            var m = sumifRegex.Match(s);
            var d = double.Parse(m.Groups[2].Value, NumberStyles.Any, cultureInfo);
            //var ss = m.Groups[1].Value;
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
                return Math.Round(a - b, 12, MidpointRounding.AwayFromZero) < 0;
            } else if (ss == "<=") {
                return Math.Round(a - b, 12, MidpointRounding.AwayFromZero) <= 0;
            } else if (ss == ">") {
                return Math.Round(a - b, 12, MidpointRounding.AwayFromZero) > 0;
            } else if (ss == ">=") {
                return Math.Round(a - b, 12, MidpointRounding.AwayFromZero) >= 0;
            } else if (ss == "=" || ss == "==") {
                return Math.Round(a - b, 12, MidpointRounding.AwayFromZero) == 0;
            }
            return Math.Round(a - b, 12, MidpointRounding.AwayFromZero) != 0;
        }

        private bool F_base_GetList(List<Operand> args, List<double> list)
        {
            foreach (var item in args) {
                if (item.Type == OperandType.NUMBER) {
                    list.Add(item.NumberValue);
                } else if (item.Type == OperandType.ARRARY) {
                    var o = F_base_GetList(item.ArrayValue, list);
                    if (o == false) { return false; }
                } else if (item.Type == OperandType.JSON) {
                    var i = item.ToArray(null);
                    if (i.IsError) { return false; }
                    var o = F_base_GetList(i.ArrayValue, list);
                    if (o == false) { return false; }
                } else {
                    var o = item.ToNumber(null);
                    if (o.IsError) { return false; }
                    list.Add(o.NumberValue);
                }
            }
            return true;
        }
        private bool F_base_GetList(Operand args, List<double> list)
        {
            if (args.IsError) { return false; }
            if (args.Type == OperandType.NUMBER) {
                list.Add(args.NumberValue);
            } else if (args.Type == OperandType.ARRARY) {
                var o = F_base_GetList(args.ArrayValue, list);
                if (o == false) { return false; }
            } else if (args.Type == OperandType.JSON) {
                var i = args.ToArray(null);
                if (i.IsError) { return false; }
                var o = F_base_GetList(i.ArrayValue, list);
                if (o == false) { return false; }
            } else {
                var o = args.ToNumber(null);
                if (o.IsError) { return false; }
                list.Add(o.NumberValue);
            }
            return true;
        }

        private bool F_base_GetList(Operand args, List<string> list)
        {
            if (args.IsError) { return false; }
            if (args.Type == OperandType.ARRARY) {
                var o = F_base_GetList(args.ArrayValue, list);
                if (o == false) { return false; }
            } else if (args.Type == OperandType.JSON) {
                var i = args.ToArray(null);
                if (i.IsError) { return false; }
                var o = F_base_GetList(i.ArrayValue, list);
                if (o == false) { return false; }
            } else {
                var o = args.ToText(null);
                if (o.IsError) { return false; }
                list.Add(o.TextValue);
            }
            return true;
        }


        private bool F_base_GetList(List<Operand> args, List<string> list)
        {
            foreach (var item in args) {
                if (item.Type == OperandType.ARRARY) {
                    var o = F_base_GetList(item.ArrayValue, list);
                    if (o == false) { return false; }
                } else if (item.Type == OperandType.JSON) {
                    var i = item.ToArray(null);
                    if (i.IsError) { return false; }
                    var o = F_base_GetList(i.ArrayValue, list);
                    if (o == false) { return false; }
                } else {
                    var o = item.ToText(null);
                    if (o.IsError) { return false; }
                    list.Add(o.TextValue);
                }
            }
            return true;
        }

        #endregion

        #region csharp

        public Operand VisitURLENCODE_fun(mathParser.URLENCODE_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToText("Function URLENCODE parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(HttpUtility.UrlEncode(firstValue.TextValue));
        }
        public Operand VisitURLDECODE_fun(mathParser.URLDECODE_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToText("Function URLDECODE parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(HttpUtility.UrlDecode(firstValue.TextValue));
        }
        public Operand VisitHTMLENCODE_fun(mathParser.HTMLENCODE_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToText("Function HTMLENCODE parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(HttpUtility.HtmlEncode(firstValue.TextValue));
        }
        public Operand VisitHTMLDECODE_fun(mathParser.HTMLDECODE_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToText("Function HTMLDECODE parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(HttpUtility.HtmlDecode(firstValue.TextValue));
        }
        public Operand VisitBASE64TOTEXT_fun(mathParser.BASE64TOTEXT_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = this.Visit(item).ToText($"Function BASE64TOTEXT parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }

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
            return Operand.Error("Function HTMLDECODE is error!");
        }
        public Operand VisitBASE64URLTOTEXT_fun(mathParser.BASE64URLTOTEXT_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = this.Visit(item).ToText($"Function BASE64URLTOTEXT parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }
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
        public Operand VisitTEXTTOBASE64_fun(mathParser.TEXTTOBASE64_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = this.Visit(item).ToText($"Function TEXTTOBASE64 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }

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
        public Operand VisitTEXTTOBASE64URL_fun(mathParser.TEXTTOBASE64URL_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = this.Visit(item).ToText($"Function TEXTTOBASE64URL parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }

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
        public Operand VisitREGEX_fun(mathParser.REGEX_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToText("Function REGEX parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToText("Function REGEX parameter 2 is error!");
            if (secondValue.IsError) { return secondValue; }

                var b = Regex.Match(firstValue.TextValue, secondValue.TextValue);
                if (b.Success == false) {
                    return Operand.Error("Function REGEX is error!");
                }
                return Operand.Create(b.Value);
        }
        public Operand VisitREGEXREPALCE_fun(mathParser.REGEXREPALCE_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = this.Visit(item).ToText($"Function REGEXREPALCE parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }

            var b = Regex.Replace(args[0].TextValue, args[1].TextValue, args[2].TextValue);
            return Operand.Create(b);
        }
        public Operand VisitISREGEX_fun(mathParser.ISREGEX_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = this.Visit(item).ToText($"Function ISREGEX parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }

            var b = Regex.IsMatch(args[0].TextValue, args[1].TextValue);
            return Operand.Create(b);
        }
        public Operand VisitGUID_fun(mathParser.GUID_funContext context)
        {
            return Operand.Create(System.Guid.NewGuid().ToString());
        }
        public Operand VisitMD5_fun(mathParser.MD5_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = this.Visit(item).ToText($"Function MD5 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }

            try {
                Encoding encoding;
                if (args.Count == 1) {
                    encoding = Encoding.UTF8;
                } else {
                    encoding = Encoding.GetEncoding(args[1].TextValue);
                }
                var t = Hash.GetMd5String(encoding.GetBytes(args[0].TextValue));
                return Operand.Create(t);
            } catch (Exception) { }
            return Operand.Error("Function MD5 is error!");
        }
        public Operand VisitSHA1_fun(mathParser.SHA1_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = this.Visit(item).ToText($"Function SHA1 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }

            try {
                Encoding encoding;
                if (args.Count == 1) {
                    encoding = Encoding.UTF8;
                } else {
                    encoding = Encoding.GetEncoding(args[1].TextValue);
                }
                var t = Hash.GetSha1String(encoding.GetBytes(args[0].TextValue));
                return Operand.Create(t);
            } catch (Exception) {
            }
            return Operand.Error("Function SHA1 is error!");
        }
        public Operand VisitSHA256_fun(mathParser.SHA256_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = this.Visit(item).ToText($"Function SHA256 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }

            try {
                Encoding encoding;
                if (args.Count == 1) {
                    encoding = Encoding.UTF8;
                } else {
                    encoding = Encoding.GetEncoding(args[1].TextValue);
                }
                var t = Hash.GetSha256String(encoding.GetBytes(args[0].TextValue));
                return Operand.Create(t);
            } catch (Exception) { }
            return Operand.Error("Function SHA256 is error!");
        }
        public Operand VisitSHA512_fun(mathParser.SHA512_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = this.Visit(item).ToText($"Function SHA512 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }

            try {
                Encoding encoding;
                if (args.Count == 1) {
                    encoding = Encoding.UTF8;
                } else {
                    encoding = Encoding.GetEncoding(args[1].TextValue);
                }
                var t = Hash.GetSha512String(encoding.GetBytes(args[0].TextValue));
                return Operand.Create(t);
            } catch (Exception) {
            }
            return Operand.Error("Function SHA512 is error!");
        }
 
        public Operand VisitCRC32_fun(mathParser.CRC32_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = this.Visit(item).ToText($"Function CRC32 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }
            try {
                Encoding encoding;
                if (args.Count == 1) {
                    encoding = Encoding.UTF8;
                } else {
                    encoding = Encoding.GetEncoding(args[1].TextValue);
                }
                var t = Hash.GetCrc32String(encoding.GetBytes(args[0].TextValue));
                return Operand.Create(t);
            } catch (Exception) { }
            return Operand.Error("Function CRC32 is error!");
        }
        public Operand VisitHMACMD5_fun(mathParser.HMACMD5_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = this.Visit(item).ToText($"Function HMACMD5 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }
            try {
                Encoding encoding;
                if (args.Count == 2) {
                    encoding = Encoding.UTF8;
                } else {
                    encoding = Encoding.GetEncoding(args[2].TextValue);
                }
                var t = Hash.GetHmacMd5String(encoding.GetBytes(args[0].TextValue), args[1].TextValue);
                return Operand.Create(t);
            } catch (Exception) { }
            return Operand.Error("Function HMACMD5 is error!");
        }
        public Operand VisitHMACSHA1_fun(mathParser.HMACSHA1_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = this.Visit(item).ToText($"Function HMACSHA1 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }
            try {
                Encoding encoding;
                if (args.Count == 2) {
                    encoding = Encoding.UTF8;
                } else {
                    encoding = Encoding.GetEncoding(args[2].TextValue);
                }
                var t = Hash.GetHmacSha1String(encoding.GetBytes(args[0].TextValue), args[1].TextValue);
                return Operand.Create(t);
            } catch (Exception) { }
            return Operand.Error("Function HMACSHA1 is error!");
        }
        public Operand VisitHMACSHA256_fun(mathParser.HMACSHA256_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = this.Visit(item).ToText($"Function HMACSHA256 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }
            try {
                Encoding encoding;
                if (args.Count == 2) {
                    encoding = Encoding.UTF8;
                } else {
                    encoding = Encoding.GetEncoding(args[2].TextValue);
                }
                var t = Hash.GetHmacSha256String(encoding.GetBytes(args[0].TextValue), args[1].TextValue);
                return Operand.Create(t);
            } catch (Exception) {            }
            return Operand.Error("Function HMACSHA256 is error!");
        }
        public Operand VisitHMACSHA512_fun(mathParser.HMACSHA512_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = this.Visit(item).ToText($"Function HMACSHA512 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }
            try {
                Encoding encoding;
                if (args.Count == 2) {
                    encoding = Encoding.UTF8;
                } else {
                    encoding = Encoding.GetEncoding(args[2].TextValue);
                }
                var t = Hash.GetHmacSha512String(encoding.GetBytes(args[0].TextValue), args[1].TextValue);
                return Operand.Create(t);
            } catch (Exception) {            }
            return Operand.Error("Function HMACSHA512 is error!");
        }
        public Operand VisitTRIMSTART_fun(mathParser.TRIMSTART_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = this.Visit(item).ToText($"Function TRIMSTART parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }

            var text = args[0].TextValue;
            if (args.Count == 2) {
#if NETSTANDARD2_1
                return Operand.Create(text.AsSpan().TrimStart(args[1].TextValue.AsSpan()).ToString());
#else
                return Operand.Create(text.TrimStart(args[1].TextValue.ToArray()));
#endif
            }
#if NETSTANDARD2_1
            return Operand.Create(text.AsSpan().TrimStart().ToString());
#else
            return Operand.Create(text.TrimStart());
#endif
        }

        public Operand VisitTRIMEND_fun(mathParser.TRIMEND_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = this.Visit(item).ToText($"Function TRIMEND parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }

            var text = args[0].TextValue;
            if (args.Count == 2) {
#if NETSTANDARD2_1
                return Operand.Create(text.AsSpan().TrimEnd(args[1].TextValue.AsSpan()).ToString());
#else
                return Operand.Create(text.TrimEnd(args[1].TextValue.ToArray()));
#endif
            }
#if NETSTANDARD2_1
            return Operand.Create(text.AsSpan().TrimEnd().ToString());
#else
            return Operand.Create(text.TrimEnd());
#endif
        }

        public Operand VisitINDEXOF_fun(mathParser.INDEXOF_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToText("Function INDEXOF parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToText("Function INDEXOF parameter 2 is error!");
            if (secondValue.IsError) { return secondValue; }

            var text = firstValue.TextValue;
            if (args.Count == 2) {
#if NETSTANDARD2_1
                return Operand.Create(text.AsSpan().IndexOf(secondValue.TextValue) + excelIndex);
#else
                return Operand.Create(text.IndexOf(secondValue.TextValue) + excelIndex);
#endif
            }
            var thirdValue = args[2].ToText("Function INDEXOF parameter 3 is error!");
            if (thirdValue.IsError) { return thirdValue; }
            if (args.Count == 3) {
#if NETSTANDARD2_1
                return Operand.Create(text.AsSpan(thirdValue.IntValue).IndexOf(secondValue.TextValue) + thirdValue.IntValue + excelIndex);
#else
                return Operand.Create(text.IndexOf(secondValue.TextValue, thirdValue.IntValue) + excelIndex);
#endif
            }
            var fourthValue = args[3].ToText("Function INDEXOF parameter 4 is error!");
            if (fourthValue.IsError) { return fourthValue; }
            return Operand.Create(text.IndexOf(secondValue.TextValue, thirdValue.IntValue, fourthValue.IntValue) + excelIndex);
        }
        public Operand VisitLASTINDEXOF_fun(mathParser.LASTINDEXOF_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToText("Function LASTINDEXOF parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToText("Function LASTINDEXOF parameter 2 is error!");
            if (secondValue.IsError) { return secondValue; }

            var text = firstValue.TextValue;
            if (args.Count == 2) {
#if NETSTANDARD2_1
                return Operand.Create(text.AsSpan().LastIndexOf(secondValue.TextValue) + excelIndex);
#else
                return Operand.Create(text.LastIndexOf(secondValue.TextValue) + excelIndex);
#endif
            }
            var thirdValue = args[2].ToText("Function LASTINDEXOF parameter 3 is error!");
            if (thirdValue.IsError) { return thirdValue; }
            if (args.Count == 3) {
#if NETSTANDARD2_1
                return Operand.Create(text.AsSpan(thirdValue.IntValue).LastIndexOf(secondValue.TextValue) + thirdValue.IntValue + excelIndex);
#else
                return Operand.Create(text.LastIndexOf(secondValue.TextValue, thirdValue.IntValue) + excelIndex);
#endif
            }
            var fourthValue = args[3].ToText("Function LASTINDEXOF parameter 4 is error!");
            if (fourthValue.IsError) { return fourthValue; }
            return Operand.Create(text.LastIndexOf(secondValue.TextValue, thirdValue.IntValue, fourthValue.IntValue) + excelIndex);
        }
        public Operand VisitSPLIT_fun(mathParser.SPLIT_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = this.Visit(item).ToText($"Function SPLIT parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }
            return Operand.Create(args[0].TextValue.Split(args[1].TextValue.ToArray()));
        }
        public Operand VisitJOIN_fun(mathParser.JOIN_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0];
            if (firstValue.Type == OperandType.JSON) {
                var o = firstValue.ToArray(null);
                if (o.IsError == false) {
                    firstValue = o;
                }
            }
            if (firstValue.Type == OperandType.ARRARY) {
                List<string> list = new List<string>();
                var o = F_base_GetList(firstValue, list);
                if (o == false) return Operand.Error("Function JOIN parameter 1 is error!");

                var secondValue = args[1].ToText("Function JOIN parameter 2 is error!");
                if (secondValue.IsError) { return secondValue; }

                return Operand.Create(string.Join(secondValue.TextValue, list));
            } else {
                firstValue = firstValue.ToText("Function JOIN parameter 1 is error!");
                if (firstValue.IsError) { return firstValue; }

                List<string> list = new List<string>();
                for (int i = 1; i < args.Count; i++) {
                    var o = F_base_GetList(args[i], list);
                    if (o == false) return Operand.Error($"Function JOIN parameter {i + 1} is error!");
                }

                return Operand.Create(string.Join(firstValue.TextValue, list));
            }
        }
        public Operand VisitSUBSTRING_fun(mathParser.SUBSTRING_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToText("Function SUBSTRING parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToNumber("Function SUBSTRING parameter 2 is error!");
            if (secondValue.IsError) { return secondValue; }

            var text = firstValue.TextValue;
            if (args.Count == 2) {
#if NETSTANDARD2_1
                return Operand.Create(text.AsSpan(secondValue.IntValue - excelIndex).ToString());
#else
                return Operand.Create(text.Substring(secondValue.IntValue - excelIndex));
#endif
            }
            var thirdValue = args[2].ToNumber("Function SUBSTRING parameter 3 is error!");
            if (thirdValue.IsError) { return thirdValue; }
#if NETSTANDARD2_1
            return Operand.Create(text.AsSpan(secondValue.IntValue - excelIndex, thirdValue.IntValue).ToString());
#else
            return Operand.Create(text.Substring(secondValue.IntValue - excelIndex, thirdValue.IntValue));
#endif
        }
        public Operand VisitSTARTSWITH_fun(mathParser.STARTSWITH_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToText("Function STARTSWITH parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToText("Function STARTSWITH parameter 2 is error!");
            if (secondValue.IsError) { return secondValue; }

            var text = firstValue.TextValue;
            if (args.Count == 2) {
#if NETSTANDARD2_1
                return Operand.Create(text.AsSpan().StartsWith(secondValue.TextValue.AsSpan()));
#else
                return Operand.Create(text.StartsWith(secondValue.TextValue));
#endif
            }
            var thirdValue = args[2].ToBoolean("Function STARTSWITH parameter 3 is error!");
            if (thirdValue.IsError) { return thirdValue; }
#if NETSTANDARD2_1
            return Operand.Create(text.AsSpan().StartsWith(secondValue.TextValue.AsSpan(), thirdValue.BooleanValue ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
#else
            return Operand.Create(text.StartsWith(secondValue.TextValue, thirdValue.BooleanValue ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
#endif
        }
        public Operand VisitENDSWITH_fun(mathParser.ENDSWITH_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToText("Function ENDSWITH parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToText("Function ENDSWITH parameter 2 is error!");
            if (secondValue.IsError) { return secondValue; }

            var text = firstValue.TextValue;
            if (args.Count == 2) {
#if NETSTANDARD2_1
                return Operand.Create(text.AsSpan().EndsWith(secondValue.TextValue.AsSpan()));
#else
                return Operand.Create(text.EndsWith(secondValue.TextValue));
#endif
            }
            var thirdValue = args[2].ToBoolean("Function ENDSWITH parameter 3 is error!"); ;
            if (thirdValue.IsError) { return thirdValue; }
#if NETSTANDARD2_1
            return Operand.Create(text.AsSpan().EndsWith(secondValue.TextValue.AsSpan(), thirdValue.BooleanValue ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
#else
            return Operand.Create(text.EndsWith(secondValue.TextValue, thirdValue.BooleanValue ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
#endif
        }
        public Operand VisitISNULLOREMPTY_fun(mathParser.ISNULLOREMPTY_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToText("Function ISNULLOREMPTY parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(string.IsNullOrEmpty(firstValue.TextValue));
        }
        public Operand VisitISNULLORWHITESPACE_fun(mathParser.ISNULLORWHITESPACE_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToText("Function ISNULLORWHITESPACE parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }

            return Operand.Create(string.IsNullOrWhiteSpace(firstValue.TextValue));
        }
        public Operand VisitREMOVESTART_fun(mathParser.REMOVESTART_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToText("Function REMOVESTART parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToText("Function REMOVESTART parameter 2 is error!");
            if (secondValue.IsError) { return secondValue; }

            StringComparison comparison = StringComparison.Ordinal;
            if (args.Count == 3) {
                var thirdValue = args[2].ToBoolean("Function REMOVESTART parameter 3 is error!");
                if (thirdValue.IsError) { return thirdValue; }
                if (thirdValue.BooleanValue) {
                    comparison = StringComparison.OrdinalIgnoreCase;
                }
            }
            var text = firstValue.TextValue;
            if (text.StartsWith(secondValue.TextValue, comparison)) {
#if NETSTANDARD2_1
                return Operand.Create(text.AsSpan(secondValue.TextValue.Length).ToString());
#else
                return Operand.Create(text.Substring(secondValue.TextValue.Length));
#endif
            }
            return firstValue;
        }
        public Operand VisitREMOVEEND_fun(mathParser.REMOVEEND_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToText("Function REMOVEEND parameter 1 is error!");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToText("Function REMOVEEND parameter 2 is error!");
            if (secondValue.IsError) { return secondValue; }

            StringComparison comparison = StringComparison.Ordinal;
            if (args.Count == 3) {
                var thirdValue = args[2].ToBoolean("Function REMOVESTART parameter 3 is error!");
                if (thirdValue.IsError) { return thirdValue; }
                if (thirdValue.BooleanValue) {
                    comparison = StringComparison.OrdinalIgnoreCase;
                }
            }
            var text = firstValue.TextValue;
            if (text.EndsWith(secondValue.TextValue, comparison)) {
#if NETSTANDARD2_1
                return Operand.Create(text.AsSpan(0, text.Length - secondValue.TextValue.Length).ToString());
#else
                return Operand.Create(text.Substring(0, text.Length - secondValue.TextValue.Length));
#endif
            }
            return firstValue;
        }
        public Operand VisitJSON_fun(mathParser.JSON_funContext context)
        {
            var firstValue = this.Visit(context.expr()).ToText("Function JSON parameter is error!");
            if (firstValue.IsError) { return firstValue; }
            var txt = firstValue.TextValue;
            if ((txt.StartsWith("{") && txt.EndsWith("}")) || (txt.StartsWith("[") && txt.EndsWith("]"))) {
                try {
                    var json = JsonMapper.ToObject(txt);
                    return Operand.Create(json);
                } catch (Exception) { }
            }
            return Operand.Error("Function JSON parameter is error!");
        }

        #endregion

        #region Lookup
        public Operand VisitVLOOKUP_fun(mathParser.VLOOKUP_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToArray("Function VLOOKUP parameter 1 error!");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1];

            var thirdValue = args[2].ToNumber("Function VLOOKUP parameter 3 is error!");
            if (thirdValue.IsError) { return thirdValue; }

            var vague = true;
            if (args.Count == 4) {
                var fourthValue = args[2].ToBoolean("Function VLOOKUP parameter 4 is error!");
                if (fourthValue.IsError) { return fourthValue; }
                vague = fourthValue.BooleanValue;
            }
            if (secondValue.Type != OperandType.NULL) {
                var sv = secondValue.ToText("Function VLOOKUP parameter 2 is error!");
                if (sv.IsError) { return sv; }
                secondValue = sv;
            }

            foreach (var item in firstValue.ArrayValue) {
                var o = item.ToArray("Function VLOOKUP parameter 1 error!");
                if (o.IsError) { return o; }
                if (o.ArrayValue.Count > 0) {
                    var o1 = o.ArrayValue[0];
                    int b = -1;
                    if (secondValue.Type == OperandType.NUMBER) {
                        var o2 = o1.ToNumber(null);
                        if (o2.IsError == false) {
                            b = Compare(o2.NumberValue, secondValue.NumberValue);
                        }
                    } else {
                        var o2 = o1.ToText(null);
                        if (o2.IsError == false) {
                            b = string.CompareOrdinal(o2.TextValue, secondValue.TextValue);
                        }
                    }
                    if (b == 0) {
                        var index = thirdValue.IntValue - excelIndex;
                        if (index < o.ArrayValue.Count) {
                            return o.ArrayValue[index];
                        }
                    }
                }
            }

            if (vague) //进行模糊匹配
            {
                Operand last = null;
                var index = thirdValue.IntValue - excelIndex;
                foreach (var item in firstValue.ArrayValue) {
                    var o = item.ToArray(null);
                    if (o.IsError) { return o; }
                    if (o.ArrayValue.Count > 0) {
                        var o1 = o.ArrayValue[0];
                        int b = -1;
                        if (secondValue.Type == OperandType.NUMBER) {
                            var o2 = o1.ToNumber(null);
                            if (o2.IsError == false) {
                                b = Compare(o2.NumberValue, secondValue.NumberValue);
                            }
                        } else {
                            var o2 = o1.ToText(null);
                            if (o2.IsError == false) {
                                b = string.CompareOrdinal(o2.TextValue, secondValue.TextValue);
                            }
                        }
                        if (b < 0 && index < o.ArrayValue.Count) {
                            last = o;
                        } else if (b > 0 && last != null) {
                            return last.ArrayValue[index];
                        }
                    }
                }
            }
            return Operand.Error("Function VLOOKUP is not match !");
        }

        public Operand VisitLOOKUP_fun(mathParser.LOOKUP_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }

            var firstValue = args[0].ToArray("Function LOOKUP parameter 1 error!");
            if (firstValue.IsError) { return firstValue; }
            var secondValue = args[1].ToText("Function LOOKUP parameter 2 is error!");
            if (secondValue.IsError) { return secondValue; }
            var thirdValue = args[2].ToText("Function LOOKUP parameter 3 is error!");
            if (thirdValue.IsError) { return thirdValue; }

            if (string.IsNullOrWhiteSpace(secondValue.TextValue)) {
                return Operand.Error("Function LOOKUP parameter 2 is null!");
            }

            var engine = new LookupAlgorithmEngine();
            if (engine.Parse(secondValue.TextValue) == false) {
                return Operand.Error("Function LOOKUP parameter 2 Parse is error!");
            }
            engine.DiyFunction += (funcName, list) => {
                if (DiyFunction != null) {
                    return DiyFunction(funcName, list);
                }
                return Operand.Error($"Function name [{funcName}] is missing.");
            };

            foreach (var item in firstValue.ArrayValue) {
                var json = item.ToJson(null);
                if (json.IsError == false) {
                    engine.Json = json;
                    try {
                        var o = engine.Evaluate().ToBoolean(null);
                        if (o.IsError == false) {
                            if (o.BooleanValue) {
                                var v = json.JsonValue[thirdValue.TextValue];
                                if (v != null) {
                                    if (v.IsString) return Operand.Create(v.StringValue);
                                    if (v.IsBoolean) return Operand.Create(v.BooleanValue);
                                    if (v.IsDouble) return Operand.Create(v.NumberValue);
                                    if (v.IsObject) return Operand.Create(v);
                                    if (v.IsArray) return Operand.Create(v);
                                    if (v.IsNull) return Operand.CreateNull();
                                    return Operand.Create(v);
                                }
                            }
                        }
                    } catch (Exception) { }
                }
            }
            return Operand.Error("Function LOOKUP not find!");
        }

        #endregion

        #region getValue

        public Operand VisitArray_fun(mathParser.Array_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = this.Visit(item); if (aa.IsError) { return aa; } args.Add(aa); }
            return Operand.Create(args);
        }
        public Operand VisitBracket_fun(mathParser.Bracket_funContext context)
        {
            return this.Visit(context.expr());
        }
        public Operand VisitNUM_fun(mathParser.NUM_funContext context)
        {
            var sub = context.SUB()?.GetText() ?? "";
            var t = context.NUM().GetText();
            var d = double.Parse(sub + t, NumberStyles.Any, cultureInfo);
            return Operand.Create(d);
        }
        public Operand VisitSTRING_fun(mathParser.STRING_funContext context)
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
        public Operand VisitNULL_fun(mathParser.NULL_funContext context)
        {
            return Operand.CreateNull();
        }
        public Operand VisitPARAMETER_fun(mathParser.PARAMETER_funContext context)
        {
            var p = this.Visit(context.parameter()).ToText("Function PARAMETER first parameter is error!");
            if (p.IsError) return p;

            if (GetParameter != null) {
                return GetParameter(p.TextValue);
            }
            return Operand.Error("Function PARAMETER first parameter is error!");
        }

        public Operand VisitParameter(mathParser.ParameterContext context)
        {
            var expr = context.expr();
            if (expr != null) {
                return this.Visit(expr);
            }
            return this.Visit(context.parameter2());
        }

        public Operand VisitParameter2(mathParser.Parameter2Context context)
        {
            return Operand.Create(context.children[0].GetText());
        }

        public Operand VisitGetJsonValue_fun(mathParser.GetJsonValue_funContext context)
        {
            var firstValue = this.Visit(context.expr());
            if (firstValue.IsError) { return firstValue; }

            var obj = firstValue;
            Operand op;
            var p1 = context.parameter();
            if (p1 != null) {
                op = this.Visit(p1);
            } else {
                op = this.Visit(context.parameter2());
            }

            if (obj.Type == OperandType.ARRARY) {
                op = op.ToNumber("ARRARY index is error!");
                if (op.IsError) { return op; }
                var index = op.IntValue - excelIndex;
                if (index < obj.ArrayValue.Count)
                    return obj.ArrayValue[index];
                return Operand.Error($"ARRARY index {index} greater than maximum length!");
            }
            if (obj.Type == OperandType.JSON) {
                var json = obj.JsonValue;
                if (json.IsArray) {
                    op = op.ToNumber("JSON parameter index is error!");
                    if (op.IsError) { return op; }
                    var index = op.IntValue - excelIndex;
                    if (index < json.Count) {
                        var v = json[op.IntValue - excelIndex];
                        if (v.IsString) return Operand.Create(v.StringValue);
                        if (v.IsBoolean) return Operand.Create(v.BooleanValue);
                        if (v.IsDouble) return Operand.Create(v.NumberValue);
                        if (v.IsObject) return Operand.Create(v);
                        if (v.IsArray) return Operand.Create(v);
                        if (v.IsNull) return Operand.CreateNull();
                        return Operand.Create(v);
                    }
                    return Operand.Error($"JSON index {index} greater than maximum length!");
                } else {
                    op = op.ToText("JSON parameter name is error!");
                    if (op.IsError) { return op; }
                    var v = json[op.TextValue];
                    if (v != null) {
                        if (v.IsString) return Operand.Create(v.StringValue);
                        if (v.IsBoolean) return Operand.Create(v.BooleanValue);
                        if (v.IsDouble) return Operand.Create(v.NumberValue);
                        if (v.IsObject) return Operand.Create(v);
                        if (v.IsArray) return Operand.Create(v);
                        if (v.IsNull) return Operand.CreateNull();
                        return Operand.Create(v);
                    }
                }
            }
            return Operand.Error(" Operator is error!");
        }

        public Operand VisitExpr2_fun(mathParser.Expr2_funContext context)
        {
            return VisitChildren(context);
        }

        public Operand VisitDiyFunction_fun(mathParser.DiyFunction_funContext context)
        {
            if (DiyFunction != null) {
                var funName = context.PARAMETER().GetText();
                var args = new List<Operand>();
                foreach (var item in context.expr()) { var aa = this.Visit(item); args.Add(aa); }
                return DiyFunction(funName, args);
            }
            return Operand.Error("DiyFunction is error!");
        }





        #endregion
    }
}
