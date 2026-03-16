package toolgood.algorithm;

import java.math.BigDecimal;
import java.util.regex.Pattern;

import org.antlr.v4.runtime.CharStreams;
import org.antlr.v4.runtime.CommonTokenStream;

import toolgood.algorithm.enums.CalculateTreeType;
import toolgood.algorithm.enums.ConditionTreeType;
import toolgood.algorithm.internals.CalculateTree;
import toolgood.algorithm.internals.ConditionTree;
import toolgood.algorithm.internals.DiyNameInfo;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.operator.Function_AND;
import toolgood.algorithm.internals.functions.operator.Function_OR;
import toolgood.algorithm.internals.functions.operator.Function_Add;
import toolgood.algorithm.internals.functions.operator.Function_Sub;
import toolgood.algorithm.internals.functions.operator.Function_Mul;
import toolgood.algorithm.internals.functions.operator.Function_Div;
import toolgood.algorithm.internals.functions.operator.Function_Mod;
import toolgood.algorithm.internals.functions.operator.Function_Connect;
import toolgood.algorithm.internals.visitors.AntlrErrorTextWriter;
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

/**
 * 算法引擎助手（静态工具类�?
 */
public final class AlgorithmEngineHelper {

    private static final Pattern UNIT_REGEX = Pattern.compile("[\\s\\(\\)（）\\[\\]<>]");

    /** 工具类，不可实例�?*/
    private AlgorithmEngineHelper() {
    }

    // -------------------------------------------------------------------------
    // 内部：创建解析上下文
    // -------------------------------------------------------------------------

    /**
     * 创建 ANTLR 解析上下�?
     *
     * @param exp 表达式字符串
     * @return [0] AntlrErrorTextWriter，[1] mathParser.ProgContext
     */
    static Object[] createParserContext(String exp) {
        AntlrErrorTextWriter errorWriter = new AntlrErrorTextWriter();
        org.antlr.v4.runtime.CharStream stream = CharStreams.fromString(exp);
        mathLexer lexer = new mathLexer(stream);
        CommonTokenStream tokens = new CommonTokenStream(lexer);
        mathParser parser = new mathParser(tokens);
        mathParser.ProgContext context = parser.prog();
        return new Object[]{errorWriter, context};
    }

    // -------------------------------------------------------------------------
    // 公共 API
    // -------------------------------------------------------------------------

    /**
     * 判断字符串是否是合法单参数名（既不包含函数调用，又只包含一个参数且与原字符串相同）
     *
     * @param parameter 待检测字符串
     * @return 是否是参�?
     */
    public static boolean IsParameter(String parameter) {
        if (parameter == null || parameter.trim().isEmpty()) {
            return false;
        }
        try {
            DiyNameInfo diy = GetDiyNames(parameter);
            if (!diy.Functions.isEmpty()) {
                return false;
            }
            if (diy.Parameters.size() == 1) {
                DiyNameInfo.KeyInfo p = diy.Parameters.get(0);
                return parameter.equals(p.Name);
            }
        } catch (Exception ignored) {
        }
        return false;
    }

    /**
     * 获取表达式中使用到的 DIY 名称（参数名和函数名�?
     *
     * @param exp 表达式字符串
     * @return DiyNameInfo
     * @throws Exception 解析失败时抛�?
     */
    public static DiyNameInfo GetDiyNames(String exp) throws Exception {
        if (exp == null || exp.trim().isEmpty()) {
            throw new Exception("Parameter exp invalid !");
        }
        Object[] result = createParserContext(exp);
        AntlrErrorTextWriter errorWriter = (AntlrErrorTextWriter) result[0];
        if (errorWriter.IsError()) {
            throw new Exception(errorWriter.ErrorMsg());
        }
        mathParser.ProgContext context = (mathParser.ProgContext) result[1];
        DiyNameVisitor visitor = new DiyNameVisitor();
        visitor.visit(context);
        return visitor.diy;
    }

