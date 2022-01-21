package toolgood.algorithm.internals;

import java.util.ArrayList;
import java.util.List;

public class CharUtil {
    public static char StandardChar(char c)
    {
        if (c <= 0) return c;
        char o = (char)c;
        if (o == '‘') return '\'';
        if (o == '’') return '\'';
        if (o == '“') return '"';
        if (o == '”') return '"';
        if (o == '〔') return '(';
        if (o == '〕') return ')';
        if (o == '＝') return '=';
        if (o == '＋') return '+';
        if (o == '－') return '-';
        if (o == '×') return '*';
        if (o == '÷') return '/';
        if (o == '／') return '/';

        if (c == 12288) {
            o = (char)32;
        } else if (c > 65280 && c < 65375) {
            o = (char)(c - 65248);
        }
        return Character.toUpperCase(o);
    }

    private static boolean EqualsOnce(String left, String right)
    {
        if (left.length() != right.length()) return false;
        for (int i = 0; i < left.length(); i++) {
            if (left.charAt(i)  != right.charAt(i)) {
                char a = StandardChar(left.charAt(i));
                char b = StandardChar(right.charAt(i));
                if (a != b) return false;
            }
        }
        return true;
    }

    public static boolean Equals(String left, String right)
    {
        if (left == null) return false;
        if (right == null) return false;
        return EqualsOnce(left, right);
    }
    public static boolean Equals(String left, String arg1, String arg2)
    {
        if (left == null) return false;
        if (arg1 != null && EqualsOnce(left, arg1))
            return true;
        if (arg2 != null && EqualsOnce(left, arg2))
            return true;
        return false;
    }

    public static boolean Equals(String left, String arg1, String arg2, String arg3)
    {
        if (left == null) return false;
        if (arg1 != null && EqualsOnce(left, arg1))
            return true;
        if (arg2 != null && EqualsOnce(left, arg2))
            return true;
        if (arg3 != null && EqualsOnce(left, arg3))
            return true;
        return false;
    }

    public static List<String> SplitFormula(String formula, List<Character> splitChars) {
        List<String> result = new ArrayList<>();
        boolean inSquareBrackets = false;
        boolean inBraceBrackets = false;
        int inBracketsCount = 0;
        boolean inText = false;
        char textChar = (char) 0;

        StringBuilder str = new StringBuilder();
        Integer i = 0;
        while (i < formula.length()) {
            char c = formula.charAt(i);
            if (inSquareBrackets) {
                str.append(c);
                if (c == ']') inSquareBrackets = false;
            } else if (inBraceBrackets) {
                str.append(c);
                if (c == '}') inBraceBrackets = false;
            } else if (inText) {
                str.append(c);
                if (c == '\\') {
                    i++;
                    if (i < formula.length()){
                        str.append(formula.charAt(i));
                    }
                } else if (c == textChar) {
                    inText = false;
                }
            } else if (splitChars.contains(c) && inBracketsCount == 0) {
                result.add(str.toString());
                result.add(((Character) c).toString());
                str = new StringBuilder();
            } else {
                str.append(c);
                if (c == '\'' || c == '"' || c == '`') {
                    textChar = c;
                    inText = true;
                } else if (c == '[') {
                    inSquareBrackets = true;
                } else if (c == '{') {
                    inBraceBrackets = true;
                } else if (c == '(') {
                    inBracketsCount++;
                } else if (c == ')') {
                    inBracketsCount--;
                }
            }
            i++;
        }
        if (str.length() > 0)
            result.add(str.toString());
        return result;
    }

    public static List<String> SplitFormulaForAnd(String formula)
    {
        List<String> result = new ArrayList<>();
        boolean inSquareBrackets = false;
        boolean inBraceBrackets = false;
        int inBracketsCount = 0;
        boolean inText = false;
        char textChar = (char) 0;

        StringBuilder str = new StringBuilder();
        Integer i = 0;
        while (i < formula.length()) {
            char c = formula.charAt(i);
            if (inSquareBrackets) {
                str.append(c);
                if (c == ']') inSquareBrackets = false;
            } else if (inBraceBrackets) {
                str.append(c);
                if (c == '}') inBraceBrackets = false;
            } else if (inText) {
                str.append(c);
                if (c == '\\') {
                    i++;
                    if (i < formula.length()){
                        str.append(formula.charAt(i));
                    }
                } else if (c == textChar) {
                    inText = false;
                }
            } else if (c == '&' && inBracketsCount == 0) {
                if (i + 1 < formula.length() && formula.charAt(i + 1) == '&') {
                    i++;
                    result.add(str.toString());
                    str = new StringBuilder();
                } else {
                    result.add(str.toString());
                }
            } else if (c == '|' && inBracketsCount == 0) {
                if (i + 1 < formula.length() && formula.charAt(i + 1) == '|') {
                    i++;
                    result.add(str.toString());
                    str = new StringBuilder();
                    str.append(String.join("&&", result));
                    str.append("||");
                    result.clear();
                } else {
                    result.add(str.toString());
                }
            } else {
                str.append(c);
                if (c == '\'' || c == '"' || c == '`') {
                    textChar = c;
                    inText = true;
                } else if (c == '[') {
                    inSquareBrackets = true;
                } else if (c == '{') {
                    inBraceBrackets = true;
                } else if (c == '(') {
                    inBracketsCount++;
                } else if (c == ')') {
                    inBracketsCount--;
                }
            }
            i++;
        }
        if (str.length() > 0)
            result.add(str.toString());
        return result;
    }

}
