using PetaTest;

namespace ToolGood.Algorithm.Test
{
    [TestFixture]
    internal class StrictModeTest
    {
        #region 二元 && 运算符

        [Test]
        public void and_strict_should_error_when_right_is_error()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseStrictMode = true;
            var t = engine.TryEvaluate("(1>2) && ERROR('test')", true);
            // 严格模式下，即使左边为 false，右边错误也会传播
            Assert.AreEqual(true, t);
            Assert.IsNotNull(engine.LastError);
        }

        [Test]
        public void and_strict_all_valid()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseStrictMode = true;
            var t = engine.TryEvaluate("2>1 && 3>2", false);
            Assert.AreEqual(true, t);
            Assert.IsNull(engine.LastError);
        }

        [Test]
        public void and_not_strict_short_circuit()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseStrictMode = false;
            // 非严格模式：1>2 为 false，短路不执行右边
            var t = engine.TryEvaluate("(1>2) && ERROR('test')", true);
            Assert.AreEqual(false, t);
            Assert.IsNull(engine.LastError);
        }

        [Test]
        public void and_not_strict_error_on_left()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseStrictMode = false;
            // 非严格模式：左边错误无法短路，应报错
            var t = engine.TryEvaluate("ERROR('left') && (2>1)", true);
            Assert.AreEqual(true, t);
            Assert.IsNotNull(engine.LastError);
        }

        [Test]
        public void and_not_strict_continue_when_left_true()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseStrictMode = false;
            // 非严格模式：左边为 true，继续执行右边
            var t = engine.TryEvaluate("(2>1) && ERROR('right')", true);
            Assert.AreEqual(true, t);
            Assert.IsNotNull(engine.LastError);
        }

        [Test]
        public void and_not_strict_all_valid()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseStrictMode = false;
            var t = engine.TryEvaluate("2>1 && 3>2", false);
            Assert.AreEqual(true, t);
            Assert.IsNull(engine.LastError);
        }

        #endregion

        #region 二元 || 运算符

        [Test]
        public void or_strict_should_error_when_right_is_error()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseStrictMode = true;
            // 严格模式下，即使左边为 true，右边错误也会传播
            var t = engine.TryEvaluate("(2>1) || ERROR('test')", false);
            Assert.AreEqual(false, t);
            Assert.IsNotNull(engine.LastError);
        }

        [Test]
        public void or_strict_all_valid()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseStrictMode = true;
            var t = engine.TryEvaluate("1>2 || 3>2", false);
            Assert.AreEqual(true, t);
            Assert.IsNull(engine.LastError);
        }

        [Test]
        public void or_not_strict_short_circuit()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseStrictMode = false;
            // 非严格模式：2>1 为 true，短路不执行右边
            var t = engine.TryEvaluate("(2>1) || ERROR('test')", false);
            Assert.AreEqual(true, t);
            Assert.IsNull(engine.LastError);
        }

        [Test]
        public void or_not_strict_error_on_left()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseStrictMode = false;
            // 非严格模式：左边错误无法短路，应报错
            var t = engine.TryEvaluate("ERROR('left') || (2>1)", false);
            Assert.AreEqual(false, t);
            Assert.IsNotNull(engine.LastError);
        }

        [Test]
        public void or_not_strict_continue_when_left_false()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseStrictMode = false;
            // 非严格模式：左边为 false，继续执行右边
            var t = engine.TryEvaluate("(1>2) || ERROR('right')", true);
            Assert.AreEqual(true, t);
            Assert.IsNotNull(engine.LastError);
        }

        [Test]
        public void or_not_strict_all_valid()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseStrictMode = false;
            var t = engine.TryEvaluate("1>2 || 3>2", false);
            Assert.AreEqual(true, t);
            Assert.IsNull(engine.LastError);
        }

        #endregion

        #region n 元 AND() 函数

        [Test]
        public void and_n_strict_should_error_when_any_param_is_error()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseStrictMode = true;
            var t = engine.TryEvaluate("AND(true(), true(), ERROR('test'))", true);
            Assert.AreEqual(true, t);
            Assert.IsNotNull(engine.LastError);
        }

        [Test]
        public void and_n_strict_all_valid()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseStrictMode = true;
            var t = engine.TryEvaluate("AND(true(), 1=1, 3>2)", false);
            Assert.AreEqual(true, t);
            Assert.IsNull(engine.LastError);
        }

        [Test]
        public void and_n_not_strict_short_circuit()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseStrictMode = false;
            // 非严格模式：遇到 false 就短路
            var t = engine.TryEvaluate("AND(true(), false(), ERROR('test'))", true);
            Assert.AreEqual(false, t);
            Assert.IsNull(engine.LastError);
        }

        [Test]
        public void and_n_not_strict_error_before_short_circuit()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseStrictMode = false;
            // 非严格模式：错误在短路条件之前，应报错
            var t = engine.TryEvaluate("AND(ERROR('first'), false(), true())", true);
            Assert.AreEqual(true, t);
            Assert.IsNotNull(engine.LastError);
        }

        [Test]
        public void and_n_not_strict_all_valid()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseStrictMode = false;
            var t = engine.TryEvaluate("AND(true(), 1=1, 3>2)", false);
            Assert.AreEqual(true, t);
            Assert.IsNull(engine.LastError);
        }

        #endregion

        #region n 元 OR() 函数

        [Test]
        public void or_n_strict_should_error_when_any_param_is_error()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseStrictMode = true;
            var t = engine.TryEvaluate("OR(false(), false(), ERROR('test'))", true);
            Assert.AreEqual(true, t);
            Assert.IsNotNull(engine.LastError);
        }

        [Test]
        public void or_n_strict_all_valid()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseStrictMode = true;
            var t = engine.TryEvaluate("OR(false(), 1=2, 3>2)", false);
            Assert.AreEqual(true, t);
            Assert.IsNull(engine.LastError);
        }

        [Test]
        public void or_n_not_strict_short_circuit()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseStrictMode = false;
            // 非严格模式：遇到 true 就短路
            var t = engine.TryEvaluate("OR(false(), true(), ERROR('test'))", false);
            Assert.AreEqual(true, t);
            Assert.IsNull(engine.LastError);
        }

        [Test]
        public void or_n_not_strict_error_before_short_circuit()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseStrictMode = false;
            // 非严格模式：错误在短路条件之前，应报错
            var t = engine.TryEvaluate("OR(ERROR('first'), true(), false())", true);
            Assert.AreEqual(true, t);
            Assert.IsNotNull(engine.LastError);
        }

        [Test]
        public void or_n_not_strict_all_valid()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseStrictMode = false;
            var t = engine.TryEvaluate("OR(false(), 1=2, 3>2)", false);
            Assert.AreEqual(true, t);
            Assert.IsNull(engine.LastError);
        }

        #endregion

        #region 链式短路

        [Test]
        public void chained_and_not_strict_short_circuit()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseStrictMode = false;
            // (1>2) && ... 短路，跳过 ERROR
            var t = engine.TryEvaluate("(1>2) && ERROR('a') && ERROR('b')", true);
            Assert.AreEqual(false, t);
            Assert.IsNull(engine.LastError);
        }

        [Test]
        public void chained_or_not_strict_short_circuit()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseStrictMode = false;
            // (2>1) || ... 短路，跳过 ERROR
            var t = engine.TryEvaluate("(2>1) || ERROR('a') || ERROR('b')", false);
            Assert.AreEqual(true, t);
            Assert.IsNull(engine.LastError);
        }

        [Test]
        public void chained_and_strict_all_evaluated()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseStrictMode = true;
            // 严格模式：所有条件都要求值，遇到错误报错
            var t = engine.TryEvaluate("(1>2) && (3>2) && ERROR('test')", true);
            Assert.AreEqual(true, t);
            Assert.IsNotNull(engine.LastError);
        }

        [Test]
        public void chained_or_strict_all_evaluated()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseStrictMode = true;
            // 严格模式：所有条件都要求值，遇到错误报错
            var t = engine.TryEvaluate("(2>1) || (3>2) || ERROR('test')", false);
            Assert.AreEqual(false, t);
            Assert.IsNotNull(engine.LastError);
        }

        #endregion

        #region 默认行为

        [Test]
        public void default_mode_is_strict()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            // 默认 UseStrictMode = true
            var t = engine.TryEvaluate("(2>1) || ERROR('test')", false);
            Assert.AreEqual(false, t);
            Assert.IsNotNull(engine.LastError);
        }

        #endregion
    }
}
