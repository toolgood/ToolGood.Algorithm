package toolgood.algorithm.internals;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.function.DoubleToIntFunction;
import java.util.function.Function;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import toolgood.algorithm.MyDate;
import toolgood.algorithm.Operand;
import toolgood.algorithm.litJson.JsonData;
import toolgood.algorithm.math.mathParser;
import toolgood.algorithm.math.mathParser2;
import toolgood.algorithm.math.mathParser2.*;
import toolgood.algorithm.math.mathBaseVisitor;
import toolgood.algorithm.internals.MyFunction;

public class MathVisitor extends mathBaseVisitor<Operand> {
    private static Pattern sumifRegex = new Pattern("(<|<=|>|>=|=|==|!=|<>) *([-+]?\\d+(\\.(\\d+)?)?)");
    private static Pattern bit_2 = new Pattern("^[01]+");
    private static Pattern bit_8 = new Pattern("^[0-8]+");
    private static Pattern bit_16 = new Pattern("^[0-9a-fA-F]+");
    private static Pattern clearRegex = new Pattern("[\\f\\n\\r\\t\\v]");
    private static Pattern numberRegex = new Pattern("^-?(0|[1-9])\\d*(\\.\\d+)?");
    private static Locale cultureInfo = Locale.US;
    public Function<String, Operand> GetParameter;
    public Function<MyFunction, Operand> DiyFunction;
    public int excelIndex;

    public Operand VisitProg(ProgContext context) {
        return visitChildren(context);
    }

