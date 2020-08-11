package toolgood.algorithm.internals;

import java.math.BigDecimal;
import java.net.URLDecoder;
import java.net.URLEncoder;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.Random;
import java.util.UUID;
import java.util.function.DoubleToIntFunction;
import java.util.function.Function;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import org.antlr.v4.runtime.tree.AbstractParseTreeVisitor;
import org.springframework.util.StringUtils;

import toolgood.algorithm.MyDate;
import toolgood.algorithm.Operand;
import toolgood.algorithm.OperandType;
import toolgood.algorithm.litJson.JsonData;
import toolgood.algorithm.litJson.JsonMapper;
import toolgood.algorithm.math.mathParser;
import toolgood.algorithm.math.mathParser2;
import toolgood.algorithm.math.mathVisitor;
import toolgood.algorithm.math.mathParser2.*;
import toolgood.algorithm.mathNet.ExcelFunctions;
import toolgood.algorithm.internals.MyFunction;

public class MathVisitor extends AbstractParseTreeVisitor<Operand> implements mathVisitor<Operand> {
    private static Pattern sumifRegex = Pattern.compile("(<|<=|>|>=|=|==|!=|<>) *([-+]?\\d+(\\.(\\d+)?)?)");
    private static Pattern bit_2 = Pattern.compile("^[01]+");
    private static Pattern bit_8 = Pattern.compile("^[0-8]+");
    private static Pattern bit_16 = Pattern.compile("^[0-9a-fA-F]+");
    private static Pattern clearRegex = Pattern.compile("[\\f\\n\\r\\t\\v]");
    private static Pattern numberRegex = Pattern.compile("^-?(0|[1-9])\\d*(\\.\\d+)?");
    private static Locale cultureInfo = Locale.US;
    public Function<String, Operand> GetParameter;
    public Function<MyFunction, Operand> DiyFunction;
    public int excelIndex;

    public Operand visitProg(ProgContext context) {
        return visitChildren(context);
    }

