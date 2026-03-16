package toolgood.algorithm.internals.functions;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.visitors.CharUtil;

public class FunctionUtil {
    public static final long START_DATE_UTC = 0;

    public static StringComparison GetStringComparison(boolean ignoreCase) {
        return ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
    }

    private static boolean FlattenToListCore(List<Operand> args, List<Operand> list) {
        for (int i = 0; i < args.size(); i++) {
            Operand item = args.get(i);
            if (item.IsArray()) {
                List<Operand> array = item.ArrayValue();
                for (int j = 0; j < array.size(); j++) {
                    List<Operand> tempList = new ArrayList<>();
                    tempList.add(array.get(j));
                    if (!FlattenToListCore(tempList, list)) return false;
                }
            } else if (item.IsJson()) {
                Operand i = item.ToArray(null);
                if (i.IsError()) return false;
                List<Operand> array = i.ArrayValue();
                for (int j = 0; j < array.size(); j++) {
                    List<Operand> tempList = new ArrayList<>();
                    tempList.add(array.get(j));
                    if (!FlattenToListCore(tempList, list)) return false;
                }
            } else {
                list.add(item);
            }
        }
        return true;
    }

    private static boolean FlattenToListCoreDecimal(List<Operand> args, List<BigDecimal> list) {
        for (int i = 0; i < args.size(); i++) {
            Operand item = args.get(i);
            if (item.IsArray()) {
                List<Operand> array = item.ArrayValue();
                for (int j = 0; j < array.size(); j++) {
                    List<Operand> tempList = new ArrayList<>();
                    tempList.add(array.get(j));
                    if (!FlattenToListCoreDecimal(tempList, list)) return false;
                }
            } else if (item.IsJson()) {
                Operand ii = item.ToArray(null);
                if (ii.IsError()) return false;
                List<Operand> array = ii.ArrayValue();
                for (int j = 0; j < array.size(); j++) {
                    List<Operand> tempList = new ArrayList<>();
                    tempList.add(array.get(j));
                    if (!FlattenToListCoreDecimal(tempList, list)) return false;
                }
            } else {
                Operand converted = item.IsNumber() ? item : item.ToNumber(null);
                if (converted.IsError()) return false;
                list.add(converted.NumberValue());
            }
        }
        return true;
    }

    private static boolean FlattenToListCoreDecimal(Operand args, List<BigDecimal> list) {
        if (args.IsError()) return false;
        if (args.IsArray()) {
            return FlattenToListCoreDecimal(args.ArrayValue(), list);
        } else if (args.IsJson()) {
            Operand i = args.ToArray(null);
            if (i.IsError()) return false;
            return FlattenToListCoreDecimal(i.ArrayValue(), list);
        } else {
            Operand converted = args.IsNumber() ? args : args.ToNumber(null);
            if (converted.IsError()) return false;
            list.add(converted.NumberValue());
        }
        return true;
    }

    private static boolean FlattenToListCoreString(Operand args, List<String> list) {
        if (args.IsError()) return false;
        if (args.IsArray()) {
            List<Operand> array = args.ArrayValue();
            for (int i = 0; i < array.size(); i++) {
                if (!FlattenToListCoreString(array.get(i), list)) return false;
            }
        } else if (args.IsJson()) {
            Operand i = args.ToArray(null);
            if (i.IsError()) return false;
            List<Operand> array = i.ArrayValue();
            for (int j = 0; j < array.size(); j++) {
                if (!FlattenToListCoreString(array.get(j), list)) return false;
            }
        } else {
            Operand converted = args.ToText(null);
            if (converted.IsError()) return false;
            list.add(converted.TextValue());
        }
        return true;
    }

    public static boolean FlattenToList(List<Operand> args, List<Operand> list) {
        return FlattenToListCore(args, list);
    }

    public static boolean FlattenToList(List<Operand> args, List<BigDecimal> list) {
        return FlattenToListCoreDecimal(args, list);
    }

    public static boolean FlattenToList(Operand args, List<BigDecimal> list) {
        return FlattenToListCoreDecimal(args, list);
    }

    public static boolean FlattenToList(Operand args, List<String> list) {
        return FlattenToListCoreString(args, list);
    }

    public static int GetCountIf(List<BigDecimal> dbs, BigDecimal d) {
        int count = 0;
        for (int i = 0; i < dbs.size(); i++) {
            BigDecimal item = dbs.get(i);
            if (item.compareTo(d) == 0) {
                count++;
            }
        }
        return count;
    }

