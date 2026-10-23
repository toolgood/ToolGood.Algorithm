package toolgood.algorithm;


import toolgood.algorithm.enums.CombineCalculateType;
import toolgood.algorithm.fast.FastParseException;
import toolgood.algorithm.fast.FastParser;
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
import toolgood.algorithm.unitConversion.AreaConverter;
import toolgood.algorithm.unitConversion.DistanceConverter;
import toolgood.algorithm.unitConversion.MassConverter;
import toolgood.algorithm.unitConversion.VolumeConverter;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

public class AlgorithmEngineHelper {

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
        List<DiyNameKeyInfo> parameters = new ArrayList<>();
        List<DiyNameKeyInfo> functions = new ArrayList<>();
        try {
            FastParser.parseDiyNames(exp, parameters, functions);
        } catch (FastParseException ex) {
            throw new IllegalArgumentException(ex.getMessage());
        }
        return new DiyNameInfo(parameters, functions);
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

        oldSrcUnit = oldSrcUnit.replaceAll("[\\s\\(\\)（）\\[\\]<>]", "");
        oldTarUnit = oldTarUnit.replaceAll("[\\s\\(\\)（）\\[\\]<>]", "");
        result = TryConvert(src, oldSrcUnit, oldTarUnit);
        if (result != null) {
            return result;
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
        return FastParser.parseFunction(exp);
    }

    public static boolean CheckFormula(String exp) {
        if (exp == null || exp.trim().isEmpty()) {
            return false;
        }
        try {
            FastParser.checkSyntax(exp);
            return true;
        } catch (FastParseException ex) {
            return false;
        }
    }

    public static ConditionTree ParseCondition(String condition) {
        ConditionTree tree = new ConditionTree();
        if (condition == null || condition.trim().isEmpty()) {
            tree.Type = ConditionTreeType.Error;
            tree.ErrorMessage = "condition is null";
            return tree;
        }
        try {
            return FastParser.parseConditionTree(condition);
        } catch (FastParseException ex) {
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
            return FastParser.parseCalculateTree(exp);
        } catch (FastParseException ex) {
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
