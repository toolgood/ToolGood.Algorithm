package toolgood.algorithm.algorithmengine;

import org.junit.Test;
import static org.junit.Assert.*;

import toolgood.algorithm.AlgorithmEngine;

import toolgood.algorithm.AlgorithmEngine;

public class MathTrigonometricTest {

    @Test
    public void degrees_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("degrees(pi())", 0.0);
        assertEquals(180.0, t, 0.0);
    }

    @Test
    public void RADIANS_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("RADIANS(180)", 0.0);
        assertEquals(Math.PI, t, 1e-10);
    }

    @Test
    public void cos_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("cos(1)", 0.0);
        assertEquals(0.540302306, t, 1e-6);
    }

    @Test
    public void cosh_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("cosh(1)", 0.0);
        assertEquals(1.543080635, t, 1e-6);
    }

    @Test
    public void sin_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("sin(1)", 0.0);
        assertEquals(0.841470985, t, 1e-6);
    }

    @Test
    public void sinh_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("sinh(1)", 0.0);
        assertEquals(1.175201194, t, 1e-6);
    }

    @Test
    public void tan_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("tan(1)", 0.0);
        assertEquals(1.557407725, t, 1e-6);
    }

    @Test
    public void tanh_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("tanh(1)", 0.0);
        assertEquals(0.761594156, t, 1e-6);
    }

    @Test
    public void acos_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("acos(0.5)", 0.0);
        assertEquals(1.047197551, t, 1e-6);
    }

    @Test
    public void acosh_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("acosh(1.5)", 0.0);
        assertEquals(0.96242365, t, 1e-6);
    }

    @Test
    public void asin_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("asin(0.5)", 0.0);
        assertEquals(0.523598776, t, 1e-6);
    }

    @Test
    public void asinh_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("asinh(1.5)", 0.0);
        assertEquals(1.194763217, t, 1e-6);
    }

    @Test
    public void atan_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("atan(1)", 0.0);
        assertEquals(0.785398163, t, 1e-6);
    }

    @Test
    public void atanh_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("atanh(0.5)", 0.0);
        assertEquals(0.549306144, t, 1e-6);
    }

    @Test
    public void atan2_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("atan2(1,2)", 0.0);
        assertEquals(1.107148718, t, 1e-6);
    }

    @Test
    public void cot_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("cot(1)", 0.0);
        assertEquals(0.642092616, t, 1e-6);
    }

    @Test
    public void coth_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("coth(1)", 0.0);
        assertEquals(1.313035285, t, 1e-6);
    }

    @Test
    public void csc_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("csc(1)", 0.0);
        assertEquals(1.188395106, t, 1e-6);
    }

    @Test
    public void csch_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("csch(1)", 0.0);
        assertEquals(0.850918128, t, 1e-6);
    }

    @Test
    public void sec_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("sec(1)", 0.0);
        assertEquals(1.850815718, t, 1e-6);
    }

    @Test
    public void sech_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("sech(1)", 0.0);
        assertEquals(0.648054274, t, 1e-6);
    }

    @Test
    public void acot_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("acot(1)", 0.0);
        assertEquals(0.785398163, t, 1e-6);
    }

    @Test
    public void acoth_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("acoth(2)", 0.0);
        assertEquals(0.549306144, t, 1e-6);
    }
}
