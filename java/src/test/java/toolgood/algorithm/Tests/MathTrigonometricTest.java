package toolgood.algorithm.Tests;

import org.junit.Test;
import toolgood.algorithm.AlgorithmEngine;

import static org.junit.Assert.assertEquals;

public class MathTrigonometricTest {

    @Test
    public void degrees_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("degrees(pi())", 0.0);
        assertEquals(180.0, t, 0.0001);
    }

    @Test
    public void RADIANS_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("RADIANS(180)", 0.0);
        assertEquals(Math.PI, t, 0.0001);
    }

    @Test
    public void cos_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("cos(1)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.540302306 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void cosh_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("cosh(1)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(1.543080635 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void sin_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("sin(1)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.841470985 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void sinh_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("sinh(1)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(1.175201194 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void tan_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("tan(1)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(1.557407725 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void tanh_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("tanh(1)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.761594156 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void acos_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("acos(0.5)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(1.047197551 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void acosh_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("acosh(1.5)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.96242365 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void asin_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("asin(0.5)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.523598776 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void asinh_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("asinh(1.5)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(1.194763217 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void atan_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("atan(1)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.785398163 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void atanh_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("atanh(0.5)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.549306144 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void atan2_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("atan2(1,2)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(1.107148718 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void cot_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("cot(1)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.642092616 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void coth_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("coth(1)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(1.313035285 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void csc_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("csc(1)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(1.188395106 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void csch_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("csch(1)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.850918128 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void sec_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("sec(1)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(1.850815718 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void sech_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("sech(1)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.648054274 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void acot_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("acot(1)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.785398163 * 1000000) / 1000000.0, rounded);
    }

    @Test
    public void acoth_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("acoth(2)", 0.0);
        double rounded = Math.round(t * 1000000) / 1000000.0;
        assertEquals(Math.round(0.549306144 * 1000000) / 1000000.0, rounded);
    }
}
