package toolgood.algorithm.Tests;

import org.junit.Test;
import toolgood.algorithm.AlgorithmEngine;

import static org.junit.Assert.*;

public class MathBaseTest {

    @Test
    public void Pi_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("pi()", 0.0);
        assertEquals(3.141592654, Math.round((Double) t * 1000000000L) / 1000000000.0);
    }

    @Test
    public void abs_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("abs(-1.2)", 0.0);
        assertEquals(1.2, t);
    }

    @Test
    public void QUOTIENT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("QUOTIENT(7,3)", 0.0);
        assertEquals(2.0, t);
    }

    @Test
    public void MOD_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("MOD(7,3)", 0.0);
        assertEquals(1.0, t);
    }

    @Test
    public void SIGN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SIGN(0)", 0.0);
        assertEquals(0, t);
        t = engine.TryEvaluate("SIGN(9)", 0.0);
        assertEquals(1, t);
        t = engine.TryEvaluate("SIGN(-9)", 0.0);
        assertEquals(-1, t);
    }

    @Test
    public void SQRT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SQRT(9)", 0.0);
        assertEquals(3.0, t);
    }

    @Test
    public void SUM_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SUM(1,2,3,4)", 0.0);
        assertEquals(10.0, t);
    }

    @Test
    public void TRUNC_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("TRUNC(9.222)", 0.0);
        assertEquals(9.0, t);
        t = engine.TryEvaluate("TRUNC(-9.222)", 0.0);
        assertEquals(-9.0, t);
    }

    @Test
    public void int_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("int(9.222)", 0.0);
        assertEquals(9.0, t);
        t = engine.TryEvaluate("int(-9.222)", 0.0);
        assertEquals(-9.0, t);
    }

    @Test
    public void GCD_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("GCD(3,5,7)", 0.0);
        assertEquals(1.0, t);
        t = engine.TryEvaluate("GCD(30,21)", 0.0);
        assertEquals(3.0, t);
    }

    @Test
    public void LCM_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("LCM(3,5,7)", 0.0);
        assertEquals(105.0, t);
    }

    @Test
    public void combin_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("combin(10,2)", 0.0);
        assertEquals(45.0, t);
    }

    @Test
    public void PERMUT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("PERMUT(10,2)", 0.0);
        assertEquals(90.0, t);
    }

    // 四舍五入
    @Test
    public void ROUND_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("ROUND(4.333,2)", 0.0);
        assertEquals(4.33, t);
    }

    @Test
    public void ROUND_single_param_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("ROUND(4.5)", 0.0);
        assertEquals(5.0, t);

        t = engine.TryEvaluate("ROUND(4.4)", 0.0);
        assertEquals(4.0, t);

        t = engine.TryEvaluate("ROUND(-4.5)", 0.0);
        assertEquals(-5.0, t);
    }

    @Test
    public void ROUNDDOWN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("ROUNDDOWN(4.333,2)", 0.0);
        assertEquals(4.33, t);

        t = engine.TryEvaluate("ROUNDDOWN(-3.141592, 3)", 0.0);
        assertEquals(-3.141, t);
    }

    @Test
    public void ROUNDUP_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("ROUNDUP(4.333,2)", 0.0);
        assertEquals(4.34, t);

        t = engine.TryEvaluate("ROUNDUP(-3.141592, 3)", 0.0);
        assertEquals(-3.142, t);
    }

    @Test
    public void CEILING_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("CEILING(4.333,0.1)", 0.0);
        assertEquals(4.4, t);

        t = engine.TryEvaluate("CEILING(4.333)", 0.0);
        assertEquals(5, t);
    }

    @Test
    public void FLOOR_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("FLOOR(4.363,0.1)", 0.0);
        assertEquals(4.3, t);

        t = engine.TryEvaluate("FLOOR(4.333)", 0.0);
        assertEquals(4, t);
    }

    @Test
    public void even_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("even(4.363)", 0.0);
        assertEquals(6.0, t);
    }

    @Test
    public void odd_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("odd(4.363)", 0.0);
        assertEquals(5, t);
    }

    @Test
    public void MROUND_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("MROUND(4.363,2)", 0.0);
        assertEquals(4, t);
        t = engine.TryEvaluate("MROUND(5.363,2)", 0.0);
        assertEquals(6, t);
    }

    // 随机数
    @Test
    public void Rand_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("RAND()", 0.0);
        double value = (Double) t;
        assertTrue(value > 0);
        assertTrue(value <= 1);
    }

    @Test
    public void RANDBETWEEN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("RANDBETWEEN(2,99)", 0.0);
        double value = (Double) t;
        assertTrue(value > 2);
        assertTrue(value <= 99);
    }

    // 幂/对数/阶乘
    @Test
    public void fact_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("fact(6)", 0.0);
        assertEquals(720.0, t);
        t = engine.TryEvaluate("fact(3)", 0.0);
        assertEquals(6.0, t);
    }

    @Test
    public void factdouble_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("factdouble(10)", 0.0);
        assertEquals(3840.0, t);
    }

    @Test
    public void POWER_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("POWER(10,2)", 0.0);
        assertEquals(100.0, t);
    }

    @Test
    public void exp_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("exp(2)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(7.389056099 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void ln_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("ln(4)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(1.386294361 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void log_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("log(10)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(1.0 * 1000000) / 1000000.0, rounded);

        t = engine.TryEvaluate("log(8,2)", 0.0);
        rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(3.0 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void log10_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("log10(10)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(1.0 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void MULTINOMIAL_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("MULTINOMIAL(1,2,3)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(60.0 * 1000000) / 1000000.0, rounded);
        t = engine.TryEvaluate("MULTINOMIAL(1,2,3,4)", 0.0);
        rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(12600.0 * 1000000) / 1000000.0, rounded);
        t = engine.TryEvaluate("MULTINOMIAL(1,2,3,4.1)", 0.0);
        rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(12600.0 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void PRODUCT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("PRODUCT(1,2,3,4)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(24.0 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void SQRTPI_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SQRTPI(3)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(3.069980124 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void SUMSQ_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SUMSQ(1,2)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(Math.round(5.0 * 1000000) / 1000000.0, rounded);
    }

    // 工程函数
    @Test
    public void ERF_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("ERF(0)", 0.0);
        assertEquals(0.0, Math.round((Double) t * 1000000) / 1000000.0);

        t = engine.TryEvaluate("ERF(1)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(0.842701, rounded);

        t = engine.TryEvaluate("ERF(-1)", 0.0);
        rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(-0.842701, rounded);

        t = engine.TryEvaluate("ERF(0.5)", 0.0);
        assertEquals(0.520499878, (Double) t, 6);

        t = engine.TryEvaluate("ERF(4)", 0.0);
        assertEquals(0.999999985, (Double) t, 6);

        t = engine.TryEvaluate("ERF(5)", 0.0);
        assertEquals(1, (Double) t, 6);

        t = engine.TryEvaluate("ERF(7)", 0.0);
        assertEquals(1, (Double) t, 6);
    }

    @Test
    public void ERFC_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("ERFC(0)", 0.0);
        assertEquals(1.0, Math.round((Double) t * 1000000) / 1000000.0);

        t = engine.TryEvaluate("ERFC(1)", 0.0);
        double rounded = Math.round((Double) t * 1000000) / 1000000.0;
        assertEquals(0.157299, rounded);

        t = engine.TryEvaluate("ERFC(-3)", 0.0);
        assertEquals(1.99997791, (Double) t, 6);

        t = engine.TryEvaluate("ERFC(1.5)", 0.0);
        assertEquals(0.033894854, (Double) t, 6);
    }

    @Test
    public void DELTA_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("DELTA(5, 5)", 0.0);
        assertEquals(1.0, t);

        t = engine.TryEvaluate("DELTA(5, 4)", 0.0);
        assertEquals(0.0, t);

        t = engine.TryEvaluate("DELTA(5)", 0.0);
        assertEquals(0.0, t);

        t = engine.TryEvaluate("DELTA(0)", 0.0);
        assertEquals(1.0, t);
    }

    @Test
    public void GESTEP_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("GESTEP(5, 4)", 0.0);
        assertEquals(1.0, t);

        t = engine.TryEvaluate("GESTEP(5, 5)", 0.0);
        assertEquals(1.0, t);

        t = engine.TryEvaluate("GESTEP(5, 6)", 0.0);
        assertEquals(0.0, t);

        t = engine.TryEvaluate("GESTEP(5)", 0.0);
        assertEquals(1.0, t);

        t = engine.TryEvaluate("GESTEP(-1)", 0.0);
        assertEquals(0.0, t);
    }

    // 百分比
    @Test
    public void Percentage_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("50%", 0.0);
        assertEquals(0.5, t);

        t = engine.TryEvaluate("100%", 0.0);
        assertEquals(1.0, t);

        t = engine.TryEvaluate("1%", 0.0);
        assertEquals(0.01, t);

        t = engine.TryEvaluate("0%", 0.0);
        assertEquals(0.0, t);

        t = engine.TryEvaluate("200%", 0.0);
        assertEquals(2.0, t);

        t = engine.TryEvaluate("25.5%", 0.0);
        assertEquals(0.255, t);
    }

    // 边界值测试
    @Test
    public void DivisionByZero_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("1/0", 0.0);
        assertEquals(t, 0);

        t = engine.TryEvaluate("0/0", 0.0);
        assertEquals(t, 0);
    }

    @Test
    public void Overflow_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("99999999999999999999*99999999999999999999", 0.0);
        assertEquals(t, 0);
    }

    @Test
    public void NullOperation_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("null+1", 0.0);
        assertEquals(t, 0);

        t = engine.TryEvaluate("null*100", 0.0);
        assertEquals(t, 0);

        t = engine.TryEvaluate("null-null", 0.0);
        assertEquals(t, 0);
    }

    @Test
    public void SquareRootNegative_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("sqrt(-1)", 0.0);
        assertEquals(t, 0);
    }

    @Test
    public void LogNegative_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("log(-1)", 0.0);
        assertEquals(t, 0);

        t = engine.TryEvaluate("ln(0)", 0.0);
        assertEquals(t, 0);
    }
}
