package toolgood.algorithm.internals.functions.string;

import java.math.BigDecimal;
import java.util.List;
import java.util.regex.Pattern;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_1;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_RMB extends Function_1 {
    private static final Pattern Regex1 = Pattern.compile("((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\\.]|$))))");
    private static final Pattern Regex2 = Pattern.compile(".");
    private static final String CHARS = "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰";

    public Function_RMB(FunctionBase func1) {
        super(func1);
    }

    @Override
    public String Name() {
        return "Rmb";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }
        return Operand.Create(F_base_ToChineseRMB(args1.NumberValue()));
    }

    private static String F_base_ToChineseRMB(BigDecimal x) {
        String s = String.format("%s", x);
        String formatStr = "#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A";
        s = formatNumber(x, formatStr);
        String d = Regex1.matcher(s).replaceAll("${b}${z}");
        StringBuilder result = new StringBuilder();
        java.util.regex.Matcher m = Regex2.matcher(d);
        while (m.find()) {
            char c = m.group().charAt(0);
            int idx = c - '-';
            if (idx >= 0 && idx < CHARS.length()) {
                result.append(CHARS.charAt(idx));
            }
        }
        return result.toString();
    }

    private static String formatNumber(BigDecimal x, String format) {
        StringBuilder sb = new StringBuilder();
        String numStr = x.abs().toPlainString();
        String[] parts = numStr.split("\\.");
        String intPart = parts[0];
        String decPart = parts.length > 1 ? parts[1] : "0";

        if (x.compareTo(BigDecimal.ZERO) < 0) {
            sb.append("-");
        }

        int formatIdx = 0;
        for (int i = 0; i < intPart.length() && formatIdx < format.length(); i++) {
            char f = format.charAt(formatIdx++);
            while (formatIdx < format.length() && (f == '#' || f == 'L' || f == 'E' || f == 'D' || f == 'C' || f == 'K' || f == 'J' || f == 'I' || f == 'H' || f == 'G' || f == 'F' || f == 'B' || f == 'A')) {
                sb.append(f);
                if (formatIdx < format.length()) {
                    f = format.charAt(formatIdx++);
                } else {
                    break;
                }
            }
            int digit = intPart.charAt(i) - '0';
            sb.append((char) ('A' + digit));
        }

        return sb.toString();
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.TEXT;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
