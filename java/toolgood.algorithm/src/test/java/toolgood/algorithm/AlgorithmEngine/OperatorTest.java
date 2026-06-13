package toolgood.algorithm.algorithmengine;

import org.junit.Test;
import static org.junit.Assert.*;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.AlgorithmEngine;

public class OperatorTest {
    @Test
    public void arithmetic_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int t = engine.TryEvaluate("1+(3*2+2)/2", 0);
        assertEquals(5, t);

        t = engine.TryEvaluate("(8-3)*(3+2)", 0);
        assertEquals(25, t);

        t = engine.TryEvaluate("(8-3)*(3+2) % 7", 0);
        assertEquals(4, t);

        int c = engine.TryEvaluate("2+3", 0);
        assertEquals(5, c);
        c = engine.TryEvaluate("(2)+3", 0);
        assertEquals(5, c);
        c = engine.TryEvaluate("2+3*2+10/2*4", 0);
        assertEquals(28, c);

        c = engine.TryEvaluate("2.1e3 + 10", 0);
        assertEquals(2110, c);

        c = engine.TryEvaluate("2.1e+03 + 10", 0);
        assertEquals(2110, c);

        c = engine.TryEvaluate("2.1e+3 + 10", 0);
        assertEquals(2110, c);

        double d = engine.TryEvaluate("2.1e-3 + 10", 0.0);
        assertEquals(10.0021, d, 1e-10);
    }

    @Test
    public void connect_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String s = engine.TryEvaluate("'aa'&'bb'", "");
        assertEquals("aabb", s);

        s = engine.TryEvaluate("'3'+2", "");
        assertEquals("5", s);
    }

    @Test
    public void conditional_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int t1 = engine.TryEvaluate("-7 < -2 ?1 : 2", 0);
        assertEquals(1, t1);
        t1 = engine.TryEvaluate("-7 < -2 ?1 ： 2", 0);
        assertEquals(1, t1);

        t1 = engine.TryEvaluate("-7 < -2 ?1 ： (7>1?3:2)", 0);
        assertEquals(1, t1);

        t1 = engine.TryEvaluate("-7 < -2 ?1 ：2", 0);
        assertEquals(1, t1);
        t1 = engine.TryEvaluate("-7 < -2 ？ 1 : 2", 0);
        assertEquals(1, t1);
        t1 = engine.TryEvaluate("-7 < -2 ？1 : 2", 0);
        assertEquals(1, t1);

        t1 = engine.TryEvaluate("-7 < -2 ？1 ： 2", 0);
        assertEquals(1, t1);

        t1 = engine.TryEvaluate("(!(-7 < -2))？1：2", 0);
        assertEquals(2, t1);
        t1 = engine.TryEvaluate("1>2？1：2", 0);
        assertEquals(2, t1);

        t1 = engine.TryEvaluate("1！=2？1：2", 0);
        assertEquals(1, t1);
    }

    @Test
    public void percentage_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("100%", 0.0);
        assertEquals(1.0, t, 1e-10);

        t = engine.TryEvaluate("50%", 0.0);
        assertEquals(0.5, t, 1e-10);

        t = engine.TryEvaluate("200%", 0.0);
        assertEquals(2.0, t, 1e-10);

        t = engine.TryEvaluate("100*50%", 0.0);
        assertEquals(50.0, t, 1e-10);

        t = engine.TryEvaluate("100+50%", 0.0);
        assertEquals(100.5, t, 1e-10);
    }

    @Test
    public void null_operation_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int tbb2 = engine.TryEvaluate("'111'*null", 0);
        assertEquals(0, tbb2);
    }
}
