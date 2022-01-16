package toolgood.algorithm.Tests;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertNotEquals;

import java.math.BigDecimal;

import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;
import org.junit.Test;

import toolgood.algorithm.AlgorithmEngine;

@SuppressWarnings("deprecation")
public class AlgorithmEngineTest {
    
    @Test
    public void Test() throws Exception
    {
        AlgorithmEngine engine = new AlgorithmEngine();

        if (engine.Parse("1+2")) {
            double t = (double)engine.Evaluate().NumberValue();
        }

        int c = engine.TryEvaluate("2+3", 0);
        assertEquals(5, c);
        c = engine.TryEvaluate("(2)+3", 0);
        assertEquals(5, c);
        c = engine.TryEvaluate("2+3*2+10/2*4", 0);
        assertEquals(28, c);

        c = engine.TryEvaluate("if(2+3*2+10/2*4,1", 0);
        assertEquals(0, c);

        c = engine.TryEvaluate("2.1e3 + 10", 0);
        assertEquals(2110, c);

        c = engine.TryEvaluate("2.1e+03 + 10", 0);
        assertEquals(2110, c);

        c = engine.TryEvaluate("2.1e+3 + 10", 0);
        assertEquals(2110, c);

        double d = engine.TryEvaluate("2.1e-3 + 10", 0.0);
        assertEquals(10.0021, d,4);


        double e = engine.TryEvaluate("e", 0.0);
        assertEquals(Math.E-e,0.0,0.01);
        e = engine.TryEvaluate("pi", 0.0);
        assertEquals(Math.PI-e,0.0,0.01);

        boolean b = engine.TryEvaluate("true", true);
        assertEquals(true, b);
        b = engine.TryEvaluate("false", false);
        assertEquals(false, b);

        int b1 = engine.TryEvaluate("if(true,1,2)", 0);
        assertEquals(1, b1);

        b1 = engine.TryEvaluate("if(false,1,2)", 0);
        assertEquals(2, b1);

        double b2 = engine.TryEvaluate("pi*4", 0.0);
        assertEquals(Math.PI * 4, b2,0.01);
        b2 = engine.TryEvaluate("e*4", 0.0);
        assertEquals(Math.E * 4, b2,0.01);

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

        DateTime dt = engine.TryEvaluate("'2016-1-1'+1", DateTime.now());
        assertEquals(new DateTime(2016,1,2,0,0,0,DateTimeZone.UTC), dt); 
        dt = engine.TryEvaluate("'2016-1-1'+9*'1:0'", DateTime.now());
        assertEquals(new DateTime(2016,1,1,9,0,0,DateTimeZone.UTC), dt); 
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
        assertNotEquals(t, "");
    }

    
    @Test
    public void Cylinder_Test() throws Exception
    {
        Cylinder c = new Cylinder(3, 10);
        double t = c.TryEvaluate("[半径]*[半径]*pi()", 0.0);      //圆底面积

        double t2 = c.TryEvaluate("半径*半径*pi()", 0.0);      //圆底面积
        double t3 = c.TryEvaluate("{半径}*{半径}*pi()", 0.0);      //圆底面积
        double t4 = c.TryEvaluate("@半径*@半径*pi()", 0.0);      //圆底面积
        double t5 = c.TryEvaluate("#半径#*#半径#*pi()", 0.0);      //圆底面积
        double t6 = c.TryEvaluate("【半径】*【半径】*pi()", 0.0);      //圆底面积
        double t7 = c.TryEvaluate("【半径】*【半径】*pi（）", 0.0);      //圆底面积

        assertEquals(t, t2,0.001);
        assertEquals(t, t3,0.001);
        assertEquals(t, t4,0.001);
        assertEquals(t, t5,0.001);
        assertEquals(t, t6,0.001);
        assertEquals(t, t7,0.001);

        assertEquals(3 * 3 * Math.PI, t,0.001);
        t = c.TryEvaluate("[直径]*pi()", 0.0);            //圆的长
        assertEquals(2 * 3 * Math.PI, t,0.001);
        t = c.TryEvaluate("[半径]*[半径]*pi()*[高]", 0.0); //圆的体积
        assertEquals(3 * 3 * Math.PI*10, t,0.001);

        if (c.Parse("[直径1]*pi()") == false) {
            assertEquals("参数[直径1]无效!", c.LastError);
        }

        t = c.TryEvaluate("['半径']*[半径]*pi()*[高]", 0.0); //圆的体积
        assertEquals(3 * 3 * Math.PI*10, t,0.001);

        t = c.TryEvaluate("求面积（10）", 0.0); //圆的体积
        assertEquals(10 * 10 * Math.PI, t,0.001);

        String json = "{'灰色':'L','canBookCount':905,'saleCount':91,'specId':'43b0e72e98731aed69e1f0cc7d64bf4d'}";
        c.AddParameterFromJson(json);

        String tt = c.TryEvaluate("['灰色']", ""); //圆的体积
        assertEquals("L", tt);

    }


    private double round(final double value, final int p) {
        final BigDecimal bigD = new BigDecimal(value);
        return bigD.setScale(p, BigDecimal.ROUND_HALF_UP).doubleValue();
    }
}