    /**
     * 单位转换
     *
     * @param src        原始数�?
     * @param oldSrcUnit 源单�?
     * @param oldTarUnit 目标单位
     * @return 转换后的 BigDecimal
     * @throws Exception 单位不兼容时抛出
     */
    public static BigDecimal UnitConversion(BigDecimal src, String oldSrcUnit, String oldTarUnit) throws Exception {
        return UnitConversion(src, oldSrcUnit, oldTarUnit, null);
    }

    /**
     * 单位转换（带名称提示�?
     *
     * @param src        原始数�?
     * @param oldSrcUnit 源单�?
     * @param oldTarUnit 目标单位
     * @param name       输入项名称（用于错误信息，可�?null�?
     * @return 转换后的 BigDecimal
     * @throws Exception 单位不兼容时抛出
     */
    public static BigDecimal UnitConversion(BigDecimal src, String oldSrcUnit, String oldTarUnit, String name) throws Exception {
        if (oldSrcUnit == null || oldSrcUnit.trim().isEmpty()
                || oldTarUnit == null || oldTarUnit.trim().isEmpty()) {
            return src;
        }
        if (oldSrcUnit.equals(oldTarUnit)) {
            return src;
        }

        BigDecimal result = tryConvert(src, oldSrcUnit, oldTarUnit);
        if (result != null) {
            return result;
        }

        String cleanedSrcUnit = UNIT_REGEX.matcher(oldSrcUnit).replaceAll("");
        result = tryConvert(src, cleanedSrcUnit, oldTarUnit);
        if (result != null) {
            return result;
        }

        if (name == null || name.isEmpty()) {
            throw new Exception(
                    "The input item has different units and cannot be converted from [" + oldSrcUnit + "] to [" + oldTarUnit + "]");
        }
        throw new Exception(
                "The input item [" + name + "] has different units and cannot be converted from [" + oldSrcUnit + "] to [" + oldTarUnit + "]");
    }

    private static BigDecimal tryConvert(BigDecimal src, String srcUnit, String tarUnit) {
        try {
            if (DistanceConverter.Exists(srcUnit, tarUnit)) {
                return new DistanceConverter(srcUnit, tarUnit).LeftToRight(src);
            }
            if (MassConverter.Exists(srcUnit, tarUnit)) {
                return new MassConverter(srcUnit, tarUnit).LeftToRight(src);
            }
            if (AreaConverter.Exists(srcUnit, tarUnit)) {
                return new AreaConverter(srcUnit, tarUnit).LeftToRight(src);
            }
            if (VolumeConverter.Exists(srcUnit, tarUnit)) {
                return new VolumeConverter(srcUnit, tarUnit).LeftToRight(src);
            }
        } catch (Exception ignored) {
        }
        return null;
    }

    /**
     * 编译公式，返回可执行�?FunctionBase
     *
     * @param exp 公式字符�?
     * @return 编译后的 FunctionBase
     * @throws Exception 解析失败时抛�?
     */
    public static FunctionBase ParseFormula(String exp) throws Exception {
        if (exp == null || exp.trim().isEmpty()) {
            throw new Exception("Parameter exp invalid !");
        }
        Object[] result = createParserContext(exp);
        AntlrErrorTextWriter errorWriter = (AntlrErrorTextWriter) result[0];
        if (errorWriter.IsError()) {
            throw new Exception(errorWriter.ErrorMsg());
        }
        mathParser.ProgContext context = (mathParser.ProgContext) result[1];
        MathFunctionVisitor visitor = new MathFunctionVisitor();
        return visitor.visit(context);
    }

    /**
     * 检查公式语法是否正�?
     *
     * @param exp 公式字符�?
     * @return 是否正确
     */
    public static boolean CheckFormula(String exp) {
        if (exp == null || exp.trim().isEmpty()) {
            return false;
        }
        Object[] result = createParserContext(exp);
        AntlrErrorTextWriter errorWriter = (AntlrErrorTextWriter) result[0];
        return !errorWriter.IsError();
    }

