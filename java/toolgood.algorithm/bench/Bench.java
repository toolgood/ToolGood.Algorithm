package toolgood.bench;

import com.sun.management.ThreadMXBean;
import java.lang.management.ManagementFactory;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.AlgorithmEngineEx;
import toolgood.algorithm.Operand;

/**
 * Java 侧性能基准，对照 C# 的 PerfBench。
 * 口径：预热 10% 后跑 7 轮，输出 min ns/op 与每线程分配字节数（B/op）。
 * B/op 是本类改动的核心指标，也是唯一不受机器噪声影响的量。
 * 注意：HotSpot 的逃逸分析会把不逃逸的 new BigDecimal 直接消除，
 * 所以「纯整数数组的 SUM」改前就没有额外分配，测不出差异；只有结果 scale 不一、
 * 走 BigInteger 路径的混合数组，分配才真正落到堆上。
 */
public final class Bench {

    private static final AlgorithmEngine ENGINE = new AlgorithmEngine();
    private static final AlgorithmEngineEx SUM_ENGINE = new AlgorithmEngineEx();
    // Java 版 Function_PARAMETER 不走 engine.GetParameter，而是由调用方传 BiFunction；
    // 无捕获 lambda 是单例，不产生分配
    private static final BiFunction<AlgorithmEngine, String, Operand> PARAM =
            (e, k) -> e.GetParameter(k);
    private static final ThreadMXBean TMX = (ThreadMXBean) ManagementFactory.getThreadMXBean();

    public static void main(String[] args) {
        var addChain = ENGINE.Parse("1+2+3+4+5+6+7+8+9+10");
        var strAdd = ENGINE.Parse("\"1\"+2+3+4+5");
        var decChain = ENGINE.Parse("1.5+2.25+3.125+4.5+5.75+6.5+7.25+8.5+9.125+10.5");
        var mulDiv = ENGINE.Parse("1.5*2.25*3.5*4.125/1.75");
        var round = ENGINE.Parse("ROUND(12.3456*3+2.25,2)");
        var sum = SUM_ENGINE.Parse("SUM(a)");

        Operand arrInt = buildIntArray100();
        Operand arrMix = buildMixArray100();

        run("Eval_AddChain10", 200000, () -> mustOk(addChain.Evaluate(ENGINE, PARAM)));
        run("Eval_StrAdd5", 200000, () -> mustOk(strAdd.Evaluate(ENGINE, PARAM)));
        run("Eval_DecChain10", 200000, () -> mustOk(decChain.Evaluate(ENGINE, PARAM)));
        run("Eval_MulDiv4", 200000, () -> mustOk(mulDiv.Evaluate(ENGINE, PARAM)));
        run("Eval_RoundExpr", 200000, () -> mustOk(round.Evaluate(ENGINE, PARAM)));

        SUM_ENGINE.AddParameter("a", arrInt);
        run("Eval_SumArray100", 50000, () -> mustOk(sum.Evaluate(SUM_ENGINE, PARAM)));

        SUM_ENGINE.AddParameter("a", arrMix);
        run("Eval_SumArrayMix100", 50000, () -> mustOk(sum.Evaluate(SUM_ENGINE, PARAM)));
    }

    private static void mustOk(Operand o) {
        if (o.IsError()) {
            throw new IllegalStateException("eval error: " + o.ErrorMsg());
        }
    }

    private static Operand buildIntArray100() {
        List<Operand> list = new ArrayList<>(100);
        for (int i = 0; i < 100; i++) {
            list.add(Operand.Create(i));
        }
        return Operand.Create(list);
    }

    private static Operand buildMixArray100() {
        List<Operand> list = new ArrayList<>(100);
        for (int i = 0; i < 50; i++) {
            list.add(Operand.Create(i));
        }
        for (int i = 0; i < 50; i++) {
            list.add(Operand.Create(new java.math.BigDecimal("0.25").add(java.math.BigDecimal.valueOf(i))));
        }
        return Operand.Create(list);
    }

    private static void run(String name, int iters, Runnable action) {
        int warm = Math.max(50, iters / 10);
        for (int i = 0; i < warm; i++) {
            action.run();
        }
        long[] ns = new long[7];
        long[] bytes = new long[7];
        long tid = Thread.currentThread().getId();
        for (int rep = 0; rep < 7; rep++) {
            action.run();
            long t0 = System.nanoTime();
            long a0 = TMX.getThreadAllocatedBytes(tid);
            for (int i = 0; i < iters; i++) {
                action.run();
            }
            long t1 = System.nanoTime();
            long a1 = TMX.getThreadAllocatedBytes(tid);
            ns[rep] = (t1 - t0) / iters;
            bytes[rep] = (a1 - a0) / iters;
        }
        Arrays.sort(ns);
        Arrays.sort(bytes);
        System.out.printf("%s: min %d ns/op (med %d), %d B/op%n", name, ns[0], ns[3], bytes[0]);
    }

    private Bench() {
    }
}
