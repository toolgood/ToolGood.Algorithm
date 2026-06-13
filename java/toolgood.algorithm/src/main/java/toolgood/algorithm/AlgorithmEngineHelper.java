package toolgood.algorithm;

import org.antlr.v4.runtime.CharStreams;
import org.antlr.v4.runtime.CommonTokenStream;

import toolgood.algorithm.enums.CombineCalculateType;
import toolgood.algorithm.enums.ConditionTreeType;
import toolgood.algorithm.enums.CalculateTreeType;
import toolgood.algorithm.internals.CalculateTree;
import toolgood.algorithm.internals.ConditionTree;
import toolgood.algorithm.internals.DiyNameInfo;
import toolgood.algorithm.internals.DiyNameKeyInfo;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.compare.*;
import toolgood.algorithm.internals.functions.flow.*;
import toolgood.algorithm.internals.functions.operator.*;
import toolgood.algorithm.internals.functions.value.Function_ERROR;
import toolgood.algorithm.internals.visitors.AntlrCharStream;
import toolgood.algorithm.internals.visitors.AntlrErrorListener;
import toolgood.algorithm.internals.visitors.DiyNameVisitor;
import toolgood.algorithm.internals.visitors.MathFunctionVisitor;
import toolgood.algorithm.internals.visitors.MathSplitVisitor;
import toolgood.algorithm.internals.visitors.MathSplitVisitor2;
import toolgood.algorithm.math.mathLexer;
import toolgood.algorithm.math.mathParser;
import toolgood.algorithm.math.mathParser.ProgContext;
import toolgood.algorithm.unitConversion.AreaConverter;
import toolgood.algorithm.unitConversion.DistanceConverter;
import toolgood.algorithm.unitConversion.MassConverter;
import toolgood.algorithm.unitConversion.VolumeConverter;

import java.math.BigDecimal;

public class AlgorithmEngineHelper {

    private static mathParser.ProgContext CreateParserContext(String exp, AntlrErrorListener errorListener) {
        AntlrCharStream stream = new AntlrCharStream(CharStreams.fromString(exp));
        mathLexer lexer = new mathLexer(stream);
        CommonTokenStream tokens = new CommonTokenStream(lexer);
        mathParser parser = new mathParser(tokens);
        lexer.removeErrorListeners();
        lexer.addErrorListener(errorListener);
        parser.removeErrorListeners();
        parser.addErrorListener(errorListener);
        return parser.prog();
    }

    public static boolean IsParameter(String parameter) {
        if (parameter == null || parameter.trim().isEmpty()) {
            return false;
        }
        try {
            DiyNameInfo diy = GetDiyNames(parameter);
            if (diy.Functions.size() > 0) {
                return false;
            }
            if (diy.Parameters.size() == 1) {
                DiyNameKeyInfo p = diy.Parameters.get(0);
                return p.Name.equals(parameter);
            }
        } catch (Exception e) {
            return false;
        }
        return false;
    }

    public static DiyNameInfo GetDiyNames(String exp) throws Exception {
        if (exp == null || exp.trim().isEmpty()) {
            throw new IllegalArgumentException("Parameter exp invalid !");
        }
        AntlrErrorListener errorListener = new AntlrErrorListener();
        ProgContext context = CreateParserContext(exp, errorListener);
        if (errorListener.IsError) {
            throw new IllegalArgumentException(errorListener.ErrorMsg);
        }
        DiyNameVisitor visitor = new DiyNameVisitor();
        visitor.visit(context);
        return new DiyNameInfo(visitor.Parameters, visitor.Functions);
    }

    public static BigDecimal UnitConversion(BigDecimal src, String oldSrcUnit, String oldTarUnit) throws Exception {
        return UnitConversion(src, oldSrcUnit, oldTarUnit, null);
    }

    public static BigDecimal UnitConversion(BigDecimal src, String oldSrcUnit, String oldTarUnit, String name) throws Exception {
        if (oldSrcUnit == null || oldSrcUnit.trim().isEmpty() || oldTarUnit == null || oldTarUnit.trim().isEmpty()) {
            return src;
        }
        if (oldSrcUnit.equals(oldTarUnit)) {
            return src;
        }

        BigDecimal result = TryConvert(src, oldSrcUnit, oldTarUnit);
        if (result != null) {
            return result;
        }

        String cleanSrcUnit = oldSrcUnit.replaceAll("[\\s\\(\\)（）\\[\\]<>]", "");
        if (!cleanSrcUnit.equals(oldSrcUnit)) {
            result = TryConvert(src, cleanSrcUnit, oldTarUnit);
            if (result != null) {
                return result;
            }
        }

        if (name == null || name.isEmpty()) {
            throw new RuntimeException("The input item has different units and cannot be converted from ["
                    + oldSrcUnit + "] to [" + oldTarUnit + "]");
        }
        throw new RuntimeException("The input item [" + name + "] has different units and cannot be converted from ["
                + oldSrcUnit + "] to [" + oldTarUnit + "]");
    }

