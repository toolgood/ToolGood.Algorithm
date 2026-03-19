package toolgood.algorithm.Tests;

import org.junit.Test;
import toolgood.algorithm.AlgorithmEngine;

import static org.junit.Assert.assertEquals;

public class OperatorTest {

    @Test
    public void arithmetic_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("1+(3*2+2)/2", 0.0);
        assertEquals(5, t, 0.0001);

        t = engine.TryEvaluate("(8-3)*(3+2)", 0.0);
        assertEquals(25, t, 0.0001);

        t = engine.TryEvaluate("(8-3)*(3+2) % 7", 0.0);
        assertEquals(4, t, 0.0001);

        double c = engine.TryEvaluate("2+3", 0.0);
        assertEquals(5, c, 0.0001);
        c = engine.TryEvaluate("(2)+3", 0.0);
        assertEquals(5, c, 0.0001);
        c = engine.TryEvaluate("2+3*2+10/2*4", 0.0);
        assertEquals(28, c, 0.0001);

        c = engine.TryEvaluate("2.1e3 + 10", 0.0);
        assertEquals(2110, c, 0.0001);

        c = engine.TryEvaluate("2.1e+03 + 10", 0.0);
        assertEquals(2110, c, 0.0001);

        c = engine.TryEvaluate("2.1e+3 + 10", 0.0);
        assertEquals(2110, c, 0.0001);

        double d = engine.TryEvaluate("2.1e-3 + 10", 0.0);
        assertEquals(10.0021, d, 0.0001);
    }

    @Test
    public void connect_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String s = engine.TryEvaluate("'aa'&'bb'", "");
        assertEquals("aabb", s);

        s = engine.TryEvaluate("'3'+2", "");
        assertEquals("32", s);
    }

    @Test
    public void conditional_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t1 = engine.TryEvaluate("-7 < -2 ? 1 : 2", 0.0);
        assertEquals(1, t1, 0.0001);

        t1 = engine.TryEvaluate("(!(-7 < -2)) ? 1 : 2", 0.0);
        assertEquals(2, t1, 0.0001);
        t1 = engine.TryEvaluate("1 > 2 ? 1 : 2", 0.0);
        assertEquals(2, t1, 0.0001);

        t1 = engine.TryEvaluate("1 != 2 ? 1 : 2", 0.0);
        assertEquals(1, t1, 0.0001);
    }

    @Test
    public void percentage_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("100%", 0.0);
        assertEquals(1.0, t, 0.0001);

        t = engine.TryEvaluate("50%", 0.0);
        assertEquals(0.5, t, 0.0001);

        t = engine.TryEvaluate("200%", 0.0);
        assertEquals(2.0, t, 0.0001);

        t = engine.TryEvaluate("100*50%", 0.0);
        assertEquals(50.0, t, 0.0001);

        t = engine.TryEvaluate("100+50%", 0.0);
        assertEquals(100.5, t, 0.0001);
    }

    @Test
    public void null_operation_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double tbb2 = engine.TryEvaluate("'111'*null", 0.0);
        assertEquals(0, tbb2, 0.0001);
    }
}
