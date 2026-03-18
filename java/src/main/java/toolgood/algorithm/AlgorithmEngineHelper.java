/**
 * 算法引擎助手
 */
package toolgood.algorithm;

import java.util.regex.Pattern;

import org.antlr.v4.runtime.CharStreams;
import org.antlr.v4.runtime.CommonTokenStream;

import java.math.BigDecimal;
import toolgood.algorithm.enums.CalculateTreeType;
import toolgood.algorithm.enums.ConditionTreeType;
import toolgood.algorithm.internals.CalculateTree;
import toolgood.algorithm.internals.ConditionTree;
import toolgood.algorithm.internals.DiyNameInfo;
import toolgood.algorithm.internals.DiyNameKeyInfo;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.visitors.AntlrCharStream;
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

public class AlgorithmEngineHelper {
    private static final Pattern unitRegex = Pattern.compile("[\\s\\(\\)（）\\[\\]<>]");

    static ParserContext CreateParserContext(String exp) {
        AntlrErrorTextWriter errorWriter = new AntlrErrorTextWriter();
        AntlrCharStream stream = new AntlrCharStream(CharStreams.fromString(exp));
        mathLexer lexer = new mathLexer(stream);
        CommonTokenStream tokens = new CommonTokenStream(lexer);
        mathParser parser = new mathParser(tokens);
        mathParser.ProgContext context = parser.prog();
        return new ParserContext(errorWriter, context);
    }

