package toolgood.algorithm.internals;

import java.math.BigDecimal;
import java.net.URLDecoder;
import java.net.URLEncoder;
import java.text.DecimalFormat;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Random;
import java.util.UUID;
import java.util.function.Function;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import org.antlr.v4.runtime.tree.AbstractParseTreeVisitor;
import org.antlr.v4.runtime.tree.TerminalNode;
import org.apache.commons.text.StringEscapeUtils;
import org.joda.time.DateTime;
import org.joda.time.DateTimeConstants;
import org.joda.time.DateTimeZone;

import toolgood.algorithm.MyDate;
import toolgood.algorithm.Operand;
import toolgood.algorithm.OperandType;
import toolgood.algorithm.litJson.JsonData;
import toolgood.algorithm.litJson.JsonMapper;
import toolgood.algorithm.math.mathVisitor;
import toolgood.algorithm.math.mathParser2.*;
import toolgood.algorithm.mathNet.ExcelFunctions;

public class MathVisitor extends AbstractParseTreeVisitor<Operand> implements mathVisitor<Operand> {
    private static Pattern sumifRegex = Pattern.compile("(<|<=|>|>=|=|==|!=|<>) *([-+]?\\d+(\\.(\\d+)?)?)");
    private static Pattern bit_2 = Pattern.compile("^[01]+");
    private static Pattern bit_8 = Pattern.compile("^[0-8]+");
    private static Pattern bit_16 = Pattern.compile("^[0-9a-fA-F]+");
    private static Pattern clearRegex = Pattern.compile("[\\f\\n\\r\\t\\v]");
    private static Pattern numberRegex = Pattern.compile("^-?(0|[1-9])\\d*(\\.\\d+)?$");
    public Function<String, Operand> GetParameter;
    public Function<MyFunction, Operand> DiyFunction;
    public int excelIndex;

    public Operand visitProg(final ProgContext context) {
        return visitChildren(context);
    }

    public Operand visitMulDiv_fun(final MulDiv_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0);
        Operand secondValue = args.get(1);
        final String t = context.op.getText();
        if (firstValue.Type() == OperandType.STRING) {
            if (numberRegex.matcher(firstValue.TextValue()).find()) {
                final Operand a = firstValue.ToNumber(null);
                if (a.IsError() == false)
                    firstValue = a;
            } else {
                final Operand a = firstValue.ToDate(null);
                if (a.IsError() == false)
                    firstValue = a;
            }
        }
        if (secondValue.Type() == OperandType.STRING) {
            if (numberRegex.matcher(secondValue.TextValue()).find()) {
                final Operand a = secondValue.ToNumber(null);
                if (a.IsError() == false)
                    secondValue = a;
            } else {
                final Operand a = secondValue.ToDate(null);
                if (a.IsError() == false)
                    secondValue = a;
            }
        }
        if (t.equals("*")) {
            if (secondValue.Type() == OperandType.BOOLEAN) {
                if (secondValue.BooleanValue())
                    return firstValue;
                else
                    return Operand.Create(0);
            } else if (firstValue.Type() == OperandType.BOOLEAN) {
                if (firstValue.BooleanValue())
                    return secondValue;
                else
                    return Operand.Create(0);
            }
            if (firstValue.Type() == OperandType.DATE) {
                secondValue = secondValue.ToNumber("Function '" + t + "' parameter 2 is error!");
                if (secondValue.IsError()) {
                    return secondValue;
                }
                return Operand.Create(firstValue.DateValue().MUL(secondValue.NumberValue()));
            }
            if (secondValue.Type() == OperandType.DATE) {
                firstValue = firstValue.ToNumber("Function '" + t + "' parameter 1 is error!");
                if (firstValue.IsError()) {
                    return firstValue;
                }
                return Operand.Create(secondValue.DateValue().MUL(firstValue.NumberValue()));
            }

            firstValue = firstValue.ToNumber("Function '" + t + "' parameter 1 is error!");
            if (firstValue.IsError()) {
                return firstValue;
            }
            secondValue = secondValue.ToNumber("Function '" + t + "' parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            return Operand.Create(firstValue.NumberValue() * secondValue.NumberValue());
        } else if (t.equals("/")) {
            if (firstValue.Type() == OperandType.DATE) {
                return Operand.Create(firstValue.DateValue().DIV(secondValue.NumberValue()));
            }

            firstValue = firstValue.ToNumber("Function '" + t + "' parameter 1 is error!");
            if (firstValue.IsError()) {
                return firstValue;
            }
            secondValue = secondValue.ToNumber("Div fun right value");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (secondValue.NumberValue() == 0) {
                return Operand.Error("Function '" + t + "' parameter 2 is error!");
            }
            return Operand.Create(firstValue.NumberValue() / secondValue.NumberValue());
        } else if (t.equals("%")) {
            firstValue = firstValue.ToNumber("% fun right value");
            if (firstValue.IsError()) {
                return firstValue;
            }
            secondValue = secondValue.ToNumber("% fun right value");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (secondValue.NumberValue() == 0) {
                return Operand.Error("Div 0 is error!");
            }
            return Operand.Create(firstValue.NumberValue() % secondValue.NumberValue());
        }
        return Operand.Error("Function '" + t + "' parameter is error!");
    }

    public Operand visitAddSub_fun(final AddSub_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0);
        Operand secondValue = args.get(1);
        final String t = context.op.getText();

        if (t.equals("&")) {
            if (firstValue.IsNull() && secondValue.IsNull()) {
                return firstValue;
            } else if (firstValue.IsNull()) {
                secondValue = secondValue.ToText("Function '" + t + "' parameter 2 is error!");
                return secondValue;
            } else if (secondValue.IsNull()) {
                firstValue = firstValue.ToText("Function '" + t + "' parameter 1 is error!");
                return firstValue;
            }

            firstValue = firstValue.ToText("Function '" + t + "' parameter 1 is error!");
            if (firstValue.IsError()) {
                return firstValue;
            }
            secondValue = secondValue.ToText("Function '" + t + "' parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            return Operand.Create(firstValue.TextValue() + secondValue.TextValue());
        }
        if (firstValue.Type() == OperandType.STRING) {
            if (numberRegex.matcher(firstValue.TextValue()).find()) {
                final Operand a = firstValue.ToNumber(null);
                if (a.IsError() == false)
                    firstValue = a;
            } else {
                final Operand a = firstValue.ToDate(null);
                if (a.IsError() == false)
                    firstValue = a;
            }
        }
        if (secondValue.Type() == OperandType.STRING) {
            if (numberRegex.matcher(secondValue.TextValue()).find()) {
                final Operand a = secondValue.ToNumber(null);
                if (a.IsError() == false)
                    secondValue = a;
            } else {
                final Operand a = secondValue.ToDate(null);
                if (a.IsError() == false)
                    secondValue = a;
            }
        }
        if (t.equals("+")) {
            if (firstValue.Type() == OperandType.DATE && secondValue.Type() == OperandType.DATE) {
                return Operand.Create(firstValue.DateValue().ADD(secondValue.DateValue()));
            } else if (firstValue.Type() == OperandType.DATE) {
                secondValue = secondValue.ToNumber("Function '" + t + "' parameter 2 is error!");
                if (secondValue.IsError()) {
                    return secondValue;
                }
                return Operand.Create(firstValue.DateValue().ADD(secondValue.NumberValue()));
            } else if (secondValue.Type() == OperandType.DATE) {
                firstValue = firstValue.ToNumber("Function '" + t + "' parameter 1 is error!");
                if (firstValue.IsError()) {
                    return firstValue;
                }
                return Operand.Create(secondValue.DateValue().ADD(firstValue.NumberValue()));
            }
            firstValue = firstValue.ToNumber("Function '" + t + "' parameter 1 is error!");
            if (firstValue.IsError()) {
                return firstValue;
            }
            secondValue = secondValue.ToNumber("Function '" + t + "' parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            return Operand.Create(firstValue.NumberValue() + secondValue.NumberValue());
        } else if (t.equals("-")) {
            if (firstValue.Type() == OperandType.DATE && secondValue.Type() == OperandType.DATE) {
                return Operand.Create(firstValue.DateValue().SUB(secondValue.DateValue()));
            } else if (firstValue.Type() == OperandType.DATE) {
                secondValue = secondValue.ToNumber("Function '" + t + "' parameter 2 is error!");
                if (secondValue.IsError()) {
                    return secondValue;
                }
                return Operand.Create(firstValue.DateValue().SUB(secondValue.NumberValue()));
            } else if (secondValue.Type() == OperandType.DATE) {
                firstValue = firstValue.ToNumber("Function '" + t + "' parameter 1 is error!");
                if (firstValue.IsError()) {
                    return firstValue;
                }
                return Operand.Create(secondValue.DateValue().SUB(firstValue.NumberValue()));
            }
            firstValue = firstValue.ToNumber(null);
            if (firstValue.IsError()) {
                return firstValue;
            }
            secondValue = secondValue.ToNumber("Function '" + t + "' parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            return Operand.Create(firstValue.NumberValue() - secondValue.NumberValue());
        }
        return Operand.Error("Function '" + t + "' parameter is error!");
    }

    public Operand visitJudge_fun(final Judge_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0);
        Operand secondValue = args.get(1);
        final String type = context.op.getText();

        if (firstValue.IsNull()) {
            if (secondValue.IsNull() && (type.equals("==") || type.equals("="))) {
                return Operand.True;
            } else if (secondValue.IsNull() == false && (type.equals("<>")|| type.equals("!="))) {
                return Operand.True;
            }
            return Operand.False;
        } else if (secondValue.IsNull()) {
            if (type.equals("==")|| type.equals("=")) {
                return Operand.False;
            }
            return Operand.True;
        }

        int r;
        if (firstValue.Type() == secondValue.Type()) {
            if (firstValue.Type() == OperandType.STRING || firstValue.Type() == OperandType.JSON) {
                firstValue = firstValue.ToText("Function '" + type + "' parameter 1 is error!");
                if (firstValue.IsError()) {
                    return firstValue;
                }
                secondValue = secondValue.ToText("Function '" + type + "' parameter 2 is error!");
                if (secondValue.IsError()) {
                    return secondValue;
                }

                r = firstValue.TextValue().compareTo(secondValue.TextValue());
            } else if (firstValue.Type() == OperandType.ARRARY) {
                return Operand.Error("两个类型无法比较");
            } else {
                firstValue = firstValue.ToNumber("Function '" + type + "' parameter 1 is error!");
                if (firstValue.IsError()) {
                    return firstValue;
                }
                secondValue = secondValue.ToNumber("Function '" + type + "' parameter 2 is error!");
                if (secondValue.IsError()) {
                    return secondValue;
                }
                r = Compare(firstValue.NumberValue(), secondValue.NumberValue());
            }
        } else if ((firstValue.Type() == OperandType.DATE && secondValue.Type() == OperandType.STRING)
                || (secondValue.Type() == OperandType.DATE && firstValue.Type() == OperandType.STRING)
                || (firstValue.Type() == OperandType.NUMBER && secondValue.Type() == OperandType.STRING)
                || (secondValue.Type() == OperandType.NUMBER && firstValue.Type() == OperandType.STRING)) {
            firstValue = firstValue.ToText("Function '" + type + "' parameter 1 is error!");
            if (firstValue.IsError()) {
                return firstValue;
            }
            secondValue = secondValue.ToText("Function '" + type + "' parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }

            r = firstValue.TextValue().compareTo(secondValue.TextValue());
        } else if ((firstValue.Type() == OperandType.BOOLEAN && secondValue.Type() == OperandType.STRING)
                || (secondValue.Type() == OperandType.BOOLEAN && firstValue.Type() == OperandType.STRING)) {
            firstValue = firstValue.ToText("Function '" + type + "' parameter 1 is error!");
            if (firstValue.IsError()) {
                return firstValue;
            }
            secondValue = secondValue.ToText("Function '" + type + "' parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            r = firstValue.TextValue().compareToIgnoreCase(secondValue.TextValue());
            // r = String.Compare(firstValue.TextValue(), secondValue.TextValue(), true);
        } else if (firstValue.Type() == OperandType.STRING || secondValue.Type() == OperandType.STRING
                || firstValue.Type() == OperandType.JSON || secondValue.Type() == OperandType.JSON
                || firstValue.Type() == OperandType.ARRARY || secondValue.Type() == OperandType.ARRARY) {
            return Operand.Error("两个类型无法比较");
        } else {
            firstValue = firstValue.ToNumber("Function '" + type + "' parameter 1 is error!");
            if (firstValue.IsError()) {
                return firstValue;
            }
            secondValue = secondValue.ToNumber("Function '" + type + "' parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }

            r = Compare(firstValue.NumberValue(), secondValue.NumberValue());
        }
        if (type.equals("<")) {
            return Operand.Create(r == -1);
        } else if (type.equals("<=")) {
            return Operand.Create(r <= 0);
        } else if (type.equals(">")) {
            return Operand.Create(r == 1);
        } else if (type.equals(">=")) {
            return Operand.Create(r >= 0);
        } else if (type.equals("=") || type.equals("==")) {
            return Operand.Create(r == 0);
        }
        return Operand.Create(r != 0);
    }

    private int Compare(final double t1, final double t2) {

        final double b = round(t1 - t2, 12);
        if (b == 0) {
            return 0;
        } else if (b > 0) {
            return 1;
        }
        return -1;
    }

    public Operand visitAndOr_fun(final AndOr_funContext context) {
        final String t = context.op.getText();
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToBoolean("Function '" + t + "' parameter " + (index++) + " is error!");
            ;
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0);
        final Operand secondValue = args.get(1);
        if (t.equals("&&") || t.toLowerCase().equals("and")) {
            if (firstValue.BooleanValue() && secondValue.BooleanValue()) {
                return Operand.True;
            }
            return Operand.False;
        }
        if (firstValue.BooleanValue() || secondValue.BooleanValue()) {
            return Operand.True;
        }
        return Operand.False;
    }

    public Operand visitIF_fun(final IF_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand b = args.get(0).ToBoolean("Function IF first parameter is error!");
        if (b.IsError()) {
            return b;
        }

        if (b.BooleanValue()) {
            if (args.size() > 1) {
                return args.get(1);
            }
            return Operand.True;
        }
        if (args.size() == 3) {
            return args.get(2);
        }
        return Operand.False;
    }

    public Operand visitIFERROR_fun(final IFERROR_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand a = visit(item);
            args.add(a);
        }

        if (args.get(0).IsError()) {
            return args.get(1);
        }
        if (args.size() == 3) {
            return args.get(2);
        }
        return Operand.False;
    }

    public Operand visitISNUMBER_fun(final ISNUMBER_funContext context) {
        final Operand firstValue = visit(context.expr());
        if (firstValue.IsError()) {
            return firstValue;
        }

        if (firstValue.Type() == OperandType.NUMBER) {
            return Operand.True;
        }
        return Operand.False;
    }

    public Operand visitISTEXT_fun(final ISTEXT_funContext context) {
        final Operand firstValue = visit(context.expr());
        if (firstValue.IsError()) {
            return firstValue;
        }

        if (firstValue.Type() == OperandType.STRING) {
            return Operand.True;
        }
        return Operand.False;
    }

    public Operand visitISERROR_fun(final ISERROR_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            args.add(aa);
        }
        if (args.size() == 2) {
            if (args.get(0).IsError()) {
                return args.get(1);
            }
            return args.get(0);
        }

        if (args.get(0).IsError()) {
            return Operand.True;
        }
        return Operand.False;
    }

    public Operand visitISNULL_fun(final ISNULL_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            args.add(aa);
        }

        if (args.size() == 2) {
            if (args.get(0).IsNull()) {
                return args.get(1);
            }
            return args.get(0);
        }
        if (args.get(0).IsNull()) {
            return Operand.True;
        }
        return Operand.False;
    }

    public Operand visitISNULLORERROR_fun(final ISNULLORERROR_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            args.add(aa);
        }

