package toolgood.algorithm.internals.functions.string;

import java.util.regex.Matcher;
import java.util.regex.Pattern;

import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_1;

public class Function_RMB extends Function_1 {
    private static final Pattern Regex1 = Pattern.compile("((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\\.]|$))))");
    private static final Pattern Regex2 = Pattern.compile(".");

    public Function_RMB(FunctionBase func1) {
        super(func1);
    }

    @Override
    public String getName() {
        return "RMB";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.isNumber() == false) {
            return Operand.CreateError("Function 'RMB' parameter is error!");
        }
        return Operand.Create(F_base_ToChineseRMB(args1.numberValue()));
    }

    private static String F_base_ToChineseRMB(double x) {
        String s = formatWithScale(x);
        String d = Regex1.matcher(s).replaceAll("${b}${z}");
        Matcher m = Regex2.matcher(d);
        StringBuilder result = new StringBuilder();
        String chars = "\u8d1f\u5143\u7a7a\u96f6\u58f9\u8d30\u53c1\u8086\u4e94\u9646\u67d2\u4e03\u634c\u4e5d\u7a7a\u7a7a\u7a7a\u7a7a\u7a7a\u7a7a\u7a7a\u5206\u89d2\u62fe\u4f70\u4edf\u4e07\u4ebf\u5147\u4eac\u57ab\u79cd\u7a37";
        while (m.find()) {
            char c = m.group().charAt(0);
            int idx = c - '-';
            if (idx >= 0 && idx < chars.length()) {
                result.append(chars.charAt(idx));
            } else {
                result.append(c);
            }
        }
        return result.toString();
    }

    private static String formatWithScale(double x) {
        if (x == 0) return "0";

        StringBuilder sb = new StringBuilder();
        boolean negative = x < 0;
        if (negative) x = -x;

        long intPart = (long) x;
        int decimalPart = (int) Math.round((x - intPart) * 100);

        if (intPart == 0) {
            sb.append("0");
        } else {
            String intStr = Long.toString(intPart);
            for (int i = 0; i < intStr.length(); i++) {
                char c = intStr.charAt(i);
                if (c >= '0' && c <= '9') {
                    int digit = c - '0';
                    if (digit == 0) {
                        sb.append('A');
                    } else {
                        sb.append((char) ('A' + digit - 1));
                    }
                }
                int scalePos = intStr.length() - 1 - i;
                if (scalePos > 0) {
                    int scale = scalePos % 4;
                    if (scale == 0) {
                        if (scalePos == 4) sb.append('J');
                        else if (scalePos == 8) sb.append('K');
                        else if (scalePos == 12) sb.append('L');
                        else if (scalePos == 16) sb.append('M');
                        else if (scalePos == 20) sb.append('N');
                    } else if (scale == 1) {
                        if (scalePos == 1) sb.append('B');
                        else if (scalePos == 5) sb.append('F');
                        else if (scalePos == 9) sb.append('J');
                        else if (scalePos == 13) sb.append('N');
                    } else if (scale == 2) {
                        if (scalePos == 2) sb.append('C');
                        else if (scalePos == 6) sb.append('G');
                        else if (scalePos == 10) sb.append('K');
                        else if (scalePos == 14) sb.append('O');
                    } else if (scale == 3) {
                        if (scalePos == 3) sb.append('D');
                        else if (scalePos == 7) sb.append('H');
                        else if (scalePos == 11) sb.append('L');
                        else if (scalePos == 15) sb.append('P');
                    }
                }
            }
        }

        if (decimalPart > 0) {
            sb.append('.');
            int jiao = decimalPart / 10;
            int fen = decimalPart % 10;
            if (jiao > 0) {
                sb.append((char) ('A' + jiao - 1));
            }
            if (fen > 0) {
                sb.append((char) ('A' + fen - 1));
            }
        } else {
            sb.append(".0B0A");
        }

        return negative ? "-" + sb.toString() : sb.toString();
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "RMB");
    }
}
