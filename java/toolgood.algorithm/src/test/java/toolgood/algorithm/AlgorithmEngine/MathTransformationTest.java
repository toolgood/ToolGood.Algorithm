package toolgood.algorithm.algorithmengine;

import org.junit.Test;
import static org.junit.Assert.*;

import toolgood.algorithm.AlgorithmEngine;

import toolgood.algorithm.AlgorithmEngine;

public class MathTransformationTest {

    @Test
    public void BIN2DEC_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int num = engine.TryEvaluate("BIN2DEC(10101)", 0);
        assertEquals(21, num);
    }

    @Test
    public void OCT2DEC_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int num = engine.TryEvaluate("OCT2DEC(12456)", 0);
        assertEquals(5422, num);
    }

    @Test
    public void HEX2DEC_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int num = engine.TryEvaluate("HEX2DEC('213adf')", 0);
        assertEquals(2177759, num);
    }

    @Test
    public void DEC2BIN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("DEC2BIN(10)", "");
        assertEquals("1010", t);
        t = engine.TryEvaluate("DEC2BIN(10,8)", "");
        assertEquals("00001010", t);
    }

    @Test
    public void OCT2BIN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("OCT2BIN('721')", "");
        assertEquals("111010001", t);
    }

    @Test
    public void HEX2BIN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("HEX2BIN('fa')", "");
        assertEquals("11111010", t);
    }

    @Test
    public void BIN2OCT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("BIN2OCT(10)", "");
        assertEquals("2", t);
    }

    @Test
    public void DEC2OCT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("DEC2OCT('75')", "");
        assertEquals("113", t);
    }

    @Test
    public void HEX2OCT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("HEX2OCT('f5')", "");
        assertEquals("365", t);
    }

    @Test
    public void BIN2HEX_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("BIN2HEX(101010100)", "");
        assertEquals("154", t);
    }

    @Test
    public void OCT2HEX_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("OCT2HEX(75212)", "");
        assertEquals("7A8A", t);
    }

    @Test
    public void DEC2HEX_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("DEC2HEX(952)", "");
        assertEquals("3B8", t);
    }

    @Test
    public void ARABIC_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int t = engine.TryEvaluate("ARABIC('I')", 0);
        assertEquals(1, t);

        t = engine.TryEvaluate("ARABIC('IV')", 0);
        assertEquals(4, t);

        t = engine.TryEvaluate("ARABIC('IX')", 0);
        assertEquals(9, t);

        t = engine.TryEvaluate("ARABIC('X')", 0);
        assertEquals(10, t);

        t = engine.TryEvaluate("ARABIC('XL')", 0);
        assertEquals(40, t);

        t = engine.TryEvaluate("ARABIC('L')", 0);
        assertEquals(50, t);

        t = engine.TryEvaluate("ARABIC('XC')", 0);
        assertEquals(90, t);

        t = engine.TryEvaluate("ARABIC('C')", 0);
        assertEquals(100, t);

        t = engine.TryEvaluate("ARABIC('CD')", 0);
        assertEquals(400, t);

        t = engine.TryEvaluate("ARABIC('D')", 0);
        assertEquals(500, t);

        t = engine.TryEvaluate("ARABIC('CM')", 0);
        assertEquals(900, t);

        t = engine.TryEvaluate("ARABIC('M')", 0);
        assertEquals(1000, t);

        t = engine.TryEvaluate("ARABIC('MMXXIII')", 0);
        assertEquals(2023, t);
    }

    @Test
    public void ROMAN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("ROMAN(1)", "");
        assertEquals("I", t);

        t = engine.TryEvaluate("ROMAN(4)", "");
        assertEquals("IV", t);

        t = engine.TryEvaluate("ROMAN(9)", "");
        assertEquals("IX", t);

        t = engine.TryEvaluate("ROMAN(10)", "");
        assertEquals("X", t);

        t = engine.TryEvaluate("ROMAN(40)", "");
        assertEquals("XL", t);

        t = engine.TryEvaluate("ROMAN(50)", "");
        assertEquals("L", t);

        t = engine.TryEvaluate("ROMAN(90)", "");
        assertEquals("XC", t);

        t = engine.TryEvaluate("ROMAN(100)", "");
        assertEquals("C", t);

        t = engine.TryEvaluate("ROMAN(400)", "");
        assertEquals("CD", t);

        t = engine.TryEvaluate("ROMAN(500)", "");
        assertEquals("D", t);

        t = engine.TryEvaluate("ROMAN(900)", "");
        assertEquals("CM", t);

        t = engine.TryEvaluate("ROMAN(1000)", "");
        assertEquals("M", t);

        t = engine.TryEvaluate("ROMAN(2023)", "");
        assertEquals("MMXXIII", t);
    }
}
