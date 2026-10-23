package toolgood.algorithm.algorithmengine;

import org.junit.Test;
import static org.junit.Assert.*;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.AlgorithmEngine;

public class FlowTest {
    @Test
    public void If_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int t = engine.TryEvaluate("if(1=1,1,2)", 0);
        assertEquals(1, t);
        t = engine.TryEvaluate("if(1=1,1)", 0);
        assertEquals(1, t);

        t = engine.TryEvaluate("if(1=1，1)", 0);
        assertEquals(1, t);

        t = engine.TryEvaluate("if('1',1,2)", 0);
        assertEquals(1, t);
        t = engine.TryEvaluate("if(0,1,2)", 0);
        assertEquals(2, t);
    }

    @Test
    public void iferror_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int t = engine.TryEvaluate("iferror(1/0,1,2)", 0);
        assertEquals(1, t);

        t = engine.TryEvaluate("iferror(1-'rrr',1,2)", 0);
        assertEquals(1, t);
    }

    @Test
    public void iserror_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int t = engine.TryEvaluate("iserror(1/0,1)", 0);
        assertEquals(1, t);

        t = engine.TryEvaluate("iserror(1-'rrr',1)", 0);
        assertEquals(1, t);
    }

    @Test
    public void ifnull_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int t = engine.TryEvaluate("isnull(null,1)", 0);
        assertEquals(1, t);

        t = engine.TryEvaluate("isnull(1,2)", 0);
        assertEquals(1, t);
    }

    @Test
    public void isnullorerror_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int t = engine.TryEvaluate("isnullorerror(null,1)", 0);
        assertEquals(1, t);

        t = engine.TryEvaluate("isnullorerror(1/0,1)", 0);
        assertEquals(1, t);

        t = engine.TryEvaluate("isnullorerror(1,2)", 0);
        assertEquals(1, t);
    }

    @Test
    public void ISNUMBER_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int t = engine.TryEvaluate("if(ISNUMBER(1),1,2)", 0);
        assertEquals(1, t);

        t = engine.TryEvaluate("if(ISNUMBER('e'),1,2)", 0);
        assertEquals(2, t);
        t = engine.TryEvaluate("if(ISNUMBER('11'),1,2)", 0);
        assertEquals(2, t);

        t = engine.TryEvaluate("if(ISNUMBER('2016-1-2'),1,2)", 0);
        assertEquals(2, t);
    }

    @Test
    public void ISTEXT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int t = engine.TryEvaluate("if(ISTEXT(1),1,2)", 0);
        assertEquals(2, t);

        t = engine.TryEvaluate("if(ISTEXT('e'),1,2)", 0);
        assertEquals(1, t);
        t = engine.TryEvaluate("if(ISTEXT('11'),1,2)", 0);
        assertEquals(1, t);

        t = engine.TryEvaluate("if(ISTEXT('2016-1-2'),1,2)", 0);
        assertEquals(1, t);
    }

    @Test
    public void ISNONTEXT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int t = engine.TryEvaluate("if(ISNONTEXT(1),1,2)", 0);
        assertEquals(1, t);

        t = engine.TryEvaluate("if(ISNONTEXT('e'),1,2)", 0);
        assertEquals(2, t);
        t = engine.TryEvaluate("if(ISNONTEXT('11'),1,2)", 0);
        assertEquals(2, t);

        t = engine.TryEvaluate("if(ISNONTEXT('2016-1-2'),1,2)", 0);
        assertEquals(2, t);
    }

    @Test
    public void ISLOGICAL_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int t = engine.TryEvaluate("if(ISLOGICAL(1),1,2)", 0);
        assertEquals(2, t);

        t = engine.TryEvaluate("if(ISLOGICAL('e'),1,2)", 0);
        assertEquals(2, t);

        t = engine.TryEvaluate("if(ISLOGICAL(true),1,2)", 0);
        assertEquals(1, t);

        t = engine.TryEvaluate("if(ISLOGICAL(false),1,2)", 0);
        assertEquals(1, t);
    }

    @Test
    public void ISEVEN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int t = engine.TryEvaluate("if(ISEVEN(1),1,2)", 0);
        assertEquals(2, t);

        t = engine.TryEvaluate("if(ISEVEN(2),1,2)", 0);
        assertEquals(1, t);

        t = engine.TryEvaluate("if(ISEVEN('e'),1,2)", 0);
        assertEquals(2, t);
    }

    @Test
    public void ISODD_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int t = engine.TryEvaluate("if(ISODD(1),1,2)", 0);
        assertEquals(1, t);

        t = engine.TryEvaluate("if(ISODD(2),1,2)", 0);
        assertEquals(2, t);

        t = engine.TryEvaluate("if(ISODD('e'),1,2)", 0);
        assertEquals(2, t);
    }

    @Test
    public void And_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean t = engine.TryEvaluate("and(true(),1=1)", false);
        assertEquals(true, t);
        t = engine.TryEvaluate("and(true(),1)", false);
        assertEquals(true, t);

        t = engine.TryEvaluate("and(true(),false(),1=1)", true);
        assertEquals(false, t);
        t = engine.TryEvaluate("and(true(),false(),1)", true);
        assertEquals(false, t);

        t = engine.TryEvaluate("and(true(),0)", true);
        assertEquals(false, t);
    }

    @Test
    public void Or_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean t = engine.TryEvaluate("or(true(),1=1)", false);
        assertEquals(true, t);
        t = engine.TryEvaluate("or(true(),1)", false);
        assertEquals(true, t);

        t = engine.TryEvaluate("or(true(),false(),1=1)", false);
        assertEquals(true, t);
        t = engine.TryEvaluate("or(true(),false(),1)", false);
        assertEquals(true, t);

        t = engine.TryEvaluate("or(true(),0)", false);
        assertEquals(true, t);

        t = engine.TryEvaluate("or(false(),1=2)", true);
        assertEquals(false, t);
    }

    @Test
    public void Not_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean t = engine.TryEvaluate("not(true())", true);
        assertEquals(false, t);
        t = engine.TryEvaluate("not(false())", false);
        assertEquals(true, t);
    }

    @Test
    public void true_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean t = engine.TryEvaluate("true()", false);
        assertEquals(true, t);
    }

    @Test
    public void false_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean t = engine.TryEvaluate("false()", true);
        assertEquals(false, t);
    }

    @Test
    public void andor_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean t = engine.TryEvaluate("1=1 && 2==2 && 3=3", false);
        assertEquals(true, t);

        t = engine.TryEvaluate("1=1 && 2!=2", true);
        assertEquals(false, t);

        t = engine.TryEvaluate("1=1 || 2!=2", false);
        assertEquals(true, t);
    }

    @Test
    public void IFS_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("IFS(1=1, 'a', 1=2, 'b')", "");
        assertEquals("a", t);

        t = engine.TryEvaluate("IFS(1=2, 'a', 2=2, 'b')", "");
        assertEquals("b", t);

        int t2 = engine.TryEvaluate("IFS(1=1, 100, 1=2, 200)", 0);
        assertEquals(100, t2);
    }

    @Test
    public void SWITCH_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("SWITCH(1, 1, 'one', 2, 'two')", "");
        assertEquals("one", t);

        t = engine.TryEvaluate("SWITCH(2, 1, 'one', 2, 'two')", "");
        assertEquals("two", t);

        int t2 = engine.TryEvaluate("SWITCH('a', 'a', 1, 'b', 2)", 0);
        assertEquals(1, t2);

        t = engine.TryEvaluate("SWITCH(5, 1, 'one', 2, 'two')", "");
        assertEquals("", t);
    }

    @Test
    public void SWITCH_ExcelCompatible_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        // 有默认值的情况
        String t = engine.TryEvaluate("SWITCH(3, 1, 'one', 2, 'two', 'unknown')", "");
        assertEquals("unknown", t);

        t = engine.TryEvaluate("SWITCH(1, 1, 'one', 2, 'two', 'unknown')", "");
        assertEquals("one", t);

        // 数字类型默认值
        int t2 = engine.TryEvaluate("SWITCH(5, 1, 100, 2, 200, 0)", 0);
        assertEquals(0, t2);

        // 多个 value/result 对 + 默认值
        t = engine.TryEvaluate("SWITCH(4, 1, 'A', 2, 'B', 3, 'C', 'D')", "");
        assertEquals("D", t);

        t = engine.TryEvaluate("SWITCH(3, 1, 'A', 2, 'B', 3, 'C', 'D')", "");
        assertEquals("C", t);

        // 字符串匹配 + 默认值
        int t3 = engine.TryEvaluate("SWITCH('x', 'a', 1, 'b', 2, -1)", 0);
        assertEquals(-1, t3);
    }

    @Test
    public void XOR_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean t = engine.TryEvaluate("XOR(true(), false())", false);
        assertEquals(true, t);

        t = engine.TryEvaluate("XOR(true(), true())", true);
        assertEquals(false, t);

        t = engine.TryEvaluate("XOR(false(), false())", true);
        assertEquals(false, t);

        t = engine.TryEvaluate("XOR(true(), true(), true())", false);
        assertEquals(true, t);

        t = engine.TryEvaluate("XOR(true(), false(), false())", false);
        assertEquals(true, t);

        t = engine.TryEvaluate("XOR(1, 0)", false);
        assertEquals(true, t);

        t = engine.TryEvaluate("XOR(1, 1)", true);
        assertEquals(false, t);
    }

    @Test
    public void MethodStyle_ISNUMBER_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean t = engine.TryEvaluate("123.ISNUMBER()", false);
        assertEquals(t, true);

        t = engine.TryEvaluate("'abc'.ISNUMBER()", true);
        assertEquals(t, false);

        t = engine.TryEvaluate("true.ISNUMBER()", true);
        assertEquals(t, false);
    }

    @Test
    public void MethodStyle_ISTEXT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean t = engine.TryEvaluate("'abc'.ISTEXT()", false);
        assertEquals(t, true);

        t = engine.TryEvaluate("123.ISTEXT()", true);
        assertEquals(t, false);
    }

    @Test
    public void MethodStyle_ISNONTEXT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean t = engine.TryEvaluate("123.ISNONTEXT()", false);
        assertEquals(t, true);

        t = engine.TryEvaluate("'abc'.ISNONTEXT()", true);
        assertEquals(t, false);
    }

    @Test
    public void MethodStyle_ISLOGICAL_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean t = engine.TryEvaluate("true.ISLOGICAL()", false);
        assertEquals(t, true);

        t = engine.TryEvaluate("false.ISLOGICAL()", false);
        assertEquals(t, true);

        t = engine.TryEvaluate("123.ISLOGICAL()", true);
        assertEquals(t, false);
    }

    @Test
    public void MethodStyle_ISNULL_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean t = engine.TryEvaluate("null.ISNULL()", false);
        assertEquals(t, true);

        t = engine.TryEvaluate("123.ISNULL()", true);
        assertEquals(t, false);
    }

    @Test
    public void MethodStyle_ISNULLORERROR_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean t = engine.TryEvaluate("null.ISNULLORERROR()", false);
        assertEquals(t, true);
    }
}