    private static BigDecimal TryConvert(BigDecimal src, String srcUnit, String tarUnit) {
        BigDecimal result;
        result = DistanceConverter.TryConvert(srcUnit, tarUnit, src);
        if (result != null) return result;
        result = MassConverter.TryConvert(srcUnit, tarUnit, src);
        if (result != null) return result;
        result = AreaConverter.TryConvert(srcUnit, tarUnit, src);
        if (result != null) return result;
        result = VolumeConverter.TryConvert(srcUnit, tarUnit, src);
        if (result != null) return result;
        return null;
    }

    public static FunctionBase ParseFormula(String exp) throws Exception {
        if (exp == null || exp.trim().isEmpty()) {
            throw new IllegalArgumentException("Parameter exp invalid !");
        }
        AntlrErrorListener errorListener = new AntlrErrorListener();
        ProgContext context = CreateParserContext(exp, errorListener);
        if (errorListener.IsError) {
            throw new IllegalArgumentException(errorListener.ErrorMsg);
        }
        MathFunctionVisitor visitor = new MathFunctionVisitor();
        return visitor.visit(context);
    }

    public static boolean CheckFormula(String exp) {
        if (exp == null || exp.trim().isEmpty()) {
            return false;
        }
        AntlrErrorListener errorListener = new AntlrErrorListener();
        CreateParserContext(exp, errorListener);
        return !errorListener.IsError;
    }

    public static ConditionTree ParseCondition(String condition) {
        ConditionTree tree = new ConditionTree();
        if (condition == null || condition.trim().isEmpty()) {
            tree.Type = ConditionTreeType.Error;
            tree.ErrorMessage = "condition is null";
            return tree;
        }
        try {
            AntlrErrorListener errorListener = new AntlrErrorListener();
            ProgContext context = CreateParserContext(condition, errorListener);
            if (errorListener.IsError) {
                tree.Type = ConditionTreeType.Error;
                tree.ErrorMessage = errorListener.ErrorMsg;
                return tree;
            }
            MathSplitVisitor visitor = new MathSplitVisitor();
            return visitor.visit(context);
        } catch (Exception ex) {
            tree.Type = ConditionTreeType.Error;
            tree.ErrorMessage = ex.getMessage();
        }
        return tree;
    }

    public static FunctionBase Condition_And(FunctionBase left, FunctionBase right) {
        return new Function_AND(left, right);
    }

    public static FunctionBase Condition_Or(FunctionBase left, FunctionBase right) {
        return new Function_OR(left, right);
    }

    public static FunctionBase Error(FunctionBase txt) {
        return new Function_ERROR(txt);
    }

    public static CalculateTree ParseCalculate(String exp) {
        CalculateTree tree = new CalculateTree();
        if (exp == null || exp.trim().isEmpty()) {
            tree.Type = CalculateTreeType.Error;
            tree.ErrorMessage = "exp is null";
            return tree;
        }
        try {
            AntlrErrorListener errorListener = new AntlrErrorListener();
            ProgContext context = CreateParserContext(exp, errorListener);
            if (errorListener.IsError) {
                tree.Type = CalculateTreeType.Error;
                tree.ErrorMessage = errorListener.ErrorMsg;
                return tree;
            }
            MathSplitVisitor2 visitor = new MathSplitVisitor2();
            return visitor.visit(context);
        } catch (Exception ex) {
            tree.Type = CalculateTreeType.Error;
            tree.ErrorMessage = ex.getMessage();
        }
        return tree;
    }

    public static FunctionBase CombineCalculate(FunctionBase left, CombineCalculateType type, FunctionBase right) {
        switch (type) {
            case Add: return new Function_Add(left, right);
            case Sub: return new Function_Sub(left, right);
            case Mul: return new Function_Mul(left, right);
            case Div: return new Function_Div(left, right);
            case Mod: return new Function_Mod(left, right);
            case Connect: return new Function_Connect(left, right);
            case And: return new Function_AND(left, right);
            case Or: return new Function_OR(left, right);
            case OpGt: return new Function_GT(left, right);
            case OpLt: return new Function_LT(left, right);
            case OpGe: return new Function_GE(left, right);
            case OpLe: return new Function_LE(left, right);
            case OpEq: return new Function_EQ(left, right);
            case OpNe:
            default: return new Function_NE(left, right);
        }
    }
}