    public Operand visitMulDiv_fun(MulDiv_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0);
        Operand secondValue = args.get(1);
        String t = context.op.getText();
        if (firstValue.Type() == OperandType.STRING) {
             if (numberRegex.matcher(firstValue.TextValue()).find()) {
                Operand a = firstValue.ToNumber(null);
                if (a.IsError() == false)
                    firstValue = a;
            } else {
                Operand a = firstValue.ToDate(null);
                if (a.IsError() == false)
                    firstValue = a;
            }
        }
        if (secondValue.Type() == OperandType.STRING) {
            if (numberRegex.matcher(secondValue.TextValue()).find()) {
                Operand a = secondValue.ToNumber(null);
                if (a.IsError() == false)
                    secondValue = a;
            } else {
                Operand a = secondValue.ToDate(null);
                if (a.IsError() == false)
                    secondValue = a;
            }
        }
        if (t == "*") {
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
                return Operand.Create((MyDate) (firstValue.DateValue() * secondValue.NumberValue()));
            }
            if (secondValue.Type() == OperandType.DATE) {
                firstValue = firstValue.ToNumber("Function '" + t + "' parameter 1 is error!");
                if (firstValue.IsError()) {
                    return firstValue;
                }
                return Operand.Create((MyDate) (secondValue.DateValue() * firstValue.NumberValue()));
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
        } else if (t == "/") {
            if (firstValue.Type() == OperandType.DATE) {
                return Operand.Create(firstValue.DateValue() / secondValue.NumberValue());
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
        } else if (t == "%") {
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

    public Operand visitAddSub_fun(AddSub_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0);
        Operand secondValue = args.get(1);
        String t = context.op.getText();

        if (t == "&") {
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
                Operand a = firstValue.ToNumber(null);
                if (a.IsError() == false)
                    firstValue = a;
            } else {
                Operand a = firstValue.ToDate(null);
                if (a.IsError() == false)
                    firstValue = a;
            }
        }
        if (secondValue.Type() == OperandType.STRING) {
            if (numberRegex.matcher(secondValue.TextValue()).find()) {
                Operand a = secondValue.ToNumber(null);
                if (a.IsError() == false)
                    secondValue = a;
            } else {
                Operand a = secondValue.ToDate(null);
                if (a.IsError() == false)
                    secondValue = a;
            }
        }
        if (t == "+") {
            if (firstValue.Type() == OperandType.DATE && secondValue.Type() == OperandType.DATE) {
                return Operand.Create(firstValue.DateValue() + secondValue.DateValue());
            } else if (firstValue.Type() == OperandType.DATE) {
                secondValue = secondValue.ToNumber("Function '" + t + "' parameter 2 is error!");
                if (secondValue.IsError()) {
                    return secondValue;
                }
                return Operand.Create(firstValue.DateValue() + secondValue.NumberValue());
            } else if (secondValue.Type() == OperandType.DATE) {
                firstValue = firstValue.ToNumber("Function '" + t + "' parameter 1 is error!");
                if (firstValue.IsError()) {
                    return firstValue;
                }
                return Operand.Create(secondValue.DateValue() + firstValue.NumberValue());
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
        } else if (t == "-") {
            if (firstValue.Type() == OperandType.DATE && secondValue.Type() == OperandType.DATE) {
                return Operand.Create(firstValue.DateValue() - secondValue.DateValue());
            } else if (firstValue.Type() == OperandType.DATE) {
                secondValue = secondValue.ToNumber("Function '" + t + "' parameter 2 is error!");
                if (secondValue.IsError()) {
                    return secondValue;
                }
                return Operand.Create(firstValue.DateValue() - secondValue.NumberValue());
            } else if (secondValue.Type() == OperandType.DATE) {
                firstValue = firstValue.ToNumber("Function '" + t + "' parameter 1 is error!");
                if (firstValue.IsError()) {
                    return firstValue;
                }
                return Operand.Create(secondValue.DateValue() - firstValue.NumberValue());
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

    public Operand visitJudge_fun(Judge_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0);
        Operand secondValue = args.get(1);
        String type = context.op.getText();

        if (firstValue.IsNull()) {
            if (secondValue.IsNull() && (type == "==" || type == "=")) {
                return Operand.True;
            } else if (secondValue.IsNull() == false && (type == "<>" || type == "!=")) {
                return Operand.True;
            }
            return Operand.False;
        } else if (secondValue.IsNull()) {
            if (type == "==" || type == "=") {
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

                r = String.CompareOrdinal(firstValue.TextValue(), secondValue.TextValue());
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

            r = String.CompareOrdinal(firstValue.TextValue(), secondValue.TextValue());
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

            r = String.Compare(firstValue.TextValue(), secondValue.TextValue(), true);
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

    private int Compare(double t1, double t2) {

        double b = round(t1 - t2, 12);
        if (b == 0) {
            return 0;
        } else if (b > 0) {
            return 1;
        }
        return -1;
    }

    public Operand visitAndOr_fun(AndOr_funContext context) {
        String t = context.op.getText();
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToBoolean("Function '" + t + "' parameter " + (index++) + " is error!");
            ;
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0);
        Operand secondValue = args.get(1);
        if (t == "&&" || t.toLowerCase().equals("and")) {
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

    public Operand visitIF_fun(IF_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand b = args.get(0).ToBoolean("Function IF first parameter is error!");
        if (b.IsError()) {
            return b;
        }

        if (b.BooleanValue()) {
            if (args.size()>1) {
                return args.get(1);
            }
            return Operand.True;
        }
        if (args.size() == 3) {
            return args.get(2);
        }
        return Operand.False;
    }

    public Operand visitIFERROR_fun(IFERROR_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand a = visit(item);
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

    public Operand visitISNUMBER_fun(ISNUMBER_funContext context) {
        Operand firstValue = visit(context.expr());
        if (firstValue.IsError()) {
            return firstValue;
        }

        if (firstValue.Type() == OperandType.NUMBER) {
            return Operand.True;
        }
        return Operand.False;
    }

    public Operand visitISTEXT_fun(ISTEXT_funContext context) {
        Operand firstValue = visit(context.expr());
        if (firstValue.IsError()) {
            return firstValue;
        }

        if (firstValue.Type() == OperandType.STRING) {
            return Operand.True;
        }
        return Operand.False;
    }

    public Operand visitISERROR_fun(ISERROR_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
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

    public Operand visitISNULL_fun(ISNULL_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
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

    public Operand visitISNULLORERROR_fun(ISNULLORERROR_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
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

    public Operand visitISEVEN_fun(ISEVEN_funContext context) {
        Operand firstValue = visit(context.expr());
        if (firstValue.Type() == OperandType.NUMBER) {
            if (firstValue.IntValue() % 2 == 0) {
                return Operand.True;
            }
        }
        return Operand.False;
    }

    public Operand visitISLOGICAL_fun(ISLOGICAL_funContext context) {
        Operand firstValue = visit(context.expr());
        if (firstValue.Type() == OperandType.BOOLEAN) {
            return Operand.True;
        }
        return Operand.False;
    }

    public Operand visitISODD_fun(ISODD_funContext context) {
        Operand firstValue = visit(context.expr());
        if (firstValue.Type() == OperandType.NUMBER) {
            if (firstValue.IntValue() % 2 == 1) {
                return Operand.True;
            }
        }
        return Operand.False;
    }

    public Operand visitISNONTEXT_fun(ISNONTEXT_funContext context) {
        Operand firstValue = visit(context.expr());
        if (firstValue.Type() != OperandType.STRING) {
            return Operand.True;
        }
        return Operand.False;
    }

    public Operand visitAND_fun(AND_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToBoolean("Function AND parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        boolean b = true;
        for (Operand item : args) {
            if (item.BooleanValue() == false) {
                b = false;
                break;
            }
        }
        return Operand.Create(b);
    }

    public Operand visitOR_fun(OR_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index =1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToBoolean("Function OR parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        boolean b = false;
        for (Operand item : args) {
            if (item.BooleanValue() == true) {
                b = true;
                break;
            }
        }
        return Operand.Create(b);
    }

    public Operand visitNOT_fun(NOT_funContext context) {
        Operand firstValue = visit(context.expr()).ToBoolean("Function NOT parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(!firstValue.BooleanValue());
    }

    public Operand visitTRUE_fun(TRUE_funContext context) {
        return Operand.True;
    }

    public Operand visitFALSE_fun(FALSE_funContext context) {
        return Operand.False;
    }

    public Operand visitE_fun(E_funContext context) {
        return Operand.Create(Math.E);
    }

    public Operand visitPI_fun(PI_funContext context) {
        return Operand.Create(Math.PI);
    }

    public Operand visitABS_fun(ABS_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function ABS parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(Math.abs(firstValue.NumberValue()));
    }

    public Operand visitQUOTIENT_fun(QUOTIENT_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function QUOTIENT parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0);
        Operand secondValue = args.get(1);

        if (secondValue.NumberValue() == 0) {
            return Operand.Error("Function QUOTIENT div 0 error!");
        }
        return Operand.Create((double) (int) (firstValue.NumberValue() / secondValue.NumberValue()));
    }

    public Operand visitMOD_fun(MOD_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function MOD parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0);
        Operand secondValue = args.get(1);

        if (secondValue.NumberValue() == 0) {
            return Operand.Error("Function MOD div 0 error!");
        }
        return Operand.Create((int) (firstValue.NumberValue() % secondValue.NumberValue()));

    }

    public Operand visitSIGN_fun(SIGN_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function SIGN parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(sign(firstValue.NumberValue()));
    }

    public Operand visitSQRT_fun(SQRT_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function SQRT parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(Math.sqrt(firstValue.NumberValue()));
    }

    public Operand visitTRUNC_fun(TRUNC_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function TRUNC parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create((int) (firstValue.NumberValue()));
    }

    public Operand visitINT_fun(INT_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function INT parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create((int) (firstValue.NumberValue()));
    }

    public Operand visitGCD_fun(GCD_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new ArrayList<Double>();
        boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function GCD parameter is error!");
        }

        return Operand.Create(F_base_gcd(list));
    }

    public Operand visitLCM_fun(LCM_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new ArrayList<Double>();
        boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function GCD parameter is error!");
        }

        return Operand.Create(F_base_lgm(list));
    }

    public Operand visitCOMBIN_fun(COMBIN_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function COMBIN parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0);
        Operand secondValue = args.get(1);

        int total = firstValue.IntValue();
        int count = secondValue.IntValue();
        double sum = 1;
        double sum2 = 1;
        for (int i = 0; i < count; i++) {
            sum *= (total - i);
            sum2 *= (i + 1);
        }
        return Operand.Create(sum / sum2);
    }

    public Operand visitPERMUT_fun(PERMUT_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function PERMUT parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0);
        Operand secondValue = args.get(1);

        int total = firstValue.IntValue();
        int count = secondValue.IntValue();

        double sum = 1;
        for (int i = 0; i < count; i++) {
            sum *= (total - i);
        }
        return Operand.Create(sum);
    }

    private int F_base_gcd(List<Double> list) {
        list = ShellSort(list);
        // list = list.OrderBy(q => q).ToList();
        int g = F_base_gcd((int)(double) list.get(1), (int)(double) list.get(0));
        for (int i = 2; i < list.size(); i++) {
            g = F_base_gcd((int)(double) list.get(i), g);
        }
        return g;
    }

    private int F_base_gcd(int a, int b) {
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
            int item =(int)(double) list.get(i);// [i];
            if (item <= 1) {
                list.remove(i);
            }
        }
        list = ShellSort(list);

        int a = (int)(double)  list.get(0);// [0];
        for (int i = 1; i < list.size(); i++) {
            int b = (int)(double)  list.get(i);
            int g = b > a ? F_base_gcd(b, a) : F_base_gcd(a, b);
            a = a / g * b;
        }
        return a;
    }

    private List<Double> ShellSort(List<Double> array) {
        int len = array.size();
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

    public Operand visitDEGREES_fun(DEGREES_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function DEGREES parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        double z = firstValue.NumberValue();
        double r = (z / Math.PI * 180);
        return Operand.Create(r);
    }

    public Operand visitRADIANS_fun(RADIANS_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function RADIANS parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        double r = firstValue.NumberValue() / 180 * Math.PI;
        return Operand.Create(r);
    }

    public Operand visitCOS_fun(COS_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function COS parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(Math.cos(firstValue.NumberValue()));
    }

    public Operand visitCOSH_fun(COSH_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function COSH parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(Math.cosh(firstValue.NumberValue()));
    }

    public Operand visitSIN_fun(SIN_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function SIN parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(Math.sin(firstValue.NumberValue()));
    }

    public Operand visitSINH_fun(SINH_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function SINH parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(Math.sinh(firstValue.NumberValue()));
    }

    public Operand visitTAN_fun(TAN_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function TAN parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(Math.tan(firstValue.NumberValue()));
    }

    public Operand visitTANH_fun(TANH_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function TANH parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(Math.tanh(firstValue.NumberValue()));
    }

    public Operand visitACOS_fun(ACOS_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function ACOS parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        double x = firstValue.NumberValue();
        if (x < -1 && x > 1) {
            return Operand.Error("Function ACOS parameter is error!");
        }
        return Operand.Create(Math.acos(x));
    }

    public Operand visitACOSH_fun(ACOSH_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function ACOSH parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        double z = firstValue.NumberValue();
        if (z < 1) {
            return Operand.Error("Function ACOSH parameter is error!");
        }
        double r = Math.log(z + Math.pow(z * z - 1, 0.5));
        return Operand.Create(r);
    }

    public Operand visitASIN_fun(ASIN_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function ASIN parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        double x = firstValue.NumberValue();
        if (x < -1 && x > 1) {
            return Operand.Error("Function ASIN parameter is error!");
        }
        return Operand.Create(Math.asin(x));
    }

    public Operand visitASINH_fun(ASINH_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function ASINH parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        double x = firstValue.NumberValue();
        double d = Math.log(x + Math.sqrt(x * x + 1));
        return Operand.Create(d);
    }

    public Operand visitATAN_fun(ATAN_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function ATAN parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(Math.atan(firstValue.NumberValue()));
    }

    public Operand visitATANH_fun(ATANH_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function ATANH parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        double x = firstValue.NumberValue();
        if (x >= 1 || x <= -1) {
            return Operand.Error("Function ATANH parameter is error!");
        }
        double d = Math.log((1 + x) / (1 - x)) / 2;
        return Operand.Create(d);
    }

    public Operand visitATAN2_fun(ATAN2_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function ATAN2 parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0);
        Operand secondValue = args.get(1);

        return Operand.Create(Math.atan2(secondValue.NumberValue(), firstValue.NumberValue()));
    }

    public Operand visitFIXED_fun(FIXED_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        int num = 2;
        if (args.size() > 1) {
            Operand secondValue = args.get(1).ToNumber("Function FIXED parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            num = secondValue.IntValue();
        }
        Operand firstValue = args.get(0).ToNumber("Function FIXED parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        double s = round(firstValue.NumberValue(), num);
        boolean no = false;
        if (args.size() == 3) {
            Operand thirdValue = args.get(2).ToBoolean("Function FIXED parameter 3 is error!");
            if (thirdValue.IsError()) {
                return thirdValue;
            }
            no = thirdValue.BooleanValue();
        }
        if (no == false) {
            return Operand.Create(s.toString("N" + num, cultureInfo));
        }
        return Operand.Create(s.toString());
    }

    public Operand visitBIN2OCT_fun(BIN2OCT_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToText("Function BIN2OCT parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        
        if (bit_2.matcher(firstValue.TextValue()).find() == false) {
            return Operand.Error("Function BIN2OCT parameter 1 is error!");
        }
        String num = Convert.toString(Convert.ToInt32(firstValue.TextValue(), 2), 8);
        if (args.size() == 2) {
            Operand secondValue = args.get(1).ToNumber("Function BIN2OCT parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (num.length() > secondValue.IntValue()) {
                return Operand.Create(padLeft(num,secondValue.IntValue(), '0'));
            }
            return Operand.Error("Function BIN2OCT parameter 2 is error!");
        }
        return Operand.Create(num);
    }

    public Operand visitBIN2DEC_fun(BIN2DEC_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function BIN2DEC parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        if (bit_2.matcher(firstValue.TextValue()).find()== false) {
            return Operand.Error("Function BIN2DEC parameter is error!");
        }
        String num = Convert.ToInt32(firstValue.TextValue(), 2);
        return Operand.Create(num);
    }

    public Operand visitBIN2HEX_fun(BIN2HEX_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToText("Function BIN2HEX parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        if (bit_2.matcher(firstValue.TextValue()).find() == false) {
            return Operand.Error("Function BIN2HEX parameter 1 is error!");
        }
        String num = Convert.toString(Convert.ToInt32(firstValue.TextValue(), 2), 16).ToUpper();
        if (args.size() == 2) {
            Operand secondValue = args.get(1).ToNumber("Function BIN2HEX parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (num.length() > secondValue.IntValue()) {
                return Operand.Create(padLeft(num,secondValue.IntValue(), '0'));
            }
            return Operand.Error("Function BIN2HEX parameter 2 is error!");
        }
        return Operand.Create(num);
    }

    public Operand visitOCT2BIN_fun(OCT2BIN_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToText("Function OCT2BIN parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        if (bit_8.matcher(firstValue.TextValue()).find() == false) {
            return Operand.Error("Function OCT2BIN parameter 1 is error!");
        }
        String num = Convert.toString(Convert.ToInt32(firstValue.TextValue(), 8), 2);
        if (args.size() == 2) {
            Operand secondValue = args.get(1).ToNumber("Function OCT2BIN parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (num.length() > secondValue.IntValue()) {
                return Operand.Create(padLeft(num,secondValue.IntValue(), '0'));
            }
            return Operand.Error("Function OCT2BIN parameter 2 is error!");
        }
        return Operand.Create(num);
    }

    public Operand visitOCT2DEC_fun(OCT2DEC_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function OCT2DEC parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        if (bit_8.matcher(firstValue.TextValue()).find() == false) {
            return Operand.Error("Function OCT2DEC parameter is error!");
        }
        String num = Convert.ToInt32(firstValue.TextValue(), 8);
        return Operand.Create(num);
    }

    public Operand visitOCT2HEX_fun(OCT2HEX_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToText("Function OCT2HEX parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        if (bit_8.matcher(firstValue.TextValue()).find() == false) {
            return Operand.Error("Function OCT2HEX parameter 1 is error!");
        }
        String num = Convert.toString(Convert.ToInt32(firstValue.TextValue(), 8), 16).ToUpper();
        if (args.size() == 2) {
            Operand secondValue = args.get(1).ToNumber("Function OCT2HEX parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (num.length() > secondValue.IntValue()) {
                return Operand.Create(padLeft(num,secondValue.IntValue(), '0'));
            }
            return Operand.Error("Function OCT2HEX parameter 2 is error!");
        }
        return Operand.Create(num);
    }

    public Operand visitDEC2BIN_fun(DEC2BIN_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToNumber("Function DEC2BIN parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        String num = System.Convert.toString(firstValue.IntValue(), 2);
        if (args.size() == 2) {
            Operand secondValue = args.get(1).ToNumber("Function DEC2BIN parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (num.length() > secondValue.IntValue()) {
                return Operand.Create(padLeft(num,secondValue.IntValue(), '0'));
            }
            return Operand.Error("Function DEC2BIN parameter 2 is error!");
        }
        return Operand.Create(num);
    }

    public Operand visitDEC2OCT_fun(DEC2OCT_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToNumber("Function DEC2OCT parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        String num = System.Convert.toString(firstValue.IntValue(), 8);
        if (args.size() == 2) {
            Operand secondValue = args.get(1).ToNumber("Function DEC2OCT parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (num.length() > secondValue.IntValue()) {
                return Operand.Create(padLeft(num,secondValue.IntValue(), '0'));
            }
            return Operand.Error("Function DEC2OCT parameter 2 is error!");
        }
        return Operand.Create(num);
    }

    public Operand visitDEC2HEX_fun(DEC2HEX_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToNumber("Function DEC2HEX parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        String num = System.Convert.toString(firstValue.IntValue(), 16).ToUpper();
        if (args.size() == 2) {
            Operand secondValue = args.get(1).ToNumber("Function DEC2HEX parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (num.length() > secondValue.IntValue()) {
                return Operand.Create(padLeft(num,secondValue.IntValue(), '0'));
            }
            return Operand.Error("Function DEC2HEX parameter 2 is error!");
        }
        return Operand.Create(num);
    }

    public Operand visitHEX2BIN_fun(HEX2BIN_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToText("Function HEX2BIN parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        if (bit_16.matcher(firstValue.TextValue()).find() == false) {
            return Operand.Error("Function HEX2BIN parameter 1 is error!");
        }

        String num = Convert.toString(Convert.ToInt32(firstValue.TextValue(), 16), 2);
        if (args.size() == 2) {
            Operand secondValue = args.get(1).ToNumber("Function HEX2BIN parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (num.length() > secondValue.IntValue()) {
                return Operand.Create(padLeft(num,secondValue.IntValue(), '0'));
            }
            return Operand.Error("Function HEX2BIN parameter 2 is error!");
        }
        return Operand.Create(num);
    }

    public Operand visitHEX2OCT_fun(HEX2OCT_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToText("Function HEX2OCT parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        if (bit_16.matcher(firstValue.TextValue()).find() == false) {
            return Operand.Error("Function HEX2OCT parameter 1 is error!");
        }
        String num = Convert.toString(Convert.ToInt32(firstValue.TextValue(), 16), 8);
        if (args.size() == 2) {
            Operand secondValue = args.get(1).ToNumber("Function HEX2OCT parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (num.length() > secondValue.IntValue()) {
                return Operand.Create(padLeft(num,secondValue.IntValue(), '0'));
            }
            return Operand.Error("Function HEX2OCT parameter 2 is error!");
        }
        return Operand.Create(num);
    }

    public Operand visitHEX2DEC_fun(HEX2DEC_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function HEX2DEC parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        if (bit_16.matcher(firstValue.TextValue()).find() == false) {
            return Operand.Error("Function HEX2DEC parameter is error!");
        }
        String num = Convert.ToInt32(firstValue.TextValue(), 16);
        return Operand.Create(num);
    }

    public Operand visitROUND_fun(ROUND_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function ROUND parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0);
        Operand secondValue = args.get(1);

        return Operand.Create((double) round((double) firstValue.NumberValue(), secondValue.IntValue()));
    }

    public Operand visitROUNDDOWN_fun(ROUNDDOWN_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function ROUNDDOWN parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0);
        Operand secondValue = args.get(1);
        if (firstValue.NumberValue() == 0.0) {
            return firstValue;
        }
        double a = Math.pow(10, secondValue.IntValue());
        double b = firstValue.NumberValue();

        b = ((double) (int) (b * a)) / a;
        return Operand.Create(b);
    }

    public Operand visitROUNDUP_fun(ROUNDUP_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function ROUNDUP parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0);
        Operand secondValue = args.get(1);
        if (firstValue.NumberValue() == 0.0) {
            return firstValue;
        }
        double a = Math.pow(10, secondValue.IntValue());
        double b = firstValue.NumberValue();

        double t = (Math.ceil(Math.abs(b) * a)) / a;
        if (b > 0)
            return Operand.Create(t);
        return Operand.Create(-t);
    }

    public Operand visitCEILING_fun(CEILING_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function CEILING parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0);
        if (args.size() == 1)
            return Operand.Create(Math.ceil(firstValue.NumberValue()));

        Operand secondValue = args.get(1);
        double b = secondValue.NumberValue();
        if (b == 0) {
            return Operand.Create(0);
        }
        if (b < 0) {
            return Operand.Error("Function CEILING parameter 2 is error!");
        }
        double a = firstValue.NumberValue();
        double d = Math.ceil(a / b) * b;
        return Operand.Create(d);
    }

    public Operand visitFLOOR_fun(FLOOR_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function FLOOR parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0);
        if (args.size() == 1)
            return Operand.Create(Math.floor(firstValue.NumberValue()));

        Operand secondValue = args.get(1);
        double b = secondValue.NumberValue();
        if (b >= 1) {
            return Operand.Create(firstValue.IntValue());
        }
        if (b <= 0) {
            return Operand.Error("Function FLOOR parameter 2 is error!");
        }
        double a = firstValue.NumberValue();
        double d = Math.floor(a / b) * b;
        return Operand.Create(d);
    }

    public Operand visitEVEN_fun(EVEN_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function EVEN parameter is error!");
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

    public Operand visitODD_fun(ODD_funContext context) {

        Operand firstValue = visit(context.expr()).ToNumber("Function ODD parameter is error!");
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

    public Operand visitMROUND_fun(MROUND_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function MROUND parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0);
        Operand secondValue = args.get(1);

        double a = secondValue.NumberValue();
        if (a <= 0) {
            return Operand.Error("Function MROUND parameter 2 is error!");
        }
        double b = firstValue.NumberValue();
        double r = round(b / a, 0) * a;
        return Operand.Create(r);
    }

    public Operand visitRAND_fun(RAND_funContext context) {
        long tick = new Date().getTime();
        Random rand = new Random((int) (tick & 0xffffffffL) | (int) (tick >> 32));
        return Operand.Create(rand.nextDouble());
    }

    public Operand visitRANDBETWEEN_fun(RANDBETWEEN_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function RANDBETWEEN parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0);
        Operand secondValue = args.get(1);

        long tick = new Date().getTime();
        Random rand = new Random((int) (tick & 0xffffffffL) | (int) (tick >> 32));
        return Operand.Create(
                rand.nextDouble() * (secondValue.NumberValue() - firstValue.NumberValue()) + firstValue.NumberValue());
    }

    public Operand visitFACT_fun(FACT_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function FACT parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        int z = firstValue.IntValue();
        if (z < 0) {
            return Operand.Error("Function FACT parameter is error!");
        }
        double d = 1;
        for (int i = 1; i <= z; i++) {
            d *= i;
        }
        return Operand.Create(d);
    }

    public Operand visitFACTDOUBLE_fun(FACTDOUBLE_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function FACTDOUBLE parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        int z = firstValue.IntValue();
        if (z < 0) {
            return Operand.Error("Function FACTDOUBLE parameter is error!");
        }
        double d = 1;
        for (int i = z; i > 0; i -= 2) {
            d *= i;
        }
        return Operand.Create(d);
    }

    public Operand visitPOWER_fun(POWER_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function POWER parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0);
        Operand secondValue = args.get(1);

        return Operand.Create(Math.pow(firstValue.NumberValue(), secondValue.NumberValue()));
    }

    public Operand visitEXP_fun(EXP_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function EXP parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(Math.exp(firstValue.NumberValue()));
    }

    public Operand visitLN_fun(LN_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function LN parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(Math.log(firstValue.NumberValue()));
    }

    public Operand visitLOG_fun(LOG_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToNumber("Function LOG parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        if (args.size() > 1) {
            return Operand.Create(Math.log(args.get(0).NumberValue(), args.get(1).NumberValue()));
        }
        return Operand.Create(Math.log10(args.get(0).NumberValue()));
    }

    public Operand visitLOG10_fun(LOG10_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function LOG10 parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(Math.log10(firstValue.NumberValue()));
    }

    public Operand visitMULTINOMIAL_fun(MULTINOMIAL_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new ArrayList<Double>();
        boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function MULTINOMIAL parameter is error!");
        }

        int sum = 0;
        int n = 1;
        for (Double a : list) {
            n *= F_base_Factorial((int) (double)a);
            sum += (int)(double) a;
        }
        int r = F_base_Factorial(sum) / n;
        return Operand.Create(r);
    }

    public Operand visitPRODUCT_fun(PRODUCT_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new ArrayList<Double>();
        boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function PRODUCT parameter is error!");
        }

        double d = 1;
        for (double a : list) {
            d *= a;
        }

        return Operand.Create(d);
    }

    public Operand visitSQRTPI_fun(SQRTPI_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function SQRTPI parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(Math.sqrt(firstValue.NumberValue() * Math.PI));
    }

    public Operand visitSUMSQ_fun(SUMSQ_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new ArrayList<Double>();
        boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function SUMSQ parameter is error!");
        }

        double d = 0;
        for (double a : list) {
            d += a * a;
        }

        return Operand.Create(d);
    }

    private int F_base_Factorial(int a) {
        if (a == 0) {
            return 1;
        }
        int r = 1;
        for (int i = a; i > 0; i--) {
            r *= i;
        }
        return r;
    }

    public Operand visitASC_fun(ASC_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function ASC parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(F_base_ToDBC(firstValue.TextValue()));
    }

    public Operand visitJIS_fun(JIS_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function JIS parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(F_base_ToSBC(firstValue.TextValue()));
    }

    public Operand visitCHAR_fun(CHAR_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function CHAR parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        char c = (char) (int) firstValue.NumberValue();
        return Operand.Create(new Character(c).toString());
    }

    public Operand visitCLEAN_fun(CLEAN_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function CLEAN parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        String t = firstValue.TextValue();
        t = clearRegex.matcher(t).replaceAll("");
        return Operand.Create(t);
    }

    public Operand visitCODE_fun(CODE_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function CODE parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        String t = firstValue.TextValue();
        if (t.length() == 0) {
            return Operand.Error("Function CODE parameter is error!");
        }
        return Operand.Create((double) (int) (char) t.charAt(0));
    }

    public Operand visitCONCATENATE_fun(CONCATENATE_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function CONCATENATE parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        StringBuilder sb = new StringBuilder();
        for (Operand item : args) {
            sb.append(item.TextValue());
        }
        return Operand.Create(sb.toString());
    }

    public Operand visitEXACT_fun(EXACT_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function EXACT parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0);
        Operand secondValue = args.get(1);

        return Operand.Create(firstValue.TextValue() == secondValue.TextValue());
    }

    public Operand visitFIND_fun(FIND_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToText("Function FIND parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        Operand secondValue = args.get(1).ToText("Function FIND parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        if (args.size() == 2) {
            int p = secondValue.TextValue().indexOf(firstValue.TextValue()) + excelIndex;
            return Operand.Create(p);
        }
        Operand count = args.get(2).ToNumber("Function FIND parameter 3 is error!");
        if (count.IsError()) {
            return count;
        }
        int p2 = secondValue.TextValue().indexOf(firstValue.TextValue(), count.IntValue()) + excelIndex;
        return Operand.Create(p2);
    }

    public Operand visitLEFT_fun(LEFT_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToText("Function LEFT parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        if (args.size() == 1) {
            return Operand.Create(new Character(firstValue.TextValue().charAt(0)).toString());
        }
        Operand secondValue = args.get(1).ToNumber("Function LEFT parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }
        return Operand.Create(firstValue.TextValue().substring(0, secondValue.IntValue()));
    }

    public Operand visitLEN_fun(LEN_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function LEN parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(firstValue.TextValue().length());
    }

    public Operand visitLOWER_fun(LOWER_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function LOWER/TOLOWER parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(firstValue.TextValue().toLowerCase());
    }

    public Operand visitMID_fun(MID_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToText("Function MID parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        Operand secondValue = args.get(1).ToNumber("Function MID parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }
        Operand thirdValue = args.get(2).ToNumber("Function MID parameter 3 is error!");
        if (thirdValue.IsError()) {
            return thirdValue;
        }
        return Operand
                .Create(firstValue.TextValue().substring(secondValue.IntValue() - excelIndex, thirdValue.IntValue()));
    }

    public Operand visitPROPER_fun(PROPER_funContext context)
    {
        Operand firstValue = visit(context.expr()).ToText("Function PROPER parameter is error!");
        if (firstValue.IsError()) { return firstValue; }

        String text = firstValue.TextValue();
        StringBuilder sb = new StringBuilder(text);
        boolean isFirst = true;
        for (int i = 0; i < text.length(); i++) {
            char t = text.charAt(i);
            if (t == ' ' || t == '\r' || t == '\n' || t == '\t' || t == '.') {
                isFirst = true;
            } else if (isFirst) {
                sb.setCharAt(i, Character.toUpperCase(t));
                isFirst = false;
            }
        }
        return Operand.Create(sb.toString());
    }

    public Operand visitREPLACE_fun(REPLACE_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToText("Function REPLACE parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        String oldtext = firstValue.TextValue();
        if (args.size() == 3) {
            Operand secondValue2 = args.get(1).ToText("Function REPLACE parameter 2 is error!");
            Operand thirdValue2 = args.get(2).ToText("Function REPLACE parameter 3 is error!");
            String old = secondValue2.TextValue();
            String newstr = thirdValue2.TextValue();
            return Operand.Create(oldtext.replace(old, newstr));
        }

        Operand secondValue = args.get(1).ToNumber("Function REPLACE parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }
        Operand thirdValue = args.get(2).ToNumber("Function REPLACE parameter 3 is error!");
        if (thirdValue.IsError()) {
            return thirdValue;
        }
        Operand fourthValue = args.get(3).ToText("Function REPLACE parameter 3 is error!");
        if (fourthValue.IsError()) {
            return fourthValue;
        }

        int start = secondValue.IntValue() - excelIndex;
        int length = thirdValue.IntValue();
        String newtext = fourthValue.TextValue();

        StringBuilder sb = new StringBuilder();
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

    public Operand visitREPT_fun(REPT_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToText("Function REPT parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        Operand secondValue = args.get(1).ToNumber("Function REPT parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        String newtext = firstValue.TextValue();
        int length = secondValue.IntValue();
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < length; i++) {
            sb.append(newtext);
        }
        return Operand.Create(sb.toString());
    }

    public Operand visitRIGHT_fun(RIGHT_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToText("Function RIGHT parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        if (args.size() == 1) {
            return Operand.Create(new Character(firstValue.TextValue().charAt(firstValue.TextValue().length() - 1)).toString());
        }
        Operand secondValue = args.get(1).ToNumber("Function RIGHT parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }
        return Operand.Create(firstValue.TextValue().substring(firstValue.TextValue().length() - secondValue.IntValue(),
                secondValue.IntValue()));
    }

    public Operand visitRMB_fun(RMB_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function RMB parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(F_base_ToChineseRMB(firstValue.NumberValue()));
    }

    public Operand visitSEARCH_fun(SEARCH_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToText("Function SEARCH parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        Operand secondValue = args.get(1).ToText("Function SEARCH parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }
        if (args.size() == 2) {
            int p = secondValue.TextValue().toLowerCase().indexOf(firstValue.TextValue().toLowerCase()) + excelIndex;
            return Operand.Create(p);
        }
        Operand thirdValue = args.get(2).ToNumber("Function SEARCH parameter 3 is error!");
        if (thirdValue.IsError()) {
            return thirdValue;
        }
        int p2 = secondValue.TextValue().toLowerCase().indexOf(firstValue.TextValue().toLowerCase(), thirdValue.IntValue()) + excelIndex;
        return Operand.Create(p2);
    }

    public Operand visitSUBSTITUTE_fun(SUBSTITUTE_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }
        Operand firstValue = args.get(0).ToText("Function SUBSTITUTE parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        Operand secondValue = args.get(1).ToText("Function SUBSTITUTE parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }
        Operand thirdValue = args.get(2).ToText("Function SUBSTITUTE parameter 3 is error!");
        if (thirdValue.IsError()) {
            return thirdValue;
        }
        if (args.size() == 3) {
            return Operand.Create(firstValue.TextValue().replace(secondValue.TextValue(), thirdValue.TextValue()));
        }
        Operand fourthValue = args.get(3).ToNumber("Function SUBSTITUTE parameter 4 is error!");
        if (fourthValue.IsError()) {
            return fourthValue;
        }

        String text = firstValue.TextValue();
        String oldtext = secondValue.TextValue();
        String newtext = thirdValue.TextValue();
        int index = fourthValue.IntValue();

        int index2 = 0;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < text.length(); i++) {
            boolean b = true;
            for (int j = 0; j < oldtext.length(); j++) {
                char t = text.charAt(i + j);
                char t2 = oldtext.charAt(j);
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

    public Operand visitT_fun(T_funContext context) {
        Operand firstValue = visit(context.expr());
        if (firstValue.Type() == OperandType.STRING) {
            return firstValue;
        }
        return Operand.Create("");
    }

    public Operand visitTEXT_fun(TEXT_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0);
        if (firstValue.IsError()) {
            return firstValue;
        }
        Operand secondValue = args.get(1).ToText("Function TEXT parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        if (firstValue.Type() == OperandType.STRING) {
            return firstValue;
        } else if (firstValue.Type() == OperandType.BOOLEAN) {
            return Operand.Create(firstValue.BooleanValue() ? "TRUE" : "FALSE");
        } else if (firstValue.Type() == OperandType.NUMBER) {
            return Operand.Create(firstValue.NumberValue().toString(secondValue.TextValue(), cultureInfo));
        } else if (firstValue.Type() == OperandType.DATE) {
            return Operand.Create(firstValue.DateValue().toString(secondValue.TextValue()));
        }
        firstValue = firstValue.ToText("Function TEXT parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(firstValue.TextValue().toString());
    }

    public Operand visitTRIM_fun(TRIM_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function TRIM parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(firstValue.TextValue().trim());
    }

    public Operand visitUPPER_fun(UPPER_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function UPPER/TOUPPER parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(firstValue.TextValue().toUpperCase());
    }

    public Operand visitVALUE_fun(VALUE_funContext context)
    {
        Operand firstValue = visit(context.expr()).ToText("Function VALUE parameter is error!");
        if (firstValue.IsError()) { return firstValue; }
        try {
            Double d=  Double.parseDouble(firstValue.TextValue());
            return Operand.Create(d);
        } catch (Exception e) {
         }
        // if (double.TryParse(firstValue.TextValue(), NumberStyles.Any, cultureInfo, out double d)) {
        //     return Operand.Create(d);
        // }
        return Operand.Error("Function VALUE parameter is error!");
    }

    private String F_base_ToSBC(String input) {
        StringBuilder sb = new StringBuilder(input);
        for (int i = 0; i < input.length(); i++) {
            char c = input.charAt(i);// [i];
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

    private String F_base_ToDBC(String input) {
        StringBuilder sb = new StringBuilder(input);
        for (int i = 0; i < input.length(); i++) {
            char c = input.charAt(i);// [i];
            if (c == 12288) {
                sb.setCharAt(i, (char) 32);
                continue;
            } else if (c > 65280 && c < 65375) {
                sb.setCharAt(i, (char) (c - 65248));
            }
        }
        return sb.toString();
    }

    private String F_base_ToChineseRMB(double number) {
        // String s =
        // x.toString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A",
        // cultureInfo);
        // String d = Regex.Replace(s,
        // "((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\\.]|$))))",
        // "${b}${z}", RegexOptions.Compiled);
        // return Regex.Replace(d, ".", m ->
        // "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].toString(),
        // RegexOptions.Compiled);

        StringBuffer chineseNumber = new StringBuffer();
        String[] num = { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };
        String[] unit = { "分", "角", "圆", "拾", "佰", "仟", "万", "拾", "佰", "仟", "亿", "拾", "佰", "仟", "万" };
        String tempNumber = String.valueOf(round(number * 100,0));
        int tempNumberLength = tempNumber.length();
        if ("0".equals(tempNumber)) {
            return "零圆整";
        }
        if (tempNumberLength > 15) {
            // throw new Exception("超出转化范围.");
        }
        boolean preReadZero = true; // 前面的字符是否读零
        for (int i = tempNumberLength; i > 0; i--) {
            if ((tempNumberLength - i + 2) % 4 == 0) {
                // 如果在（圆，万，亿，万）位上的四个数都为零,如果标志为未读零，则只读零，如果标志为已读零，则略过这四位
                if (i - 4 >= 0 && "0000".equals(tempNumber.substring(i - 4, i))) {
                    if (!preReadZero) {
                        chineseNumber.insert(0, "零");
                        preReadZero = true;
                    }
                    i -= 3; // 下面还有一个i--
                    continue;
                }
                // 如果当前位在（圆，万，亿，万）位上，则设置标志为已读零（即重置读零标志）
                preReadZero = true;
            }
            Integer digit = Integer.parseInt(tempNumber.substring(i - 1, i));
            if (digit == 0) {
                // 如果当前位是零并且标志为未读零，则读零，并设置标志为已读零
                if (!preReadZero) {
                    chineseNumber.insert(0, "零");
                    preReadZero = true;
                }
                // 如果当前位是零并且在（圆，万，亿，万）位上，则读出（圆，万，亿，万）
                if ((tempNumberLength - i + 2) % 4 == 0) {
                    chineseNumber.insert(0, unit[tempNumberLength - i]);
                }
            }
            // 如果当前位不为零，则读出此位，并且设置标志为未读零
            else {
                chineseNumber.insert(0, num[digit] + unit[tempNumberLength - i]);
                preReadZero = false;
            }
        }
        // 如果分角两位上的值都为零，则添加一个“整”字
        if (tempNumberLength - 2 >= 0 && "00".equals(tempNumber.substring(tempNumberLength - 2, tempNumberLength))) {
            chineseNumber.append("整");
        }
        return chineseNumber.toString();
    }

    public Operand visitDATEVALUE_fun(DATEVALUE_funContext context)
    {
        Operand firstValue = visit(context.expr()).ToText("Function DATEVALUE parameter is error!");
        if (firstValue.IsError()) { return firstValue; }
        try {
            SimpleDateFormat fmt=new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
            Date date= fmt.parse(firstValue.TextValue());
            return Operand.Create(date);
        } catch (Exception e) {
        }
        // if (DateTime.TryParse(firstValue.TextValue(), cultureInfo, DateTimeStyles.None, out DateTime dt)) {
        //     return Operand.Create(dt);
        // }
        return Operand.Error("Function DATEVALUE parameter is error!");
    }

    public Operand visitTIMEVALUE_fun(TIMEVALUE_funContext context)
    {
        Operand firstValue = visit(context.expr()).ToText("Function TIMEVALUE parameter is error!");
        if (firstValue.IsError()) { return firstValue; }
        try {
            SimpleDateFormat fmt = new SimpleDateFormat("d HH:mm:ss");
            Date dt= fmt.parse( firstValue.TextValue() ); 
            return Operand.Create(dt);
        } catch (Exception e) {
        }
 
        // if (TimeSpan.TryParse(firstValue.TextValue(), cultureInfo, out TimeSpan dt)) {
        //     return Operand.Create(dt);
        // }
        return Operand.Error("Function TIMEVALUE parameter is error!");
    }

    public Operand visitDATE_fun(DATE_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function DATE parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        MyDate d;
        if (args.size() == 3) {
            d = new MyDate(args.get(0).IntValue(), args.get(1).IntValue(), args.get(2).IntValue(), 0, 0, 0);
        } else if (args.size() == 4) {
            d = new MyDate(args.get(0).IntValue(), args.get(1).IntValue(), args.get(2).IntValue(), args.get(3).IntValue(),
                    0, 0);
        } else if (args.size() == 5) {
            d = new MyDate(args.get(0).IntValue(), args.get(1).IntValue(), args.get(2).IntValue(), args.get(3).IntValue(),
                    args.get(4).IntValue(), 0);
        } else {
            d = new MyDate(args.get(0).IntValue(), args.get(1).IntValue(), args.get(2).IntValue(), args.get(3).IntValue(),
                    args.get(4).IntValue(), args.get(5).IntValue());
        }
        return Operand.Create(d);
    }

    public Operand visitTIME_fun(TIME_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function TIME parameter " + (index++) + " is error!");
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

    public Operand visitNOW_fun(NOW_funContext context) {
        return Operand.Create(new MyDate(new Date()));
    }

    public Operand visitTODAY_fun(TODAY_funContext context) {
        return Operand.Create(new MyDate(DateTime.Today));
    }

    public Operand visitYEAR_fun(YEAR_funContext context) {
        Operand firstValue = visit(context.expr()).ToDate("Function YEAR parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(firstValue.DateValue().Year);
    }

    public Operand visitMONTH_fun(MONTH_funContext context) {
        Operand firstValue = visit(context.expr()).ToDate("Function MONTH parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(firstValue.DateValue().Month);
    }

    public Operand visitDAY_fun(DAY_funContext context) {
        Operand firstValue = visit(context.expr()).ToDate("Function DAY parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(firstValue.DateValue().Day);
    }

    public Operand visitHOUR_fun(HOUR_funContext context) {
        Operand firstValue = visit(context.expr()).ToDate("Function HOUR parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(firstValue.DateValue().Hour);
    }

    public Operand visitMINUTE_fun(MINUTE_funContext context) {
        Operand firstValue = visit(context.expr()).ToDate("Function MINUTE parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(firstValue.DateValue().Minute);
    }

    public Operand visitSECOND_fun(SECOND_funContext context) {
        Operand firstValue = visit(context.expr()).ToDate("Function SECOND parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(firstValue.DateValue().Second);
    }

    public Operand visitWEEKDAY_fun(WEEKDAY_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToDate("Function WEEKDAY parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        int type = 1;
        if (args.size() == 2) {
            Operand secondValue = args.get(1).ToNumber("Function WEEKDAY parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            type = secondValue.IntValue();
        }

        int t = ((DateTime) firstValue.DateValue()).DayOfWeek;
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

    public Operand visitDATEDIF_fun(DATEDIF_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToDate("Function DATEDIF parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        Operand secondValue = args.get(1).ToDate("Function DATEDIF parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }
        Operand thirdValue = args.get(2).ToText("Function DATEDIF parameter 3 is error!");
        if (thirdValue.IsError()) {
            return thirdValue;
        }

        MyDate startDate = (DateTime) firstValue.DateValue();
        MyDate endDate = (DateTime) secondValue.DateValue();
        String t = thirdValue.TextValue().toLowerCase();

        if (t == "y") {
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
        } else if (t == "m") {
            boolean b = false;
            if (startDate.Day <= endDate.Day)
                b = true;
            if (b) {
                return Operand.Create((endDate.Year * 12 + endDate.Month - startDate.Year * 12 - startDate.Month));
            } else {
                return Operand.Create((endDate.Year * 12 + endDate.Month - startDate.Year * 12 - startDate.Month - 1));
            }
        } else if (t == "d") {
            return Operand.Create((endDate - startDate).Days);
        } else if (t == "yd") {
            int day = endDate.DayOfYear - startDate.DayOfYear;
            if (endDate.Year > startDate.Year && day < 0) {
                int days = new DateTime(startDate.Year, 12, 31).DayOfYear;
                day = days + day;
            }
            return Operand.Create((day));
        } else if (t == "md") {
            int mo = endDate.Day - startDate.Day;
            if (mo < 0) {
                int days = new DateTime(startDate.Year, startDate.Month + 1, 1).AddDays(-1).Day;
                mo += days;
            }
            return Operand.Create((mo));
        } else if (t == "ym") {
            int mo = endDate.Month - startDate.Month;
            if (endDate.Day < startDate.Day)
                mo = mo - 1;
            if (mo < 0)
                mo += 12;
            return Operand.Create((mo));
        }
        return Operand.Error("Function DATEDIF parameter 3 is error!");
    }

    public Operand visitDAYS360_fun(DAYS360_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToDate("Function DAYS360 parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        Operand secondValue = args.get(1).ToDate("Function DAYS360 parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        MyDate startDate = (DateTime) firstValue.DateValue();
        MyDate endDate = (DateTime) secondValue.DateValue();

        boolean method = false;
        if (args.size() == 3) {
            Operand thirdValue = args.get(2).ToDate("Function DAYS360 parameter 3 is error!");
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

    public Operand visitEDATE_fun(EDATE_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToDate("Function EDATE parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        Operand secondValue = args.get(1).ToNumber("Function EDATE parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        return Operand.Create((MyDate) (((DateTime) firstValue.DateValue()).AddMonths(secondValue.IntValue())));
    }

    public Operand visitEOMONTH_fun(EOMONTH_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToDate("Function EOMONTH parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        Operand secondValue = args.get(1).ToNumber("Function EOMONTH parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        DateTime dt = ((DateTime) firstValue.DateValue()).AddMonths(secondValue.IntValue() + 1);
        dt = new DateTime(dt.Year, dt.Month, 1).AddDays(-1);
        return Operand.Create(dt);
    }

    public Operand visitNETWORKDAYS_fun(NETWORKDAYS_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToDate("Function NETWORKDAYS parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        MyDate startDate = (DateTime) args.get(0).DateValue();
        MyDate endDate = (DateTime) args.get(1).DateValue();

        List<DateTime> list = new List<DateTime>();
        for (int i = 2; i < args.size(); i++) {
            list.add(args.get(i).DateValue());
        }

        int days = 0;
        while (startDate <= endDate) {
            if (startDate.DayOfWeek != DayOfWeek.Sunday && startDate.DayOfWeek != DayOfWeek.Saturday) {
                if (list.Contains(startDate) == false) {
                    days++;
                }
            }
            startDate = startDate.addDays(1);
        }
        return Operand.Create(days);
    }

    public Operand visitWORKDAY_fun(WORKDAY_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToDate("Function WORKDAY parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        Operand secondValue = args.get(1).ToNumber("Function WORKDAY parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        MyDate startDate = (DateTime) firstValue.DateValue();
        int days = secondValue.IntValue();
        List<DateTime> list = new List<DateTime>();
        for (int i = 2; i < args.size(); i++) {
            args.get(i) = args.get(i).ToDate("Function WORKDAY parameter {i + 1} is error!");
            if (args.get(i).IsError()) {
                return args.get(i);
            }
            list.add(args.get(i).DateValue());
        }
        while (days > 0) {
            startDate = startDate.AddDays(1);
            if (startDate.DayOfWeek == DayOfWeek.Saturday)
                continue;
            if (startDate.DayOfWeek == DayOfWeek.Sunday)
                continue;
            if (list.Contains(startDate))
                continue;
            days--;
        }
        return Operand.Create(startDate);
    }

    public Operand visitWEEKNUM_fun(WEEKNUM_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToDate("Function WEEKNUM parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        MyDate startDate = (DateTime) firstValue.DateValue();

        int days = startDate.DayOfYear + (int) (new DateTime(startDate.Year, 1, 1).DayOfWeek);
        if (args.size() == 2) {
            Operand secondValue = args.get(1).ToNumber("Function WEEKNUM parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (secondValue.IntValue() == 2) {
                days--;
            }
        }

        double week = Math.ceil(days / 7.0);
        return Operand.Create(week);
    }

    public Operand visitMAX_fun(MAX_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function MAX parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new ArrayList<Double>();
        boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function MAX parameter error!");
        }
        return Operand.Create(Max(list));
    }

    public Operand visitMEDIAN_fun(MEDIAN_funContext context)
    {
        List<Operand> args = new ArrayList<Operand>(); int index = 1;
        for (ExprContext item : context.expr()) { Operand aa = visit(item).ToNumber("Function MEDIAN parameter "+(index++)+" is error!"); if (aa.IsError()) { return aa; } args.add(aa); }

        List<Double>  list = new ArrayList<Double> ();
        boolean o = F_base_GetList_1(args, list);
        if (o == false) { return Operand.Error("Function MEDIAN parameter error!"); }

        list=ShellSort(list);
        // list = list.OrderBy(q => q).ToList();
        return Operand.Create(list.get(list.size() / 2));
    }

    public Operand visitMIN_fun(MIN_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function MIN parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new ArrayList<Double>();
        boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function MIN parameter error!");
        }

        return Operand.Create(Min(list));
    }

    public Operand visitQUARTILE_fun(QUARTILE_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToArray("Function QUARTILE parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        Operand secondValue = args.get(1).ToNumber("Function QUARTILE parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        List<Double> list = new ArrayList<Double>();
        boolean o = F_base_GetList_2(firstValue, list);
        if (o == false) {
            return Operand.Error("Function QUARTILE parameter 1 error!");
        }

        int quant = secondValue.IntValue();
        if (quant < 0 || quant > 4) {
            return Operand.Error("Function QUARTILE parameter 2 is error!");
        }
        try {
            return Operand.Create(ExcelFunctions.Quartile(list, quant));
        } catch (Exception e) {
        }
        return Operand.Error("Function QUARTILE is error!");
    }

    public Operand visitMODE_fun(MODE_funContext context)
    {
        List<Operand> args = new ArrayList<Operand>(); int index = 1;
        for (ExprContext item : context.expr()) { Operand aa = visit(item).ToNumber("Function MODE parameter "+(index++)+" is error!"); if (aa.IsError()) { return aa; } args.add(aa); }

        List<Double>  list = new ArrayList<Double> ();
        boolean o = F_base_GetList_1(args, list);
        if (o == false) { return Operand.Error("Function MODE parameter error!"); }


        Map<Double, Integer> dict = new HashMap<Double, Integer>();
        for (double item : list) { 
            if (dict.containsKey(item)) {
                dict.put(item, dict.get(item)+1);
            } else {
                dict.put(item, 1);
            }
        }
        int maxCount=0;
        double maxValue=0;
        for (Double d : dict.keySet()) {
            int count= dict.get(d);
            if(count> maxCount){
                maxCount=count;
                maxValue=d;
            }
        }
        return Operand.Create(maxValue);
        // return Operand.Create(dict.OrderByDescending(q => q.Value).First().Key);
    }

    public Operand visitLARGE_fun(LARGE_funContext context)
    {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) { Operand aa = visit(item); if (aa.IsError()) { return aa; } args.add(aa); }

        Operand firstValue = args.get(0).ToArray("Function LARGE parameter 1 is error!");
        if (firstValue.IsError()) { return firstValue; }
        Operand secondValue = args.get(1).ToNumber("Function LARGE parameter 2 is error!");
        if (secondValue.IsError()) { return secondValue; }


        List<Double>  list = new ArrayList<Double> ();
        boolean o = F_base_GetList_1(args, list);
        if (o == false) { return Operand.Error("Function LARGE parameter error!"); }

        list=ShellSort(list);
        // list = list.OrderByDescending(q => q).ToList();
        int quant = secondValue.IntValue();
        return Operand.Create(list.get(list.size()-1-(secondValue.IntValue() - excelIndex)));
    }

    public Operand visitSMALL_fun(SMALL_funContext context)
    {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) { Operand aa = visit(item); if (aa.IsError()) { return aa; } args.add(aa); }

        Operand firstValue = args.get(0).ToArray("Function SMALL parameter 1 is error!");
        if (firstValue.IsError()) { return firstValue; }
        Operand secondValue = args.get(1).ToNumber("Function SMALL parameter 2 is error!");
        if (secondValue.IsError()) { return secondValue; }

        List<Double>  list = new ArrayList<Double> ();
        boolean o = F_base_GetList_1(args, list);
        if (o == false) { return Operand.Error("Function SMALL parameter error!"); }

        list=ShellSort(list);
        // list = list.OrderBy(q => q).ToList();
        int quant = secondValue.IntValue();
        return Operand.Create(list.get(secondValue.IntValue() - excelIndex) );
        // return Operand.Create(list[secondValue.IntValue() - excelIndex]);
    }

    public Operand visitPERCENTILE_fun(PERCENTILE_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToArray("Function PERCENTILE parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        Operand secondValue = args.get(1).ToNumber("Function PERCENTILE parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        List<Double> list = new ArrayList<Double>();
        boolean o = F_base_GetList_2(firstValue, list);
        if (o == false) {
            return Operand.Error("Function PERCENTILE parameter error!");
        }

        double k = secondValue.NumberValue();
        try {
            return Operand.Create(ExcelFunctions.Percentile(list, k));
        } catch (Exception e) {
        }
        return Operand.Error("Function PERCENTILE parameter error!");
    }

    public Operand visitPERCENTRANK_fun(PERCENTRANK_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToArray("Function PERCENTRANK parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        Operand secondValue = args.get(1).ToNumber("Function PERCENTRANK parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        List<Double> list = new ArrayList<Double>();
        boolean o = F_base_GetList_2(firstValue, list);
        if (o == false) {
            return Operand.Error("Function PERCENTRANK parameter error!");
        }

        double k = secondValue.NumberValue();
        double v = ExcelFunctions.PercentRank(list, k);
        int d = 3;
        if (args.size() == 3) {
            Operand thirdValue = args.get(2).ToNumber("Function PERCENTRANK parameter 3 is error!");
            if (thirdValue.IsError()) {
                return thirdValue;
            }
            d = thirdValue.IntValue();
        }
        return Operand.Create(round(v, d));
    }

    public Operand visitAVERAGE_fun(AVERAGE_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new ArrayList<Double>();
        boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function AVERAGE parameter error!");
        }

        return Operand.Create(Average(list));
    }

    public Operand visitAVERAGEIF_fun(AVERAGEIF_funContext context)
    {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) { Operand aa = visit(item); if (aa.IsError()) { return aa; } args.add(aa); }

        List<Double>  list = new ArrayList<Double> ();
        boolean o = F_base_GetList_2(args.get(0), list);
        if (o == false) { return Operand.Error("Function AVERAGE parameter 1 error!"); }

        List<Double>  sumdbs;
        if (args.size() == 3) {
            sumdbs = new ArrayList<Double> ();
            boolean o2 = F_base_GetList_2(args.get(2), sumdbs);
            if (o2 == false) { return Operand.Error("Function AVERAGE parameter 3 error!"); }
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
                Double d=Double.parseDouble(args.get(1).TextValue().trim());
                count = F_base_countif(list, d);
                sum = F_base_sumif(list, "=" + args.get(1).TextValue().trim(), sumdbs);
            } catch (Exception e) {
                String sunif = args.get(1).TextValue().trim();
                if (sumifRegex.matcher(sunif).find()) {
                    count = F_base_countif(list, sunif);
                    sum = F_base_sumif(list, sunif, sumdbs);
                } else {
                    return Operand.Error("Function AVERAGE parameter 2 error!");
                }
            }
            // if (double.TryParse(args.get(1).TextValue().trim(), NumberStyles.Any, cultureInfo, out double d)) {
            //     count = F_base_countif(list, d);
            //     sum = F_base_sumif(list, "=" + args.get(1).TextValue().trim(), sumdbs);
            // } else {
            //     String sunif = args.get(1).TextValue().trim();
            //     if (sumifRegex.matcher(sunif)) {
            //         count = F_base_countif(list, sunif);
            //         sum = F_base_sumif(list, sunif, sumdbs);
            //     } else {
            //         return Operand.Error("Function AVERAGE parameter 2 error!");
            //     }
            // }
        }
        return Operand.Create(sum / count);
    }

    public Operand visitGEOMEAN_fun(GEOMEAN_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        if (args.size() == 1)
            return args.get(0);

        List<Double> list = new ArrayList<Double>();
        boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function GEOMEAN parameter error!");
        }

        double sum = 1;
        for (double db : list) {
            sum *= db;
        }
        return Operand.Create(Math.pow(sum, 1.0 / list.size()));
    }

    public Operand visitHARMEAN_fun(HARMEAN_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        if (args.size() == 1)
            return args.get(0);

        List<Double> list = new ArrayList<Double>();
        boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function HARMEAN parameter error!");
        }

        double sum = 0;
        for (double db : list) {
            if (db == 0) {
                return Operand.Error("Function HARMEAN parameter error!");
            }
            sum += 1 / db;
        }
        return Operand.Create(list.size() / sum);
    }

    public Operand visitCOUNT_fun(COUNT_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new ArrayList<Double>();
        boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function COUNT parameter error!");
        }

        return Operand.Create(list.size());
    }

    public Operand visitCOUNTIF_fun(COUNTIF_funContext context)
    {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) { Operand aa = visit(item); if (aa.IsError()) { return aa; } args.add(aa); }

        List<Double>  list = new ArrayList<Double> ();
        boolean o = F_base_GetList_2(args.get(0), list);
        if (o == false) { return Operand.Error("Function COUNTIF parameter error!"); }

        int count;
        if (args.get(1).Type() == OperandType.NUMBER) {
            count = F_base_countif(list, args.get(1).NumberValue());
        } else {
            try {
                Double d=Double.parseDouble(args.get(1).TextValue().trim());
                count = F_base_countif(list, d);
            } catch (Exception e) {
                String sunif = args.get(1).TextValue().trim();
                if (sumifRegex.matcher(sunif).find()) {
                    count = F_base_countif(list, sunif);
                } else {
                    return Operand.Error("Function COUNTIF parameter 2 error!");
                }
            }
            // if (double.TryParse(args.get(1).TextValue().trim(), NumberStyles.Any, cultureInfo, out double d)) {
            //     count = F_base_countif(list, d);
            // } else {
            //     String sunif = args.get(1).TextValue().trim();
            //     if (sumifRegex.matcher(sunif)) {
            //         count = F_base_countif(list, sunif);
            //     } else {
            //         return Operand.Error("Function COUNTIF parameter 2 error!");
            //     }
            // }
        }
        return Operand.Create(count);
    }

    public Operand visitSUM_fun(SUM_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        if (args.size() == 1)
            return args.get(0);

        List<Double> list = new ArrayList<Double>();
        boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function SUM parameter error!");
        }

        double sum = Sum(list);
        return Operand.Create(sum);
    }

    public Operand visitSUMIF_fun(SUMIF_funContext context)
    {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) { Operand aa = visit(item); if (aa.IsError()) { return aa; } args.add(aa); }

        List<Double>  list = new ArrayList<Double> ();
        boolean o = F_base_GetList_2(args.get(0), list);
        if (o == false) { return Operand.Error("Function SUMIF parameter 1 error!"); }

        List<Double>  sumdbs;
        if (args.size() == 3) {
            sumdbs = new ArrayList<Double> ();
            boolean o2 = F_base_GetList_2(args.get(2), sumdbs);
            if (o2 == false) { return Operand.Error("Function SUMIF parameter 3 error!"); }
        } else {
            sumdbs = list;
        }

        double sum;
        if (args.get(1).Type() == OperandType.NUMBER) {
            sum = F_base_countif(list, args.get(1).NumberValue()) * args.get(1).NumberValue();
        } else {
            try {
                Double d=Double.parseDouble(args.get(1).TextValue().trim());
                sum = F_base_sumif(list, "=" + args.get(1).TextValue().trim(), sumdbs);
            } catch (Exception e) {
                String sunif = args.get(1).TextValue().trim();
                if (sumifRegex.matcher(sunif).find()) {
                    sum = F_base_sumif(list, sunif, sumdbs);
                } else {
                    return Operand.Error("Function SUMIF parameter 2 error!");
                }
            }
            // if (double.TryParse(args.get(1).TextValue().trim(), NumberStyles.Any, cultureInfo, out _)) {
            //     sum = F_base_sumif(list, "=" + args.get(1).TextValue().trim(), sumdbs);
            // } else {
            //     String sunif = args.get(1).TextValue().trim();
            //     if (sumifRegex.matcher(sunif)) {
            //         sum = F_base_sumif(list, sunif, sumdbs);
            //     } else {
            //         return Operand.Error("Function SUMIF parameter 2 error!");
            //     }
            // }
        }
        return Operand.Create(sum);
    }

    public Operand visitAVEDEV_fun(AVEDEV_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new ArrayList<Double>();
        boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function AVEDEV parameter error!");
        }

        double avg = Average(list);
        double sum = 0;
        for (int i = 0; i < list.size(); i++) {
            sum += Math.abs(list.get(i) - avg);
        }
        return Operand.Create(sum / list.size());
    }

    public Operand visitSTDEV_fun(STDEV_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        if (args.size() == 1) {
            return Operand.Error("Function STDEV parameter only one error!");
        }
        List<Double> list = new ArrayList<Double>();
        boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function STDEV parameter error!");
        }

        double avg = Average(list);
        double sum = 0;
        for (int i = 0; i < list.size(); i++) {
            sum += (list.get(i) - avg) * (list.get(i) - avg);
        }
        return Operand.Create(Math.sqrt(sum / (list.size() - 1)));
    }

    public Operand visitSTDEVP_fun(STDEVP_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new ArrayList<Double>();
        boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function STDEVP parameter error!");
        }

        double sum = 0;
        double avg = Average(list);

        for (int i = 0; i < list.size(); i++) {
            sum += (list.get(i) - avg) * (list.get(i) - avg);
        }
        return Operand.Create(Math.sqrt(sum / (list.size())));
    }

    public Operand visitDEVSQ_fun(DEVSQ_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new ArrayList<Double>();
        boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function DEVSQ parameter error!");
        }

        double avg = Average(list);
        double sum = 0;
        for (int i = 0; i < list.size(); i++) {
            sum += (list.get(i) - avg) * (list.get(i) - avg);
        }
        return Operand.Create(sum);
    }

    public Operand visitVAR_fun(VAR_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        if (args.size() == 1) {
            return Operand.Error("Function VAR parameter only one error!");
        }
        List<Double> list = new ArrayList<Double>();
        boolean o = F_base_GetList_1(args, list);
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

    public Operand visitVARP_fun(VARP_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new ArrayList<Double>();
        boolean o = F_base_GetList_1(args, list);
        if (o == false) {
            return Operand.Error("Function VARP parameter error!");
        }

        double sum = 0;
        double avg = Average(list);
        for (int i = 0; i < list.size(); i++) {
            sum += (avg - list.get(i)) * (avg - list.get(i));
        }
        return Operand.Create(sum / list.size());
    }

    public Operand visitNORMDIST_fun(NORMDIST_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToNumber("Function NORMDIST parameter 1 error!");
        if (firstValue.IsError())
            return firstValue;
        Operand secondValue = args.get(1).ToNumber("Function NORMDIST parameter 2 error!");
        if (secondValue.IsError())
            return secondValue;
        Operand thirdValue = args.get(2).ToNumber("Function NORMDIST parameter 3 error!");
        if (thirdValue.IsError())
            return thirdValue;

        Operand fourthValue = args.get(3).ToBoolean("Function NORMDIST parameter 4 error!");
        if (fourthValue.IsError())
            return fourthValue;

        double num = firstValue.NumberValue();
        double avg = secondValue.NumberValue();
        double STDEV = thirdValue.NumberValue();
        boolean b = fourthValue.BooleanValue();
        return Operand.Create(ExcelFunctions.NormDist(num, avg, STDEV, b));
    }

    public Operand visitNORMINV_fun(NORMINV_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function NORMINV parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        double num = args.get(0).NumberValue();
        double avg = args.get(1).NumberValue();
        double STDEV = args.get(2).NumberValue();
        return Operand.Create(ExcelFunctions.NormInv(num, avg, STDEV));
    }

    public Operand visitNORMSDIST_fun(NORMSDIST_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function NORMSDIST parameter 1 error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        double k = firstValue.NumberValue();
        return Operand.Create(ExcelFunctions.NormSDist(k));
    }

    public Operand visitNORMSINV_fun(NORMSINV_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function NORMSINV parameter 1 error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        double k = firstValue.NumberValue();
        return Operand.Create(ExcelFunctions.NormSInv(k));
    }

    public Operand visitBETADIST_fun(BETADIST_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function BETADIST parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        double x = args.get(0).NumberValue();
        double alpha = args.get(1).NumberValue();
        double beta = args.get(2).NumberValue();

        if (alpha < 0.0 || beta < 0.0) {
            return Operand.Error("Function BETADIST parameter error!");
        }

        return Operand.Create(ExcelFunctions.BetaDist(x, alpha, beta));
    }

    public Operand visitBETAINV_fun(BETAINV_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function BETAINV parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        double probability = args.get(0).NumberValue();
        double alpha = args.get(1).NumberValue();
        double beta = args.get(2).NumberValue();
        if (alpha < 0.0 || beta < 0.0 || probability < 0.0 || probability > 1.0) {
            return Operand.Error("Function BETAINV parameter error!");
        }
        try {
            return Operand.Create(ExcelFunctions.BetaInv(probability, alpha, beta));
        } catch (Exception e) {
        }
        return Operand.Error("Function BETAINV parameter error!");
    }

    public Operand visitBINOMDIST_fun(BINOMDIST_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToNumber("Function BINOMDIST parameter 1 error!");
        if (firstValue.IsError())
            return firstValue;
        Operand secondValue = args.get(1).ToNumber("Function BINOMDIST parameter 2 error!");
        if (secondValue.IsError())
            return secondValue;
        Operand thirdValue = args.get(2).ToNumber("Function BINOMDIST parameter 3 error!");
        if (thirdValue.IsError())
            return thirdValue;

        Operand fourthValue = args.get(3).ToBoolean("Function BINOMDIST parameter 4 error!");
        if (fourthValue.IsError())
            return fourthValue;

        if (!(thirdValue.NumberValue() >= 0.0 && thirdValue.NumberValue() <= 1.0 && secondValue.NumberValue() >= 0)) {
            return Operand.Error("Function BINOMDIST parameter error!");
        }
        return Operand.Create(ExcelFunctions.BinomDist(firstValue.IntValue(), secondValue.IntValue(),
                thirdValue.NumberValue(), fourthValue.BooleanValue()));
    }

    public Operand visitEXPONDIST_fun(EXPONDIST_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToNumber("Function EXPONDIST parameter 1 error!");
        if (firstValue.IsError())
            return firstValue;
        Operand secondValue = args.get(1).ToNumber("Function EXPONDIST parameter 2 error!");
        if (secondValue.IsError())
            return secondValue;
        Operand thirdValue = args.get(2).ToBoolean("Function EXPONDIST parameter 3 error!");
        if (thirdValue.IsError())
            return thirdValue;

        if (firstValue.NumberValue() < 0.0) {
            return Operand.Error("Function EXPONDIST parameter error!");
        }

        return Operand.Create(ExcelFunctions.ExponDist(firstValue.NumberValue(), secondValue.NumberValue(),
                thirdValue.BooleanValue()));
    }

    public Operand visitFDIST_fun(FDIST_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function FDIST parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        double x = args.get(0).NumberValue();
        int degreesFreedom = args.get(1).IntValue();
        int degreesFreedom2 = args.get(2).IntValue();
        if (degreesFreedom <= 0.0 || degreesFreedom2 <= 0.0) {
            return Operand.Error("Function FDIST parameter error!");
        }
        return Operand.Create(ExcelFunctions.FDist(x, degreesFreedom, degreesFreedom2));
    }

    public Operand visitFINV_fun(FINV_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function FINV parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        double probability = args.get(0).NumberValue();
        int degreesFreedom = args.get(1).IntValue();
        int degreesFreedom2 = args.get(2).IntValue();
        if (degreesFreedom <= 0.0 || degreesFreedom2 <= 0.0) {
            return Operand.Error("Function FINV parameter error!");
        }
        try {
            return Operand.Create(ExcelFunctions.FInv(probability, degreesFreedom, degreesFreedom2));
        } catch (Exception e) {
        }
        return Operand.Error("Function FINV parameter error!");
    }

    public Operand visitFISHER_fun(FISHER_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function FISHER parameter error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        double x = firstValue.NumberValue();
        if (x >= 1 || x <= -1) {
            return Operand.Error("Function FISHER parameter error!");
        }
        double n = 0.5 * Math.log((1 + x) / (1 - x));
        return Operand.Create(n);
    }

    public Operand visitFISHERINV_fun(FISHERINV_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function FISHERINV parameter error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        double x = firstValue.NumberValue();
        double n = (Math.exp(2 * x) - 1) / (Math.exp(2 * x) + 1);
        return Operand.Create(n);
    }

    public Operand visitGAMMADIST_fun(GAMMADIST_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToNumber("Function GAMMADIST parameter 1 error!");
        if (firstValue.IsError())
            return firstValue;
        Operand secondValue = args.get(1).ToNumber("Function GAMMADIST parameter 2 error!");
        if (secondValue.IsError())
            return secondValue;
        Operand thirdValue = args.get(2).ToNumber("Function GAMMADIST parameter 3 error!");
        if (thirdValue.IsError())
            return thirdValue;

        Operand fourthValue = args.get(3).ToBoolean("Function GAMMADIST parameter 4 error!");
        if (fourthValue.IsError())
            return fourthValue;

        double x = firstValue.NumberValue();
        double alpha = secondValue.NumberValue();
        double beta = thirdValue.NumberValue();
        boolean cumulative = fourthValue.BooleanValue();
        if (alpha < 0.0 || beta < 0.0) {
            return Operand.Error("Function GAMMADIST parameter error!");
        }
        return Operand.Create(ExcelFunctions.GammaDist(x, alpha, beta, cumulative));
    }

    public Operand visitGAMMAINV_fun(GAMMAINV_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function GAMMAINV parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        double probability = args.get(0).NumberValue();
        double alpha = args.get(1).NumberValue();
        double beta = args.get(2).NumberValue();
        if (alpha < 0.0 || beta < 0.0 || probability < 0 || probability > 1.0) {
            return Operand.Error("Function GAMMAINV parameter error!");
        }
        return Operand.Create(ExcelFunctions.GammaInv(probability, alpha, beta));
    }

    public Operand visitGAMMALN_fun(GAMMALN_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function GAMMALN parameter error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(ExcelFunctions.GAMMALN(firstValue.NumberValue()));
    }

    public Operand visitHYPGEOMDIST_fun(HYPGEOMDIST_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function HYPGEOMDIST parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        int k = args.get(0).IntValue();
        int draws = args.get(1).IntValue();
        int success = args.get(2).IntValue();
        int population = args.get(3).IntValue();
        if (!(population >= 0 && success >= 0 && draws >= 0 && success <= population && draws <= population)) {
            return Operand.Error("Function HYPGEOMDIST parameter error!");
        }
        return Operand.Create(ExcelFunctions.HypgeomDist(k, draws, success, population));
    }

    public Operand visitLOGINV_fun(LOGINV_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function LOGINV parameter " + (index++) + " is error!");
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

    public Operand visitLOGNORMDIST_fun(LOGNORMDIST_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function LOGNORMDIST parameter " + (index++) + " is error!");
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

    public Operand visitNEGBINOMDIST_fun(NEGBINOMDIST_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function NEGBINOMDIST parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        int k = args.get(0).IntValue();
        double r = args.get(1).NumberValue();
        double p = args.get(2).NumberValue();

        if (!(r >= 0.0 && p >= 0.0 && p <= 1.0)) {
            return Operand.Error("Function NEGBINOMDIST parameter error!");
        }
        return Operand.Create(ExcelFunctions.NegbinomDist(k, r, p));
    }

    public Operand visitPOISSON_fun(POISSON_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToNumber("Function POISSON parameter 1 error!");
        if (firstValue.IsError())
            return firstValue;
        Operand secondValue = args.get(1).ToNumber("Function POISSON parameter 2 error!");
        if (secondValue.IsError())
            return secondValue;
        Operand thirdValue = args.get(2).ToBoolean("Function POISSON parameter 3 error!");
        if (thirdValue.IsError())
            return thirdValue;

        int k = firstValue.IntValue();
        double lambda = secondValue.NumberValue();
        boolean state = thirdValue.BooleanValue();
        if (!(lambda > 0.0)) {
            return Operand.Error("Function POISSON parameter error!");
        }
        return Operand.Create(ExcelFunctions.POISSON(k, lambda, state));
    }

    public Operand visitTDIST_fun(TDIST_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function TDIST parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        double x = args.get(0).NumberValue();
        int degreesFreedom = args.get(1).IntValue();
        int tails = args.get(2).IntValue();
        if (degreesFreedom <= 0.0 || tails < 1 || tails > 2) {
            return Operand.Error("Function TDIST parameter error!");
        }
        try {
            return Operand.Create(ExcelFunctions.TDist(x, degreesFreedom, tails));
        } catch (Exception e) {
        }
        return Operand.Error("Function TDIST parameter error!");
    }

    public Operand visitTINV_fun(TINV_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function TINV parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        double probability = args.get(0).NumberValue();
        int degreesFreedom = args.get(1).IntValue();
        if (degreesFreedom <= 0.0) {
            return Operand.Error("Function TINV parameter error!");
        }
        try {
            return Operand.Create(ExcelFunctions.TInv(probability, degreesFreedom));
        } catch (Exception e) {
        }
        return Operand.Error("Function TINV parameter error!");
    }

    public Operand visitWEIBULL_fun(WEIBULL_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToNumber("Function WEIBULL parameter 1 error!");
        if (firstValue.IsError())
            return firstValue;
        Operand secondValue = args.get(1).ToNumber("Function WEIBULL parameter 2 error!");
        if (secondValue.IsError())
            return secondValue;
        Operand thirdValue = args.get(2).ToNumber("Function WEIBULL parameter 3 error!");
        if (thirdValue.IsError())
            return thirdValue;

        Operand fourthValue = args.get(3).ToBoolean("Function WEIBULL parameter 4 error!");
        if (fourthValue.IsError())
            return fourthValue;

        double x = firstValue.NumberValue();
        double shape = secondValue.NumberValue();
        double scale = thirdValue.NumberValue();
        boolean state = fourthValue.BooleanValue();
        if (shape <= 0.0 || scale <= 0.0) {
            return Operand.Error("Function WEIBULL parameter error!");
        }

        return Operand.Create(ExcelFunctions.WEIBULL(x, shape, scale, state));
    }

    private int F_base_countif(List<Double> dbs, double d) {
        int count = 0;
        d = round(d, 12);
        for (double item : dbs) {
            if (round(item, 12) == d) {
                count++;
            }
        }
        return count;
    }

    private int F_base_countif(List<Double> dbs, String s)
    {
        Matcher m = sumifRegex.matcher(s);
        Double d = Double.parseDouble(m.group(2));
        int count = 0;

        for (double item : dbs) {
            if (F_base_compare(item, d, s)) {
                count++;
            }
        }
        return count;
    }

    private double F_base_sumif(List<Double>  dbs, String s, List<Double>  sumdbs)
    {
        Matcher m = sumifRegex.matcher(s);
        Double d = Double.parseDouble(m.group(2));
        double sum = 0;

        for (int i = 0; i < dbs.size(); i++) {
            if (F_base_compare(dbs.get(i), d, s)) {
                sum += sumdbs.get(i);
            }
        }
        return sum;
    }

    private boolean F_base_compare(double a, double b, String ss) {
        if (ss == "<") {
            return round(a - b, 12) < 0;
        } else if (ss == "<=") {
            return round(a - b, 12) <= 0;
        } else if (ss == ">") {
            return round(a - b, 12) > 0;
        } else if (ss == ">=") {
            return round(a - b, 12) >= 0;
        } else if (ss == "=" || ss == "==") {
            return round(a - b, 12) == 0;
        }
        return round(a - b, 12) != 0;
    }

    private boolean F_base_GetList_1(List<Operand> args, List<Double> list) {
        for (Operand item : args) {
            if (item.Type() == OperandType.NUMBER) {
                list.add(item.NumberValue());
            } else if (item.Type() == OperandType.ARRARY) {
                boolean o = F_base_GetList_1(item.ArrayValue(), list);
                if (o == false) {
                    return false;
                }
            } else if (item.Type() == OperandType.JSON) {
                Operand i = item.ToArray(null);
                if (i.IsError()) {
                    return false;
                }
                boolean o = F_base_GetList_1(i.ArrayValue(), list);
                if (o == false) {
                    return false;
                }
            } else {
                Operand o = item.ToNumber(null);
                if (o.IsError()) {
                    return false;
                }
                list.add(o.NumberValue());
            }
        }
        return true;
    }

    private boolean F_base_GetList_2(Operand args, List<Double> list) {
        if (args.IsError()) {
            return false;
        }
        if (args.Type() == OperandType.NUMBER) {
            list.add(args.NumberValue());
        } else if (args.Type() == OperandType.ARRARY) {
            boolean o = F_base_GetList_1(args.ArrayValue(), list);
            if (o == false) {
                return false;
            }
        } else if (args.Type() == OperandType.JSON) {
            Operand i = args.ToArray(null);
            if (i.IsError()) {
                return false;
            }
            boolean o = F_base_GetList_1(i.ArrayValue(), list);
            if (o == false) {
                return false;
            }
        } else {
            Operand o = args.ToNumber(null);
            if (o.IsError()) {
                return false;
            }
            list.add(o.NumberValue());
        }
        return true;
    }

    private boolean F_base_GetList(Operand args, List<String> list) {
        if (args.IsError()) {
            return false;
        }
        if (args.Type() == OperandType.ARRARY) {
            boolean o = F_base_GetList(args.ArrayValue(), list);
            if (o == false) {
                return false;
            }
        } else if (args.Type() == OperandType.JSON) {
            Operand i = args.ToArray(null);
            if (i.IsError()) {
                return false;
            }
            boolean o = F_base_GetList(i.ArrayValue(), list);
            if (o == false) {
                return false;
            }
        } else {
            Operand o = args.ToText(null);
            if (o.IsError()) {
                return false;
            }
            list.add(o.TextValue());
        }
        return true;
    }

    private boolean F_base_GetList(List<Operand> args, List<String> list) {
        for (Operand item : args) {
            if (item.Type() == OperandType.ARRARY) {
                boolean o = F_base_GetList(item.ArrayValue(), list);
                if (o == false) {
                    return false;
                }
            } else if (item.Type() == OperandType.JSON) {
                Operand i = item.ToArray(null);
                if (i.IsError()) {
                    return false;
                }
                boolean o = F_base_GetList(i.ArrayValue(), list);
                if (o == false) {
                    return false;
                }
            } else {
                Operand o = item.ToText(null);
                if (o.IsError()) {
                    return false;
                }
                list.add(o.TextValue());
            }
        }
        return true;
    }

    public Operand visitURLENCODE_fun(URLENCODE_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function URLENCODE parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(URLEncoder.encode(firstValue.TextValue()));
    }

    public Operand visitURLDECODE_fun(URLDECODE_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function URLDECODE parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(URLDecoder.decode(firstValue.TextValue()));
    }

    public Operand visitHTMLENCODE_fun(HTMLENCODE_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function HTMLENCODE parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(StringUtils.unescapeHtml(firstValue.TextValue()));
    }

    public Operand visitHTMLDECODE_fun(HTMLDECODE_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function HTMLDECODE parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(StringEscapeUtils.HtmlDecode(firstValue.TextValue()));
    }

    public Operand visitBASE64TOTEXT_fun(BASE64TOTEXT_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function BASE64TOTEXT parameter " + (index++) + " is error!");
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
        String t = new String(Base64.FromBase64String(args.get(0).TextValue()),encoding);
        return Operand.Create(t);
    }

    public Operand visitBASE64URLTOTEXT_fun(BASE64URLTOTEXT_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function BASE64URLTOTEXT parameter " + (index++) + " is error!");
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
        String t = new String(Base64.FromBase64ForUrlString(args.get(0).TextValue()),encoding);
        return Operand.Create(t);
    }

    public Operand visitTEXTTOBASE64_fun(TEXTTOBASE64_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function TEXTTOBASE64 parameter " + (index++) + " is error!");
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
        byte[] bytes = encoding.GetBytes(args.get(0).TextValue());
        String t = Base64.ToBase64String(bytes);
        return Operand.Create(t);
    }

    public Operand visitTEXTTOBASE64URL_fun(TEXTTOBASE64URL_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function TEXTTOBASE64URL parameter " + (index++) + " is error!");
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
        byte[] bytes = encoding.GetBytes(args.get(0).TextValue());
        String t = Base64.ToBase64ForUrlString(bytes);
        return Operand.Create(t);
    }

    public Operand visitREGEX_fun(REGEX_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToText("Function REGEX parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        Operand secondValue = args.get(1).ToText("Function REGEX parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        if (args.size() == 2) {
            Matcher b = Pattern.compile(firstValue.TextValue()).matcher(secondValue.TextValue());
            if (b.find()== false) {
                return Operand.Error("Function REGEX is error!");
            }
            return Operand.Create(b.group(0));
        } else  {
        // } else if (args.size() == 3) {
            Matcher ms = Pattern.compile(firstValue.TextValue()).matcher(secondValue.TextValue());
            // Matches ms = Regex.Matches(firstValue.TextValue(), secondValue.TextValue());
            Operand thirdValue = args.get(2).ToNumber("Function REGEX parameter 3 is error!");
            if (thirdValue.IsError()) {
                return thirdValue;
            }
            if (ms.groupCount() <= thirdValue.IntValue() - excelIndex) {
                return Operand.Error("Function REGEX is error!");
            }
            return Operand.Create(ms.group(thirdValue.IntValue() - excelIndex));
            //  [thirdValue.IntValue() - excelIndex].Value);
        // } else {
        //     Matcher ms = Pattern.compile(firstValue.TextValue()).matcher(secondValue.TextValue());
        //     // Matches ms = Regex.Matches(firstValue.TextValue(), secondValue.TextValue());
        //     Operand thirdValue = args.get(2).ToNumber("Function REGEX parameter 3 is error!");
        //     if (thirdValue.IsError()) {
        //         return thirdValue;
        //     }
        //     if (ms.groupCount() <= thirdValue.IntValue() + excelIndex) {
        //         return Operand.Error("Function REGEX parameter 3 is error!");
        //     }
        //     Operand fourthValue = args.get(3).ToNumber("Function REGEX parameter 4 is error!");
        //     if (fourthValue.IsError()) {
        //         return fourthValue;
        //     }
        //     return Operand.Create(ms[thirdValue.IntValue() - excelIndex].Groups[fourthValue.IntValue()].Value);
        }
    }

    public Operand visitREGEXREPALCE_fun(REGEXREPALCE_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function REGEXREPALCE parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        Matcher b =Pattern.compile(args.get(0).TextValue()).matcher(args.get(1).TextValue());//  .Replace(args.get(0).TextValue(), args.get(1).TextValue(), args.get(2).TextValue());
        String t=  b.replaceAll(args.get(2).TextValue());
        return Operand.Create(t);
    }

    public Operand visitISREGEX_fun(ISREGEX_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function ISREGEX parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        boolean b = Pattern.compile(args.get(0).TextValue()).matcher(args.get(1).TextValue()).find();
        return Operand.Create(b);
    }

    public Operand visitGUID_fun(GUID_funContext context) {
        return Operand.Create(UUID.randomUUID().toString().replaceAll("-",""));
    }

    public Operand visitMD5_fun(MD5_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function MD5 parameter " + (index++) + " is error!");
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
            String t = Hash.GetMd5String(args.get(0).TextValue().getBytes(encoding));
            return Operand.Create(t);
        } catch (Exception e) {
        }
        return Operand.Error("Function MD5 is error!");
    }

    public Operand visitSHA1_fun(SHA1_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function SHA1 parameter " + (index++) + " is error!");
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
            String t = Hash.GetSha1String(args.get(0).TextValue().getBytes(encoding));
            return Operand.Create(t);
        } catch (Exception e) {
        }
        return Operand.Error("Function SHA1 is error!");
    }

    public Operand visitSHA256_fun(SHA256_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function SHA256 parameter " + (index++) + " is error!");
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
            String t = Hash.GetSha256String(args.get(0).TextValue().getBytes(encoding));
            return Operand.Create(t);
        } catch (Exception e) {
        }
        return Operand.Error("Function SHA256 is error!");
    }

    public Operand visitSHA512_fun(SHA512_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function SHA512 parameter " + (index++) + " is error!");
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
            String t = Hash.GetSha512String(args.get(0).TextValue().getBytes(encoding));
            return Operand.Create(t);
        } catch (Exception e) {
        }
        return Operand.Error("Function SHA512 is error!");
    }

    public Operand visitCRC8_fun(CRC8_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function CRC8 parameter " + (index++) + " is error!");
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
            String t = Hash.GetCrc8String(args.get(0).TextValue().getBytes(encoding));
            return Operand.Create(t);
        } catch (Exception e) {
        }
        return Operand.Error("Function CRC8 is error!");
    }

    public Operand visitCRC16_fun(CRC16_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function CRC16 parameter " + (index++) + " is error!");
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
            String t = Hash.GetCrc16String(args.get(0).TextValue().getBytes(encoding));
            return Operand.Create(t);
        } catch (Exception e) {
        }
        return Operand.Error("Function CRC16 is error!");
    }

    public Operand visitCRC32_fun(CRC32_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function CRC32 parameter " + (index++) + " is error!");
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
            String t = Hash.GetCrc32String(args.get(0).TextValue().getBytes(encoding));
            return Operand.Create(t);
        } catch (Exception e) {
        }
        return Operand.Error("Function CRC32 is error!");
    }

    public Operand visitHMACMD5_fun(HMACMD5_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function HMACMD5 parameter " + (index++) + " is error!");
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
            String t = Hash.GetHmacMd5String(args.get(0).TextValue().getBytes(encoding), args.get(1).TextValue());
            return Operand.Create(t);
        } catch (Exception e) {
        }
        return Operand.Error("Function HMACMD5 is error!");
    }

    public Operand visitHMACSHA1_fun(HMACSHA1_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function HMACSHA1 parameter " + (index++) + " is error!");
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
            String t = Hash.GetHmacSha1String(args.get(0).TextValue().getBytes(encoding), args.get(1).TextValue());
            return Operand.Create(t);
        } catch (Exception e) {
        }
        return Operand.Error("Function HMACSHA1 is error!");
    }

    public Operand visitHMACSHA256_fun(HMACSHA256_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function HMACSHA256 parameter " + (index++) + " is error!");
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
            String t = Hash.GetHmacSha256String(args.get(0).TextValue().getBytes(encoding), args.get(1).TextValue());
            return Operand.Create(t);
        } catch (Exception e) {
        }
        return Operand.Error("Function HMACSHA256 is error!");
    }

    public Operand visitHMACSHA512_fun(HMACSHA512_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function HMACSHA512 parameter " + (index++) + " is error!");
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
            String t = Hash.GetHmacSha512String(args.get(0).TextValue().getBytes(encoding), args.get(1).TextValue());
            return Operand.Create(t);
        } catch (Exception e) {
        }
        return Operand.Error("Function HMACSHA512 is error!");
    }

    public Operand visitTRIMSTART_fun(TRIMSTART_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function TRIMSTART parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        String text = args.get(0).TextValue();
        if (args.size() == 2) {
            if(text.startsWith(args.get(1).TextValue())){
                return Operand.Create(text.substring(args.get(1).TextValue().length()));
            }
            return Operand.Create(text);
        }
        text= text.replaceAll("\\s+$", "");
        return Operand.Create(text);
    }

    public Operand visitTRIMEND_fun(TRIMEND_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function TRIMEND parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        String text = args.get(0).TextValue();
        if (args.size() == 2) {
            return Operand.Create(text.trimEnd(args.get(1).TextValue().ToArray()));
        }
        return Operand.Create(text.trimEnd());
    }

    public Operand visitINDEXOF_fun(INDEXOF_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToText("Function INDEXOF parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        Operand secondValue = args.get(1).ToText("Function INDEXOF parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        String text = firstValue.TextValue();
        if (args.size() == 2) {
            return Operand.Create(text.indexOf(secondValue.TextValue()) + excelIndex);
        }
        Operand thirdValue = args.get(2).ToText("Function INDEXOF parameter 3 is error!");
        if (thirdValue.IsError()) {
            return thirdValue;
        }
        if (args.size() == 3) {
            return Operand.Create(text.indexOf(secondValue.TextValue(), thirdValue.IntValue()) + excelIndex);
        }
        Operand fourthValue = args.get(3).ToText("Function INDEXOF parameter 4 is error!");
        if (fourthValue.IsError()) {
            return fourthValue;
        }
        return Operand.Create(
                text.indexOf(secondValue.TextValue(), thirdValue.IntValue(), fourthValue.IntValue()) + excelIndex);
    }

    public Operand visitLASTINDEXOF_fun(LASTINDEXOF_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToText("Function LASTINDEXOF parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        Operand secondValue = args.get(1).ToText("Function LASTINDEXOF parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        String text = firstValue.TextValue();
        if (args.size() == 2) {
            return Operand.Create(text.lastIndexOf(secondValue.TextValue()) + excelIndex);
        }
        Operand thirdValue = args.get(2).ToText("Function LASTINDEXOF parameter 3 is error!");
        if (thirdValue.IsError()) {
            return thirdValue;
        }
        if (args.size() == 3) {
            return Operand.Create(text.lastIndexOf(secondValue.TextValue(), thirdValue.IntValue()) + excelIndex);
        }
        Operand fourthValue = args.get(3).ToText("Function LASTINDEXOF parameter 4 is error!");
        if (fourthValue.IsError()) {
            return fourthValue;
        }
        return Operand.Create(
                text.lastIndexOf(secondValue.TextValue(), thirdValue.IntValue(), fourthValue.IntValue()) + excelIndex);
    }

    public Operand visitSPLIT_fun(SPLIT_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function SPLIT parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }
        String[] txts=args.get(0).TextValue().split(args.get(1).TextValue());
        List<Operand> array = new ArrayList<Operand>();
        for (int i = 0; i < txts.length; i++) {
            array.add(Operand.Create(txts[i]));
        }
        return Operand.Create(array);
    }

    public Operand visitJOIN_fun(JOIN_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0);
        if (firstValue.Type() == OperandType.JSON) {
            Operand o = firstValue.ToArray(null);
            if (o.IsError() == false) {
                firstValue = o;
            }
        }
        if (firstValue.Type() == OperandType.ARRARY) {
            List<String> list = new ArrayList<String>();
            boolean o = F_base_GetList(firstValue, list);
            if (o == false)
                return Operand.Error("Function JOIN parameter 1 is error!");

            Operand secondValue = args.get(1).ToText("Function JOIN parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }

            return Operand.Create(String.join(secondValue.TextValue(), list));
        } else {
            firstValue = firstValue.ToText("Function JOIN parameter 1 is error!");
            if (firstValue.IsError()) {
                return firstValue;
            }

            List<String> list = new ArrayList<String>();
            for (int i = 1; i < args.size(); i++) {
                boolean o = F_base_GetList(args.get(i), list);
                if (o == false)
                    return Operand.Error("Function JOIN parameter {i + 1} is error!");
            }

            return Operand.Create(String.join(firstValue.TextValue(), list));
        }
    }

    public Operand visitSUBSTRING_fun(SUBSTRING_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToText("Function SUBSTRING parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        Operand secondValue = args.get(1).ToNumber("Function SUBSTRING parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        String text = firstValue.TextValue();
        if (args.size() == 2) {
            return Operand.Create(text.substring(secondValue.IntValue() - excelIndex));
        }
        Operand thirdValue = args.get(2).ToNumber("Function SUBSTRING parameter 3 is error!");
        if (thirdValue.IsError()) {
            return thirdValue;
        }
        return Operand.Create(text.substring(secondValue.IntValue() - excelIndex, thirdValue.IntValue()));
    }

    public Operand visitSTARTSWITH_fun(STARTSWITH_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToText("Function STARTSWITH parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        Operand secondValue = args.get(1).ToText("Function STARTSWITH parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        String text = firstValue.TextValue();
        if (args.size() == 2) {
            return Operand.Create(text.startsWith(secondValue.TextValue()));
        }
        Operand thirdValue = args.get(2).ToBoolean("Function STARTSWITH parameter 3 is error!");
        if (thirdValue.IsError()) {
            return thirdValue;
        }
        if(thirdValue.BooleanValue()){
            return Operand.Create(text.toLowerCase().startsWith(secondValue.TextValue().toLowerCase()));
        }
        return Operand.Create(text.startsWith(secondValue.TextValue()));
    }

    public Operand visitENDSWITH_fun(ENDSWITH_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToText("Function ENDSWITH parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        Operand secondValue = args.get(1).ToText("Function ENDSWITH parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        String text = firstValue.TextValue();
        if (args.size() == 2) {
            return Operand.Create(text.endsWith(secondValue.TextValue()));
        }
        Operand thirdValue = args.get(2).ToBoolean("Function ENDSWITH parameter 3 is error!");
        ;
        if (thirdValue.IsError()) {
            return thirdValue;
        }
        if(thirdValue.BooleanValue() ){
            return Operand.Create(text.toLowerCase().endsWith(secondValue.TextValue().toLowerCase()));
        }
        return Operand.Create(text.endsWith(secondValue.TextValue()));
    }

    public Operand visitISNULLOREMPTY_fun(ISNULLOREMPTY_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function ISNULLOREMPTY parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        boolean b=false;
        if(firstValue.TextValue()==null || firstValue.TextValue().equals("")){
            b=true;
        }
        return Operand.Create(b);
        // return Operand.Create(String.IsNullOrEmpty(firstValue.TextValue()));
    }

    public Operand visitISNULLORWHITESPACE_fun(ISNULLORWHITESPACE_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function ISNULLORWHITESPACE parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        boolean b=false;
        if(firstValue.TextValue()==null || firstValue.TextValue().trim().equals("")){
            b=true;
        }
        return Operand.Create(b);
    }

    public Operand visitREMOVESTART_fun(REMOVESTART_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToText("Function REMOVESTART parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        Operand secondValue = args.get(1).ToText("Function REMOVESTART parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }
        String text = firstValue.TextValue();
        if (args.size() == 3) {
            Operand thirdValue = args.get(2).ToBoolean("Function REMOVESTART parameter 3 is error!");
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

    public Operand visitREMOVEEND_fun(REMOVEEND_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToText("Function REMOVEEND parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        Operand secondValue = args.get(1).ToText("Function REMOVEEND parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }

        String text = firstValue.TextValue();
        // StringComparison comparison = StringComparison.Ordinal;
        if (args.size() == 3) {
            Operand thirdValue = args.get(2).ToBoolean("Function REMOVESTART parameter 3 is error!");
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

    public Operand visitJSON_fun(JSON_funContext context)
    {
        Operand firstValue = visit(context.expr()).ToText("Function JSON parameter is error!");
        if (firstValue.IsError()) { return firstValue; }
        String txt = firstValue.TextValue();
        if ((txt.startsWith("{") && txt.endsWith("}")) || (txt.startsWith("[") && txt.endsWith("]"))) {
            try {
                JsonData json = JsonMapper.ToObject(txt);
                return Operand.Create(json);
            } catch (Exception e) { }
        }
        return Operand.Error("Function JSON parameter is error!");
    }

    public Operand visitVLOOKUP_fun(VLOOKUP_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0).ToArray("Function VLOOKUP parameter 1 error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        Operand secondValue = args.get(1);

        Operand thirdValue = args.get(2).ToNumber("Function VLOOKUP parameter 3 is error!");
        if (thirdValue.IsError()) {
            return thirdValue;
        }

        boolean vague = true;
        if (args.size() == 4) {
            Operand fourthValue = args.get(2).ToBoolean("Function VLOOKUP parameter 4 is error!");
            if (fourthValue.IsError()) {
                return fourthValue;
            }
            vague = fourthValue.BooleanValue();
        }
        if (secondValue.Type() != OperandType.NULL) {
            Operand sv = secondValue.ToText("Function VLOOKUP parameter 2 is error!");
            if (sv.IsError()) {
                return sv;
            }
            secondValue = sv;
        }
        for (Operand item : firstValue.ArrayValue()) {
            Operand o = item.ToArray("Function VLOOKUP parameter 1 error!");
            if (o.IsError()) {
                return o;
            }
            if (o.ArrayValue().size() > 0) {
                Operand o1 = o.ArrayValue().get(0);// [0];
                int b = -1;
                if (secondValue.Type() == OperandType.NUMBER) {
                    Operand o2 = o1.ToNumber(null);
                    if (o2.IsError() == false) {
                        b = Compare(o2.NumberValue(), secondValue.NumberValue());
                    }
                } else {
                    Operand o2 = o1.ToText(null);
                    if (o2.IsError() == false) {
                        b = String.CompareOrdinal(o2.TextValue(), secondValue.TextValue());
                    }
                }
                if (b == 0) {
                    int index = thirdValue.IntValue() - excelIndex;
                    if (index < o.ArrayValue().size()) {
                        return o.ArrayValue().get(index);// [index];
                    }
                }
            }
        }

        if (vague) // 进行模糊匹配
        {
            Operand last = null;
            int index = thirdValue.IntValue() - excelIndex;
            for (Operand item : firstValue.ArrayValue()) {
                Operand o = item.ToArray(null);
                if (o.IsError()) {
                    return o;
                }
                if (o.ArrayValue().size() > 0) {
                    Operand o1 = o.ArrayValue().get(0);// [0];
                    int b = -1;
                    if (secondValue.Type() == OperandType.NUMBER) {
                        Operand o2 = o1.ToNumber(null);
                        if (o2.IsError() == false) {
                            b = Compare(o2.NumberValue(), secondValue.NumberValue());
                        }
                    } else {
                        Operand o2 = o1.ToText(null);
                        if (o2.IsError() == false) {
                            b = String.CompareOrdinal(o2.TextValue(), secondValue.TextValue());
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

    public Operand visitLOOKUP_fun(LOOKUP_funContext context)
    {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) { Operand aa = visit(item); if (aa.IsError()) { return aa; } args.add(aa); }

        Operand firstValue = args.get(0).ToArray("Function LOOKUP parameter 1 error!");
        if (firstValue.IsError()) { return firstValue; }
        Operand secondValue = args.get(1).ToText("Function LOOKUP parameter 2 is error!");
        if (secondValue.IsError()) { return secondValue; }
        Operand thirdValue = args.get(2).ToText("Function LOOKUP parameter 3 is error!");
        if (thirdValue.IsError()) { return thirdValue; }

        if ( secondValue.TextValue()==null || secondValue.TextValue().equals("") ){
            return Operand.Error("Function LOOKUP parameter 2 is null!");
        }

        LookupAlgorithmEngine engine = new LookupAlgorithmEngine();
        if (engine.Parse(secondValue.TextValue()) == false) {
            return Operand.Error("Function LOOKUP parameter 2 Parse is error!");
        }
 
        engine.DiyFunction = DoDiyFunction;

        for (Operand item : firstValue.ArrayValue()) {
            Operand json = item.ToJson(null);
            if (json.IsError() == false) {
                engine.Json = json;
                try {
                    Operand o = engine.Evaluate().ToBoolean(null);
                    if (o.IsError() == false) {
                        if (o.BooleanValue()) {
                            JsonData v = json.JsonValue().GetChild(thirdValue.TextValue());// [thirdValue.TextValue()];
                            if (v != null) {
                                if (v.IsString()) return Operand.Create(v.StringValue);
                                if (v.IsBoolean()) return Operand.Create(v.BooleanValue());
                                if (v.IsDouble()) return Operand.Create(v.NumberValue());
                                if (v.IsObject()) return Operand.Create(v);
                                if (v.IsArray()) return Operand.Create(v);
                                if (v.IsNull()) return Operand.CreateNull();
                                return Operand.Create(v);
                            }
                        }
                    }
                } catch (Exception e) { }
            }
        }
        return Operand.Error("Function LOOKUP not find!");
    }

    private Operand DoDiyFunction(MyFunction fun) {
        if (DiyFunction != null) {
            return DiyFunction.apply(fun);
        }
        return Operand.Error("Function name [" + fun.Name + "] is missing.");
    }

    public Operand visitArray_fun(Array_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }
        return Operand.Create(args);
    }

    public Operand visitBracket_fun(Bracket_funContext context) {
        return visit(context.expr());
    }

    public Operand visitNUM_fun(NUM_funContext context)
    {
        String sub = context.SUB().getText();
        // String sub = context.SUB()?.GetText() ?? "";
        String t = context.NUM().getText();

        Double d=Double.parseDouble(sub + t);
        // double d = double.Parse(sub + t, NumberStyles.Any, cultureInfo);
        return Operand.Create(d);
    }

    public Operand visitSTRING_fun(STRING_funContext context) {
        String opd = context.STRING().getText();
        StringBuilder sb = new StringBuilder();
        int index = 1;
        while (index < opd.length() - 1) {
            char c = opd.charAt(index++);// [index++];
            if (c == '\\') {
                char c2 = opd.charAt(index++);
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

    public Operand visitNULL_fun(NULL_funContext context) {
        return Operand.CreateNull();
    }

    public Operand visitPARAMETER_fun(PARAMETER_funContext context) {
        Operand p = visit(context.parameter()).ToText("Function PARAMETER first parameter is error!");
        if (p.IsError())
            return p;

        if (GetParameter != null) {
            return GetParameter.apply(p.TextValue());
        }
        return Operand.Error("Function PARAMETER first parameter is error!");
    }

    public Operand visitParameter(ParameterContext context) {
        ExprContext expr = context.expr();
        if (expr != null) {
            return visit(expr);
        }
        return visit(context.parameter2());
    }

    public Operand visitParameter2(Parameter2Context context) {
        return Operand.Create(context.children.get(0).getText());
    }

    public Operand visitGetJsonValue_fun(GetJsonValue_funContext context) {
        Operand firstValue = visit(context.expr());
        if (firstValue.IsError()) {
            return firstValue;
        }

        Operand obj = firstValue;
        Operand op;
        ParameterContext p1 = context.parameter();
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
            int index = op.IntValue() - excelIndex;
            if (index < obj.ArrayValue().size())
                return obj.ArrayValue().get(index);// [index];
            return Operand.Error("ARRARY index " + index + " greater than maximum length!");
        }
        if (obj.Type() == OperandType.JSON) {
            JsonData json = obj.JsonValue();
            if (json.IsArray()) {
                op = op.ToNumber("JSON parameter index is error!");
                if (op.IsError()) {
                    return op;
                }
                int index = op.IntValue() - excelIndex;
                if (index < json.inst_array.size()) {
                    JsonData v = json.GetChild(index);// [op.IntValue() - excelIndex];
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
                JsonData v = json.GetChild(op.TextValue());// [op.TextValue()];
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

    public Operand visitExpr2_fun(Expr2_funContext context) {
        return visitChildren(context);
    }

    public Operand visitDiyFunction_fun(DiyFunction_funContext context) {
        if (DiyFunction != null) {
            String funName = context.PARAMETER().getText();
            List<Operand> args = new ArrayList<Operand>();
            for (ExprContext item : context.expr()) {
                Operand aa = visit(item);
                args.add(aa);
            }
            MyFunction myFunction=new MyFunction();
            myFunction.Name=funName;
            myFunction.OperandList=args;
            return DiyFunction.apply(myFunction);
        }
        return Operand.Error("DiyFunction is error!");
    }

    private double round(double value, int p) {
        BigDecimal bigD = new BigDecimal(value);
        return bigD.setScale(p, BigDecimal.ROUND_HALF_UP).doubleValue();
    }
    static int sign(double a){
        if(a==0.0){
            return 0;
        }
        if(a<0){
            return -1;
        }
        return 1;
    }
    private double Sum(List<Double> array){
        double d=0;
        for (int i = 0; i < array.size(); i++) {
            d+=array.get(i);
        }
        return d;
    }
    private double Average(List<Double> array){
        double d=0;
        for (int i = 0; i < array.size(); i++) {
            d+=array.get(i);
        }
        return d/array.size();
    }
    private double Max(List<Double> array){
        double d=Double.MIN_VALUE;
        for (int i = 0; i < array.size(); i++) {
            if(d<array.get(i)){
                d=array.get(i);
            }
        }
        return d;
    }
    private double Min(List<Double> array){
        double d=Double.MAX_VALUE;
        for (int i = 0; i < array.size(); i++) {
            if(d>array.get(i)){
                d=array.get(i);
            }
        }
        return d;
    }
    private String padLeft(String src, int len, char ch) {
        int diff = len - src.length();
        if (diff <= 0) {
            return src;
        }

        char[] charr = new char[len];
        System.arraycopy(src.toCharArray(), 0, charr, 0, src.length());
        for (int i = src.length(); i < len; i++) {
            charr[i] = ch;
        }
        return new String(charr);
    }
    private String padRight(String src, int len, char ch) {
        int diff = len - src.length();
        if (diff <= 0) {
            return src;
        }

        char[] charr = new char[len];
        System.arraycopy(src.toCharArray(), 0, charr, diff, src.length());
        for (int i = 0; i < diff; i++) {
            charr[i] = ch;
        }
        return new String(charr);
    }

}