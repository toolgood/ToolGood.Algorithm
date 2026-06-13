package toolgood.algorithm.internals.functions;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.AbstractMap.SimpleEntry;
import java.util.List;
import java.util.ArrayList;
import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.visitors.CharUtil;

public class FunctionUtil {
    public static final DateTime StartDateUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeZone.UTC);

    public static boolean GetStringComparison(boolean ignoreCase) {
        return ignoreCase;
    }

    private static int EstimateCount(List<Operand> args) {
        int count = 0;
        for (int i = 0; i < args.size(); i++) {
            Operand item = args.get(i);
            if (item.Type() == OperandType.ARRAY) {
                count += item.ArrayValue().size();
            } else if (item.Type() == OperandType.JSON) {
                count += 8;
            } else {
                count++;
            }
        }
        return count;
    }

    public static boolean FlattenToList(List<Operand> args, List<Operand> list) {
        list.ensureCapacity(Math.max(list.size(), EstimateCount(args)));
        List<Operand> stack = new ArrayList<>(args);
        while (stack.size() > 0) {
            Operand item = stack.remove(stack.size() - 1);
            if (item.Type() == OperandType.ARRAY) {
                List<Operand> array = item.ArrayValue();
                for (int i = array.size() - 1; i >= 0; i--)
                    stack.add(array.get(i));
            } else if (item.Type() == OperandType.JSON) {
                Operand i = item.ToArray(null);
                if (i.IsError())
                    return false;
                List<Operand> array = i.ArrayValue();
                for (int j = array.size() - 1; j >= 0; j--)
                    stack.add(array.get(j));
            } else {
                list.add(item);
            }
        }
        return true;
    }

    public static boolean FlattenToBigDecimalList(List<Operand> args, List<BigDecimal> list) {
        list.ensureCapacity(Math.max(list.size(), EstimateCount(args)));
        List<Operand> stack = new ArrayList<>(args);
        while (stack.size() > 0) {
            Operand item = stack.remove(stack.size() - 1);
            if (item.Type() == OperandType.ARRAY) {
                List<Operand> array = item.ArrayValue();
                for (int i = array.size() - 1; i >= 0; i--)
                    stack.add(array.get(i));
            } else if (item.Type() == OperandType.JSON) {
                Operand i = item.ToArray(null);
                if (i.IsError())
                    return false;
                List<Operand> array = i.ArrayValue();
                for (int j = array.size() - 1; j >= 0; j--)
                    stack.add(array.get(j));
            } else {
                if (item.Type() == OperandType.NUMBER) {
                    list.add(item.NumberValue());
                } else {
                    Operand converted = item.ToNumber(null);
                    if (converted.IsError())
                        return false;
                    list.add(converted.NumberValue());
                }
            }
        }
        return true;
    }

    public static boolean FlattenToNumberList(Operand args, List<BigDecimal> list) {
        if (args.IsError())
            return false;
        if (args.Type() == OperandType.ARRAY)
            return FlattenToBigDecimalList(args.ArrayValue(), list);
        if (args.Type() == OperandType.JSON) {
            Operand i = args.ToArray(null);
            if (i.IsError())
                return false;
            return FlattenToBigDecimalList(i.ArrayValue(), list);
        }
        if (args.Type() == OperandType.NUMBER) {
            list.add(args.NumberValue());
        } else {
            Operand converted = args.ToNumber(null);
            if (converted.IsError())
                return false;
            list.add(converted.NumberValue());
        }
        return true;
    }

    public static boolean FlattenToStringList(Operand args, List<String> list) {
        if (args.IsError()) {
            return false;
        }
        if (args.Type() == OperandType.ARRAY) {
            List<Operand> array = args.ArrayValue();
            list.ensureCapacity(Math.max(list.size(), array.size()));
            for (int i = 0; i < array.size(); i++) {
                Operand item = array.get(i);
                if (item.Type() == OperandType.ARRAY || item.Type() == OperandType.JSON) {
                    if (!FlattenToStringList(item, list))
                        return false;
                } else {
                    Operand converted = item.ToText(null);
                    if (converted.IsError()) {
                        return false;
                    }
                    list.add(converted.TextValue());
                }
            }
        } else if (args.Type() == OperandType.JSON) {
            Operand i = args.ToArray(null);
            if (i.IsError()) {
                return false;
            }
            return FlattenToStringList(i, list);
        } else {
            Operand converted = args.ToText(null);
            if (converted.IsError()) {
                return false;
            }
            list.add(converted.TextValue());
        }
        return true;
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
        if (CharUtil.Equals(ss, "<")) {
            return a.compareTo(b) < 0;
        } else if (CharUtil.Equals(ss, "<=")) {
            return a.compareTo(b) <= 0;
        } else if (CharUtil.Equals(ss, ">")) {
            return a.compareTo(b) > 0;
        } else if (CharUtil.Equals(ss, ">=")) {
            return a.compareTo(b) >= 0;
        } else if (CharUtil.Equals(ss, "=", "==", "===")) {
            return a.compareTo(b) == 0;
        }
        return a.compareTo(b) != 0;
    }

    public static BigDecimal GetGcd(List<BigDecimal> list) {
        if (list.size() == 0)
            return BigDecimal.ONE;

        BigDecimal g = list.get(0).setScale(0, RoundingMode.DOWN);
        for (int i = 1; i < list.size(); i++) {
            g = GetGcd(g, list.get(i).setScale(0, RoundingMode.DOWN));
            if (g.compareTo(BigDecimal.ONE) == 0)
                break;
        }
        return g;
    }

    public static BigDecimal GetGcd(BigDecimal a, BigDecimal b) {
        while (b.compareTo(BigDecimal.ZERO) != 0) {
            BigDecimal t = b;
            b = a.remainder(b);
            a = t;
        }
        return a;
    }

    public static BigDecimal GetLcm(List<BigDecimal> list) {
        if (list.size() == 0)
            return BigDecimal.ONE;

        BigDecimal a = BigDecimal.ZERO;
        boolean foundFirst = false;

        for (int i = 0; i < list.size(); i++) {
            BigDecimal val = list.get(i).setScale(0, RoundingMode.DOWN);
            if (val.compareTo(BigDecimal.ONE) <= 0)
                continue;

            if (!foundFirst) {
                a = val;
                foundFirst = true;
                continue;
            }

            BigDecimal b = val;
            BigDecimal g = b.compareTo(a) > 0 ? GetGcd(b, a) : GetGcd(a, b);
            a = a.divide(g, 0, RoundingMode.DOWN).multiply(b);
        }
        return foundFirst ? a : BigDecimal.ONE;
    }

    public static BigDecimal GetFactorial(BigDecimal a) {
        if (a.compareTo(BigDecimal.ZERO) <= 0) {
            return BigDecimal.ONE;
        }
        a = a.setScale(0, RoundingMode.DOWN);
        BigDecimal r = BigDecimal.ONE;
        for (BigDecimal i = a; i.compareTo(BigDecimal.ZERO) > 0; i = i.subtract(BigDecimal.ONE)) {
            r = r.multiply(i);
        }
        return r;
    }

    public static SimpleEntry<String, BigDecimal> ParseSumIfMatch(String s) {
        if (s.length() == 0) {
            return null;
        }
        char c = s.charAt(0);
        if (c == '>' || c == '＞') {
            if (s.length() > 1 && (s.charAt(1) == '=' || s.charAt(1) == '＝')) {
                BigDecimal d = TryParseDecimal(s.substring(2).trim());
                if (d != null) {
                    return new SimpleEntry<>(">=", d);
                }
            } else {
                BigDecimal d = TryParseDecimal(s.substring(1).trim());
                if (d != null) {
                    return new SimpleEntry<>(">", d);
                }
            }
        } else if (c == '<' || c == '＜') {
            if (s.length() > 1 && (s.charAt(1) == '=' || s.charAt(1) == '＝')) {
                BigDecimal d = TryParseDecimal(s.substring(2).trim());
                if (d != null) {
                    return new SimpleEntry<>("<=", d);
                }
            } else if (s.length() > 1 && (s.charAt(1) == '>' || s.charAt(1) == '＞')) {
                BigDecimal d = TryParseDecimal(s.substring(2).trim());
                if (d != null) {
                    return new SimpleEntry<>("!=", d);
                }
            } else {
                BigDecimal d = TryParseDecimal(s.substring(1).trim());
                if (d != null) {
                    return new SimpleEntry<>("<", d);
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
            BigDecimal d = TryParseDecimal(s.substring(index).trim());
            if (d != null) {
                return new SimpleEntry<>("=", d);
            }
        } else if (c == '!' || c == '！') {
            int index = 1;
            if (s.length() > 1 && (s.charAt(1) == '=' || s.charAt(1) == '＝')) {
                index = 2;
                if (s.length() > 2 && (s.charAt(2) == '=' || s.charAt(2) == '＝')) {
                    index = 3;
                }
            }
            BigDecimal d = TryParseDecimal(s.substring(index).trim());
            if (d != null) {
                return new SimpleEntry<>("!=", d);
            }
        }
        return null;
    }

    private static BigDecimal TryParseDecimal(String s) {
        try {
            return new BigDecimal(s);
        } catch (Exception e) {
            return null;
        }
    }

    public static Boolean TryParseBoolean(String textValue) {
        if (textValue == null)
            return null;
        int len = textValue.length();
        switch (len) {
            case 1: {
                char c = textValue.charAt(0);
                if (c == '1' || c == '是' || c == '有')
                    return true;
                if (c == '0' || c == '否' || c == '无')
                    return false;
                break;
            }
            case 2: {
                if (textValue.equalsIgnoreCase("no"))
                    return false;
                if (textValue.equals("不是"))
                    return false;
                if (textValue.equals("没有"))
                    return false;
                break;
            }
            case 3: {
                if (textValue.equalsIgnoreCase("yes"))
                    return true;
                break;
            }
            case 4: {
                if (textValue.equalsIgnoreCase("true"))
                    return true;
                break;
            }
            case 5: {
                if (textValue.equalsIgnoreCase("false"))
                    return false;
                break;
            }
        }
        return null;
    }

    public static BigDecimal QuickSelect(List<BigDecimal> list, int k, boolean largest) {
        if (list.size() == 1)
            return list.get(0);

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
            } else if ((descending && values.get(i).compareTo(num) > 0)
                    || (!descending && values.get(i).compareTo(num) < 0)) {
                rank++;
            }
        }
        return count > 0 ? rank : 0;
    }
}
