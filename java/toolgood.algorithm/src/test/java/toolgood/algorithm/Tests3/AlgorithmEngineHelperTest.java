package toolgood.algorithm.Tests3;

import static org.junit.Assert.assertEquals;

import org.junit.Test;

import toolgood.algorithm.AlgorithmEngineHelper;
import toolgood.algorithm.DiyNameInfo;

public class AlgorithmEngineHelperTest {

    @Test
    public void Test() throws Exception {
        AlgorithmEngineHelper helper = new AlgorithmEngineHelper();
        DiyNameInfo p = helper.GetDiyNames("[dd]");
        assertEquals("dd", p.Parameters.get(0));

        DiyNameInfo p2 = helper.GetDiyNames("{ddd}");
        assertEquals("ddd", p2.Parameters.get(0));

        DiyNameInfo p3 = helper.GetDiyNames("【dd】");
        assertEquals("dd", p3.Parameters.get(0));

        DiyNameInfo p4 = helper.GetDiyNames("@ddd+2");
        assertEquals("ddd", p4.Parameters.get(0));

        DiyNameInfo p5 = helper.GetDiyNames("ddd(d1,22)");
        assertEquals("ddd", p5.Functions.get(0));
        assertEquals("d1", p5.Parameters.get(0));

        DiyNameInfo p6 = helper.GetDiyNames("长");
        assertEquals("长", p6.Parameters.get(0));

        DiyNameInfo p7 = helper.GetDiyNames("#ddd#+2");
        assertEquals("ddd", p7.Parameters.get(0));

    }

    @Test
    public void Test2() {
        AlgorithmEngineHelper helper = new AlgorithmEngineHelper();
        boolean b = helper.IsKeywords("true");
        assertEquals(true, b);

    }
}