    public Operand VisitMulDiv_fun(MulDiv_funContext context) {
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
        if (firstValue.Type == OperandType.STRING) {
            if (numberRegex.IsMatch(firstValue.TextValue())) {
                Operand a = firstValue.ToNumber(null);
                if (a.IsError() == false)
                    firstValue = a;
            } else {
                Operand a = firstValue.ToDate(null);
                if (a.IsError() == false)
                    firstValue = a;
            }
        }
        if (secondValue.Type == OperandType.STRING) {
            if (numberRegex.IsMatch(secondValue.TextValue())) {
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
            if (secondValue.Type == OperandType.BOOLEAN) {
                if (secondValue.BooleanValue())
                    return firstValue;
                else
                    return Operand.Create(0);
            } else if (firstValue.Type == OperandType.BOOLEAN) {
                if (firstValue.BooleanValue())
                    return secondValue;
                else
                    return Operand.Create(0);
            }
            if (firstValue.Type == OperandType.DATE) {
                secondValue = secondValue.ToNumber("Function '" + t + "' parameter 2 is error!");
                if (secondValue.IsError()) {
                    return secondValue;
                }
                return Operand.Create((MyDate) (firstValue.DateValue() * secondValue.NumberValue()));
            }
            if (secondValue.Type == OperandType.DATE) {
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
            if (firstValue.Type == OperandType.DATE) {
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

    public Operand VisitAddSub_fun(AddSub_funContext context) {
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
        String t = context.op.Text;

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
        if (firstValue.Type == OperandType.STRING) {
            if (numberRegex.IsMatch(firstValue.TextValue())) {
                Operand a = firstValue.ToNumber(null);
                if (a.IsError() == false)
                    firstValue = a;
            } else {
                Operand a = firstValue.ToDate(null);
                if (a.IsError() == false)
                    firstValue = a;
            }
        }
        if (secondValue.Type == OperandType.STRING) {
            if (numberRegex.IsMatch(secondValue.TextValue())) {
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
            if (firstValue.Type == OperandType.DATE && secondValue.Type == OperandType.DATE) {
                return Operand.Create(firstValue.DateValue() + secondValue.DateValue());
            } else if (firstValue.Type == OperandType.DATE) {
                secondValue = secondValue.ToNumber("Function '" + t + "' parameter 2 is error!");
                if (secondValue.IsError()) {
                    return secondValue;
                }
                return Operand.Create(firstValue.DateValue() + secondValue.NumberValue());
            } else if (secondValue.Type == OperandType.DATE) {
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
            if (firstValue.Type == OperandType.DATE && secondValue.Type == OperandType.DATE) {
                return Operand.Create(firstValue.DateValue() - secondValue.DateValue());
            } else if (firstValue.Type == OperandType.DATE) {
                secondValue = secondValue.ToNumber("Function '" + t + "' parameter 2 is error!");
                if (secondValue.IsError()) {
                    return secondValue;
                }
                return Operand.Create(firstValue.DateValue() - secondValue.NumberValue());
            } else if (secondValue.Type == OperandType.DATE) {
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

    public Operand VisitJudge_fun(Judge_funContext context) {
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
        String type = context.op.Text;

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
        if (firstValue.Type == secondValue.Type) {
            if (firstValue.Type == OperandType.STRING || firstValue.Type == OperandType.JSON) {
                firstValue = firstValue.ToText("Function '" + type + "' parameter 1 is error!");
                if (firstValue.IsError()) {
                    return firstValue;
                }
                secondValue = secondValue.ToText("Function '" + type + "' parameter 2 is error!");
                if (secondValue.IsError()) {
                    return secondValue;
                }

                r = String.CompareOrdinal(firstValue.TextValue(), secondValue.TextValue());
            } else if (firstValue.Type == OperandType.ARRARY) {
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
        } else if ((firstValue.Type == OperandType.DATE && secondValue.Type == OperandType.STRING)
                || (secondValue.Type == OperandType.DATE && firstValue.Type == OperandType.STRING)
                || (firstValue.Type == OperandType.NUMBER && secondValue.Type == OperandType.STRING)
                || (secondValue.Type == OperandType.NUMBER && firstValue.Type == OperandType.STRING)) {
            firstValue = firstValue.ToText("Function '" + type + "' parameter 1 is error!");
            if (firstValue.IsError()) {
                return firstValue;
            }
            secondValue = secondValue.ToText("Function '" + type + "' parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }

            r = String.CompareOrdinal(firstValue.TextValue(), secondValue.TextValue());
        } else if ((firstValue.Type == OperandType.BOOLEAN && secondValue.Type == OperandType.STRING)
                || (secondValue.Type == OperandType.BOOLEAN && firstValue.Type == OperandType.STRING)) {
            firstValue = firstValue.ToText("Function '" + type + "' parameter 1 is error!");
            if (firstValue.IsError()) {
                return firstValue;
            }
            secondValue = secondValue.ToText("Function '" + type + "' parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }

            r = String.Compare(firstValue.TextValue(), secondValue.TextValue(), true);
        } else if (firstValue.Type == OperandType.STRING || secondValue.Type == OperandType.STRING
                || firstValue.Type == OperandType.JSON || secondValue.Type == OperandType.JSON
                || firstValue.Type == OperandType.ARRARY || secondValue.Type == OperandType.ARRARY) {
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

    public Operand VisitAndOr_fun(AndOr_funContext context) {
        String t = context.op.Text;
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
        if (t == "&&" || t.Equals("and", StringComparison.OrdinalIgnoreCase)) {
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

    public Operand VisitIF_fun(IF_funContext context) {
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
            if (args.Count > 1) {
                return args.get(1);
            }
            return Operand.True;
        }
        if (args.Count == 3) {
            return args.get(2);
        }
        return Operand.False;
    }

    public Operand VisitIFERROR_fun(IFERROR_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand a = visit(item);
            args.add(a);
        }

        if (args.get(0).IsError()) {
            return args.get(1);
        }
        if (args.Count == 3) {
            return args.get(2);
        }
        return Operand.False;
    }

    public Operand VisitISNUMBER_fun(ISNUMBER_funContext context) {
        Operand firstValue = visit(context.expr());
        if (firstValue.IsError()) {
            return firstValue;
        }

        if (firstValue.Type == OperandType.NUMBER) {
            return Operand.True;
        }
        return Operand.False;
    }

    public Operand VisitISTEXT_fun(ISTEXT_funContext context) {
        Operand firstValue = visit(context.expr());
        if (firstValue.IsError()) {
            return firstValue;
        }

        if (firstValue.Type == OperandType.STRING) {
            return Operand.True;
        }
        return Operand.False;
    }

    public Operand VisitIsError_fun(IsError_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            args.add(aa);
        }
        if (args.Count == 2) {
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

    public Operand VisitISNULL_fun(ISNULL_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            args.add(aa);
        }

        if (args.Count == 2) {
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

    public Operand VisitISNULLORERROR_fun(ISNULLORERROR_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            args.add(aa);
        }

        if (args.Count == 2) {
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

    public Operand VisitISEVEN_fun(ISEVEN_funContext context) {
        Operand firstValue = visit(context.expr());
        if (firstValue.Type == OperandType.NUMBER) {
            if (firstValue.IntValue() % 2 == 0) {
                return Operand.True;
            }
        }
        return Operand.False;
    }

    public Operand VisitISLOGICAL_fun(ISLOGICAL_funContext context) {
        Operand firstValue = visit(context.expr());
        if (firstValue.Type == OperandType.BOOLEAN) {
            return Operand.True;
        }
        return Operand.False;
    }

    public Operand VisitISODD_fun(ISODD_funContext context) {
        Operand firstValue = visit(context.expr());
        if (firstValue.Type == OperandType.NUMBER) {
            if (firstValue.IntValue() % 2 == 1) {
                return Operand.True;
            }
        }
        return Operand.False;
    }

    public Operand VisitISNONTEXT_fun(ISNONTEXT_funContext context) {
        Operand firstValue = visit(context.expr());
        if (firstValue.Type != OperandType.STRING) {
            return Operand.True;
        }
        return Operand.False;
    }

    public Operand VisitAND_fun(AND_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToBoolean("Function AND parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        Operand b = true;
        for (Operand item : args) {
            if (item.BooleanValue() == false) {
                b = false;
                break;
            }
        }
        return Operand.Create(b);
    }

    public Operand VisitOR_fun(OR_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        Operand index = 1;
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

    public Operand VisitNOT_fun(NOT_funContext context) {
        Operand firstValue = visit(context.expr()).ToBoolean("Function NOT parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(!firstValue.BooleanValue());
    }

    public Operand VisitTRUE_fun(TRUE_funContext context) {
        return Operand.True;
    }

    public Operand VisitFALSE_fun(FALSE_funContext context) {
        return Operand.False;
    }

    public Operand VisitE_fun(E_funContext context) {
        return Operand.Create(Math.E);
    }

    public Operand VisitPI_fun(PI_funContext context) {
        return Operand.Create(Math.PI);
    }

    public Operand VisitABS_fun(ABS_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function ABS parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(Math.abs(firstValue.NumberValue()));
    }

    public Operand VisitQUOTIENT_fun(QUOTIENT_funContext context) {
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

    public Operand VisitMOD_fun(MOD_funContext context) {
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

    public Operand VisitSIGN_fun(SIGN_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function SIGN parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(Math.sign(firstValue.NumberValue()));
    }

    public Operand VisitSQRT_fun(SQRT_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function SQRT parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(Math.sqrt(firstValue.NumberValue()));
    }

    public Operand VisitTRUNC_fun(TRUNC_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function TRUNC parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create((int) (firstValue.NumberValue()));
    }

    public Operand VisitINT_fun(INT_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function INT parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create((int) (firstValue.NumberValue()));
    }

    public Operand VisitGCD_fun(GCD_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new List<Double>();
        boolean o = F_base_GetList(args, list);
        if (o == false) {
            return Operand.Error("Function GCD parameter is error!");
        }

        return Operand.Create(F_base_gcd(list));
    }

    public Operand VisitLCM_fun(LCM_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new ArrayList<Double>();
        boolean o = F_base_GetList(args, list);
        if (o == false) {
            return Operand.Error("Function GCD parameter is error!");
        }

        return Operand.Create(F_base_lgm(list));
    }

    public Operand VisitCOMBIN_fun(COMBIN_funContext context) {
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

    public Operand VisitPERMUT_fun(PERMUT_funContext context) {
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
        int g = F_base_gcd((int) list.get(1), (int) list.get(0));
        for (int i = 2; i < list.Count; i++) {
            g = F_base_gcd((int) list.get(i), g);
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
            int item = list.get(i);// [i];
            if (item <= 1) {
                list.remove(i);
            }
        }
        list = ShellSort(list);

        int a = (int) list.get(0);// [0];
        for (int i = 1; i < list.Count; i++) {
            int b = (int) list.get(i);
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

    public Operand VisitDEGREES_fun(DEGREES_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function DEGREES parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        double z = firstValue.NumberValue();
        double r = (z / Math.PI * 180);
        return Operand.Create(r);
    }

    public Operand VisitRADIANS_fun(RADIANS_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function RADIANS parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        double r = firstValue.NumberValue() / 180 * Math.PI;
        return Operand.Create(r);
    }

    public Operand VisitCOS_fun(COS_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function COS parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(Math.cos(firstValue.NumberValue()));
    }

    public Operand VisitCOSH_fun(COSH_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function COSH parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(Math.cosh(firstValue.NumberValue()));
    }

    public Operand VisitSIN_fun(SIN_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function SIN parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(Math.sin(firstValue.NumberValue()));
    }

    public Operand VisitSINH_fun(SINH_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function SINH parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(Math.sinh(firstValue.NumberValue()));
    }

    public Operand VisitTAN_fun(TAN_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function TAN parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(Math.tan(firstValue.NumberValue()));
    }

    public Operand VisitTANH_fun(TANH_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function TANH parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(Math.tanh(firstValue.NumberValue()));
    }

    public Operand VisitACOS_fun(ACOS_funContext context) {
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

    public Operand VisitACOSH_fun(ACOSH_funContext context) {
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

    public Operand VisitASIN_fun(ASIN_funContext context) {
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

    public Operand VisitASINH_fun(ASINH_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function ASINH parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        double x = firstValue.NumberValue();
        double d = Math.log(x + Math.sqrt(x * x + 1));
        return Operand.Create(d);
    }

    public Operand VisitATAN_fun(ATAN_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function ATAN parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(Math.atan(firstValue.NumberValue()));
    }

    public Operand VisitATANH_fun(ATANH_funContext context) {
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

    public Operand VisitATAN2_fun(ATAN2_funContext context) {
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

    public Operand VisitFIXED_fun(FIXED_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        int num = 2;
        if (args.Count > 1) {
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
        if (args.Count == 3) {
            Operand thirdValue = args.get(2).ToBoolean("Function FIXED parameter 3 is error!");
            if (thirdValue.IsError()) {
                return thirdValue;
            }
            no = thirdValue.BooleanValue();
        }
        if (no == false) {
            return Operand.Create(s.toString("N" + num, cultureInfo));
        }
        return Operand.Create(s.toString(cultureInfo));
    }

    public Operand VisitBIN2OCT_fun(BIN2OCT_funContext context) {
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

        if (bit_2.IsMatch(firstValue.TextValue()) == false) {
            return Operand.Error("Function BIN2OCT parameter 1 is error!");
        }
        String num = Convert.toString(Convert.ToInt32(firstValue.TextValue(), 2), 8);
        if (args.Count == 2) {
            Operand secondValue = args.get(1).ToNumber("Function BIN2OCT parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (num.Length > secondValue.IntValue()) {
                return Operand.Create(num.PadLeft(secondValue.IntValue(), '0'));
            }
            return Operand.Error("Function BIN2OCT parameter 2 is error!");
        }
        return Operand.Create(num);
    }

    public Operand VisitBIN2DEC_fun(BIN2DEC_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function BIN2DEC parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        if (bit_2.IsMatch(firstValue.TextValue()) == false) {
            return Operand.Error("Function BIN2DEC parameter is error!");
        }
        String num = Convert.ToInt32(firstValue.TextValue(), 2);
        return Operand.Create(num);
    }

    public Operand VisitBIN2HEX_fun(BIN2HEX_funContext context) {
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

        if (bit_2.IsMatch(firstValue.TextValue()) == false) {
            return Operand.Error("Function BIN2HEX parameter 1 is error!");
        }
        String num = Convert.toString(Convert.ToInt32(firstValue.TextValue(), 2), 16).ToUpper();
        if (args.Count == 2) {
            Operand secondValue = args.get(1).ToNumber("Function BIN2HEX parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (num.Length > secondValue.IntValue()) {
                return Operand.Create(num.PadLeft(secondValue.IntValue(), '0'));
            }
            return Operand.Error("Function BIN2HEX parameter 2 is error!");
        }
        return Operand.Create(num);
    }

    public Operand VisitOCT2BIN_fun(OCT2BIN_funContext context) {
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

        if (bit_8.IsMatch(firstValue.TextValue()) == false) {
            return Operand.Error("Function OCT2BIN parameter 1 is error!");
        }
        String num = Convert.toString(Convert.ToInt32(firstValue.TextValue(), 8), 2);
        if (args.Count == 2) {
            Operand secondValue = args.get(1).ToNumber("Function OCT2BIN parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (num.Length > secondValue.IntValue()) {
                return Operand.Create(num.PadLeft(secondValue.IntValue(), '0'));
            }
            return Operand.Error("Function OCT2BIN parameter 2 is error!");
        }
        return Operand.Create(num);
    }

    public Operand VisitOCT2DEC_fun(OCT2DEC_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function OCT2DEC parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        if (bit_8.IsMatch(firstValue.TextValue()) == false) {
            return Operand.Error("Function OCT2DEC parameter is error!");
        }
        String num = Convert.ToInt32(firstValue.TextValue(), 8);
        return Operand.Create(num);
    }

    public Operand VisitOCT2HEX_fun(OCT2HEX_funContext context) {
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
        if (bit_8.IsMatch(firstValue.TextValue()) == false) {
            return Operand.Error("Function OCT2HEX parameter 1 is error!");
        }
        String num = Convert.toString(Convert.ToInt32(firstValue.TextValue(), 8), 16).ToUpper();
        if (args.Count == 2) {
            Operand secondValue = args.get(1).ToNumber("Function OCT2HEX parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (num.Length > secondValue.IntValue()) {
                return Operand.Create(num.PadLeft(secondValue.IntValue(), '0'));
            }
            return Operand.Error("Function OCT2HEX parameter 2 is error!");
        }
        return Operand.Create(num);
    }

    public Operand VisitDEC2BIN_fun(DEC2BIN_funContext context) {
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
        if (args.Count == 2) {
            Operand secondValue = args.get(1).ToNumber("Function DEC2BIN parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (num.Length > secondValue.IntValue()) {
                return Operand.Create(num.PadLeft(secondValue.IntValue(), '0'));
            }
            return Operand.Error("Function DEC2BIN parameter 2 is error!");
        }
        return Operand.Create(num);
    }

    public Operand VisitDEC2OCT_fun(DEC2OCT_funContext context) {
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
        if (args.Count == 2) {
            Operand secondValue = args.get(1).ToNumber("Function DEC2OCT parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (num.Length > secondValue.IntValue()) {
                return Operand.Create(num.PadLeft(secondValue.IntValue(), '0'));
            }
            return Operand.Error("Function DEC2OCT parameter 2 is error!");
        }
        return Operand.Create(num);
    }

    public Operand VisitDEC2HEX_fun(DEC2HEX_funContext context) {
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
        if (args.Count == 2) {
            Operand secondValue = args.get(1).ToNumber("Function DEC2HEX parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (num.Length > secondValue.IntValue()) {
                return Operand.Create(num.PadLeft(secondValue.IntValue(), '0'));
            }
            return Operand.Error("Function DEC2HEX parameter 2 is error!");
        }
        return Operand.Create(num);
    }

    public Operand VisitHEX2BIN_fun(HEX2BIN_funContext context) {
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
        if (bit_16.IsMatch(firstValue.TextValue()) == false) {
            return Operand.Error("Function HEX2BIN parameter 1 is error!");
        }

        String num = Convert.toString(Convert.ToInt32(firstValue.TextValue(), 16), 2);
        if (args.Count == 2) {
            Operand secondValue = args.get(1).ToNumber("Function HEX2BIN parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (num.Length > secondValue.IntValue()) {
                return Operand.Create(num.PadLeft(secondValue.IntValue(), '0'));
            }
            return Operand.Error("Function HEX2BIN parameter 2 is error!");
        }
        return Operand.Create(num);
    }

    public Operand VisitHEX2OCT_fun(HEX2OCT_funContext context) {
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
        if (bit_16.IsMatch(firstValue.TextValue()) == false) {
            return Operand.Error("Function HEX2OCT parameter 1 is error!");
        }
        String num = Convert.toString(Convert.ToInt32(firstValue.TextValue(), 16), 8);
        if (args.Count == 2) {
            Operand secondValue = args.get(1).ToNumber("Function HEX2OCT parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }
            if (num.Length > secondValue.IntValue()) {
                return Operand.Create(num.PadLeft(secondValue.IntValue(), '0'));
            }
            return Operand.Error("Function HEX2OCT parameter 2 is error!");
        }
        return Operand.Create(num);
    }

    public Operand VisitHEX2DEC_fun(HEX2DEC_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function HEX2DEC parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        if (bit_16.IsMatch(firstValue.TextValue()) == false) {
            return Operand.Error("Function HEX2DEC parameter is error!");
        }
        String num = Convert.ToInt32(firstValue.TextValue(), 16);
        return Operand.Create(num);
    }

    public Operand VisitROUND_fun(ROUND_funContext context) {
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

        return Operand.Create((double) round((decimal) firstValue.NumberValue(), secondValue.IntValue(),
                MidpointRounding.AwayFromZero));
    }

    public Operand VisitROUNDDOWN_fun(ROUNDDOWN_funContext context) {
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

    public Operand VisitROUNDUP_fun(ROUNDUP_funContext context) {
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

    public Operand VisitCEILING_fun(CEILING_funContext context) {
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
        if (args.Count == 1)
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

    public Operand VisitFLOOR_fun(FLOOR_funContext context) {
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
        if (args.Count == 1)
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

    public Operand VisitEVEN_fun(EVEN_funContext context) {
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

    public Operand VisitODD_fun(ODD_funContext context) {

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

    public Operand VisitMROUND_fun(MROUND_funContext context) {
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

    public Operand VisitRAND_fun(RAND_funContext context) {
        long tick = DateTime.Now.Ticks;
        Random rand = new Random((int) (tick & 0xffffffffL) | (int) (tick >> 32));
        return Operand.Create(rand.NextDouble());
    }

    public Operand VisitRANDBETWEEN_fun(RANDBETWEEN_funContext context) {
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

        long tick = DateTime.Now.Ticks;
        Random rand = new Random((int) (tick & 0xffffffffL) | (int) (tick >> 32));
        return Operand.Create(
                rand.NextDouble() * (secondValue.NumberValue() - firstValue.NumberValue()) + firstValue.NumberValue());
    }

    public Operand VisitFACT_fun(FACT_funContext context) {
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

    public Operand VisitFACTDOUBLE_fun(FACTDOUBLE_funContext context) {
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

    public Operand VisitPOWER_fun(POWER_funContext context) {
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

    public Operand VisitEXP_fun(EXP_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function EXP parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(Math.exp(firstValue.NumberValue()));
    }

    public Operand VisitLN_fun(LN_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function LN parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(Math.log(firstValue.NumberValue()));
    }

    public Operand VisitLOG_fun(LOG_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToNumber("Function LOG parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        if (args.Count > 1) {
            return Operand.Create(Math.log(args.get(0).NumberValue(), args.get(1).NumberValue()));
        }
        return Operand.Create(Math.log(args.get(0).NumberValue(), 10));
    }

    public Operand VisitLOG10_fun(LOG10_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function LOG10 parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(Math.log(firstValue.NumberValue(), 10));
    }

    public Operand VisitMULTINOMIAL_fun(MULTINOMIAL_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new List<Double>();
        boolean o = F_base_GetList(args, list);
        if (o == false) {
            return Operand.Error("Function MULTINOMIAL parameter is error!");
        }

        int sum = 0;
        int n = 1;
        for (Double a : list) {
            n *= F_base_Factorial((int) a);
            sum += (int) a;
        }
        int r = F_base_Factorial(sum) / n;
        return Operand.Create(r);
    }

    public Operand VisitPRODUCT_fun(PRODUCT_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new List<Double>();
        boolean o = F_base_GetList(args, list);
        if (o == false) {
            return Operand.Error("Function PRODUCT parameter is error!");
        }

        double d = 1;
        for (double a : list) {
            d *= a;
        }

        return Operand.Create(d);
    }

    public Operand VisitSQRTPI_fun(SQRTPI_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function SQRTPI parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(Math.sqrt(firstValue.NumberValue() * Math.PI));
    }

    public Operand VisitSUMSQ_fun(SUMSQ_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new List<Double>();
        boolean o = F_base_GetList(args, list);
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

    public Operand VisitASC_fun(ASC_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function ASC parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(F_base_ToDBC(firstValue.TextValue()));
    }

    public Operand VisitJIS_fun(JIS_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function JIS parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(F_base_ToSBC(firstValue.TextValue()));
    }

    public Operand VisitCHAR_fun(CHAR_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function CHAR parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        char c = (char) (int) firstValue.NumberValue();
        return Operand.Create(c.toString());
    }

    public Operand VisitCLEAN_fun(CLEAN_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function CLEAN parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        String t = firstValue.TextValue();
        t = clearRegex.Replace(t, "");
        return Operand.Create(t);
    }

    public Operand VisitCODE_fun(CODE_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function CODE parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        String t = firstValue.TextValue();
        if (t.Length == 0) {
            return Operand.Error("Function CODE parameter is error!");
        }
        return Operand.Create((double) (int) (char) t.charAt(0));
    }

    public Operand VisitCONCATENATE_fun(CONCATENATE_funContext context) {
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

    public Operand VisitEXACT_fun(EXACT_funContext context) {
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

    public Operand VisitFIND_fun(FIND_funContext context) {
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

        if (args.Count == 2) {
            int p = secondValue.TextValue().IndexOf(firstValue.TextValue()) + excelIndex;
            return Operand.Create(p);
        }
        Operand count = args.get(2).ToNumber("Function FIND parameter 3 is error!");
        if (count.IsError()) {
            return count;
        }
        int p2 = secondValue.TextValue().IndexOf(firstValue.TextValue(), count.IntValue()) + excelIndex;
        return Operand.Create(p2);
    }

    public Operand VisitLEFT_fun(LEFT_funContext context) {
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

        if (args.Count == 1) {
            return Operand.Create(firstValue.TextValue().charAt(0).toString());
        }
        Operand secondValue = args.get(1).ToNumber("Function LEFT parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }
        return Operand.Create(firstValue.TextValue().Substring(0, secondValue.IntValue()));
    }

    public Operand VisitLEN_fun(LEN_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function LEN parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(firstValue.TextValue().Length);
    }

    public Operand VisitLOWER_fun(LOWER_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function LOWER/TOLOWER parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(firstValue.TextValue().ToLower());
    }

    public Operand VisitMID_fun(MID_funContext context) {
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
                .Create(firstValue.TextValue().Substring(secondValue.IntValue() - excelIndex, thirdValue.IntValue()));
    }

    public Operand VisitPROPER_fun(PROPER_funContext context)
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

    public Operand VisitREPLACE_fun(REPLACE_funContext context) {
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
        if (args.Count == 3) {
            Operand secondValue2 = args.get(1).ToText("Function REPLACE parameter 2 is error!");
            Operand thirdValue2 = args.get(2).ToText("Function REPLACE parameter 3 is error!");
            String old = secondValue2.TextValue();
            String newstr = thirdValue2.TextValue();
            return Operand.Create(oldtext.Replace(old, newstr));
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
        for (int i = 0; i < oldtext.Length; i++) {
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

    public Operand VisitREPT_fun(REPT_funContext context) {
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

    public Operand VisitRIGHT_fun(RIGHT_funContext context) {
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

        if (args.Count == 1) {
            return Operand.Create(firstValue.TextValue().charAt(firstValue.TextValue().Length - 1).toString());
        }
        Operand secondValue = args.get(1).ToNumber("Function RIGHT parameter 2 is error!");
        if (secondValue.IsError()) {
            return secondValue;
        }
        return Operand.Create(firstValue.TextValue().Substring(firstValue.TextValue().Length - secondValue.IntValue(),
                secondValue.IntValue()));
    }

    public Operand VisitRMB_fun(RMB_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function RMB parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(F_base_ToChineseRMB(firstValue.NumberValue()));
    }

    public Operand VisitSEARCH_fun(SEARCH_funContext context) {
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
        if (args.Count == 2) {
            int p = secondValue.TextValue().IndexOf(firstValue.TextValue(), StringComparison.OrdinalIgnoreCase)
                    + excelIndex;
            return Operand.Create(p);
        }
        Operand thirdValue = args.get(2).ToNumber("Function SEARCH parameter 3 is error!");
        if (thirdValue.IsError()) {
            return thirdValue;
        }
        int p2 = secondValue.TextValue().IndexOf(firstValue.TextValue(), thirdValue.IntValue(),
                StringComparison.OrdinalIgnoreCase) + excelIndex;
        return Operand.Create(p2);
    }

    public Operand VisitSUBSTITUTE_fun(SUBSTITUTE_funContext context) {
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
        if (args.Count == 3) {
            return Operand.Create(firstValue.TextValue().Replace(secondValue.TextValue(), thirdValue.TextValue()));
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
        for (int i = 0; i < text.Length; i++) {
            boolean b = true;
            for (int j = 0; j < oldtext.Length; j++) {
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
                i += oldtext.Length - 1;
            } else {
                sb.append(text.charAt(i));
            }
        }
        return Operand.Create(sb.toString());
    }

    public Operand VisitT_fun(T_funContext context) {
        Operand firstValue = visit(context.expr());
        if (firstValue.Type == OperandType.STRING) {
            return firstValue;
        }
        return Operand.Create("");
    }

    public Operand VisitTEXT_fun(TEXT_funContext context) {
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

        if (firstValue.Type == OperandType.STRING) {
            return firstValue;
        } else if (firstValue.Type == OperandType.BOOLEAN) {
            return Operand.Create(firstValue.BooleanValue() ? "TRUE" : "FALSE");
        } else if (firstValue.Type == OperandType.NUMBER) {
            return Operand.Create(firstValue.NumberValue().toString(secondValue.TextValue(), cultureInfo));
        } else if (firstValue.Type == OperandType.DATE) {
            return Operand.Create(firstValue.DateValue().toString(secondValue.TextValue()));
        }
        firstValue = firstValue.ToText("Function TEXT parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(firstValue.TextValue().toString());
    }

    public Operand VisitTRIM_fun(TRIM_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function TRIM parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(firstValue.TextValue().trim());
    }

    public Operand VisitUPPER_fun(UPPER_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function UPPER/TOUPPER parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }
        return Operand.Create(firstValue.TextValue().ToUpper());
    }

    public Operand VisitVALUE_fun(VALUE_funContext context)
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
        for (int i = 0; i < input.Length; i++) {
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
        for (int i = 0; i < input.Length; i++) {
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

    private String F_base_ToChineseRMB(double x) {
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
        String tempNumber = String.valueOf(round((number * 100)));
        int tempNumberLength = tempNumber.length();
        if ("0".equals(tempNumber)) {
            return "零圆整";
        }
        if (tempNumberLength > 15) {
            throw new InputException("超出转化范围.");
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

    public Operand VisitDATEVALUE_fun(DATEVALUE_funContext context)
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

    public Operand VisitTIMEVALUE_fun(TIMEVALUE_funContext context)
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

    public Operand VisitDATE_fun(DATE_funContext context) {
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
        if (args.Count == 3) {
            d = new MyDate(args.get(0).IntValue(), args.get(1).IntValue(), args.get(2).IntValue(), 0, 0, 0);
        } else if (args.Count == 4) {
            d = new MyDate(args.get(0).IntValue(), args.get(1).IntValue(), args.get(2).IntValue(), args.get(3).IntValue(),
                    0, 0);
        } else if (args.Count == 5) {
            d = new MyDate(args.get(0).IntValue(), args.get(1).IntValue(), args.get(2).IntValue(), args.get(3).IntValue(),
                    args.get(4).IntValue(), 0);
        } else {
            d = new MyDate(args.get(0).IntValue(), args.get(1).IntValue(), args.get(2).IntValue(), args.get(3).IntValue(),
                    args.get(4).IntValue(), args.get(5).IntValue());
        }
        return Operand.Create(d);
    }

    public Operand VisitTIME_fun(TIME_funContext context) {
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
        if (args.Count == 3) {
            d = new MyDate(0, 0, 0, args.get(0).IntValue(), args.get(1).IntValue(), args.get(2).IntValue());
        } else {
            d = new MyDate(0, 0, 0, args.get(0).IntValue(), args.get(1).IntValue(), 0);
        }
        return Operand.Create(d);
    }

    public Operand VisitNOW_fun(NOW_funContext context) {
        return Operand.Create(new MyDate(new Date()));
    }

    public Operand VisitTODAY_fun(TODAY_funContext context) {
        return Operand.Create(new MyDate(DateTime.Today));
    }

    public Operand VisitYEAR_fun(YEAR_funContext context) {
        Operand firstValue = visit(context.expr()).ToDate("Function YEAR parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(firstValue.DateValue().Year);
    }

    public Operand VisitMONTH_fun(MONTH_funContext context) {
        Operand firstValue = visit(context.expr()).ToDate("Function MONTH parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(firstValue.DateValue().Month);
    }

    public Operand VisitDAY_fun(DAY_funContext context) {
        Operand firstValue = visit(context.expr()).ToDate("Function DAY parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(firstValue.DateValue().Day);
    }

    public Operand VisitHOUR_fun(HOUR_funContext context) {
        Operand firstValue = visit(context.expr()).ToDate("Function HOUR parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(firstValue.DateValue().Hour);
    }

    public Operand VisitMINUTE_fun(MINUTE_funContext context) {
        Operand firstValue = visit(context.expr()).ToDate("Function MINUTE parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(firstValue.DateValue().Minute);
    }

    public Operand VisitSECOND_fun(SECOND_funContext context) {
        Operand firstValue = visit(context.expr()).ToDate("Function SECOND parameter is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(firstValue.DateValue().Second);
    }

    public Operand VisitWEEKDAY_fun(WEEKDAY_funContext context) {
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
        if (args.Count == 2) {
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

    public Operand VisitDATEDIF_fun(DATEDIF_funContext context) {
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
        String t = thirdValue.TextValue().ToLower();

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

    public Operand VisitDAYS360_fun(DAYS360_funContext context) {
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
        if (args.Count == 3) {
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

    public Operand VisitEDATE_fun(EDATE_funContext context) {
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

    public Operand VisitEOMONTH_fun(EOMONTH_funContext context) {
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

    public Operand VisitNETWORKDAYS_fun(NETWORKDAYS_funContext context) {
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
        for (int i = 2; i < args.Count; i++) {
            list.add(args.get(i).DateValue());
        }

        int days = 0;
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

    public Operand VisitWORKDAY_fun(WORKDAY_funContext context) {
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
        for (int i = 2; i < args.Count; i++) {
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

    public Operand VisitWEEKNUM_fun(WEEKNUM_funContext context) {
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
        if (args.Count == 2) {
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

    public Operand VisitMAX_fun(MAX_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function MAX parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new List<Double>();
        boolean o = F_base_GetList(args, list);
        if (o == false) {
            return Operand.Error("Function MAX parameter error!");
        }

        return Operand.Create(list.Max());
    }

    public Operand VisitMEDIAN_fun(MEDIAN_funContext context)
    {
        List<Operand> args = new ArrayList<Operand>(); int index = 1;
        for (ExprContext item : context.expr()) { Operand aa = visit(item).ToNumber("Function MEDIAN parameter "+(index++)+" is error!"); if (aa.IsError()) { return aa; } args.add(aa); }

        List<Double>  list = new List<Double> ();
        boolean o = F_base_GetList(args, list);
        if (o == false) { return Operand.Error("Function MEDIAN parameter error!"); }

        list=ShellSort(list);
        // list = list.OrderBy(q => q).ToList();
        return Operand.Create(list[list.Count / 2]);
    }

    public Operand VisitMIN_fun(MIN_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item).ToNumber("Function MIN parameter " + (index++) + " is error!");
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new List<Double>();
        boolean o = F_base_GetList(args, list);
        if (o == false) {
            return Operand.Error("Function MIN parameter error!");
        }

        return Operand.Create(list.Min());
    }

    public Operand VisitQUARTILE_fun(QUARTILE_funContext context) {
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

        List<Double> list = new List<Double>();
        boolean o = F_base_GetList(firstValue, list);
        if (o == false) {
            return Operand.Error("Function QUARTILE parameter 1 error!");
        }

        int quant = secondValue.IntValue();
        if (quant < 0 || quant > 4) {
            return Operand.Error("Function QUARTILE parameter 2 is error!");
        }
        return Operand.Create(ExcelFunctions.Quartile(list.ToArray(), quant));
    }

    public Operand VisitMODE_fun(MODE_funContext context)
    {
        List<Operand> args = new ArrayList<Operand>(); int index = 1;
        for (ExprContext item : context.expr()) { Operand aa = visit(item).ToNumber("Function MODE parameter "+(index++)+" is error!"); if (aa.IsError()) { return aa; } args.add(aa); }

        List<Double>  list = new List<Double> ();
        boolean o = F_base_GetList(args, list);
        if (o == false) { return Operand.Error("Function MODE parameter error!"); }


        Map<Double, Integer> dict = new HashMap<Double, Integer>();
        for (double item : list) { 
            if (dict.containsKey(item)) {
                dict[item] += 1;
            } else {
                dict[item] = 1;
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

    public Operand VisitLARGE_fun(LARGE_funContext context)
    {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) { Operand aa = visit(item); if (aa.IsError()) { return aa; } args.add(aa); }

        Operand firstValue = args.get(0).ToArray("Function LARGE parameter 1 is error!");
        if (firstValue.IsError()) { return firstValue; }
        Operand secondValue = args.get(1).ToNumber("Function LARGE parameter 2 is error!");
        if (secondValue.IsError()) { return secondValue; }


        List<Double>  list = new List<Double> ();
        boolean o = F_base_GetList(args, list);
        if (o == false) { return Operand.Error("Function LARGE parameter error!"); }

        list=ShellSort(list);
        // list = list.OrderByDescending(q => q).ToList();
        int quant = secondValue.IntValue();
        return Operand.Create(list.get(list.size()-1-(secondValue.IntValue() - excelIndex)));
    }

    public Operand VisitSMALL_fun(SMALL_funContext context)
    {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) { Operand aa = visit(item); if (aa.IsError()) { return aa; } args.add(aa); }

        Operand firstValue = args.get(0).ToArray("Function SMALL parameter 1 is error!");
        if (firstValue.IsError()) { return firstValue; }
        Operand secondValue = args.get(1).ToNumber("Function SMALL parameter 2 is error!");
        if (secondValue.IsError()) { return secondValue; }

        List<Double>  list = new List<Double> ();
        boolean o = F_base_GetList(args, list);
        if (o == false) { return Operand.Error("Function SMALL parameter error!"); }

        list=ShellSort(list);
        // list = list.OrderBy(q => q).ToList();
        int quant = secondValue.IntValue();
        return Operand.Create(list.get(secondValue.IntValue() - excelIndex) );
        // return Operand.Create(list[secondValue.IntValue() - excelIndex]);
    }

    public Operand VisitPERCENTILE_fun(PERCENTILE_funContext context) {
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

        List<Double> list = new List<Double>();
        boolean o = F_base_GetList(firstValue, list);
        if (o == false) {
            return Operand.Error("Function PERCENTILE parameter error!");
        }

        double k = secondValue.NumberValue();
        return Operand.Create(ExcelFunctions.Percentile(list.ToArray(), k));
    }

    public Operand VisitPERCENTRANK_fun(PERCENTRANK_funContext context) {
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

        List<Double> list = new List<Double>();
        boolean o = F_base_GetList(firstValue, list);
        if (o == false) {
            return Operand.Error("Function PERCENTRANK parameter error!");
        }

        double k = secondValue.NumberValue();
        double v = ExcelFunctions.PercentRank(list.ToArray(), k);
        int d = 3;
        if (args.Count == 3) {
            Operand thirdValue = args.get(2).ToNumber("Function PERCENTRANK parameter 3 is error!");
            if (thirdValue.IsError()) {
                return thirdValue;
            }
            d = thirdValue.IntValue();
        }
        return Operand.Create(round(v, d));
    }

    public Operand VisitAVERAGE_fun(AVERAGE_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new List<Double>();
        boolean o = F_base_GetList(args, list);
        if (o == false) {
            return Operand.Error("Function AVERAGE parameter error!");
        }

        return Operand.Create(list.Average());
    }

    public Operand VisitAVERAGEIF_fun(AVERAGEIF_funContext context)
    {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) { Operand aa = visit(item); if (aa.IsError()) { return aa; } args.add(aa); }

        List<Double>  list = new List<Double> ();
        boolean o = F_base_GetList(args.get(0), list);
        if (o == false) { return Operand.Error("Function AVERAGE parameter 1 error!"); }

        List<Double>  sumdbs;
        if (args.Count == 3) {
            sumdbs = new List<Double> ();
            boolean o2 = F_base_GetList(args.get(2), sumdbs);
            if (o2 == false) { return Operand.Error("Function AVERAGE parameter 3 error!"); }
        } else {
            sumdbs = list;
        }

        double sum;
        int count;
        if (args.get(1).Type == OperandType.NUMBER) {
            count = F_base_countif(list, args.get(1).NumberValue());
            sum = count * args.get(1).NumberValue();
        } else {
            try {
                Double d=Double.parseDouble(args.get(1).TextValue().trim());
                count = F_base_countif(list, d);
                sum = F_base_sumif(list, "=" + args.get(1).TextValue().trim(), sumdbs);
            } catch (Exception e) {
                String sunif = args.get(1).TextValue().trim();
                if (sumifRegex.IsMatch(sunif)) {
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
            //     if (sumifRegex.IsMatch(sunif)) {
            //         count = F_base_countif(list, sunif);
            //         sum = F_base_sumif(list, sunif, sumdbs);
            //     } else {
            //         return Operand.Error("Function AVERAGE parameter 2 error!");
            //     }
            // }
        }
        return Operand.Create(sum / count);
    }

    public Operand VisitGEOMEAN_fun(GEOMEAN_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        if (args.Count == 1)
            return args.get(0);

        List<Double> list = new List<Double>();
        boolean o = F_base_GetList(args, list);
        if (o == false) {
            return Operand.Error("Function GEOMEAN parameter error!");
        }

        double sum = 1;
        for (double db : list) {
            sum *= db;
        }
        return Operand.Create(Math.pow(sum, 1.0 / list.Count));
    }

    public Operand VisitHARMEAN_fun(HARMEAN_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        if (args.Count == 1)
            return args.get(0);

        List<Double> list = new List<Double>();
        boolean o = F_base_GetList(args, list);
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
        return Operand.Create(list.Count / sum);
    }

    public Operand VisitCOUNT_fun(COUNT_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new List<Double>();
        boolean o = F_base_GetList(args, list);
        if (o == false) {
            return Operand.Error("Function COUNT parameter error!");
        }

        return Operand.Create(list.Count);
    }

    public Operand VisitCOUNTIF_fun(COUNTIF_funContext context)
    {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) { Operand aa = visit(item); if (aa.IsError()) { return aa; } args.add(aa); }

        List<Double>  list = new List<Double> ();
        boolean o = F_base_GetList(args.get(0), list);
        if (o == false) { return Operand.Error("Function COUNTIF parameter error!"); }

        int count;
        if (args.get(1).Type == OperandType.NUMBER) {
            count = F_base_countif(list, args.get(1).NumberValue());
        } else {
            try {
                Double d=Double.parseDouble(args.get(1).TextValue().trim());
                count = F_base_countif(list, d);
            } catch (Exception e) {
                String sunif = args.get(1).TextValue().trim();
                if (sumifRegex.IsMatch(sunif)) {
                    count = F_base_countif(list, sunif);
                } else {
                    return Operand.Error("Function COUNTIF parameter 2 error!");
                }
            }
            // if (double.TryParse(args.get(1).TextValue().trim(), NumberStyles.Any, cultureInfo, out double d)) {
            //     count = F_base_countif(list, d);
            // } else {
            //     String sunif = args.get(1).TextValue().trim();
            //     if (sumifRegex.IsMatch(sunif)) {
            //         count = F_base_countif(list, sunif);
            //     } else {
            //         return Operand.Error("Function COUNTIF parameter 2 error!");
            //     }
            // }
        }
        return Operand.Create(count);
    }

    public Operand VisitSUM_fun(SUM_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        if (args.Count == 1)
            return args.get(0);

        List<Double> list = new List<Double>();
        boolean o = F_base_GetList(args, list);
        if (o == false) {
            return Operand.Error("Function SUM parameter error!");
        }

        double sum = list.Sum();
        return Operand.Create(sum);
    }

    public Operand VisitSUMIF_fun(SUMIF_funContext context)
    {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) { Operand aa = visit(item); if (aa.IsError()) { return aa; } args.add(aa); }

        List<Double>  list = new List<Double> ();
        boolean o = F_base_GetList(args.get(0), list);
        if (o == false) { return Operand.Error("Function SUMIF parameter 1 error!"); }

        List<Double>  sumdbs;
        if (args.Count == 3) {
            sumdbs = new List<Double> ();
            boolean o2 = F_base_GetList(args.get(2), sumdbs);
            if (o2 == false) { return Operand.Error("Function SUMIF parameter 3 error!"); }
        } else {
            sumdbs = list;
        }

        double sum;
        if (args.get(1).Type == OperandType.NUMBER) {
            sum = F_base_countif(list, args.get(1).NumberValue()) * args.get(1).NumberValue();
        } else {
            try {
                Double d=Double.parseDouble(args.get(1).TextValue().trim());
                sum = F_base_sumif(list, "=" + args.get(1).TextValue().trim(), sumdbs);
            } catch (Exception e) {
                String sunif = args.get(1).TextValue().trim();
                if (sumifRegex.IsMatch(sunif)) {
                    sum = F_base_sumif(list, sunif, sumdbs);
                } else {
                    return Operand.Error("Function SUMIF parameter 2 error!");
                }
            }
            // if (double.TryParse(args.get(1).TextValue().trim(), NumberStyles.Any, cultureInfo, out _)) {
            //     sum = F_base_sumif(list, "=" + args.get(1).TextValue().trim(), sumdbs);
            // } else {
            //     String sunif = args.get(1).TextValue().trim();
            //     if (sumifRegex.IsMatch(sunif)) {
            //         sum = F_base_sumif(list, sunif, sumdbs);
            //     } else {
            //         return Operand.Error("Function SUMIF parameter 2 error!");
            //     }
            // }
        }
        return Operand.Create(sum);
    }

    public Operand VisitAVEDEV_fun(AVEDEV_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new List<Double>();
        boolean o = F_base_GetList(args, list);
        if (o == false) {
            return Operand.Error("Function AVEDEV parameter error!");
        }

        double avg = list.Average();
        double sum = 0;
        for (int i = 0; i < list.Count; i++) {
            sum += Math.abs(list.get(i) - avg);
        }
        return Operand.Create(sum / list.Count);
    }

    public Operand VisitSTDEV_fun(STDEV_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        if (args.Count == 1) {
            return Operand.Error("Function STDEV parameter only one error!");
        }
        List<Double> list = new List<Double>();
        boolean o = F_base_GetList(args, list);
        if (o == false) {
            return Operand.Error("Function STDEV parameter error!");
        }

        double avg = list.Average();
        double sum = 0;
        for (int i = 0; i < list.Count; i++) {
            sum += (list.get(i) - avg) * (list.get(i) - avg);
        }
        return Operand.Create(Math.sqrt(sum / (list.Count - 1)));
    }

    public Operand VisitSTDEVP_fun(STDEVP_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new List<Double>();
        boolean o = F_base_GetList(args, list);
        if (o == false) {
            return Operand.Error("Function STDEVP parameter error!");
        }

        double sum = 0;
        double avg = list.Average();

        for (int i = 0; i < list.Count; i++) {
            sum += (list.get(i) - avg) * (list.get(i) - avg);
        }
        return Operand.Create(Math.sqrt(sum / (list.Count)));
    }

    public Operand VisitDEVSQ_fun(DEVSQ_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new List<Double>();
        boolean o = F_base_GetList(args, list);
        if (o == false) {
            return Operand.Error("Function DEVSQ parameter error!");
        }

        double avg = list.Average();
        double sum = 0;
        for (int i = 0; i < list.Count; i++) {
            sum += (list.get(i) - avg) * (list.get(i) - avg);
        }
        return Operand.Create(sum);
    }

    public Operand VisitVAR_fun(VAR_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        if (args.Count == 1) {
            return Operand.Error("Function VAR parameter only one error!");
        }
        List<Double> list = new List<Double>();
        boolean o = F_base_GetList(args, list);
        if (o == false) {
            return Operand.Error("Function VAR parameter error!");
        }

        double sum = 0;
        double sum2 = 0;
        for (int i = 0; i < list.Count; i++) {
            sum += list.get(i) * list.get(i);
            sum2 += list.get(i);
        }
        return Operand.Create((list.Count * sum - sum2 * sum2) / list.Count / (list.Count - 1));
    }

    public Operand VisitVARP_fun(VARP_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        List<Double> list = new List<Double>();
        boolean o = F_base_GetList(args, list);
        if (o == false) {
            return Operand.Error("Function VARP parameter error!");
        }

        double sum = 0;
        double avg = list.Average();
        for (int i = 0; i < list.Count; i++) {
            sum += (avg - list.get(i)) * (avg - list.get(i));
        }
        return Operand.Create(sum / list.Count);
    }

    public Operand VisitNORMDIST_fun(NORMDIST_funContext context) {
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

    public Operand VisitNORMINV_fun(NORMINV_funContext context) {
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

    public Operand VisitNORMSDIST_fun(NORMSDIST_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function NORMSDIST parameter 1 error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        double k = firstValue.NumberValue();
        return Operand.Create(ExcelFunctions.NormSDist(k));
    }

    public Operand VisitNORMSINV_fun(NORMSINV_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function NORMSINV parameter 1 error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        double k = firstValue.NumberValue();
        return Operand.Create(ExcelFunctions.NormSInv(k));
    }

    public Operand VisitBETADIST_fun(BETADIST_funContext context) {
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

    public Operand VisitBETAINV_fun(BETAINV_funContext context) {
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
        return Operand.Create(ExcelFunctions.BetaInv(probability, alpha, beta));
    }

    public Operand VisitBINOMDIST_fun(BINOMDIST_funContext context) {
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

    public Operand VisitEXPONDIST_fun(EXPONDIST_funContext context) {
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

    public Operand VisitFDIST_fun(FDIST_funContext context) {
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

    public Operand VisitFINV_fun(FINV_funContext context) {
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
        return Operand.Create(ExcelFunctions.FInv(probability, degreesFreedom, degreesFreedom2));
    }

    public Operand VisitFISHER_fun(FISHER_funContext context) {
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

    public Operand VisitFISHERINV_fun(FISHERINV_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function FISHERINV parameter error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        double x = firstValue.NumberValue();
        double n = (Math.exp(2 * x) - 1) / (Math.exp(2 * x) + 1);
        return Operand.Create(n);
    }

    public Operand VisitGAMMADIST_fun(GAMMADIST_funContext context) {
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

    public Operand VisitGAMMAINV_fun(GAMMAINV_funContext context) {
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

    public Operand VisitGAMMALN_fun(GAMMALN_funContext context) {
        Operand firstValue = visit(context.expr()).ToNumber("Function GAMMALN parameter error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(ExcelFunctions.GAMMALN(firstValue.NumberValue()));
    }

    public Operand VisitHYPGEOMDIST_fun(HYPGEOMDIST_funContext context) {
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

    public Operand VisitLOGINV_fun(LOGINV_funContext context) {
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

    public Operand VisitLOGNORMDIST_fun(LOGNORMDIST_funContext context) {
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

    public Operand VisitNEGBINOMDIST_fun(NEGBINOMDIST_funContext context) {
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

    public Operand VisitPOISSON_fun(POISSON_funContext context) {
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

    public Operand VisitTDIST_fun(TDIST_funContext context) {
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
        return Operand.Create(ExcelFunctions.TDist(x, degreesFreedom, tails));
    }

    public Operand VisitTINV_fun(TINV_funContext context) {
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
        return Operand.Create(ExcelFunctions.TInv(probability, degreesFreedom));
    }

    public Operand VisitWEIBULL_fun(WEIBULL_funContext context) {
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
        int state = fourthValue.BooleanValue();
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
        Matcher m = sumifRegex.Match(s);
        Double d = Double.parseDouble(m.Groups[2].Value);
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
        Matcher m = sumifRegex.Match(s);
        Double d = Double.parseDouble(m.Groups[2].Value);
        double sum = 0;

        for (int i = 0; i < dbs.Count; i++) {
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

    private boolean F_base_GetList(List<Operand> args, List<Double> list) {
        for (Operand item : args) {
            if (item.Type == OperandType.NUMBER) {
                list.add(item.NumberValue());
            } else if (item.Type == OperandType.ARRARY) {
                boolean o = F_base_GetList(item.ArrayValue(), list);
                if (o == false) {
                    return false;
                }
            } else if (item.Type == OperandType.JSON) {
                Operand i = item.ToArray(null);
                if (i.IsError()) {
                    return false;
                }
                boolean o = F_base_GetList(i.ArrayValue(), list);
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

    private boolean F_base_GetList(Operand args, List<Double> list) {
        if (args.IsError()) {
            return false;
        }
        if (args.Type == OperandType.NUMBER) {
            list.add(args.NumberValue());
        } else if (args.Type == OperandType.ARRARY) {
            boolean o = F_base_GetList(args.ArrayValue(), list);
            if (o == false) {
                return false;
            }
        } else if (args.Type == OperandType.JSON) {
            Operand i = args.ToArray(null);
            if (i.IsError()) {
                return false;
            }
            boolean o = F_base_GetList(i.ArrayValue(), list);
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
        if (args.Type == OperandType.ARRARY) {
            boolean o = F_base_GetList(args.ArrayValue(), list);
            if (o == false) {
                return false;
            }
        } else if (args.Type == OperandType.JSON) {
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
            if (item.Type == OperandType.ARRARY) {
                boolean o = F_base_GetList(item.ArrayValue(), list);
                if (o == false) {
                    return false;
                }
            } else if (item.Type == OperandType.JSON) {
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

    public Operand VisitURLENCODE_fun(URLENCODE_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function URLENCODE parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(HttpUtility.UrlEncode(firstValue.TextValue()));
    }

    public Operand VisitURLDECODE_fun(URLDECODE_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function URLDECODE parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(HttpUtility.UrlDecode(firstValue.TextValue()));
    }

    public Operand VisitHTMLENCODE_fun(HTMLENCODE_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function HTMLENCODE parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(HttpUtility.HtmlEncode(firstValue.TextValue()));
    }

    public Operand VisitHTMLDECODE_fun(HTMLDECODE_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function HTMLDECODE parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(HttpUtility.HtmlDecode(firstValue.TextValue()));
    }

    public Operand VisitBASE64TOTEXT_fun(BASE64TOTEXT_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function BASE64TOTEXT parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        Encoding encoding;
        if (args.Count == 1) {
            encoding = Encoding.UTF8;
        } else {
            encoding = Encoding.GetEncoding(args.get(1).TextValue());
        }
        String t = encoding.GetString(Base64.FromBase64String(args.get(0).TextValue()));
        return Operand.Create(t);
    }

    public Operand VisitBASE64URLTOTEXT_fun(BASE64URLTOTEXT_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function BASE64URLTOTEXT parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        Encoding encoding;
        if (args.Count == 1) {
            encoding = Encoding.UTF8;
        } else {
            encoding = Encoding.GetEncoding(args.get(1).TextValue());
        }
        String t = encoding.GetString(Base64.FromBase64ForUrlString(args.get(0).TextValue()));
        return Operand.Create(t);
    }

    public Operand VisitTEXTTOBASE64_fun(TEXTTOBASE64_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function TEXTTOBASE64 parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        Encoding encoding;
        if (args.Count == 1) {
            encoding = Encoding.UTF8;
        } else {
            encoding = Encoding.GetEncoding(args.get(1).TextValue());
        }
        byte[] bytes = encoding.GetBytes(args.get(0).TextValue());
        String t = Base64.ToBase64String(bytes);
        return Operand.Create(t);
    }

    public Operand VisitTEXTTOBASE64URL_fun(TEXTTOBASE64URL_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function TEXTTOBASE64URL parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        Encoding encoding;
        if (args.Count == 1) {
            encoding = Encoding.UTF8;
        } else {
            encoding = Encoding.GetEncoding(args.get(1).TextValue());
        }
        byte[] bytes = encoding.GetBytes(args.get(0).TextValue());
        String t = Base64.ToBase64ForUrlString(bytes);
        return Operand.Create(t);
    }

    public Operand VisitREGEX_fun(REGEX_funContext context) {
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

        if (args.Count == 2) {
            Match b = Regex.Match(firstValue.TextValue(), secondValue.TextValue());
            if (b.Success == false) {
                return Operand.Error("Function REGEX is error!");
            }
            return Operand.Create(b.Value);
        } else if (args.Count == 3) {
            Matches ms = Regex.Matches(firstValue.TextValue(), secondValue.TextValue());
            Operand thirdValue = args.get(2).ToNumber("Function REGEX parameter 3 is error!");
            if (thirdValue.IsError()) {
                return thirdValue;
            }
            if (ms.Count <= thirdValue.IntValue() - excelIndex) {
                return Operand.Error("Function REGEX is error!");
            }
            return Operand.Create(ms[thirdValue.IntValue() - excelIndex].Value);
        } else {
            Matches ms = Regex.Matches(firstValue.TextValue(), secondValue.TextValue());
            Operand thirdValue = args.get(2).ToNumber("Function REGEX parameter 3 is error!");
            if (thirdValue.IsError()) {
                return thirdValue;
            }
            if (ms.Count <= thirdValue.IntValue() + excelIndex) {
                return Operand.Error("Function REGEX parameter 3 is error!");
            }
            Operand fourthValue = args.get(3).ToNumber("Function REGEX parameter 4 is error!");
            if (fourthValue.IsError()) {
                return fourthValue;
            }
            return Operand.Create(ms[thirdValue.IntValue() - excelIndex].Groups[fourthValue.IntValue()].Value);
        }
    }

    public Operand VisitREGEXREPALCE_fun(REGEXREPALCE_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function REGEXREPALCE parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        String b = Regex.Replace(args.get(0).TextValue(), args.get(1).TextValue(), args.get(2).TextValue());
        return Operand.Create(b);
    }

    public Operand VisitISREGEX_fun(ISREGEX_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function ISREGEX parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        boolean b = Regex.IsMatch(args.get(0).TextValue(), args.get(1).TextValue());
        return Operand.Create(b);
    }

    public Operand VisitGUID_fun(GUID_funContext context) {
        return Operand.Create(System.Guid.NewGuid().toString());
    }

    public Operand VisitMD5_fun(MD5_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function MD5 parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        Encoding encoding;
        if (args.Count == 1) {
            encoding = Encoding.UTF8;
        } else {
            encoding = Encoding.GetEncoding(args.get(1).TextValue());
        }
        String t = Hash.GetMd5String(encoding.GetBytes(args.get(0).TextValue()));
        return Operand.Create(t);
    }

    public Operand VisitSHA1_fun(SHA1_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function SHA1 parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        Encoding encoding;
        if (args.Count == 1) {
            encoding = Encoding.UTF8;
        } else {
            encoding = Encoding.GetEncoding(args.get(1).TextValue());
        }
        String t = Hash.GetSha1String(encoding.GetBytes(args.get(0).TextValue()));
        return Operand.Create(t);
    }

    public Operand VisitSHA256_fun(SHA256_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function SHA256 parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        Encoding encoding;
        if (args.Count == 1) {
            encoding = Encoding.UTF8;
        } else {
            encoding = Encoding.GetEncoding(args.get(1).TextValue());
        }
        String t = Hash.GetSha256String(encoding.GetBytes(args.get(0).TextValue()));
        return Operand.Create(t);
    }

    public Operand VisitSHA512_fun(SHA512_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function SHA512 parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        Encoding encoding;
        if (args.Count == 1) {
            encoding = Encoding.UTF8;
        } else {
            encoding = Encoding.GetEncoding(args.get(1).TextValue());
        }
        String t = Hash.GetSha512String(encoding.GetBytes(args.get(0).TextValue()));
        return Operand.Create(t);
    }

    public Operand VisitCRC8_fun(CRC8_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function CRC8 parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        Encoding encoding;
        if (args.Count == 1) {
            encoding = Encoding.UTF8;
        } else {
            encoding = Encoding.GetEncoding(args.get(1).TextValue());
        }
        String t = Hash.GetCrc8String(encoding.GetBytes(args.get(0).TextValue()));
        return Operand.Create(t);
    }

    public Operand VisitCRC16_fun(CRC16_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function CRC16 parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        Encoding encoding;
        if (args.Count == 1) {
            encoding = Encoding.UTF8;
        } else {
            encoding = Encoding.GetEncoding(args.get(1).TextValue());
        }
        String t = Hash.GetCrc16String(encoding.GetBytes(args.get(0).TextValue()));
        return Operand.Create(t);
    }

    public Operand VisitCRC32_fun(CRC32_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function CRC32 parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        Encoding encoding;
        if (args.Count == 1) {
            encoding = Encoding.UTF8;
        } else {
            encoding = Encoding.GetEncoding(args.get(1).TextValue());
        }
        String t = Hash.GetCrc32String(encoding.GetBytes(args.get(0).TextValue()));
        return Operand.Create(t);
    }

    public Operand VisitHMACMD5_fun(HMACMD5_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function HMACMD5 parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        Encoding encoding;
        if (args.Count == 2) {
            encoding = Encoding.UTF8;
        } else {
            encoding = Encoding.GetEncoding(args.get(2).TextValue());
        }
        String t = Hash.GetHmacMd5String(encoding.GetBytes(args.get(0).TextValue()), args.get(1).TextValue());
        return Operand.Create(t);
    }

    public Operand VisitHMACSHA1_fun(HMACSHA1_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function HMACSHA1 parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        Encoding encoding;
        if (args.Count == 2) {
            encoding = Encoding.UTF8;
        } else {
            encoding = Encoding.GetEncoding(args.get(2).TextValue());
        }
        String t = Hash.GetHmacSha1String(encoding.GetBytes(args.get(0).TextValue()), args.get(1).TextValue());
        return Operand.Create(t);
    }

    public Operand VisitHMACSHA256_fun(HMACSHA256_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function HMACSHA256 parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        Encoding encoding;
        if (args.Count == 2) {
            encoding = Encoding.UTF8;
        } else {
            encoding = Encoding.GetEncoding(args.get(2).TextValue());
        }
        String t = Hash.GetHmacSha256String(encoding.GetBytes(args.get(0).TextValue()), args.get(1).TextValue());
        return Operand.Create(t);
    }

    public Operand VisitHMACSHA512_fun(HMACSHA512_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function HMACSHA512 parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }

        Encoding encoding;
        if (args.Count == 2) {
            encoding = Encoding.UTF8;
        } else {
            encoding = Encoding.GetEncoding(args.get(2).TextValue());
        }
        String t = Hash.GetHmacSha512String(encoding.GetBytes(args.get(0).TextValue()), args.get(1).TextValue());
        return Operand.Create(t);
    }

    public Operand VisitTRIMSTART_fun(TRIMSTART_funContext context) {
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
        if (args.Count == 2) {
            return Operand.Create(text.TrimStart(args.get(1).TextValue().ToArray()));
        }
        return Operand.Create(text.TrimStart());
    }

    public Operand VisitTRIMEND_fun(TRIMEND_funContext context) {
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
        if (args.Count == 2) {
            return Operand.Create(text.TrimEnd(args.get(1).TextValue().ToArray()));
        }
        return Operand.Create(text.TrimEnd());
    }

    public Operand VisitINDEXOF_fun(INDEXOF_funContext context) {
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
        if (args.Count == 2) {
            return Operand.Create(text.IndexOf(secondValue.TextValue()) + excelIndex);
        }
        Operand thirdValue = args.get(2).ToText("Function INDEXOF parameter 3 is error!");
        if (thirdValue.IsError()) {
            return thirdValue;
        }
        if (args.Count == 3) {
            return Operand.Create(text.IndexOf(secondValue.TextValue(), thirdValue.IntValue()) + excelIndex);
        }
        Operand fourthValue = args.get(3).ToText("Function INDEXOF parameter 4 is error!");
        if (fourthValue.IsError()) {
            return fourthValue;
        }
        return Operand.Create(
                text.IndexOf(secondValue.TextValue(), thirdValue.IntValue(), fourthValue.IntValue()) + excelIndex);
    }

    public Operand VisitLASTINDEXOF_fun(LASTINDEXOF_funContext context) {
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
        if (args.Count == 2) {
            return Operand.Create(text.LastIndexOf(secondValue.TextValue()) + excelIndex);
        }
        Operand thirdValue = args.get(2).ToText("Function LASTINDEXOF parameter 3 is error!");
        if (thirdValue.IsError()) {
            return thirdValue;
        }
        if (args.Count == 3) {
            return Operand.Create(text.LastIndexOf(secondValue.TextValue(), thirdValue.IntValue()) + excelIndex);
        }
        Operand fourthValue = args.get(3).ToText("Function LASTINDEXOF parameter 4 is error!");
        if (fourthValue.IsError()) {
            return fourthValue;
        }
        return Operand.Create(
                text.LastIndexOf(secondValue.TextValue(), thirdValue.IntValue(), fourthValue.IntValue()) + excelIndex);
    }

    public Operand VisitSPLIT_fun(SPLIT_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        int index = 1;
        for (ExprContext item : context.expr()) {
            Operand a = visit(item).ToText("Function SPLIT parameter " + (index++) + " is error!");
            if (a.IsError()) {
                return a;
            }
            args.add(a);
        }
        return Operand.Create(args.get(0).TextValue().Split(args.get(1).TextValue().ToArray()));
    }

    public Operand VisitJOIN_fun(JOIN_funContext context) {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) {
            Operand aa = visit(item);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }

        Operand firstValue = args.get(0);
        if (firstValue.Type == OperandType.JSON) {
            Operand o = firstValue.ToArray(null);
            if (o.IsError() == false) {
                firstValue = o;
            }
        }
        if (firstValue.Type == OperandType.ARRARY) {
            List<String> list = new List<String>();
            boolean o = F_base_GetList(firstValue, list);
            if (o == false)
                return Operand.Error("Function JOIN parameter 1 is error!");

            Operand secondValue = args.get(1).ToText("Function JOIN parameter 2 is error!");
            if (secondValue.IsError()) {
                return secondValue;
            }

            return Operand.Create(String.Join(secondValue.TextValue(), list));
        } else {
            firstValue = firstValue.ToText("Function JOIN parameter 1 is error!");
            if (firstValue.IsError()) {
                return firstValue;
            }

            List<String> list = new List<String>();
            for (int i = 1; i < args.Count; i++) {
                boolean o = F_base_GetList(args.get(i), list);
                if (o == false)
                    return Operand.Error("Function JOIN parameter {i + 1} is error!");
            }

            return Operand.Create(String.Join(firstValue.TextValue(), list));
        }
    }

    public Operand VisitSUBSTRING_fun(SUBSTRING_funContext context) {
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
        if (args.Count == 2) {
            return Operand.Create(text.Substring(secondValue.IntValue() - excelIndex));
        }
        Operand thirdValue = args.get(2).ToNumber("Function SUBSTRING parameter 3 is error!");
        if (thirdValue.IsError()) {
            return thirdValue;
        }
        return Operand.Create(text.Substring(secondValue.IntValue() - excelIndex, thirdValue.IntValue()));
    }

    public Operand VisitSTARTSWITH_fun(STARTSWITH_funContext context) {
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
        if (args.Count == 2) {
            return Operand.Create(text.StartsWith(secondValue.TextValue()));
        }
        Operand thirdValue = args.get(2).ToBoolean("Function STARTSWITH parameter 3 is error!");
        if (thirdValue.IsError()) {
            return thirdValue;
        }
        return Operand.Create(text.StartsWith(secondValue.TextValue(),
                thirdValue.BooleanValue() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
    }

    public Operand VisitENDSWITH_fun(ENDSWITH_funContext context) {
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
        if (args.Count == 2) {
            return Operand.Create(text.EndsWith(secondValue.TextValue()));
        }
        Operand thirdValue = args.get(2).ToBoolean("Function ENDSWITH parameter 3 is error!");
        ;
        if (thirdValue.IsError()) {
            return thirdValue;
        }
        return Operand.Create(text.EndsWith(secondValue.TextValue(),
                thirdValue.BooleanValue() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
    }

    public Operand VisitISNULLOREMPTY_fun(ISNULLOREMPTY_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function ISNULLOREMPTY parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(String.IsNullOrEmpty(firstValue.TextValue()));
    }

    public Operand VisitISNULLORWHITESPACE_fun(ISNULLORWHITESPACE_funContext context) {
        Operand firstValue = visit(context.expr()).ToText("Function ISNULLORWHITESPACE parameter 1 is error!");
        if (firstValue.IsError()) {
            return firstValue;
        }

        return Operand.Create(String.IsNullOrWhiteSpace(firstValue.TextValue()));
    }

    public Operand VisitREMOVESTART_fun(REMOVESTART_funContext context) {
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

        StringComparison comparison = StringComparison.Ordinal;
        if (args.Count == 3) {
            Operand thirdValue = args.get(2).ToBoolean("Function REMOVESTART parameter 3 is error!");
            if (thirdValue.IsError()) {
                return thirdValue;
            }
            if (thirdValue.BooleanValue()) {
                comparison = StringComparison.OrdinalIgnoreCase;
            }
        }
        String text = firstValue.TextValue();
        if (text.StartsWith(secondValue.TextValue(), comparison)) {
            return Operand.Create(text.Substring(secondValue.TextValue().Length));
        }
        return firstValue;
    }

    public Operand VisitREMOVEEND_fun(REMOVEEND_funContext context) {
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

        StringComparison comparison = StringComparison.Ordinal;
        if (args.Count == 3) {
            Operand thirdValue = args.get(2).ToBoolean("Function REMOVESTART parameter 3 is error!");
            if (thirdValue.IsError()) {
                return thirdValue;
            }
            if (thirdValue.BooleanValue()) {
                comparison = StringComparison.OrdinalIgnoreCase;
            }
        }
        String text = firstValue.TextValue();
        if (text.EndsWith(secondValue.TextValue(), comparison)) {
            return Operand.Create(text.Substring(0, text.Length - secondValue.TextValue().Length));
        }
        return firstValue;
    }

    public Operand VisitJSON_fun(JSON_funContext context)
    {
        Operand firstValue = visit(context.expr()).ToText("Function JSON parameter is error!");
        if (firstValue.IsError()) { return firstValue; }
        String txt = firstValue.TextValue();
        if ((txt.StartsWith("{") && txt.EndsWith("}")) || (txt.StartsWith("[") && txt.EndsWith("]"))) {
            try {
                JsonData json = JsonMapper.ToObject(txt);
                return Operand.Create(json);
            } catch (Exception e) { }
        }
        return Operand.Error("Function JSON parameter is error!");
    }

    public Operand VisitVLOOKUP_fun(VLOOKUP_funContext context) {
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
        if (args.Count == 4) {
            Operand fourthValue = args.get(2).ToBoolean("Function VLOOKUP parameter 4 is error!");
            if (fourthValue.IsError()) {
                return fourthValue;
            }
            vague = fourthValue.BooleanValue();
        }
        if (secondValue.Type != OperandType.NULL) {
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
            if (o.ArrayValue().Count > 0) {
                Operand o1 = o.ArrayValue().get(0);// [0];
                int b = -1;
                if (secondValue.Type == OperandType.NUMBER) {
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
                    if (index < o.ArrayValue().Count) {
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
                if (o.ArrayValue().Count > 0) {
                    Operand o1 = o.ArrayValue().get(0);// [0];
                    int b = -1;
                    if (secondValue.Type == OperandType.NUMBER) {
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
                    if (b < 0 && index < o.ArrayValue().Count) {
                        last = o;
                    } else if (b > 0 && last != null) {
                        return last.ArrayValue().get(index);// [index];
                    }
                }
            }
        }
        return Operand.Error("Function VLOOKUP is not match !");
    }

    public Operand VisitLOOKUP_fun(LOOKUP_funContext context)
    {
        List<Operand> args = new ArrayList<Operand>();
        for (ExprContext item : context.expr()) { Operand aa = visit(item); if (aa.IsError()) { return aa; } args.add(aa); }

        Operand firstValue = args.get(0).ToArray("Function LOOKUP parameter 1 error!");
        if (firstValue.IsError()) { return firstValue; }
        Operand secondValue = args.get(1).ToText("Function LOOKUP parameter 2 is error!");
        if (secondValue.IsError()) { return secondValue; }
        Operand thirdValue = args.get(2).ToText("Function LOOKUP parameter 3 is error!");
        if (thirdValue.IsError()) { return thirdValue; }

        if (String.IsNullOrWhiteSpace(secondValue.TextValue())) {
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

    public Operand VisitArray_fun(Array_funContext context) {
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

    public Operand VisitBracket_fun(Bracket_funContext context) {
        return visit(context.expr());
    }

    public Operand VisitNUM_fun(NUM_funContext context)
    {
        String sub = context.SUB().getText();
        // String sub = context.SUB()?.GetText() ?? "";
        String t = context.NUM().getText();

        Double d=Double.parseDouble(sub + t);
        // double d = double.Parse(sub + t, NumberStyles.Any, cultureInfo);
        return Operand.Create(d);
    }

    public Operand VisitSTRING_fun(STRING_funContext context) {
        String opd = context.STRING().GetText();
        StringBuilder sb = new StringBuilder();
        int index = 1;
        while (index < opd.Length - 1) {
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

    public Operand VisitNULL_fun(NULL_funContext context) {
        return Operand.CreateNull();
    }

    public Operand VisitPARAMETER_fun(PARAMETER_funContext context) {
        Operand p = visit(context.parameter()).ToText("Function PARAMETER first parameter is error!");
        if (p.IsError())
            return p;

        if (GetParameter != null) {
            return GetParameter.apply(p.TextValue());
        }
        return Operand.Error("Function PARAMETER first parameter is error!");
    }

    public Operand VisitParameter(ParameterContext context) {
        Operand expr = context.expr();
        if (expr != null) {
            return visit(expr);
        }
        return visit(context.parameter2());
    }

    public Operand VisitParameter2(Parameter2Context context) {
        return Operand.Create(context.children[0].GetText());
    }

    public Operand VisitGetJsonValue_fun(GetJsonValue_funContext context) {
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

        if (obj.Type == OperandType.ARRARY) {
            op = op.ToNumber("ARRARY index is error!");
            if (op.IsError()) {
                return op;
            }
            int index = op.IntValue() - excelIndex;
            if (index < obj.ArrayValue().Count)
                return obj.ArrayValue().get(index);// [index];
            return Operand.Error("ARRARY index " + index + " greater than maximum length!");
        }
        if (obj.Type == OperandType.JSON) {
            JsonData json = obj.JsonValue();
            if (json.IsArray) {
                op = op.ToNumber("JSON parameter index is error!");
                if (op.IsError()) {
                    return op;
                }
                int index = op.IntValue() - excelIndex;
                if (index < json.Count) {
                    JsonData v = json.GetChild(index);// [op.IntValue() - excelIndex];
                    if (v.IsString())
                        return Operand.Create(v.StringValue);
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
                        return Operand.Create(v.StringValue);
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

    public Operand VisitExpr2_fun(Expr2_funContext context) {
        return VisitChildren(context);
    }

    public Operand VisitDiyFunction_fun(DiyFunction_funContext context) {
        if (DiyFunction != null) {
            String funName = context.PARAMETER().GetText();
            List<Operand> args = new ArrayList<Operand>();
            for (ExprContext item : context.expr()) {
                Operand aa = visit(item);
                args.add(aa);
            }
            return DiyFunction(funName, args);
        }
        return Operand.Error("DiyFunction is error!");
    }

    private double round(double value, int p) {
        BigDecimal bigD = new BigDecimal(tpD);
        return bigD.setScale(2, BigDecimal.ROUND_HALF_UP).doubleValue();
    }
}