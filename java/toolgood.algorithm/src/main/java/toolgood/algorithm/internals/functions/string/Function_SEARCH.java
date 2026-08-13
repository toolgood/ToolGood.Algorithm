package toolgood.algorithm.internals.functions.string;

import java.util.List;
import java.util.function.BiFunction;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_SEARCH extends Function_3 {

    public Function_SEARCH(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 2 || funcs.length > 3) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires 2 to 3 parameters.");
        }
    }

    @Override
    public String Name() {
        return "Search";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetText_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }
        Operand args2 = GetText_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) { return args2; }

        int excelIndex = engine.ExcelIndex;

        if (func3 == null) {
            int index = wildcardIndexOf(args2.TextValue(), args1.TextValue(), 0);
            if (index < 0) {
                // 未找到:Excel 模式(索引从1开始)返回错误,C# 模式(索引从0开始)返回 -1
                return engine.ExcelIndex == 1 ? FunctionError() : Operand.Create(-1);
            }
            return Operand.Create(index + excelIndex);
        }
        Operand args3 = GetNumber_3(engine, tempParameter);
        if (args3.IsErrorOrNone()) { return args3; }
        int startIndex = args3.IntValue() - excelIndex;
        if (startIndex < 0 || startIndex >= args2.TextValue().length()) {
            return ParameterError(3);
        }
        int p2 = wildcardIndexOf(args2.TextValue(), args1.TextValue(), startIndex);
        if (p2 < 0) {
            return engine.ExcelIndex == 1 ? FunctionError() : Operand.Create(-1);
        }
        return Operand.Create(p2 + excelIndex);
    }

    // 在指定起始位置起做大小写不敏感的通配符查找,支持 Excel 的 ? 与 * 及 ~ 转义。
    private static int wildcardIndexOf(String text, String pattern, int startIndex) {
        Matcher m = Pattern.compile(wildcardToRegex(pattern), Pattern.CASE_INSENSITIVE).matcher(text);
        m.region(startIndex, text.length());
        if (m.find()) { return m.start(); }
        return -1;
    }

    private static String wildcardToRegex(String pattern) {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < pattern.length(); i++) {
            char c = pattern.charAt(i);
            if (c == '~') {
                if (i + 1 < pattern.length() && (pattern.charAt(i + 1) == '?' || pattern.charAt(i + 1) == '*' || pattern.charAt(i + 1) == '~')) {
                    sb.append(Pattern.quote(String.valueOf(pattern.charAt(i + 1))));
                    i++;
                } else {
                    sb.append(Pattern.quote("~"));
                }
            } else if (c == '*') {
                sb.append("[\\s\\S]*");
            } else if (c == '?') {
                sb.append("[\\s\\S]");
            } else {
                sb.append(Pattern.quote(String.valueOf(c)));
            }
        }
        return sb.toString();
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
        func2.GetParameterTypes(noneEngine, result, OperandType.TEXT);
        if (func3 != null) {
            func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        }
    }
}
