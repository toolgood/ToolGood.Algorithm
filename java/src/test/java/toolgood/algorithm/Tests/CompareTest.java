package toolgood.algorithm.Tests;

import org.junit.Test;
import toolgood.algorithm.AlgorithmEngine;

import static org.junit.Assert.assertEquals;

class CompareTest {

    @Test
    void base_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean b = engine.TryEvaluate("1=1", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1=2", true);
        assertEquals(b, false);

        b = engine.TryEvaluate("1<>2", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1!=2", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1>2", true);
        assertEquals(b, false);

        b = engine.TryEvaluate("1<2", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1<=2", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1>=2", true);
        assertEquals(b, false);

        b = engine.TryEvaluate("'1'='1'", false);
        assertEquals(b, true);
        b = engine.TryEvaluate("'e'='e'", false);
        assertEquals(b, true);
        b = engine.TryEvaluate("'1'='2'", true);
        assertEquals(b, false);
        b = engine.TryEvaluate("'1'!='2'", false);
        assertEquals(b, true);
    }

    @Test
    void strict_equality_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean b = engine.TryEvaluate("1===1", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1===2", true);
        assertEquals(b, false);

        b = engine.TryEvaluate("'1'==='1'", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("'1'==='2'", true);
        assertEquals(b, false);

        b = engine.TryEvaluate("1!==2", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("1!==1", true);
        assertEquals(b, false);

        b = engine.TryEvaluate("'1'!=='2'", false);
        assertEquals(b, true);

        b = engine.TryEvaluate("'1'!=='1'", true);
        assertEquals(b, false);
    }

    @Test
    void null_compare_test() {
        AlgorithmEngine engine = new AlgorithmEngine();

        boolean bb2 = engine.TryEvaluate("1>null", false);
        assertEquals(bb2, false);

        bb2 = engine.TryEvaluate("1>=null", false);
        assertEquals(bb2, false);

        bb2 = engine.TryEvaluate("1<=null", false);
        assertEquals(bb2, false);

        bb2 = engine.TryEvaluate("1<null", false);
        assertEquals(bb2, false);

        bb2 = engine.TryEvaluate("1==null", false);
        assertEquals(bb2, false);

        bb2 = engine.TryEvaluate("1!=null", false);
        assertEquals(bb2, true);

        bb2 = engine.TryEvaluate("null=null", false);
        assertEquals(bb2, true);

        bb2 = engine.TryEvaluate("null!=null", false);
        assertEquals(bb2, false);

        bb2 = engine.TryEvaluate("'111'=null", false);
        assertEquals(bb2, false);

        bb2 = engine.TryEvaluate("'111'!=null", false);
        assertEquals(bb2, true);
    }

    @Test
    void negative_compare_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean value = engine.TryEvaluate("1 > (-2)", false);
        assertEquals(value, true);

        value = engine.TryEvaluate("(-1) > (-2)", false);
        assertEquals(value, true);

        value = engine.TryEvaluate("-1 > (-2)", false);
        assertEquals(value, true);

        value = engine.TryEvaluate("-1 > -2", false);
        assertEquals(value, true);

        boolean value3 = engine.TryEvaluate("-7 < -2", false);
        assertEquals(value3, true);

        value3 = engine.TryEvaluate("-7*Yes < -2", false);
        assertEquals(value3, true);

        value3 = engine.TryEvaluate("-7*No > -2", false);
        assertEquals(value3, true);
    }
}
