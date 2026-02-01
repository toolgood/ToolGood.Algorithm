package toolgood.algorithm.internals.functions;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.Objects;
import java.util.function.Predicate;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.visitors.CharUtil;

public class FunctionUtil {
    public static final long START_DATE_UTC = 0; // 1970-01-01 00:00:00 UTC in milliseconds

    public static boolean F_base_GetList_BigDecimal(List<Operand> args, List<BigDecimal> list) {
        for (Operand item : args) {
            if (item.IsNumber()) {
                list.add(item.NumberValue());
            } else if (item.IsArray()) {
                boolean o = F_base_GetList(item.ArrayValue(), list);
                if (!o) {
                    return false;
                }
            } else if (item.IsJson()) {
                Operand i = item.ToArray(null);
                if (i.IsError()) {
                    return false;
                }
                boolean o = F_base_GetList(i.ArrayValue(), list);
                if (!o) {
                    return false;
                }
            } else {
                Operand o = item.ToNumber(null);
                if (o.IsError()) {
                    return false;
                }
                list.add(o.DoubleValue
());
            }
        }
        return true;
    }

    public static boolean F_base_GetList_Double(Operand args, List<Double> list) {
        if (args.IsError()) {
            return false;
        }
        if (args.IsNumber()) {
            list.add(args.DoubleValue
());
        } else if (args.IsArray()) {
            boolean o = F_base_GetList(args.ArrayValue(), list);
            if (!o) {
                return false;
            }
        } else if (args.IsJson()) {
            Operand i = args.ToArray(null);
            if (i.IsError()) {
                return false;
            }
            boolean o = F_base_GetList(i.ArrayValue(), list);
            if (!o) {
                return false;
            }
        } else {
            Operand o = args.ToNumber(null);
            if (o.IsError()) {
                return false;
            }
            list.add(o.DoubleValue
());
        }
        return true;
    }

    public static boolean F_base_GetList(List<Operand> args, List<String> list) {
        for (Operand item : args) {
            if (item.IsArray()) {
                boolean o = F_base_GetList(item.ArrayValue(), list);
                if (!o) {
                    return false;
                }
            } else if (item.IsJson()) {
                Operand i = item.ToArray(null);
                if (i.IsError()) {
                    return false;
                }
                boolean o = F_base_GetList(i.ArrayValue(), list);
                if (!o) {
                    return false;
                }
            } else {
                Operand o = item.ToText(null);
                if (o.IsError()) {
                    return false;
                }
                list.add(o.TextValue());
            }
        }
        return true;
    }

    public static boolean F_base_GetList_String(Operand args, List<String> list) {
        if (args.IsError()) {
            return false;
        }
        if (args.IsArray()) {
            boolean o = F_base_GetList(args.ArrayValue(), list);
            if (!o) {
                return false;
            }
        } else if (args.IsJson()) {
            Operand i = args.ToArray(null);
            if (i.IsError()) {
                return false;
            }
            boolean o = F_base_GetList(i.ArrayValue(), list);
            if (!o) {
                return false;
            }
        } else {
            Operand o = args.ToText(null);
            if (o.IsError()) {
                return false;
            }
            list.add(o.TextValue());
        }
        return true;
    }

    public static int F_base_countif(List<Double> dbs, double d) {
        int count = 0;
        for (double item : dbs) {
            if (item == d) {
                count++;
            }
        }
        return count;
    }

    public static int F_base_countif(List<Double> dbs, String s, double d) {
        int count = 0;
        for (double item : dbs) {
            if (F_base_compare(item, d, s)) {
                count++;
            }
        }
        return count;
    }

    public static double F_base_sumif(List<Double> dbs, double d, List<Double> sumdbs) {
        double sum = 0;
        for (int i = 0; i < dbs.size(); i++) {
            double item = dbs.get(i);
            if (item == d) {
                sum += sumdbs.get(i);
            }
        }
        return sum;
    }

    public static double F_base_sumif(List<Double> dbs, String s, double d, List<Double> sumdbs) {
        double sum = 0;
        for (int i = 0; i < dbs.size(); i++) {
            if (F_base_compare(dbs.get(i), d, s)) {
                sum += sumdbs.get(i);
            }
        }
        return sum;
    }

    public static boolean F_base_compare(double a, double b, String ss) {
        if (CharUtil.Equals(ss, "<")) {
            return a < b;
        } else if (CharUtil.Equals(ss, "<=")) {
            return a <= b;
        } else if (CharUtil.Equals(ss, ">")) {
            return a > b;
        } else if (CharUtil.Equals(ss, ">=")) {
            return a >= b;
        } else if (CharUtil.Equals(ss, "=") || CharUtil.Equals(ss, "==") || CharUtil.Equals(ss, "===")) {
            return a == b;
        }
        return a != b;
    }

    public static int F_base_gcd(List<Double> list) {
        List<Double> sortedList = new ArrayList<>(list);
        Collections.sort(sortedList);
        int g = F_base_gcd((int) sortedList.get(1).doubleValue(), (int) sortedList.get(0).doubleValue());
        for (int i = 2; i < sortedList.size(); i++) {
            g = F_base_gcd((int) sortedList.get(i).doubleValue(), g);
        }
        return g;
    }

