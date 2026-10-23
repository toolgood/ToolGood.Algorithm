package toolgood.algorithm;

import org.junit.Test;
import static org.junit.Assert.*;
import toolgood.algorithm.AlgorithmEngine;

public class StrictModeTest {

    // #region 二元 && 运算符

    @Test
    public void and_strict_should_error_when_right_is_error() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseStrictMode = true;
        boolean t = engine.TryEvaluate("(1>2) && ERROR('test')", true);
        // 严格模式下，即使左边为 false，右边错误也会传播
        assertEquals(true, t);
        assertNotNull(engine.LastError);
    }

    @Test
    public void and_strict_all_valid() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseStrictMode = true;
        boolean t = engine.TryEvaluate("2>1 && 3>2", false);
        assertEquals(true, t);
        assertNull(engine.LastError);
    }

    @Test
    public void and_not_strict_short_circuit() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseStrictMode = false;
        // 非严格模式：1>2 为 false，短路不执行右边
        boolean t = engine.TryEvaluate("(1>2) && ERROR('test')", true);
        assertEquals(false, t);
        assertNull(engine.LastError);
    }

    @Test
    public void and_not_strict_error_on_left() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseStrictMode = false;
        // 非严格模式：左边错误无法短路，应报错
        boolean t = engine.TryEvaluate("ERROR('left') && (2>1)", true);
        assertEquals(true, t);
        assertNotNull(engine.LastError);
    }

    @Test
    public void and_not_strict_continue_when_left_true() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseStrictMode = false;
        // 非严格模式：左边为 true，继续执行右边
        boolean t = engine.TryEvaluate("(2>1) && ERROR('right')", true);
        assertEquals(true, t);
        assertNotNull(engine.LastError);
    }

    @Test
    public void and_not_strict_all_valid() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseStrictMode = false;
        boolean t = engine.TryEvaluate("2>1 && 3>2", false);
        assertEquals(true, t);
        assertNull(engine.LastError);
    }

    // #endregion

    // #region 二元 || 运算符

    @Test
    public void or_strict_should_error_when_right_is_error() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseStrictMode = true;
        // 严格模式下，即使左边为 true，右边错误也会传播
        boolean t = engine.TryEvaluate("(2>1) || ERROR('test')", false);
        assertEquals(false, t);
        assertNotNull(engine.LastError);
    }

    @Test
    public void or_strict_all_valid() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseStrictMode = true;
        boolean t = engine.TryEvaluate("1>2 || 3>2", false);
        assertEquals(true, t);
        assertNull(engine.LastError);
    }

    @Test
    public void or_not_strict_short_circuit() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseStrictMode = false;
        // 非严格模式：2>1 为 true，短路不执行右边
        boolean t = engine.TryEvaluate("(2>1) || ERROR('test')", false);
        assertEquals(true, t);
        assertNull(engine.LastError);
    }

    @Test
    public void or_not_strict_error_on_left() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseStrictMode = false;
        // 非严格模式：左边错误无法短路，应报错
        boolean t = engine.TryEvaluate("ERROR('left') || (2>1)", false);
        assertEquals(false, t);
        assertNotNull(engine.LastError);
    }

    @Test
    public void or_not_strict_continue_when_left_false() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseStrictMode = false;
        // 非严格模式：左边为 false，继续执行右边
        boolean t = engine.TryEvaluate("(1>2) || ERROR('right')", true);
        assertEquals(true, t);
        assertNotNull(engine.LastError);
    }

    @Test
    public void or_not_strict_all_valid() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseStrictMode = false;
        boolean t = engine.TryEvaluate("1>2 || 3>2", false);
        assertEquals(true, t);
        assertNull(engine.LastError);
    }

    // #endregion

    // #region n 元 AND() 函数

    @Test
    public void and_n_strict_should_error_when_any_param_is_error() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseStrictMode = true;
        boolean t = engine.TryEvaluate("AND(true(), true(), ERROR('test'))", true);
        assertEquals(true, t);
        assertNotNull(engine.LastError);
    }

    @Test
    public void and_n_strict_all_valid() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseStrictMode = true;
        boolean t = engine.TryEvaluate("AND(true(), 1=1, 3>2)", false);
        assertEquals(true, t);
        assertNull(engine.LastError);
    }

    @Test
    public void and_n_not_strict_short_circuit() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseStrictMode = false;
        // 非严格模式：遇到 false 就短路
        boolean t = engine.TryEvaluate("AND(true(), false(), ERROR('test'))", true);
        assertEquals(false, t);
        assertNull(engine.LastError);
    }

    @Test
    public void and_n_not_strict_error_before_short_circuit() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseStrictMode = false;
        // 非严格模式：错误在短路条件之前，应报错
        boolean t = engine.TryEvaluate("AND(ERROR('first'), false(), true())", true);
        assertEquals(true, t);
        assertNotNull(engine.LastError);
    }

    @Test
    public void and_n_not_strict_all_valid() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseStrictMode = false;
        boolean t = engine.TryEvaluate("AND(true(), 1=1, 3>2)", false);
        assertEquals(true, t);
        assertNull(engine.LastError);
    }

    // #endregion

    // #region n 元 OR() 函数

    @Test
    public void or_n_strict_should_error_when_any_param_is_error() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseStrictMode = true;
        boolean t = engine.TryEvaluate("OR(false(), false(), ERROR('test'))", true);
        assertEquals(true, t);
        assertNotNull(engine.LastError);
    }

    @Test
    public void or_n_strict_all_valid() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseStrictMode = true;
        boolean t = engine.TryEvaluate("OR(false(), 1=2, 3>2)", false);
        assertEquals(true, t);
        assertNull(engine.LastError);
    }

    @Test
    public void or_n_not_strict_short_circuit() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseStrictMode = false;
        // 非严格模式：遇到 true 就短路
        boolean t = engine.TryEvaluate("OR(false(), true(), ERROR('test'))", false);
        assertEquals(true, t);
        assertNull(engine.LastError);
    }

    @Test
    public void or_n_not_strict_error_before_short_circuit() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseStrictMode = false;
        // 非严格模式：错误在短路条件之前，应报错
        boolean t = engine.TryEvaluate("OR(ERROR('first'), true(), false())", true);
        assertEquals(true, t);
        assertNotNull(engine.LastError);
    }

    @Test
    public void or_n_not_strict_all_valid() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseStrictMode = false;
        boolean t = engine.TryEvaluate("OR(false(), 1=2, 3>2)", false);
        assertEquals(true, t);
        assertNull(engine.LastError);
    }

    // #endregion

    // #region 链式短路

    @Test
    public void chained_and_not_strict_short_circuit() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseStrictMode = false;
        // (1>2) && ... 短路，跳过 ERROR
        boolean t = engine.TryEvaluate("(1>2) && ERROR('a') && ERROR('b')", true);
        assertEquals(false, t);
        assertNull(engine.LastError);
    }

    @Test
    public void chained_or_not_strict_short_circuit() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseStrictMode = false;
        // (2>1) || ... 短路，跳过 ERROR
        boolean t = engine.TryEvaluate("(2>1) || ERROR('a') || ERROR('b')", false);
        assertEquals(true, t);
        assertNull(engine.LastError);
    }

    @Test
    public void chained_and_strict_all_evaluated() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseStrictMode = true;
        // 严格模式：所有条件都要求值，遇到错误报错
        boolean t = engine.TryEvaluate("(1>2) && (3>2) && ERROR('test')", true);
        assertEquals(true, t);
        assertNotNull(engine.LastError);
    }

    @Test
    public void chained_or_strict_all_evaluated() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseStrictMode = true;
        // 严格模式：所有条件都要求值，遇到错误报错
        boolean t = engine.TryEvaluate("(2>1) || (3>2) || ERROR('test')", false);
        assertEquals(false, t);
        assertNotNull(engine.LastError);
    }

    // #endregion

    // #region 默认行为

    @Test
    public void default_mode_is_strict() {
        AlgorithmEngine engine = new AlgorithmEngine();
        // 默认 UseStrictMode = true
        boolean t = engine.TryEvaluate("(2>1) || ERROR('test')", false);
        assertEquals(false, t);
        assertNotNull(engine.LastError);
    }

    // #endregion
}
