package toolgood.algorithm.Tests;

import static org.junit.Assert.assertEquals;

import org.joda.time.DateTime;
import org.junit.Test;

import toolgood.algorithm.AlgorithmEngine;

@SuppressWarnings("deprecation")
public class AlgorithmEngineTest {
    
    @Test
    public void Test() throws Exception
    {
        AlgorithmEngine engine = new AlgorithmEngine();

        double t = 0.0;
        if (engine.Parse("1+2")) {
            t = (double)engine.Evaluate().NumberValue();
        }

        int c = engine.TryEvaluate("2+3", 0);
        assertEquals(5, c);
        c = engine.TryEvaluate("(2)+3", 0);
        assertEquals(5, c);
        c = engine.TryEvaluate("2+3*2+10/2*4", 0);
        assertEquals(28, c);

        c = engine.TryEvaluate("if(2+3*2+10/2*4,1", 0);
        assertEquals(0, c);



        double e = engine.TryEvaluate("e", 0.0);
        assertEquals(Math.E, e);
        e = engine.TryEvaluate("pi", 0.0);
        assertEquals(Math.PI, e);

        boolean b = engine.TryEvaluate("true", true);
        assertEquals(true, b);
        b = engine.TryEvaluate("false", false);
        assertEquals(false, b);

        int b1 = engine.TryEvaluate("if(true,1,2)", 0);
        assertEquals(1, b1);

        b1 = engine.TryEvaluate("if(false,1,2)", 0);
        assertEquals(2, b1);

        double b2 = engine.TryEvaluate("pi*4", 0.0);
        assertEquals(Math.PI * 4, b2);
        b2 = engine.TryEvaluate("e*4", 0.0);
        assertEquals(Math.E * 4, b2);

        String s = engine.TryEvaluate("'aa'&'bb'", "");
        assertEquals("aabb", s);

        s = engine.TryEvaluate("'3'+2", "");
        assertEquals("5", s);

        int r = engine.TryEvaluate("count({ 1,2,3,4})", 0);
        assertEquals(4, r);


        r = engine.TryEvaluate("(1=1)*9+2", 0);
        assertEquals(11, r); ;
        r = engine.TryEvaluate("(1=2)*9+2", 0);
        assertEquals(2, r); ;

        DateTime dt = engine.TryEvaluate("'2016-1-1'+1", DateTime.now());
        assertEquals(DateTime.parse("2016-1-2"), dt); ;
        dt = engine.TryEvaluate("'2016-1-1'+9*'1:0'", DateTime.now());
        assertEquals(DateTime.parse("2016-1-1 9:0"), dt); ;

        boolean value = engine.TryEvaluate("1 > (-2)", false);
        assertEquals(value, true);


        value = engine.TryEvaluate("(-1) > (-2）", false);
        assertEquals(value, true);


        value = engine.TryEvaluate("-1 > (-2)", false);
        assertEquals(value, true);

        value = engine.TryEvaluate("-1 > -2", false);
        assertEquals(value, true);

        boolean value2 = engine.TryEvaluate("-1 > -2", false);
        assertEquals(value2, true);


        boolean value3 = engine.TryEvaluate("-7 < -2", false);
        assertEquals(value3, true);
    }

    @Test
    public void base_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        int t = engine.TryEvaluate("1+(3*2+2)/2", 0);
        assertEquals(5, t);

        t = engine.TryEvaluate("(8-3)*(3+2)", 0);
        assertEquals(25, t);

        t = engine.TryEvaluate("(8-3)*(3+2) % 7", 0);
        assertEquals(4, t);

        boolean b = engine.TryEvaluate("1=1", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1=2", false);
        assertEquals(false, b);

        b = engine.TryEvaluate("1<>2", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1!=2", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1>2", false);
        assertEquals(false, b);

        b = engine.TryEvaluate("1<2", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1<=2", false);
        assertEquals(true, b);

        b = engine.TryEvaluate("1>=2", false);
        assertEquals(false, b);

    }
    @Test
    public void base_test2()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("1+(3*2+2)/2 & '11' & '11:20'*9 & isnumber(22)*3", "");
    }

}