package toolgood.algorithm;

import java.util.regex.Pattern;

import org.antlr.v4.runtime.CharStreams;
import org.antlr.v4.runtime.CommonTokenStream;
import org.antlr.v4.runtime.Token;

import java.math.BigDecimal;
import toolgood.algorithm.enums.CalculateTreeType;
import toolgood.algorithm.enums.ConditionTreeType;
import toolgood.algorithm.internals.CalculateTree;
import toolgood.algorithm.internals.ConditionTree;
import toolgood.algorithm.internals.DiyNameInfo;
import toolgood.algorithm.internals.DiyNameKeyInfo;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.visitors.AntlrCharStream;
import toolgood.algorithm.internals.visitors.AntlrErrorData;
import toolgood.algorithm.internals.visitors.AntlrErrorListener;
import toolgood.algorithm.internals.visitors.DiyNameVisitor;
import toolgood.algorithm.internals.visitors.MathFunctionVisitor;
import toolgood.algorithm.internals.visitors.MathSplitVisitor;
import toolgood.algorithm.internals.visitors.MathSplitVisitor2;
import toolgood.algorithm.math.mathLexer;
import toolgood.algorithm.math.mathParser;
import toolgood.algorithm.unitConversion.AreaConverter;
import toolgood.algorithm.unitConversion.DistanceConverter;
import toolgood.algorithm.unitConversion.MassConverter;
import toolgood.algorithm.unitConversion.VolumeConverter;

public class AlgorithmEngineHelper {
    private static final Pattern unitRegex = Pattern.compile("[\\s\\(\\)（）\\[\\]<>]");

    static ParserContext CreateParserContext(String exp) {
        AntlrErrorData data = new AntlrErrorData();
        AntlrCharStream stream = new AntlrCharStream(CharStreams.fromString(exp));
        mathLexer lexer = new mathLexer(stream);
        CommonTokenStream tokens = new CommonTokenStream(lexer);
        mathParser parser = new mathParser(tokens);

        AntlrErrorListener<Integer> listener = new AntlrErrorListener<>(data);
        AntlrErrorListener<Token> listener2 = new AntlrErrorListener<>(data);
        lexer.removeErrorListeners();
        lexer.addErrorListener(listener);
        parser.removeErrorListeners();
        parser.addErrorListener(listener2);

        mathParser.ProgContext context = parser.prog();
        return new ParserContext(data, context);
    }

    public static boolean IsParameter(String parameter) {
        if (parameter == null || parameter.trim().isEmpty()) {
            return false;
        }
        try {
            DiyNameInfo diy = GetDiyNames(parameter);
            if (diy.getFunctions().size() > 0) {
                return false;
            }
            if (diy.getParameters().size() == 1) {
                DiyNameKeyInfo p = diy.getParameters().get(0);
                return p.getName().equals(parameter);
            }
        } catch (Exception e) {
        }
        return false;
    }

    public static DiyNameInfo GetDiyNames(String exp) throws Exception {
        if (exp == null || exp.trim().isEmpty()) {
            throw new Exception("Parameter exp invalid !");
        }
        ParserContext context = CreateParserContext(exp);
        if (context.data.isError()) {
            throw new Exception(context.data.getErrorMsg());
        }
        DiyNameVisitor visitor = new DiyNameVisitor();
        visitor.visit(context.context);
        return visitor.diy;
    }

    public static double UnitConversion(double src, String oldSrcUnit, String oldTarUnit, String name) throws Exception {
        if (oldSrcUnit == null || oldSrcUnit.trim().isEmpty() || oldTarUnit == null || oldTarUnit.trim().isEmpty()) {
            return src;
        }
        if (oldSrcUnit.equals(oldTarUnit)) {
            return src;
        }

        Double result = TryConvert(src, oldSrcUnit, oldTarUnit);
        if (result != null) {
            return result;
        }

        oldSrcUnit = unitRegex.matcher(oldSrcUnit).replaceAll("");
        result = TryConvert(src, oldSrcUnit, oldTarUnit);
        if (result != null) {
            return result;
        }

        if (name == null || name.isEmpty()) {
            throw new Exception(String.format("The input item has different units and cannot be converted from [%s] to [%s]", oldSrcUnit, oldTarUnit));
        }
        throw new Exception(String.format("The input item [%s] has different units and cannot be converted from [%s] to [%s]", name, oldSrcUnit, oldTarUnit));
    }

