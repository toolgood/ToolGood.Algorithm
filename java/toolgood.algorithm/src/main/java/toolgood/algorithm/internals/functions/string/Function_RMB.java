package toolgood.algorithm.internals.functions.string;

import java.math.BigDecimal;
import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_1;

final class Function_RMB extends Function_1 {
    public Function_RMB(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 1) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 1 parameter.");
        }
    }

    @Override
    public String Name() {
        return "Rmb";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }
        return Operand.Create(F_base_ToChineseRMB(args1.NumberValue()));
    }

    // Character mapping: each char in formatted string maps via index = char - 45
    // Index: 0='-' 1='.' 2='/' 3='0' 4='1' 5='2' 6='3' 7='4' 8='5' 9='6' 10='7'
    //        11='8' 12='9' 13=':' 14=';' 15='<' 16='=' 17='>' 18='?' 19='@'
    //        20='A' 21='B' 22='C' 23='D' 24='E' 25='F' 26='G' 27='H' 28='I'
    //        29='J' 30='K' 31='L'
    private static final String CN_MAP = "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰";

    // Suffixes for integer positions (rightmost first): C D E F | C D E G | C D E H | ...
    // Pattern: C,D,E repeat; every 4th is F,G,H,I,J,K,L
    private static final String SUFFIXES = "CDEFCDEGCDEHCDEICDEJCDEKCDEL";

    private static String F_base_ToChineseRMB(BigDecimal x) {
        // Step 1: Build the same formatted string as C# decimal.ToString("#L#E#D#C...0B0A")
        StringBuilder fs = new StringBuilder();

        boolean isNegative = x.signum() < 0;
        if (isNegative) {
            fs.append('-');
            x = x.abs();
        }

        // Get plain string representation
        String plain = x.toPlainString();
        int dotIdx = plain.indexOf('.');
        String intPart;
        String decPart;
        if (dotIdx < 0) {
            intPart = plain;
            decPart = "00";
        } else {
            intPart = plain.substring(0, dotIdx);
            decPart = plain.substring(dotIdx + 1);
            if (decPart.length() > 2) {
                decPart = decPart.substring(0, 2);
            }
            while (decPart.length() < 2) {
                decPart = decPart + "0";
            }
        }

        // Build integer part with suffixes (rightmost digit gets suffix C, etc.)
        int suffixIdx = 0;
        StringBuilder intFormatted = new StringBuilder();
        for (int i = intPart.length() - 1; i >= 0 && suffixIdx < SUFFIXES.length(); i--, suffixIdx++) {
            intFormatted.insert(0, SUFFIXES.charAt(suffixIdx));
            intFormatted.insert(0, intPart.charAt(i));
        }

        fs.append(intFormatted);
        fs.append('.');
        // Decimal: .0B0A
        fs.append('0').append(decPart.charAt(0));
        fs.append('B');
        fs.append('0').append(decPart.charAt(1));
        fs.append('A');

        String s = fs.toString();

        // Step 2: Apply zero-group removal (same logic as Regex1)
        s = removeZeroGroups(s);

        // Step 3: Map each character to Chinese
        StringBuilder result = new StringBuilder();
        for (int i = 0; i < s.length(); i++) {
            char c = s.charAt(i);
            if (c == '-') {
                result.append(CN_MAP.charAt(0)); // negative
            } else if (c == '.') {
                result.append(CN_MAP.charAt(1)); // dot
            } else {
                int idx = c - '-';
                if (idx >= 0 && idx < CN_MAP.length()) {
                    result.append(CN_MAP.charAt(idx));
                }
            }
        }
        return result.toString();
    }

    // Replicates the C# Regex1 behavior:
    // ((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))
    // Replacement: ${b}${z}
    private static String removeZeroGroups(String s) {
        StringBuilder sb = new StringBuilder();
        int i = 0;
        int len = s.length();

        // Part 1: Skip leading non-digit chars (except '-') before first 1-9
        // This matches ((?<=-|^)[^1-9]*)
        // Keep '-' if it's at the beginning, then skip everything until first 1-9
        if (i < len && s.charAt(i) == '-') {
            sb.append('-');
            i++;
            // Skip until first 1-9
            while (i < len) {
                char c = s.charAt(i);
                if (c >= '1' && c <= '9') break;
                i++;
            }
        } else {
            // Skip leading chars before first 1-9
            while (i < len) {
                char c = s.charAt(i);
                if (c >= '1' && c <= '9') break;
                i++;
            }
        }

        // Process remaining characters
        // Part 2: (?'z'0)[0A-E]* until [1-9] or [F-L\.]|$
        // Part 3: (?'b'[F-L])(?'z'0)[0A-L]* until [1-9] or [\.\n]|$  (well, [\.]|$)
        while (i < len) {
            char c = s.charAt(i);

            if (c >= '1' && c <= '9') {
                sb.append(c);
                i++;
                continue;
            }

            if (c >= 'F' && c <= 'L') {
                // Part 3: group marker [F-L] followed by zero group
                // Check if followed by '0' + [0A-L]* that looks like a pure zero group
                int look = i + 1;
                if (look < len && s.charAt(look) == '0') {
                    boolean isPureZeroGroup = true;
                    int j = look;
                    while (j < len) {
                        char cc = s.charAt(j);
                        if (cc >= '1' && cc <= '9') {
                            // Non-zero found, this is NOT a pure zero group
                            isPureZeroGroup = false;
                            break;
                        }
                        if (cc == '.' || (cc >= 'F' && cc <= 'L') || cc == '$' || j == len - 1) {
                            // Reached group boundary or end
                            break;
                        }
                        // Check if char is in [0A-L] range
                        if (!(cc == '0' || (cc >= 'A' && cc <= 'L'))) {
                            break;
                        }
                        j++;
                    }
                    if (isPureZeroGroup) {
                        // Keep the group marker, skip the zero group
                        sb.append(c); // keep [F-L]
                        i++;
                        // Skip the '0' + [0A-L]* part
                        while (i < len) {
                            char cc = s.charAt(i);
                            if (cc >= '1' && cc <= '9') break;
                            if (cc == '.' || (cc >= 'F' && cc <= 'L')) break;
                            if (cc == '0' || (cc >= 'A' && cc <= 'L')) {
                                i++;
                            } else {
                                break;
                            }
                        }
                        continue;
                    }
                }
                // Not a pure zero group, keep the char
                sb.append(c);
                i++;
                continue;
            }

            if (c == '0') {
                // Part 2: zero potentially starting a zero group
                // Check if followed by [0A-E]* until [1-9] or [F-L\.]|$
                boolean isPureZeroGroup = true;
                int j = i + 1;
                while (j < len) {
                    char cc = s.charAt(j);
                    if (cc >= '1' && cc <= '9') {
                        isPureZeroGroup = false;
                        break;
                    }
                    if (cc == '.' || (cc >= 'F' && cc <= 'L') || j == len - 1) {
                        break;
                    }
                    if (!(cc == '0' || (cc >= 'A' && cc <= 'E'))) {
                        break;
                    }
                    j++;
                }
                if (isPureZeroGroup) {
                    // Skip the zero group
                    while (i < len) {
                        char cc = s.charAt(i);
                        if (cc >= '1' && cc <= '9') break;
                        if (cc == '.' || (cc >= 'F' && cc <= 'L')) break;
                        if (cc == '0' || (cc >= 'A' && cc <= 'E')) {
                            i++;
                        } else {
                            break;
                        }
                    }
                    continue;
                }
                // Not a pure zero group
                sb.append(c);
                i++;
                continue;
            }

            // Other chars (A-E, F-L digits? No they're covered above, others)
            sb.append(c);
            i++;
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
