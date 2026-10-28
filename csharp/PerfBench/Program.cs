using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using ToolGood.Algorithm;
using ToolGood.Algorithm.Internals.Functions;
using ToolGood.Algorithm.Operands;

namespace PerfBench
{
    /// <summary>
    /// 性能修复前后对比基准（同一份代码分别跑在修复前/修复后的库上）。
    /// 输出：每项 中位数 ns/op + 分配 B/op。
    /// </summary>
    internal static class Program
    {
        private const string Json1Kb =
            "{\"id\":12345,\"name\":\"ToolGood Algorithm Perf\",\"tags\":[\"a\",\"b\",\"c\",\"dd\",\"eee\"]," +
            "\"active\":true,\"verified\":false,\"score\":98.5,\"count\":1024,\"ratio\":0.75," +
            "\"items\":[" +
            "{\"sku\":\"A001\",\"price\":10.5,\"qty\":3},{\"sku\":\"A002\",\"price\":20.25,\"qty\":1}," +
            "{\"sku\":\"A003\",\"price\":5,\"qty\":7},{\"sku\":\"A004\",\"price\":100,\"qty\":2}," +
            "{\"sku\":\"A005\",\"price\":33.33,\"qty\":5}],\"note\":\"hello world\",\"empty\":\"\",\"null\":null}";

        private static readonly MyDate SampleDate = new MyDate(new DateTime(2026, 1, 1, 12, 30, 45));

        private static readonly Operand Array100 = BuildArray100();
        private static readonly AlgorithmEngine Engine = new AlgorithmEngine();
        private static readonly FunctionBase ConcatFunc =
            Engine.Parse("CONCATENATE(1,2,3.5,TRUE,FALSE,\"x\",\"y\",10,DATE(2026,1,1),\"z\")");

        private static Operand BuildArray100()
        {
            var list = new List<Operand>(100);
            for (int i = 0; i < 20; i++) list.Add(Operand.Create("item-" + i));
            for (int i = 0; i < 20; i++) list.Add(Operand.Create(100000 + i));
            for (int i = 0; i < 20; i++) list.Add(Operand.Create(1.5m + i));
            for (int i = 0; i < 10; i++) list.Add(Operand.Create(i % 2 == 0));
            for (int i = 0; i < 10; i++) list.Add(Operand.Null);
            for (int i = 0; i < 10; i++) list.Add(Operand.Create(new DateTime(2026, 1, 1 + i)));
            for (int i = 0; i < 10; i++) list.Add(Operand.Create(new List<Operand> { Operand.Create(i), Operand.Create("n" + i) }));
            return Operand.Create(list);
        }

        private static void Main()
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

            Run("CreateJson_1KB(Lexer)", 2000, () => {
                var r = Operand.CreateJson(Json1Kb);
                if (r.IsError) throw new Exception("CreateJson failed");
            });

            Run("ArrayToJson_100", 5000, () => {
                var r = Array100.ToJson();
                if (r.IsError) throw new Exception("ToJson failed");
            });

            Run("StrToDate_Common", 20000, () => {
                var r = Operand.Create("2026-01-01 12:30:45").ToMyDate();
                if (r.IsError) throw new Exception("date failed");
            });

            Run("StrToDate_Fallback", 20000, () => {
                var r = Operand.Create("1/1/2026").ToMyDate();
                if (r.IsError) throw new Exception("date failed");
            });

            Run("MyDate_ToString", 50000, () => {
                if (SampleDate.ToString() == null) throw new Exception("toString failed");
            });

            Run("Eval_Concat10", 200000, () => {
                var r = ConcatFunc.TryEvaluate(Engine, "");
                if (string.IsNullOrEmpty(r)) throw new Exception("eval failed");
            });
        }

        private static void Run(string name, int iters, Action action)
        {
            // 预热（含 JIT）
            var warm = Math.Max(50, iters / 10);
            for (int i = 0; i < warm; i++) action();

            double[] nsPerOp = new double[7];
            double[] bPerOp = new double[7];
            for (int rep = 0; rep < 7; rep++) {
                action(); // 稳定状态
                long t0 = Stopwatch.GetTimestamp();
                long a0 = GC.GetAllocatedBytesForCurrentThread();
                for (int i = 0; i < iters; i++) action();
                long t1 = Stopwatch.GetTimestamp();
                long a1 = GC.GetAllocatedBytesForCurrentThread();
                nsPerOp[rep] = (t1 - t0) * 1_000_000_000.0 / Stopwatch.Frequency / iters;
                bPerOp[rep] = (a1 - a0) / (double)iters;
            }
            Array.Sort(nsPerOp);
            Array.Sort(bPerOp);
            // 噪声环境下 min 比 median 更稳健：取最优轮次代表真实吞吐
            Console.WriteLine($"{name}: min {nsPerOp[0]:F1} ns/op (med {nsPerOp[3]:F1}), {bPerOp[0]:F1} B/op");
        }
    }
}