    private static Double TryConvert(double src, String srcUnit, String tarUnit) {
        try {
            if (DistanceConverter.Exists(srcUnit, tarUnit)) {
                return new DistanceConverter(srcUnit, tarUnit).LeftToRight(new BigDecimal(src)).doubleValue();
            }
            if (MassConverter.Exists(srcUnit, tarUnit)) {
                return new MassConverter(srcUnit, tarUnit).LeftToRight(new BigDecimal(src)).doubleValue();
            }
            if (AreaConverter.Exists(srcUnit, tarUnit)) {
                return new AreaConverter(srcUnit, tarUnit).LeftToRight(new BigDecimal(src)).doubleValue();
            }
            if (VolumeConverter.Exists(srcUnit, tarUnit)) {
                return new VolumeConverter(srcUnit, tarUnit).LeftToRight(new BigDecimal(src)).doubleValue();
            }
        } catch (Exception e) {
        }
        return null;
    }

    public static FunctionBase ParseFormula(String exp) throws Exception {
        if (exp == null || exp.trim().isEmpty()) {
            throw new Exception("Parameter exp invalid !");
        }
        ParserContext context = CreateParserContext(exp);
        if (context.data.isError()) {
            throw new Exception(context.data.getErrorMsg());
        }
        MathFunctionVisitor visitor = new MathFunctionVisitor();
        return visitor.visitProg(context.context);
    }

    public static boolean CheckFormula(String exp) {
        if (exp == null || exp.trim().isEmpty()) {
            return false;
        }
        ParserContext context = CreateParserContext(exp);
        return !context.data.isError();
    }

    public static ConditionTree ParseCondition(String condition) {
        ConditionTree tree = new ConditionTree();
        if (condition == null || condition.trim().isEmpty()) {
            tree.Type = ConditionTreeType.Error;
            tree.ErrorMessage = "condition is null";
            return tree;
        }
        try {
            ParserContext context = CreateParserContext(condition);
            if (context.data.isError()) {
                tree.Type = ConditionTreeType.Error;
                tree.ErrorMessage = context.data.getErrorMsg();
                return tree;
            }
            MathSplitVisitor visitor = new MathSplitVisitor();
            return visitor.visitProg(context.context);
        } catch (Exception ex) {
            tree.Type = ConditionTreeType.Error;
            tree.ErrorMessage = ex.getMessage();
        }
        return tree;
    }

    public static FunctionBase Condition_And(FunctionBase left, FunctionBase right) {
        return new toolgood.algorithm.internals.functions.operator.Function_AND(left, right);
    }

    public static FunctionBase Condition_Or(FunctionBase left, FunctionBase right) {
        return new toolgood.algorithm.internals.functions.operator.Function_OR(left, right);
    }

    public static CalculateTree ParseCalculate(String exp) {
        CalculateTree tree = new CalculateTree();
        if (exp == null || exp.trim().isEmpty()) {
            tree.Type = CalculateTreeType.Error;
            tree.ErrorMessage = "exp is null";
            return tree;
        }
        try {
            ParserContext context = CreateParserContext(exp);
            if (context.data.isError()) {
                tree.Type = CalculateTreeType.Error;
                tree.ErrorMessage = context.data.getErrorMsg();
                return tree;
            }
            MathSplitVisitor2 visitor = new MathSplitVisitor2();
            return visitor.visitProg(context.context);
        } catch (Exception ex) {
            tree.Type = CalculateTreeType.Error;
            tree.ErrorMessage = ex.getMessage();
        }
        return tree;
    }

    public static FunctionBase Calculate_Add(FunctionBase left, FunctionBase right) {
        return new toolgood.algorithm.internals.functions.operator.Function_Add(left, right);
    }

    public static FunctionBase Calculate_Subtract(FunctionBase left, FunctionBase right) {
        return new toolgood.algorithm.internals.functions.operator.Function_Sub(left, right);
    }

    public static FunctionBase Calculate_Multiply(FunctionBase left, FunctionBase right) {
        return new toolgood.algorithm.internals.functions.operator.Function_Mul(left, right);
    }

    public static FunctionBase Calculate_Divide(FunctionBase left, FunctionBase right) {
        return new toolgood.algorithm.internals.functions.operator.Function_Div(left, right);
    }

    public static FunctionBase Calculate_Mod(FunctionBase left, FunctionBase right) {
        return new toolgood.algorithm.internals.functions.operator.Function_Mod(left, right);
    }

    public static FunctionBase Calculate_Connect(FunctionBase left, FunctionBase right) {
        return new toolgood.algorithm.internals.functions.operator.Function_Connect(left, right);
    }

    static class ParserContext {
        public AntlrErrorData data;
        public mathParser.ProgContext context;

        public ParserContext(AntlrErrorData data, mathParser.ProgContext context) {
            this.data = data;
            this.context = context;
        }
    }
}
