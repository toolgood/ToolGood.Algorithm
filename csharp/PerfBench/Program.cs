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
        private static readonly FunctionBase ConcatTextFunc =
            Engine.Parse("CONCATENATE(\"a\",\"b\",\"c\",\"d\",\"e\",\"f\",\"g\",\"h\",\"i\",\"j\")");
        private static readonly FunctionBase AddChainFunc =
            Engine.Parse("1+2+3+4+5+6+7+8+9+10");
        private static readonly FunctionBase StrAddFunc =
            Engine.Parse("\"1\"+2+3+4+5");
        private static readonly BenchEngine SumEngine = new BenchEngine();
        private static readonly FunctionBase SumFunc = SumEngine.Parse("SUM(a)");
        private static readonly FunctionBase AvgFunc = SumEngine.Parse("ROUND(SUM(a)/COUNT(a),2)");
        private static readonly Operand ArrayInt100 = BuildIntArray100();
        private static readonly Operand ArrayDec100 = BuildDecArray100();
        private static readonly Operand ArrayMix100 = BuildMixArray100();

        // 非整数场景：小数/混合类型/日期，用于检验「整数类型被削弱」时对常规公式的影响
        private static readonly FunctionBase DecChainFunc =
            Engine.Parse("1.5+2.25+3.125+4.5+5.75+6.5+7.25+8.5+9.125+10.5");
        private static readonly FunctionBase MixChainFunc =
            Engine.Parse("1+2.5+3+4.75+5+6.5+7+8.25+9+10.5");
        private static readonly FunctionBase MulDivFunc =
            Engine.Parse("1.5*2.25*3.5*4.125/1.75");
        private static readonly FunctionBase IntDivFunc =
            Engine.Parse("1000/7/3");
        private static readonly FunctionBase RoundFunc =
            Engine.Parse("ROUND(12.3456*3+2.25,2)");
        private static readonly FunctionBase DatedifFunc =
            Engine.Parse("DATEDIF(DATE(2026,1,1),DATE(2026,12,31),\"D\")");

        // 解析（词法+Parsing）基准用表达式
        private const string ParseSimpleExp = "1+2+3+4+5+6+7+8+9+10";
        private const string ParseFuncExp = "CONCATENATE(\"a\",\"b\",\"c\",\"d\",\"e\",\"f\",\"g\",\"h\",\"i\",\"j\")";
        private const string ParseNestedExp = "ROUND(SUM(1,2,3,4,5)*2/3+IF(1>0,MAX(10,20),MIN(5,6)),2)";
        private const string ParseLongExp =
            "IF(AND(1<2,3>2,OR(4=4,5=6)),CONCATENATE(SUM(1,2,3),\"-\",ROUND(3.14159*2,2),\"-\",DATE(2026,1,1))," +
            "IF(NOT(FALSE),LEFT(\"abcdefgh\",3),RIGHT(\"abcdefgh\",3)))";
        private const string ParseUnitExp = "1.5M+2KM+300mm+0.5L+1.25KG+10m2";
        private static FunctionBase _parseSink;

        private sealed class BenchEngine : AlgorithmEngine
        {
            public Operand A;
            public override Operand GetParameter(string parameter)
            {
                return parameter == "a" ? A : base.GetParameter(parameter);
            }
        }

        private static Operand BuildIntArray100()
        {
            var list = new List<Operand>(100);
            for (int i = 0; i < 100; i++) list.Add(Operand.Create(i));
            return Operand.Create(list);
        }

        private static Operand BuildDecArray100()
        {
            var list = new List<Operand>(100);
            for (int i = 0; i < 100; i++) list.Add(Operand.Create(0.5m + i));
            return Operand.Create(list);
        }

        private static Operand BuildMixArray100()
        {
            var list = new List<Operand>(100);
            for (int i = 0; i < 50; i++) list.Add(Operand.Create(i));
            for (int i = 0; i < 50; i++) list.Add(Operand.Create(0.25m + i));
            return Operand.Create(list);
        }

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

            // ---- 解析（Lexer + Parser）基准：每次都是全新解析，不走 UseParseCache ----
            Run("Parse_Simple", 5000, () => {
                _parseSink = Engine.Parse(ParseSimpleExp);
                if (_parseSink == null) throw new Exception("parse failed");
            });

            Run("Parse_Func10", 5000, () => {
                _parseSink = Engine.Parse(ParseFuncExp);
                if (_parseSink == null) throw new Exception("parse failed");
            });

            Run("Parse_Nested", 5000, () => {
                _parseSink = Engine.Parse(ParseNestedExp);
                if (_parseSink == null) throw new Exception("parse failed");
            });

            Run("Parse_Long", 5000, () => {
                _parseSink = Engine.Parse(ParseLongExp);
                if (_parseSink == null) throw new Exception("parse failed");
            });

            Run("Parse_UnitMix", 5000, () => {
                _parseSink = Engine.Parse(ParseUnitExp);
                if (_parseSink == null) throw new Exception("parse failed");
            });

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

            Run("Eval_Concat10Text", 200000, () => {
                var r = ConcatTextFunc.TryEvaluate(Engine, "");
                if (string.IsNullOrEmpty(r)) throw new Exception("eval failed");
            });

            Run("Eval_AddChain10", 200000, () => {
                var r = AddChainFunc.TryEvaluate(Engine, 0m);
                if (r != 55m) throw new Exception("eval failed: " + r);
            });

            Run("Eval_StrAdd5", 200000, () => {
                var r = StrAddFunc.TryEvaluate(Engine, 0m);
                if (r != 15m) throw new Exception("eval failed: " + r);
            });

            Run("Eval_SumArray100", 50000, () => {
                SumEngine.A = ArrayInt100;
                var r = SumFunc.TryEvaluate(SumEngine, 0m);
                if (r != 4950m) throw new Exception("eval failed: " + r);
            });

            // ---- 非整数场景 ----
            Run("Eval_DecChain10", 200000, () => {
                var r = DecChainFunc.Evaluate(Engine);
                if (r.IsError) throw new Exception(r.ErrorMsg);
            });

            Run("Eval_MixChain10", 200000, () => {
                var r = MixChainFunc.Evaluate(Engine);
                if (r.IsError) throw new Exception(r.ErrorMsg);
            });

            Run("Eval_MulDiv4", 200000, () => {
                var r = MulDivFunc.Evaluate(Engine);
                if (r.IsError) throw new Exception(r.ErrorMsg);
            });

            Run("Eval_IntDiv3", 200000, () => {
                var r = IntDivFunc.Evaluate(Engine);
                if (r.IsError) throw new Exception(r.ErrorMsg);
            });

            Run("Eval_RoundExpr", 200000, () => {
                var r = RoundFunc.Evaluate(Engine);
                if (r.IsError) throw new Exception(r.ErrorMsg);
            });

            Run("Eval_Datedif", 100000, () => {
                var r = DatedifFunc.Evaluate(Engine);
                if (r.IsError) throw new Exception(r.ErrorMsg);
            });

            Run("Eval_SumArrayDec100", 50000, () => {
                SumEngine.A = ArrayDec100;
                var r = SumFunc.TryEvaluate(SumEngine, 0m);
                if (r <= 0m) throw new Exception("eval failed: " + r);
            });

            Run("Eval_SumArrayMix100", 50000, () => {
                SumEngine.A = ArrayMix100;
                var r = SumFunc.TryEvaluate(SumEngine, 0m);
                if (r <= 0m) throw new Exception("eval failed: " + r);
            });

            Run("Eval_AvgArrayMix100", 50000, () => {
                SumEngine.A = ArrayMix100;
                var r = AvgFunc.TryEvaluate(SumEngine, 0m);
                if (r <= 0m) throw new Exception("eval failed: " + r);
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
