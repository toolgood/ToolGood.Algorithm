package toolgood.algorithm.Tests;

import org.junit.Test;
import toolgood.algorithm.AlgorithmEngine;

import static org.junit.Assert.assertEquals;

public class FlowTest {

    @Test
    public void If_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("if(1=1,1,2)", 0.0);
        assertEquals(t, 1);
        t = engine.TryEvaluate("if(1=1,1)", 0.0);
        assertEquals(t, 1);

        t = engine.TryEvaluate("if(1=1,1)", 0.0);
        assertEquals(t, 1);

        t = engine.TryEvaluate("if('1',1,2)", 0.0);
        assertEquals(t, 1);
        t = engine.TryEvaluate("if(0,1,2)", 0.0);
        assertEquals(t, 2);
    }

    @Test
    public void iferror_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("iferror(1/0,1,2)", 0.0);
        assertEquals(t, 1);

        t = engine.TryEvaluate("iferror(1-'rrr',1,2)", 0.0);
        assertEquals(t, 1);
    }

    @Test
    public void iserror_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("iserror(1/0,1)", 0.0);
        assertEquals(t, 1);

        t = engine.TryEvaluate("iserror(1-'rrr',1)", 0.0);
        assertEquals(t, 1);
    }

    @Test
    public void ifnull_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("isnull(null,1)", 0.0);
        assertEquals(t, 1);

        t = engine.TryEvaluate("isnull(1,2)", 0.0);
        assertEquals(t, 1);
    }

    @Test
    public void isnullorerror_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("isnullorerror(null,1)", 0.0);
        assertEquals(t, 1);

        t = engine.TryEvaluate("isnullorerror(1/0,1)", 0.0);
        assertEquals(t, 1);

        t = engine.TryEvaluate("isnullorerror(1,2)", 0.0);
        assertEquals(t, 1);
    }

    @Test
    public void ISNUMBER_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("if(ISNUMBER(1),1,2)", 0.0);
        assertEquals(t, 1);

        t = engine.TryEvaluate("if(ISNUMBER('e'),1,2)", 0.0);
        assertEquals(t, 2);
        t = engine.TryEvaluate("if(ISNUMBER('11'),1,2)", 0.0);
        assertEquals(t, 2);

        t = engine.TryEvaluate("if(ISNUMBER('2016-1-2'),1,2)", 0.0);
        assertEquals(t, 2);
    }

    @Test
    public void ISTEXT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("if(ISTEXT(1),1,2)", 0.0);
        assertEquals(t, 2);

        t = engine.TryEvaluate("if(ISTEXT('e'),1,2)", 0.0);
        assertEquals(t, 1);
        t = engine.TryEvaluate("if(ISTEXT('11'),1,2)", 0.0);
        assertEquals(t, 1);

        t = engine.TryEvaluate("if(ISTEXT('2016-1-2'),1,2)", 0.0);
        assertEquals(t, 1);
    }

    @Test
    public void ISNONTEXT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("if(ISNONTEXT(1),1,2)", 0.0);
        assertEquals(t, 1);

        t = engine.TryEvaluate("if(ISNONTEXT('e'),1,2)", 0.0);
        assertEquals(t, 2);
        t = engine.TryEvaluate("if(ISNONTEXT('11'),1,2)", 0.0);
        assertEquals(t, 2);

        t = engine.TryEvaluate("if(ISNONTEXT('2016-1-2'),1,2)", 0.0);
        assertEquals(t, 2);
    }

    @Test
    public void ISLOGICAL_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("if(ISLOGICAL(1),1,2)", 0.0);
        assertEquals(t, 2);

        t = engine.TryEvaluate("if(ISLOGICAL('e'),1,2)", 0.0);
        assertEquals(t, 2);

        t = engine.TryEvaluate("if(ISLOGICAL(true),1,2)", 0.0);
        assertEquals(t, 1);

        t = engine.TryEvaluate("if(ISLOGICAL(false),1,2)", 0.0);
        assertEquals(t, 1);
    }

    @Test
    public void ISEVEN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("if(ISEVEN(1),1,2)", 0.0);
        assertEquals(t, 2);

        t = engine.TryEvaluate("if(ISEVEN(2),1,2)", 0.0);
        assertEquals(t, 1);

        t = engine.TryEvaluate("if(ISEVEN('e'),1,2)", 0.0);
        assertEquals(t, 2);
    }

    @Test
    public void ISODD_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("if(ISODD(1),1,2)", 0.0);
        assertEquals(t, 1);

        t = engine.TryEvaluate("if(ISODD(2),1,2)", 0.0);
        assertEquals(t, 2);

        t = engine.TryEvaluate("if(ISODD('e'),1,2)", 0.0);
        assertEquals(t, 2);
    }

    @Test
    public void And_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("and(true(),1=1)", 0.0);
        assertEquals(t, true);
        t = engine.TryEvaluate("and(true(),1)", 0.0);
        assertEquals(t, true);

        t = engine.TryEvaluate("and(true(),false(),1=1)", 0.0);
        assertEquals(t, false);
        t = engine.TryEvaluate("and(true(),false(),1)", 0.0);
        assertEquals(t, false);

        t = engine.TryEvaluate("and(true(),0)", 0.0);
        assertEquals(t, false);
    }

    @Test
    public void Or_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("or(true(),1=1)", 0.0);
        assertEquals(t, true);
        t = engine.TryEvaluate("or(true(),1)", 0.0);
        assertEquals(t, true);

        t = engine.TryEvaluate("or(true(),false(),1=1)", 0.0);
        assertEquals(t, true);
        t = engine.TryEvaluate("or(true(),false(),1)", 0.0);
        assertEquals(t, true);

        t = engine.TryEvaluate("or(true(),0)", 0.0);
        assertEquals(t, true);

        t = engine.TryEvaluate("or(false(),1=2)", 0.0);
        assertEquals(t, false);
    }

    @Test
    public void Not_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("not(true())", 0.0);
        assertEquals(t, false);
        t = engine.TryEvaluate("not(false())", 0.0);
        assertEquals(t, true);
    }

    @Test
    public void true_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("true()", 0.0);
        assertEquals(t, true);
    }

    @Test
    public void false_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("false()", 0.0);
        assertEquals(t, false);
    }

    @Test
    public void andor_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean t = engine.TryEvaluate("1=1 && 2==2 && 3=3", false);
        assertEquals(t, true);

        t = engine.TryEvaluate("1=1 && 2!=2", false);
        assertEquals(t, false);

        t = engine.TryEvaluate("1=1 || 2!=2", false);
        assertEquals(t, true);
    }

    @Test
    public void IFS_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("IFS(1=1, 'a', 1=2, 'b')", 0.0);
        assertEquals(t, "a");

        t = engine.TryEvaluate("IFS(1=2, 'a', 2=2, 'b')", 0.0);
        assertEquals(t, "b");

        Object t2 = engine.TryEvaluate("IFS(1=1, 100, 1=2, 200)", 0.0);
        assertEquals(t2, 100);
    }

    @Test
    public void SWITCH_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        double t = engine.TryEvaluate("SWITCH(1, 1, 'one', 2, 'two')", 0.0);
        assertEquals(t, "one");

        t = engine.TryEvaluate("SWITCH(2, 1, 'one', 2, 'two')", 0.0);
        assertEquals(t, "two");

        Object t2 = engine.TryEvaluate("SWITCH('a', 'a', 1, 'b', 2)", 0.0);
        assertEquals(t2, 1);

        t = engine.TryEvaluate("SWITCH(5, 1, 'one', 2, 'two')", 0.0);
        assertEquals(t, "");
    }

    @Test
    public void XOR_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean t = engine.TryEvaluate("XOR(true(), false())", false);
        assertEquals(t, true);

        t = engine.TryEvaluate("XOR(true(), true())", false);
        assertEquals(t, false);

        t = engine.TryEvaluate("XOR(false(), false())", false);
        assertEquals(t, false);

        t = engine.TryEvaluate("XOR(true(), true(), true())", false);
        assertEquals(t, true);

        t = engine.TryEvaluate("XOR(true(), false(), false())", false);
        assertEquals(t, true);

        t = engine.TryEvaluate("XOR(1, 0)", false);
        assertEquals(t, true);

        t = engine.TryEvaluate("XOR(1, 1)", false);
        assertEquals(t, false);
    }

    @Test
    public void MethodStyle_ISNUMBER_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean t = engine.TryEvaluate("123.ISNUMBER()", false);
        assertEquals(t, true);

        t = engine.TryEvaluate("'abc'.ISNUMBER()", false);
        assertEquals(t, false);

        t = engine.TryEvaluate("true.ISNUMBER()", false);
        assertEquals(t, false);
    }

    @Test
    public void MethodStyle_ISTEXT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean t = engine.TryEvaluate("'abc'.ISTEXT()", false);
        assertEquals(t, true);

        t = engine.TryEvaluate("123.ISTEXT()", false);
        assertEquals(t, false);
    }

    @Test
    public void MethodStyle_ISNONTEXT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean t = engine.TryEvaluate("123.ISNONTEXT()", false);
        assertEquals(t, true);

        t = engine.TryEvaluate("'abc'.ISNONTEXT()", false);
        assertEquals(t, false);
    }

    @Test
    public void MethodStyle_ISLOGICAL_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean t = engine.TryEvaluate("true.ISLOGICAL()", false);
        assertEquals(t, true);

        t = engine.TryEvaluate("false.ISLOGICAL()", false);
        assertEquals(t, true);

        t = engine.TryEvaluate("123.ISLOGICAL()", false);
        assertEquals(t, false);
    }

    @Test
    public void MethodStyle_ISNULL_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean t = engine.TryEvaluate("null.ISNULL()", false);
        assertEquals(t, true);

        t = engine.TryEvaluate("123.ISNULL()", false);
        assertEquals(t, false);
    }

    @Test
    public void MethodStyle_ISNULLORERROR_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean t = engine.TryEvaluate("null.ISNULLORERROR()", false);
        assertEquals(t, true);
    }
}