    public static int GetCountIf(List<BigDecimal> dbs, String s, BigDecimal d) {
        int count = 0;
        for (int i = 0; i < dbs.size(); i++) {
            BigDecimal item = dbs.get(i);
            if (CompareValues(item, d, s)) {
                count++;
            }
        }
        return count;
    }

    public static BigDecimal GetSumIf(List<BigDecimal> dbs, BigDecimal d, List<BigDecimal> sumdbs) {
        BigDecimal sum = BigDecimal.ZERO;
        for (int i = 0; i < dbs.size(); i++) {
            BigDecimal item = dbs.get(i);
            if (item.compareTo(d) == 0) {
                sum = sum.add(sumdbs.get(i));
            }
        }
        return sum;
    }

    public static BigDecimal GetSumIf(List<BigDecimal> dbs, String s, BigDecimal d, List<BigDecimal> sumdbs) {
        BigDecimal sum = BigDecimal.ZERO;
        for (int i = 0; i < dbs.size(); i++) {
            if (CompareValues(dbs.get(i), d, s)) {
                sum = sum.add(sumdbs.get(i));
            }
        }
        return sum;
    }

    public static boolean CompareValues(BigDecimal a, BigDecimal b, String ss) {
        if (CharUtil.Equals(ss, '<')) {
            return a.compareTo(b) < 0;
        } else if (CharUtil.Equals(ss, "<=")) {
            return a.compareTo(b) <= 0;
        } else if (CharUtil.Equals(ss, '>')) {
            return a.compareTo(b) > 0;
        } else if (CharUtil.Equals(ss, ">=")) {
            return a.compareTo(b) >= 0;
        } else if (CharUtil.Equals(ss, "=", "==", "===")) {
            return a.compareTo(b) == 0;
        }
        return a.compareTo(b) != 0;
    }

    public static int GetGcd(List<BigDecimal> list) {
        if (list.size() == 0) return 1;
        
        int g = list.get(0).intValue();
        for (int i = 1; i < list.size(); i++) {
            g = GetGcd(g, list.get(i).intValue());
            if (g == 1) break;
        }
        return g;
    }

    public static int GetGcd(int a, int b) {
        while (b != 0) {
            int t = b;
            b = a % b;
            a = t;
        }
        return a;
    }

    public static int GetLcm(List<BigDecimal> list) {
        if (list.size() == 0) return 1;
        
        int a = 0;
        boolean foundFirst = false;
        
        for (int i = 0; i < list.size(); i++) {
            int val = list.get(i).intValue();
            if (val <= 1) continue;
            
            if (!foundFirst) {
                a = val;
                foundFirst = true;
                continue;
            }
            
            int b = val;
            int g = b > a ? GetGcd(b, a) : GetGcd(a, b);
            a = a / g * b;
        }
        return foundFirst ? a : 1;
    }

    public static int GetFactorial(int a) {
        if (a <= 0) return 1;
        int r = 1;
        for (int i = a; i > 0; i--) {
            r *= i;
        }
        return r;
    }

    public static Pair<String, BigDecimal> ParseSumIfMatch(String s) {
        if (s == null || s.length() == 0) return null;
        
        char c = s.charAt(0);
        if (c == '>' || c == '\uff1e') {
            if (s.length() > 1 && (s.charAt(1) == '=' || s.charAt(1) == '\uff1d')) {
                try {
                    BigDecimal d = new BigDecimal(s.substring(2).trim());
                    return new Pair<>(">=", d);
                } catch (NumberFormatException e) {
                    return null;
                }
            } else {
                try {
                    BigDecimal d = new BigDecimal(s.substring(1).trim());
                    return new Pair<>(">", d);
                } catch (NumberFormatException e) {
                    return null;
                }
            }
        } else if (c == '<' || c == '\uff1c') {
            if (s.length() > 1 && (s.charAt(1) == '=' || s.charAt(1) == '\uff1d')) {
                try {
                    BigDecimal d = new BigDecimal(s.substring(2).trim());
                    return new Pair<>("<=", d);
                } catch (NumberFormatException e) {
                    return null;
                }
            } else if (s.length() > 1 && (s.charAt(1) == '>' || s.charAt(1) == '\uff1e')) {
                try {
                    BigDecimal d = new BigDecimal(s.substring(2).trim());
                    return new Pair<>("!=", d);
                } catch (NumberFormatException e) {
                    return null;
                }
            } else {
                try {
                    BigDecimal d = new BigDecimal(s.substring(1).trim());
                    return new Pair<>("<", d);
                } catch (NumberFormatException e) {
                    return null;
                }
            }
        } else if (c == '=' || c == '\uff1d') {
            int index = 1;
            if (s.length() > 1 && (s.charAt(1) == '=' || s.charAt(1) == '\uff1d')) {
                index = 2;
                if (s.length() > 2 && (s.charAt(2) == '=' || s.charAt(2) == '\uff1d')) {
                    index = 3;
                }
            }
            try {
                BigDecimal d = new BigDecimal(s.substring(index).trim());
                return new Pair<>("=", d);
            } catch (NumberFormatException e) {
                return null;
            }
        } else if (c == '!' || c == '\uff01') {
            int index = 1;
            if (s.length() > 1 && (s.charAt(1) == '=' || s.charAt(1) == '\uff1d')) {
                index = 2;
                if (s.length() > 2 && (s.charAt(2) == '=' || s.charAt(2) == '\uff1d')) {
                    index = 3;
                }
            }
            try {
                BigDecimal d = new BigDecimal(s.substring(index).trim());
                return new Pair<>("!=", d);
            } catch (NumberFormatException e) {
                return null;
            }
        }
        return null;
    }

