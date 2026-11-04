using System;
using PetaTest;

namespace ToolGood.Algorithm.Test.Operator
{
    [TestFixture]
    internal class OperatorTest
    {
        [Test]
        public void arithmetic_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("1+(3*2+2)/2", 0);
            Assert.AreEqual(5, t);

            t = engine.TryEvaluate("(8-3)*(3+2)", 0);
            Assert.AreEqual(25, t);

			t = engine.TryEvaluate("8/3*6", 0);
			Assert.AreEqual(16, t);

			var t2 = engine.TryEvaluate("8/3", 0.0);
			Assert.AreEqual(8/3.0, t2,6);

			t = engine.TryEvaluate("(8-3)*(3+2) % 7", 0);
            Assert.AreEqual(4, t);

            var c = engine.TryEvaluate("2+3", 0);
            Assert.AreEqual(5, c);
            c = engine.TryEvaluate("(2)+3", 0);
            Assert.AreEqual(5, c);
            c = engine.TryEvaluate("2+3*2+10/2*4", 0);
            Assert.AreEqual(28, c);

            c = engine.TryEvaluate("2.1e3 + 10", 0);
            Assert.AreEqual(2110, c);

            c = engine.TryEvaluate("2.1e+03 + 10", 0);
            Assert.AreEqual(2110, c);

            c = engine.TryEvaluate("2.1e+3 + 10", 0);
            Assert.AreEqual(2110, c);

            var d = engine.TryEvaluate("2.1e-3 + 10", 0.0);
            Assert.AreEqual(10.0021, d);
        }

        [Test]
        public void connect_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var s = engine.TryEvaluate("'aa'&'bb'", "");
            Assert.AreEqual("aabb", s);

