package toolgood.bench;

import com.sun.management.ThreadMXBean;
import java.lang.management.ManagementFactory;
import java.util.Arrays;

import toolgood.algorithm.AlgorithmEngineHelper;
import toolgood.algorithm.internals.functions.FunctionBase;

/**
 * Java Fast 版「解析」基准，对照 C# 的 PerfStr/PerfFast。
 * 口径：预热 10% 后跑 7 轮，输出 min ns/op 与每线程分配字节数（B/op）。
 */
public final class ParseBench {

    private static final ThreadMXBean TMX = (ThreadMXBean) ManagementFactory.getThreadMXBean();

    public static void main(String[] args) throws Exception {
        String s100 = "x".repeat(100);
        StringBuilder add100 = new StringBuilder("1");
        for (int i = 0; i < 99; i++) add100.append("+1");
        StringBuilder bracket = new StringBuilder();
        for (int i = 0; i < 200; i++) bracket.append('(');
        bracket.append('1');
        for (int i = 0; i < 200; i++) bracket.append(')');

        run("Str1",        200000, "\"abc\"");
        run("Str100",      200000, "\"" + s100 + "\"");
        run("StrEsc",      200000, "\"a\\tb\\nc\"");
        run("Str5Func",    200000, "CONCATENATE(\"a1\",\"b22\",\"c333\",\"d4444\",\"e55555\")");
        run("Param3",      200000, "a+b+c");
        run("Param10",     200000, "a1+b2+c3+d4+e5+f6+g7+h8+i9+j10");
        run("FuncChain10", 200000, "SUM(1)+ABS(2)+INT(3)+LEN(\"x\")+MAX(4,5)+MIN(6,7)+ROUND(8.5,0)+TRUNC(9.9)+SIGN(-1)+MOD(10,3)");
        run("Mixed50",     200000, "SUM(1,2,3)+ROUND(12.35*3+2.25,2)+IF(1>2,\"a\",\"b\")+LEFT(\"abcdef\",3)+DATE(2026,1,1)");
        run("Add10",       200000, "1+2+3+4+5+6+7+8+9+10");
        run("Add100",       20000, add100.toString());
        run("Bracket200",   20000, bracket.toString());
    }

    private static void run(String name, int iters, String exp) {
        int warm = Math.max(50, iters / 10);
        for (int i = 0; i < warm; i++) {
            parse(exp);
        }
        long[] ns = new long[7];
        long[] bytes = new long[7];
        long tid = Thread.currentThread().getId();
        for (int rep = 0; rep < 7; rep++) {
            parse(exp);
            long t0 = System.nanoTime();
            long a0 = TMX.getThreadAllocatedBytes(tid);
            for (int i = 0; i < iters; i++) {
                parse(exp);
            }
            long t1 = System.nanoTime();
            long a1 = TMX.getThreadAllocatedBytes(tid);
            ns[rep] = (t1 - t0) / iters;
            bytes[rep] = (a1 - a0) / iters;
        }
        Arrays.sort(ns);
        Arrays.sort(bytes);
        System.out.printf("%-12s min %6d ns/op (med %6d), %6d B/op%n", name, ns[0], ns[3], bytes[0]);
    }

    private static FunctionBase parse(String exp) {
        try {
            return AlgorithmEngineHelper.ParseFormula(exp);
        } catch (Exception e) {
            throw new IllegalStateException("parse failed: " + exp, e);
        }
    }

    private ParseBench() {
    }
}