        if (args.size() == 2) {
            if (args.get(0).IsNull() || args.get(0).IsError()) {
                return args.get(1);
            }
            return args.get(0);
        }
        if (args.get(0).IsNull() || args.get(0).IsError()) {
            return Operand.True;
        }
        return Operand.False;
    }

    public Operand visitISEVEN_fun(final ISEVEN_funContext context) {
        final Operand firstValue = visit(context.expr());
        if (firstValue.Type() == OperandType.NUMBER) {
            if (firstValue.IntValue() % 2 == 0) {
                return Operand.True;
            }
        }
        return Operand.False;
    }

    public Operand visitISLOGICAL_fun(final ISLOGICAL_funContext context) {
        final Operand firstValue = visit(context.expr());
        if (firstValue.Type() == OperandType.BOOLEAN) {
            return Operand.True;
        }
        return Operand.False;
    }

    public Operand visitISODD_fun(final ISODD_funContext context) {
        final Operand firstValue = visit(context.expr());
        if (firstValue.Type() == OperandType.NUMBER) {
            if (firstValue.IntValue() % 2 == 1) {
                return Operand.True;
            }
        }
        return Operand.False;
    }

    public Operand visitISNONTEXT_fun(final ISNONTEXT_funContext context) {
        final Operand firstValue = visit(context.expr());
        if (firstValue.Type() != OperandType.STRING) {
            return Operand.True;
        }
        return Operand.False;
    }

    public Operand visitAND_fun(final AND_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand a = visit(item).ToBoolean("Function AND parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        boolean b = true;
        for (final Operand item : args) {
            if (item.BooleanValue() == false) {
                b = false;
                break;
            }
        }
        return Operand.Create(b);
    }

    public Operand visitOR_fun(final OR_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand a = visit(item).ToBoolean("Function OR parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        boolean b = false;
        for (final Operand item : args) {
            if (item.BooleanValue() == true) {
                b = true;
                break;
            }
        }
        return Operand.Create(b);
    }

    public Operand visitNOT_fun(final NOT_funContext context) {
        final Operand firstValue = visit(context.expr()).ToBoolean("Function NOT parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(!firstValue.BooleanValue());
    }

    public Operand visitTRUE_fun(final TRUE_funContext context) {
        return Operand.True;
    }

    public Operand visitFALSE_fun(final FALSE_funContext context) {
        return Operand.False;
    }

    public Operand visitE_fun(final E_funContext context) {
        return Operand.Create(Math.E);
    }

    public Operand visitPI_fun(final PI_funContext context) {
        return Operand.Create(Math.PI);
    }

    public Operand visitABS_fun(final ABS_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function ABS parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(Math.abs(firstValue.NumberValue()));
    }

    public Operand visitQUOTIENT_fun(final QUOTIENT_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function QUOTIENT parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0);
        final Operand secondValue = args.get(1);

        if (secondValue.NumberValue() == 0) {
            return Operand.Error("Function QUOTIENT div 0 error!");
        }
        return Operand.Create((double) (int) (firstValue.NumberValue() / secondValue.NumberValue()));
    }

    public Operand visitMOD_fun(final MOD_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function MOD parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0);
        final Operand secondValue = args.get(1);

        if (secondValue.NumberValue() == 0) {
            return Operand.Error("Function MOD div 0 error!");
        }
        return Operand.Create((int) (firstValue.NumberValue() % secondValue.NumberValue()));

    }

    public Operand visitSIGN_fun(final SIGN_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function SIGN parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(sign(firstValue.NumberValue()));
    }

    public Operand visitSQRT_fun(final SQRT_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function SQRT parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(Math.sqrt(firstValue.NumberValue()));
    }

    public Operand visitTRUNC_fun(final TRUNC_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function TRUNC parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create((int) (firstValue.NumberValue()));
    }

    public Operand visitINT_fun(final INT_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function INT parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create((int) (firstValue.NumberValue()));
    }

    public Operand visitGCD_fun(final GCD_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final List<Double> list = new ArrayList<Double>();
        final boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function GCD parameter is error!");
        }

        return Operand.Create(F_base_gcd(list));
    }

    public Operand visitLCM_fun(final LCM_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final List<Double> list = new ArrayList<Double>();
        final boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function GCD parameter is error!");
        }

        return Operand.Create(F_base_lgm(list));
    }

    public Operand visitCOMBIN_fun(final COMBIN_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function COMBIN parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0);
        final Operand secondValue = args.get(1);

        final int total = firstValue.IntValue();
        final int count = secondValue.IntValue();
        double sum = 1;
        double sum2 = 1;
        for (int i = 0; i < count; i++) {
            sum *= (total - i);
            sum2 *= (i + 1);
        }
        return Operand.Create(sum / sum2);
    }

    public Operand visitPERMUT_fun(final PERMUT_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function PERMUT parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0);
        final Operand secondValue = args.get(1);

        final int total = firstValue.IntValue();
        final int count = secondValue.IntValue();

        double sum = 1;
        for (int i = 0; i < count; i++) {
            sum *= (total - i);
        }
        return Operand.Create(sum);
    }

    private int F_base_gcd(List<Double> list) {
        list = ShellSort(list);
        // list = list.OrderBy(q => q).ToList();
        int g = F_base_gcd((int) (double) list.get(1), (int) (double) list.get(0));
        for (int i = 2; i < list.size(); i++) {
            g = F_base_gcd((int) (double) list.get(i), g);
        }
        return g;
    }

    private int F_base_gcd(final int a, final int b) {
        if (b == 1) {
            return 1;
        }
        if (b == 0) {
            return a;
        }
        return F_base_gcd(b, a % b);
    }

    private int F_base_lgm(List<Double> list) {
        for (int i = list.size() - 1; i >= 0; i--) {
            final int item = (int) (double) list.get(i);// [i];
            if (item <= 1) {
                list.remove(i);
            }
        }
        list = ShellSort(list);

        int a = (int) (double) list.get(0);// [0];
        for (int i = 1; i < list.size(); i++) {
            final int b = (int) (double) list.get(i);
            final int g = b > a ? F_base_gcd(b, a) : F_base_gcd(a, b);
            a = a / g * b;
        }
        return a;
    }

    private List<Double> ShellSort(final List<Double> array) {
        final int len = array.size();
        double temp;
        int gap = len / 2;
        while (gap > 0) {
            for (int i = gap; i < len; i++) {
                temp = array.get(i);
                int preIndex = i - gap;
                while (preIndex >= 0 && array.get(preIndex) > temp) {
                    array.set(preIndex + gap, array.get(preIndex));
                    // array[preIndex + gap] = array[preIndex];
                    preIndex -= gap;
                }
                array.set(preIndex + gap, temp);
                // array[preIndex + gap] = temp;
            }
            gap /= 2;
        }
        return array;
    }

    public Operand visitDEGREES_fun(final DEGREES_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function DEGREES parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        final double z = firstValue.NumberValue();
        final double r = (z / Math.PI * 180);
        return Operand.Create(r);
    }

    public Operand visitRADIANS_fun(final RADIANS_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function RADIANS parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        final double r = firstValue.NumberValue() / 180 * Math.PI;
        return Operand.Create(r);
    }

    public Operand visitCOS_fun(final COS_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function COS parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(Math.cos(firstValue.NumberValue()));
    }

    public Operand visitCOSH_fun(final COSH_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function COSH parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(Math.cosh(firstValue.NumberValue()));
    }

    public Operand visitSIN_fun(final SIN_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function SIN parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(Math.sin(firstValue.NumberValue()));
    }

    public Operand visitSINH_fun(final SINH_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function SINH parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(Math.sinh(firstValue.NumberValue()));
    }

    public Operand visitTAN_fun(final TAN_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function TAN parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(Math.tan(firstValue.NumberValue()));
    }

    public Operand visitTANH_fun(final TANH_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function TANH parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(Math.tanh(firstValue.NumberValue()));
    }

    public Operand visitACOS_fun(final ACOS_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function ACOS parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final double x = firstValue.NumberValue();
        if (x < -1 && x > 1) {
            return Operand.Error("Function ACOS parameter is error!");
        }
        return Operand.Create(Math.acos(x));
    }

    public Operand visitACOSH_fun(final ACOSH_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function ACOSH parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        final double z = firstValue.NumberValue();
        if (z < 1) {
            return Operand.Error("Function ACOSH parameter is error!");
        }
        final double r = Math.log(z + Math.pow(z * z - 1, 0.5));
        return Operand.Create(r);
    }

    public Operand visitASIN_fun(final ASIN_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function ASIN parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final double x = firstValue.NumberValue();
        if (x < -1 && x > 1) {
            return Operand.Error("Function ASIN parameter is error!");
        }
        return Operand.Create(Math.asin(x));
    }

    public Operand visitASINH_fun(final ASINH_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function ASINH parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final double x = firstValue.NumberValue();
        final double d = Math.log(x + Math.sqrt(x * x + 1));
        return Operand.Create(d);
    }

    public Operand visitATAN_fun(final ATAN_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function ATAN parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(Math.atan(firstValue.NumberValue()));
    }

    public Operand visitATANH_fun(final ATANH_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function ATANH parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final double x = firstValue.NumberValue();
        if (x >= 1 || x <= -1) {
            return Operand.Error("Function ATANH parameter is error!");
        }
        final double d = Math.log((1 + x) / (1 - x)) / 2;
        return Operand.Create(d);
    }

    public Operand visitATAN2_fun(final ATAN2_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function ATAN2 parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0);
        final Operand secondValue = args.get(1);

        return Operand.Create(Math.atan2(secondValue.NumberValue(), firstValue.NumberValue()));
    }

    public Operand visitFIXED_fun(final FIXED_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        int num = 2;
        if (args.size() > 1) {
            final Operand secondValue = args.get(1).ToNumber("Function FIXED parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            num = secondValue.IntValue();
        }
        final Operand firstValue = args.get(0).ToNumber("Function FIXED parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        final double s = round(firstValue.NumberValue(), num);
        boolean no = false;
        if (args.size() == 3) {
            final Operand thirdValue = args.get(2).ToBoolean("Function FIXED parameter 3 is error!");
            if (thirdValue.IsError()) {
                return thirdValue;
            }
            no = thirdValue.BooleanValue();
        }
            if (num <= 0) {
                String f = no? "#":"#,###";
                final DecimalFormat myFormatter = new DecimalFormat(f);
                return Operand.Create(myFormatter.format(s));
            } else {
                String f = no? "#.":"#,###.";
                for (int i = 0; i < num; i++) {
                    f += "#";
                }
                final DecimalFormat myFormatter = new DecimalFormat(f);
                return Operand.Create(myFormatter.format(s));
            }
        // return Operand.Create(((Double)s).toString());
    }

    public Operand visitBIN2OCT_fun(final BIN2OCT_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToText("Function BIN2OCT parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        if (bit_2.matcher(firstValue.TextValue()).find() == false) {
            return Operand.Error("Function BIN2OCT parameter 1 is error!");
        }
        final String num = Integer.toOctalString(Integer.parseUnsignedInt(firstValue.TextValue(), 2));
        // String num = Convert.toString(Convert.ToInt32(firstValue.TextValue(), 2), 8);
        if (args.size() == 2) {
            final Operand secondValue = args.get(1).ToNumber("Function BIN2OCT parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (num.length() > secondValue.IntValue()) {
                return Operand.Create(padLeft(num, secondValue.IntValue(), '0'));
            }
            return Operand.Error("Function BIN2OCT parameter 2 is error!");
        }
        return Operand.Create(num);
    }

    public Operand visitBIN2DEC_fun(final BIN2DEC_funContext context) {
        final Operand firstValue = visit(context.expr()).ToText("Function BIN2DEC parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        if (bit_2.matcher(firstValue.TextValue()).find() == false) {
            return Operand.Error("Function BIN2DEC parameter is error!");
        }
        final int num = Integer.parseUnsignedInt(firstValue.TextValue(), 2);
        // String num = Convert.ToInt32(firstValue.TextValue(), 2);
        return Operand.Create(num);
    }

    public Operand visitBIN2HEX_fun(final BIN2HEX_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToText("Function BIN2HEX parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        if (bit_2.matcher(firstValue.TextValue()).find() == false) {
            return Operand.Error("Function BIN2HEX parameter 1 is error!");
        }
        final String num = Integer.toHexString(Integer.parseUnsignedInt(firstValue.TextValue(), 2)).toUpperCase();
        // String num = Convert.toString(Convert.ToInt32(firstValue.TextValue(), 2),
        // 16).ToUpper();
        if (args.size() == 2) {
            final Operand secondValue = args.get(1).ToNumber("Function BIN2HEX parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (num.length() > secondValue.IntValue()) {
                return Operand.Create(padLeft(num, secondValue.IntValue(), '0'));
            }
            return Operand.Error("Function BIN2HEX parameter 2 is error!");
        }
        return Operand.Create(num);
    }

    public Operand visitOCT2BIN_fun(final OCT2BIN_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToText("Function OCT2BIN parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        if (bit_8.matcher(firstValue.TextValue()).find() == false) {
            return Operand.Error("Function OCT2BIN parameter 1 is error!");
        }
        final String num = Integer.toBinaryString(Integer.parseUnsignedInt(firstValue.TextValue(), 8));
        // String num = Convert.toString(Convert.ToInt32(firstValue.TextValue(), 8), 2);
        if (args.size() == 2) {
            final Operand secondValue = args.get(1).ToNumber("Function OCT2BIN parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (num.length() > secondValue.IntValue()) {
                return Operand.Create(padLeft(num, secondValue.IntValue(), '0'));
            }
            return Operand.Error("Function OCT2BIN parameter 2 is error!");
        }
        return Operand.Create(num);
    }

    public Operand visitOCT2DEC_fun(final OCT2DEC_funContext context) {
        final Operand firstValue = visit(context.expr()).ToText("Function OCT2DEC parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        if (bit_8.matcher(firstValue.TextValue()).find() == false) {
            return Operand.Error("Function OCT2DEC parameter is error!");
        }
        final int num = Integer.parseUnsignedInt(firstValue.TextValue(), 8);
        // String num = Convert.ToInt32(firstValue.TextValue(), 8);
        return Operand.Create(num);
    }

    public Operand visitOCT2HEX_fun(final OCT2HEX_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToText("Function OCT2HEX parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        if (bit_8.matcher(firstValue.TextValue()).find() == false) {
            return Operand.Error("Function OCT2HEX parameter 1 is error!");
        }
        final String num = Integer.toHexString(Integer.parseUnsignedInt(firstValue.TextValue(), 8)).toUpperCase();
        if (args.size() == 2) {
            final Operand secondValue = args.get(1).ToNumber("Function OCT2HEX parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (num.length() > secondValue.IntValue()) {
                return Operand.Create(padLeft(num, secondValue.IntValue(), '0'));
            }
            return Operand.Error("Function OCT2HEX parameter 2 is error!");
        }
        return Operand.Create(num);
    }

    public Operand visitDEC2BIN_fun(final DEC2BIN_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToNumber("Function DEC2BIN parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final String num = Integer.toBinaryString(firstValue.IntValue());
        // String num = System.Convert.toString(firstValue.IntValue(), 2);
        if (args.size() == 2) {
            final Operand secondValue = args.get(1).ToNumber("Function DEC2BIN parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (num.length() > secondValue.IntValue()) {
                return Operand.Create(padLeft(num, secondValue.IntValue(), '0'));
            }
            return Operand.Error("Function DEC2BIN parameter 2 is error!");
        }
        return Operand.Create(num);
    }

    public Operand visitDEC2OCT_fun(final DEC2OCT_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToNumber("Function DEC2OCT parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final String num = Integer.toOctalString(firstValue.IntValue());
        if (args.size() == 2) {
            final Operand secondValue = args.get(1).ToNumber("Function DEC2OCT parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (num.length() > secondValue.IntValue()) {
                return Operand.Create(padLeft(num, secondValue.IntValue(), '0'));
            }
            return Operand.Error("Function DEC2OCT parameter 2 is error!");
        }
        return Operand.Create(num);
    }

    public Operand visitDEC2HEX_fun(final DEC2HEX_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToNumber("Function DEC2HEX parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final String num = Integer.toHexString(firstValue.IntValue()).toUpperCase();
        // String num = System.Convert.toString(firstValue.IntValue(), 16).ToUpper();
        if (args.size() == 2) {
            final Operand secondValue = args.get(1).ToNumber("Function DEC2HEX parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (num.length() > secondValue.IntValue()) {
                return Operand.Create(padLeft(num, secondValue.IntValue(), '0'));
            }
            return Operand.Error("Function DEC2HEX parameter 2 is error!");
        }
        return Operand.Create(num);
    }

    public Operand visitHEX2BIN_fun(final HEX2BIN_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToText("Function HEX2BIN parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        if (bit_16.matcher(firstValue.TextValue()).find() == false) {
            return Operand.Error("Function HEX2BIN parameter 1 is error!");
        }
        final String num = Integer.toBinaryString(Integer.parseUnsignedInt(firstValue.TextValue(), 16));
        // String num = Convert.toString(Convert.ToInt32(firstValue.TextValue(), 16),
        // 2);
        if (args.size() == 2) {
            final Operand secondValue = args.get(1).ToNumber("Function HEX2BIN parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (num.length() > secondValue.IntValue()) {
                return Operand.Create(padLeft(num, secondValue.IntValue(), '0'));
            }
            return Operand.Error("Function HEX2BIN parameter 2 is error!");
        }
        return Operand.Create(num);
    }

    public Operand visitHEX2OCT_fun(final HEX2OCT_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToText("Function HEX2OCT parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        if (bit_16.matcher(firstValue.TextValue()).find() == false) {
            return Operand.Error("Function HEX2OCT parameter 1 is error!");
        }
        final String num = Integer.toOctalString(Integer.parseUnsignedInt(firstValue.TextValue(), 16));
        // String num = Convert.toString(Convert.ToInt32(firstValue.TextValue(), 16),
        // 8);
        if (args.size() == 2) {
            final Operand secondValue = args.get(1).ToNumber("Function HEX2OCT parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (num.length() > secondValue.IntValue()) {
                return Operand.Create(padLeft(num, secondValue.IntValue(), '0'));
            }
            return Operand.Error("Function HEX2OCT parameter 2 is error!");
        }
        return Operand.Create(num);
    }

    public Operand visitHEX2DEC_fun(final HEX2DEC_funContext context) {
        final Operand firstValue = visit(context.expr()).ToText("Function HEX2DEC parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        if (bit_16.matcher(firstValue.TextValue()).find() == false) {
            return Operand.Error("Function HEX2DEC parameter is error!");
        }
        final int num = Integer.parseUnsignedInt(firstValue.TextValue(), 16);
        // String num = Convert.ToInt32(firstValue.TextValue(), 16);
        return Operand.Create(num);
    }

    public Operand visitROUND_fun(final ROUND_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function ROUND parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0);
        final Operand secondValue = args.get(1);

        return Operand.Create((double) round((double) firstValue.NumberValue(), secondValue.IntValue()));
    }

    public Operand visitROUNDDOWN_fun(final ROUNDDOWN_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function ROUNDDOWN parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0);
        final Operand secondValue = args.get(1);
        if (firstValue.NumberValue() == 0.0) {
            return firstValue;
        }
        final double a = Math.pow(10, secondValue.IntValue());
        double b = firstValue.NumberValue();

        b = ((double) (int) (b * a)) / a;
        return Operand.Create(b);
    }

    public Operand visitROUNDUP_fun(final ROUNDUP_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function ROUNDUP parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0);
        final Operand secondValue = args.get(1);
        if (firstValue.NumberValue() == 0.0) {
            return firstValue;
        }
        final double a = Math.pow(10, secondValue.IntValue());
        final double b = firstValue.NumberValue();

        final double t = (Math.ceil(Math.abs(b) * a)) / a;
        if (b > 0)
            return Operand.Create(t);
        return Operand.Create(-t);
    }

    public Operand visitCEILING_fun(final CEILING_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function CEILING parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0);
        if (args.size() == 1)
            return Operand.Create(Math.ceil(firstValue.NumberValue()));

        final Operand secondValue = args.get(1);
        final double b = secondValue.NumberValue();
        if (b == 0) {
            return Operand.Create(0);
        }
        if (b < 0) {
            return Operand.Error("Function CEILING parameter 2 is error!");
        }
        final double a = firstValue.NumberValue();
        final double d = Math.ceil(a / b) * b;
        return Operand.Create(d);
    }

    public Operand visitFLOOR_fun(final FLOOR_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function FLOOR parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0);
        if (args.size() == 1)
            return Operand.Create(Math.floor(firstValue.NumberValue()));

        final Operand secondValue = args.get(1);
        final double b = secondValue.NumberValue();
        if (b >= 1) {
            return Operand.Create(firstValue.IntValue());
        }
        if (b <= 0) {
            return Operand.Error("Function FLOOR parameter 2 is error!");
        }
        final double a = firstValue.NumberValue();
        final double d = Math.floor(a / b) * b;
        return Operand.Create(d);
    }

    public Operand visitEVEN_fun(final EVEN_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function EVEN parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        double z = firstValue.NumberValue();
        if (z % 2 == 0) {
            return firstValue;
        }
        z = Math.ceil(z);
        if (z % 2 == 0) {
            return Operand.Create(z);
        }
        z = z + 1;
        return Operand.Create(z);
    }

    public Operand visitODD_fun(final ODD_funContext context) {

        final Operand firstValue = visit(context.expr()).ToNumber("Function ODD parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        double z = firstValue.NumberValue();
        if (z % 2 == 1) {
            return firstValue;
        }
        z = Math.ceil(z);
        if (z % 2 == 1) {
            return Operand.Create(z);
        }
        z = z + 1;
        return Operand.Create(z);
    }

    public Operand visitMROUND_fun(final MROUND_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function MROUND parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0);
        final Operand secondValue = args.get(1);

        final double a = secondValue.NumberValue();
        if (a <= 0) {
            return Operand.Error("Function MROUND parameter 2 is error!");
        }
        final double b = firstValue.NumberValue();
        final double r = round(b / a, 0) * a;
        return Operand.Create(r);
    }

    public Operand visitRAND_fun(final RAND_funContext context) {
        final long tick = DateTime.now().getMillis();
        final Random rand = new Random((int) (tick & 0xffffffffL) | (int) (tick >> 32));
        return Operand.Create(rand.nextDouble());
    }

    public Operand visitRANDBETWEEN_fun(final RANDBETWEEN_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function RANDBETWEEN parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0);
        final Operand secondValue = args.get(1);

        final long tick = DateTime.now().getMillis();
        final Random rand = new Random((int) (tick & 0xffffffffL) | (int) (tick >> 32));
        return Operand.Create(
                rand.nextDouble() * (secondValue.NumberValue() - firstValue.NumberValue()) + firstValue.NumberValue());
    }

    public Operand visitFACT_fun(final FACT_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function FACT parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        final int z = firstValue.IntValue();
        if (z < 0) {
            return Operand.Error("Function FACT parameter is error!");
        }
        double d = 1;
        for (int i = 1; i <= z; i++) {
            d *= i;
        }
        return Operand.Create(d);
    }

    public Operand visitFACTDOUBLE_fun(final FACTDOUBLE_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function FACTDOUBLE parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        final int z = firstValue.IntValue();
        if (z < 0) {
            return Operand.Error("Function FACTDOUBLE parameter is error!");
        }
        double d = 1;
        for (int i = z; i > 0; i -= 2) {
            d *= i;
        }
        return Operand.Create(d);
    }

    public Operand visitPOWER_fun(final POWER_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function POWER parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0);
        final Operand secondValue = args.get(1);

        return Operand.Create(Math.pow(firstValue.NumberValue(), secondValue.NumberValue()));
    }

    public Operand visitEXP_fun(final EXP_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function EXP parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(Math.exp(firstValue.NumberValue()));
    }

    public Operand visitLN_fun(final LN_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function LN parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(Math.log(firstValue.NumberValue()));
    }

    public Operand visitLOG_fun(final LOG_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand a = visit(item).ToNumber("Function LOG parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        if (args.size() > 1) {
            return Operand.Create(log(args.get(0).NumberValue(), args.get(1).NumberValue()));
        }
        return Operand.Create(Math.log10(args.get(0).NumberValue()));
    }

    public Operand visitLOG10_fun(final LOG10_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function LOG10 parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(Math.log10(firstValue.NumberValue()));
    }

    public Operand visitMULTINOMIAL_fun(final MULTINOMIAL_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final List<Double> list = new ArrayList<Double>();
        final boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function MULTINOMIAL parameter is error!");
        }

        int sum = 0;
        int n = 1;
        for (final Double a : list) {
            n *= F_base_Factorial((int) (double) a);
            sum += (int) (double) a;
        }
        final int r = F_base_Factorial(sum) / n;
        return Operand.Create(r);
    }

    public Operand visitPRODUCT_fun(final PRODUCT_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final List<Double> list = new ArrayList<Double>();
        final boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function PRODUCT parameter is error!");
        }

        double d = 1;
        for (final double a : list) {
            d *= a;
        }

        return Operand.Create(d);
    }

    public Operand visitSQRTPI_fun(final SQRTPI_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function SQRTPI parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(Math.sqrt(firstValue.NumberValue() * Math.PI));
    }

    public Operand visitSUMSQ_fun(final SUMSQ_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final List<Double> list = new ArrayList<Double>();
        final boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function SUMSQ parameter is error!");
        }

        double d = 0;
        for (final double a : list) {
            d += a * a;
        }

        return Operand.Create(d);
    }

    private int F_base_Factorial(final int a) {
        if (a == 0) {
            return 1;
        }
        int r = 1;
        for (int i = a; i > 0; i--) {
            r *= i;
        }
        return r;
    }

    public Operand visitASC_fun(final ASC_funContext context) {
        final Operand firstValue = visit(context.expr()).ToText("Function ASC parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(F_base_ToDBC(firstValue.TextValue()));
    }

    public Operand visitJIS_fun(final JIS_funContext context) {
        final Operand firstValue = visit(context.expr()).ToText("Function JIS parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(F_base_ToSBC(firstValue.TextValue()));
    }

    public Operand visitCHAR_fun(final CHAR_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function CHAR parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        final char c = (char) (int) firstValue.NumberValue();
        return Operand.Create(((Character)c).toString());
    }

    public Operand visitCLEAN_fun(final CLEAN_funContext context) {
        final Operand firstValue = visit(context.expr()).ToText("Function CLEAN parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        String t = firstValue.TextValue();
        t = clearRegex.matcher(t).replaceAll("");
        return Operand.Create(t);
    }

    public Operand visitCODE_fun(final CODE_funContext context) {
        final Operand firstValue = visit(context.expr()).ToText("Function CODE parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        final String t = firstValue.TextValue();
        if (t.length() == 0) {
            return Operand.Error("Function CODE parameter is error!");
        }
        return Operand.Create((double) (int) (char) t.charAt(0));
    }

    public Operand visitCONCATENATE_fun(final CONCATENATE_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand a = visit(item).ToText("Function CONCATENATE parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        final StringBuilder sb = new StringBuilder();
        for (final Operand item : args) {
            sb.append(item.TextValue());
        }
        return Operand.Create(sb.toString());
    }

    public Operand visitEXACT_fun(final EXACT_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function EXACT parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0);
        final Operand secondValue = args.get(1);

        return Operand.Create(firstValue.TextValue() == secondValue.TextValue());
    }

    public Operand visitFIND_fun(final FIND_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToText("Function FIND parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final Operand secondValue = args.get(1).ToText("Function FIND parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        if (args.size() == 2) {
            final int p = secondValue.TextValue().indexOf(firstValue.TextValue()) + excelIndex;
            return Operand.Create(p);
        }
        final Operand count = args.get(2).ToNumber("Function FIND parameter 3 is error!");
        if (count.IsError()) {
            return count;
        }
        final int p2 = secondValue.TextValue().indexOf(firstValue.TextValue(), count.IntValue()) + excelIndex;
        return Operand.Create(p2);
    }

    public Operand visitLEFT_fun(final LEFT_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToText("Function LEFT parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        if (args.size() == 1) {
            return Operand.Create(((Character)firstValue.TextValue().charAt(0)).toString());
        }
        final Operand secondValue = args.get(1).ToNumber("Function LEFT parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }
        return Operand.Create(firstValue.TextValue().substring(0, secondValue.IntValue()));
    }

    public Operand visitLEN_fun(final LEN_funContext context) {
        final Operand firstValue = visit(context.expr()).ToText("Function LEN parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(firstValue.TextValue().length());
    }

    public Operand visitLOWER_fun(final LOWER_funContext context) {
        final Operand firstValue = visit(context.expr()).ToText("Function LOWER/TOLOWER parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(firstValue.TextValue().toLowerCase());
    }

    public Operand visitMID_fun(final MID_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToText("Function MID parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final Operand secondValue = args.get(1).ToNumber("Function MID parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }
        final Operand thirdValue = args.get(2).ToNumber("Function MID parameter 3 is error!");
        if (thirdValue.IsError()) {
            return thirdValue;
        }
        return Operand
                .Create(firstValue.TextValue().substring(secondValue.IntValue() - excelIndex,
                    secondValue.IntValue() - excelIndex + thirdValue.IntValue()));
    }

    public Operand visitPROPER_fun(final PROPER_funContext context) {
        final Operand firstValue = visit(context.expr()).ToText("Function PROPER parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        final String text = firstValue.TextValue();
        final StringBuilder sb = new StringBuilder(text);
        boolean isFirst = true;
        for (int i = 0; i < text.length(); i++) {
            final char t = text.charAt(i);
            if (t == ' ' || t == '\r' || t == '\n' || t == '\t' || t == '.') {
                isFirst = true;
            } else if (isFirst) {
                sb.setCharAt(i, Character.toUpperCase(t));
                isFirst = false;
            }
        }
        return Operand.Create(sb.toString());
    }

    public Operand visitREPLACE_fun(final REPLACE_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToText("Function REPLACE parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final String oldtext = firstValue.TextValue();
        if (args.size() == 3) {
            final Operand secondValue2 = args.get(1).ToText("Function REPLACE parameter 2 is error!");
            final Operand thirdValue2 = args.get(2).ToText("Function REPLACE parameter 3 is error!");
            final String old = secondValue2.TextValue();
            final String newstr = thirdValue2.TextValue();
            return Operand.Create(oldtext.replace(old, newstr));
        }

        final Operand secondValue = args.get(1).ToNumber("Function REPLACE parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }
        final Operand thirdValue = args.get(2).ToNumber("Function REPLACE parameter 3 is error!");
        if (thirdValue.IsError()) {
            return thirdValue;
        }
        final Operand fourthValue = args.get(3).ToText("Function REPLACE parameter 3 is error!");
        if (fourthValue.IsError()) {
            return fourthValue;
        }

        final int start = secondValue.IntValue() - excelIndex;
        final int length = thirdValue.IntValue();
        final String newtext = fourthValue.TextValue();

        final StringBuilder sb = new StringBuilder();
        for (int i = 0; i < oldtext.length(); i++) {
            if (i < start) {
                sb.append(oldtext.charAt(i));
            } else if (i == start) {
                sb.append(newtext);
            } else if (i >= start + length) {
                sb.append(oldtext.charAt(i));
            }
        }
        return Operand.Create(sb.toString());
    }

    public Operand visitREPT_fun(final REPT_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToText("Function REPT parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final Operand secondValue = args.get(1).ToNumber("Function REPT parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        final String newtext = firstValue.TextValue();
        final int length = secondValue.IntValue();
        final StringBuilder sb = new StringBuilder();
        for (int i = 0; i < length; i++) {
            sb.append(newtext);
        }
        return Operand.Create(sb.toString());
    }

    public Operand visitRIGHT_fun(final RIGHT_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToText("Function RIGHT parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        if (args.size() == 1) {
            return Operand.Create(
                      ((Character)firstValue.TextValue().charAt(firstValue.TextValue().length() - 1)).toString());
        }
        final Operand secondValue = args.get(1).ToNumber("Function RIGHT parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }
        return Operand.Create(firstValue.TextValue()
                .substring(firstValue.TextValue().length() - secondValue.IntValue(),
                    firstValue.TextValue().length()));
    }

    public Operand visitRMB_fun(final RMB_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function RMB parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(F_base_ToChineseRMB(new BigDecimal(firstValue.NumberValue())));
    }

    public Operand visitSEARCH_fun(final SEARCH_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToText("Function SEARCH parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final Operand secondValue = args.get(1).ToText("Function SEARCH parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }
        if (args.size() == 2) {
            final int p = secondValue.TextValue().toLowerCase().indexOf(firstValue.TextValue().toLowerCase())
                    + excelIndex;
            return Operand.Create(p);
        }
        final Operand thirdValue = args.get(2).ToNumber("Function SEARCH parameter 3 is error!");
        if (thirdValue.IsError()) {
            return thirdValue;
        }
        final int p2 = secondValue.TextValue().toLowerCase().indexOf(firstValue.TextValue().toLowerCase(),
                thirdValue.IntValue()) + excelIndex;
        return Operand.Create(p2);
    }

    public Operand visitSUBSTITUTE_fun(final SUBSTITUTE_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }
        final Operand firstValue = args.get(0).ToText("Function SUBSTITUTE parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final Operand secondValue = args.get(1).ToText("Function SUBSTITUTE parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }
        final Operand thirdValue = args.get(2).ToText("Function SUBSTITUTE parameter 3 is error!");
        if (thirdValue.IsError()) {
            return thirdValue;
        }
        if (args.size() == 3) {
            return Operand.Create(firstValue.TextValue().replace(secondValue.TextValue(), thirdValue.TextValue()));
        }
        final Operand fourthValue = args.get(3).ToNumber("Function SUBSTITUTE parameter 4 is error!");
        if (fourthValue.IsError()) {
            return fourthValue;
        }

        final String text = firstValue.TextValue();
        final String oldtext = secondValue.TextValue();
        final String newtext = thirdValue.TextValue();
        final int index = fourthValue.IntValue();

        int index2 = 0;
        final StringBuilder sb = new StringBuilder();
        for (int i = 0; i < text.length(); i++) {
            boolean b = true;
            for (int j = 0; j < oldtext.length(); j++) {
                final char t = text.charAt(i + j);
                final char t2 = oldtext.charAt(j);
                if (t != t2) {
                    b = false;
                    break;
                }
            }
            if (b) {
                index2++;
            }
            if (b && index2 == index) {
                sb.append(newtext);
                i += oldtext.length() - 1;
            } else {
                sb.append(text.charAt(i));
            }
        }
        return Operand.Create(sb.toString());
    }

    public Operand visitT_fun(final T_funContext context) {
        final Operand firstValue = visit(context.expr());
        if (firstValue.Type() == OperandType.STRING) {
            return firstValue;
        }
        return Operand.Create("");
    }

    public Operand visitTEXT_fun(final TEXT_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0);
        if (firstValue.IsError()) {
            return firstValue;
        }
        final Operand secondValue = args.get(1).ToText("Function TEXT parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        if (firstValue.Type() == OperandType.STRING) {
            return firstValue;
        } else if (firstValue.Type() == OperandType.BOOLEAN) {
            return Operand.Create(firstValue.BooleanValue() ? "TRUE" : "FALSE");
        } else if (firstValue.Type() == OperandType.NUMBER) {
            final DecimalFormat myFormatter = new DecimalFormat(secondValue.TextValue());
            return Operand.Create(myFormatter.format(firstValue.NumberValue()));
        } else if (firstValue.Type() == OperandType.DATE) {
            return Operand.Create(firstValue.DateValue().toString(secondValue.TextValue()));
        }
        firstValue = firstValue.ToText("Function TEXT parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(firstValue.TextValue().toString());
    }

    public Operand visitTRIM_fun(final TRIM_funContext context) {
        final Operand firstValue = visit(context.expr()).ToText("Function TRIM parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(firstValue.TextValue().trim());
    }

    public Operand visitUPPER_fun(final UPPER_funContext context) {
        final Operand firstValue = visit(context.expr()).ToText("Function UPPER/TOUPPER parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(firstValue.TextValue().toUpperCase());
    }

    public Operand visitVALUE_fun(final VALUE_funContext context) {
        final Operand firstValue = visit(context.expr()).ToText("Function VALUE parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        try {
            final Double d = Double.parseDouble(firstValue.TextValue());
            return Operand.Create(d);
        } catch (final Exception e) {
        }
        // if (double.TryParse(firstValue.TextValue(), NumberStyles.Any, cultureInfo,
        // out double d)) {
        // return Operand.Create(d);
        // }
        return Operand.Error("Function VALUE parameter is error!");
    }

    private String F_base_ToSBC(final String input) {
        final StringBuilder sb = new StringBuilder(input);
        for (int i = 0; i < input.length(); i++) {
            final char c = input.charAt(i);// [i];
            if (c == ' ') {
                sb.setCharAt(i, (char) 12288);
                // sb[i] = (char) 12288;
            } else if (c < 127) {
                sb.setCharAt(i, (char) (c + 65248));
                // sb[i] = (char) (c + 65248);
            }
        }
        return sb.toString();
    }

    private String F_base_ToDBC(final String input) {
        final StringBuilder sb = new StringBuilder(input);
        for (int i = 0; i < input.length(); i++) {
            final char c = input.charAt(i);// [i];
            if (c == 12288) {
                sb.setCharAt(i, (char) 32);
                continue;
            } else if (c > 65280 && c < 65375) {
                sb.setCharAt(i, (char) (c - 65248));
            }
        }
        return sb.toString();
    }

    static final String[] CN_UPPER_NUMBER = { "零", "壹", "贰", "叁", "肆","伍", "陆", "柒", "捌", "玖" };
    static final String[] CN_UPPER_MONETRAY_UNIT = { "分", "角", "元", "拾", "佰", "仟", "万", "拾", "佰", "仟", "亿", "拾", "佰", "仟", "兆", "拾",    "佰", "仟" };
    static final String CN_FULL = "整";
    static final String CN_NEGATIVE = "负";
    static final int MONEY_PRECISION = 2;
    static final String CN_ZEOR_FULL = "零元" + CN_FULL;
    private String F_base_ToChineseRMB(final BigDecimal  numberOfMoney) {
        StringBuffer sb = new StringBuffer();
        int signum = numberOfMoney.signum();
        if (signum == 0) {
            return CN_ZEOR_FULL;
        }
        long number = numberOfMoney.movePointRight(MONEY_PRECISION).setScale(0, 4).abs().longValue();
        long scale = number % 100;
        int numUnit = 0; 
        int numIndex = 0;
        boolean getZero = false;
        if (!(scale > 0)) {
            numIndex = 2;
            number = number / 100;
            getZero = true;
        }
        if ((scale > 0) && (!(scale % 10 > 0))) {
            numIndex = 1;
            number = number / 10;
            getZero = true;
        }
        int zeroSize = 0;
        while (true) {
            if (number <= 0) {
                break;
            }
            // 每次获取到最后一个数
            numUnit = (int) (number % 10);
            if (numUnit > 0) {
                if ((numIndex == 9) && (zeroSize >= 3)) {
                    sb.insert(0, CN_UPPER_MONETRAY_UNIT[6]);
                }
                if ((numIndex == 13) && (zeroSize >= 3)) {
                    sb.insert(0, CN_UPPER_MONETRAY_UNIT[10]);
                }
                sb.insert(0, CN_UPPER_MONETRAY_UNIT[numIndex]);
                sb.insert(0, CN_UPPER_NUMBER[numUnit]);
                getZero = false;
                zeroSize = 0;
            } else {
                ++zeroSize;
                if (!(getZero)) {
                    sb.insert(0, CN_UPPER_NUMBER[numUnit]);
                }
                if (numIndex == 2) {
                    if (number > 0) {
                        sb.insert(0, CN_UPPER_MONETRAY_UNIT[numIndex]);
                    }
                } else if (((numIndex - 2) % 4 == 0) && (number % 1000 > 0)) {
                    sb.insert(0, CN_UPPER_MONETRAY_UNIT[numIndex]);
                }
                getZero = true;
            }
            // 让number每次都去掉最后一个数
            number = number / 10;
            ++numIndex;
        }
        // 如果signum == -1，则说明输入的数字为负数，就在最前面追加特殊字符：负
        if (signum == -1) {
            sb.insert(0, CN_NEGATIVE);
        }
        // 输入的数字小数点后两位为"00"的情况，则要在最后追加特殊字符：整
        if (!(scale > 0)) {
            sb.append(CN_FULL);
        }
        return sb.toString();
    }

    public Operand visitDATEVALUE_fun(final DATEVALUE_funContext context) {
        final Operand firstValue = visit(context.expr()).ToText("Function DATEVALUE parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        MyDate date= MyDate.parse(firstValue.TextValue());
        if(date !=null){
            return Operand.Create(date);
        }
        return Operand.Error("Function DATEVALUE parameter is error!");
    }

    public Operand visitTIMEVALUE_fun(final TIMEVALUE_funContext context) {
        final Operand firstValue = visit(context.expr()).ToText("Function TIMEVALUE parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        MyDate date= MyDate.parse(firstValue.TextValue());
        if(date !=null){
            return Operand.Create(date);
        }
        return Operand.Error("Function TIMEVALUE parameter is error!");
    }

    public Operand visitDATE_fun(final DATE_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function DATE parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        MyDate d;
        if (args.size() == 3) {
            d = new MyDate(args.get(0).IntValue(), args.get(1).IntValue(), args.get(2).IntValue(), 0, 0, 0);
        } else if (args.size() == 4) {
            d = new MyDate(args.get(0).IntValue(), args.get(1).IntValue(), args.get(2).IntValue(),
                    args.get(3).IntValue(), 0, 0);
        } else if (args.size() == 5) {
            d = new MyDate(args.get(0).IntValue(), args.get(1).IntValue(), args.get(2).IntValue(),
                    args.get(3).IntValue(), args.get(4).IntValue(), 0);
        } else {
            d = new MyDate(args.get(0).IntValue(), args.get(1).IntValue(), args.get(2).IntValue(),
                    args.get(3).IntValue(), args.get(4).IntValue(), args.get(5).IntValue());
        }
        return Operand.Create(d);
    }

    public Operand visitTIME_fun(final TIME_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function TIME parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        MyDate d;
        if (args.size() == 3) {
            d = new MyDate(0, 0, 0, args.get(0).IntValue(), args.get(1).IntValue(), args.get(2).IntValue());
        } else {
            d = new MyDate(0, 0, 0, args.get(0).IntValue(), args.get(1).IntValue(), 0);
        }
        return Operand.Create(d);
    }

    public Operand visitNOW_fun(final NOW_funContext context) {
        return Operand.Create(new MyDate(DateTime.now()));
    }

    public Operand visitTODAY_fun(final TODAY_funContext context) {
        final DateTime dt = DateTime.now();
        return Operand.Create(new MyDate(dt.getYear(), dt.getMonthOfYear() , dt.getDayOfMonth(), 0, 0, 0));
    }

    public Operand visitYEAR_fun(final YEAR_funContext context) {
        final Operand firstValue = visit(context.expr()).ToDate("Function YEAR parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(firstValue.DateValue().Year);
    }

    public Operand visitMONTH_fun(final MONTH_funContext context) {
        final Operand firstValue = visit(context.expr()).ToDate("Function MONTH parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(firstValue.DateValue().Month);
    }

    public Operand visitDAY_fun(final DAY_funContext context) {
        final Operand firstValue = visit(context.expr()).ToDate("Function DAY parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(firstValue.DateValue().Day);
    }

    public Operand visitHOUR_fun(final HOUR_funContext context) {
        final Operand firstValue = visit(context.expr()).ToDate("Function HOUR parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(firstValue.DateValue().Hour);
    }

    public Operand visitMINUTE_fun(final MINUTE_funContext context) {
        final Operand firstValue = visit(context.expr()).ToDate("Function MINUTE parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(firstValue.DateValue().Minute);
    }

    public Operand visitSECOND_fun(final SECOND_funContext context) {
        final Operand firstValue = visit(context.expr()).ToDate("Function SECOND parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(firstValue.DateValue().Second);
    }

    public Operand visitWEEKDAY_fun(final WEEKDAY_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToDate("Function WEEKDAY parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        int type = 1;
        if (args.size() == 2) {
            final Operand secondValue = args.get(1).ToNumber("Function WEEKDAY parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            type = secondValue.IntValue();
        }

        final int t = firstValue.DateValue().DayOfWeek();
        if (type == 1) {
            return Operand.Create((double) (t + 1));
        } else if (type == 2) {
            if (t == 0)
                return Operand.Create(7d);
            return Operand.Create((double) t);
        }
        if (t == 0) {
            return Operand.Create(6d);
        }
        return Operand.Create((double) (t - 1));
    }

    public Operand visitDATEDIF_fun(final DATEDIF_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToDate("Function DATEDIF parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final Operand secondValue = args.get(1).ToDate("Function DATEDIF parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }
        final Operand thirdValue = args.get(2).ToText("Function DATEDIF parameter 3 is error!");
        if (thirdValue.IsError()) {
            return thirdValue;
        }

        final MyDate startDate = firstValue.DateValue();
        final MyDate endDate = secondValue.DateValue();
        final String t = thirdValue.TextValue().toLowerCase();

        if (t.equals("y")) {
            boolean b = false;
            if (startDate.Month < endDate.Month) {
                b = true;
            } else if (startDate.Month == endDate.Month) {
                if (startDate.Day <= endDate.Day)
                    b = true;
            }
            if (b) {
                return Operand.Create((endDate.Year - startDate.Year));
            } else {
                return Operand.Create((endDate.Year - startDate.Year - 1));
            }
        } else if (t.equals("m")) {
            boolean b = false;
            if (startDate.Day <= endDate.Day)
                b = true;
            if (b) {
                return Operand.Create((endDate.Year * 12 + endDate.Month - startDate.Year * 12 - startDate.Month));
            } else {
                return Operand.Create((endDate.Year * 12 + endDate.Month - startDate.Year * 12 - startDate.Month - 1));
            }
        } else if (t.equals("d")) {
            return Operand.Create((int) endDate.SUB(startDate).ToNumber());
        } else if (t.equals("yd")) {
            int day = endDate.DayOfYear() - startDate.DayOfYear();
            if (endDate.Year > startDate.Year && day < 0) {
                final int days = new DateTime(startDate.Year, 12, 31, 0, 0, 0, DateTimeZone.UTC).getDayOfYear();
                day = days + day;
            }
            return Operand.Create((day));
        } else if (t.equals("md")) {
            int mo = endDate.Day - startDate.Day;
            if (mo < 0) {
                final int days = new DateTime(startDate.Year, startDate.Month + 1, 1, 0, 00, DateTimeZone.UTC)
                        .plusDays(-1).dayOfMonth().get();
                mo += days;
            }
            return Operand.Create((mo));
        } else if (t.equals("ym")) {
            int mo = endDate.Month - startDate.Month;
            if (endDate.Day < startDate.Day)
                mo = mo - 1;
            if (mo < 0)
                mo += 12;
            return Operand.Create((mo));
        }
        return Operand.Error("Function DATEDIF parameter 3 is error!");
    }

    public Operand visitDAYS360_fun(final DAYS360_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToDate("Function DAYS360 parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final Operand secondValue = args.get(1).ToDate("Function DAYS360 parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        final MyDate startDate = firstValue.DateValue();
        final MyDate endDate = secondValue.DateValue();

        boolean method = false;
        if (args.size() == 3) {
            final Operand thirdValue = args.get(2).ToDate("Function DAYS360 parameter 3 is error!");
            if (thirdValue.IsError()) {
                return thirdValue;
            }
            method = thirdValue.BooleanValue();
        }
        int days = endDate.Year * 360 + (endDate.Month - 1) * 30 - startDate.Year * 360 - (startDate.Month - 1) * 30;
        if (method) {
            if (endDate.Day == 31)
                days += 30;
            if (startDate.Day == 31)
                days -= 30;
        } else {
            if (startDate.Day == new DateTime(startDate.Year, startDate.Month + 1, 1, 0, 0, 0, DateTimeZone.UTC)
                    .plusDays(-1).dayOfMonth().get()) {
                days -= 30;
            } else {
                days -= startDate.Day;
            }
            if (endDate.Day == new DateTime(endDate.Year, endDate.Month + 1, 1, 0, 0, 0, DateTimeZone.UTC).plusDays(-1)
                    .dayOfMonth().get()) {
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

    public Operand visitEDATE_fun(final EDATE_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToDate("Function EDATE parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final Operand secondValue = args.get(1).ToNumber("Function EDATE parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        return Operand.Create(firstValue.DateValue().AddMonths(secondValue.IntValue()));
    }

    public Operand visitEOMONTH_fun(final EOMONTH_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToDate("Function EOMONTH parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final Operand secondValue = args.get(1).ToNumber("Function EOMONTH parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        final MyDate dt = firstValue.DateValue().AddMonths(secondValue.IntValue() + 1);
        final DateTime dt2 = new DateTime(dt.Year, dt.Month, 1, 0, 0, 0, DateTimeZone.UTC).plusDays(-1);
        return Operand.Create(dt2);
    }

    public Operand visitNETWORKDAYS_fun(final NETWORKDAYS_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand a = visit(item).ToDate("Function NETWORKDAYS parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        DateTime startDate = args.get(0).DateValue().ToDateTime();
        final DateTime endDate = args.get(1).DateValue().ToDateTime();

        final List<DateTime> list = new ArrayList<DateTime>();
        for (int i = 2; i < args.size(); i++) {
            list.add(args.get(i).DateValue().ToDateTime());
        }

        int days = 0;
        while (startDate.isBefore(endDate)) {
            if (startDate.dayOfWeek().get() != DateTimeConstants.SUNDAY
                    && startDate.dayOfWeek().get() != DateTimeConstants.SATURDAY) {
                if (list.contains(startDate) == false) {
                    days++;
                }
            }
            startDate = startDate.plusDays(1);
        }
        return Operand.Create(days);
    }

    public Operand visitWORKDAY_fun(final WORKDAY_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToDate("Function WORKDAY parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final Operand secondValue = args.get(1).ToNumber("Function WORKDAY parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        DateTime startDate = firstValue.DateValue().ToDateTime();
        int days = secondValue.IntValue();
        final List<DateTime> list = new ArrayList<DateTime>();
        for (int i = 2; i < args.size(); i++) {
            args.set(i, args.get(i).ToDate("Function WORKDAY parameter {i + 1} is error!"));
            if (args.get(i).IsError()) {
                return args.get(i);
            }
            list.add(args.get(i).DateValue().ToDateTime());
        }
        while (days > 0) {
            startDate = startDate.plusDays(1);
            if (startDate.dayOfWeek().get() == DateTimeConstants.SATURDAY)
                continue;
            if (startDate.dayOfWeek().get() == DateTimeConstants.SUNDAY)
                continue;
            if (list.contains(startDate))
                continue;
            days--;
        }
        return Operand.Create(startDate);
    }

    public Operand visitWEEKNUM_fun(final WEEKNUM_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToDate("Function WEEKNUM parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        final DateTime startDate = firstValue.DateValue().ToDateTime();

        int days = startDate.dayOfYear().get()
                + new DateTime(startDate.getYear(), 1, 1, 0, 0, 0, DateTimeZone.UTC).dayOfWeek().get();
        if (args.size() == 2) {
            final Operand secondValue = args.get(1).ToNumber("Function WEEKNUM parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (secondValue.IntValue() == 2) {
                days--;
            }
        }

        final double week = Math.ceil(days / 7.0);
        return Operand.Create(week);
    }

    public Operand visitMAX_fun(final MAX_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function MAX parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final List<Double> list = new ArrayList<Double>();
        final boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function MAX parameter error!");
        }
        return Operand.Create(Max(list));
    }

    public Operand visitMEDIAN_fun(final MEDIAN_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function MEDIAN parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new ArrayList<Double>();
        final boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function MEDIAN parameter error!");
        }

        list = ShellSort(list);
        // list = list.OrderBy(q => q).ToList();
        return Operand.Create(list.get(list.size() / 2));
    }

    public Operand visitMIN_fun(final MIN_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function MIN parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final List<Double> list = new ArrayList<Double>();
        final boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function MIN parameter error!");
        }

        return Operand.Create(Min(list));
    }

    public Operand visitQUARTILE_fun(final QUARTILE_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToArray("Function QUARTILE parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final Operand secondValue = args.get(1).ToNumber("Function QUARTILE parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        final List<Double> list = new ArrayList<Double>();
        final boolean o = F_base_GetList_2(firstValue, list);
        if (o == false) {
            return Operand.Error("Function QUARTILE parameter 1 error!");
        }

        final int quant = secondValue.IntValue();
        if (quant < 0 || quant > 4) {
            return Operand.Error("Function QUARTILE parameter 2 is error!");
        }
        try {
            return Operand.Create(ExcelFunctions.Quartile(list, quant));
        } catch (final Exception e) {
        }
        return Operand.Error("Function QUARTILE is error!");
    }

    public Operand visitMODE_fun(final MODE_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function MODE parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final List<Double> list = new ArrayList<Double>();
        final boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function MODE parameter error!");
        }

        final Map<Double, Integer> dict = new HashMap<Double, Integer>();
        for (final double item : list) {
            if (dict.containsKey(item)) {
                dict.put(item, dict.get(item) + 1);
            } else {
                dict.put(item, 1);
            }
        }
        int maxCount = 0;
        double maxValue = 0;
        for (final Double d : dict.keySet()) {
            final int count = dict.get(d);
            if (count > maxCount) {
                maxCount = count;
                maxValue = d;
            }
        }
        return Operand.Create(maxValue);
        // return Operand.Create(dict.OrderByDescending(q => q.Value).First().Key);
    }

    public Operand visitLARGE_fun(final LARGE_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToArray("Function LARGE parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final Operand secondValue = args.get(1).ToNumber("Function LARGE parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        List<Double> list = new ArrayList<Double>();
        final boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function LARGE parameter error!");
        }

        list = ShellSort(list);
        // list = list.OrderByDescending(q => q).ToList();
        // int quant = secondValue.IntValue();
        return Operand.Create(list.get(list.size() - 1 - (secondValue.IntValue() - excelIndex)));
    }

    public Operand visitSMALL_fun(final SMALL_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToArray("Function SMALL parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final Operand secondValue = args.get(1).ToNumber("Function SMALL parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        List<Double> list = new ArrayList<Double>();
        final boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function SMALL parameter error!");
        }

        list = ShellSort(list);
        // list = list.OrderBy(q => q).ToList();
        // int quant = secondValue.IntValue();
        return Operand.Create(list.get(secondValue.IntValue() - excelIndex));
        // return Operand.Create(list[secondValue.IntValue() - excelIndex]);
    }

    public Operand visitPERCENTILE_fun(final PERCENTILE_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToArray("Function PERCENTILE parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final Operand secondValue = args.get(1).ToNumber("Function PERCENTILE parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        final List<Double> list = new ArrayList<Double>();
        final boolean o = F_base_GetList_2(firstValue, list);
        if (o == false) {
            return Operand.Error("Function PERCENTILE parameter error!");
        }

        final double k = secondValue.NumberValue();
        try {
            return Operand.Create(ExcelFunctions.Percentile(list, k));
        } catch (final Exception e) {
        }
        return Operand.Error("Function PERCENTILE parameter error!");
    }

    public Operand visitPERCENTRANK_fun(final PERCENTRANK_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToArray("Function PERCENTRANK parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final Operand secondValue = args.get(1).ToNumber("Function PERCENTRANK parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        final List<Double> list = new ArrayList<Double>();
        final boolean o = F_base_GetList_2(firstValue, list);
        if (o == false) {
            return Operand.Error("Function PERCENTRANK parameter error!");
        }

        final double k = secondValue.NumberValue();
        final double v = ExcelFunctions.PercentRank(list, k);
        int d = 3;
        if (args.size() == 3) {
            final Operand thirdValue = args.get(2).ToNumber("Function PERCENTRANK parameter 3 is error!");
            if (thirdValue.IsError()) {
                return thirdValue;
            }
            d = thirdValue.IntValue();
        }
        return Operand.Create(round(v, d));
    }

    public Operand visitAVERAGE_fun(final AVERAGE_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final List<Double> list = new ArrayList<Double>();
        final boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function AVERAGE parameter error!");
        }

        return Operand.Create(Average(list));
    }

    public Operand visitAVERAGEIF_fun(final AVERAGEIF_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final List<Double> list = new ArrayList<Double>();
        final boolean o = F_base_GetList_2(args.get(0), list);
        if (o == false) {
            return Operand.Error("Function AVERAGE parameter 1 error!");
        }

        List<Double> sumdbs;
        if (args.size() == 3) {
            sumdbs = new ArrayList<Double>();
            final boolean o2 = F_base_GetList_2(args.get(2), sumdbs);
            if (o2 == false) {
                return Operand.Error("Function AVERAGE parameter 3 error!");
            }
        } else {
            sumdbs = list;
        }

        double sum;
        int count;
        if (args.get(1).Type() == OperandType.NUMBER) {
            count = F_base_countif(list, args.get(1).NumberValue());
            sum = count * args.get(1).NumberValue();
        } else {
            try {
                final Double d = Double.parseDouble(args.get(1).TextValue().trim());
                count = F_base_countif(list, d);
                sum = F_base_sumif(list, "=" + args.get(1).TextValue().trim(), sumdbs);
            } catch (final Exception e) {
                final String sunif = args.get(1).TextValue().trim();
                if (sumifRegex.matcher(sunif).find()) {
                    count = F_base_countif(list, sunif);
                    sum = F_base_sumif(list, sunif, sumdbs);
                } else {
                    return Operand.Error("Function AVERAGE parameter 2 error!");
                }
            }
            // if (double.TryParse(args.get(1).TextValue().trim(), NumberStyles.Any,
            // cultureInfo, out double d)) {
            // count = F_base_countif(list, d);
            // sum = F_base_sumif(list, "=" + args.get(1).TextValue().trim(), sumdbs);
            // } else {
            // String sunif = args.get(1).TextValue().trim();
            // if (sumifRegex.matcher(sunif)) {
            // count = F_base_countif(list, sunif);
            // sum = F_base_sumif(list, sunif, sumdbs);
            // } else {
            // return Operand.Error("Function AVERAGE parameter 2 error!");
            // }
            // }
        }
        return Operand.Create(sum / count);
    }

    public Operand visitGEOMEAN_fun(final GEOMEAN_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        if (args.size() == 1)
            return args.get(0);

        final List<Double> list = new ArrayList<Double>();
        final boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function GEOMEAN parameter error!");
        }

        double sum = 1;
        for (final double db : list) {
            sum *= db;
        }
        return Operand.Create(Math.pow(sum, 1.0 / list.size()));
    }

    public Operand visitHARMEAN_fun(final HARMEAN_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        if (args.size() == 1)
            return args.get(0);

        final List<Double> list = new ArrayList<Double>();
        final boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function HARMEAN parameter error!");
        }

        double sum = 0;
        for (final double db : list) {
            if (db == 0) {
                return Operand.Error("Function HARMEAN parameter error!");
            }
            sum += 1 / db;
        }
        return Operand.Create(list.size() / sum);
    }

    public Operand visitCOUNT_fun(final COUNT_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final List<Double> list = new ArrayList<Double>();
        final boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function COUNT parameter error!");
        }

        return Operand.Create(list.size());
    }

    public Operand visitCOUNTIF_fun(final COUNTIF_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final List<Double> list = new ArrayList<Double>();
        final boolean o = F_base_GetList_2(args.get(0), list);
        if (o == false) {
            return Operand.Error("Function COUNTIF parameter error!");
        }

        int count;
        if (args.get(1).Type() == OperandType.NUMBER) {
            count = F_base_countif(list, args.get(1).NumberValue());
        } else {
            try {
                final Double d = Double.parseDouble(args.get(1).TextValue().trim());
                count = F_base_countif(list, d);
            } catch (final Exception e) {
                final String sunif = args.get(1).TextValue().trim();
                if (sumifRegex.matcher(sunif).find()) {
                    count = F_base_countif(list, sunif);
                } else {
                    return Operand.Error("Function COUNTIF parameter 2 error!");
                }
            }
            // if (double.TryParse(args.get(1).TextValue().trim(), NumberStyles.Any,
            // cultureInfo, out double d)) {
            // count = F_base_countif(list, d);
            // } else {
            // String sunif = args.get(1).TextValue().trim();
            // if (sumifRegex.matcher(sunif)) {
            // count = F_base_countif(list, sunif);
            // } else {
            // return Operand.Error("Function COUNTIF parameter 2 error!");
            // }
            // }
        }
        return Operand.Create(count);
    }

    public Operand visitSUM_fun(final SUM_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        if (args.size() == 1)
            return args.get(0);

        final List<Double> list = new ArrayList<Double>();
        final boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function SUM parameter error!");
        }

        final double sum = Sum(list);
        return Operand.Create(sum);
    }

    public Operand visitSUMIF_fun(final SUMIF_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final List<Double> list = new ArrayList<Double>();
        final boolean o = F_base_GetList_2(args.get(0), list);
        if (o == false) {
            return Operand.Error("Function SUMIF parameter 1 error!");
        }

        List<Double> sumdbs;
        if (args.size() == 3) {
            sumdbs = new ArrayList<Double>();
            final boolean o2 = F_base_GetList_2(args.get(2), sumdbs);
            if (o2 == false) {
                return Operand.Error("Function SUMIF parameter 3 error!");
            }
        } else {
            sumdbs = list;
        }

        double sum;
        if (args.get(1).Type() == OperandType.NUMBER) {
            sum = F_base_countif(list, args.get(1).NumberValue()) * args.get(1).NumberValue();
        } else {
                if (Pattern.compile("^-?(\\d+)(\\.\\d+)?$").matcher(args.get(1).TextValue().trim()).find()){
                    sum = F_base_sumif(list, "=" + args.get(1).TextValue().trim(), sumdbs);
                }else{
                    final String sunif = args.get(1).TextValue().trim();
                    if (sumifRegex.matcher(sunif).find()) {
                        sum = F_base_sumif(list, sunif, sumdbs);
                    } else {
                        return Operand.Error("Function SUMIF parameter 2 error!");
                    }
                }
            // if (double.TryParse(args.get(1).TextValue().trim(), NumberStyles.Any,
            // cultureInfo, out _)) {
            // sum = F_base_sumif(list, "=" + args.get(1).TextValue().trim(), sumdbs);
            // } else {
            // String sunif = args.get(1).TextValue().trim();
            // if (sumifRegex.matcher(sunif)) {
            // sum = F_base_sumif(list, sunif, sumdbs);
            // } else {
            // return Operand.Error("Function SUMIF parameter 2 error!");
            // }
            // }
        }
        return Operand.Create(sum);
    }

    public Operand visitAVEDEV_fun(final AVEDEV_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final List<Double> list = new ArrayList<Double>();
        final boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function AVEDEV parameter error!");
        }

        final double avg = Average(list);
        double sum = 0;
        for (int i = 0; i < list.size(); i++) {
            sum += Math.abs(list.get(i) - avg);
        }
        return Operand.Create(sum / list.size());
    }

    public Operand visitSTDEV_fun(final STDEV_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        if (args.size() == 1) {
            return Operand.Error("Function STDEV parameter only one error!");
        }
        final List<Double> list = new ArrayList<Double>();
        final boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function STDEV parameter error!");
        }

        final double avg = Average(list);
        double sum = 0;
        for (int i = 0; i < list.size(); i++) {
            sum += (list.get(i) - avg) * (list.get(i) - avg);
        }
        return Operand.Create(Math.sqrt(sum / (list.size() - 1)));
    }

    public Operand visitSTDEVP_fun(final STDEVP_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final List<Double> list = new ArrayList<Double>();
        final boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function STDEVP parameter error!");
        }

        double sum = 0;
        final double avg = Average(list);

        for (int i = 0; i < list.size(); i++) {
            sum += (list.get(i) - avg) * (list.get(i) - avg);
        }
        return Operand.Create(Math.sqrt(sum / (list.size())));
    }

    public Operand visitDEVSQ_fun(final DEVSQ_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final List<Double> list = new ArrayList<Double>();
        final boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function DEVSQ parameter error!");
        }

        final double avg = Average(list);
        double sum = 0;
        for (int i = 0; i < list.size(); i++) {
            sum += (list.get(i) - avg) * (list.get(i) - avg);
        }
        return Operand.Create(sum);
    }

    public Operand visitVAR_fun(final VAR_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        if (args.size() == 1) {
            return Operand.Error("Function VAR parameter only one error!");
        }
        final List<Double> list = new ArrayList<Double>();
        final boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function VAR parameter error!");
        }

        double sum = 0;
        double sum2 = 0;
        for (int i = 0; i < list.size(); i++) {
            sum += list.get(i) * list.get(i);
            sum2 += list.get(i);
        }
        return Operand.Create((list.size() * sum - sum2 * sum2) / list.size() / (list.size() - 1));
    }

    public Operand visitVARP_fun(final VARP_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final List<Double> list = new ArrayList<Double>();
        final boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function VARP parameter error!");
        }

        double sum = 0;
        final double avg = Average(list);
        for (int i = 0; i < list.size(); i++) {
            sum += (avg - list.get(i)) * (avg - list.get(i));
        }
        return Operand.Create(sum / list.size());
    }

    public Operand visitNORMDIST_fun(final NORMDIST_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToNumber("Function NORMDIST parameter 1 error!");
        if (firstValue.IsError())
            return firstValue;
        final Operand secondValue = args.get(1).ToNumber("Function NORMDIST parameter 2 error!");
        if (secondValue.IsError())
            return secondValue;
        final Operand thirdValue = args.get(2).ToNumber("Function NORMDIST parameter 3 error!");
        if (thirdValue.IsError())
            return thirdValue;

        final Operand fourthValue = args.get(3).ToBoolean("Function NORMDIST parameter 4 error!");
        if (fourthValue.IsError())
            return fourthValue;

        final double num = firstValue.NumberValue();
        final double avg = secondValue.NumberValue();
        final double STDEV = thirdValue.NumberValue();
        final boolean b = fourthValue.BooleanValue();
        return Operand.Create(ExcelFunctions.NormDist(num, avg, STDEV, b));
    }

    public Operand visitNORMINV_fun(final NORMINV_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function NORMINV parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final double num = args.get(0).NumberValue();
        final double avg = args.get(1).NumberValue();
        final double STDEV = args.get(2).NumberValue();
        return Operand.Create(ExcelFunctions.NormInv(num, avg, STDEV));
    }

    public Operand visitNORMSDIST_fun(final NORMSDIST_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function NORMSDIST parameter 1 error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        final double k = firstValue.NumberValue();
        return Operand.Create(ExcelFunctions.NormSDist(k));
    }

    public Operand visitNORMSINV_fun(final NORMSINV_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function NORMSINV parameter 1 error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        final double k = firstValue.NumberValue();
        return Operand.Create(ExcelFunctions.NormSInv(k));
    }

    public Operand visitBETADIST_fun(final BETADIST_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function BETADIST parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final double x = args.get(0).NumberValue();
        final double alpha = args.get(1).NumberValue();
        final double beta = args.get(2).NumberValue();

        if (alpha < 0.0 || beta < 0.0) {
            return Operand.Error("Function BETADIST parameter error!");
        }

        return Operand.Create(ExcelFunctions.BetaDist(x, alpha, beta));
    }

    public Operand visitBETAINV_fun(final BETAINV_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function BETAINV parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final double probability = args.get(0).NumberValue();
        final double alpha = args.get(1).NumberValue();
        final double beta = args.get(2).NumberValue();
        if (alpha < 0.0 || beta < 0.0 || probability < 0.0 || probability > 1.0) {
            return Operand.Error("Function BETAINV parameter error!");
        }
        try {
            return Operand.Create(ExcelFunctions.BetaInv(probability, alpha, beta));
        } catch (final Exception e) {
        }
        return Operand.Error("Function BETAINV parameter error!");
    }

    public Operand visitBINOMDIST_fun(final BINOMDIST_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToNumber("Function BINOMDIST parameter 1 error!");
        if (firstValue.IsError())
            return firstValue;
        final Operand secondValue = args.get(1).ToNumber("Function BINOMDIST parameter 2 error!");
        if (secondValue.IsError())
            return secondValue;
        final Operand thirdValue = args.get(2).ToNumber("Function BINOMDIST parameter 3 error!");
        if (thirdValue.IsError())
            return thirdValue;

        final Operand fourthValue = args.get(3).ToBoolean("Function BINOMDIST parameter 4 error!");
        if (fourthValue.IsError())
            return fourthValue;

        if (!(thirdValue.NumberValue() >= 0.0 && thirdValue.NumberValue() <= 1.0 && secondValue.NumberValue() >= 0)) {
            return Operand.Error("Function BINOMDIST parameter error!");
        }
        return Operand.Create(ExcelFunctions.BinomDist(firstValue.IntValue(), secondValue.IntValue(),
                thirdValue.NumberValue(), fourthValue.BooleanValue()));
    }

    public Operand visitEXPONDIST_fun(final EXPONDIST_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToNumber("Function EXPONDIST parameter 1 error!");
        if (firstValue.IsError())
            return firstValue;
        final Operand secondValue = args.get(1).ToNumber("Function EXPONDIST parameter 2 error!");
        if (secondValue.IsError())
            return secondValue;
        final Operand thirdValue = args.get(2).ToBoolean("Function EXPONDIST parameter 3 error!");
        if (thirdValue.IsError())
            return thirdValue;

        if (firstValue.NumberValue() < 0.0) {
            return Operand.Error("Function EXPONDIST parameter error!");
        }

        return Operand.Create(ExcelFunctions.ExponDist(firstValue.NumberValue(), secondValue.NumberValue(),
                thirdValue.BooleanValue()));
    }

    public Operand visitFDIST_fun(final FDIST_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function FDIST parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final double x = args.get(0).NumberValue();
        final int degreesFreedom = args.get(1).IntValue();
        final int degreesFreedom2 = args.get(2).IntValue();
        if (degreesFreedom <= 0.0 || degreesFreedom2 <= 0.0) {
            return Operand.Error("Function FDIST parameter error!");
        }
        return Operand.Create(ExcelFunctions.FDist(x, degreesFreedom, degreesFreedom2));
    }

    public Operand visitFINV_fun(final FINV_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function FINV parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final double probability = args.get(0).NumberValue();
        final int degreesFreedom = args.get(1).IntValue();
        final int degreesFreedom2 = args.get(2).IntValue();
        if (degreesFreedom <= 0.0 || degreesFreedom2 <= 0.0) {
            return Operand.Error("Function FINV parameter error!");
        }
        try {
            return Operand.Create(ExcelFunctions.FInv(probability, degreesFreedom, degreesFreedom2));
        } catch (final Exception e) {
        }
        return Operand.Error("Function FINV parameter error!");
    }

    public Operand visitFISHER_fun(final FISHER_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function FISHER parameter error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        final double x = firstValue.NumberValue();
        if (x >= 1 || x <= -1) {
            return Operand.Error("Function FISHER parameter error!");
        }
        final double n = 0.5 * Math.log((1 + x) / (1 - x));
        return Operand.Create(n);
    }

    public Operand visitFISHERINV_fun(final FISHERINV_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function FISHERINV parameter error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        final double x = firstValue.NumberValue();
        final double n = (Math.exp(2 * x) - 1) / (Math.exp(2 * x) + 1);
        return Operand.Create(n);
    }

    public Operand visitGAMMADIST_fun(final GAMMADIST_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToNumber("Function GAMMADIST parameter 1 error!");
        if (firstValue.IsError())
            return firstValue;
        final Operand secondValue = args.get(1).ToNumber("Function GAMMADIST parameter 2 error!");
        if (secondValue.IsError())
            return secondValue;
        final Operand thirdValue = args.get(2).ToNumber("Function GAMMADIST parameter 3 error!");
        if (thirdValue.IsError())
            return thirdValue;

        final Operand fourthValue = args.get(3).ToBoolean("Function GAMMADIST parameter 4 error!");
        if (fourthValue.IsError())
            return fourthValue;

        final double x = firstValue.NumberValue();
        final double alpha = secondValue.NumberValue();
        final double beta = thirdValue.NumberValue();
        final boolean cumulative = fourthValue.BooleanValue();
        if (alpha < 0.0 || beta < 0.0) {
            return Operand.Error("Function GAMMADIST parameter error!");
        }
        return Operand.Create(ExcelFunctions.GammaDist(x, alpha, beta, cumulative));
    }

    public Operand visitGAMMAINV_fun(final GAMMAINV_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function GAMMAINV parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final double probability = args.get(0).NumberValue();
        final double alpha = args.get(1).NumberValue();
        final double beta = args.get(2).NumberValue();
        if (alpha < 0.0 || beta < 0.0 || probability < 0 || probability > 1.0) {
            return Operand.Error("Function GAMMAINV parameter error!");
        }
        return Operand.Create(ExcelFunctions.GammaInv(probability, alpha, beta));
    }

    public Operand visitGAMMALN_fun(final GAMMALN_funContext context) {
        final Operand firstValue = visit(context.expr()).ToNumber("Function GAMMALN parameter error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(ExcelFunctions.GAMMALN(firstValue.NumberValue()));
    }

    public Operand visitHYPGEOMDIST_fun(final HYPGEOMDIST_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function HYPGEOMDIST parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final int k = args.get(0).IntValue();
        final int draws = args.get(1).IntValue();
        final int success = args.get(2).IntValue();
        final int population = args.get(3).IntValue();
        if (!(population >= 0 && success >= 0 && draws >= 0 && success <= population && draws <= population)) {
            return Operand.Error("Function HYPGEOMDIST parameter error!");
        }
        return Operand.Create(ExcelFunctions.HypgeomDist(k, draws, success, population));
    }

    public Operand visitLOGINV_fun(final LOGINV_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function LOGINV parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        if (args.get(2).NumberValue() < 0.0) {
            return Operand.Error("Function LOGINV parameter error!");
        }
        return Operand.Create(
                ExcelFunctions.LogInv(args.get(0).NumberValue(), args.get(1).NumberValue(), args.get(2).NumberValue()));
    }

    public Operand visitLOGNORMDIST_fun(final LOGNORMDIST_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function LOGNORMDIST parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        if (args.get(2).NumberValue() < 0.0) {
            return Operand.Error("Function LOGNORMDIST parameter error!");
        }
        return Operand.Create(ExcelFunctions.LognormDist(args.get(0).NumberValue(), args.get(1).NumberValue(),
                args.get(2).NumberValue()));
    }

    public Operand visitNEGBINOMDIST_fun(final NEGBINOMDIST_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function NEGBINOMDIST parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final int k = args.get(0).IntValue();
        final double r = args.get(1).NumberValue();
        final double p = args.get(2).NumberValue();

        if (!(r >= 0.0 && p >= 0.0 && p <= 1.0)) {
            return Operand.Error("Function NEGBINOMDIST parameter error!");
        }
        return Operand.Create(ExcelFunctions.NegbinomDist(k, r, p));
    }

    public Operand visitPOISSON_fun(final POISSON_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToNumber("Function POISSON parameter 1 error!");
        if (firstValue.IsError())
            return firstValue;
        final Operand secondValue = args.get(1).ToNumber("Function POISSON parameter 2 error!");
        if (secondValue.IsError())
            return secondValue;
        final Operand thirdValue = args.get(2).ToBoolean("Function POISSON parameter 3 error!");
        if (thirdValue.IsError())
            return thirdValue;

        final int k = firstValue.IntValue();
        final double lambda = secondValue.NumberValue();
        final boolean state = thirdValue.BooleanValue();
        if (!(lambda > 0.0)) {
            return Operand.Error("Function POISSON parameter error!");
        }
        return Operand.Create(ExcelFunctions.POISSON(k, lambda, state));
    }

    public Operand visitTDIST_fun(final TDIST_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function TDIST parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final double x = args.get(0).NumberValue();
        final int degreesFreedom = args.get(1).IntValue();
        final int tails = args.get(2).IntValue();
        if (degreesFreedom <= 0.0 || tails < 1 || tails > 2) {
            return Operand.Error("Function TDIST parameter error!");
        }
        try {
            return Operand.Create(ExcelFunctions.TDist(x, degreesFreedom, tails));
        } catch (final Exception e) {
        }
        return Operand.Error("Function TDIST parameter error!");
    }

    public Operand visitTINV_fun(final TINV_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item).ToNumber("Function TINV parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final double probability = args.get(0).NumberValue();
        final int degreesFreedom = args.get(1).IntValue();
        if (degreesFreedom <= 0.0) {
            return Operand.Error("Function TINV parameter error!");
        }
        try {
            return Operand.Create(ExcelFunctions.TInv(probability, degreesFreedom));
        } catch (final Exception e) {
        }
        return Operand.Error("Function TINV parameter error!");
    }

    public Operand visitWEIBULL_fun(final WEIBULL_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToNumber("Function WEIBULL parameter 1 error!");
        if (firstValue.IsError())
            return firstValue;
        final Operand secondValue = args.get(1).ToNumber("Function WEIBULL parameter 2 error!");
        if (secondValue.IsError())
            return secondValue;
        final Operand thirdValue = args.get(2).ToNumber("Function WEIBULL parameter 3 error!");
        if (thirdValue.IsError())
            return thirdValue;

        final Operand fourthValue = args.get(3).ToBoolean("Function WEIBULL parameter 4 error!");
        if (fourthValue.IsError())
            return fourthValue;

        final double x = firstValue.NumberValue();
        final double shape = secondValue.NumberValue();
        final double scale = thirdValue.NumberValue();
        final boolean state = fourthValue.BooleanValue();
        if (shape <= 0.0 || scale <= 0.0) {
            return Operand.Error("Function WEIBULL parameter error!");
        }

        return Operand.Create(ExcelFunctions.WEIBULL(x, shape, scale, state));
    }

    private int F_base_countif(final List<Double> dbs, double d) {
        int count = 0;
        d = round(d, 12);
        for (final double item : dbs) {
            if (round(item, 12) == d) {
                count++;
            }
        }
        return count;
    }

    private int F_base_countif(final List<Double> dbs, final String s) {
        final Matcher m = sumifRegex.matcher(s);
        final Double d = Double.parseDouble(m.group(2));
        int count = 0;

        for (final double item : dbs) {
            if (F_base_compare(item, d, s)) {
                count++;
            }
        }
        return count;
    }

    private double F_base_sumif(final List<Double> dbs, final String s, final List<Double> sumdbs) {
        final Matcher m = sumifRegex.matcher(s);
        final Double d = Double.parseDouble(m.group(2));
        double sum = 0;

        for (int i = 0; i < dbs.size(); i++) {
            if (F_base_compare(dbs.get(i), d, s)) {
                sum += sumdbs.get(i);
            }
        }
        return sum;
    }

    private boolean F_base_compare(final double a, final double b, final String ss) {
        if (ss.equals("<")) {
            return round(a - b, 12) < 0;
        } else if (ss.equals("<=")) {
            return round(a - b, 12) <= 0;
        } else if (ss.equals(">")) {
            return round(a - b, 12) > 0;
        } else if (ss.equals(">=")) {
            return round(a - b, 12) >= 0;
        } else if (ss.equals("=") || ss.equals("==")) {
            return round(a - b, 12) == 0;
        }
        return round(a - b, 12) != 0;
    }

    private boolean F_base_GetList_1(final List<Operand> args, final List<Double> list) {
        for (final Operand item : args) {
            if (item.Type() == OperandType.NUMBER) {
                list.add(item.NumberValue());
            } else if (item.Type() == OperandType.ARRARY) {
                final boolean o = F_base_GetList_1(item.ArrayValue(), list);
                if (o == false) {
                    return false;
                }
            } else if (item.Type() == OperandType.JSON) {
                final Operand i = item.ToArray(null);
                if (i.IsError()) {
                    return false;
                }
                final boolean o = F_base_GetList_1(i.ArrayValue(), list);
                if (o == false) {
                    return false;
                }
            } else {
                final Operand o = item.ToNumber(null);
                if (o.IsError()) {
                    return false;
                }
                list.add(o.NumberValue());
            }
        }
        return true;
    }

    private boolean F_base_GetList_2(final Operand args, final List<Double> list) {
        if (args.IsError()) {
            return false;
        }
        if (args.Type() == OperandType.NUMBER) {
            list.add(args.NumberValue());
        } else if (args.Type() == OperandType.ARRARY) {
            final boolean o = F_base_GetList_1(args.ArrayValue(), list);
            if (o == false) {
                return false;
            }
        } else if (args.Type() == OperandType.JSON) {
            final Operand i = args.ToArray(null);
            if (i.IsError()) {
                return false;
            }
            final boolean o = F_base_GetList_1(i.ArrayValue(), list);
            if (o == false) {
                return false;
            }
        } else {
            final Operand o = args.ToNumber(null);
            if (o.IsError()) {
                return false;
            }
            list.add(o.NumberValue());
        }
        return true;
    }

    private boolean F_base_GetList(final Operand args, final List<String> list) {
        if (args.IsError()) {
            return false;
        }
        if (args.Type() == OperandType.ARRARY) {
            final boolean o = F_base_GetList(args.ArrayValue(), list);
            if (o == false) {
                return false;
            }
        } else if (args.Type() == OperandType.JSON) {
            final Operand i = args.ToArray(null);
            if (i.IsError()) {
                return false;
            }
            final boolean o = F_base_GetList(i.ArrayValue(), list);
            if (o == false) {
                return false;
            }
        } else {
            final Operand o = args.ToText(null);
            if (o.IsError()) {
                return false;
            }
            list.add(o.TextValue());
        }
        return true;
    }

    private boolean F_base_GetList(final List<Operand> args, final List<String> list) {
        for (final Operand item : args) {
            if (item.Type() == OperandType.ARRARY) {
                final boolean o = F_base_GetList(item.ArrayValue(), list);
                if (o == false) {
                    return false;
                }
            } else if (item.Type() == OperandType.JSON) {
                final Operand i = item.ToArray(null);
                if (i.IsError()) {
                    return false;
                }
                final boolean o = F_base_GetList(i.ArrayValue(), list);
                if (o == false) {
                    return false;
                }
            } else {
                final Operand o = item.ToText(null);
                if (o.IsError()) {
                    return false;
                }
                list.add(o.TextValue());
            }
        }
        return true;
    }

    public Operand visitURLENCODE_fun(final URLENCODE_funContext context) {
        final Operand firstValue = visit(context.expr()).ToText("Function URLENCODE parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        try {
            return Operand.Create(URLEncoder.encode(firstValue.TextValue(),"GB18030"));
        } catch (Exception e) {
        }
        return Operand.Error("Function URLENCODE is error!");
    }

    public Operand visitURLDECODE_fun(final URLDECODE_funContext context) {
        final Operand firstValue = visit(context.expr()).ToText("Function URLDECODE parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        try {
            return Operand.Create(URLDecoder.decode(firstValue.TextValue(),"GB18030"));
        } catch (Exception e) {
        }
        return Operand.Error("Function URLDECODE is error!");
    }

    public Operand visitHTMLENCODE_fun(final HTMLENCODE_funContext context) {
        final Operand firstValue = visit(context.expr()).ToText("Function HTMLENCODE parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(StringEscapeUtils.unescapeHtml4(firstValue.TextValue()));
    }

    public Operand visitHTMLDECODE_fun(final HTMLDECODE_funContext context) {
        final Operand firstValue = visit(context.expr()).ToText("Function HTMLDECODE parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(StringEscapeUtils.escapeHtml4(firstValue.TextValue()));
    }

    public Operand visitBASE64TOTEXT_fun(final BASE64TOTEXT_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand a = visit(item).ToText("Function BASE64TOTEXT parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        String encoding;
        if (args.size() == 1) {
            encoding = "utf-8";
        } else {
            encoding = args.get(1).TextValue();
        }
        try {
            final String t = new String(FromBase64String(args.get(0).TextValue()), encoding);
            return Operand.Create(t);
        } catch (final Exception e) {
        }
        return Operand.Error("Function BASE64TOTEXT is error!");
    }

    public Operand visitBASE64URLTOTEXT_fun(final BASE64URLTOTEXT_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand a = visit(item).ToText("Function BASE64URLTOTEXT parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        String encoding;
        if (args.size() == 1) {
            encoding = "utf-8";
        } else {
            encoding = args.get(1).TextValue();
        }
        try {
            final String t = new String(FromBase64ForUrlString(args.get(0).TextValue()), encoding);
            return Operand.Create(t);
        } catch (final Exception e) {
        }
        return Operand.Error("Function BASE64URLTOTEXT is error!");
    }

    public Operand visitTEXTTOBASE64_fun(final TEXTTOBASE64_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand a = visit(item).ToText("Function TEXTTOBASE64 parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        String encoding;
        if (args.size() == 1) {
            encoding = "utf-8";
        } else {
            encoding = args.get(1).TextValue();
        }
        try {
            final String t = ToBase64String(args.get(0).TextValue().getBytes(encoding));
            return Operand.Create(t);
        } catch (final Exception e) {
        }
        return Operand.Error("Function TEXTTOBASE64 is error!");
    }

    public Operand visitTEXTTOBASE64URL_fun(final TEXTTOBASE64URL_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand a = visit(item).ToText("Function TEXTTOBASE64URL parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        String encoding;
        if (args.size() == 1) {
            encoding = "utf-8";
        } else {
            encoding = args.get(1).TextValue();
        }
        try {
            final String t = ToBase64ForUrlString(args.get(0).TextValue().getBytes(encoding));
            return Operand.Create(t);
        } catch (final Exception e) {
        }
        return Operand.Error("Function TEXTTOBASE64 is error!");
    }

    public Operand visitREGEX_fun(final REGEX_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToText("Function REGEX parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final Operand secondValue = args.get(1).ToText("Function REGEX parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        if (args.size() == 2) {
            final Matcher b = Pattern.compile(firstValue.TextValue()).matcher(secondValue.TextValue());
            if (b.find() == false) {
                return Operand.Error("Function REGEX is error!");
            }
            return Operand.Create(b.group(0));
        } else {
            // } else if (args.size() == 3) {
            final Matcher ms = Pattern.compile(firstValue.TextValue()).matcher(secondValue.TextValue());
            // Matches ms = Regex.Matches(firstValue.TextValue(), secondValue.TextValue());
            final Operand thirdValue = args.get(2).ToNumber("Function REGEX parameter 3 is error!");
            if (thirdValue.IsError()) {
                return thirdValue;
            }
            if (ms.groupCount() <= thirdValue.IntValue() - excelIndex) {
                return Operand.Error("Function REGEX is error!");
            }
            return Operand.Create(ms.group(thirdValue.IntValue() - excelIndex));
            // [thirdValue.IntValue() - excelIndex].Value);
            // } else {
            // Matcher ms =
            // Pattern.compile(firstValue.TextValue()).matcher(secondValue.TextValue());
            // // Matches ms = Regex.Matches(firstValue.TextValue(),
            // secondValue.TextValue());
            // Operand thirdValue = args.get(2).ToNumber("Function REGEX parameter 3 is
            // error!");
            // if (thirdValue.IsError()) {
            // return thirdValue;
            // }
            // if (ms.groupCount() <= thirdValue.IntValue() + excelIndex) {
            // return Operand.Error("Function REGEX parameter 3 is error!");
            // }
            // Operand fourthValue = args.get(3).ToNumber("Function REGEX parameter 4 is
            // error!");
            // if (fourthValue.IsError()) {
            // return fourthValue;
            // }
            // return Operand.Create(ms[thirdValue.IntValue() -
            // excelIndex].Groups[fourthValue.IntValue()].Value);
        }
    }

    public Operand visitREGEXREPALCE_fun(final REGEXREPALCE_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand a = visit(item).ToText("Function REGEXREPALCE parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        final Matcher b = Pattern.compile(args.get(0).TextValue()).matcher(args.get(1).TextValue());// .Replace(args.get(0).TextValue(),
                                                                                                    // args.get(1).TextValue(),
                                                                                                    // args.get(2).TextValue());
        final String t = b.replaceAll(args.get(2).TextValue());
        return Operand.Create(t);
    }

    public Operand visitISREGEX_fun(final ISREGEX_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand a = visit(item).ToText("Function ISREGEX parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        final boolean b = Pattern.compile(args.get(0).TextValue()).matcher(args.get(1).TextValue()).find();
        return Operand.Create(b);
    }

    public Operand visitGUID_fun(final GUID_funContext context) {
        return Operand.Create(UUID.randomUUID().toString().replaceAll("-", ""));
    }

    public Operand visitMD5_fun(final MD5_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand a = visit(item).ToText("Function MD5 parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        String encoding;
        if (args.size() == 1) {
            encoding = "utf-8";
        } else {
            encoding = args.get(1).TextValue();
        }
        try {
            final String t = Hash.GetMd5String(args.get(0).TextValue().getBytes(encoding));
            return Operand.Create(t);
        } catch (final Exception e) {
        }
        return Operand.Error("Function MD5 is error!");
    }

    public Operand visitSHA1_fun(final SHA1_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand a = visit(item).ToText("Function SHA1 parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        String encoding;
        if (args.size() == 1) {
            encoding = "utf-8";
        } else {
            encoding = args.get(1).TextValue();
        }
        try {
            final String t = Hash.GetSha1String(args.get(0).TextValue().getBytes(encoding));
            return Operand.Create(t);
        } catch (final Exception e) {
        }
        return Operand.Error("Function SHA1 is error!");
    }

    public Operand visitSHA256_fun(final SHA256_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand a = visit(item).ToText("Function SHA256 parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        String encoding;
        if (args.size() == 1) {
            encoding = "utf-8";
        } else {
            encoding = args.get(1).TextValue();
        }
        try {
            final String t = Hash.GetSha256String(args.get(0).TextValue().getBytes(encoding));
            return Operand.Create(t);
        } catch (final Exception e) {
        }
        return Operand.Error("Function SHA256 is error!");
    }

    public Operand visitSHA512_fun(final SHA512_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand a = visit(item).ToText("Function SHA512 parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        String encoding;
        if (args.size() == 1) {
            encoding = "utf-8";
        } else {
            encoding = args.get(1).TextValue();
        }
        try {
            final String t = Hash.GetSha512String(args.get(0).TextValue().getBytes(encoding));
            return Operand.Create(t);
        } catch (final Exception e) {
        }
        return Operand.Error("Function SHA512 is error!");
    }

    public Operand visitCRC8_fun(final CRC8_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand a = visit(item).ToText("Function CRC8 parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        String encoding;
        if (args.size() == 1) {
            encoding = "utf-8";
        } else {
            encoding = args.get(1).TextValue();
        }
        try {
            final String t = Hash.GetCrc8String(args.get(0).TextValue().getBytes(encoding));
            return Operand.Create(t);
        } catch (final Exception e) {
        }
        return Operand.Error("Function CRC8 is error!");
    }

    public Operand visitCRC16_fun(final CRC16_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand a = visit(item).ToText("Function CRC16 parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        String encoding;
        if (args.size() == 1) {
            encoding = "utf-8";
        } else {
            encoding = args.get(1).TextValue();
        }
        try {
            final String t = Hash.GetCrc16String(args.get(0).TextValue().getBytes(encoding));
            return Operand.Create(t);
        } catch (final Exception e) {
        }
        return Operand.Error("Function CRC16 is error!");
    }

    public Operand visitCRC32_fun(final CRC32_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand a = visit(item).ToText("Function CRC32 parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        String encoding;
        if (args.size() == 1) {
            encoding = "utf-8";
        } else {
            encoding = args.get(1).TextValue();
        }
        try {
            final String t = Hash.GetCrc32String(args.get(0).TextValue().getBytes(encoding));
            return Operand.Create(t);
        } catch (final Exception e) {
        }
        return Operand.Error("Function CRC32 is error!");
    }

    public Operand visitHMACMD5_fun(final HMACMD5_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand a = visit(item).ToText("Function HMACMD5 parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        String encoding;
        if (args.size() == 2) {
            encoding = "utf-8";
        } else {
            encoding = args.get(2).TextValue();
        }
        try {
            final String t = Hash.GetHmacMd5String(args.get(0).TextValue().getBytes(encoding), args.get(1).TextValue());
            return Operand.Create(t);
        } catch (final Exception e) {
        }
        return Operand.Error("Function HMACMD5 is error!");
    }

    public Operand visitHMACSHA1_fun(final HMACSHA1_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand a = visit(item).ToText("Function HMACSHA1 parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        String encoding;
        if (args.size() == 2) {
            encoding = "utf-8";
        } else {
            encoding = args.get(2).TextValue();
        }
        try {
            final String t = Hash.GetHmacSha1String(args.get(0).TextValue().getBytes(encoding),
                    args.get(1).TextValue());
            return Operand.Create(t);
        } catch (final Exception e) {
        }
        return Operand.Error("Function HMACSHA1 is error!");
    }

    public Operand visitHMACSHA256_fun(final HMACSHA256_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand a = visit(item).ToText("Function HMACSHA256 parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        String encoding;
        if (args.size() == 2) {
            encoding = "utf-8";
        } else {
            encoding = args.get(2).TextValue();
        }
        try {
            final String t = Hash.GetHmacSha256String(args.get(0).TextValue().getBytes(encoding),
                    args.get(1).TextValue());
            return Operand.Create(t);
        } catch (final Exception e) {
        }
        return Operand.Error("Function HMACSHA256 is error!");
    }

    public Operand visitHMACSHA512_fun(final HMACSHA512_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand a = visit(item).ToText("Function HMACSHA512 parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        String encoding;
        if (args.size() == 2) {
            encoding = "utf-8";
        } else {
            encoding = args.get(2).TextValue();
        }
        try {
            final String t = Hash.GetHmacSha512String(args.get(0).TextValue().getBytes(encoding),
                    args.get(1).TextValue());
            return Operand.Create(t);
        } catch (final Exception e) {
        }
        return Operand.Error("Function HMACSHA512 is error!");
    }

    public Operand visitTRIMSTART_fun(final TRIMSTART_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand a = visit(item).ToText("Function TRIMSTART parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        String text = args.get(0).TextValue();
        if (args.size() == 2) {
            if (text.startsWith(args.get(1).TextValue())) {
                text = text.replace("^[" + args.get(1).TextValue() + "]*", "");
                return Operand.Create(text);
            }
            return Operand.Create(text);
        }
        text = text.replaceAll("\\s+$", "");
        return Operand.Create(text);
    }

    public Operand visitTRIMEND_fun(final TRIMEND_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand a = visit(item).ToText("Function TRIMEND parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        String text = args.get(0).TextValue();
        if (args.size() == 2) {
            text = text.replace("[" + args.get(1).TextValue() + "]*$", "");
            return Operand.Create(text);
        }
        text = text.replace("\\s*$", "");
        return Operand.Create(text);
    }

    public Operand visitINDEXOF_fun(final INDEXOF_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToText("Function INDEXOF parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final Operand secondValue = args.get(1).ToText("Function INDEXOF parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        final String text = firstValue.TextValue();
        if (args.size() == 2) {
            return Operand.Create(text.indexOf(secondValue.TextValue()) + excelIndex);
        }
        final Operand thirdValue = args.get(2).ToText("Function INDEXOF parameter 3 is error!");
        if (thirdValue.IsError()) {
            return thirdValue;
        }
        if (args.size() == 3) {
            return Operand.Create(text.substring(thirdValue.IntValue(), text.length()).indexOf(secondValue.TextValue())
                    + text.length() + excelIndex);
        }
        final Operand fourthValue = args.get(3).ToText("Function INDEXOF parameter 4 is error!");
        if (fourthValue.IsError()) {
            return fourthValue;
        }
        return Operand.Create(text.substring(thirdValue.IntValue(), text.length()).indexOf(secondValue.TextValue(),
                fourthValue.IntValue()) + thirdValue.IntValue() + excelIndex);
    }

    public Operand visitLASTINDEXOF_fun(final LASTINDEXOF_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToText("Function LASTINDEXOF parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final Operand secondValue = args.get(1).ToText("Function LASTINDEXOF parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        final String text = firstValue.TextValue();
        if (args.size() == 2) {
            return Operand.Create(text.lastIndexOf(secondValue.TextValue()) + excelIndex);
        }
        final Operand thirdValue = args.get(2).ToText("Function LASTINDEXOF parameter 3 is error!");
        if (thirdValue.IsError()) {
            return thirdValue;
        }
        if (args.size() == 3) {
            return Operand
                    .Create(text.substring(thirdValue.IntValue(), text.length()).lastIndexOf(secondValue.TextValue())
                            + thirdValue.IntValue() + excelIndex);
        }
        final Operand fourthValue = args.get(3).ToText("Function LASTINDEXOF parameter 4 is error!");
        if (fourthValue.IsError()) {
            return fourthValue;
        }
        return Operand.Create(text.substring(thirdValue.IntValue(), text.length()).lastIndexOf(secondValue.TextValue(),
                fourthValue.IntValue()) + thirdValue.IntValue() + excelIndex);
    }

    public Operand visitSPLIT_fun(final SPLIT_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (final ExprContext item : context.expr()) {
            final Operand a = visit(item).ToText("Function SPLIT parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }
        final String[] txts = args.get(0).TextValue().split(args.get(1).TextValue());
        final List<Operand> array = new ArrayList<Operand>();
        for (int i = 0; i < txts.length; i++) {
            array.add(Operand.Create(txts[i]));
        }
        return Operand.Create(array);
    }

    public Operand visitJOIN_fun(final JOIN_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0);
        if (firstValue.Type() == OperandType.JSON) {
            final Operand o = firstValue.ToArray(null);
            if (o.IsError() == false) {
                firstValue = o;
            }
        }
        if (firstValue.Type() == OperandType.ARRARY) {
            final List<String> list = new ArrayList<String>();
            final boolean o = F_base_GetList(firstValue, list);
            if (o == false)
                return Operand.Error("Function JOIN parameter 1 is error!");

            final Operand secondValue = args.get(1).ToText("Function JOIN parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }

            return Operand.Create(String.join(secondValue.TextValue(), list));
        } else {
            firstValue = firstValue.ToText("Function JOIN parameter 1 is error!");
            if (firstValue.IsError()) {
                return firstValue;
            }

            final List<String> list = new ArrayList<String>();
            for (int i = 1; i < args.size(); i++) {
                final boolean o = F_base_GetList(args.get(i), list);
                if (o == false)
                    return Operand.Error("Function JOIN parameter {i + 1} is error!");
            }

            return Operand.Create(String.join(firstValue.TextValue(), list));
        }
    }

    public Operand visitSUBSTRING_fun(final SUBSTRING_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToText("Function SUBSTRING parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final Operand secondValue = args.get(1).ToNumber("Function SUBSTRING parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        final String text = firstValue.TextValue();
        if (args.size() == 2) {
            return Operand.Create(text.substring(secondValue.IntValue() - excelIndex));
        }
        final Operand thirdValue = args.get(2).ToNumber("Function SUBSTRING parameter 3 is error!");
        if (thirdValue.IsError()) {
            return thirdValue;
        }
        return Operand.Create(text.substring(secondValue.IntValue() - excelIndex, thirdValue.IntValue()));
    }

    public Operand visitSTARTSWITH_fun(final STARTSWITH_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToText("Function STARTSWITH parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final Operand secondValue = args.get(1).ToText("Function STARTSWITH parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        final String text = firstValue.TextValue();
        if (args.size() == 2) {
            return Operand.Create(text.startsWith(secondValue.TextValue()));
        }
        final Operand thirdValue = args.get(2).ToBoolean("Function STARTSWITH parameter 3 is error!");
        if (thirdValue.IsError()) {
            return thirdValue;
        }
        if (thirdValue.BooleanValue()) {
            return Operand.Create(text.toLowerCase().startsWith(secondValue.TextValue().toLowerCase()));
        }
        return Operand.Create(text.startsWith(secondValue.TextValue()));
    }

    public Operand visitENDSWITH_fun(final ENDSWITH_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToText("Function ENDSWITH parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final Operand secondValue = args.get(1).ToText("Function ENDSWITH parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        final String text = firstValue.TextValue();
        if (args.size() == 2) {
            return Operand.Create(text.endsWith(secondValue.TextValue()));
        }
        final Operand thirdValue = args.get(2).ToBoolean("Function ENDSWITH parameter 3 is error!");
        ;
        if (thirdValue.IsError()) {
            return thirdValue;
        }
        if (thirdValue.BooleanValue()) {
            return Operand.Create(text.toLowerCase().endsWith(secondValue.TextValue().toLowerCase()));
        }
        return Operand.Create(text.endsWith(secondValue.TextValue()));
    }

    public Operand visitISNULLOREMPTY_fun(final ISNULLOREMPTY_funContext context) {
        final Operand firstValue = visit(context.expr()).ToText("Function ISNULLOREMPTY parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        boolean b = false;
        if (firstValue.TextValue() == null || firstValue.TextValue().equals("")) {
            b = true;
        }
        return Operand.Create(b);
        // return Operand.Create(String.IsNullOrEmpty(firstValue.TextValue()));
    }

    public Operand visitISNULLORWHITESPACE_fun(final ISNULLORWHITESPACE_funContext context) {
        final Operand firstValue = visit(context.expr()).ToText("Function ISNULLORWHITESPACE parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        boolean b = false;
        if (firstValue.TextValue() == null || firstValue.TextValue().trim().equals("")) {
            b = true;
        }
        return Operand.Create(b);
    }

    public Operand visitREMOVESTART_fun(final REMOVESTART_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToText("Function REMOVESTART parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final Operand secondValue = args.get(1).ToText("Function REMOVESTART parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }
        final String text = firstValue.TextValue();
        if (args.size() == 3) {
            final Operand thirdValue = args.get(2).ToBoolean("Function REMOVESTART parameter 3 is error!");
            if (thirdValue.IsError()) {
                return thirdValue;
            }
            if (thirdValue.BooleanValue()) {
                if (text.toLowerCase().startsWith(secondValue.TextValue().toLowerCase())) {
                    return Operand.Create(text.substring(secondValue.TextValue().length()));
                }
                return firstValue;
            }
        }
        if (text.startsWith(secondValue.TextValue())) {
            return Operand.Create(text.substring(secondValue.TextValue().length()));
        }
        return firstValue;
    }

    public Operand visitREMOVEEND_fun(final REMOVEEND_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToText("Function REMOVEEND parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final Operand secondValue = args.get(1).ToText("Function REMOVEEND parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        final String text = firstValue.TextValue();
        // StringComparison comparison = StringComparison.Ordinal;
        if (args.size() == 3) {
            final Operand thirdValue = args.get(2).ToBoolean("Function REMOVESTART parameter 3 is error!");
            if (thirdValue.IsError()) {
                return thirdValue;
            }
            if (thirdValue.BooleanValue()) {
                if (text.toLowerCase().endsWith(secondValue.TextValue().toLowerCase())) {
                    return Operand.Create(text.substring(0, text.length() - secondValue.TextValue().length()));
                }
                return firstValue;
            }
        }
        if (text.endsWith(secondValue.TextValue())) {
            return Operand.Create(text.substring(0, text.length() - secondValue.TextValue().length()));
        }
        return firstValue;
    }

    public Operand visitJSON_fun(final JSON_funContext context) {
        final Operand firstValue = visit(context.expr()).ToText("Function JSON parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final String txt = firstValue.TextValue();
        if ((txt.startsWith("{") && txt.endsWith("}")) || (txt.startsWith("[") && txt.endsWith("]"))) {
            try {
                final JsonData json = JsonMapper.ToObject(txt);
                return Operand.Create(json);
            } catch (final Exception e) {
            }
        }
        return Operand.Error("Function JSON parameter is error!");
    }

    public Operand visitVLOOKUP_fun(final VLOOKUP_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToArray("Function VLOOKUP parameter 1 error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        Operand secondValue = args.get(1);

        final Operand thirdValue = args.get(2).ToNumber("Function VLOOKUP parameter 3 is error!");
        if (thirdValue.IsError()) {
            return thirdValue;
        }

        boolean vague = true;
        if (args.size() == 4) {
            final Operand fourthValue = args.get(2).ToBoolean("Function VLOOKUP parameter 4 is error!");
            if (fourthValue.IsError()) {
                return fourthValue;
            }
            vague = fourthValue.BooleanValue();
        }
        if (secondValue.Type() != OperandType.NULL) {
            final Operand sv = secondValue.ToText("Function VLOOKUP parameter 2 is error!");
            if (sv.IsError()) {
                return sv;
            }
            secondValue = sv;
        }
        for (final Operand item : firstValue.ArrayValue()) {
            final Operand o = item.ToArray("Function VLOOKUP parameter 1 error!");
            if (o.IsError()) {
                return o;
            }
            if (o.ArrayValue().size() > 0) {
                final Operand o1 = o.ArrayValue().get(0);// [0];
                int b = -1;
                if (secondValue.Type() == OperandType.NUMBER) {
                    final Operand o2 = o1.ToNumber(null);
                    if (o2.IsError() == false) {
                        b = Compare(o2.NumberValue(), secondValue.NumberValue());
                    }
                } else {
                    final Operand o2 = o1.ToText(null);
                    if (o2.IsError() == false) {
                        b = o2.TextValue().compareTo(secondValue.TextValue());
                        // b = String.CompareOrdinal(o2.TextValue(), secondValue.TextValue());
                    }
                }
                if (b == 0) {
                    final int index = thirdValue.IntValue() - excelIndex;
                    if (index < o.ArrayValue().size()) {
                        return o.ArrayValue().get(index);// [index];
                    }
                }
            }
        }

        if (vague) // 进行模糊匹配
        {
            Operand last = null;
            final int index = thirdValue.IntValue() - excelIndex;
            for (final Operand item : firstValue.ArrayValue()) {
                final Operand o = item.ToArray(null);
                if (o.IsError()) {
                    return o;
                }
                if (o.ArrayValue().size() > 0) {
                    final Operand o1 = o.ArrayValue().get(0);// [0];
                    int b = -1;
                    if (secondValue.Type() == OperandType.NUMBER) {
                        final Operand o2 = o1.ToNumber(null);
                        if (o2.IsError() == false) {
                            b = Compare(o2.NumberValue(), secondValue.NumberValue());
                        }
                    } else {
                        final Operand o2 = o1.ToText(null);
                        if (o2.IsError() == false) {
                            b = o2.TextValue().compareTo(secondValue.TextValue());
                            // b = String.CompareOrdinal(o2.TextValue(), secondValue.TextValue());
                        }
                    }
                    if (b < 0 && index < o.ArrayValue().size()) {
                        last = o;
                    } else if (b > 0 && last != null) {
                        return last.ArrayValue().get(index);// [index];
                    }
                }
            }
        }
        return Operand.Error("Function VLOOKUP is not match !");
    }

    public Operand visitLOOKUP_fun(final LOOKUP_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        final Operand firstValue = args.get(0).ToArray("Function LOOKUP parameter 1 error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        final Operand secondValue = args.get(1).ToText("Function LOOKUP parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }
        final Operand thirdValue = args.get(2).ToText("Function LOOKUP parameter 3 is error!");
        if (thirdValue.IsError()) {
            return thirdValue;
        }

        if (secondValue.TextValue() == null || secondValue.TextValue().equals("")) {
            return Operand.Error("Function LOOKUP parameter 2 is null!");
        }

        final LookupAlgorithmEngine engine = new LookupAlgorithmEngine();
        if (engine.Parse(secondValue.TextValue()) == false) {
            return Operand.Error("Function LOOKUP parameter 2 Parse is error!");
        }
        // Function<MyFunction, Operand> f=s->{
        // return DoDiyFunction(s);
        // }
        engine.DiyFunction = s -> {
            return DoDiyFunction(s);
        };

        for (final Operand item : firstValue.ArrayValue()) {
            final Operand json = item.ToJson(null);
            if (json.IsError() == false) {
                engine.Json = json;
                try {
                    final Operand o = engine.Evaluate().ToBoolean(null);
                    if (o.IsError() == false) {
                        if (o.BooleanValue()) {
                            final JsonData v = json.JsonValue().GetChild(thirdValue.TextValue());// [thirdValue.TextValue()];
                            if (v != null) {
                                if (v.IsString())
                                    return Operand.Create(v.StringValue());
                                if (v.IsBoolean())
                                    return Operand.Create(v.BooleanValue());
                                if (v.IsDouble())
                                    return Operand.Create(v.NumberValue());
                                if (v.IsObject())
                                    return Operand.Create(v);
                                if (v.IsArray())
                                    return Operand.Create(v);
                                if (v.IsNull())
                                    return Operand.CreateNull();
                                return Operand.Create(v);
                            }
                        }
                    }
                } catch (final Exception e) {
                }
            }
        }
        return Operand.Error("Function LOOKUP not find!");
    }

    private Operand DoDiyFunction(final MyFunction fun) {
        if (DiyFunction != null) {
            return DiyFunction.apply(fun);
        }
        return Operand.Error("Function name [" + fun.Name + "] is missing.");
    }

    public Operand visitArray_fun(final Array_funContext context) {
        final List<Operand> args = new ArrayList<Operand>();
        for (final ExprContext item : context.expr()) {
            final Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }
        return Operand.Create(args);
    }

    public Operand visitBracket_fun(final Bracket_funContext context) {
        return visit(context.expr());
    }

    public Operand visitNUM_fun(final NUM_funContext context) {
        final String t = context.NUM().getText();
        TerminalNode subNode=  context.SUB();
        if(subNode!=null){
            final String sub =subNode.getText();
            final Double d = Double.parseDouble(sub + t);
            return Operand.Create(d);
        }
        final Double d2 = Double.parseDouble( t);
        return Operand.Create(d2);
    }

    public Operand visitSTRING_fun(final STRING_funContext context) {
        final String opd = context.STRING().getText();
        final StringBuilder sb = new StringBuilder();
        int index = 1;
        while (index < opd.length() - 1) {
            final char c = opd.charAt(index++);// [index++];
            if (c == '\\') {
                final char c2 = opd.charAt(index++);
                if (c2 == 'n')
                    sb.append('\n');
                else if (c2 == 'r')
                    sb.append('\r');
                else if (c2 == 't')
                    sb.append('\t');
                else if (c2 == '0')
                    sb.append('\0');
                // else if (c2 == 'v') sb.append('\v');
                // else if (c2 == 'a') sb.append('\a');
                else if (c2 == 'b')
                    sb.append('\b');
                else if (c2 == 'f')
                    sb.append('\f');
                else
                    sb.append(opd.charAt(index++));
            } else {
                sb.append(c);
            }
        }
        return Operand.Create(sb.toString());
    }

    public Operand visitNULL_fun(final NULL_funContext context) {
        return Operand.CreateNull();
    }

    public Operand visitPARAMETER_fun(final PARAMETER_funContext context) {
        final Operand p = visit(context.parameter()).ToText("Function PARAMETER first parameter is error!");
        if (p.IsError())
            return p;

        if (GetParameter != null) {
            return GetParameter.apply(p.TextValue());
        }
        return Operand.Error("Function PARAMETER first parameter is error!");
    }

    public Operand visitParameter(final ParameterContext context) {
        final ExprContext expr = context.expr();
        if (expr != null) {
            return visit(expr);
        }
        return visit(context.parameter2());
    }

    public Operand visitParameter2(final Parameter2Context context) {
        return Operand.Create(context.children.get(0).getText());
    }

    public Operand visitGetJsonValue_fun(final GetJsonValue_funContext context) {
        final Operand firstValue = visit(context.expr());
        if (firstValue.IsError()) {
            return firstValue;
        }

        final Operand obj = firstValue;
        Operand op;
        final ParameterContext p1 = context.parameter();
        if (p1 != null) {
            op = visit(p1);
        } else {
            op = visit(context.parameter2());
        }

        if (obj.Type() == OperandType.ARRARY) {
            op = op.ToNumber("ARRARY index is error!");
            if (op.IsError()) {
                return op;
            }
            final int index = op.IntValue() - excelIndex;
            if (index < obj.ArrayValue().size())
                return obj.ArrayValue().get(index);// [index];
            return Operand.Error("ARRARY index " + index + " greater than maximum length!");
        }
        if (obj.Type() == OperandType.JSON) {
            final JsonData json = obj.JsonValue();
            if (json.IsArray()) {
                op = op.ToNumber("JSON parameter index is error!");
                if (op.IsError()) {
                    return op;
                }
                final int index = op.IntValue() - excelIndex;
                if (index < json.inst_array.size()) {
                    final JsonData v = json.GetChild(index);// [op.IntValue() - excelIndex];
                    if (v.IsString())
                        return Operand.Create(v.StringValue());
                    if (v.IsBoolean())
                        return Operand.Create(v.BooleanValue());
                    if (v.IsDouble())
                        return Operand.Create(v.NumberValue());
                    if (v.IsObject())
                        return Operand.Create(v);
                    if (v.IsArray())
                        return Operand.Create(v);
                    if (v.IsNull())
                        return Operand.CreateNull();
                    return Operand.Create(v);
                }
                return Operand.Error("JSON index " + index + " greater than maximum length!");
            } else {
                op = op.ToText("JSON parameter name is error!");
                if (op.IsError()) {
                    return op;
                }
                final JsonData v = json.GetChild(op.TextValue());// [op.TextValue()];
                if (v != null) {
                    if (v.IsString())
                        return Operand.Create(v.StringValue());
                    if (v.IsBoolean())
                        return Operand.Create(v.BooleanValue());
                    if (v.IsDouble())
                        return Operand.Create(v.NumberValue());
                    if (v.IsObject())
                        return Operand.Create(v);
                    if (v.IsArray())
                        return Operand.Create(v);
                    if (v.IsNull())
                        return Operand.CreateNull();
                    return Operand.Create(v);
                }
            }
        }
        return Operand.Error(" Operator is error!");
    }

    public Operand visitExpr2_fun(final Expr2_funContext context) {
        return visitChildren(context);
    }

    public Operand visitDiyFunction_fun(final DiyFunction_funContext context) {
        if (DiyFunction != null) {
            final String funName = context.PARAMETER().getText();
            final List<Operand> args = new ArrayList<Operand>();
            for (final ExprContext item : context.expr()) {
                final Operand aa = visit(item);
                args.add(aa);
            }
            final MyFunction myFunction = new MyFunction();
            myFunction.Name = funName;
            myFunction.OperandList = args;
            return DiyFunction.apply(myFunction);
        }
        return Operand.Error("DiyFunction is error!");
    }

    @SuppressWarnings("deprecation")
    private double round(final double value, final int p) {
        final BigDecimal bigD = new BigDecimal(value);
        return bigD.setScale(p, BigDecimal.ROUND_HALF_UP).doubleValue();
    }

    static int sign(final double a) {
        if (a == 0.0) {
            return 0;
        }
        if (a < 0) {
            return -1;
        }
        return 1;
    }

    private double Sum(final List<Double> array) {
        double d = 0;
        for (int i = 0; i < array.size(); i++) {
            d += array.get(i);
        }
        return d;
    }

    private double Average(final List<Double> array) {
        double d = 0;
        for (int i = 0; i < array.size(); i++) {
            d += array.get(i);
        }
        return d / array.size();
    }

    private double Max(final List<Double> array) {
        double d = Double.MIN_VALUE;
        for (int i = 0; i < array.size(); i++) {
            if (d < array.get(i)) {
                d = array.get(i);
            }
        }
        return d;
    }

    private double Min(final List<Double> array) {
        double d = Double.MAX_VALUE;
        for (int i = 0; i < array.size(); i++) {
            if (d > array.get(i)) {
                d = array.get(i);
            }
        }
        return d;
    }

    private String padLeft(final String src, final int len, final char ch) {
        final int diff = len - src.length();
        if (diff <= 0) {
            return src;
        }

        final char[] charr = new char[len];
        System.arraycopy(src.toCharArray(), 0, charr, 0, src.length());
        for (int i = src.length(); i < len; i++) {
            charr[i] = ch;
        }
        return new String(charr);
    }

    // private String padRight(final String src, final int len, final char ch) {
    //     final int diff = len - src.length();
    //     if (diff <= 0) {
    //         return src;
    //     }

    //     final char[] charr = new char[len];
    //     System.arraycopy(src.toCharArray(), 0, charr, diff, src.length());
    //     for (int i = 0; i < diff; i++) {
    //         charr[i] = ch;
    //     }
    //     return new String(charr);
    // }

    private String ToBase64String(final byte[] input) {
        return Base64Util.encode(input);
    }

    private byte[] FromBase64String(final String base64) {
        return Base64Util.decode(base64);
    }

    private String ToBase64ForUrlString(final byte[] input) {
        return ToBase64String(input).replace("=*$", "").replace("\\+", "-").replace("/", "-");
    }

    private byte[] FromBase64ForUrlString(final String base64ForUrlInput) {
        final String base64 = "===========================================+=+=/0123456789=======ABCDEFGHIJKLMNOPQRSTUVWXYZ====/=abcdefghijklmnopqrstuvwxyz=====";
        final StringBuilder sb = new StringBuilder();
        for (int i = 0; i < base64ForUrlInput.length(); i++) {
            final char c = base64ForUrlInput.charAt(i);
            if ((int) c >= 128)
                continue;
            final char k = base64.charAt(c);// [c];
            if (k == '=')
                continue;
            sb.append(k);
        }
        final int len = sb.length();
        final int padChars = (len % 4) == 0 ? 0 : (4 - (len % 4));
        for (int i = 0; i < padChars; i++) {
            sb.append('=');
        }
        return FromBase64String(sb.toString());
    }

    private double log(final double value, final double base) {
        return Math.log(value) / Math.log(base);
    }
    
    public static class Base64Util {
        private static final int BASELENGTH = 128;
        private static final int LOOKUPLENGTH = 64;
        private static final int TWENTYFOURBITGROUP = 24;
        private static final int EIGHTBIT = 8;
        private static final int SIXTEENBIT = 16;
        private static final int FOURBYTE = 4;
        private static final int SIGN = -128;
        private static char PAD = '=';
        private static byte[] base64Alphabet = new byte[BASELENGTH];
        private static char[] lookUpBase64Alphabet = new char[LOOKUPLENGTH];
        static {
            for (int i = 0; i < BASELENGTH; ++i) {
                base64Alphabet[i] = -1;
            }
            for (int i = 'Z'; i >= 'A'; i--) {
                base64Alphabet[i] = (byte) (i - 'A');
            }
            for (int i = 'z'; i >= 'a'; i--) {
                base64Alphabet[i] = (byte) (i - 'a' + 26);
            }
            for (int i = '9'; i >= '0'; i--) {
                base64Alphabet[i] = (byte) (i - '0' + 52);
            }
            base64Alphabet['+'] = 62;
            base64Alphabet['/'] = 63;
            for (int i = 0; i <= 25; i++) {
                lookUpBase64Alphabet[i] = (char) ('A' + i);
            }
            for (int i = 26, j = 0; i <= 51; i++, j++) {
                lookUpBase64Alphabet[i] = (char) ('a' + j);
            }
            for (int i = 52, j = 0; i <= 61; i++, j++) {
                lookUpBase64Alphabet[i] = (char) ('0' + j);
            }
            lookUpBase64Alphabet[62] = (char) '+';
            lookUpBase64Alphabet[63] = (char) '/';
        }

        private static boolean isWhiteSpace(char octect) {
            return (octect == 0x20 || octect == 0xd || octect == 0xa || octect == 0x9);
        }

        private static boolean isPad(char octect) {
            return (octect == PAD);
        }

        private static boolean isData(char octect) {
            return (octect < BASELENGTH && base64Alphabet[octect] != -1);
        }

        public static String encode(byte[] binaryData) {
            if (binaryData == null) {
                return null;
            }
            int lengthDataBits = binaryData.length * EIGHTBIT;
            if (lengthDataBits == 0) {
                return "";
            }
            int fewerThan24bits = lengthDataBits % TWENTYFOURBITGROUP;
            int numberTriplets = lengthDataBits / TWENTYFOURBITGROUP;
            int numberQuartet = fewerThan24bits != 0 ? numberTriplets + 1 : numberTriplets;
            char encodedData[] = null;
            encodedData = new char[numberQuartet * 4];
            byte k = 0, l = 0, b1 = 0, b2 = 0, b3 = 0;
            int encodedIndex = 0;
            int dataIndex = 0;
            for (int i = 0; i < numberTriplets; i++) {
                b1 = binaryData[dataIndex++];
                b2 = binaryData[dataIndex++];
                b3 = binaryData[dataIndex++];
                l = (byte) (b2 & 0x0f);
                k = (byte) (b1 & 0x03);
                byte val1 = ((b1 & SIGN) == 0) ? (byte) (b1 >> 2) : (byte) ((b1) >> 2 ^ 0xc0);
                byte val2 = ((b2 & SIGN) == 0) ? (byte) (b2 >> 4) : (byte) ((b2) >> 4 ^ 0xf0);
                byte val3 = ((b3 & SIGN) == 0) ? (byte) (b3 >> 6) : (byte) ((b3) >> 6 ^ 0xfc);
                encodedData[encodedIndex++] = lookUpBase64Alphabet[val1];
                encodedData[encodedIndex++] = lookUpBase64Alphabet[val2 | (k << 4)];
                encodedData[encodedIndex++] = lookUpBase64Alphabet[(l << 2) | val3];
                encodedData[encodedIndex++] = lookUpBase64Alphabet[b3 & 0x3f];
            }
            // form integral number of 6-bit groups
            if (fewerThan24bits == EIGHTBIT) {
                b1 = binaryData[dataIndex];
                k = (byte) (b1 & 0x03);
                byte val1 = ((b1 & SIGN) == 0) ? (byte) (b1 >> 2) : (byte) ((b1) >> 2 ^ 0xc0);
                encodedData[encodedIndex++] = lookUpBase64Alphabet[val1];
                encodedData[encodedIndex++] = lookUpBase64Alphabet[k << 4];
                encodedData[encodedIndex++] = PAD;
                encodedData[encodedIndex++] = PAD;
            } else if (fewerThan24bits == SIXTEENBIT) {
                b1 = binaryData[dataIndex];
                b2 = binaryData[dataIndex + 1];
                l = (byte) (b2 & 0x0f);
                k = (byte) (b1 & 0x03);
                byte val1 = ((b1 & SIGN) == 0) ? (byte) (b1 >> 2) : (byte) ((b1) >> 2 ^ 0xc0);
                byte val2 = ((b2 & SIGN) == 0) ? (byte) (b2 >> 4) : (byte) ((b2) >> 4 ^ 0xf0);
                encodedData[encodedIndex++] = lookUpBase64Alphabet[val1];
                encodedData[encodedIndex++] = lookUpBase64Alphabet[val2 | (k << 4)];
                encodedData[encodedIndex++] = lookUpBase64Alphabet[l << 2];
                encodedData[encodedIndex++] = PAD;
            }
            return new String(encodedData);
        }

        public static byte[] decode(String encoded) {
            if (encoded == null) {
                return null;
            }
            char[] base64Data = encoded.toCharArray();
            // remove white spaces
            int len = removeWhiteSpace(base64Data);
            if (len % FOURBYTE != 0) {
                return null;// should be divisible by four
            }
            int numberQuadruple = (len / FOURBYTE);
            if (numberQuadruple == 0) {
                return new byte[0];
            }
            byte decodedData[] = null;
            byte b1 = 0, b2 = 0, b3 = 0, b4 = 0;
            char d1 = 0, d2 = 0, d3 = 0, d4 = 0;
            int i = 0;
            int encodedIndex = 0;
            int dataIndex = 0;
            decodedData = new byte[(numberQuadruple) * 3];
            for (; i < numberQuadruple - 1; i++) {
                if (!isData((d1 = base64Data[dataIndex++])) || !isData((d2 = base64Data[dataIndex++]))
                        || !isData((d3 = base64Data[dataIndex++])) || !isData((d4 = base64Data[dataIndex++]))) {
                    return null;
                } // if found "no data" just return null
                b1 = base64Alphabet[d1];
                b2 = base64Alphabet[d2];
                b3 = base64Alphabet[d3];
                b4 = base64Alphabet[d4];
                decodedData[encodedIndex++] = (byte) (b1 << 2 | b2 >> 4);
                decodedData[encodedIndex++] = (byte) (((b2 & 0xf) << 4) | ((b3 >> 2) & 0xf));
                decodedData[encodedIndex++] = (byte) (b3 << 6 | b4);
            }
            if (!isData((d1 = base64Data[dataIndex++])) || !isData((d2 = base64Data[dataIndex++]))) {
                return null;// if found "no data" just return null
            }
            b1 = base64Alphabet[d1];
            b2 = base64Alphabet[d2];
            d3 = base64Data[dataIndex++];
            d4 = base64Data[dataIndex++];
            if (!isData((d3)) || !isData((d4))) {// Check if they are PAD characters
                if (isPad(d3) && isPad(d4)) {
                    if ((b2 & 0xf) != 0)// last 4 bits should be zero
                    {
                        return null;
                    }
                    byte[] tmp = new byte[i * 3 + 1];
                    System.arraycopy(decodedData, 0, tmp, 0, i * 3);
                    tmp[encodedIndex] = (byte) (b1 << 2 | b2 >> 4);
                    return tmp;
                } else if (!isPad(d3) && isPad(d4)) {
                    b3 = base64Alphabet[d3];
                    if ((b3 & 0x3) != 0)// last 2 bits should be zero
                    {
                        return null;
                    }
                    byte[] tmp = new byte[i * 3 + 2];
                    System.arraycopy(decodedData, 0, tmp, 0, i * 3);
                    tmp[encodedIndex++] = (byte) (b1 << 2 | b2 >> 4);
                    tmp[encodedIndex] = (byte) (((b2 & 0xf) << 4) | ((b3 >> 2) & 0xf));
                    return tmp;
                } else {
                    return null;
                }
            } else { // No PAD e.g 3cQl
                b3 = base64Alphabet[d3];
                b4 = base64Alphabet[d4];
                decodedData[encodedIndex++] = (byte) (b1 << 2 | b2 >> 4);
                decodedData[encodedIndex++] = (byte) (((b2 & 0xf) << 4) | ((b3 >> 2) & 0xf));
                decodedData[encodedIndex++] = (byte) (b3 << 6 | b4);
            }
            return decodedData;
        }

        /**
         * remove WhiteSpace from MIME containing encoded Base64Util data.
         * 
         * @param data the byte array of base64 data (with WS)
         * @return the new length
         */
        private static int removeWhiteSpace(char[] data) {
            if (data == null) {
                return 0;
            }
            // count characters that's not whitespace
            int newSize = 0;
            int len = data.length;
            for (int i = 0; i < len; i++) {
                if (!isWhiteSpace(data[i])) {
                    data[newSize++] = data[i];
                }
            }
            return newSize;
        }
    }
}