    /**
     * 解析条件表达式，生成 ConditionTree
     *
     * @param condition 条件字符�?
     * @return ConditionTree
     */
    public static ConditionTree ParseCondition(String condition) {
        ConditionTree tree = new ConditionTree();
        if (condition == null || condition.trim().isEmpty()) {
            tree.Type = ConditionTreeType.Error;
            tree.ErrorMessage = "condition is null";
            return tree;
        }
        try {
            Object[] result = createParserContext(condition);
            AntlrErrorTextWriter errorWriter = (AntlrErrorTextWriter) result[0];
            if (errorWriter.IsError()) {
                tree.Type = ConditionTreeType.Error;
                tree.ErrorMessage = errorWriter.ErrorMsg();
                return tree;
            }
            mathParser.ProgContext context = (mathParser.ProgContext) result[1];
            MathSplitVisitor visitor = new MathSplitVisitor();
            return visitor.visit(context);
        } catch (Exception ex) {
            tree.Type = ConditionTreeType.Error;
            tree.ErrorMessage = ex.getMessage();
        }
        return tree;
    }

    /**
     * 创建 AND 逻辑函数（左 AND 右）
     *
     * @param left  左操作数函数
     * @param right 右操作数函数
     * @return AND 函数
     */
    public static FunctionBase Condition_And(FunctionBase left, FunctionBase right) {
        return new Function_AND(left, right);
    }

    /**
     * 创建 OR 逻辑函数（左 OR 右）
     *
     * @param left  左操作数函数
     * @param right 右操作数函数
     * @return OR 函数
     */
    public static FunctionBase Condition_Or(FunctionBase left, FunctionBase right) {
        return new Function_OR(left, right);
    }

    /**
     * 解析计算表达式，生成 CalculateTree
     *
     * @param exp 表达式字符串
     * @return CalculateTree
     */
    public static CalculateTree ParseCalculate(String exp) {
        CalculateTree tree = new CalculateTree();
        if (exp == null || exp.trim().isEmpty()) {
            tree.Type = CalculateTreeType.Error;
            tree.ErrorMessage = "exp is null";
            return tree;
        }
        try {
            Object[] result = createParserContext(exp);
            AntlrErrorTextWriter errorWriter = (AntlrErrorTextWriter) result[0];
            if (errorWriter.IsError()) {
                tree.Type = CalculateTreeType.Error;
                tree.ErrorMessage = errorWriter.ErrorMsg();
                return tree;
            }
            mathParser.ProgContext context = (mathParser.ProgContext) result[1];
            MathSplitVisitor2 visitor = new MathSplitVisitor2();
            return visitor.visit(context);
        } catch (Exception ex) {
            tree.Type = CalculateTreeType.Error;
            tree.ErrorMessage = ex.getMessage();
        }
        return tree;
    }

    /**
     * 创建加法运算函数
     */
    public static FunctionBase Calculate_Add(FunctionBase left, FunctionBase right) {
        return new Function_Add(left, right);
    }

    /**
     * 创建减法运算函数
     */
    public static FunctionBase Calculate_Subtract(FunctionBase left, FunctionBase right) {
        return new Function_Sub(left, right);
    }

    /**
     * 创建乘法运算函数
     */
    public static FunctionBase Calculate_Multiply(FunctionBase left, FunctionBase right) {
        return new Function_Mul(left, right);
    }

    /**
     * 创建除法运算函数
     */
    public static FunctionBase Calculate_Divide(FunctionBase left, FunctionBase right) {
        return new Function_Div(left, right);
    }

    /**
     * 创建取模运算函数
     */
    public static FunctionBase Calculate_Mod(FunctionBase left, FunctionBase right) {
        return new Function_Mod(left, right);
    }

    /**
     * 创建字符串连接函�?
     */
    public static FunctionBase Calculate_Connect(FunctionBase left, FunctionBase right) {
        return new Function_Connect(left, right);
    }
}
