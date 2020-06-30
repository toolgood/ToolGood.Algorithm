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
        #region base
        private Operand ThrowError(string errMsg, params Operand[] ops)
        {
            foreach (var item in ops)
            {
                if (item.Type == OperandType.ERROR)
                {
                    return item;
                }
            }
            return new Operand(OperandType.ERROR, errMsg);
        }

        public Operand VisitProg([NotNull] mathParser.ProgContext context)
        {
            return this.Visit(context.expr());
        }

        public Operand VisitMulDiv_fun([NotNull] mathParser.MulDiv_funContext context)
        {
            var leftValue = this.Visit(context.expr(0));
            if (leftValue.Type == OperandType.ERROR) { return leftValue; }
            var rightValue = this.Visit(context.expr(1));
            if (rightValue.Type == OperandType.ERROR) { return rightValue; }

            if (context.op.Type == mathLexer.MUL)
            {
                if (rightValue.Type == OperandType.NUMBER)
                {
                    if (leftValue.Type == OperandType.NUMBER)
                    {
                        return new Operand(OperandType.NUMBER, leftValue.NumberValue * rightValue.NumberValue);
                    }
                    else if (leftValue.Type == OperandType.DATE)
                    {
                        return new Operand(OperandType.DATE, leftValue.DateValue * rightValue.NumberValue);
                    }
                    else if (leftValue.Type == OperandType.BOOLEAN)
                    {
                        return new Operand(OperandType.NUMBER, (leftValue.BooleanValue ? 1.0 : 0.0) * rightValue.NumberValue);
                    }
                }
                if (rightValue.Type == OperandType.BOOLEAN)
                {
                    if (leftValue.Type == OperandType.NUMBER)
                    {
                        return new Operand(OperandType.NUMBER, leftValue.NumberValue * (rightValue.BooleanValue ? 1.0 : 0.0));
                    }
                    else if (leftValue.Type == OperandType.DATE)
                    {
                        return new Operand(OperandType.DATE, leftValue.DateValue * (rightValue.BooleanValue ? 1.0 : 0.0));
                    }
                    else if (leftValue.Type == OperandType.BOOLEAN)
                    {
                        return new Operand(OperandType.NUMBER, (leftValue.BooleanValue ? 1.0 : 0.0) * (rightValue.BooleanValue ? 1.0 : 0.0));
                    }
                }
                if (rightValue.Type == OperandType.DATE)
                {
                    if (leftValue.Type == OperandType.NUMBER)
                    {
                        return new Operand(OperandType.NUMBER, leftValue.NumberValue * rightValue.DateValue);
                    }
                    else if (leftValue.Type == OperandType.BOOLEAN)
                    {
                        return new Operand(OperandType.NUMBER, (leftValue.BooleanValue ? 1.0 : 0.0) * rightValue.DateValue);
                    }
                }
                return ThrowError("两个参数不能相乘");
            }
            else if (context.op.Type == mathLexer.DIV)
            {
                if (rightValue.Type == OperandType.NUMBER)
                {
                    if (rightValue.NumberValue == 0)
                    {
                        return ThrowError("无法除0");
                    }

                    if (leftValue.Type == OperandType.NUMBER)
                    {
                        return new Operand(OperandType.NUMBER, leftValue.NumberValue / rightValue.NumberValue);
                    }
                    else if (leftValue.Type == OperandType.DATE)
                    {
                        return new Operand(OperandType.DATE, leftValue.DateValue / rightValue.NumberValue);
                    }
                    else if (leftValue.Type == OperandType.BOOLEAN)
                    {
                        return new Operand(OperandType.NUMBER, (leftValue.BooleanValue ? 1.0 : 0.0) / rightValue.NumberValue);
                    }
                }
                if (rightValue.Type == OperandType.DATE)
                {
                    if (leftValue.Type == OperandType.NUMBER)
                    {
                        return new Operand(OperandType.NUMBER, leftValue.NumberValue / rightValue.DateValue);
                    }
                    else if (leftValue.Type == OperandType.BOOLEAN)
                    {
                        return new Operand(OperandType.NUMBER, (leftValue.BooleanValue ? 1.0 : 0.0) / rightValue.DateValue);
                    }
                }
                return ThrowError("两个参数不能相除");
            }
            else if (context.op.Type == mathLexer.MOD_2)
            {
                if (rightValue.Type == OperandType.NUMBER)
                {
                    if (rightValue.NumberValue == 0)
                    {
                        return ThrowError("无法除0");
                    }
                    if (leftValue.Type == OperandType.NUMBER)
                    {
                        return new Operand(OperandType.NUMBER, leftValue.NumberValue % rightValue.NumberValue);
                    }
                    else if (leftValue.Type == OperandType.DATE)
                    {
                        return new Operand(OperandType.DATE, leftValue.DateValue % rightValue.NumberValue);
                    }
                    else if (leftValue.Type == OperandType.BOOLEAN)
                    {
                        return new Operand(OperandType.NUMBER, (leftValue.BooleanValue ? 1.0 : 0.0) % rightValue.NumberValue);
                    }
                }
                if (rightValue.Type == OperandType.DATE)
                {
                    if (leftValue.Type == OperandType.NUMBER)
                    {
                        return new Operand(OperandType.NUMBER, leftValue.NumberValue % rightValue.DateValue);
                    }
                    else if (leftValue.Type == OperandType.BOOLEAN)
                    {
                        return new Operand(OperandType.NUMBER, (leftValue.BooleanValue ? 1.0 : 0.0) % rightValue.DateValue);
                    }
                }
                return ThrowError("两个参数不能相除取余");
            }
            return ThrowError("VisitMulDiv_fun出错了");
        }

        public Operand VisitAddSub_fun([NotNull] mathParser.AddSub_funContext context)
        {
            var leftValue = this.Visit(context.expr(0));
            if (leftValue.Type == OperandType.ERROR) { return leftValue; }
            var rightValue = this.Visit(context.expr(1));
            if (rightValue.Type == OperandType.ERROR) { return rightValue; }

            if (context.op.Type == mathLexer.ADD)
            {
                if (rightValue.Type == OperandType.NUMBER)
                {
                    if (leftValue.Type == OperandType.NUMBER)
                    {
                        return new Operand(OperandType.NUMBER, leftValue.NumberValue + rightValue.NumberValue);
                    }
                    else if (leftValue.Type == OperandType.DATE)
                    {
                        return new Operand(OperandType.DATE, leftValue.DateValue + rightValue.NumberValue);
                    }
                    else if (leftValue.Type == OperandType.BOOLEAN)
                    {
                        return new Operand(OperandType.NUMBER, (leftValue.BooleanValue ? 1.0 : 0.0) + rightValue.NumberValue);
                    }
                }
                if (rightValue.Type == OperandType.BOOLEAN)
                {
                    if (leftValue.Type == OperandType.NUMBER)
                    {
                        return new Operand(OperandType.NUMBER, leftValue.NumberValue + (rightValue.BooleanValue ? 1.0 : 0.0));
                    }
                    else if (leftValue.Type == OperandType.DATE)
                    {
                        return new Operand(OperandType.DATE, leftValue.DateValue + (rightValue.BooleanValue ? 1.0 : 0.0));
                    }
                    else if (leftValue.Type == OperandType.BOOLEAN)
                    {
                        return new Operand(OperandType.NUMBER, (leftValue.BooleanValue ? 1.0 : 0.0) + (rightValue.BooleanValue ? 1.0 : 0.0));
                    }
                }
                if (rightValue.Type == OperandType.DATE)
                {
                    if (leftValue.Type == OperandType.NUMBER)
                    {
                        return new Operand(OperandType.NUMBER, leftValue.NumberValue + rightValue.DateValue);
                    }
                    else if (leftValue.Type == OperandType.DATE)
                    {
                        return new Operand(OperandType.DATE, leftValue.DateValue + rightValue.DateValue);
                    }
                    else if (leftValue.Type == OperandType.BOOLEAN)
                    {
                        return new Operand(OperandType.NUMBER, (leftValue.BooleanValue ? 1.0 : 0.0) + rightValue.DateValue);
                    }
                }
                //if (rightValue.Type == OperandType.STRING || leftValue.Type == OperandType.STRING) {
                //    return new Operand(OperandType.STRING, leftValue.StringValue + rightValue.StringValue);
                //}
                return ThrowError("两个参数不能相加");
            }
            else if (context.op.Type == mathLexer.SUB)
            {
                if (rightValue.Type == OperandType.NUMBER)
                {
                    if (leftValue.Type == OperandType.NUMBER)
                    {
                        return new Operand(OperandType.NUMBER, leftValue.NumberValue - rightValue.NumberValue);
                    }
                    else if (leftValue.Type == OperandType.DATE)
                    {
                        return new Operand(OperandType.DATE, leftValue.DateValue - rightValue.NumberValue);
                    }
                    else if (leftValue.Type == OperandType.BOOLEAN)
                    {
                        return new Operand(OperandType.NUMBER, (leftValue.BooleanValue ? 1.0 : 0.0) - rightValue.NumberValue);
                    }
                }
                if (rightValue.Type == OperandType.BOOLEAN)
                {
                    if (leftValue.Type == OperandType.NUMBER)
                    {
                        return new Operand(OperandType.NUMBER, leftValue.NumberValue - (rightValue.BooleanValue ? 1.0 : 0.0));
                    }
                    else if (leftValue.Type == OperandType.DATE)
                    {
                        return new Operand(OperandType.DATE, leftValue.DateValue - (rightValue.BooleanValue ? 1.0 : 0.0));
                    }
                    else if (leftValue.Type == OperandType.BOOLEAN)
                    {
                        return new Operand(OperandType.NUMBER, (leftValue.BooleanValue ? 1.0 : 0.0) - (rightValue.BooleanValue ? 1.0 : 0.0));
                    }
                }
                if (rightValue.Type == OperandType.DATE)
                {
                    if (leftValue.Type == OperandType.NUMBER)
                    {
                        return new Operand(OperandType.NUMBER, leftValue.NumberValue - rightValue.DateValue);
                    }
                    else if (leftValue.Type == OperandType.DATE)
                    {
                        return new Operand(OperandType.DATE, leftValue.DateValue - rightValue.DateValue);
                    }
                    else if (leftValue.Type == OperandType.BOOLEAN)
                    {
                        return new Operand(OperandType.NUMBER, (leftValue.BooleanValue ? 1.0 : 0.0) - rightValue.DateValue);
                    }
                }
                return ThrowError("两个参数不能相减");
            }
            else if (context.op.Type == mathLexer.MERGE)
            {
                return new Operand(OperandType.STRING, leftValue.StringValue + rightValue.StringValue);
            }


            throw new NotImplementedException();
        }

        public Operand VisitJudge_fun([NotNull] mathParser.Judge_funContext context)
        {
            var leftValue = this.Visit(context.expr(0));
            if (leftValue.Type == OperandType.ERROR) { return leftValue; }
            var rightValue = this.Visit(context.expr(1));
            if (rightValue.Type == OperandType.ERROR) { return rightValue; }
            int type = context.op.Type;

            int r = 0;
            if (leftValue.Type == rightValue.Type)
            {
                if (leftValue.Type == OperandType.STRING)
                {
                    r = compare(leftValue.StringValue, rightValue.StringValue);
                }
                else
                {
                    r = compare(leftValue.NumberValue, rightValue.NumberValue);
                }
            }
            else if ((leftValue.Type == OperandType.DATE && rightValue.Type == OperandType.STRING) || (rightValue.Type == OperandType.DATE && leftValue.Type == OperandType.STRING))
            {
                r = compare(leftValue.StringValue, rightValue.StringValue);
            }
            else if (leftValue.Type == OperandType.STRING || rightValue.Type == OperandType.STRING)
            {
                return new Operand(OperandType.ERROR, "两个类型无法比较");
            }
            else
            {
                r = compare(leftValue.NumberValue, rightValue.NumberValue);
            }
            if (type == mathLexer.LT)
            {
                return new Operand(OperandType.BOOLEAN, r == -1);
            }
            else if (type == mathLexer.LE)
            {
                return new Operand(OperandType.BOOLEAN, r <= 0);
            }
            else if (type == mathLexer.GT)
            {
                return new Operand(OperandType.BOOLEAN, r == 1);
            }
            else if (type == mathLexer.GE)
            {
                return new Operand(OperandType.BOOLEAN, r >= 0);
            }
            else if (type == mathLexer.ET)
            {
                return new Operand(OperandType.BOOLEAN, r == 0);
            }
            return new Operand(OperandType.BOOLEAN, r != 0);

            throw new NotImplementedException();
        }
        private int compare(double t1, double t2)
        {
            t1 = Math.Round(t1, 12);
            t2 = Math.Round(t2, 12);
            if (t1 == t2)
            {
                return 0;
            }
            else if (t1 > t2)
            {
                return 1;
            }
            return -1;
        }
        private int compare(string t1, string t2)
        {
            if (t1 == t2)
            {
                return 0;
            }
            List<string> ts = new List<string>() { t1, t2 };
            ts = ts.OrderBy(q => q).ToList();
            if (t1 == ts[1])
            {
                return 1;
            }
            return -1;
        }

        public Operand VisitArray_fun([NotNull] mathParser.Array_funContext context)
        {
            throw new NotImplementedException();
        }
        public Operand VisitBracket_fun([NotNull] mathParser.Bracket_funContext context)
        {
            return this.Visit(context.expr());
        }

        public Operand VisitNUM_fun([NotNull] mathParser.NUM_funContext context)
        {
            var t = context.NUM().GetText();
            var d = double.Parse(t, CultureInfo.GetCultureInfo("en-US"));
            return new Operand(OperandType.NUMBER, d);
        }
        public Operand VisitSTRING_fun([NotNull] mathParser.STRING_funContext context)
        {
            var t = context.STRING().GetText();
            t = t.Substring(1, t.Length - 2);
            return new Operand(OperandType.STRING, t);
        }
        public Operand VisitPARAMETER_fun([NotNull] mathParser.PARAMETER_funContext context)
        {
            var t = context.PARAMETER().GetText();
            t = t.Substring(1, t.Length - 2);
            if (GetParameter != null)
            {
                return GetParameter(t);
            }
            return new Operand(OperandType.ERROR, t);
        }

        #endregion

        #region flow
        public Operand VisitIF_fun([NotNull] mathParser.IF_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { arg.Add(this.Visit(item)); }

            var op = CheckArgsCount("IF", arg, new OperandType[][] {
                new [] { OperandType.ANY },
                new [] { OperandType.ANY, OperandType.ANY },
                new [] { OperandType.ANY, OperandType.ANY, OperandType.ANY },
                 });
            if (op != null) { return op; }

            var b = true;
            if (arg[0].Type == OperandType.BOOLEAN)
            {
                b = arg[0].BooleanValue;
            }
            else if (arg[0].Type == OperandType.NUMBER)
            {
                b = arg[0].IntValue != 0;
            }

            if (b)
            {
                if (arg.Count > 1)
                {
                    return arg[1];
                }
                return new Operand(OperandType.BOOLEAN, true);
            }
            if (arg.Count == 3)
            {
                return arg[2];
            }
            return new Operand(OperandType.BOOLEAN, false);
        }
        public Operand VisitIFERROR_fun([NotNull] mathParser.IFERROR_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { arg.Add(this.Visit(item)); }

            var op = CheckArgsCount("IFERROR", arg, new OperandType[][] {
                new [] { OperandType.ANY },
                new [] { OperandType.ANY, OperandType.ANY },
                new [] { OperandType.ANY, OperandType.ANY, OperandType.ANY },
                 });
            if (op != null) { return op; }

            if (arg[0].Type == OperandType.ERROR)
            {
                if (arg.Count > 1)
                {
                    return arg[1];
                }
                return new Operand(OperandType.BOOLEAN, true);
            }
            if (arg.Count == 3)
            {
                return arg[2];
            }
            return new Operand(OperandType.BOOLEAN, false);

        }
        public Operand VisitIFNUMBER_fun([NotNull] mathParser.IFNUMBER_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { arg.Add(this.Visit(item)); }

            var op = CheckArgsCount("IFNUMBER", arg, new OperandType[][] {
                new OperandType[] { OperandType.ANY },
                new OperandType[] { OperandType.ANY, OperandType.ANY },
                new OperandType[] { OperandType.ANY, OperandType.ANY, OperandType.ANY },
                 });
            if (op != null) { return op; }

            if (arg.Count < 2) return ThrowError("IFNUMBER 中参数不足");
            var b = false;
            if (arg[0].Type == OperandType.NUMBER)
            {
                b = arg[0].IntValue != 0;
            }
            else if (arg[0].Type == OperandType.DATE)
            {
                b = true;
            }

            if (b)
            {
                if (arg.Count > 1)
                {
                    return arg[1];
                }
                return new Operand(OperandType.BOOLEAN, true);
            }
            if (arg.Count == 3)
            {
                return arg[2];
            }
            return new Operand(OperandType.BOOLEAN, false);
        }
        public Operand VisitIFTEXT_fun([NotNull] mathParser.IFTEXT_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { arg.Add(this.Visit(item)); }


            if (arg.Count < 2) return ThrowError("ISSTRING 中参数不足");
            if (arg[0].Type == OperandType.STRING)
            {
                if (arg.Count > 1)
                {
                    return arg[1];
                }
                return new Operand(OperandType.BOOLEAN, true);
            }
            else if (arg[0].Type == OperandType.DATE)
            {
                if (arg[0].DateValue.srcText != null)
                {
                    if (arg.Count > 1)
                    {
                        return arg[1];
                    }
                    return new Operand(OperandType.BOOLEAN, true);
                }
            }
            if (arg.Count == 3) return arg[2];
            return new Operand(OperandType.BOOLEAN, false);
        }
        public Operand VisitISNUMBER_fun([NotNull] mathParser.ISNUMBER_funContext context)
        {
            var leftValue = this.Visit(context.expr());
            if (leftValue.Type == OperandType.ERROR) { return leftValue; }

            if (leftValue.Type == OperandType.NUMBER)
            {
                return new Operand(OperandType.BOOLEAN, true);
            }
            else if (leftValue.Type == OperandType.DATE)
            {
                return new Operand(OperandType.BOOLEAN, true);
            }
            return new Operand(OperandType.BOOLEAN, false);
        }
        public Operand VisitISTEXT_fun([NotNull] mathParser.ISTEXT_funContext context)
        {
            var leftValue = this.Visit(context.expr());
            if (leftValue.Type == OperandType.ERROR) { return leftValue; }

            if (leftValue.Type == OperandType.STRING)
            {
                return new Operand(OperandType.BOOLEAN, true);
            }
            else if (leftValue.Type == OperandType.DATE)
            {
                return new Operand(OperandType.BOOLEAN, leftValue.DateValue.srcText != null);
            }
            return new Operand(OperandType.BOOLEAN, false);
        }
        public Operand VisitISERROR_fun([NotNull] mathParser.ISERROR_funContext context)
        {
            var leftValue = this.Visit(context.expr());
            if (leftValue.Type == OperandType.ERROR)
            {
                return new Operand(OperandType.BOOLEAN, true);
            }
            return new Operand(OperandType.BOOLEAN, false);
        }
        public Operand VisitAND_fun([NotNull] mathParser.AND_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { arg.Add(this.Visit(item)); }

            var b = true;
            foreach (var item in arg)
            {
                if (item.Type == OperandType.BOOLEAN)
                {
                    if (item.BooleanValue == false)
                    {
                        b = false;
                        break;
                    }
                }
                else if (item.Type == OperandType.NUMBER)
                {
                    if (item.IntValue == 0)
                    {
                        b = false;
                        break;
                    }
                }
            }
            return new Operand(OperandType.BOOLEAN, b);
        }
        public Operand VisitOR_fun([NotNull] mathParser.OR_funContext context)
        {
            var arg = new List<Operand>();
            foreach (var item in context.expr()) { arg.Add(this.Visit(item)); }

            var b = false;

            foreach (var item in arg)
            {
                if (item.Type == OperandType.BOOLEAN)
                {
                    if (item.BooleanValue == true)
                    {
                        b = true;
                        break;
                    }
                }
                else if (item.Type == OperandType.NUMBER)
                {
                    if (item.IntValue != 0)
                    {
                        b = true;
                        break;
                    }
                }
            }
            return new Operand(OperandType.BOOLEAN, b);
        }
        public Operand VisitNOT_fun([NotNull] mathParser.NOT_funContext context)
        {
            var leftValue = this.Visit(context.expr());
            if (leftValue.Type == OperandType.ERROR) { return leftValue; }

            if (leftValue.Type == OperandType.BOOLEAN)
            {
                return new Operand(OperandType.BOOLEAN, !leftValue.BooleanValue);
            }
            else if (leftValue.Type == OperandType.NUMBER)
            {
                return new Operand(OperandType.BOOLEAN, (leftValue.NumberValue == 0));
            }
            return new Operand(OperandType.BOOLEAN, false);
        }
        public Operand VisitTRUE_fun([NotNull] mathParser.TRUE_funContext context)
        {
            return new Operand(OperandType.BOOLEAN, true);
        }
        public Operand VisitFALSE_fun([NotNull] mathParser.FALSE_funContext context)
        {
            return new Operand(OperandType.BOOLEAN, false);
        }
        #endregion

        #region math
        public Operand VisitPI_fun([NotNull] mathParser.PI_funContext context)
        {
            return new Operand(OperandType.NUMBER, Math.PI);
        }

        public Operand VisitABS_fun([NotNull] mathParser.ABS_funContext context)
        {
            var leftValue = this.Visit(context.expr()).ToNumber("ABS left value");
            if (leftValue.Type == OperandType.ERROR) { return leftValue; }


            return new Operand(OperandType.NUMBER, Math.Abs(leftValue.NumberValue));
        }
        public Operand VisitQUOTIENT_fun([NotNull] mathParser.QUOTIENT_funContext context)
        {
            var leftValue = this.Visit(context.expr(0)).ToNumber("ABS leftValue");
            if (leftValue.Type == OperandType.ERROR) { return leftValue; }
            var rightValue = this.Visit(context.expr(1)).ToNumber("ABS rightValue");
            if (rightValue.Type == OperandType.ERROR) { return rightValue; }

            if (rightValue.Type == OperandType.NUMBER || rightValue.Type == OperandType.BOOLEAN)
            {
                if (rightValue.NumberValue == 0)
                {
                    return ThrowError("div 0 error!");
                }
                if (leftValue.Type == OperandType.NUMBER || leftValue.Type == OperandType.BOOLEAN)
                {
                    return new Operand(OperandType.NUMBER, (double) (int) (leftValue.NumberValue / rightValue.NumberValue));
                }
                else if (leftValue.Type == OperandType.DATE)
                {
                    return new Operand(OperandType.DATE, (double) (int) (leftValue.DateValue / rightValue.NumberValue));
                }
            }
            if (rightValue.Type == OperandType.DATE)
            {
                if (leftValue.Type == OperandType.NUMBER || leftValue.Type == OperandType.BOOLEAN)
                {
                    return new Operand(OperandType.NUMBER, (double) (int) (leftValue.NumberValue / rightValue.DateValue));
                }
            }
            return ThrowError("两个参数不能相除取商的整数部分");

        }
        public Operand VisitMOD_fun([NotNull] mathParser.MOD_funContext context)
        {
            var leftValue = this.Visit(context.expr(0));
            if (leftValue.Type == OperandType.ERROR) { return leftValue; }
            var rightValue = this.Visit(context.expr(1));
            if (rightValue.Type == OperandType.ERROR) { return rightValue; }

            if (rightValue.Type == OperandType.NUMBER || rightValue.Type == OperandType.BOOLEAN)
            {
                if (rightValue.NumberValue == 0)
                {
                    return ThrowError("div 0 error!");
                }
                if (leftValue.Type == OperandType.NUMBER || leftValue.Type == OperandType.BOOLEAN)
                {
                    return new Operand(OperandType.NUMBER, leftValue.NumberValue % rightValue.NumberValue);
                }
                else if (leftValue.Type == OperandType.DATE)
                {
                    return new Operand(OperandType.DATE, leftValue.DateValue % rightValue.NumberValue);
                }
            }
            if (rightValue.Type == OperandType.DATE)
            {
                if (leftValue.Type == OperandType.NUMBER || leftValue.Type == OperandType.BOOLEAN)
                {
                    return new Operand(OperandType.NUMBER, leftValue.NumberValue % rightValue.DateValue);
                }

            }
            return ThrowError("两个参数不能相除取余");
        }
        public Operand VisitSIGN_fun([NotNull] mathParser.SIGN_funContext context)
        {
            var leftValue = this.Visit(context.expr());
            if (leftValue.Type == OperandType.ERROR) { return leftValue; }

            if (Operand.IsNumber(leftValue))
            {
                return new Operand(OperandType.NUMBER, (double) Math.Sign(leftValue.NumberValue));
            }
            return ThrowError("SIGN fun is error");
        }



        #endregion




        public Operand VisitACOSH_fun([NotNull] mathParser.ACOSH_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitACOS_fun([NotNull] mathParser.ACOS_funContext context)
        {
            throw new NotImplementedException();
        }




        public Operand VisitASC_fun([NotNull] mathParser.ASC_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitASINH_fun([NotNull] mathParser.ASINH_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitASIN_fun([NotNull] mathParser.ASIN_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitATAN2_fun([NotNull] mathParser.ATAN2_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitATANH_fun([NotNull] mathParser.ATANH_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitATAN_fun([NotNull] mathParser.ATAN_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitAVEDEV_fun([NotNull] mathParser.AVEDEV_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitAVERAGEIF_fun([NotNull] mathParser.AVERAGEIF_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitAVERAGE_fun([NotNull] mathParser.AVERAGE_funContext context)
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


        public Operand VisitCEILING_fun([NotNull] mathParser.CEILING_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitCHAR_fun([NotNull] mathParser.CHAR_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitCLEAN_fun([NotNull] mathParser.CLEAN_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitCODE_fun([NotNull] mathParser.CODE_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitCOMBIN_fun([NotNull] mathParser.COMBIN_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitCONCATENATE_fun([NotNull] mathParser.CONCATENATE_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitCOSH_fun([NotNull] mathParser.COSH_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitCOS_fun([NotNull] mathParser.COS_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitCOUNTIF_fun([NotNull] mathParser.COUNTIF_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitCOUNT_fun([NotNull] mathParser.COUNT_funContext context)
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

        public Operand VisitCRC8_fun([NotNull] mathParser.CRC8_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitDATEDIF_fun([NotNull] mathParser.DATEDIF_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitDATEVALUE_fun([NotNull] mathParser.DATEVALUE_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitDATE_fun([NotNull] mathParser.DATE_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitDAYS360_fun([NotNull] mathParser.DAYS360_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitDAY_fun([NotNull] mathParser.DAY_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitDEGREES_fun([NotNull] mathParser.DEGREES_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitDEVSQ_fun([NotNull] mathParser.DEVSQ_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitEDATE_fun([NotNull] mathParser.EDATE_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitENDSWITH_fun([NotNull] mathParser.ENDSWITH_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitEOMONTH_fun([NotNull] mathParser.EOMONTH_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitEVEN_fun([NotNull] mathParser.EVEN_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitEXACT_fun([NotNull] mathParser.EXACT_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitEXPONDIST_fun([NotNull] mathParser.EXPONDIST_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitEXP_fun([NotNull] mathParser.EXP_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitFACTDOUBLE_fun([NotNull] mathParser.FACTDOUBLE_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitFACT_fun([NotNull] mathParser.FACT_funContext context)
        {
            throw new NotImplementedException();
        }



        public Operand VisitFDIST_fun([NotNull] mathParser.FDIST_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitFIND_fun([NotNull] mathParser.FIND_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitFINV_fun([NotNull] mathParser.FINV_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitFISHERINV_fun([NotNull] mathParser.FISHERINV_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitFISHER_fun([NotNull] mathParser.FISHER_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitFIXED_fun([NotNull] mathParser.FIXED_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitFLOOR_fun([NotNull] mathParser.FLOOR_funContext context)
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

        public Operand VisitGCD_fun([NotNull] mathParser.GCD_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitGEOMEAN_fun([NotNull] mathParser.GEOMEAN_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitGUID_fun([NotNull] mathParser.GUID_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitHARMEAN_fun([NotNull] mathParser.HARMEAN_funContext context)
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

        public Operand VisitHOUR_fun([NotNull] mathParser.HOUR_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitHTMLDECODE_fun([NotNull] mathParser.HTMLDECODE_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitHTMLENCODE_fun([NotNull] mathParser.HTMLENCODE_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitHYPGEOMDIST_fun([NotNull] mathParser.HYPGEOMDIST_funContext context)
        {
            throw new NotImplementedException();
        }






        public Operand VisitINDEXOF_fun([NotNull] mathParser.INDEXOF_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitISMATCH_fun([NotNull] mathParser.ISMATCH_funContext context)
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


        public Operand VisitISREGEX_fun([NotNull] mathParser.ISREGEX_funContext context)
        {
            throw new NotImplementedException();
        }


        public Operand VisitJIS_fun([NotNull] mathParser.JIS_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitJOIN_fun([NotNull] mathParser.JOIN_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitJSON_fun([NotNull] mathParser.JSON_funContext context)
        {
            throw new NotImplementedException();
        }


        public Operand VisitLARGE_fun([NotNull] mathParser.LARGE_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitLASTINDEXOF_fun([NotNull] mathParser.LASTINDEXOF_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitLCM_fun([NotNull] mathParser.LCM_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitLEFT_fun([NotNull] mathParser.LEFT_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitLEN_fun([NotNull] mathParser.LEN_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitLN_fun([NotNull] mathParser.LN_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitLOG10_fun([NotNull] mathParser.LOG10_funContext context)
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

        public Operand VisitLOG_fun([NotNull] mathParser.LOG_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitLOWER_fun([NotNull] mathParser.LOWER_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitLTRIM_fun([NotNull] mathParser.LTRIM_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitMAX_fun([NotNull] mathParser.MAX_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitMD5_fun([NotNull] mathParser.MD5_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitMEDIAN_fun([NotNull] mathParser.MEDIAN_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitMID_fun([NotNull] mathParser.MID_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitMINUTE_fun([NotNull] mathParser.MINUTE_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitMIN_fun([NotNull] mathParser.MIN_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitMODE_fun([NotNull] mathParser.MODE_funContext context)
        {
            throw new NotImplementedException();
        }



        public Operand VisitMONTH_fun([NotNull] mathParser.MONTH_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitMROUND_fun([NotNull] mathParser.MROUND_funContext context)
        {
            throw new NotImplementedException();
        }


        public Operand VisitMULTINOMIAL_fun([NotNull] mathParser.MULTINOMIAL_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitNEGBINOMDIST_fun([NotNull] mathParser.NEGBINOMDIST_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitNETWORKDAYS_fun([NotNull] mathParser.NETWORKDAYS_funContext context)
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



        public Operand VisitNOW_fun([NotNull] mathParser.NOW_funContext context)
        {
            throw new NotImplementedException();
        }



        public Operand VisitODD_fun([NotNull] mathParser.ODD_funContext context)
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

        public Operand VisitPERMUT_fun([NotNull] mathParser.PERMUT_funContext context)
        {
            throw new NotImplementedException();
        }



        public Operand VisitPOISSON_fun([NotNull] mathParser.POISSON_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitPOWER_fun([NotNull] mathParser.POWER_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitPRODUCT_fun([NotNull] mathParser.PRODUCT_funContext context)
        {
            throw new NotImplementedException();
        }


        public Operand VisitPROPER_fun([NotNull] mathParser.PROPER_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitP_fun([NotNull] mathParser.P_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitQUARTILE_fun([NotNull] mathParser.QUARTILE_funContext context)
        {
            throw new NotImplementedException();
        }



        public Operand VisitRADIANS_fun([NotNull] mathParser.RADIANS_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitRANDBETWEEN_fun([NotNull] mathParser.RANDBETWEEN_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitRAND_fun([NotNull] mathParser.RAND_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitREGEXREPALCE_fun([NotNull] mathParser.REGEXREPALCE_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitREGEX_fun([NotNull] mathParser.REGEX_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitREMOVEBOTH_fun([NotNull] mathParser.REMOVEBOTH_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitREMOVEEND_fun([NotNull] mathParser.REMOVEEND_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitREMOVESTART_fun([NotNull] mathParser.REMOVESTART_funContext context)
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

        public Operand VisitROUNDDOWN_fun([NotNull] mathParser.ROUNDDOWN_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitROUNDUP_fun([NotNull] mathParser.ROUNDUP_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitROUND_fun([NotNull] mathParser.ROUND_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitRTRIM_fun([NotNull] mathParser.RTRIM_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitSEARCH_fun([NotNull] mathParser.SEARCH_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitSECOND_fun([NotNull] mathParser.SECOND_funContext context)
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


        public Operand VisitSINH_fun([NotNull] mathParser.SINH_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitSIN_fun([NotNull] mathParser.SIN_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitSMALL_fun([NotNull] mathParser.SMALL_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitSPLIT_fun([NotNull] mathParser.SPLIT_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitSQRTPI_fun([NotNull] mathParser.SQRTPI_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitSQRT_fun([NotNull] mathParser.SQRT_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitSTARTSWITH_fun([NotNull] mathParser.STARTSWITH_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitSTDEVP_fun([NotNull] mathParser.STDEVP_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitSTDEV_fun([NotNull] mathParser.STDEV_funContext context)
        {
            throw new NotImplementedException();
        }



        public Operand VisitSUBSTITUTE_fun([NotNull] mathParser.SUBSTITUTE_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitSUBSTRING_fun([NotNull] mathParser.SUBSTRING_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitSUMIF_fun([NotNull] mathParser.SUMIF_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitSUMSQ_fun([NotNull] mathParser.SUMSQ_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitSUM_fun([NotNull] mathParser.SUM_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitTANH_fun([NotNull] mathParser.TANH_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitTAN_fun([NotNull] mathParser.TAN_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitTDIST_fun([NotNull] mathParser.TDIST_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitTEXTTOBASE64URL_fun([NotNull] mathParser.TEXTTOBASE64URL_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitTEXTTOBASE64_fun([NotNull] mathParser.TEXTTOBASE64_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitTEXT_fun([NotNull] mathParser.TEXT_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitTIMEVALUE_fun([NotNull] mathParser.TIMEVALUE_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitTIME_fun([NotNull] mathParser.TIME_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitTINV_fun([NotNull] mathParser.TINV_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitTODAY_fun([NotNull] mathParser.TODAY_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitTOLOWER_fun([NotNull] mathParser.TOLOWER_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitTOUPPER_fun([NotNull] mathParser.TOUPPER_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitTRIMEND_fun([NotNull] mathParser.TRIMEND_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitTRIMSTART_fun([NotNull] mathParser.TRIMSTART_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitTRIM_fun([NotNull] mathParser.TRIM_funContext context)
        {
            throw new NotImplementedException();
        }



        public Operand VisitTRUNC_fun([NotNull] mathParser.TRUNC_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitTRYJSON_fun([NotNull] mathParser.TRYJSON_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitT_fun([NotNull] mathParser.T_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitUPPER_fun([NotNull] mathParser.UPPER_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitURLDECODE_fun([NotNull] mathParser.URLDECODE_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitURLENCODE_fun([NotNull] mathParser.URLENCODE_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitVALUE_fun([NotNull] mathParser.VALUE_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitVARP_fun([NotNull] mathParser.VARP_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitVAR_fun([NotNull] mathParser.VAR_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitWEEKDAY_fun([NotNull] mathParser.WEEKDAY_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitWEEKNUM_fun([NotNull] mathParser.WEEKNUM_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitWEIBULL_fun([NotNull] mathParser.WEIBULL_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitWORKDAY_fun([NotNull] mathParser.WORKDAY_funContext context)
        {
            throw new NotImplementedException();
        }

        public Operand VisitYEAR_fun([NotNull] mathParser.YEAR_funContext context)
        {
            throw new NotImplementedException();
        }


    }
}
