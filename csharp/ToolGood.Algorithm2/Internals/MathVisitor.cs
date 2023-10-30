using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.LitJson;
using ToolGood.Algorithm.math;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals
{
    public class MathVisitor : AbstractParseTreeVisitor<Operand>, ImathVisitor<Operand>
    {
        public event Func<string, Operand> GetParameter;
        public event Func<string, List<Operand>, Operand> DiyFunction;
        public int excelIndex;
        public bool useLocalTime;
        public DistanceUnitType DistanceUnit = DistanceUnitType.M;
        public AreaUnitType AreaUnit = AreaUnitType.M2;
        public VolumeUnitType VolumeUnit = VolumeUnitType.M3;
        public MassUnitType MassUnit = MassUnitType.KG;

        #region base

        public virtual Operand VisitProg(mathParser.ProgContext context)
        {
            return context.expr().Accept(this);
        }

        public virtual Operand VisitMulDiv_fun(mathParser.MulDiv_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.IsError) { return args1; }
            var args2 = exprs[1].Accept(this); if (args2.IsError) { return args2; }

            if (args1.Type == OperandType.TEXT) {
                if (decimal.TryParse(args1.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    args1 = Operand.Create(d);
                } else if (bool.TryParse(args1.TextValue, out bool b)) {
                    args1 = b ? Operand.One : Operand.Zero;
                } else if (TimeSpan.TryParse(args1.TextValue, CultureInfo.InvariantCulture, out TimeSpan ts)) {
                    args1 = Operand.Create(ts);
                } else if (DateTime.TryParse(args1.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
                    args1 = Operand.Create(new MyDate(dt));
                } else {
                    return Operand.Error("Two types cannot be multiplied or divided.");
                }
            }
            if (args2.Type == OperandType.TEXT) {
                if (decimal.TryParse(args2.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    args2 = Operand.Create(d);
                } else if (bool.TryParse(args2.TextValue, out bool b)) {
                    args2 = b ? Operand.One : Operand.Zero;
                } else if (TimeSpan.TryParse(args2.TextValue, CultureInfo.InvariantCulture, out TimeSpan ts)) {
                    args2 = Operand.Create(ts);
                } else if (DateTime.TryParse(args2.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
                    args2 = Operand.Create(new MyDate(dt));
                } else {
                    return Operand.Error("Two types cannot be multiplied or divided.");
                }
            }
            var t = context.op.Text;
            if (CharUtil.Equals(t, '*')) {
                if (args1.Type == OperandType.DATE) {
                    if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '*' parameter 2 is error!"); if (args2.IsError) { return args2; } }
                    if (args2.NumberValue == 1) { return args1; }
                    return Operand.Create((MyDate)(args1.DateValue * args2.NumberValue));
                } else if (args2.Type == OperandType.DATE) {
                    if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '*' parameter 1 is error!"); if (args1.IsError) { return args1; } }
                    if (args1.NumberValue == 1) { return args2; }
                    return Operand.Create((MyDate)(args2.DateValue * args1.NumberValue));
                }

                if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '*' parameter 1 is error!"); if (args1.IsError) { return args1; } }
                if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '*' parameter 2 is error!"); if (args2.IsError) { return args2; } }
                if (args1.NumberValue == 1) { return args2; }
                if (args2.NumberValue == 1) { return args1; }

                return Operand.Create(args1.NumberValue * args2.NumberValue);
            } else if (CharUtil.Equals(t, '/')) {
                if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '/' parameter 2 is error!"); if (args2.IsError) { return args2; } }
                if (args2.NumberValue == 0) { return Operand.Error("Div 0 is error!"); }
                if (args2.NumberValue == 1) { return args1; }

                if (args1.Type == OperandType.DATE) { return Operand.Create(args1.DateValue / args2.NumberValue); }
                if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '/' parameter 1 is error!"); if (args1.IsError) { return args1; } }

                return Operand.Create(args1.NumberValue / args2.NumberValue);
                //} else if (CharUtil.Equals(t, "%")) {
            } else {
                if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function % parameter 1 is error!"); if (args1.IsError) { return args1; } }
                if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function % parameter 2 is error!"); if (args2.IsError) { return args2; } }
                if (args2.NumberValue == 0) { return Operand.Error("Div 0 is error!"); }

                return Operand.Create(args1.NumberValue % args2.NumberValue);
            }
        }

        public virtual Operand VisitAddSub_fun(mathParser.AddSub_funContext context)
        {
            var exprs = context.expr();

            var args1 = exprs[0].Accept(this); if (args1.IsError) { return args1; }
            var args2 = exprs[1].Accept(this); if (args2.IsError) { return args2; }
            var t = context.op.Text;
            if (CharUtil.Equals(t, '&')) {
                if (args1.IsNull) {
                    if (args2.IsNull) return args1;
                    return args2.ToText("Function '&' parameter 2 is error!");
                } else if (args2.IsNull) {
                    return args1.ToText("Function '&' parameter 1 is error!");
                }
                if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function '&' parameter 1 is error!"); if (args1.IsError) { return args1; } }
                if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function '&' parameter 2 is error!"); if (args2.IsError) { return args2; } }
                return Operand.Create(args1.TextValue + args2.TextValue);
            }
            if (args1.Type == OperandType.TEXT) {
                if (decimal.TryParse(args1.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    args1 = Operand.Create(d);
                } else if (bool.TryParse(args1.TextValue, out bool b)) {
                    args1 = b ? Operand.One : Operand.Zero;
                } else if (TimeSpan.TryParse(args1.TextValue, CultureInfo.InvariantCulture, out TimeSpan ts)) {
                    args1 = Operand.Create(ts);
                } else if (DateTime.TryParse(args1.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
                    args1 = Operand.Create(new MyDate(dt));
                } else {
                    return Operand.Error("Function '+' or '-' is error");
                }
            }
            if (args2.Type == OperandType.TEXT) {
                if (decimal.TryParse(args2.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    args2 = Operand.Create(d);
                } else if (bool.TryParse(args2.TextValue, out bool b)) {
                    args2 = b ? Operand.One : Operand.Zero;
                } else if (TimeSpan.TryParse(args2.TextValue, CultureInfo.InvariantCulture, out TimeSpan ts)) {
                    args2 = Operand.Create(ts);
                } else if (DateTime.TryParse(args2.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
                    args2 = Operand.Create(new MyDate(dt));
                } else {
                    return Operand.Error("Function '+' or '-' is error");
                }
            }
            if (CharUtil.Equals(t, '+')) {
                if (args1.Type == OperandType.DATE) {
                    if (args2.Type == OperandType.DATE) return Operand.Create(args1.DateValue + args2.DateValue);
                    if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '+' parameter 2 is error!"); if (args2.IsError) { return args2; } }
                    if (args2.NumberValue == 0) { return args1; }
                    return Operand.Create(args1.DateValue + args2.NumberValue);
                } else if (args2.Type == OperandType.DATE) {
                    if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '+' parameter 1 is error!"); if (args1.IsError) { return args1; } }
                    if (args1.NumberValue == 0) { return args2; }
                    return Operand.Create(args2.DateValue + args1.NumberValue);
                }
                if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '+' parameter 1 is error!"); if (args1.IsError) { return args1; } }
                if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '+' parameter 2 is error!"); if (args2.IsError) { return args2; } }
                if (args1.NumberValue == 0) { return args2; }
                if (args2.NumberValue == 0) { return args1; }

                return Operand.Create(args1.NumberValue + args2.NumberValue);
                //} else if (CharUtil.Equals(t, "-")) {
            } else {
                if (args1.Type == OperandType.DATE) {
                    if (args2.Type == OperandType.DATE) return Operand.Create(args1.DateValue - args2.DateValue);
                    if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '-' parameter 2 is error!"); if (args2.IsError) { return args2; } }
                    if (args2.NumberValue == 0) { return args1; }
                    return Operand.Create(args1.DateValue - args2.NumberValue);
                } else if (args2.Type == OperandType.DATE) {
                    if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '-' parameter 1 is error!"); if (args1.IsError) { return args1; } }
                    return Operand.Create(args1.NumberValue - args2.DateValue);
                }
                if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '-' parameter 1 is error!"); if (args1.IsError) { return args1; } }
                if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '-' parameter 2 is error!"); if (args2.IsError) { return args2; } }
                if (args2.NumberValue == 0) { return args1; }

                return Operand.Create(args1.NumberValue - args2.NumberValue);
            }
        }

        public virtual Operand VisitJudge_fun(mathParser.Judge_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.IsError) { return args1; }
            var args2 = exprs[1].Accept(this); if (args2.IsError) { return args2; }

            string type = context.op.Text;
            int r;
            if (args1.Type == args2.Type) {
                if (args1.Type == OperandType.NUMBER) {
                    return MathVisitor.Compare(args1, args2, type);
                } else if (args1.Type == OperandType.TEXT) {
                    r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
                } else if (args1.Type == OperandType.DATE || args1.Type == OperandType.BOOLEAN) {
                    args1 = args1.ToNumber();
                    args2 = args2.ToNumber();
                    return MathVisitor.Compare(args1, args2, type);
                } else if (args1.Type == OperandType.JSON) {
                    args1 = args1.ToText();
                    args2 = args2.ToText();
                    r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
                } else if (args1.Type == OperandType.NULL) {
                    return CharUtil.Equals(type, "=", "==", "===") ? Operand.True : Operand.False;
                } else {
                    return Operand.Error("Function compare is error.");
                }
            } else if (args1.Type == OperandType.NULL || args2.Type == OperandType.NULL) {
                return CharUtil.Equals(type, "<>", "!=", "!==") ? Operand.True : Operand.False;
            } else if (args1.Type == OperandType.TEXT) {
                if (args2.Type == OperandType.BOOLEAN) {
                    var a = args1.ToBoolean();
                    if (a.IsError == false) {
                        if (CharUtil.Equals(type, "=", "==", "===")) {
                            return a.BooleanValue == args2.BooleanValue ? Operand.True : Operand.False;
                        } else if (CharUtil.Equals(type, "<>", "!=", "!==")) {
                            return a.BooleanValue != args2.BooleanValue ? Operand.True : Operand.False;
                        }
                    }
                    args2 = args2.ToText();
                    r = string.Compare(args1.TextValue, args2.TextValue, true);
                } else if (args2.Type == OperandType.DATE || args2.Type == OperandType.NUMBER || args2.Type == OperandType.JSON) {
                    args2 = args2.ToText();
                    r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
                } else {
                    return Operand.Error("Function compare is error.");
                }
            } else if (args2.Type == OperandType.TEXT) {
                if (args1.Type == OperandType.BOOLEAN) {
                    var a = args2.ToBoolean();
                    if (a.IsError == false) {
                        if (CharUtil.Equals(type, "=", "==", "===")) {
                            return a.BooleanValue == args1.BooleanValue ? Operand.True : Operand.False;
                        } else if (CharUtil.Equals(type, "<>", "!=", "!==")) {
                            return a.BooleanValue != args1.BooleanValue ? Operand.True : Operand.False;
                        }
                    }
                    args1 = args1.ToText();
                    r = string.Compare(args1.TextValue, args2.TextValue, true);
                } else if (args1.Type == OperandType.DATE || args1.Type == OperandType.NUMBER || args1.Type == OperandType.JSON) {
                    args1 = args1.ToText();
                    r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
                } else {
                    return Operand.Error("Function compare is error.");
                }
            } else if (args1.Type == OperandType.JSON || args2.Type == OperandType.JSON
                  || args1.Type == OperandType.ARRARY || args2.Type == OperandType.ARRARY
                  || args1.Type == OperandType.ARRARYJSON || args2.Type == OperandType.ARRARYJSON
                  ) {
                return Operand.Error("Function compare is error.");
            } else {
                if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber($"Function '{type}' parameter 1 is error!"); if (args1.IsError) { return args1; } }
                if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber($"Function '{type}' parameter 2 is error!"); if (args2.IsError) { return args2; } }

                return MathVisitor.Compare(args1, args2, type);
            }
            if (type.Length == 1) {
                if (CharUtil.Equals(type, '<')) {
                    return Operand.Create(r < 0);
                } else if (CharUtil.Equals(type, '>')) {
                    return Operand.Create(r > 0);
                } else /*if (CharUtil.Equals(type, '=')*/{
                    return Operand.Create(r == 0);
                }
            } else if (CharUtil.Equals(type, "<=")) {
                return Operand.Create(r <= 0);
            } else if (CharUtil.Equals(type, ">=")) {
                return Operand.Create(r >= 0);
            } else if (CharUtil.Equals(type, "==", "===")) {
                return Operand.Create(r == 0);
            }
            return Operand.Create(r != 0);
        }
        private static Operand Compare(Operand op1, Operand op2, string type)
        {
            var t1 = op1.NumberValue;
            var t2 = op2.NumberValue;

            var r = t1 - t2;
            if (type.Length == 1) {
                if (CharUtil.Equals(type, '<')) {
                    return Operand.Create(r < 0);
                } else if (CharUtil.Equals(type, '>')) {
                    return Operand.Create(r > 0);
                } else /*if (CharUtil.Equals(type, '=')*/{
                    return Operand.Create(r == 0);
                }
            } else if (CharUtil.Equals(type, "<=")) {
                return Operand.Create(r <= 0);
            } else if (CharUtil.Equals(type, ">=")) {
                return Operand.Create(r >= 0);
            } else if (CharUtil.Equals(type, "==", "===")) {
                return Operand.Create(r == 0);
            }
            return Operand.Create(r != 0);
        }

        private static int Compare(decimal t1, decimal t2)
        {
            var b = t1 - t2;
            //var b = Math.Round(t1 - t2, 12, MidpointRounding.AwayFromZero);
            if (b == 0) {
                return 0;
            } else if (b > 0) {
                return 1;
            }
            return -1;
        }

        public virtual Operand VisitAndOr_fun(mathParser.AndOr_funContext context)
        {
            // 程序 && and || or 与 excel的  AND(x,y) OR(x,y) 有区别
            // 在excel内 AND(x,y) OR(x,y) 先报错，
            // 在程序中，&& and  有true 直接返回true 就不会检测下一个会不会报错
            // 在程序中，|| or  有false 直接返回false 就不会检测下一个会不会报错
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.BOOLEAN) { args1 = args1.ToBoolean(); if (args1.IsError) { return args1; } }
            var t = context.op.Text;
            if (CharUtil.Equals(t, "&&", "AND")) {
                if (args1.BooleanValue == false) return Operand.False;
            } else {
                if (args1.BooleanValue) return Operand.True;
            }
            return exprs[1].Accept(this).ToBoolean();
        }

        #endregion

        #region flow
        public virtual Operand VisitIF_fun(mathParser.IF_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.BOOLEAN) { args1 = args1.ToBoolean("Function IF first parameter is error!"); if (args1.IsError) { return args1; } }

            if (args1.BooleanValue) {
                if (exprs.Length > 1) { return exprs[1].Accept(this); }
                return Operand.True;
            }
            if (exprs.Length == 3) { return exprs[2].Accept(this); }
            return Operand.False;
        }

        public virtual Operand VisitIFERROR_fun(mathParser.IFERROR_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);

            if (args1.IsError) { return exprs[1].Accept(this); }
            if (exprs.Length == 3) { return exprs[2].Accept(this); }
            return Operand.False;
        }
        public virtual Operand VisitISNUMBER_fun(mathParser.ISNUMBER_funContext context)
        {
            var args1 = this.Visit(context.expr()); if (args1.IsError) { return args1; }

            if (args1.Type == OperandType.NUMBER) { return Operand.True; }
            return Operand.False;
        }
        public virtual Operand VisitISTEXT_fun(mathParser.ISTEXT_funContext context)
        {
            var args1 = this.Visit(context.expr()); if (args1.IsError) { return args1; }

            if (args1.Type == OperandType.TEXT) { return Operand.True; }
            return Operand.False;
        }
        public virtual Operand VisitISERROR_fun(mathParser.ISERROR_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);

            if (exprs.Length == 2) {
                if (args1.IsError) { return exprs[1].Accept(this); }
                return args1;
            }
            if (args1.IsError) { return Operand.True; }
            return Operand.False;
        }

        public virtual Operand VisitISNULL_fun(mathParser.ISNULL_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);

            if (exprs.Length == 2) {
                if (args1.IsNull) { return exprs[1].Accept(this); }
                return args1;
            }
            if (args1.IsNull) { return Operand.True; }
            return Operand.False;
        }

        public virtual Operand VisitISNULLORERROR_fun(mathParser.ISNULLORERROR_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);

            if (exprs.Length == 2) {
                if (args1.IsNull || args1.IsError) { return exprs[1].Accept(this); }
                return args1;
            }
            if (args1.IsNull || args1.IsError) { return Operand.True; }
            return Operand.False;
        }

        public virtual Operand VisitISEVEN_fun(mathParser.ISEVEN_funContext context)
        {
            var args1 = context.expr().Accept(this);
            if (args1.Type == OperandType.NUMBER) {
                if (args1.IntValue % 2 == 0) { return Operand.True; }
            }
            return Operand.False;
        }

        public virtual Operand VisitISLOGICAL_fun(mathParser.ISLOGICAL_funContext context)
        {
            var args1 = context.expr().Accept(this);
            if (args1.Type == OperandType.BOOLEAN) { return Operand.True; }
            return Operand.False;
        }

        public virtual Operand VisitISODD_fun(mathParser.ISODD_funContext context)
        {
            var args1 = context.expr().Accept(this);
            if (args1.Type == OperandType.NUMBER) {
                if (args1.IntValue % 2 == 1) { return Operand.True; }
            }
            return Operand.False;
        }

        public virtual Operand VisitISNONTEXT_fun(mathParser.ISNONTEXT_funContext context)
        {
            var args1 = context.expr().Accept(this);
            if (args1.Type != OperandType.TEXT) { return Operand.True; }
            return Operand.False;
        }

        public virtual Operand VisitAND_fun(mathParser.AND_funContext context)
        {
            var index = 1;
            bool b = true;
            foreach (var item in context.expr()) {
                var a = item.Accept(this).ToBoolean($"Function AND parameter {index++} is error!");
                if (a.IsError) { return a; }
                if (a.BooleanValue == false) b = false;
            }
            return b ? Operand.True : Operand.False;
        }
        public virtual Operand VisitOR_fun(mathParser.OR_funContext context)
        {
            var index = 1;
            bool b = false;
            foreach (var item in context.expr()) {
                var a = item.Accept(this).ToBoolean($"Function OR parameter {index++} is error!");
                if (a.IsError) { return a; }
                if (a.BooleanValue) b = true;
            }
            return b ? Operand.True : Operand.False;
        }
        public virtual Operand VisitNOT_fun(mathParser.NOT_funContext context)
        {
            var args1 = context.expr().Accept(this).ToBoolean("Function NOT parameter is error!");
            if (args1.IsError) { return args1; }
            return Operand.Create(!args1.BooleanValue);
        }
        public virtual Operand VisitTRUE_fun(mathParser.TRUE_funContext context)
        {
            return Operand.True;
        }
        public virtual Operand VisitFALSE_fun(mathParser.FALSE_funContext context)
        {
            return Operand.False;
        }
        #endregion

        #region math

        #region base

        public virtual Operand VisitE_fun(mathParser.E_funContext context)
        {
            return Operand.Create(Math.E);
        }
        public virtual Operand VisitPI_fun(mathParser.PI_funContext context)
        {
            return Operand.Create(Math.PI);
        }

        public virtual Operand VisitABS_fun(mathParser.ABS_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ABS parameter is error!"); if (args1.IsError) { return args1; } }

            return Operand.Create(Math.Abs(args1.NumberValue));
        }
        public virtual Operand VisitQUOTIENT_fun(mathParser.QUOTIENT_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function QUOTIENT parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function QUOTIENT parameter 2 is error!"); if (args2.IsError) { return args2; } }

            if (args2.NumberValue == 0) {
                return Operand.Error("Function QUOTIENT div 0 error!");
            }
            return Operand.Create((int)(args1.NumberValue / args2.NumberValue));
        }
        public virtual Operand VisitMOD_fun(mathParser.MOD_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function MOD parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function MOD parameter 2 is error!"); if (args2.IsError) { return args2; } }

            if (args2.NumberValue == 0) {
                return Operand.Error("Function MOD div 0 error!");
            }
            return Operand.Create((int)(args1.NumberValue % args2.NumberValue));
        }
        public virtual Operand VisitSIGN_fun(mathParser.SIGN_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function SIGN parameter is error!"); if (args1.IsError) { return args1; } }

            return Operand.Create(Math.Sign(args1.NumberValue));
        }
        public virtual Operand VisitSQRT_fun(mathParser.SQRT_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function SQRT parameter is error!"); if (args1.IsError) { return args1; } }

            return Operand.Create(Math.Sqrt((double)args1.NumberValue));
        }
        public virtual Operand VisitTRUNC_fun(mathParser.TRUNC_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function TRUNC parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create((int)(args1.NumberValue));
        }
        public virtual Operand VisitINT_fun(mathParser.INT_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function INT parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create((int)(args1.NumberValue));
        }
        public virtual Operand VisitGCD_fun(mathParser.GCD_funContext context)
        {
            var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
            for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function GCD parameter is error!"); }

            return Operand.Create(F_base_gcd(list));
        }
        public virtual Operand VisitLCM_fun(mathParser.LCM_funContext context)
        {
            var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
            for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function GCD parameter is error!"); }

            return Operand.Create(F_base_lgm(list));
        }
        public virtual Operand VisitCOMBIN_fun(mathParser.COMBIN_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function COMBIN parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function COMBIN parameter 2 is error!"); if (args2.IsError) { return args2; } }

            var total = args1.IntValue;
            var count = args2.IntValue;
            decimal sum = 1;
            decimal sum2 = 1;
            for (int i = 0; i < count; i++) {
                sum *= (total - i);
                sum2 *= (i + 1);
            }
            return Operand.Create(sum / sum2);
        }
        public virtual Operand VisitPERMUT_fun(mathParser.PERMUT_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function PERMUT parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function PERMUT parameter 2 is error!"); if (args2.IsError) { return args2; } }

            var total = args1.IntValue;
            var count = args2.IntValue;

            double sum = 1;
            for (int i = 0; i < count; i++) {
                sum *= (total - i);
            }
            return Operand.Create(sum);
        }

        public virtual Operand VisitPercentage_fun(mathParser.Percentage_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function Percentage parameter is error!"); if (args1.IsError) { return args1; } }

            return Operand.Create(args1.NumberValue / 100.0m);
        }

        private int F_base_gcd(List<decimal> list)
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
        private int F_base_lgm(List<decimal> list)
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
        public virtual Operand VisitDEGREES_fun(mathParser.DEGREES_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function DEGREES parameter is error!"); if (args1.IsError) { return args1; } }

            var z = (double)args1.NumberValue;
            var r = (z / Math.PI * 180);
            return Operand.Create(r);
        }
        public virtual Operand VisitRADIANS_fun(mathParser.RADIANS_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function RADIANS parameter is error!"); if (args1.IsError) { return args1; } }

            var r = (double)args1.NumberValue / 180 * Math.PI;
            return Operand.Create(r);
        }
        public virtual Operand VisitCOS_fun(mathParser.COS_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function COS parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Cos((double)args1.NumberValue));
        }
        public virtual Operand VisitCOSH_fun(mathParser.COSH_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function COSH parameter is error!"); if (args1.IsError) { return args1; } }

            return Operand.Create(Math.Cosh((double)args1.NumberValue));
        }
        public virtual Operand VisitSIN_fun(mathParser.SIN_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function SIN parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Sin((double)args1.NumberValue));
        }
        public virtual Operand VisitSINH_fun(mathParser.SINH_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function SINH parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Sinh((double)args1.NumberValue));
        }

        public virtual Operand VisitTAN_fun(mathParser.TAN_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function TAN parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Tan((double)args1.NumberValue));
        }

        public virtual Operand VisitTANH_fun(mathParser.TANH_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function TANH parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Tanh((double)args1.NumberValue));
        }

        public virtual Operand VisitACOS_fun(mathParser.ACOS_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ACOS parameter is error!"); if (args1.IsError) { return args1; } }
            var x = args1.NumberValue;
            if (x < -1 || x > 1) {
                return Operand.Error("Function ACOS parameter is error!");
            }
            return Operand.Create(Math.Acos((double)x));
        }
        public virtual Operand VisitACOSH_fun(mathParser.ACOSH_funContext context)
        {
            var args1 = context.expr().Accept(this).ToNumber("Function ACOSH parameter is error!");
            if (args1.IsError) { return args1; }

            var z = args1.NumberValue;
            if (z < 1) {
                return Operand.Error("Function ACOSH parameter is error!");
            }
            return Operand.Create(Math.Acosh((double)z));
        }
        public virtual Operand VisitASIN_fun(mathParser.ASIN_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ASIN parameter is error!"); if (args1.IsError) { return args1; } }
            var x = args1.NumberValue;
            if (x < -1 || x > 1) {
                return Operand.Error("Function ASIN parameter is error!");
            }
            return Operand.Create(Math.Asin((double)x));
        }
        public virtual Operand VisitASINH_fun(mathParser.ASINH_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ASINH parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Asinh((double)args1.NumberValue));
        }

        public virtual Operand VisitATAN_fun(mathParser.ATAN_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ATAN parameter is error!"); if (args1.IsError) { return args1; } }

            return Operand.Create(Math.Atan((double)args1.NumberValue));
        }

        public virtual Operand VisitATANH_fun(mathParser.ATANH_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ATANH parameter is error!"); if (args1.IsError) { return args1; } }
            var x = args1.NumberValue;
            if (x >= 1 || x <= -1) {
                return Operand.Error("Function ATANH parameter is error!");
            }
            return Operand.Create(Math.Atanh((double)x));
        }
        public virtual Operand VisitATAN2_fun(mathParser.ATAN2_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ATAN2 parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function ATAN2 parameter 2 is error!"); if (args2.IsError) { return args2; } }

            return Operand.Create(Math.Atan2((double)args2.NumberValue, (double)args1.NumberValue));
        }
        public virtual Operand VisitFIXED_fun(mathParser.FIXED_funContext context)
        {
            var exprs = context.expr();
            var num = 2;
            if (exprs.Length > 1) {
                var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function FIXED parameter 2 is error!"); if (args2.IsError) { return args2; } }
                num = args2.IntValue;
            }
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function FIXED parameter 1 is error!"); if (args1.IsError) { return args1; } }

            var s = Math.Round(args1.NumberValue, num, MidpointRounding.AwayFromZero);
            var no = false;
            if (exprs.Length == 3) {
                var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.BOOLEAN) { args3 = args3.ToBoolean("Function FIXED parameter 3 is error!"); if (args3.IsError) { return args3; } }
                no = args3.BooleanValue;
            }
            if (no == false) {
                return Operand.Create(s.ToString('N' + num.ToString(), CultureInfo.InvariantCulture));
            }
            return Operand.Create(s.ToString(CultureInfo.InvariantCulture));
        }

        #endregion

        #region transformation
        public virtual Operand VisitBIN2OCT_fun(mathParser.BIN2OCT_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0].ToText("Function BIN2OCT parameter 1 is error!");
            if (args1.IsError) { return args1; }

            if (Regex.IsMatch(args1.TextValue, "^[01]+$") == false) { return Operand.Error("Function BIN2OCT parameter 1 is error!"); }
            var num = Convert.ToString(Convert.ToInt32(args1.TextValue, 2), 8);
            if (args.Count == 2) {
                var args2 = args[1].ToNumber("Function BIN2OCT parameter 2 is error!");
                if (args2.IsError) { return args2; }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function BIN2OCT parameter 2 is error!");
            }
            return Operand.Create(num);
        }
        public virtual Operand VisitBIN2DEC_fun(mathParser.BIN2DEC_funContext context)
        {
            var args1 = context.expr().Accept(this).ToText("Function BIN2DEC parameter is error!");
            if (args1.IsError) { return args1; }

            if (Regex.IsMatch(args1.TextValue, "^[01]+$") == false) { return Operand.Error("Function BIN2DEC parameter is error!"); }
            var num = Convert.ToInt32(args1.TextValue, 2);
            return Operand.Create(num);
        }
        public virtual Operand VisitBIN2HEX_fun(mathParser.BIN2HEX_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0].ToText("Function BIN2HEX parameter 1 is error!");
            if (args1.IsError) { return args1; }

            if (Regex.IsMatch(args1.TextValue, "^[01]+$") == false) { return Operand.Error("Function BIN2HEX parameter 1 is error!"); }
            var num = Convert.ToString(Convert.ToInt32(args1.TextValue, 2), 16).ToUpper();
            if (args.Count == 2) {
                var args2 = args[1].ToNumber("Function BIN2HEX parameter 2 is error!");
                if (args2.IsError) { return args2; }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function BIN2HEX parameter 2 is error!");
            }
            return Operand.Create(num);
        }

        public virtual Operand VisitOCT2BIN_fun(mathParser.OCT2BIN_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0].ToText("Function OCT2BIN parameter 1 is error!");
            if (args1.IsError) { return args1; }

            if (Regex.IsMatch(args1.TextValue, "^[0-8]+$") == false) { return Operand.Error("Function OCT2BIN parameter 1 is error!"); }
            var num = Convert.ToString(Convert.ToInt32(args1.TextValue, 8), 2);
            if (args.Count == 2) {
                var args2 = args[1].ToNumber("Function OCT2BIN parameter 2 is error!");
                if (args2.IsError) { return args2; }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function OCT2BIN parameter 2 is error!");
            }
            return Operand.Create(num);
        }
        public virtual Operand VisitOCT2DEC_fun(mathParser.OCT2DEC_funContext context)
        {
            var args1 = context.expr().Accept(this).ToText("Function OCT2DEC parameter is error!");
            if (args1.IsError) { return args1; }

            if (Regex.IsMatch(args1.TextValue, "^[0-8]+$") == false) { return Operand.Error("Function OCT2DEC parameter is error!"); }
            var num = Convert.ToInt32(args1.TextValue, 8);
            return Operand.Create(num);
        }
        public virtual Operand VisitOCT2HEX_fun(mathParser.OCT2HEX_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0].ToText("Function OCT2HEX parameter 1 is error!");
            if (args1.IsError) { return args1; }
            if (Regex.IsMatch(args1.TextValue, "^[0-8]+$") == false) { return Operand.Error("Function OCT2HEX parameter 1 is error!"); }
            var num = Convert.ToString(Convert.ToInt32(args1.TextValue, 8), 16).ToUpper();
            if (args.Count == 2) {
                var args2 = args[1].ToNumber("Function OCT2HEX parameter 2 is error!");
                if (args2.IsError) { return args2; }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function OCT2HEX parameter 2 is error!");
            }
            return Operand.Create(num);
        }
        public virtual Operand VisitDEC2BIN_fun(mathParser.DEC2BIN_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0].ToNumber("Function DEC2BIN parameter 1 is error!");
            if (args1.IsError) { return args1; }
            var num = System.Convert.ToString(args1.IntValue, 2);
            if (args.Count == 2) {
                var args2 = args[1].ToNumber("Function DEC2BIN parameter 2 is error!");
                if (args2.IsError) { return args2; }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function DEC2BIN parameter 2 is error!");
            }
            return Operand.Create(num);
        }
        public virtual Operand VisitDEC2OCT_fun(mathParser.DEC2OCT_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0].ToNumber("Function DEC2OCT parameter 1 is error!");
            if (args1.IsError) { return args1; }
            var num = System.Convert.ToString(args1.IntValue, 8);
            if (args.Count == 2) {
                var args2 = args[1].ToNumber("Function DEC2OCT parameter 2 is error!");
                if (args2.IsError) { return args2; }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function DEC2OCT parameter 2 is error!");
            }
            return Operand.Create(num);
        }
        public virtual Operand VisitDEC2HEX_fun(mathParser.DEC2HEX_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0].ToNumber("Function DEC2HEX parameter 1 is error!");
            if (args1.IsError) { return args1; }
            var num = System.Convert.ToString(args1.IntValue, 16).ToUpper();
            if (args.Count == 2) {
                var args2 = args[1].ToNumber("Function DEC2HEX parameter 2 is error!");
                if (args2.IsError) { return args2; }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function DEC2HEX parameter 2 is error!");
            }
            return Operand.Create(num);
        }

        public virtual Operand VisitHEX2BIN_fun(mathParser.HEX2BIN_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0].ToText("Function HEX2BIN parameter 1 is error!");
            if (args1.IsError) { return args1; }
            if (Regex.IsMatch(args1.TextValue, "^[0-9a-fA-F]+$") == false) { return Operand.Error("Function HEX2BIN parameter 1 is error!"); }

            var num = Convert.ToString(Convert.ToInt32(args1.TextValue, 16), 2);
            if (args.Count == 2) {
                var args2 = args[1].ToNumber("Function HEX2BIN parameter 2 is error!");
                if (args2.IsError) { return args2; }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function HEX2BIN parameter 2 is error!");
            }
            return Operand.Create(num);
        }
        public virtual Operand VisitHEX2OCT_fun(mathParser.HEX2OCT_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0].ToText("Function HEX2OCT parameter 1 is error!");
            if (args1.IsError) { return args1; }
            if (Regex.IsMatch(args1.TextValue, "^[0-9a-fA-F]+$") == false) { return Operand.Error("Function HEX2OCT parameter 1 is error!"); }
            var num = Convert.ToString(Convert.ToInt32(args1.TextValue, 16), 8);
            if (args.Count == 2) {
                var args2 = args[1].ToNumber("Function HEX2OCT parameter 2 is error!");
                if (args2.IsError) { return args2; }
                if (num.Length > args2.IntValue) {
                    return Operand.Create(num.PadLeft(args2.IntValue, '0'));
                }
                return Operand.Error("Function HEX2OCT parameter 2 is error!");
            }
            return Operand.Create(num);
        }
        public virtual Operand VisitHEX2DEC_fun(mathParser.HEX2DEC_funContext context)
        {
            var args1 = context.expr().Accept(this).ToText("Function HEX2DEC parameter is error!");
            if (args1.IsError) { return args1; }

            if (Regex.IsMatch(args1.TextValue, "^[0-9a-fA-F]+$") == false) { return Operand.Error("Function HEX2DEC parameter is error!"); }
            var num = Convert.ToInt32(args1.TextValue, 16);
            return Operand.Create(num);
        }
        #endregion


        #region rounding

        public virtual Operand VisitROUND_fun(mathParser.ROUND_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ROUND parameter 1 is error!"); if (args1.IsError) { return args1; } }
            if (exprs.Length == 1) {
                return Operand.Create((double)Math.Round((decimal)args1.NumberValue, 0, MidpointRounding.AwayFromZero));
            }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function ROUND parameter 2 is error!"); if (args2.IsError) { return args2; } }
            return Operand.Create((double)Math.Round((decimal)args1.NumberValue, args2.IntValue, MidpointRounding.AwayFromZero));
        }
        public virtual Operand VisitROUNDDOWN_fun(mathParser.ROUNDDOWN_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ROUNDDOWN parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function ROUNDDOWN parameter 2 is error!"); if (args2.IsError) { return args2; } }

            if (args1.NumberValue == 0.0m) {
                return args1;
            }
            var a = (decimal)Math.Pow(10, args2.IntValue);
            var b = args1.NumberValue;

            b = ((int)(b * a)) / a;
            return Operand.Create(b);
        }
        public virtual Operand VisitROUNDUP_fun(mathParser.ROUNDUP_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ROUNDUP parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function ROUNDUP parameter 2 is error!"); if (args2.IsError) { return args2; } }

            if (args1.NumberValue == 0.0m) { return args1; }
            var a = Math.Pow(10, args2.IntValue);
            var b = args1.NumberValue;

            var t = (Math.Ceiling(Math.Abs((double)b) * a)) / a;
            if (b > 0) return Operand.Create(t);
            return Operand.Create(-t);
        }
        public virtual Operand VisitCEILING_fun(mathParser.CEILING_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function CEILING parameter 1 is error!"); if (args1.IsError) { return args1; } }

            if (exprs.Length == 1)
                return Operand.Create(Math.Ceiling(args1.NumberValue));

            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function CEILING parameter 2 is error!"); if (args2.IsError) { return args2; } }
            var b = args2.NumberValue;
            if (b == 0) { return Operand.Create(0); }
            if (b < 0) { return Operand.Error("Function CEILING parameter 2 is error!"); }

            var a = args1.NumberValue;
            var d = Math.Ceiling(a / b) * b;
            return Operand.Create(d);
        }
        public virtual Operand VisitFLOOR_fun(mathParser.FLOOR_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function FLOOR parameter 1 is error!"); if (args1.IsError) { return args1; } }

            if (exprs.Length == 1) return Operand.Create(Math.Floor(args1.NumberValue));

            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function FLOOR parameter 2 is error!"); if (args2.IsError) { return args2; } }
            var b = args2.NumberValue;
            if (b >= 1) { return Operand.Create(args1.IntValue); }
            if (b <= 0) { return Operand.Error("Function FLOOR parameter 2 is error!"); }

            var a = args1.NumberValue;
            var d = Math.Floor(a / b) * b;
            return Operand.Create(d);
        }
        public virtual Operand VisitEVEN_fun(mathParser.EVEN_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function EVEN parameter is error!"); if (args1.IsError) { return args1; } }

            var z = args1.NumberValue;
            if (z % 2 == 0) { return args1; }
            z = Math.Ceiling(z);
            if (z % 2 == 0) { return Operand.Create(z); }
            z++;
            return Operand.Create(z);
        }
        public virtual Operand VisitODD_fun(mathParser.ODD_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function ODD parameter is error!"); if (args1.IsError) { return args1; } }
            var z = args1.NumberValue;
            if (z % 2 == 1) { return args1; }
            z = Math.Ceiling(z);
            if (z % 2 == 1) { return Operand.Create(z); }
            z++;
            return Operand.Create(z);
        }
        public virtual Operand VisitMROUND_fun(mathParser.MROUND_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function MROUND parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function MROUND parameter 2 is error!"); if (args2.IsError) { return args2; } }

            var a = args2.NumberValue;
            if (a <= 0) { return Operand.Error("Function MROUND parameter 2 is error!"); }

            var b = args1.NumberValue;
            var r = Math.Round(b / a, 0, MidpointRounding.AwayFromZero) * a;
            return Operand.Create(r);
        }
        #endregion

        #region RAND
        public virtual Operand VisitRAND_fun(mathParser.RAND_funContext context)
        {
#if NETSTANDARD2_1
            var tick = DateTime.Now.Ticks;
            Random rand = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
#else
            Random rand = Random.Shared;
#endif
            return Operand.Create(rand.NextDouble());
        }
        public virtual Operand VisitRANDBETWEEN_fun(mathParser.RANDBETWEEN_funContext context)
        {
            var args = new List<Operand>(); var index = 1;
            foreach (var item in context.expr()) { var aa = item.Accept(this).ToNumber($"Function RANDBETWEEN parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0];
            var args2 = args[1];

#if NETSTANDARD2_1
            var tick = DateTime.Now.Ticks;
            Random rand = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
#else
            Random rand = Random.Shared;
#endif
            return Operand.Create((decimal)rand.NextDouble() * (args2.NumberValue - args1.NumberValue) + args1.NumberValue);
        }
        #endregion

        #region  power logarithm factorial
        public virtual Operand VisitFACT_fun(mathParser.FACT_funContext context)
        {
            var args1 = context.expr().Accept(this).ToNumber("Function FACT parameter is error!");
            if (args1.IsError) { return args1; }

            var z = args1.IntValue;
            if (z < 0) {
                return Operand.Error("Function FACT parameter is error!");
            }
            double d = 1;
            for (int i = 1; i <= z; i++) {
                d *= i;
            }
            return Operand.Create(d);
        }
        public virtual Operand VisitFACTDOUBLE_fun(mathParser.FACTDOUBLE_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function FACTDOUBLE parameter is error!"); if (args1.IsError) { return args1; } }

            var z = args1.IntValue;
            if (z < 0) { return Operand.Error("Function FACTDOUBLE parameter is error!"); }

            double d = 1;
            for (int i = z; i > 0; i -= 2) {
                d *= i;
            }
            return Operand.Create(d);
        }
        public virtual Operand VisitPOWER_fun(mathParser.POWER_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function POWER parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function POWER parameter 2 is error!"); if (args2.IsError) { return args2; } }

            return Operand.Create(Math.Pow((double)args1.NumberValue, (double)args2.NumberValue));
        }
        public virtual Operand VisitEXP_fun(mathParser.EXP_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function EXP parameter is error!"); if (args1.IsError) { return args1; } }

            return Operand.Create(Math.Exp((double)args1.NumberValue));
        }
        public virtual Operand VisitLN_fun(mathParser.LN_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function LN parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Log((double)args1.NumberValue));
        }
        public virtual Operand VisitLOG_fun(mathParser.LOG_funContext context)
        {
            var exprs = context.expr();

            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function LOG parameter 1 is error!"); if (args1.IsError) { return args1; } }
            if (exprs.Length > 1) {
                var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function POWER parameter 2 is error!"); if (args2.IsError) { return args2; } }
                return Operand.Create(Math.Log((double)args1.NumberValue, (double)args2.NumberValue));
            }
            return Operand.Create(Math.Log((double)args1.NumberValue, 10));
        }
        public virtual Operand VisitLOG10_fun(mathParser.LOG10_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function LOG10 parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Log((double)args1.NumberValue, 10));
        }
        public virtual Operand VisitMULTINOMIAL_fun(mathParser.MULTINOMIAL_funContext context)
        {
            var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
            for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function MULTINOMIAL parameter is error!"); }

            int sum = 0;
            int n = 1;
            for (int i = 0; i < list.Count; i++) {
                var a = (int)list[i];
                n *= F_base_Factorial(a);
                sum += a;
            }

            var r = F_base_Factorial(sum) / n;
            return Operand.Create(r);
        }
        public virtual Operand VisitPRODUCT_fun(mathParser.PRODUCT_funContext context)
        {
            var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
            for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function PRODUCT parameter is error!"); }

            decimal d = 1;
            for (int i = 0; i < list.Count; i++) {
                var a = list[i];
                d *= a;
            }

            return Operand.Create(d);
        }
        public virtual Operand VisitSQRTPI_fun(mathParser.SQRTPI_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function SQRTPI parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(Math.Sqrt((double)args1.NumberValue * Math.PI));
        }
        public virtual Operand VisitSUMSQ_fun(mathParser.SUMSQ_funContext context)
        {
            var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
            for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function SUMSQ parameter is error!"); }

            decimal d = 0;
            for (int i = 0; i < list.Count; i++) {
                var a = list[i];
                d += a * a;
            }

            return Operand.Create(d);
        }
        private static int F_base_Factorial(int a)
        {
            if (a == 0) { return 1; }
            int r = 1;
            for (int i = a; i > 0; i--) {
                r *= i;
            }
            return r;
        }
        #endregion

        #endregion

        #region string

        public virtual Operand VisitASC_fun(mathParser.ASC_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function ASC parameter is error!"); if (args1.IsError) { return args1; } }

            return Operand.Create(F_base_ToDBC(args1.TextValue));
        }
        public virtual Operand VisitJIS_fun(mathParser.JIS_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function JIS parameter is error!"); if (args1.IsError) { return args1; } }

            return Operand.Create(F_base_ToSBC(args1.TextValue));
        }
        public virtual Operand VisitCHAR_fun(mathParser.CHAR_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function CHAR parameter is error!"); if (args1.IsError) { return args1; } }

            char c = (char)(int)args1.NumberValue;
            return Operand.Create(c.ToString());
        }
        public virtual Operand VisitCLEAN_fun(mathParser.CLEAN_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function CLEAN parameter is error!"); if (args1.IsError) { return args1; } }

            var t = args1.TextValue;
            StringBuilder sb = new StringBuilder(t.Length);
            for (int i = 0; i < t.Length; i++) {
                var c = t[i];
                if (c != '\f' && c != '\n' && c != '\r' && c != '\t' && c != '\v') {
                    sb.Append(c);
                }
            }
            return Operand.Create(sb.ToString());
        }
        public virtual Operand VisitCODE_fun(mathParser.CODE_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function CODE parameter is error!"); if (args1.IsError) { return args1; } }

            var t = args1.TextValue;
            if (t.Length == 0) { return Operand.Error("Function CODE parameter is error!"); }

            return Operand.Create((int)t[0]);
        }
        public virtual Operand VisitCONCATENATE_fun(mathParser.CONCATENATE_funContext context)
        {
            var exprs = context.expr();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < exprs.Length; i++) {
                var a = this.Visit(exprs[i]); if (a.Type != OperandType.TEXT) { a = a.ToText($"Function CONCATENATE parameter {i + 1} is error!"); if (a.IsError) { return a; } }
                sb.Append(a.TextValue);
            }
            return Operand.Create(sb.ToString());
        }
        public virtual Operand VisitEXACT_fun(mathParser.EXACT_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function EXACT parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function EXACT parameter 2 is error!"); if (args2.IsError) { return args2; } }

            return Operand.Create(args1.TextValue == args2.TextValue);
        }
        public virtual Operand VisitFIND_fun(mathParser.FIND_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function FIND parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function FIND parameter 2 is error!"); if (args2.IsError) { return args2; } }

            if (exprs.Length == 2) {
                var p = args2.TextValue.AsSpan().IndexOf(args1.TextValue) + excelIndex;
                return Operand.Create(p);
            }
            var count = exprs[2].Accept(this).ToNumber("Function FIND parameter 3 is error!"); if (count.IsError) { return count; }
            var p2 = args2.TextValue.AsSpan(count.IntValue).IndexOf(args1.TextValue) + count.IntValue + excelIndex;
            return Operand.Create(p2);
        }
        public virtual Operand VisitLEFT_fun(mathParser.LEFT_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function LEFT parameter 1 is error!"); if (args1.IsError) { return args1; } }

            if (exprs.Length == 1) {
                return Operand.Create(args1.TextValue[0].ToString());
            }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function LEFT parameter 2 is error!"); if (args2.IsError) { return args2; } }
            return Operand.Create(args1.TextValue.AsSpan(0, args2.IntValue).ToString());
        }
        public virtual Operand VisitLEN_fun(mathParser.LEN_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function LEN parameter is error!"); if (args1.IsError) { return args1; } }

            return Operand.Create(args1.TextValue.Length);
        }
        public virtual Operand VisitLOWER_fun(mathParser.LOWER_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function LOWER/TOLOWER parameter is error!"); if (args1.IsError) { return args1; } }

            return Operand.Create(args1.TextValue.ToLower());
        }
        public virtual Operand VisitMID_fun(mathParser.MID_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function MID parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function MID parameter 2 is error!"); if (args2.IsError) { return args2; } }
            var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function MID parameter 3 is error!"); if (args3.IsError) { return args3; } }

            return Operand.Create(args1.TextValue.AsSpan(args2.IntValue - excelIndex, args3.IntValue).ToString());
        }
        public virtual Operand VisitPROPER_fun(mathParser.PROPER_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function PROPER parameter is error!"); if (args1.IsError) { return args1; } }

            var text = args1.TextValue;
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
        public virtual Operand VisitREPLACE_fun(mathParser.REPLACE_funContext context)
        {
            var exprs = context.expr();

            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function REPLACE parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var oldtext = args1.TextValue;
            if (exprs.Length == 3) {
                var args22 = exprs[1].Accept(this); if (args22.Type != OperandType.TEXT) { args22 = args22.ToText("Function REPLACE parameter 2 is error!"); if (args22.IsError) { return args22; } }
                var args32 = exprs[2].Accept(this); if (args32.Type != OperandType.TEXT) { args32 = args32.ToText("Function REPLACE parameter 3 is error!"); if (args32.IsError) { return args32; } }

                var old = args22.TextValue;
                var newstr = args32.TextValue;
                return Operand.Create(oldtext.Replace(old, newstr));
            }

            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function REPLACE parameter 2 is error!"); if (args2.IsError) { return args2; } }
            var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function REPLACE parameter 3 is error!"); if (args3.IsError) { return args3; } }
            var args4 = exprs[3].Accept(this); if (args4.Type != OperandType.TEXT) { args4 = args4.ToText("Function REPLACE parameter 4 is error!"); if (args4.IsError) { return args4; } }

            var start = args2.IntValue - excelIndex;
            var length = args3.IntValue;
            var newtext = args4.TextValue;

            StringBuilder sb = new StringBuilder(oldtext.Length + newtext.Length);
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
        public virtual Operand VisitREPT_fun(mathParser.REPT_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function REPT parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function REPT parameter 2 is error!"); if (args2.IsError) { return args2; } }

            var newtext = args1.TextValue;
            var length = args2.IntValue;
            StringBuilder sb = new StringBuilder(newtext.Length * length);
            for (int i = 0; i < length; i++) {
                sb.Append(newtext);
            }
            return Operand.Create(sb.ToString());
        }
        public virtual Operand VisitRIGHT_fun(mathParser.RIGHT_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function RIGHT parameter 1 is error!"); if (args1.IsError) { return args1; } }

            if (exprs.Length == 1) {
                return Operand.Create(args1.TextValue[args1.TextValue.Length - 1].ToString());
            }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function RIGHT parameter 2 is error!"); if (args2.IsError) { return args2; } }
            return Operand.Create(args1.TextValue.AsSpan(args1.TextValue.Length - args2.IntValue, args2.IntValue).ToString());
        }
        public virtual Operand VisitRMB_fun(mathParser.RMB_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function RMB parameter is error!"); if (args1.IsError) { return args1; } }

            return Operand.Create(MathVisitor.F_base_ToChineseRMB(args1.NumberValue));
        }
        public virtual Operand VisitSEARCH_fun(mathParser.SEARCH_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function SEARCH parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function SEARCH parameter 2 is error!"); if (args2.IsError) { return args2; } }

            if (exprs.Length == 2) {
                var p = args2.TextValue.AsSpan().IndexOf(args1.TextValue, StringComparison.OrdinalIgnoreCase) + excelIndex;
                return Operand.Create(p);
            }
            var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function SEARCH parameter 3 is error!"); if (args3.IsError) { return args3; } }
            var p2 = args2.TextValue.AsSpan(args3.IntValue).IndexOf(args1.TextValue, StringComparison.OrdinalIgnoreCase) + args3.IntValue + excelIndex;
            return Operand.Create(p2);
        }
        public virtual Operand VisitSUBSTITUTE_fun(mathParser.SUBSTITUTE_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function SUBSTITUTE parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function SUBSTITUTE parameter 2 is error!"); if (args2.IsError) { return args2; } }
            var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.TEXT) { args3 = args3.ToText("Function SUBSTITUTE parameter 3 is error!"); if (args3.IsError) { return args3; } }
            if (exprs.Length == 3) {
                return Operand.Create(args1.TextValue.Replace(args2.TextValue, args3.TextValue));
            }
            var args4 = exprs[3].Accept(this); if (args4.Type != OperandType.NUMBER) { args4 = args4.ToNumber("Function SUBSTITUTE parameter 4 is error!"); if (args4.IsError) { return args4; } }

            string text = args1.TextValue;
            string oldtext = args2.TextValue;
            string newtext = args3.TextValue;
            int index = args4.IntValue;

            int index2 = 0;
            StringBuilder sb = new StringBuilder(text.Length + newtext.Length);
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
        public virtual Operand VisitT_fun(mathParser.T_funContext context)
        {
            var args1 = context.expr().Accept(this);
            if (args1.Type == OperandType.TEXT) {
                return args1;
            }
            return Operand.Create("");
        }
        public virtual Operand VisitTEXT_fun(mathParser.TEXT_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.IsError) { return args1; }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function TEXT parameter 2 is error!"); if (args2.IsError) { return args2; } }

            if (args1.Type == OperandType.TEXT) {
                return args1;
            } else if (args1.Type == OperandType.BOOLEAN) {
                return Operand.Create(args1.BooleanValue ? "TRUE" : "FALSE");
            } else if (args1.Type == OperandType.NUMBER) {
                return Operand.Create(args1.NumberValue.ToString(args2.TextValue, CultureInfo.InvariantCulture));
            } else if (args1.Type == OperandType.DATE) {
                return Operand.Create(args1.DateValue.ToString(args2.TextValue));
            }
            args1 = args1.ToText("Function TEXT parameter 1 is error!"); if (args1.IsError) { return args1; }
            return Operand.Create(args1.TextValue.ToString());
        }
        public virtual Operand VisitTRIM_fun(mathParser.TRIM_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function TRIM parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(args1.TextValue.AsSpan().Trim().ToString());
        }
        public virtual Operand VisitUPPER_fun(mathParser.UPPER_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function UPPER/TOUPPER parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(args1.TextValue.ToUpper());
        }
        public virtual Operand VisitVALUE_fun(mathParser.VALUE_funContext context)
        {
            var args1 = context.expr().Accept(this);
            if (args1.Type == OperandType.NUMBER) { return args1; }
            if (args1.Type == OperandType.BOOLEAN) { return args1.BooleanValue ? Operand.One : Operand.Zero; }
            if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function VALUE parameter is error!"); if (args1.IsError) { return args1; } }

            if (decimal.TryParse(args1.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                return Operand.Create(d);
            }
            return Operand.Error("Function VALUE parameter is error!");
        }

        private static String F_base_ToSBC(String input)
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
        private static String F_base_ToDBC(String input)
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
        private static string F_base_ToChineseRMB(decimal x)
        {
            string s = x.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A", CultureInfo.InvariantCulture);
            string d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}", RegexOptions.Compiled);
            return Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString(), RegexOptions.Compiled);
        }
        #endregion

        #region MyDate time

        public virtual Operand VisitDATEVALUE_fun(mathParser.DATEVALUE_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }
            if (args[0].Type == OperandType.DATE) { return args[0]; }
            int type = 0;
            if (args.Count == 2) {
                var args2 = args[1].ToNumber("Function DATEVALUE parameter 2 is error!");
                if (args2.IsError) { return args2; }
                type = args2.IntValue;
            }
            if (type == 0) {
                if (args[0].Type == OperandType.TEXT) {
                    if (DateTime.TryParse(args[0].TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime time)) {
                        return Operand.Create(time);
                    }
                }
                var args1 = args[0].ToNumber("Function DATEVALUE parameter 1 is error!");
                if (args1.LongValue <= 2958465L) { // 9999-12-31 日时间在excel的数字为 2958465
                    return args1.ToMyDate();
                }
                if (args1.LongValue <= 253402232399L) { // 9999-12-31 12:59:59 日时间 转 时间截 为 253402232399L， 
                    var time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(args1.LongValue);
                    if (useLocalTime) { return Operand.Create(time.ToLocalTime()); }
                    return Operand.Create(time);
                }
                // 注：时间截 253402232399 ms 转时间 为 1978-01-12 05:30:32
                var time2 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(args1.LongValue);
                if (useLocalTime) { return Operand.Create(time2.ToLocalTime()); }
                return Operand.Create(time2);
            } else if (type == 1) {
                var args1 = args[0].ToText("Function DATEVALUE parameter 1 is error!");
                if (args1.IsError) { return args1; }
                if (DateTime.TryParse(args1.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
                    return Operand.Create(dt);
                }
            } else if (type == 2) {
                return args[0].ToNumber("Function DATEVALUE parameter is error!").ToMyDate();
            } else if (type == 3) {
                var args1 = args[0].ToNumber("Function DATEVALUE parameter 1 is error!");
                var time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(args1.LongValue);
                if (useLocalTime) { return Operand.Create(time.ToLocalTime()); }
                return Operand.Create(time);
            } else if (type == 4) {
                var args1 = args[0].ToNumber("Function DATEVALUE parameter 1 is error!");
                var time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(args1.LongValue);
                if (useLocalTime) { return Operand.Create(time.ToLocalTime()); }
                return Operand.Create(time);
            }
            return Operand.Error("Function DATEVALUE parameter is error!");
        }
        public Operand VisitTIMESTAMP_fun(mathParser.TIMESTAMP_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }
            int type = 0; // 毫秒
            if (args.Count == 2) {
                var args2 = args[1].ToNumber("Function TIMESTAMP parameter 2 is error!");
                if (args2.IsError) { return args2; }
                type = args2.IntValue;
            }
            DateTime args1;
            if (useLocalTime) {
                args1 = args[0].ToMyDate("Function TIMESTAMP parameter 1 is error!").DateValue.ToDateTime(DateTimeKind.Local).ToUniversalTime();
            } else {
                args1 = args[0].ToMyDate("Function TIMESTAMP parameter 1 is error!").DateValue.ToDateTime(DateTimeKind.Utc);
            }
            if (type == 0) {
                var ms = (args1 - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                return Operand.Create(ms);
            } else if (type == 1) {
                var s = (args1 - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
                return Operand.Create(s);
            }
            return Operand.Error("Function TIMESTAMP parameter is error!");
        }
        public virtual Operand VisitTIMEVALUE_fun(mathParser.TIMEVALUE_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function TIMEVALUE parameter is error!"); if (args1.IsError) { return args1; } }

            if (TimeSpan.TryParse(args1.TextValue, CultureInfo.InvariantCulture, out TimeSpan dt)) {
                return Operand.Create(dt);
            }
            return Operand.Error("Function TIMEVALUE parameter is error!");
        }
        public virtual Operand VisitDATE_fun(mathParser.DATE_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function DATE parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function DATE parameter 2 is error!"); if (args2.IsError) { return args2; } }
            var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function DATE parameter 3 is error!"); if (args3.IsError) { return args3; } }

            MyDate d;
            if (exprs.Length == 3) {
                d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, 0, 0, 0);
            } else if (exprs.Length == 4) {
                var args4 = exprs[3].Accept(this); if (args4.Type != OperandType.NUMBER) { args4 = args4.ToNumber("Function DATE parameter 4 is error!"); if (args4.IsError) { return args4; } }

                d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, args4.IntValue, 0, 0);
            } else if (exprs.Length == 5) {
                var args4 = exprs[3].Accept(this); if (args4.Type != OperandType.NUMBER) { args4 = args4.ToNumber("Function DATE parameter 4 is error!"); if (args4.IsError) { return args4; } }
                var args5 = exprs[4].Accept(this); if (args5.Type != OperandType.NUMBER) { args5 = args5.ToNumber("Function DATE parameter 5 is error!"); if (args5.IsError) { return args5; } }
                d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, args4.IntValue, args5.IntValue, 0);
            } else {
                var args4 = exprs[3].Accept(this); if (args4.Type != OperandType.NUMBER) { args4 = args4.ToNumber("Function DATE parameter 4 is error!"); if (args4.IsError) { return args4; } }
                var args5 = exprs[4].Accept(this); if (args5.Type != OperandType.NUMBER) { args5 = args5.ToNumber("Function DATE parameter 5 is error!"); if (args5.IsError) { return args5; } }
                var args6 = exprs[5].Accept(this); if (args6.Type != OperandType.NUMBER) { args6 = args6.ToNumber("Function DATE parameter 6 is error!"); if (args6.IsError) { return args6; } }
                d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, args4.IntValue, args5.IntValue, args6.IntValue);
            }
            return Operand.Create(d);
        }
        public virtual Operand VisitTIME_fun(mathParser.TIME_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function TIME parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function TIME parameter 2 is error!"); if (args2.IsError) { return args2; } }

            MyDate d;
            if (exprs.Length == 3) {
                var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function TIME parameter 3 is error!"); if (args3.IsError) { return args3; } }
                d = new MyDate(0, 0, 0, args1.IntValue, args2.IntValue, args3.IntValue);
            } else {
                d = new MyDate(0, 0, 0, args1.IntValue, args2.IntValue, 0);
            }
            return Operand.Create(d);
        }
        public virtual Operand VisitNOW_fun(mathParser.NOW_funContext context)
        {
            return Operand.Create(new MyDate(DateTime.Now));
        }
        public virtual Operand VisitTODAY_fun(mathParser.TODAY_funContext context)
        {
            return Operand.Create(new MyDate(DateTime.Today));
        }
        public virtual Operand VisitYEAR_fun(mathParser.YEAR_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function YEAR parameter is error!"); if (args1.IsError) { return args1; } }
            if (args1.DateValue.Year == null) {
                return Operand.Error("Function YEAR is error!");
            }
            return Operand.Create(args1.DateValue.Year.Value);
        }
        public virtual Operand VisitMONTH_fun(mathParser.MONTH_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function MONTH parameter is error!"); if (args1.IsError) { return args1; } }
            if (args1.DateValue.Month == null) {
                return Operand.Error("Function MONTH is error!");
            }
            return Operand.Create((int)args1.DateValue.Month.Value);
        }
        public virtual Operand VisitDAY_fun(mathParser.DAY_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function DAY parameter is error!"); if (args1.IsError) { return args1; } }
            if (args1.DateValue.Day == null) {
                return Operand.Error("Function DAY is error!");
            }
            return Operand.Create(args1.DateValue.Day.Value);
        }
        public virtual Operand VisitHOUR_fun(mathParser.HOUR_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function HOUR parameter is error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(args1.DateValue.Hour);
        }
        public virtual Operand VisitMINUTE_fun(mathParser.MINUTE_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function MINUTE parameter is error!"); if (args1.IsError) { return args1; } }

            return Operand.Create(args1.DateValue.Minute);
        }
        public virtual Operand VisitSECOND_fun(mathParser.SECOND_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function SECOND parameter is error!"); if (args1.IsError) { return args1; } }

            return Operand.Create(args1.DateValue.Second);
        }
        public virtual Operand VisitWEEKDAY_fun(mathParser.WEEKDAY_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function WEEKDAY parameter 1 is error!"); if (args1.IsError) { return args1; } }

            var type = 1;
            if (exprs.Length == 2) {
                var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function WEEKDAY parameter 2 is error!"); if (args2.IsError) { return args2; } }
                type = args2.IntValue;
            }

            var t = ((DateTime)args1.DateValue).DayOfWeek;
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
        public virtual Operand VisitDATEDIF_fun(mathParser.DATEDIF_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function DATEDIF parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.DATE) { args2 = args2.ToMyDate("Function DATEDIF parameter 2 is error!"); if (args2.IsError) { return args2; } }
            var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.TEXT) { args3 = args3.ToText("Function DATEDIF parameter 3 is error!"); if (args3.IsError) { return args3; } }

            var startMyDate = (DateTime)args1.DateValue;
            var endMyDate = (DateTime)args2.DateValue;
            var t = args3.TextValue.ToLower();

            if (CharUtil.Equals(t, 'Y')) {
                #region y
                bool b = false;
                if (startMyDate.Month < endMyDate.Month) {
                    b = true;
                } else if (startMyDate.Month == endMyDate.Month) {
                    if (startMyDate.Day <= endMyDate.Day) b = true;
                }
                if (b) {
                    return Operand.Create((endMyDate.Year - startMyDate.Year));
                } else {
                    return Operand.Create((endMyDate.Year - startMyDate.Year - 1));
                }
                #endregion
            } else if (CharUtil.Equals(t, 'M')) {
                #region m
                bool b = false;
                if (startMyDate.Day <= endMyDate.Day) b = true;
                if (b) {
                    return Operand.Create((endMyDate.Year * 12 + endMyDate.Month - startMyDate.Year * 12 - startMyDate.Month));
                } else {
                    return Operand.Create((endMyDate.Year * 12 + endMyDate.Month - startMyDate.Year * 12 - startMyDate.Month - 1));
                }
                #endregion
            } else if (CharUtil.Equals(t, 'D')) {
                return Operand.Create((endMyDate - startMyDate).Days);
            } else if (CharUtil.Equals(t, "YD")) {
                #region yd
                var day = endMyDate.DayOfYear - startMyDate.DayOfYear;
                if (endMyDate.Year > startMyDate.Year && day < 0) {
                    var days = new DateTime(startMyDate.Year, 12, 31).DayOfYear;
                    day = days + day;
                }
                return Operand.Create((day));
                #endregion
            } else if (CharUtil.Equals(t, "MD")) {
                #region md
                var mo = endMyDate.Day - startMyDate.Day;
                if (mo < 0) {
                    int days;
                    if (startMyDate.Month == 12) {
                        days = new DateTime(startMyDate.Year + 1, 1, 1).AddDays(-1).Day;
                    } else {
                        days = new DateTime(startMyDate.Year, startMyDate.Month + 1, 1).AddDays(-1).Day;

                    }
                    mo += days;
                }
                return Operand.Create((mo));
                #endregion
            } else if (CharUtil.Equals(t, "YM")) {
                #region ym
                var mo = endMyDate.Month - startMyDate.Month;
                if (endMyDate.Day < startMyDate.Day) mo--;
                if (mo < 0) mo += 12;
                return Operand.Create((mo));
                #endregion
            }
            return Operand.Error("Function DATEDIF parameter 3 is error!");
        }
        public virtual Operand VisitDAYS360_fun(mathParser.DAYS360_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function DAYS360 parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.DATE) { args2 = args2.ToMyDate("Function DAYS360 parameter 2 is error!"); if (args2.IsError) { return args2; } }

            var startMyDate = (DateTime)args1.DateValue;
            var endMyDate = (DateTime)args2.DateValue;

            var method = false;
            if (exprs.Length == 3) {
                var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.DATE) { args3 = args3.ToMyDate("Function DAYS360 parameter 3 is error!"); if (args3.IsError) { return args3; } }
                if (args3.IsError) { return args3; }
                method = args3.BooleanValue;
            }
            var days = endMyDate.Year * 360 + (endMyDate.Month - 1) * 30
                        - startMyDate.Year * 360 - (startMyDate.Month - 1) * 30;
            if (method) {
                if (endMyDate.Day == 31) days += 30;
                if (startMyDate.Day == 31) days -= 30;
            } else {
                if (startMyDate.Month == 12) {
                    if (startMyDate.Day == new DateTime(startMyDate.Year + 1, 1, 1).AddDays(-1).Day) {
                        days -= 30;
                    } else {
                        days -= startMyDate.Day;
                    }
                } else {
                    if (startMyDate.Day == new DateTime(startMyDate.Year, startMyDate.Month + 1, 1).AddDays(-1).Day) {
                        days -= 30;
                    } else {
                        days -= startMyDate.Day;
                    }
                }
                if (endMyDate.Month == 12) {
                    if (endMyDate.Day == new DateTime(endMyDate.Year + 1, 1, 1).AddDays(-1).Day) {
                        if (startMyDate.Day < 30) {
                            days += 31;
                        } else {
                            days += 30;
                        }
                    } else {
                        days += endMyDate.Day;
                    }
                } else {
                    if (endMyDate.Day == new DateTime(endMyDate.Year, endMyDate.Month + 1, 1).AddDays(-1).Day) {
                        if (startMyDate.Day < 30) {
                            days += 31;
                        } else {
                            days += 30;
                        }
                    } else {
                        days += endMyDate.Day;
                    }
                }
            }
            return Operand.Create(days);
        }
        public virtual Operand VisitEDATE_fun(mathParser.EDATE_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function EDATE parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function EDATE parameter 2 is error!"); if (args2.IsError) { return args2; } }

            return Operand.Create((MyDate)(((DateTime)args1.DateValue).AddMonths(args2.IntValue)));
        }
        public virtual Operand VisitEOMONTH_fun(mathParser.EOMONTH_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0].ToMyDate("Function EOMONTH parameter 1 is error!");
            if (args1.IsError) { return args1; }
            var args2 = args[1].ToNumber("Function EOMONTH parameter 2 is error!");
            if (args2.IsError) { return args2; }

            var dt = ((DateTime)args1.DateValue).AddMonths(args2.IntValue + 1);
            dt = new DateTime(dt.Year, dt.Month, 1).AddDays(-1);
            return Operand.Create(dt);
        }
        public virtual Operand VisitNETWORKDAYS_fun(mathParser.NETWORKDAYS_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = item.Accept(this).ToMyDate($"Function NETWORKDAYS parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }

            var startMyDate = (DateTime)args[0].DateValue;
            var endMyDate = (DateTime)args[1].DateValue;

            List<DateTime> list = new List<DateTime>();
            for (int i = 2; i < args.Count; i++) {
                list.Add(args[i].DateValue);
            }

            var days = 0;
            while (startMyDate <= endMyDate) {
                if (startMyDate.DayOfWeek != DayOfWeek.Sunday && startMyDate.DayOfWeek != DayOfWeek.Saturday) {
                    if (list.Contains(startMyDate) == false) {
                        days++;
                    }
                }
                startMyDate = startMyDate.AddDays(1);
            }
            return Operand.Create(days);
        }
        public virtual Operand VisitWORKDAY_fun(mathParser.WORKDAY_funContext context)
        {
            var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
            for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0]; if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function WORKDAY parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = args[1]; if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function WORKDAY parameter 2 is error!"); if (args2.IsError) { return args2; } }

            var startMyDate = (DateTime)args1.DateValue;
            var days = args2.IntValue;
            List<DateTime> list = new List<DateTime>();
            for (int i = 2; i < args.Count; i++) {
                args[i] = args[i].ToMyDate($"Function WORKDAY parameter {i + 1} is error!");
                if (args[i].IsError) { return args[i]; }
                list.Add(args[i].DateValue);
            }
            while (days > 0) {
                startMyDate = startMyDate.AddDays(1);
                if (startMyDate.DayOfWeek == DayOfWeek.Saturday) continue;
                if (startMyDate.DayOfWeek == DayOfWeek.Sunday) continue;
                if (list.Contains(startMyDate)) continue;
                days--;
            }
            return Operand.Create(startMyDate);
        }
        public virtual Operand VisitWEEKNUM_fun(mathParser.WEEKNUM_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function WEEKNUM parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var startMyDate = (DateTime)args1.DateValue;

            var days = startMyDate.DayOfYear + (int)(new DateTime(startMyDate.Year, 1, 1).DayOfWeek);
            if (exprs.Length == 2) {
                var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function WEEKNUM parameter 2 is error!"); if (args2.IsError) { return args2; } }
                if (args2.IntValue == 2) {
                    days--;
                }
            }

            var week = Math.Ceiling(days / 7.0);
            return Operand.Create(week);
        }

        public virtual Operand VisitADDMONTHS_fun(mathParser.ADDMONTHS_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0].ToMyDate("Function AddMonths parameter 1 is error!");
            if (args1.IsError) { return args1; };
            var args2 = args[1].ToNumber("Function AddMonths parameter 2 is error!");
            if (args2.IsError) { return args2; };

            var date = args1.DateValue.AddMonths(args2.IntValue);
            return Operand.Create(date);
        }

        public virtual Operand VisitADDYEARS_fun(mathParser.ADDYEARS_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0].ToMyDate("Function AddYears parameter 1 is error!");
            if (args1.IsError) { return args1; };
            var args2 = args[1].ToNumber("Function AddYears parameter 2 is error!");
            if (args2.IsError) { return args2; };

            var date = args1.DateValue.AddYears(args2.IntValue);
            return Operand.Create(date);
        }

        public virtual Operand VisitADDSECONDS_fun(mathParser.ADDSECONDS_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0].ToMyDate("Function AddSeconds parameter 1 is error!");
            if (args1.IsError) { return args1; };
            var args2 = args[1].ToNumber("Function AddSeconds parameter 2 is error!");
            if (args2.IsError) { return args2; };

            var date = args1.DateValue.AddSeconds(args2.IntValue);
            return Operand.Create(date);
        }

        public virtual Operand VisitADDMINUTES_fun(mathParser.ADDMINUTES_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0].ToMyDate("Function AddMinutes parameter 1 is error!");
            if (args1.IsError) { return args1; };
            var args2 = args[1].ToNumber("Function AddMinutes parameter 2 is error!");
            if (args2.IsError) { return args2; };

            var date = args1.DateValue.AddMinutes(args2.IntValue);
            return Operand.Create(date);
        }

        public virtual Operand VisitADDDAYS_fun(mathParser.ADDDAYS_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0].ToMyDate("Function AddDays parameter 1 is error!");
            if (args1.IsError) { return args1; };
            var args2 = args[1].ToNumber("Function AddDays parameter 2 is error!");
            if (args2.IsError) { return args2; };

            var date = args1.DateValue.AddDays(args2.IntValue);
            return Operand.Create(date);
        }

        public virtual Operand VisitADDHOURS_fun(mathParser.ADDHOURS_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0].ToMyDate("Function AddHours parameter 1 is error!");
            if (args1.IsError) { return args1; };
            var args2 = args[1].ToNumber("Function AddHours parameter 2 is error!");
            if (args2.IsError) { return args2; };

            var date = args1.DateValue.AddHours(args2.IntValue);
            return Operand.Create(date);
        }

        #endregion

        #region sum

        public virtual Operand VisitMAX_fun(mathParser.MAX_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var aa = item.Accept(this).ToNumber($"Function MAX parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function MAX parameter error!"); }

            return Operand.Create(list.Max());
        }
        public virtual Operand VisitMEDIAN_fun(mathParser.MEDIAN_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var aa = item.Accept(this).ToNumber($"Function MEDIAN parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function MEDIAN parameter error!"); }

            list = list.OrderBy(q => q).ToList();
            return Operand.Create(list[list.Count / 2]);
        }
        public virtual Operand VisitMIN_fun(mathParser.MIN_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var aa = item.Accept(this).ToNumber($"Function MIN parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function MIN parameter error!"); }

            return Operand.Create(list.Min());
        }
        public virtual Operand VisitQUARTILE_fun(mathParser.QUARTILE_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.ARRARY) { args1 = args1.ToArray("Function QUARTILE parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function QUARTILE parameter 2 is error!"); if (args2.IsError) { return args2; } }


            List<decimal> list = new List<decimal>();
            var o = F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function QUARTILE parameter 1 error!"); }

            var quant = args2.IntValue;
            if (quant < 0 || quant > 4) {
                return Operand.Error("Function QUARTILE parameter 2 is error!");
            }
            return Operand.Create(ExcelFunctions.Quartile(list.Select(q => (double)q).ToArray(), quant));
        }
        public virtual Operand VisitMODE_fun(mathParser.MODE_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var aa = item.Accept(this).ToNumber($"Function MODE parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function MODE parameter error!"); }

            Dictionary<decimal, int> dict = new Dictionary<decimal, int>();
            foreach (var item in list) {
                if (dict.ContainsKey(item)) {
                    dict[item] += 1;
                } else {
                    dict[item] = 1;
                }
            }
            return Operand.Create(dict.OrderByDescending(q => q.Value).First().Key);
        }
        public virtual Operand VisitLARGE_fun(mathParser.LARGE_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0].ToArray("Function LARGE parameter 1 is error!");
            if (args1.IsError) { return args1; }
            var args2 = args[1].ToNumber("Function LARGE parameter 2 is error!");
            if (args2.IsError) { return args2; }


            List<decimal> list = new List<decimal>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function LARGE parameter error!"); }


            list = list.OrderByDescending(q => q).ToList();
            var quant = args2.IntValue;
            return Operand.Create(list[args2.IntValue - excelIndex]);
        }
        public virtual Operand VisitSMALL_fun(mathParser.SMALL_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0].ToArray("Function SMALL parameter 1 is error!");
            if (args1.IsError) { return args1; }
            var args2 = args[1].ToNumber("Function SMALL parameter 2 is error!");
            if (args2.IsError) { return args2; }

            List<decimal> list = new List<decimal>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function SMALL parameter error!"); }

            list = list.OrderBy(q => q).ToList();
            var quant = args2.IntValue;
            return Operand.Create(list[args2.IntValue - excelIndex]);
        }
        public virtual Operand VisitPERCENTILE_fun(mathParser.PERCENTILE_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0].ToArray("Function PERCENTILE parameter 1 is error!");
            if (args1.IsError) { return args1; }
            var args2 = args[1].ToNumber("Function PERCENTILE parameter 2 is error!");
            if (args2.IsError) { return args2; }

            List<decimal> list = new List<decimal>();
            var o = F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function PERCENTILE parameter error!"); }

            var k = args2.NumberValue;
            return Operand.Create(ExcelFunctions.Percentile(list.Select(q => (double)q).ToArray(), (double)k));
        }
        public virtual Operand VisitPERCENTRANK_fun(mathParser.PERCENTRANK_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0].ToArray("Function PERCENTRANK parameter 1 is error!");
            if (args1.IsError) { return args1; }
            var args2 = args[1].ToNumber("Function PERCENTRANK parameter 2 is error!");
            if (args2.IsError) { return args2; }

            List<decimal> list = new List<decimal>();
            var o = F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function PERCENTRANK parameter error!"); }

            var k = args2.NumberValue;
            var v = ExcelFunctions.PercentRank(list.Select(q => (double)q).ToArray(), (double)k);
            var d = 3;
            if (args.Count == 3) {
                var args3 = args[2].ToNumber("Function PERCENTRANK parameter 3 is error!");
                if (args3.IsError) { return args3; }
                d = args3.IntValue;
            }
            return Operand.Create(Math.Round(v, d, MidpointRounding.AwayFromZero));
        }
        public virtual Operand VisitAVERAGE_fun(mathParser.AVERAGE_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function AVERAGE parameter error!"); }

            return Operand.Create(list.Average());
        }


        public virtual Operand VisitAVERAGEIF_fun(mathParser.AVERAGEIF_funContext context)
        {
            var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
            for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = F_base_GetList(args[0], list);
            if (o == false) { return Operand.Error("Function AVERAGE parameter 1 error!"); }

            List<decimal> sumdbs;
            if (args.Count == 3) {
                sumdbs = new List<decimal>();
                var o2 = F_base_GetList(args[2], sumdbs);
                if (o2 == false) { return Operand.Error("Function AVERAGE parameter 3 error!"); }
            } else {
                sumdbs = list;
            }

            decimal sum;
            int count;
            if (args[1].Type == OperandType.NUMBER) {
                count = F_base_countif(list, args[1].NumberValue);
                sum = count * args[1].NumberValue;
            } else {
                if (decimal.TryParse(args[1].TextValue.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    count = F_base_countif(list, d);
                    sum = F_base_sumif(list, d, sumdbs);
                } else {
                    var sunif = args[1].TextValue.Trim();
                    var m2 = sumifMatch(sunif);
                    if (m2 != null) {
                        count = F_base_countif(list, m2.Item1, m2.Item2);
                        sum = F_base_sumif(list, m2.Item1, m2.Item2, sumdbs);
                    } else {
                        return Operand.Error("Function AVERAGE parameter 2 error!");
                    }
                }
            }
            return Operand.Create(sum / count);
        }
        public virtual Operand VisitGEOMEAN_fun(mathParser.GEOMEAN_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            if (args.Count == 1) return args[0];

            List<decimal> list = new List<decimal>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function GEOMEAN parameter error!"); }

            decimal sum = 1;
            foreach (var db in list) {
                sum *= db;
            }
            return Operand.Create(Math.Pow((double)sum, 1.0 / list.Count));
        }
        public virtual Operand VisitHARMEAN_fun(mathParser.HARMEAN_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            if (args.Count == 1) return args[0];

            List<decimal> list = new List<decimal>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function HARMEAN parameter error!"); }

            decimal sum = 0;
            foreach (var db in list) {
                if (db == 0) {
                    return Operand.Error("Function HARMEAN parameter error!");
                }
                sum += 1 / db;
            }
            return Operand.Create(list.Count / sum);
        }
        public virtual Operand VisitCOUNT_fun(mathParser.COUNT_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function COUNT parameter error!"); }

            return Operand.Create(list.Count);
        }
        public virtual Operand VisitCOUNTIF_fun(mathParser.COUNTIF_funContext context)
        {
            var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
            for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = F_base_GetList(args[0], list);
            if (o == false) { return Operand.Error("Function COUNTIF parameter error!"); }

            int count;
            if (args[1].Type == OperandType.NUMBER) {
                count = F_base_countif(list, args[1].NumberValue);
            } else {
                if (decimal.TryParse(args[1].TextValue.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    count = F_base_countif(list, d);
                } else {
                    var sunif = args[1].TextValue.Trim();
                    var m2 = sumifMatch(sunif);
                    if (m2 != null) {
                        count = F_base_countif(list, m2.Item1, m2.Item2);
                    } else {
                        return Operand.Error("Function COUNTIF parameter 2 error!");
                    }
                }
            }
            return Operand.Create(count);
        }
        public virtual Operand VisitSUM_fun(mathParser.SUM_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            if (args.Count == 1) return args[0];

            List<decimal> list = new List<decimal>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function SUM parameter error!"); }

            decimal sum = list.Sum();
            return Operand.Create(sum);
        }
        public virtual Operand VisitSUMIF_fun(mathParser.SUMIF_funContext context)
        {
            var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
            for (int i = 0; i < exprs.Length; i++) { var aa = this.Visit(exprs[i]); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = F_base_GetList(args[0], list);
            if (o == false) { return Operand.Error("Function SUMIF parameter 1 error!"); }

            List<decimal> sumdbs;
            if (args.Count == 3) {
                sumdbs = new List<decimal>();
                var o2 = F_base_GetList(args[2], sumdbs);
                if (o2 == false) { return Operand.Error("Function SUMIF parameter 3 error!"); }
            } else {
                sumdbs = list;
            }

            decimal sum;
            if (args[1].Type == OperandType.NUMBER) {
                sum = F_base_countif(list, args[1].NumberValue) * args[1].NumberValue;
            } else {
                if (decimal.TryParse(args[1].TextValue.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    sum = F_base_sumif(list, d, sumdbs);
                } else {
                    var sunif = args[1].TextValue.Trim();
                    var m2 = sumifMatch(sunif);
                    if (m2 != null) {
                        sum = F_base_sumif(list, m2.Item1, m2.Item2, sumdbs);
                    } else {
                        return Operand.Error("Function SUMIF parameter 2 error!");
                    }
                }
            }
            return Operand.Create(sum);
        }
        public virtual Operand VisitAVEDEV_fun(mathParser.AVEDEV_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function AVEDEV parameter error!"); }

            decimal avg = list.Average();
            decimal sum = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += Math.Abs(list[i] - avg);
            }
            return Operand.Create(sum / list.Count);
        }

        public virtual Operand VisitSTDEV_fun(mathParser.STDEV_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            if (args.Count == 1) {
                return Operand.Error("Function STDEV parameter only one error!");
            }
            List<decimal> list = new List<decimal>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function STDEV parameter error!"); }

            var avg = list.Average();
            decimal sum = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += (list[i] - avg) * (list[i] - avg);
            }
            return Operand.Create(Math.Sqrt((double)sum / (list.Count - 1)));
        }
        public virtual Operand VisitSTDEVP_fun(mathParser.STDEVP_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function STDEVP parameter error!"); }

            decimal sum = 0;
            decimal avg = list.Average();

            for (int i = 0; i < list.Count; i++) {
                sum += (list[i] - avg) * (list[i] - avg);
            }
            return Operand.Create(Math.Sqrt((double)sum / (list.Count)));
        }
        public virtual Operand VisitDEVSQ_fun(mathParser.DEVSQ_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function DEVSQ parameter error!"); }

            decimal avg = list.Average();
            decimal sum = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += (list[i] - avg) * (list[i] - avg);
            }
            return Operand.Create(sum);
        }
        public virtual Operand VisitVAR_fun(mathParser.VAR_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            if (args.Count == 1) {
                return Operand.Error("Function VAR parameter only one error!");
            }
            List<decimal> list = new List<decimal>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function VAR parameter error!"); }

            decimal sum = 0;
            decimal sum2 = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += list[i] * list[i];
                sum2 += list[i];
            }
            return Operand.Create((list.Count * sum - sum2 * sum2) / list.Count / (list.Count - 1));
        }
        public virtual Operand VisitVARP_fun(mathParser.VARP_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function VARP parameter error!"); }

            decimal sum = 0;
            decimal avg = list.Average();
            for (int i = 0; i < list.Count; i++) {
                sum += (avg - list[i]) * (avg - list[i]);
            }
            return Operand.Create(sum / list.Count);
        }
        public virtual Operand VisitNORMDIST_fun(mathParser.NORMDIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function NORMDIST parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function NORMDIST parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function NORMDIST parameter 3 error!"); if (args3.IsError) return args3; }
            var args4 = exprs[3].Accept(this); if (args4.Type != OperandType.BOOLEAN) { args4 = args4.ToBoolean("Function NORMDIST parameter 4 error!"); if (args4.IsError) return args4; }


            var num = args1.NumberValue;
            var avg = args2.NumberValue;
            var STDEV = args3.NumberValue;
            var b = args4.BooleanValue;
            return Operand.Create(ExcelFunctions.NormDist((double)num, (double)avg, (double)STDEV, b));
        }
        public virtual Operand VisitNORMINV_fun(mathParser.NORMINV_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function NORMINV parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function NORMINV parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function NORMINV parameter 3 error!"); if (args3.IsError) return args3; }

            var num = args1.NumberValue;
            var avg = args2.NumberValue;
            var STDEV = args3.NumberValue;
            return Operand.Create(ExcelFunctions.NormInv((double)num, (double)avg, (double)STDEV));
        }
        public virtual Operand VisitNORMSDIST_fun(mathParser.NORMSDIST_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function NORMSDIST parameter 1 error!"); if (args1.IsError) { return args1; } }

            var k = args1.NumberValue;
            return Operand.Create(ExcelFunctions.NormSDist((double)k));
        }
        public virtual Operand VisitNORMSINV_fun(mathParser.NORMSINV_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function NORMSINV parameter 1 error!"); if (args1.IsError) { return args1; } }

            var k = args1.NumberValue;
            return Operand.Create(ExcelFunctions.NormSInv((double)k));
        }
        public virtual Operand VisitBETADIST_fun(mathParser.BETADIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function BETADIST parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function BETADIST parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function BETADIST parameter 3 error!"); if (args3.IsError) return args3; }

            var x = args1.NumberValue;
            var alpha = args2.NumberValue;
            var beta = args3.NumberValue;

            if (alpha < 0.0m || beta < 0.0m) {
                return Operand.Error("Function BETADIST parameter error!");
            }

            return Operand.Create(ExcelFunctions.BetaDist((double)x, (double)alpha, (double)beta));
        }
        public virtual Operand VisitBETAINV_fun(mathParser.BETAINV_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var aa = item.Accept(this).ToNumber($"Function BETAINV parameter {index++} is error!"); if (aa.IsError) { return aa; } args.Add(aa); }

            var probability = args[0].NumberValue;
            var alpha = args[1].NumberValue;
            var beta = args[2].NumberValue;
            if (alpha < 0.0m || beta < 0.0m || probability < 0.0m || probability > 1.0m) {
                return Operand.Error("Function BETAINV parameter error!");
            }
            return Operand.Create(ExcelFunctions.BetaInv((double)probability, (double)alpha, (double)beta));
        }
        public virtual Operand VisitBINOMDIST_fun(mathParser.BINOMDIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function BINOMDIST parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function BINOMDIST parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function BINOMDIST parameter 3 error!"); if (args3.IsError) return args3; }
            var args4 = exprs[3].Accept(this); if (args4.Type != OperandType.BOOLEAN) { args4 = args4.ToBoolean("Function BINOMDIST parameter 4 error!"); if (args4.IsError) return args4; }

            if (!(args3.NumberValue >= 0.0m && args3.NumberValue <= 1.0m && args2.NumberValue >= 0)) {
                return Operand.Error("Function BINOMDIST parameter error!");
            }
            return Operand.Create(ExcelFunctions.BinomDist(args1.IntValue, args2.IntValue, (double)args3.NumberValue, args4.BooleanValue));
        }
        public virtual Operand VisitEXPONDIST_fun(mathParser.EXPONDIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function EXPONDIST parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function EXPONDIST parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.BOOLEAN) { args3 = args3.ToBoolean("Function EXPONDIST parameter 3 error!"); if (args3.IsError) return args3; }

            if (args1.NumberValue < 0.0m) {
                return Operand.Error("Function EXPONDIST parameter error!");
            }

            return Operand.Create(ExcelFunctions.ExponDist((double)args1.NumberValue, (double)args2.NumberValue, args3.BooleanValue));
        }
        public virtual Operand VisitFDIST_fun(mathParser.FDIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function FDIST parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function FDIST parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function FDIST parameter 3 error!"); if (args3.IsError) return args3; }

            var x = args1.NumberValue;
            var degreesFreedom = args2.IntValue;
            var degreesFreedom2 = args3.IntValue;
            if (degreesFreedom <= 0.0m || degreesFreedom2 <= 0.0m) {
                return Operand.Error("Function FDIST parameter error!");
            }
            return Operand.Create(ExcelFunctions.FDist((double)x, degreesFreedom, degreesFreedom2));
        }
        public virtual Operand VisitFINV_fun(mathParser.FINV_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function FINV parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function FINV parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function FINV parameter 3 error!"); if (args3.IsError) return args3; }

            var probability = args1.NumberValue;
            var degreesFreedom = args2.IntValue;
            var degreesFreedom2 = args3.IntValue;
            if (degreesFreedom <= 0.0m || degreesFreedom2 <= 0.0m) {
                return Operand.Error("Function FINV parameter error!");
            }
            return Operand.Create(ExcelFunctions.FInv((double)probability, degreesFreedom, degreesFreedom2));
        }
        public virtual Operand VisitFISHER_fun(mathParser.FISHER_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function FISHER parameter error!"); if (args1.IsError) { return args1; } }

            var x = args1.NumberValue;
            if (x >= 1 || x <= -1) {
                return Operand.Error("Function FISHER parameter error!");
            }
            var n = 0.5 * Math.Log((double)((1 + x) / (1 - x)));
            return Operand.Create(n);
        }
        public virtual Operand VisitFISHERINV_fun(mathParser.FISHERINV_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function FISHERINV parameter error!"); if (args1.IsError) { return args1; } }

            var x = (double)args1.NumberValue;
            var n = (Math.Exp((2 * x)) - 1) / (Math.Exp((2 * x)) + 1);
            return Operand.Create(n);
        }
        public virtual Operand VisitGAMMADIST_fun(mathParser.GAMMADIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function GAMMADIST parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function GAMMADIST parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function GAMMADIST parameter 3 error!"); if (args3.IsError) return args3; }
            var args4 = exprs[3].Accept(this); if (args4.Type != OperandType.BOOLEAN) { args4 = args4.ToBoolean("Function GAMMADIST parameter 4 error!"); if (args4.IsError) return args4; }

            var x = args1.NumberValue;
            var alpha = args2.NumberValue;
            var beta = args3.NumberValue;
            var cumulative = args4.BooleanValue;
            if (alpha < 0.0m || beta < 0.0m) {
                return Operand.Error("Function GAMMADIST parameter error!");
            }
            return Operand.Create(ExcelFunctions.GammaDist((double)x, (double)alpha, (double)beta, cumulative));
        }
        public virtual Operand VisitGAMMAINV_fun(mathParser.GAMMAINV_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function GAMMAINV parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function GAMMAINV parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function GAMMAINV parameter 3 error!"); if (args3.IsError) return args3; }

            var probability = args1.NumberValue;
            var alpha = args2.NumberValue;
            var beta = args3.NumberValue;
            if (alpha < 0.0m || beta < 0.0m || probability < 0 || probability > 1.0m) {
                return Operand.Error("Function GAMMAINV parameter error!");
            }
            return Operand.Create(ExcelFunctions.GammaInv((double)probability, (double)alpha, (double)beta));
        }
        public virtual Operand VisitGAMMALN_fun(mathParser.GAMMALN_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function GAMMALN parameter error!"); if (args1.IsError) { return args1; } }

            return Operand.Create(ExcelFunctions.GAMMALN((double)args1.NumberValue));
        }
        public virtual Operand VisitHYPGEOMDIST_fun(mathParser.HYPGEOMDIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function HYPGEOMDIST parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function HYPGEOMDIST parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function HYPGEOMDIST parameter 3 error!"); if (args3.IsError) return args3; }
            var args4 = exprs[3].Accept(this); if (args4.Type != OperandType.NUMBER) { args4 = args4.ToNumber("Function HYPGEOMDIST parameter 4 error!"); if (args4.IsError) return args4; }

            int k = args1.IntValue;
            int draws = args2.IntValue;
            int success = args3.IntValue;
            int population = args4.IntValue;
            if (!(population >= 0 && success >= 0 && draws >= 0 && success <= population && draws <= population)) {
                return Operand.Error("Function HYPGEOMDIST parameter error!");
            }
            return Operand.Create(ExcelFunctions.HypgeomDist(k, draws, success, population));
        }
        public virtual Operand VisitLOGINV_fun(mathParser.LOGINV_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function LOGINV parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function LOGINV parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function LOGINV parameter 3 error!"); if (args3.IsError) return args3; }

            if (args3.NumberValue < 0.0m) {
                return Operand.Error("Function LOGINV parameter error!");
            }
            return Operand.Create(ExcelFunctions.LogInv((double)args1.NumberValue, (double)args2.NumberValue, (double)args3.NumberValue));
        }
        public virtual Operand VisitLOGNORMDIST_fun(mathParser.LOGNORMDIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function LOGNORMDIST parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function LOGNORMDIST parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function LOGNORMDIST parameter 3 error!"); if (args3.IsError) return args3; }

            if (args3.NumberValue < 0.0m) {
                return Operand.Error("Function LOGNORMDIST parameter error!");
            }
            return Operand.Create(ExcelFunctions.LognormDist((double)args1.NumberValue, (double)args2.NumberValue, (double)args3.NumberValue));
        }
        public virtual Operand VisitNEGBINOMDIST_fun(mathParser.NEGBINOMDIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function NEGBINOMDIST parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function NEGBINOMDIST parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function NEGBINOMDIST parameter 3 error!"); if (args3.IsError) return args3; }

            int k = args1.IntValue;
            var r = args2.NumberValue;
            var p = args3.NumberValue;

            if (!(r >= 0.0m && p >= 0.0m && p <= 1.0m)) {
                return Operand.Error("Function NEGBINOMDIST parameter error!");
            }
            return Operand.Create(ExcelFunctions.NegbinomDist(k, (double)r, (double)p));
        }
        public virtual Operand VisitPOISSON_fun(mathParser.POISSON_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function POISSON parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function POISSON parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.BOOLEAN) { args3 = args3.ToBoolean("Function POISSON parameter 3 error!"); if (args3.IsError) return args3; }

            int k = args1.IntValue;
            var lambda = args2.NumberValue;
            bool state = args3.BooleanValue;
            if (!(lambda > 0.0m)) {
                return Operand.Error("Function POISSON parameter error!");
            }
            return Operand.Create(ExcelFunctions.POISSON(k, (double)lambda, state));
        }
        public virtual Operand VisitTDIST_fun(mathParser.TDIST_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function TDIST parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function TDIST parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function TDIST parameter 3 error!"); if (args3.IsError) return args3; }

            var x = args1.NumberValue;
            var degreesFreedom = args2.IntValue;
            var tails = args3.IntValue;
            if (degreesFreedom <= 0.0m || tails < 1 || tails > 2) {
                return Operand.Error("Function TDIST parameter error!");
            }
            return Operand.Create(ExcelFunctions.TDist((double)x, degreesFreedom, tails));
        }
        public virtual Operand VisitTINV_fun(mathParser.TINV_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function TDIST parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function TDIST parameter 2 error!"); if (args2.IsError) return args2; }

            var probability = args1.NumberValue;
            var degreesFreedom = args2.IntValue;
            if (degreesFreedom <= 0.0m) {
                return Operand.Error("Function TINV parameter error!");
            }
            return Operand.Create(ExcelFunctions.TInv((double)probability, degreesFreedom));
        }
        public virtual Operand VisitWEIBULL_fun(mathParser.WEIBULL_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function WEIBULL parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function WEIBULL parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function WEIBULL parameter 3 error!"); if (args3.IsError) return args3; }
            var args4 = exprs[3].Accept(this); if (args4.Type != OperandType.BOOLEAN) { args4 = args4.ToBoolean("Function WEIBULL parameter 4 error!"); if (args4.IsError) return args4; }

            var x = args1.NumberValue;
            var shape = args2.NumberValue;
            var scale = args3.NumberValue;
            var state = args4.BooleanValue;
            if (shape <= 0.0m || scale <= 0.0m) {
                return Operand.Error("Function WEIBULL parameter error!");
            }

            return Operand.Create(ExcelFunctions.WEIBULL((double)x, (double)shape, (double)scale, state));
        }

        private static Tuple<string, decimal> sumifMatch(string s)
        {
            var c = s[0];
            if (c == '>' || c == '＞') {
                if (s.Length > 1 && (s[1] == '=' || s[1] == '＝')) {
                    if (decimal.TryParse(s.AsSpan(2).Trim(), out decimal d)) {
                        return Tuple.Create(">=", d);
                    }
                } else if (decimal.TryParse(s.AsSpan(1).Trim(), out decimal d)) {
                    return Tuple.Create(">", d);
                }
            } else if (c == '<' || c == '＜') {
                if (s.Length > 1 && (s[1] == '=' || s[1] == '＝')) {
                    if (decimal.TryParse(s.AsSpan(2).Trim(), out decimal d)) {
                        return Tuple.Create("<=", d);
                    }
                } else if (s.Length > 1 && (s[1] == '>' || s[1] == '＞')) {
                    if (decimal.TryParse(s.AsSpan(2).Trim(), out decimal d)) {
                        return Tuple.Create("!=", d);
                    }
                } else if (decimal.TryParse(s.AsSpan(1).Trim(), out decimal d)) {
                    return Tuple.Create("<", d);
                }
            } else if (c == '=' || c == '＝') {
                var index = 1;
                if (s.Length > 1 && (s[1] == '=' || s[1] == '＝')) {
                    index = 2;
                    if (s.Length > 2 && (s[2] == '=' || s[2] == '＝')) {
                        index = 3;
                    }
                }
                if (decimal.TryParse(s.AsSpan(index).Trim(), out decimal d)) {
                    return Tuple.Create("=", d);
                }
            } else if (c == '!' || c == '！') {
                var index = 1;
                if (s.Length > 1 && (s[1] == '=' || s[1] == '＝')) {
                    index = 2;
                    if (s.Length > 2 && (s[2] == '=' || s[2] == '＝')) {
                        index = 3;
                    }
                }
                if (decimal.TryParse(s.AsSpan(index).Trim(), out decimal d)) {
                    return Tuple.Create("!=", d);
                }
            }
            return null;
        }
        private static int F_base_countif(List<decimal> dbs, decimal d)
        {
            int count = 0;
            for (int i = 0; i < dbs.Count; i++) {
                var item = dbs[i];
                if (item == d) {
                    count++;
                }
            }
            return count;
        }
        private static int F_base_countif(List<decimal> dbs, string s, decimal d)
        {
            int count = 0;
            for (int i = 0; i < dbs.Count; i++) {
                var item = dbs[i];
                if (F_base_compare(item, d, s)) {
                    count++;
                }
            }
            return count;
        }
        private static decimal F_base_sumif(List<decimal> dbs, decimal d, List<decimal> sumdbs)
        {
            decimal sum = 0;
            for (int i = 0; i < dbs.Count; i++) {
                var item = dbs[i];
                if (item == d) {
                    sum += sumdbs[i];
                }
                //if (Math.Round(item, 10, MidpointRounding.AwayFromZero) == d) {
                //	sum += item;
                //}
            }
            return sum;
        }
        private static decimal F_base_sumif(List<decimal> dbs, string s, decimal d, List<decimal> sumdbs)
        {
            decimal sum = 0;
            for (int i = 0; i < dbs.Count; i++) {
                if (F_base_compare(dbs[i], d, s)) {
                    sum += sumdbs[i];
                }
            }
            return sum;
        }
        private static bool F_base_compare(decimal a, decimal b, string ss)
        {
            if (CharUtil.Equals(ss, '<')) {
                return a < b;
                //return Math.Round(a - b, 12, MidpointRounding.AwayFromZero) < 0;
            } else if (CharUtil.Equals(ss, "<=")) {
                return a <= b;
                //return Math.Round(a - b, 12, MidpointRounding.AwayFromZero) <= 0;
            } else if (CharUtil.Equals(ss, '>')) {
                return a > b;
                //return Math.Round(a - b, 12, MidpointRounding.AwayFromZero) > 0;
            } else if (CharUtil.Equals(ss, ">=")) {
                return (a >= b);
                //return Math.Round(a - b, 12, MidpointRounding.AwayFromZero) >= 0;
            } else if (CharUtil.Equals(ss, "=", "==", "===")) {
                return a == b;
                //return Math.Round(a - b, 12, MidpointRounding.AwayFromZero) == 0;
            }
            return a != b;
            //return Math.Round(a - b, 12, MidpointRounding.AwayFromZero) != 0;
        }
        private static bool F_base_GetList(List<Operand> args, List<decimal> list)
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
        private static bool F_base_GetList(Operand args, List<decimal> list)
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
        private static bool F_base_GetList(Operand args, List<string> list)
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
        private static bool F_base_GetList(List<Operand> args, List<string> list)
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

        public virtual Operand VisitURLENCODE_fun(mathParser.URLENCODE_funContext context)
        {
            var args1 = context.expr().Accept(this).ToText("Function URLENCODE parameter 1 is error!");
            if (args1.IsError) { return args1; }

            return Operand.Create(HttpUtility.UrlEncode(args1.TextValue));
        }
        public virtual Operand VisitURLDECODE_fun(mathParser.URLDECODE_funContext context)
        {
            var args1 = context.expr().Accept(this).ToText("Function URLDECODE parameter 1 is error!");
            if (args1.IsError) { return args1; }

            return Operand.Create(HttpUtility.UrlDecode(args1.TextValue));
        }
        public virtual Operand VisitHTMLENCODE_fun(mathParser.HTMLENCODE_funContext context)
        {
            var args1 = context.expr().Accept(this).ToText("Function HTMLENCODE parameter 1 is error!");
            if (args1.IsError) { return args1; }

            return Operand.Create(HttpUtility.HtmlEncode(args1.TextValue));
        }
        public virtual Operand VisitHTMLDECODE_fun(mathParser.HTMLDECODE_funContext context)
        {
            var args1 = context.expr().Accept(this).ToText("Function HTMLDECODE parameter 1 is error!");
            if (args1.IsError) { return args1; }

            return Operand.Create(HttpUtility.HtmlDecode(args1.TextValue));
        }
        public virtual Operand VisitBASE64TOTEXT_fun(mathParser.BASE64TOTEXT_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = item.Accept(this).ToText($"Function BASE64TOTEXT parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }

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
        public virtual Operand VisitBASE64URLTOTEXT_fun(mathParser.BASE64URLTOTEXT_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = item.Accept(this).ToText($"Function BASE64URLTOTEXT parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }
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
        public virtual Operand VisitTEXTTOBASE64_fun(mathParser.TEXTTOBASE64_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = item.Accept(this).ToText($"Function TEXTTOBASE64 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }

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
        public virtual Operand VisitTEXTTOBASE64URL_fun(mathParser.TEXTTOBASE64URL_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = item.Accept(this).ToText($"Function TEXTTOBASE64URL parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }

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
        public virtual Operand VisitREGEX_fun(mathParser.REGEX_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0].ToText("Function REGEX parameter 1 is error!");
            if (args1.IsError) { return args1; }
            var args2 = args[1].ToText("Function REGEX parameter 2 is error!");
            if (args2.IsError) { return args2; }

            var b = Regex.Match(args1.TextValue, args2.TextValue);
            if (b.Success == false) {
                return Operand.Error("Function REGEX is error!");
            }
            return Operand.Create(b.Value);
        }
        public virtual Operand VisitREGEXREPALCE_fun(mathParser.REGEXREPALCE_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function REGEXREPALCE parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function REGEXREPALCE parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.TEXT) { args3 = args3.ToText("Function REGEXREPALCE parameter 3 error!"); if (args3.IsError) return args3; }

            var b = Regex.Replace(args1.TextValue, args2.TextValue, args3.TextValue);
            return Operand.Create(b);
        }
        public virtual Operand VisitISREGEX_fun(mathParser.ISREGEX_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function REGEXREPALCE parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function REGEXREPALCE parameter 2 error!"); if (args2.IsError) return args2; }

            var b = Regex.IsMatch(args1.TextValue, args2.TextValue);
            return Operand.Create(b);
        }
        public virtual Operand VisitGUID_fun(mathParser.GUID_funContext context)
        {
            return Operand.Create(System.Guid.NewGuid().ToString());
        }
        public virtual Operand VisitMD5_fun(mathParser.MD5_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = item.Accept(this).ToText($"Function MD5 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }

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
        public virtual Operand VisitSHA1_fun(mathParser.SHA1_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = item.Accept(this).ToText($"Function SHA1 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }

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
        public virtual Operand VisitSHA256_fun(mathParser.SHA256_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = item.Accept(this).ToText($"Function SHA256 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }

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
        public virtual Operand VisitSHA512_fun(mathParser.SHA512_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = item.Accept(this).ToText($"Function SHA512 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }

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

        public virtual Operand VisitCRC32_fun(mathParser.CRC32_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = item.Accept(this).ToText($"Function CRC32 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }
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
        public virtual Operand VisitHMACMD5_fun(mathParser.HMACMD5_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = item.Accept(this).ToText($"Function HMACMD5 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }
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
        public virtual Operand VisitHMACSHA1_fun(mathParser.HMACSHA1_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = item.Accept(this).ToText($"Function HMACSHA1 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }
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
        public virtual Operand VisitHMACSHA256_fun(mathParser.HMACSHA256_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = item.Accept(this).ToText($"Function HMACSHA256 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }
            try {
                Encoding encoding;
                if (args.Count == 2) {
                    encoding = Encoding.UTF8;
                } else {
                    encoding = Encoding.GetEncoding(args[2].TextValue);
                }
                var t = Hash.GetHmacSha256String(encoding.GetBytes(args[0].TextValue), args[1].TextValue);
                return Operand.Create(t);
            } catch (Exception) { }
            return Operand.Error("Function HMACSHA256 is error!");
        }
        public virtual Operand VisitHMACSHA512_fun(mathParser.HMACSHA512_funContext context)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in context.expr()) { var a = item.Accept(this).ToText($"Function HMACSHA512 parameter {index++} is error!"); if (a.IsError) { return a; } args.Add(a); }
            try {
                Encoding encoding;
                if (args.Count == 2) {
                    encoding = Encoding.UTF8;
                } else {
                    encoding = Encoding.GetEncoding(args[2].TextValue);
                }
                var t = Hash.GetHmacSha512String(encoding.GetBytes(args[0].TextValue), args[1].TextValue);
                return Operand.Create(t);
            } catch (Exception) { }
            return Operand.Error("Function HMACSHA512 is error!");
        }
        public virtual Operand VisitTRIMSTART_fun(mathParser.TRIMSTART_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function TRIMSTART parameter 1 error!"); if (args1.IsError) return args1; }

            var text = args1.TextValue;
            if (exprs.Length == 2) {
                var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function TRIMSTART parameter 2 error!"); if (args2.IsError) return args2; }
                return Operand.Create(text.AsSpan().TrimStart(args2.TextValue.AsSpan()).ToString());
            }
            return Operand.Create(text.AsSpan().TrimStart().ToString());
        }

        public virtual Operand VisitTRIMEND_fun(mathParser.TRIMEND_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function TRIMEND parameter 1 error!"); if (args1.IsError) return args1; }

            var text = args1.TextValue;
            if (exprs.Length == 2) {
                var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function TRIMEND parameter 2 error!"); if (args2.IsError) return args2; }
                return Operand.Create(text.AsSpan().TrimEnd(args2.TextValue.AsSpan()).ToString());
            }
            return Operand.Create(text.AsSpan().TrimEnd().ToString());
        }

        public virtual Operand VisitINDEXOF_fun(mathParser.INDEXOF_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function INDEXOF parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function INDEXOF parameter 2 is error!"); if (args2.IsError) { return args2; } }

            var text = args1.TextValue;
            if (exprs.Length == 2) {
                return Operand.Create(text.AsSpan().IndexOf(args2.TextValue) + excelIndex);
            }
            var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.TEXT) { args3 = args3.ToText("Function INDEXOF parameter 3 is error!"); if (args3.IsError) { return args3; } }
            if (exprs.Length == 3) {
                return Operand.Create(text.AsSpan(args3.IntValue).IndexOf(args2.TextValue) + args3.IntValue + excelIndex);
            }
            var args4 = exprs[3].Accept(this); if (args4.Type != OperandType.TEXT) { args4 = args4.ToText("Function INDEXOF parameter 4 is error!"); if (args4.IsError) { return args4; } }
            return Operand.Create(text.IndexOf(args2.TextValue, args3.IntValue, args4.IntValue) + excelIndex);
        }
        public virtual Operand VisitLASTINDEXOF_fun(mathParser.LASTINDEXOF_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function LASTINDEXOF parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function LASTINDEXOF parameter 2 is error!"); if (args2.IsError) { return args2; } }

            var text = args1.TextValue;
            if (exprs.Length == 2) {
                return Operand.Create(text.AsSpan().LastIndexOf(args2.TextValue) + excelIndex);
            }
            var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.TEXT) { args3 = args3.ToText("Function LASTINDEXOF parameter 3 is error!"); if (args3.IsError) { return args3; } }
            if (exprs.Length == 3) {
                return Operand.Create(text.AsSpan(args3.IntValue).LastIndexOf(args2.TextValue) + args3.IntValue + excelIndex);
            }
            var args4 = exprs[3].Accept(this); if (args4.Type != OperandType.TEXT) { args4 = args4.ToText("Function LASTINDEXOF parameter 4 is error!"); if (args4.IsError) { return args4; } }
            return Operand.Create(text.LastIndexOf(args2.TextValue, args3.IntValue, args4.IntValue) + excelIndex);
        }
        public virtual Operand VisitSPLIT_fun(mathParser.SPLIT_funContext context)
        {
            var exprs = context.expr(); var args = new List<Operand>(exprs.Length);
            for (int i = 0; i < exprs.Length; i++) { var a = this.Visit(exprs[i]); if (a.Type != OperandType.TEXT) { a = a.ToText($"Function SPLIT parameter {i + 1} is error!"); if (a.IsError) { return a; } } args.Add(a); }
            return Operand.Create(args[0].TextValue.Split(args[1].TextValue.ToArray()));

        }
        public virtual Operand VisitJOIN_fun(mathParser.JOIN_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0];
            if (args1.Type == OperandType.JSON) {
                var o = args1.ToArray(null);
                if (o.IsError == false) {
                    args1 = o;
                }
            }
            if (args1.Type == OperandType.ARRARY) {
                List<string> list = new List<string>();
                var o = F_base_GetList(args1, list);
                if (o == false) return Operand.Error("Function JOIN parameter 1 is error!");

                var args2 = args[1].ToText("Function JOIN parameter 2 is error!");
                if (args2.IsError) { return args2; }

                return Operand.Create(string.Join(args2.TextValue, list));
            } else {
                args1 = args1.ToText("Function JOIN parameter 1 is error!");
                if (args1.IsError) { return args1; }

                List<string> list = new List<string>();
                for (int i = 1; i < args.Count; i++) {
                    var o = F_base_GetList(args[i], list);
                    if (o == false) return Operand.Error($"Function JOIN parameter {i + 1} is error!");
                }

                return Operand.Create(string.Join(args1.TextValue, list));
            }
        }
        public virtual Operand VisitSUBSTRING_fun(mathParser.SUBSTRING_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function SUBSTRING parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function SUBSTRING parameter 2 is error!"); if (args2.IsError) { return args2; } }

            var text = args1.TextValue;
            if (exprs.Length == 2) {
                return Operand.Create(text.AsSpan(args2.IntValue - excelIndex).ToString());
            }
            var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function SUBSTRING parameter 3 is error!"); if (args3.IsError) { return args3; } }
            return Operand.Create(text.AsSpan(args2.IntValue - excelIndex, args3.IntValue).ToString());
        }
        public virtual Operand VisitSTARTSWITH_fun(mathParser.STARTSWITH_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function STARTSWITH parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function STARTSWITH parameter 2 is error!"); if (args2.IsError) { return args2; } }

            var text = args1.TextValue;
            if (exprs.Length == 2) {
                return Operand.Create(text.AsSpan().StartsWith(args2.TextValue.AsSpan()));
            }
            var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.BOOLEAN) { args3 = args3.ToBoolean("Function STARTSWITH parameter 3 is error!"); if (args3.IsError) { return args3; } }
            return Operand.Create(text.AsSpan().StartsWith(args2.TextValue.AsSpan(), args3.BooleanValue ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
        }
        public virtual Operand VisitENDSWITH_fun(mathParser.ENDSWITH_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function ENDSWITH parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function ENDSWITH parameter 2 is error!"); if (args2.IsError) { return args2; } }

            var text = args1.TextValue;
            if (exprs.Length == 2) {
                return Operand.Create(text.AsSpan().EndsWith(args2.TextValue.AsSpan()));
            }
            var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.BOOLEAN) { args3 = args3.ToBoolean("Function ENDSWITH parameter 3 is error!"); if (args3.IsError) { return args3; } }
            return Operand.Create(text.AsSpan().EndsWith(args2.TextValue.AsSpan(), args3.BooleanValue ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
        }
        public virtual Operand VisitISNULLOREMPTY_fun(mathParser.ISNULLOREMPTY_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function ISNULLOREMPTY parameter 1 is error!"); if (args1.IsError) { return args1; } }

            return Operand.Create(string.IsNullOrEmpty(args1.TextValue));
        }
        public virtual Operand VisitISNULLORWHITESPACE_fun(mathParser.ISNULLORWHITESPACE_funContext context)
        {
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function ISNULLORWHITESPACE parameter 1 is error!"); if (args1.IsError) { return args1; } }

            return Operand.Create(string.IsNullOrWhiteSpace(args1.TextValue));
        }
        public virtual Operand VisitREMOVESTART_fun(mathParser.REMOVESTART_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function REMOVESTART parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function REMOVESTART parameter 2 is error!"); if (args2.IsError) { return args2; } }

            StringComparison comparison = StringComparison.Ordinal;
            if (exprs.Length == 3) {
                var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.BOOLEAN) { args3 = args3.ToBoolean("Function REMOVESTART parameter 3 is error!"); if (args3.IsError) { return args3; } }
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
        public virtual Operand VisitREMOVEEND_fun(mathParser.REMOVEEND_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function REMOVEEND parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = exprs[1].Accept(this); if (args2.Type != OperandType.TEXT) { args2 = args2.ToText("Function REMOVEEND parameter 2 is error!"); if (args2.IsError) { return args2; } }

            StringComparison comparison = StringComparison.Ordinal;
            if (exprs.Length == 3) {
                var args3 = exprs[2].Accept(this); if (args3.Type != OperandType.BOOLEAN) { args3 = args3.ToBoolean("Function REMOVESTART parameter 3 is error!"); if (args3.IsError) { return args3; } }
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
        public virtual Operand VisitJSON_fun(mathParser.JSON_funContext context)
        {
            var args1 = context.expr().Accept(this).ToText("Function JSON parameter is error!");
            if (args1.IsError) { return args1; }
            var txt = args1.TextValue;
            if ((txt.StartsWith('{') && txt.EndsWith('}')) || (txt.StartsWith('[') && txt.EndsWith(']'))) {
                try {
                    var json = JsonMapper.ToObject(txt);
                    return Operand.Create(json);
                } catch (Exception) { }
            }
            return Operand.Error("Function JSON parameter is error!");
        }

        #endregion

        #region Lookup
        public virtual Operand VisitVLOOKUP_fun(mathParser.VLOOKUP_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0].ToArray("Function VLOOKUP parameter 1 error!");
            if (args1.IsError) { return args1; }
            var args2 = args[1];

            var args3 = args[2].ToNumber("Function VLOOKUP parameter 3 is error!");
            if (args3.IsError) { return args3; }

            var vague = true;
            if (args.Count == 4) {
                var args4 = args[2].ToBoolean("Function VLOOKUP parameter 4 is error!");
                if (args4.IsError) { return args4; }
                vague = args4.BooleanValue;
            }
            if (args2.Type != OperandType.NULL) {
                var sv = args2.ToText("Function VLOOKUP parameter 2 is error!");
                if (sv.IsError) { return sv; }
                args2 = sv;
            }

            foreach (var item in args1.ArrayValue) {
                var o = item.ToArray("Function VLOOKUP parameter 1 error!");
                if (o.IsError) { return o; }
                if (o.ArrayValue.Count > 0) {
                    var o1 = o.ArrayValue[0];
                    int b = -1;
                    if (args2.Type == OperandType.NUMBER) {
                        var o2 = o1.ToNumber(null);
                        if (o2.IsError == false) {
                            b = MathVisitor.Compare(o2.NumberValue, args2.NumberValue);
                        }
                    } else {
                        var o2 = o1.ToText(null);
                        if (o2.IsError == false) {
                            b = string.CompareOrdinal(o2.TextValue, args2.TextValue);
                        }
                    }
                    if (b == 0) {
                        var index = args3.IntValue - excelIndex;
                        if (index < o.ArrayValue.Count) {
                            return o.ArrayValue[index];
                        }
                    }
                }
            }

            if (vague) //进行模糊匹配
            {
                Operand last = null;
                var index = args3.IntValue - excelIndex;
                foreach (var item in args1.ArrayValue) {
                    var o = item.ToArray(null);
                    if (o.IsError) { return o; }
                    if (o.ArrayValue.Count > 0) {
                        var o1 = o.ArrayValue[0];
                        int b = -1;
                        if (args2.Type == OperandType.NUMBER) {
                            var o2 = o1.ToNumber(null);
                            if (o2.IsError == false) {
                                b = MathVisitor.Compare(o2.NumberValue, args2.NumberValue);
                            }
                        } else {
                            var o2 = o1.ToText(null);
                            if (o2.IsError == false) {
                                b = string.CompareOrdinal(o2.TextValue, args2.TextValue);
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

        public virtual Operand VisitLOOKUP_fun(mathParser.LOOKUP_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0].ToArray("Function LOOKUP parameter 1 error!");
            if (args1.IsError) { return args1; }
            var args2 = args[1].ToText("Function LOOKUP parameter 2 is error!");
            if (args2.IsError) { return args2; }
            if (args1.Type == OperandType.ARRARYJSON) {
                args2 = args2.ToNumber(); if (args2.IsError) { return args2; }
                var range = false;
                if (args.Count == 3) {
                    var t = args[2].ToBoolean("Function LOOKUP parameter 3 is error!"); if (t.IsError) { return t; }
                    range = t.BooleanValue;
                }
                if (((OperandKeyValueList)args1).TryGetValueFloor(args2.NumberValue, range, out Operand operand)) {
                    return operand;
                }
                return Operand.Error("Function LOOKUP not find !");
            }
            var args3 = args[2].ToText("Function LOOKUP parameter 3 is error!");
            if (args3.IsError) { return args3; }

            if (string.IsNullOrWhiteSpace(args2.TextValue)) {
                return Operand.Error("Function LOOKUP parameter 2 is null!");
            }

            var engine = new AntlrLookupEngine();
            if (engine.Parse(args2.TextValue) == false) {
                return Operand.Error("Function LOOKUP parameter 2 Parse is error!");
            }

            foreach (var item in args1.ArrayValue) {
                var json = item.ToJson(null);
                if (json.IsError == false) {
                    engine.Json = json;
                    try {
                        var o = engine.Evaluate().ToBoolean(null);
                        if (o.IsError == false) {
                            if (o.BooleanValue) {
                                var v = json.JsonValue[args3.TextValue];
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

        public virtual Operand VisitArray_fun(mathParser.Array_funContext context)
        {
            var args = new List<Operand>();
            foreach (var item in context.expr()) { var aa = item.Accept(this); if (aa.IsError) { return aa; } args.Add(aa); }
            return Operand.Create(args);
        }
        public virtual Operand VisitBracket_fun(mathParser.Bracket_funContext context)
        {
            return context.expr().Accept(this);
        }

        public virtual Operand VisitNUM_fun(mathParser.NUM_funContext context)
        {
            var d = decimal.Parse(context.num().GetText(), NumberStyles.Any, CultureInfo.InvariantCulture);
            if (context.unit() == null) { return Operand.Create(d); }

            var unit = context.unit().GetText();
            var dict = NumberUnitTypeHelper.GetUnitTypedict();
            d = NumberUnitTypeHelper.TransformationUnit(d, dict[unit], DistanceUnit, AreaUnit, VolumeUnit, MassUnit);
            return Operand.Create(d);
        }
        public Operand VisitNum(mathParser.NumContext context)
        {
            var d = decimal.Parse(context.GetText(), NumberStyles.Any, CultureInfo.InvariantCulture);
            return Operand.Create(d);
        }
        public Operand VisitUnit(mathParser.UnitContext context)
        {
            string text = context.GetText();
            return Operand.Create(text);
        }
        public virtual Operand VisitSTRING_fun(mathParser.STRING_funContext context)
        {
            var opd = context.GetText();
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
        public virtual Operand VisitNULL_fun(mathParser.NULL_funContext context)
        {
            return Operand.CreateNull();
        }
        public virtual Operand VisitPARAMETER_fun(mathParser.PARAMETER_funContext context)
        {
            ITerminalNode node = context.PARAMETER();
            if (node != null) {
                return GetParameter(node.GetText());
            }
            node = context.PARAMETER2();
            if (node != null) {
                string str = node.GetText();
                if (str.StartsWith('@')) {
                    return GetParameter(str.AsSpan(1).ToString());
                }
                return GetParameter(str.AsSpan(1, str.Length - 2).ToString());
            }
            //防止 多重引用 
            if (context.expr() != null) {
                var p = context.expr().Accept(this).ToText("Function PARAMETER first parameter is error!");
                if (p.IsError) return p;

                if (GetParameter != null) {
                    return GetParameter(p.TextValue);
                }
            }
            return Operand.Error("Function PARAMETER first parameter is error!");
        }


        public virtual Operand VisitParameter2(mathParser.Parameter2Context context)
        {
            return Operand.Create(context.children[0].GetText());
        }

        public virtual Operand VisitGetJsonValue_fun(mathParser.GetJsonValue_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this);
            if (args1.IsError) { return args1; }

            var obj = args1;
            Operand op;
            if (context.parameter2() != null) {
                op = context.parameter2().Accept(this);
            } else {
                op = exprs[1].Accept(this);
                if (op.IsError) {
                    op = Operand.Create(exprs[1].GetText());
                }
            }

            if (obj.Type == OperandType.ARRARY) {
                op = op.ToNumber("ARRARY index is error!");
                if (op.IsError) { return op; }
                var index = op.IntValue - excelIndex;
                if (index < obj.ArrayValue.Count)
                    return obj.ArrayValue[index];
                return Operand.Error($"ARRARY index {index} greater than maximum length!");
            }
            if (obj.Type == OperandType.ARRARYJSON) {
                if (op.Type == OperandType.NUMBER) {
                    if (((OperandKeyValueList)obj).TryGetValue(op.NumberValue.ToString(), out Operand operand)) {
                        return operand;
                    }
                    return Operand.Error($"Parameter name `{op.TextValue}` is missing!");
                } else if (op.Type == OperandType.TEXT) {
                    if (((OperandKeyValueList)obj).TryGetValue(op.TextValue, out Operand operand)) {
                        return operand;
                    }
                    return Operand.Error($"Parameter name `{op.TextValue}` is missing!");
                }
                return Operand.Error("Parameter name is missing!");
            }

            if (obj.Type == OperandType.JSON) {
                var json = obj.JsonValue;
                if (json.IsArray) {
                    op = op.ToNumber("JSON parameter index is error!");
                    if (op.IsError) { return op; }
                    var index = op.IntValue - excelIndex;
                    if (index < json.Count) {
                        var v = json[index];
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


        public virtual Operand VisitDiyFunction_fun(mathParser.DiyFunction_funContext context)
        {
            if (DiyFunction != null) {
                var funName = context.PARAMETER().GetText();
                var args = new List<Operand>();
                foreach (var item in context.expr()) { var aa = item.Accept(this); args.Add(aa); }
                return DiyFunction(funName, args);
            }
            return Operand.Error("DiyFunction is error!");
        }

        public Operand VisitPARAM_fun(mathParser.PARAM_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText(); if (args1.IsError) return args1; }
            var result = GetParameter(args1.TextValue);
            if (result.IsError) {
                if (exprs.Length == 2) {
                    return exprs[1].Accept(this);
                }
            }
            return result;
        }

        public Operand VisitHAS_fun(mathParser.HAS_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.IsError) { return args1; }
            var args2 = exprs[1].Accept(this).ToText("Function HAS parameter 2 is error!"); if (args2.IsError) { return args2; }

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
                            if (v.BooleanValue.ToString().ToUpper() == args2.TextValue) { return Operand.True; }
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
            return Operand.Error("Function HAS parameter 1 is error!");
        }
        public Operand VisitHASVALUE_fun([Antlr4.Runtime.Misc.NotNull] mathParser.HASVALUE_funContext context)
        {
            var exprs = context.expr();
            var args1 = exprs[0].Accept(this); if (args1.IsError) { return args1; }
            var args2 = exprs[1].Accept(this).ToText("Function HASVALUE parameter 2 is error!"); if (args2.IsError) { return args2; }

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
                            if (v.BooleanValue.ToString().ToUpper() == args2.TextValue) { return Operand.True; }
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
                            if (v.BooleanValue.ToString().ToUpper() == args2.TextValue) { return Operand.True; }
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
            return Operand.Error("Function HASVALUE parameter 1 is error!");
        }

        public Operand VisitArrayJson_fun(mathParser.ArrayJson_funContext context)
        {
            OperandKeyValueList result = new OperandKeyValueList(null);
            var js = context.arrayJson();
            for (int i = 0; i < js.Length; i++) {
                var item = js[i];
                var aa = item.Accept(this); if (aa.IsError) { return aa; }
                result.AddValue((KeyValue)((OperandKeyValue)aa).Value);
            }
            return result;
        }

        public Operand VisitArrayJson(mathParser.ArrayJsonContext context)
        {
            KeyValue keyValue = new KeyValue();
            if (context.NUM() != null) {
                if (int.TryParse(context.NUM().GetText(), out int key)) {
                    keyValue.Key = key.ToString();
                } else {
                    return Operand.Error("Json key '" + context.NUM().GetText() + "' is error!");
                }
            }
            if (context.STRING() != null) {
                var opd = context.STRING().GetText();
                StringBuilder sb = new StringBuilder(opd.Length - 2);
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
                keyValue.Key = sb.ToString();
            }
            if (context.parameter2() != null) {
                keyValue.Key = context.parameter2().GetText();
            }
            keyValue.Value = context.expr().Accept(this);
            return new OperandKeyValue(keyValue);
        }

        public Operand VisitERROR_fun(mathParser.ERROR_funContext context)
        {
            if (context.expr() == null) { return Operand.Error(""); }
            var args1 = context.expr().Accept(this); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText(); if (args1.IsError) return args1; }
            return Operand.Error(args1.TextValue);
        }



        #endregion
    }
}