            s = engine.TryEvaluate("'3'+2", "");
            Assert.AreEqual("5", s);
        }

        [Test]
        public void conditional_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t1 = engine.TryEvaluate("-7 < -2 ?1 : 2", 0);
            Assert.AreEqual(t1, 1);
            t1 = engine.TryEvaluate("-7 < -2 ?1 ： 2", 0);
            Assert.AreEqual(t1, 1);

			t1 = engine.TryEvaluate("-7 < -2 ?1 ： (7>1?3:2)", 0);
			Assert.AreEqual(t1, 1);

			t1 = engine.TryEvaluate("-7 < -2 ?1 ：2", 0);
            Assert.AreEqual(t1, 1);
            t1 = engine.TryEvaluate("-7 < -2 ？ 1 : 2", 0);
            Assert.AreEqual(t1, 1);
            t1 = engine.TryEvaluate("-7 < -2 ？1 : 2", 0);
            Assert.AreEqual(t1, 1);

            t1 = engine.TryEvaluate("-7 < -2 ？1 ： 2", 0);
            Assert.AreEqual(t1, 1);

            t1 = engine.TryEvaluate("(!(-7 < -2))？1：2", 0);
            Assert.AreEqual(t1, 2);
            t1 = engine.TryEvaluate("1>2？1：2", 0);
            Assert.AreEqual(t1, 2);

            t1 = engine.TryEvaluate("1！=2？1：2", 0);
            Assert.AreEqual(t1, 1);
        }

        [Test]
        public void percentage_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("100%", 0.0);
            Assert.AreEqual(1.0, t);

            t = engine.TryEvaluate("50%", 0.0);
            Assert.AreEqual(0.5, t);

            t = engine.TryEvaluate("200%", 0.0);
            Assert.AreEqual(2.0, t);

            t = engine.TryEvaluate("100*50%", 0.0);
            Assert.AreEqual(50.0, t);

            t = engine.TryEvaluate("100+50%", 0.0);
            Assert.AreEqual(100.5, t);
        }

        [Test]
        public void null_operation_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var tbb2 = engine.TryEvaluate("'111'*null", 0);
            Assert.AreEqual(tbb2, 0);
        }

        [Test]
        public void overflow_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("79228162514264337593543950335 + 1", 0.0);
            Assert.IsTrue(engine.LastError != null);

            engine = new AlgorithmEngine();
            engine.TryEvaluate("-79228162514264337593543950335 - 1", 0.0);
            Assert.IsTrue(engine.LastError != null);

            engine = new AlgorithmEngine();
            engine.TryEvaluate("79228162514264337593543950335 * 2", 0.0);
            Assert.IsTrue(engine.LastError != null);

            engine = new AlgorithmEngine();
            engine.TryEvaluate("79228162514264337593543950335 / 0.1", 0.0);
            Assert.IsTrue(engine.LastError != null);
        }

        /// <summary>
        /// 往返一致性校验：Parse(exp) -> ToString 还原 -> 重新 Parse 求值，
        /// 要求还原前后语义完全一致(结果值 + 是否错误)。
        /// 用于防止 ToString 丢失子表达式括号导致的语义漂移。
        /// </summary>
        private static void AssertRoundTrip(string exp)
        {
            var engine1 = new AlgorithmEngine();
            var function = engine1.Parse(exp);
            var text = function.ToString();
            var result1 = engine1.Evaluate(function);

            var engine2 = new AlgorithmEngine();
            Operand result2;
            try {
                result2 = engine2.Evaluate(engine2.Parse(text));
            } catch (Exception ex) {
                throw new Exception($"往返测试失败: [{exp}] 还原为 [{text}] 后重新求值抛出 {ex.GetType().Name}: {ex.Message}");
            }

            if(result1.IsError != result2.IsError || result1.ToString() != result2.ToString()) {
                throw new Exception($"往返测试失败: [{exp}] 还原为 [{text}]，原结果=[{result1}](IsError={result1.IsError})，往返结果=[{result2}](IsError={result2.IsError})");
            }
        }

        [Test]
        public void tostring_roundtrip_test()
        {
            // Sub 减法非结合：右操作数为同优先级或更低优先级表达式时必须保留括号
            AssertRoundTrip("1 - (2 - 3)");
            AssertRoundTrip("10 - (2 + 3)");
            AssertRoundTrip("2 - (3 + 4)");
            AssertRoundTrip("(1 + 2) - (3 - 4)");
            AssertRoundTrip("1 - (2 - (3 - 4))");

            // Add / Connect：与连接运算符 & 及比较运算符混用
            AssertRoundTrip("1 + (2 & 3)");
            AssertRoundTrip("10 - (2 & 3)");
            AssertRoundTrip("(1 & 2) + 3");
            AssertRoundTrip("'a' & (1 - 2)");
            AssertRoundTrip("1 & (2 - 3)");
            AssertRoundTrip("(1 > 0) + 1");
            AssertRoundTrip("(1 > 0) & 'a'");

            // AND / OR：与低优先级逻辑运算符混用
            AssertRoundTrip("false() && (true() || true())");
            AssertRoundTrip("true() || (false() && true())");

            // 回归：乘除模与算术混合(修复前已正确，防止修复引入退化)
            AssertRoundTrip("(2 + 3) * 4");
            AssertRoundTrip("8 / (2 * 2)");
            AssertRoundTrip("2 * (3 * 4)");
            AssertRoundTrip("1 + 2 * 3");
            AssertRoundTrip("(1 + 2) * 3");
            AssertRoundTrip("(1 - 2) * (3 - 4)");

            // 回归：字符串连接链
            AssertRoundTrip("'a' & 'b' & 'c'");
            AssertRoundTrip("'a' & ('b' & 'c')");

            // 回归：IF 嵌套
            AssertRoundTrip("true() && (1 > 0 ? true() : false())");
        }

        [Test]
        public void minus_space_number_test()
        {
            // 回归：负号与数字之间允许空白。Antlr 版按 GetText() 取 token 文本不含空白，
            // 手写前端曾把空白一起切进字面量（"- 1"）导致 ArgumentException，两套前端行为必须一致。
            AlgorithmEngine engine = new AlgorithmEngine();
            Assert.AreEqual(-1m, engine.TryEvaluate("- 1", 0m));
            Assert.AreEqual(-1.5m, engine.TryEvaluate("- 1.5", 0m));
            Assert.AreEqual(-1m, engine.TryEvaluate("-\t1", 0m));
            Assert.AreEqual(-1m, engine.TryEvaluate("-  1", 0m));
            Assert.AreEqual(1m, engine.TryEvaluate("- 1 + 2", 0m));
            Assert.AreEqual(3m, engine.TryEvaluate("1 - - 2", 0m));
            Assert.AreEqual(-2m, engine.TryEvaluate("2*- 1", 0m));
            // 字面量解析路径不变：无空白的紧贴写法结果一致
            Assert.AreEqual(-1m, engine.TryEvaluate("-1", 0m));
        }
    }
}
