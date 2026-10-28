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

    private static <T> void EnsureCapacity(List<T> list, int min) {
        if (list instanceof ArrayList) {
            ArrayList<T> arrayList = (ArrayList<T>) list;
            int need = arrayList.size() + min;
            if (need > arrayList.size()) arrayList.ensureCapacity(need);
        }
    }

    public static boolean FlattenToList(List<Operand> args, List<Operand> list) {
        return FlattenToList(args, list, -1);
    }

    /**
     * 展平参数列表为 Operand 列表。countHint 为调用方已知的元素总数
     * （如单数组参数的 ArrayValue().size()），传入后可跳过 EstimateCount 扫描；传 -1 时自行估算。
     */
    public static boolean FlattenToList(List<Operand> args, List<Operand> list, int countHint) {
        EnsureCapacity(list, countHint >= 0 ? countHint : EstimateCount(args));
        // 快速路径 1：遇数组/Json 之前的顶层标量直接入 list，不分配展开队列
        int i = 0;
        for (; i < args.size(); i++) {
            Operand item = args.get(i);
            if (item.IsArray() || item.IsJson()) break;
            list.add(item);
        }
        if (i >= args.size()) return true;
        // 快速路径 2：首个嵌套项恰为最后一个参数时，直接展开该数组的标量元素，仍不分配队列
        if (i == args.size() - 1 && args.get(i).IsArray()) {
            List<Operand> array = args.get(i).ArrayValue();
            int j = 0;
            for (; j < array.size(); j++) {
                Operand el = array.get(j);
                if (el.IsArray() || el.IsJson()) break;
                list.add(el);
            }
            if (j >= array.size()) return true;
            return FlattenQueue(array, j, list);
        }
        return FlattenQueue(args, i, list);
    }

    /**
     * 单操作数展平成 Operand 列表。与「把该操作数作为单元素参数列表展平」等价，
     * 供 COUNT 等需要逐项保留 Operand 的函数免去参数中转列表。
     */
    public static boolean FlattenToList(Operand args, List<Operand> list) {
        if (args.IsError()) return false;
        if (args.IsArray()) {
            List<Operand> array = args.ArrayValue();
            return FlattenToList(array, list, array.size());
        }
        if (args.IsJson()) {
            Operand i = args.ToArray(null);
            if (i.IsError()) return false;
            return FlattenToList(i.ArrayValue(), list);
        }
        list.add(args);
        return true;
    }

    public static boolean FlattenToBigDecimalList(List<Operand> args, List<BigDecimal> list) {
        return FlattenToBigDecimalList(args, list, -1);
    }

    /**
     * 展平参数列表为 BigDecimal 列表。countHint 见 {@link #FlattenToList(List, List, int)}。
     */
    public static boolean FlattenToBigDecimalList(List<Operand> args, List<BigDecimal> list, int countHint) {
        EnsureCapacity(list, countHint >= 0 ? countHint : EstimateCount(args));
        // 快速路径 1：顶层标量直接转换入 list，SUMIF/COUNTIF/AVERAGEIF 热点场景零队列分配
        int i = 0;
        for (; i < args.size(); i++) {
            Operand item = args.get(i);
            if (item.IsArray() || item.IsJson()) break;
            if (item.IsNumber()) {
                list.add(item.NumberValue());
            } else {
                Operand converted = item.ToNumber(null);
                if (converted.IsError()) return false;
                list.add(converted.NumberValue());
            }
        }
        if (i >= args.size()) return true;
        // 快速路径 2：首个嵌套项恰为最后一个参数，直接展开该数组的标量元素
        if (i == args.size() - 1 && args.get(i).IsArray()) {
            List<Operand> array = args.get(i).ArrayValue();
            int j = 0;
            for (; j < array.size(); j++) {
                Operand el = array.get(j);
                if (el.IsArray() || el.IsJson()) break;
                if (el.IsNumber()) {
                    list.add(el.NumberValue());
                } else {
                    Operand converted = el.ToNumber(null);
                    if (converted.IsError()) return false;
                    list.add(converted.NumberValue());
                }
            }
            if (j >= array.size()) return true;
            return FlattenQueueToNumber(array, j, list);
        }
        return FlattenQueueToNumber(args, i, list);
    }

    /**
     * 队列 BFS 展开（FlattenToList 的通用路径）。
     * 用 ArrayList + head 指针实现 FIFO，展开次序与按全部参数建队后逐项出队的实现完全一致。
     */
    private static boolean FlattenQueue(List<Operand> source, int from, List<Operand> list) {
        List<Operand> queue = new ArrayList<>(source.size() - from + 8);
        for (int i = from; i < source.size(); i++) queue.add(source.get(i));
        for (int head = 0; head < queue.size(); head++) {
            Operand item = queue.get(head);
            if (item.IsArray()) {
                List<Operand> array = item.ArrayValue();
                for (int j = 0; j < array.size(); j++) queue.add(array.get(j));
            } else if (item.IsJson()) {
                Operand arr = item.ToArray(null);
                if (arr.IsError()) return false;
                List<Operand> array = arr.ArrayValue();
                for (int j = 0; j < array.size(); j++) queue.add(array.get(j));
            } else {
                list.add(item);
            }
        }
        return true;
    }

    /**
     * 队列 BFS 展开（FlattenToBigDecimalList 的通用路径）。见 {@link #FlattenQueue}。
     */
    private static boolean FlattenQueueToNumber(List<Operand> source, int from, List<BigDecimal> list) {
        List<Operand> queue = new ArrayList<>(source.size() - from + 8);
        for (int i = from; i < source.size(); i++) queue.add(source.get(i));
        for (int head = 0; head < queue.size(); head++) {
            Operand item = queue.get(head);
            if (item.IsArray()) {
                List<Operand> array = item.ArrayValue();
                for (int j = 0; j < array.size(); j++) queue.add(array.get(j));
            } else if (item.IsJson()) {
                Operand arr = item.ToArray(null);
                if (arr.IsError()) return false;
                List<Operand> array = arr.ArrayValue();
                for (int j = 0; j < array.size(); j++) queue.add(array.get(j));
            } else if (item.IsNumber()) {
                list.add(item.NumberValue());
            } else {
                Operand converted = item.ToNumber(null);
                if (converted.IsError()) return false;
                list.add(converted.NumberValue());
            }
        }
        return true;
    }

    public static boolean FlattenToNumberList(Operand args, List<BigDecimal> list) {
        if (args.IsError())
            return false;
        if (args.Type() == OperandType.ARRAY) {
            List<Operand> array = args.ArrayValue();
            return FlattenToBigDecimalList(array, list, array.size());
        }
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

    public static int GetCountIf(List<BigDecimal> dbs, CompareOperator op, BigDecimal d) {
        // 比较分支提升到循环外：行内只剩 BigDecimal.compareTo，不再做 CharUtil.Equals 逐字符比较
        switch (op) {
            case Lt: {
                int count = 0;
                for (int i = 0; i < dbs.size(); i++) { if (dbs.get(i).compareTo(d) < 0) count++; }
                return count;
            }
            case Le: {
                int count = 0;
                for (int i = 0; i < dbs.size(); i++) { if (dbs.get(i).compareTo(d) <= 0) count++; }
                return count;
            }
            case Gt: {
                int count = 0;
                for (int i = 0; i < dbs.size(); i++) { if (dbs.get(i).compareTo(d) > 0) count++; }
                return count;
            }
            case Ge: {
                int count = 0;
                for (int i = 0; i < dbs.size(); i++) { if (dbs.get(i).compareTo(d) >= 0) count++; }
                return count;
            }
            case Eq: {
                int count = 0;
                for (int i = 0; i < dbs.size(); i++) { if (dbs.get(i).compareTo(d) == 0) count++; }
                return count;
            }
            default: {
                int count = 0;
                for (int i = 0; i < dbs.size(); i++) { if (dbs.get(i).compareTo(d) != 0) count++; }
                return count;
            }
        }
    }

    public static BigDecimal GetSumIf(List<BigDecimal> dbs, BigDecimal d, List<BigDecimal> sumdbs) {
        BigDecimal sum = BigDecimal.ZERO;
        int n = Math.min(dbs.size(), sumdbs.size());
        for (int i = 0; i < n; i++) {
            BigDecimal item = dbs.get(i);
            if (item.compareTo(d) == 0) {
                sum = sum.add(sumdbs.get(i));
            }
        }
        return sum;
    }

    public static BigDecimal GetSumIf(List<BigDecimal> dbs, CompareOperator op, BigDecimal d, List<BigDecimal> sumdbs) {
        int n = Math.min(dbs.size(), sumdbs.size());
        switch (op) {
            case Lt: {
                BigDecimal sum = BigDecimal.ZERO;
                for (int i = 0; i < n; i++) { if (dbs.get(i).compareTo(d) < 0) sum = sum.add(sumdbs.get(i)); }
                return sum;
            }
            case Le: {
                BigDecimal sum = BigDecimal.ZERO;
                for (int i = 0; i < n; i++) { if (dbs.get(i).compareTo(d) <= 0) sum = sum.add(sumdbs.get(i)); }
                return sum;
            }
            case Gt: {
                BigDecimal sum = BigDecimal.ZERO;
                for (int i = 0; i < n; i++) { if (dbs.get(i).compareTo(d) > 0) sum = sum.add(sumdbs.get(i)); }
                return sum;
            }
            case Ge: {
                BigDecimal sum = BigDecimal.ZERO;
                for (int i = 0; i < n; i++) { if (dbs.get(i).compareTo(d) >= 0) sum = sum.add(sumdbs.get(i)); }
                return sum;
            }
            case Eq: {
                BigDecimal sum = BigDecimal.ZERO;
                for (int i = 0; i < n; i++) { if (dbs.get(i).compareTo(d) == 0) sum = sum.add(sumdbs.get(i)); }
                return sum;
            }
            default: {
                BigDecimal sum = BigDecimal.ZERO;
                for (int i = 0; i < n; i++) { if (dbs.get(i).compareTo(d) != 0) sum = sum.add(sumdbs.get(i)); }
                return sum;
            }
        }
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

    public static SimpleEntry<CompareOperator, BigDecimal> ParseSumIfMatch(String s) {
        if (s.length() == 0) {
            return null;
        }
        char c = s.charAt(0);
        if (c == '>' || c == '＞') {
            if (s.length() > 1 && (s.charAt(1) == '=' || s.charAt(1) == '＝')) {
                BigDecimal d = TryParseDecimal(s.substring(2).trim());
                if (d != null) {
                    return new SimpleEntry<>(CompareOperator.Ge, d);
                }
            } else {
                BigDecimal d = TryParseDecimal(s.substring(1).trim());
                if (d != null) {
                    return new SimpleEntry<>(CompareOperator.Gt, d);
                }
            }
        } else if (c == '<' || c == '＜') {
            if (s.length() > 1 && (s.charAt(1) == '=' || s.charAt(1) == '＝')) {
                BigDecimal d = TryParseDecimal(s.substring(2).trim());
                if (d != null) {
                    return new SimpleEntry<>(CompareOperator.Le, d);
                }
            } else if (s.length() > 1 && (s.charAt(1) == '>' || s.charAt(1) == '＞')) {
                BigDecimal d = TryParseDecimal(s.substring(2).trim());
                if (d != null) {
                    return new SimpleEntry<>(CompareOperator.Ne, d);
                }
            } else {
                BigDecimal d = TryParseDecimal(s.substring(1).trim());
                if (d != null) {
                    return new SimpleEntry<>(CompareOperator.Lt, d);
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
                return new SimpleEntry<>(CompareOperator.Eq, d);
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
                return new SimpleEntry<>(CompareOperator.Ne, d);
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
        int left = 0, right = list.size() - 1;
        while (left < right) {
            int pivotIndex = SelectPivot(list, left, right);
            pivotIndex = Partition(list, left, right, pivotIndex);
            if (targetIndex == pivotIndex) {
                return list.get(targetIndex);
            } else if (targetIndex < pivotIndex) {
                right = pivotIndex - 1;
            } else {
                left = pivotIndex + 1;
            }
        }
        return list.get(left);
    }

    private static int SelectPivot(List<BigDecimal> list, int left, int right) {
        int mid = left + (right - left) / 2;
        BigDecimal a = list.get(left), b = list.get(mid), c = list.get(right);
        if (a.compareTo(b) < 0) {
            if (b.compareTo(c) < 0) return mid;
            if (a.compareTo(c) < 0) return right;
            return left;
        }
        if (a.compareTo(c) < 0) return left;
        if (b.compareTo(c) < 0) return right;
        return mid;
    }

    private static int Partition(List<BigDecimal> list, int left, int right, int pivotIndex) {
        BigDecimal pivot = list.get(pivotIndex);
        list.set(pivotIndex, list.get(right));
        list.set(right, pivot);
        int storeIndex = left;
        for (int i = left; i < right; i++) {
            if (list.get(i).compareTo(pivot) < 0) {
                Swap(list, storeIndex, i);
                storeIndex++;
            }
        }
        list.set(right, list.get(storeIndex));
        list.set(storeIndex, pivot);
        return storeIndex;
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