    public static boolean TryParseBoolean(String textValue, BooleanHolder boolValue) {
        int len = textValue.length();
        switch (len) {
            case 1: {
                char c = textValue.charAt(0);
                if (c == '1' || c == '\u662f' || c == '\u6709') {
                    boolValue.value = true;
                    return true;
                }
                if (c == '0' || c == '\u5426' || c == '\u65e0') {
                    boolValue.value = false;
                    return true;
                }
                break;
            }
            case 2: {
                if (textValue.equalsIgnoreCase("no")) {
                    boolValue.value = false;
                    return true;
                }
                if (textValue.equals("\u4e0d\u662f")) {
                    boolValue.value = false;
                    return true;
                }
                if (textValue.equals("\u6ca1\u6709")) {
                    boolValue.value = false;
                    return true;
                }
                break;
            }
            case 3: {
                if (textValue.equalsIgnoreCase("yes")) {
                    boolValue.value = true;
                    return true;
                }
                break;
            }
            case 4: {
                if (textValue.equalsIgnoreCase("true")) {
                    boolValue.value = true;
                    return true;
                }
                break;
            }
            case 5: {
                if (textValue.equalsIgnoreCase("false")) {
                    boolValue.value = false;
                    return true;
                }
                break;
            }
        }
        boolValue.value = false;
        return false;
    }

    public static BigDecimal QuickSelect(List<BigDecimal> list, int k, boolean largest) {
        if (list.size() == 1) return list.get(0);
        
        int targetIndex = largest ? list.size() - 1 - k : k;
        return QuickSelectCore(list, 0, list.size() - 1, targetIndex);
    }

    private static BigDecimal QuickSelectCore(List<BigDecimal> list, int left, int right, int k) {
        while (left < right) {
            int pivotIndex = Partition(list, left, right);
            if (k == pivotIndex) {
                return list.get(k);
            } else if (k < pivotIndex) {
                right = pivotIndex - 1;
            } else {
                left = pivotIndex + 1;
            }
        }
        return list.get(left);
    }

    private static int Partition(List<BigDecimal> list, int left, int right) {
        BigDecimal pivot = list.get(right);
        int i = left;
        for (int j = left; j < right; j++) {
            if (list.get(j).compareTo(pivot) <= 0) {
                Swap(list, i, j);
                i++;
            }
        }
        Swap(list, i, right);
        return i;
    }

    private static void Swap(List<BigDecimal> list, int i, int j) {
        if (i != j) {
            BigDecimal temp = list.get(i);
            list.set(i, list.get(j));
            list.set(j, temp);
        }
    }

    public static int GetRank(List<BigDecimal> values, BigDecimal num, boolean descending) {
        int rank = 1;
        int count = 0;
        for (int i = 0; i < values.size(); i++) {
            if (values.get(i).compareTo(num) == 0) {
                count++;
            } else if ((descending && values.get(i).compareTo(num) > 0) || (!descending && values.get(i).compareTo(num) < 0)) {
                rank++;
            }
        }
        return count > 0 ? rank : 0;
    }

    public static class BooleanHolder {
        public boolean value;
    }

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

    public enum StringComparison {
        Ordinal,
        OrdinalIgnoreCase
    }
}