    /// <summary>
    /// 是不是参数
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 获取 DIY 名称
    /// </summary>
    /// <param name="exp"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static DiyNameInfo GetDiyNames(String exp) throws Exception {
        if (exp == null || exp.trim().isEmpty()) {
            throw new Exception("Parameter exp invalid !");
        }
        ParserContext context = CreateParserContext(exp);
        if (context.errorWriter.IsError()) {
            throw new Exception(context.errorWriter.ErrorMsg());
        }
        DiyNameVisitor visitor = new DiyNameVisitor();
        visitor.visit(context.context);
        return visitor.diy;
    }

    /// <summary>
    /// 单位转换
    /// </summary>
    /// <param name="src"></param>
    /// <param name="oldSrcUnit"></param>
    /// <param name="oldTarUnit"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
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

    /// <summary>
    /// 编译公式
    /// </summary>
    /// <param name="exp">公式</param>
    /// <returns></returns>
    public static FunctionBase ParseFormula(String exp) throws Exception {
        if (exp == null || exp.trim().isEmpty()) {
            throw new Exception("Parameter exp invalid !");
        }
        ParserContext context = CreateParserContext(exp);
        if (context.errorWriter.IsError()) {
            throw new Exception(context.errorWriter.ErrorMsg());
        }
        MathFunctionVisitor visitor = new MathFunctionVisitor();
        return visitor.visitProg(context.context);
    }

    /// <summary>
    /// 检查公式是否正确
    /// </summary>
    /// <param name="exp"></param>
    /// <returns></returns>
    public static boolean CheckFormula(String exp) {
        if (exp == null || exp.trim().isEmpty()) {
            return false;
        }
        ParserContext context = CreateParserContext(exp);
        return !context.errorWriter.IsError();
    }

    /// <summary>
    /// 解析条件
    /// </summary>
    /// <param name="condition"></param>
    /// <returns></returns>
    public static ConditionTree ParseCondition(String condition) {
        ConditionTree tree = new ConditionTree();
        if (condition == null || condition.trim().isEmpty()) {
            tree.Type = ConditionTreeType.Error;
            tree.ErrorMessage = "condition is null";
            return tree;
        }
        try {
            ParserContext context = CreateParserContext(condition);
            if (context.errorWriter.IsError()) {
                tree.Type = ConditionTreeType.Error;
                tree.ErrorMessage = context.errorWriter.ErrorMsg();
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

    /// <summary>
    /// Creates a logical AND function that combines two specified functions.
    /// </summary>
    /// <param name="left">The left operand of the AND operation, representing the first function to be combined.</param>
    /// <param name="right">The right operand of the AND operation, representing the second function to be combined.</param>
    /// <returns>A new <see cref="FunctionBase"/> instance that represents the logical AND of the specified functions.</returns>
    public static FunctionBase Condition_And(FunctionBase left, FunctionBase right) {
        return new toolgood.algorithm.internals.functions.operator.Function_AND(left, right);
    }

    /// <summary>
    /// Creates a logical OR function that combines two specified functions.
    /// </summary>
    /// <param name="left">The left operand of the OR operation, representing the first function to be combined.</param>
    /// <param name="right">The right operand of the OR operation, representing the second function to be combined.</param>
    /// <returns>A new <see cref="FunctionBase"/> instance that represents the logical OR of the specified functions.</returns>
    public static FunctionBase Condition_Or(FunctionBase left, FunctionBase right) {
        return new toolgood.algorithm.internals.functions.operator.Function_OR(left, right);
    }

    /// <summary>
    /// 解析计算表达式
    /// </summary>
    /// <param name="exp"></param>
    /// <returns></returns>
    public static CalculateTree ParseCalculate(String exp) {
        CalculateTree tree = new CalculateTree();
        if (exp == null || exp.trim().isEmpty()) {
            tree.Type = CalculateTreeType.Error;
            tree.ErrorMessage = "exp is null";
            return tree;
        }
        try {
            ParserContext context = CreateParserContext(exp);
            if (context.errorWriter.IsError()) {
                tree.Type = CalculateTreeType.Error;
                tree.ErrorMessage = context.errorWriter.ErrorMsg();
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

    /// <summary>
    /// Creates a function that represents the sum of two specified functions.
    /// </summary>
    /// <param name="left">The first function to be added.</param>
    /// <param name="right">The second function to be added.</param>
    /// <returns>A function that computes the sum of the values returned by the specified functions.</returns>
    public static FunctionBase Calculate_Add(FunctionBase left, FunctionBase right) {
        return new toolgood.algorithm.internals.functions.operator.Function_Add(left, right);
    }

    /// <summary>
    /// Creates a function that represents the subtraction of two functions.
    /// </summary>
    /// <param name="left">The function to use as the minuend in the subtraction operation. Cannot be null.</param>
    /// <param name="right">The function to use as the subtrahend in the subtraction operation. Cannot be null.</param>
    /// <returns>A function that, when evaluated, returns the result of subtracting the value of the right function from the value
    /// of the left function.</returns>
    public static FunctionBase Calculate_Subtract(FunctionBase left, FunctionBase right) {
        return new toolgood.algorithm.internals.functions.operator.Function_Sub(left, right);
    }

    /// <summary>
    /// Creates a function that represents the multiplication of two functions.
    /// </summary>
    /// <param name="left">The left operand function to be multiplied.</param>
    /// <param name="right">The right operand function to be multiplied.</param>
    /// <returns>A function representing the product of the specified left and right functions.</returns>
    public static FunctionBase Calculate_Multiply(FunctionBase left, FunctionBase right) {
        return new toolgood.algorithm.internals.functions.operator.Function_Mul(left, right);
    }

    /// <summary>
    /// Creates a function that represents the division of two functions.
    /// </summary>
    /// <param name="left">The numerator function to be divided.</param>
    /// <param name="right">The denominator function by which to divide.</param>
    /// <returns>A function representing the result of dividing the left function by the right function.</returns>
    public static FunctionBase Calculate_Divide(FunctionBase left, FunctionBase right) {
        return new toolgood.algorithm.internals.functions.operator.Function_Div(left, right);
    }

    /// <summary>
    /// Creates a function that computes the remainder after dividing the result of the left function by the result of the
    /// right function.
    /// </summary>
    /// <param name="left">The function representing the dividend in the modulo operation. Cannot be null.</param>
    /// <param name="right">The function representing the divisor in the modulo operation. Cannot be null.</param>
    /// <returns>A function that, when evaluated, returns the result of the left function modulo the right function.</returns>
    public static FunctionBase Calculate_Mod(FunctionBase left, FunctionBase right) {
        return new toolgood.algorithm.internals.functions.operator.Function_Mod(left, right);
    }

    /// <summary>
    /// Creates a new function that represents the connection of two functions.
    /// </summary>
    /// <param name="left">The first function to be connected. Cannot be null.</param>
    /// <param name="right">The second function to be connected. Cannot be null.</param>
    /// <returns>A FunctionBase instance representing the connection of the specified left and right functions.</returns>
    public static FunctionBase Calculate_Connect(FunctionBase left, FunctionBase right) {
        return new toolgood.algorithm.internals.functions.operator.Function_Connect(left, right);
    }

    static class ParserContext {
        public AntlrErrorTextWriter errorWriter;
        public mathParser.ProgContext context;

        public ParserContext(AntlrErrorTextWriter errorWriter, mathParser.ProgContext context) {
            this.errorWriter = errorWriter;
            this.context = context;
        }
    }
}