    public static int F_base_gcd(int a, int b) {
        if (b == 1) {
            return 1;
        }
        if (b == 0) {
            return a;
        }
        return F_base_gcd(b, a % b);
    }

    public static int F_base_lgm(List<Double> list) {
        List<Double> sortedList = new ArrayList<>(list);
        Collections.sort(sortedList);
        sortedList.removeIf(d -> d <= 1);

        int a = (int) sortedList.get(0).doubleValue();
        for (int i = 1; i < sortedList.size(); i++) {
            int b = (int) sortedList.get(i).doubleValue();
            int g = b > a ? F_base_gcd(b, a) : F_base_gcd(a, b);
            a = a / g * b;
        }
        return a;
    }

    public static int F_base_Factorial(int a) {
        if (a <= 0) {
            return 1;
        }
        int r = 1;
        for (int i = a; i > 0; i--) {
            r *= i;
        }
        return r;
    }

    public static Pair<String, Double> sumifMatch(String s) {
        if (s == null || s.isEmpty()) {
            return null;
        }
        char c = s.charAt(0);
        if (c == '>' || c == '＞') {
            if (s.length() > 1 && (s.charAt(1) == '=' || s.charAt(1) == '＝')) {
                try {
                    double d = Double.parseDouble(s.substring(2).trim());
                    return new Pair<>(">=", d);
                } catch (NumberFormatException e) {
                    return null;
                }
            } else {
                try {
                    double d = Double.parseDouble(s.substring(1).trim());
                    return new Pair<>(">", d);
                } catch (NumberFormatException e) {
                    return null;
                }
            }
        } else if (c == '<' || c == '＜') {
            if (s.length() > 1 && (s.charAt(1) == '=' || s.charAt(1) == '＝')) {
                try {
                    double d = Double.parseDouble(s.substring(2).trim());
                    return new Pair<>("<=", d);
                } catch (NumberFormatException e) {
                    return null;
                }
            } else if (s.length() > 1 && (s.charAt(1) == '>' || s.charAt(1) == '＞')) {
                try {
                    double d = Double.parseDouble(s.substring(2).trim());
                    return new Pair<>("!=", d);
                } catch (NumberFormatException e) {
                    return null;
                }
            } else {
                try {
                    double d = Double.parseDouble(s.substring(1).trim());
                    return new Pair<>("<", d);
                } catch (NumberFormatException e) {
                    return null;
                }
            }
        } else if (c == '=' || c == '＝') {
            int index = 1;
            if (s.length() > 1 && (s.charAt(1) == '=' || s.charAt(1) == '＝')) {
                index = 2;
                if (s.length() > 2 && (s.charAt(2) == '=' || s.charAt(2) == '＝')) {
                    index = 3;
                }
            }
            try {
                double d = Double.parseDouble(s.substring(index).trim());
                return new Pair<>("=", d);
            } catch (NumberFormatException e) {
                return null;
            }
        } else if (c == '!' || c == '！') {
            int index = 1;
            if (s.length() > 1 && (s.charAt(1) == '=' || s.charAt(1) == '＝')) {
                index = 2;
                if (s.length() > 2 && (s.charAt(2) == '=' || s.charAt(2) == '＝')) {
                    index = 3;
                }
            }
            try {
                double d = Double.parseDouble(s.substring(index).trim());
                return new Pair<>("!=", d);
            } catch (NumberFormatException e) {
                return null;
            }
        }
        return null;
    }

    public static int Compare(double t1, double t2) {
        double b = t1 - t2;
        if (b == 0) {
            return 0;
        } else if (b > 0) {
            return 1;
        }
        return -1;
    }

    public static boolean tryParseBoolean(String textValue, BooleanHolder boolValue) {
        if (textValue.equalsIgnoreCase("true")) {
            boolValue.value = true;
            return true;
        }
        if (textValue.equalsIgnoreCase("false")) {
            boolValue.value = false;
            return true;
        }
        if (textValue.equalsIgnoreCase("yes")) {
            boolValue.value = true;
            return true;
        }
        if (textValue.equalsIgnoreCase("no")) {
            boolValue.value = false;
            return true;
        }
        if (textValue.equals("1") || textValue.equals("是") || textValue.equals("有")) {
            boolValue.value = true;
            return true;
        }
        if (textValue.equals("0") || textValue.equals("否") || textValue.equals("不是") || textValue.equals("无") || textValue.equals("没有")) {
            boolValue.value = false;
            return true;
        }
        boolValue.value = false;
        return false;
    }

    // 辅助类，用于在tryParseBoolean中返回布尔值
    public static class BooleanHolder {
        public boolean value;
    }

    // 辅助类，用于替代Tuple
    public static class Pair<F, S> {
        private final F first;
        private final S second;

        public Pair(F first, S second) {
            this.first = first;
            this.second = second;
        }

        public F getFirst() {
            return first;
        }

        public S getSecond() {
            return second;
        }
    }
}
