package toolgood.algorithm.algorithmengine;

import org.junit.Test;
import static org.junit.Assert.*;

import toolgood.algorithm.AlgorithmEngine;

import toolgood.algorithm.AlgorithmEngine;

public class MathBaseTest {

    @Test
    public void Pi_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("pi()", 0.0);
        assertEquals(3.141592654, t, 1e-9);
    }

    @Test
    public void abs_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("abs(-1.2)", 0.0);
        assertEquals(1.2, t, 0.0);
    }

    @Test
    public void QUOTIENT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("QUOTIENT(7,3)", 0.0);
        assertEquals(2.0, t, 0.0);
    }

    @Test
    public void MOD_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("MOD(7,3)", 0.0);
        assertEquals(1.0, t, 0.0);
    }

    @Test
    public void SIGN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int t = engine.TryEvaluate("SIGN(0)", 0);
        assertEquals(0, t);
        t = engine.TryEvaluate("SIGN(9)", 0);
        assertEquals(1, t);
        t = engine.TryEvaluate("SIGN(-9)", 0);
        assertEquals(-1, t);
    }

    @Test
    public void SQRT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SQRT(9)", 0.0);
        assertEquals(3.0, t, 0.0);
    }

    @Test
    public void SUM_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SUM(1,2,3,4)", 0.0);
        assertEquals(10.0, t, 0.0);
    }

    @Test
    public void TRUNC_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("TRUNC(9.222)", 0.0);
        assertEquals(9.0, t, 0.0);
        t = engine.TryEvaluate("TRUNC(-9.222)", 0.0);
        assertEquals(-9.0, t, 0.0);
    }

    @Test
    public void TRUNC_ExcelCompatible_test() {
        AlgorithmEngine engine = new AlgorithmEngine();

        double t = engine.TryEvaluate("TRUNC(8.9)", 0.0);
        assertEquals(8.0, t, 0.0);

        t = engine.TryEvaluate("TRUNC(8.912, 2)", 0.0);
        assertEquals(8.91, t, 0.0);

        t = engine.TryEvaluate("TRUNC(8.912, 1)", 0.0);
        assertEquals(8.9, t, 0.0);

        t = engine.TryEvaluate("TRUNC(-5.5)", 0.0);
        assertEquals(-5.0, t, 0.0);

        t = engine.TryEvaluate("TRUNC(123.456, -2)", 0.0);
        assertEquals(100.0, t, 0.0);

        t = engine.TryEvaluate("TRUNC(123.456, -1)", 0.0);
        assertEquals(120.0, t, 0.0);

        t = engine.TryEvaluate("TRUNC(-123.456, -2)", 0.0);
        assertEquals(-100.0, t, 0.0);

        t = engine.TryEvaluate("TRUNC(0)", 0.0);
        assertEquals(0.0, t, 0.0);

        t = engine.TryEvaluate("TRUNC(PI(), 4)", 0.0);
        assertEquals(3.1415, t, 1e-4);
    }

    @Test
    public void int_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("int(9.222)", 0.0);
        assertEquals(9.0, t, 0.0);
        t = engine.TryEvaluate("int(-9.222)", 0.0);
        assertEquals(-9.0, t, 0.0);
    }

    @Test
    public void GCD_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("GCD(3,5,7)", 0.0);
        assertEquals(1.0, t, 0.0);
        t = engine.TryEvaluate("GCD(30,21)", 0.0);
        assertEquals(3.0, t, 0.0);
    }

    @Test
    public void LCM_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("LCM(3,5,7)", 0.0);
        assertEquals(105.0, t, 0.0);
    }

    @Test
    public void combin_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("combin(10,2)", 0.0);
        assertEquals(45.0, t, 0.0);
    }

    @Test
    public void PERMUT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("PERMUT(10,2)", 0.0);
        assertEquals(90.0, t, 0.0);
    }

    // 四舍五入

    @Test
    public void ROUND_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("ROUND(4.333,2)", 0.0);
        assertEquals(4.33, t, 0.0);
    }

    @Test
    public void ROUND_single_param_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("ROUND(4.5)", 0.0);
        assertEquals(5.0, t, 0.0);

        t = engine.TryEvaluate("ROUND(4.4)", 0.0);
        assertEquals(4.0, t, 0.0);

        t = engine.TryEvaluate("ROUND(-4.5)", 0.0);
        assertEquals(-5.0, t, 0.0);
    }

    @Test
    public void ROUNDDOWN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("ROUNDDOWN(4.333,2)", 0.0);
        assertEquals(4.33, t, 0.0);

        t = engine.TryEvaluate("ROUNDDOWN(-3.141592, 3)", 0.0);
        assertEquals(-3.141, t, 0.0);
    }

    @Test
    public void ROUNDUP_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("ROUNDUP(4.333,2)", 0.0);
        assertEquals(4.34, t, 0.0);

        t = engine.TryEvaluate("ROUNDUP(-3.141592, 3)", 0.0);
        assertEquals(-3.142, t, 0.0);
    }

    @Test
    public void CEILING_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("CEILING(4.333,0.1)", 0.0);
        assertEquals(4.4, t, 0.0);

        t = engine.TryEvaluate("CEILING(4.333)", 0.0);
        assertEquals(5.0, t, 0.0);
    }

    @Test
    public void FLOOR_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("FLOOR(4.363,0.1)", 0.0);
        assertEquals(4.3, t, 0.0);

        t = engine.TryEvaluate("FLOOR(4.333)", 0.0);
        assertEquals(4.0, t, 0.0);
    }

    @Test
    public void even_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("even(4.363)", 0.0);
        assertEquals(6.0, t, 0.0);
    }

    @Test
    public void odd_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("odd(4.363)", 0.0);
        assertEquals(5.0, t, 0.0);
    }

    @Test
    public void MROUND_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("MROUND(4.363,2)", 0.0);
        assertEquals(4.0, t, 0.0);
        t = engine.TryEvaluate("MROUND(5.363,2)", 0.0);
        assertEquals(6.0, t, 0.0);
    }

    // 随机数

    @Test
    public void Rand_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("RAND()", 0.0);
        assertTrue(t >= 0);
        assertTrue(t < 1);
    }

    @Test
    public void RANDBETWEEN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("RANDBETWEEN(2,99)", 0.0);
        assertTrue(t >= 2);
        assertTrue(t <= 99);
    }

    // 幂/对数/阶乘

    @Test
    public void fact_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("fact(6)", 0.0);
        assertEquals(720.0, t, 0.0);
        t = engine.TryEvaluate("fact(3)", 0.0);
        assertEquals(6.0, t, 0.0);
    }

    @Test
    public void factdouble_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("factdouble(10)", 0.0);
        assertEquals(3840.0, t, 0.0);
    }

    @Test
    public void POWER_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("POWER(10,2)", 0.0);
        assertEquals(100.0, t, 0.0);
    }

    @Test
    public void exp_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("exp(2)", 0.0);
        assertEquals(7.389056099, t, 1e-6);
    }

    @Test
    public void ln_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("ln(4)", 0.0);
        assertEquals(1.386294361, t, 1e-6);
    }

    @Test
    public void log_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("log(10)", 0.0);
        assertEquals(1.0, t, 1e-6);

        t = engine.TryEvaluate("log(8,2)", 0.0);
        assertEquals(3.0, t, 1e-6);
    }

    @Test
    public void log10_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("log10(10)", 0.0);
        assertEquals(1.0, t, 1e-6);
    }

    @Test
    public void MULTINOMIAL_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("MULTINOMIAL(1,2,3)", 0.0);
        assertEquals(60.0, t, 1e-6);
        t = engine.TryEvaluate("MULTINOMIAL(1,2,3,4)", 0.0);
        assertEquals(12600.0, t, 1e-6);
        t = engine.TryEvaluate("MULTINOMIAL(1,2,3,4.1)", 0.0);
        assertEquals(12600.0, t, 1e-6);
    }

    @Test
    public void PRODUCT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("PRODUCT(1,2,3,4)", 0.0);
        assertEquals(24.0, t, 1e-6);
    }

    @Test
    public void SQRTPI_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SQRTPI(3)", 0.0);
        assertEquals(3.069980124, t, 1e-6);
    }

    @Test
    public void SUMSQ_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SUMSQ(1,2)", 0.0);
        assertEquals(5.0, t, 1e-6);
    }

    // 工程函数

    @Test
    public void ERF_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("ERF(0)", 0.0);
        assertEquals(0.0, t, 1e-6);

        t = engine.TryEvaluate("ERF(1)", 0.0);
        assertEquals(0.842701, t, 1e-6);

        t = engine.TryEvaluate("ERF(-1)", 0.0);
        assertEquals(-0.842701, t, 1e-6);

        t = engine.TryEvaluate("ERF(0.5)", 0.0);
        assertEquals(0.520499878, t, 1e-6);

        t = engine.TryEvaluate("ERF(4)", 0.0);
        assertEquals(0.999999985, t, 1e-6);

        t = engine.TryEvaluate("ERF(5)", 0.0);
        assertEquals(1.0, t, 1e-6);

        t = engine.TryEvaluate("ERF(7)", 0.0);
        assertEquals(1.0, t, 1e-6);
    }

    @Test
    public void ERFC_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("ERFC(0)", 0.0);
        assertEquals(1.0, t, 1e-6);

        t = engine.TryEvaluate("ERFC(1)", 0.0);
        assertEquals(0.157299, t, 1e-6);

        t = engine.TryEvaluate("ERFC(-3)", 0.0);
        assertEquals(1.99997791, t, 1e-6);

        t = engine.TryEvaluate("ERFC(1.5)", 0.0);
        assertEquals(0.033894854, t, 1e-6);
    }

    @Test
    public void DELTA_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("DELTA(5, 5)", 0.0);
        assertEquals(1.0, t, 0.0);

        t = engine.TryEvaluate("DELTA(5, 4)", 0.0);
        assertEquals(0.0, t, 0.0);

        t = engine.TryEvaluate("DELTA(5)", 0.0);
        assertEquals(0.0, t, 0.0);

        t = engine.TryEvaluate("DELTA(0)", 0.0);
        assertEquals(1.0, t, 0.0);
    }

    @Test
    public void GESTEP_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("GESTEP(5, 4)", 0.0);
        assertEquals(1.0, t, 0.0);

        t = engine.TryEvaluate("GESTEP(5, 5)", 0.0);
        assertEquals(1.0, t, 0.0);

        t = engine.TryEvaluate("GESTEP(5, 6)", 0.0);
        assertEquals(0.0, t, 0.0);

        t = engine.TryEvaluate("GESTEP(5)", 0.0);
        assertEquals(1.0, t, 0.0);

        t = engine.TryEvaluate("GESTEP(-1)", 0.0);
        assertEquals(0.0, t, 0.0);
    }

    // 百分比

    @Test
    public void Percentage_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("50%", 0.0);
        assertEquals(0.5, t, 0.0);

        t = engine.TryEvaluate("100%", 0.0);
        assertEquals(1.0, t, 0.0);

        t = engine.TryEvaluate("1%", 0.0);
        assertEquals(0.01, t, 0.0);

        t = engine.TryEvaluate("0%", 0.0);
        assertEquals(0.0, t, 0.0);

        t = engine.TryEvaluate("200%", 0.0);
        assertEquals(2.0, t, 0.0);

        t = engine.TryEvaluate("25.5%", 0.0);
        assertEquals(0.255, t, 0.0);
    }

    // 边界值测试

    @Test
    public void DivisionByZero_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("1/0", 0.0);
        assertEquals(0.0, t, 0.0);

        t = engine.TryEvaluate("0/0", 0.0);
        assertEquals(0.0, t, 0.0);
    }

    @Test
    public void Overflow_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("99999999999999999999*99999999999999999999", 0.0);
        assertEquals(0.0, t, 0.0);
    }

    @Test
    public void NullOperation_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("null+1", 0.0);
        assertEquals(0.0, t, 0.0);

        t = engine.TryEvaluate("null*100", 0.0);
        assertEquals(0.0, t, 0.0);

        t = engine.TryEvaluate("null-null", 0.0);
        assertEquals(0.0, t, 0.0);
    }

    @Test
    public void SquareRootNegative_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("sqrt(-1)", 0.0);
        assertEquals(0.0, t, 0.0);
    }

    @Test
    public void LogNegative_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("log(-1)", 0.0);
        assertEquals(0.0, t, 0.0);

        t = engine.TryEvaluate("ln(0)", 0.0);
        assertEquals(0.0, t, 0.0);
    }

    // Excel兼容性测试

    @Test
    public void ODD_ExcelCompatible_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("ODD(1)", 0.0);
        assertEquals(1.0, t, 0.0);

        t = engine.TryEvaluate("ODD(2)", 0.0);
        assertEquals(3.0, t, 0.0);

        t = engine.TryEvaluate("ODD(3)", 0.0);
        assertEquals(3.0, t, 0.0);

        t = engine.TryEvaluate("ODD(-1)", 0.0);
        assertEquals(-1.0, t, 0.0);

        t = engine.TryEvaluate("ODD(-2)", 0.0);
        assertEquals(-3.0, t, 0.0);

        t = engine.TryEvaluate("ODD(-3)", 0.0);
        assertEquals(-3.0, t, 0.0);

        t = engine.TryEvaluate("ODD(-4)", 0.0);
        assertEquals(-5.0, t, 0.0);

        t = engine.TryEvaluate("ODD(1.5)", 0.0);
        assertEquals(3.0, t, 0.0);

        t = engine.TryEvaluate("ODD(-1.5)", 0.0);
        assertEquals(-3.0, t, 0.0);
    }

    @Test
    public void EVEN_ExcelCompatible_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("EVEN(2)", 0.0);
        assertEquals(2.0, t, 0.0);

        t = engine.TryEvaluate("EVEN(1)", 0.0);
        assertEquals(2.0, t, 0.0);

        t = engine.TryEvaluate("EVEN(-1)", 0.0);
        assertEquals(-2.0, t, 0.0);

        t = engine.TryEvaluate("EVEN(-2)", 0.0);
        assertEquals(-2.0, t, 0.0);

        t = engine.TryEvaluate("EVEN(-3)", 0.0);
        assertEquals(-4.0, t, 0.0);

        t = engine.TryEvaluate("EVEN(1.5)", 0.0);
        assertEquals(2.0, t, 0.0);

        t = engine.TryEvaluate("EVEN(-1.5)", 0.0);
        assertEquals(-2.0, t, 0.0);

        t = engine.TryEvaluate("EVEN(0)", 0.0);
        assertEquals(0.0, t, 0.0);
    }

    @Test
    public void CEILING_ExcelCompatible_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("CEILING(2.5, 1)", 0.0);
        assertEquals(3.0, t, 0.0);

        t = engine.TryEvaluate("CEILING(-2.5, -1)", 0.0);
        assertEquals(-3.0, t, 0.0);

        t = engine.TryEvaluate("CEILING(-2.5, 1)", 0.0);
        assertEquals(-2.0, t, 0.0);

        t = engine.TryEvaluate("CEILING(1.5, 0.1)", 0.0);
        assertEquals(1.5, t, 0.0);

        t = engine.TryEvaluate("CEILING(0.234, 0.01)", 0.0);
        assertEquals(0.24, t, 0.0);
    }

    @Test
    public void FLOOR_ExcelCompatible_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("FLOOR(3.7, 1)", 0.0);
        assertEquals(3.0, t, 0.0);

        t = engine.TryEvaluate("FLOOR(-3.7, -1)", 0.0);
        assertEquals(-3.0, t, 0.0);

        t = engine.TryEvaluate("FLOOR(-3.7, 1)", 0.0);
        assertEquals(-4.0, t, 0.0);

        t = engine.TryEvaluate("FLOOR(3.7, 2)", 0.0);
        assertEquals(2.0, t, 0.0);

        t = engine.TryEvaluate("FLOOR(0, 1)", 0.0);
        assertEquals(0.0, t, 0.0);
    }

    @Test
    public void MROUND_ExcelCompatible_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("MROUND(10, 3)", 0.0);
        assertEquals(9.0, t, 0.0);

        t = engine.TryEvaluate("MROUND(-10, -3)", 0.0);
        assertEquals(-9.0, t, 0.0);

        t = engine.TryEvaluate("MROUND(1.3, 0.2)", 0.0);
        assertEquals(1.4, t, 0.0);

        t = engine.TryEvaluate("MROUND(-1.3, -0.2)", 0.0);
        assertEquals(-1.4, t, 0.0);
    }

    @Test
    public void ROUNDDOWN_ExcelCompatible_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("ROUNDDOWN(3.7, 0)", 0.0);
        assertEquals(3.0, t, 0.0);

        t = engine.TryEvaluate("ROUNDDOWN(-3.7, 0)", 0.0);
        assertEquals(-3.0, t, 0.0);

        t = engine.TryEvaluate("ROUNDDOWN(3.777, 2)", 0.0);
        assertEquals(3.77, t, 0.0);

        t = engine.TryEvaluate("ROUNDDOWN(-3.777, 2)", 0.0);
        assertEquals(-3.77, t, 0.0);

        t = engine.TryEvaluate("ROUNDDOWN(76.9, -1)", 0.0);
        assertEquals(70.0, t, 0.0);

        t = engine.TryEvaluate("ROUNDDOWN(-76.9, -1)", 0.0);
        assertEquals(-70.0, t, 0.0);
    }
}
