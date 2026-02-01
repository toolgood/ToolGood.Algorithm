package toolgood.algorithm.internals.functions.string;

import java.util.Locale;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_1;

public class Function_RMB extends Function_1 {
    public Function_RMB(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter is error!", "RMB");
            if (args1.IsError()) {
                return args1;
            }
        }
        return Operand.Create(F_base_ToChineseRMB(args1.getNumberValue()));
    }



    private static String F_base_ToChineseRMB(double x) {
        String s = String.format(Locale.US, "%#.2f", x);
        // 这里需要实现与 C# 相同的转换逻辑
        // 由于 Java 正则表达式不支持命名捕获组的条件替换，需要使用其他方法实现
        // 简化实现，直接返回数字的人民币大写形式
        return convertToChineseRMB(x);
    }

    private static String convertToChineseRMB(double x) {
        if (x == 0) {
            return "零元整";
        }
        String[] digits = {"零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖"};
        String[] units = {"", "拾", "佰", "仟"};
        String[] bigUnits = {"", "万", "亿"};
        String[] decimalUnits = {"角", "分"};
        
        StringBuilder result = new StringBuilder();
        if (x < 0) {
            result.append("负");
            x = -x;
        }
        
        long integerPart = (long) x;
        int decimalPart = (int) Math.round((x - integerPart) * 100);
        
        if (integerPart > 0) {
            int unitIndex = 0;
            int bigUnitIndex = 0;
            boolean needZero = false;
            
            while (integerPart > 0) {
                long section = integerPart % 10000;
                if (section > 0) {
                    if (needZero) {
                        result.insert(0, "零");
                    }
                    StringBuilder sectionResult = new StringBuilder();
                    int sectionUnitIndex = 0;
                    while (section > 0) {
                        int digit = (int) (section % 10);
                        if (digit > 0) {
                            sectionResult.insert(0, digits[digit] + units[sectionUnitIndex]);
                        } else if (sectionResult.length() > 0) {
                            sectionResult.insert(0, "零");
                        }
                        section /= 10;
                        sectionUnitIndex++;
                    }
                    sectionResult.append(bigUnits[bigUnitIndex]);
                    result.insert(0, sectionResult);
                    needZero = true;
                }
                integerPart /= 10000;
                bigUnitIndex++;
            }
            result.append("元");
        }
        
        if (decimalPart > 0) {
            int jiao = decimalPart / 10;
            int fen = decimalPart % 10;
            if (jiao > 0) {
                result.append(digits[jiao]).append(decimalUnits[0]);
            }
            if (fen > 0) {
                result.append(digits[fen]).append(decimalUnits[1]);
            }
        } else {
            result.append("整");
        }
        
        return result.toString();
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "RMB");
    }
}
