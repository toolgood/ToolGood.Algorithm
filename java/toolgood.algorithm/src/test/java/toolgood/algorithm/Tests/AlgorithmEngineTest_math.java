package toolgood.algorithm.Tests;

import static org.junit.Assert.assertEquals;

import java.math.BigDecimal;

import org.junit.Test;

import toolgood.algorithm.AlgorithmEngine;

@SuppressWarnings("deprecation")
public class AlgorithmEngineTest_math {
    @Test
    public void Pi_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("pi()", 0.0);
        assertEquals(3.141592654, round(t, 9),0.01);
    }
    @Test
    public void abs_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("abs(-1.2)", 0.0);
        assertEquals(1.2, t,0.01);
    }
    @Test
    public void QUOTIENT_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("QUOTIENT(7,3)", 0.0);
        assertEquals(2.0, t,0.01);
    }
    @Test
    public void MOD_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("MOD(7,3)", 0.0);
        assertEquals(1.0, t,0.01);
    }
    @Test
    public void SIGN_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SIGN(0)", 0);
        assertEquals(0, t,0.01);
        t = engine.TryEvaluate("SIGN(9)", 0);
        assertEquals(1, t,0.01);
        t = engine.TryEvaluate("SIGN(-9)", 0);
        assertEquals(-1, t,0.01);
    }
    @Test
    public void SQRT_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SQRT(9)", 0.0);
        assertEquals(3.0, t,0.01);
    }
    @Test
    public void SUM_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SUM(1,2,3,4)", 0.0);
        assertEquals(10.0, t,0.01);
    }
    @Test
    public void TRUNC_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("TRUNC(9.222)", 0.0);
        assertEquals(9.0, t,0.01);
        t = engine.TryEvaluate("TRUNC(-9.222)", 0.0);
        assertEquals(-9.0, t,0.01);
    }
    @Test
    public void int_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("int(9.222)", 0.0);
        assertEquals(9.0, t,0.01);
        t = engine.TryEvaluate("int(-9.222)", 0.0);
        assertEquals(-9.0, t,0.01);
    }
    @Test
    public void GCD_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("GCD(3,5,7)", 0.0);
        assertEquals(1.0, t,0.01);
        t = engine.TryEvaluate("GCD(30,21)", 0.0);
        assertEquals(3.0, t,0.01);
    }
    @Test
    public void LCM_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("LCM(3,5,7)", 0.0);
        assertEquals(105.0, t,0.01);

    }
    @Test
    public void combin_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("combin(10,2)", 0.0);
        assertEquals(45.0, t,0.01);
    }
    @Test
    public void PERMUT_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("PERMUT(10,2)", 0.0);
        assertEquals(90.0, t,0.01);

    }


    @Test
    public void degrees_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("degrees(pi())", 0.0);
        assertEquals(180.0, t,0.01);
    }
    @Test
    public void RADIANS_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("RADIANS(180)", 0.0);
        assertEquals(Math.PI, t,0.01);
    }
    @Test
    public void cos_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("cos(1)", 0.0);
        t = round(t, 6);
        assertEquals(round(0.540302306, 6), t,0.01);
    }
    @Test
    public void cosh_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("cosh(1)", 0.0);
        t = round(t, 6);
        assertEquals(round(1.543080635, 6), t,0.01);
    }
    @Test
    public void sin_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("sin(1)", 0.0);
        t = round(t, 6);
        assertEquals(round(0.841470985, 6), t,0.01);
    }
    @Test
    public void sinh_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("sinh(1)", 0.0);
        t = round(t, 6);
        assertEquals(round(1.175201194, 6), t,0.01);
    }
    @Test
    public void tan_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("tan(1)", 0.0);
        t = round(t, 6);
        assertEquals(round(1.557407725, 6), t,0.01);
    }
    @Test
    public void tanh_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("tanh(1)", 0.0);
        t = round(t, 6);
        assertEquals(round(0.761594156, 6), t,0.01);
    }
    @Test
    public void acos_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("acos(0.5)", 0.0);
        t = round(t, 6);
        assertEquals(round(1.047197551, 6), t,0.01);
    }
    @Test
    public void acosh_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("acosh(1.5)", 0.0);
        t = round(t, 6);
        assertEquals(round(0.96242365, 6), t,0.01);
    }
    @Test
    public void asin_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("asin(0.5)", 0.0);
        t = round(t, 6);
        assertEquals(round(0.523598776, 6), t,0.01);
    }
    @Test
    public void asinh_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("asinh(1.5)", 0.0);
        t = round(t, 6);
        assertEquals(round(1.194763217, 6), t,0.01);
    }
    @Test
    public void atan_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("atan(1)", 0.0);
        t = round(t, 6);
        assertEquals(round(0.785398163, 6), t,0.01);
    }
    @Test
    public void atanh_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("atanh(0.5)", 0.0);
        t = round(t, 6);
        assertEquals(round(0.549306144, 6), t,0.01);
    }
    @Test
    public void atan2_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("atan2(1,2)", 0.0);
        t = round(t, 6);
        assertEquals(round(1.107148718, 6), t,0.01);
    }


    @Test
    public void ROUND_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("ROUND(4.333,2)", 0.0);
        assertEquals(4.33, t,0.01);
    }
    @Test
    public void ROUNDDOWN_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("ROUNDDOWN(4.333,2)", 0.0);
        assertEquals(4.33, t,0.01);

        t = engine.TryEvaluate("ROUNDDOWN(-3.141592, 3)", 0.0);
        assertEquals(-3.141, t,0.01);
    }
    @Test
    public void ROUNDUP_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("ROUNDUP(4.333,2)", 0.0);
        assertEquals(4.34, t,0.01);

        t = engine.TryEvaluate("ROUNDUP(-3.141592, 3)", 0.0);
        assertEquals(-3.142, t,0.01);
    }
    @Test
    public void CEILING_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("CEILING(4.333,0.1)", 0.0);
        assertEquals(4.4, t,0.01);

        t = engine.TryEvaluate("CEILING(4.333)", 0.0);
        assertEquals(5, t,0.01);
    }
    @Test
    public void FLOOR_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("FLOOR(4.363,0.1)", 0.0);
        assertEquals(4.3, t,0.01);

        t = engine.TryEvaluate("FLOOR(4.333)", 0.0);
        assertEquals(4, t,0.01);
    }
    @Test
    public void even_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("even(4.363)", 0.0);
        assertEquals(6.0, t,0.01);
    }
    @Test
    public void odd_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("odd(4.363)", 0.0);
        assertEquals(5, t,0.01);
    }
    @Test
    public void MROUND_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("MROUND(4.363,2)", 0.0);
        assertEquals(4, t,0.01);
        t = engine.TryEvaluate("MROUND(5.363,2)", 0.0);
        assertEquals(6, t,0.01);
    }



    @Test
    public void Rand_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("RAND()", 0.0);
        assertEquals(true, t>=0);
        assertEquals(true, t<1);
    }
    @Test
    public void RANDBETWEEN_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("RANDBETWEEN(2,99)", 0.0);
        assertEquals(true, t>2);
        assertEquals(true, t<=99);
    }

    public void fact_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("fact(6)", 0.0);
        assertEquals(720.0, t,0.01);
        t = engine.TryEvaluate("fact(3)", 0.0);
        assertEquals(6.0, t,0.01);
    }
    @Test
    public void factdouble_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("factdouble(10)", 0.0);
        assertEquals(3840.0, t,0.01);
    }
    @Test
    public void POWER_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("POWER(10,2)", 0.0);
        assertEquals(100.0, t,0.01);
    }
    @Test
    public void exp_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("exp(2)", 0.0);
        t = round(t, 6);
        assertEquals(round(7.389056099, 6), t,0.01);
    }
    @Test
    public void ln_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("ln(4)", 0.0);
        t = round(t, 6);
        assertEquals(round(1.386294361, 6), t,0.01);
    }
    @Test
    public void log_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("log(10)", 0.0);
        t = round(t, 6);
        assertEquals(round(1.0, 6), t,0.01);

        t = engine.TryEvaluate("log(8,2)", 0.0);
        t = round(t, 6);
        assertEquals(round(3.0, 6), t,0.01);
    }
    @Test
    public void log10_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("log10(10)", 0.0);
        t = round(t, 6);
        assertEquals(round(1.0, 6), t,0.01);
    }
    @Test
    public void MULTINOMIAL_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("MULTINOMIAL(1,2,3)", 0.0);
        t = round(t, 6);
        assertEquals(round(60.0, 6), t,0.01);
        t = engine.TryEvaluate("MULTINOMIAL(1,2,3,4)", 0.0);
        t = round(t, 6);
        assertEquals(round(12600.0, 6), t,0.01);
    }
    @Test
    public void PRODUCT_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("PRODUCT(1,2,3,4)", 0.0);
        t = round(t, 6);
        assertEquals(round(24.0, 6), t,0.01);
    }
    @Test
    public void SQRTPI_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SQRTPI(3)", 0.0);
        t = round(t, 6);
        assertEquals(round(3.069980124, 6), t,0.01);
    }
    @Test
    public void SUMSQ_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SUMSQ(1,2)", 0.0);
        t = round(t, 6);
        assertEquals(round(5.0, 6), t,0.01);
    }


    @Test
    public void transformation_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        double num = engine.TryEvaluate("BIN2DEC(10101)", 0);
        assertEquals(num, 21);
        num = engine.TryEvaluate("OCT2DEC(12456)", 0);
        assertEquals(num, 5422);
        num = engine.TryEvaluate("HEX2DEC('213adf')", 0);
        assertEquals(num, 2177759);

        String t = engine.TryEvaluate("DEC2BIN(10)", "");
        assertEquals(t, "1010");
        t = engine.TryEvaluate("OCT2BIN('721')", "");
        assertEquals(t, "111010001");
        t = engine.TryEvaluate("HEX2BIN('fa')", "");
        assertEquals(t, "11111010");
        t = engine.TryEvaluate("'fa'.HEX2BIN()", "");
        assertEquals(t, "11111010");

        t = engine.TryEvaluate("BIN2OCT(10)", "");
        assertEquals(t, "2");
        t = engine.TryEvaluate("DEC2OCT('75')", "");
        assertEquals(t, "113");
        t = engine.TryEvaluate("HEX2OCT('f5')", "");
        assertEquals(t, "365");

        t = engine.TryEvaluate("BIN2HEX(101010100)", "");
        assertEquals(t, "154");
        t = engine.TryEvaluate("OCT2HEX(75212)", "");
        assertEquals(t, "7A8A");
        t = engine.TryEvaluate("DEC2HEX(952)", "");
        assertEquals(t, "3B8");

    }


    // @SuppressWarnings("deprecation")
    private double round(final double value, final int p) {
        final BigDecimal bigD = new BigDecimal(value);
        return bigD.setScale(p, BigDecimal.ROUND_HALF_UP).doubleValue();
    }
}