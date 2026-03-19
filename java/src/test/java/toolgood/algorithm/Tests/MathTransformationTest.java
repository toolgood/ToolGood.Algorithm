package toolgood.algorithm.Tests;

import org.junit.Test;
import toolgood.algorithm.AlgorithmEngine;

import static org.junit.Assert.assertEquals;

public class MathTransformationTest {

    @Test
    void BIN2DEC_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object num = engine.TryEvaluate("BIN2DEC(10101)", 0);
        assertEquals(num, 21);
    }

    @Test
    void OCT2DEC_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object num = engine.TryEvaluate("OCT2DEC(12456)", 0);
        assertEquals(num, 5422);
    }

    @Test
    void HEX2DEC_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object num = engine.TryEvaluate("HEX2DEC('213adf')", 0);
        assertEquals(num, 2177759);
    }

    @Test
    void DEC2BIN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object t = engine.TryEvaluate("DEC2BIN(10)", "");
        assertEquals(t, "1010");
        t = engine.TryEvaluate("DEC2BIN(10,8)", "");
        assertEquals(t, "00001010");
    }

    @Test
    void OCT2BIN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object t = engine.TryEvaluate("OCT2BIN('721')", "");
        assertEquals(t, "111010001");
    }

    @Test
    void HEX2BIN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object t = engine.TryEvaluate("HEX2BIN('fa')", "");
        assertEquals(t, "11111010");
    }

    @Test
    void BIN2OCT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object t = engine.TryEvaluate("BIN2OCT(10)", "");
        assertEquals(t, "2");
    }

    @Test
    void DEC2OCT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object t = engine.TryEvaluate("DEC2OCT('75')", "");
        assertEquals(t, "113");
    }

    @Test
    void HEX2OCT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object t = engine.TryEvaluate("HEX2OCT('f5')", "");
        assertEquals(t, "365");
    }

    @Test
    void BIN2HEX_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object t = engine.TryEvaluate("BIN2HEX(101010100)", "");
        assertEquals(t, "154");
    }

    @Test
    void OCT2HEX_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object t = engine.TryEvaluate("OCT2HEX(75212)", "");
        assertEquals(t, "7A8A");
    }

    @Test
    void DEC2HEX_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object t = engine.TryEvaluate("DEC2HEX(952)", "");
        assertEquals(t, "3B8");
    }

    @Test
    void ARABIC_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object t = engine.TryEvaluate("ARABIC('I')", 0);
        assertEquals(t, 1);

        t = engine.TryEvaluate("ARABIC('IV')", 0);
        assertEquals(t, 4);

        t = engine.TryEvaluate("ARABIC('IX')", 0);
        assertEquals(t, 9);

        t = engine.TryEvaluate("ARABIC('X')", 0);
        assertEquals(t, 10);

        t = engine.TryEvaluate("ARABIC('XL')", 0);
        assertEquals(t, 40);

        t = engine.TryEvaluate("ARABIC('L')", 0);
        assertEquals(t, 50);

        t = engine.TryEvaluate("ARABIC('XC')", 0);
        assertEquals(t, 90);

        t = engine.TryEvaluate("ARABIC('C')", 0);
        assertEquals(t, 100);

        t = engine.TryEvaluate("ARABIC('CD')", 0);
        assertEquals(t, 400);

        t = engine.TryEvaluate("ARABIC('D')", 0);
        assertEquals(t, 500);

        t = engine.TryEvaluate("ARABIC('CM')", 0);
        assertEquals(t, 900);

        t = engine.TryEvaluate("ARABIC('M')", 0);
        assertEquals(t, 1000);

        t = engine.TryEvaluate("ARABIC('MMXXIII')", 0);
        assertEquals(t, 2023);
    }

    @Test
    void ROMAN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object t = engine.TryEvaluate("ROMAN(1)", "");
        assertEquals(t, "I");

        t = engine.TryEvaluate("ROMAN(4)", "");
        assertEquals(t, "IV");

        t = engine.TryEvaluate("ROMAN(9)", "");
        assertEquals(t, "IX");

        t = engine.TryEvaluate("ROMAN(10)", "");
        assertEquals(t, "X");

        t = engine.TryEvaluate("ROMAN(40)", "");
        assertEquals(t, "XL");

        t = engine.TryEvaluate("ROMAN(50)", "");
        assertEquals(t, "L");

        t = engine.TryEvaluate("ROMAN(90)", "");
        assertEquals(t, "XC");

        t = engine.TryEvaluate("ROMAN(100)", "");
        assertEquals(t, "C");

        t = engine.TryEvaluate("ROMAN(400)", "");
        assertEquals(t, "CD");

        t = engine.TryEvaluate("ROMAN(500)", "");
        assertEquals(t, "D");

        t = engine.TryEvaluate("ROMAN(900)", "");
        assertEquals(t, "CM");

        t = engine.TryEvaluate("ROMAN(1000)", "");
        assertEquals(t, "M");

        t = engine.TryEvaluate("ROMAN(2023)", "");
        assertEquals(t, "MMXXIII");
    }
}
