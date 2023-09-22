package toolgood.algorithm.Tests3;

import static org.junit.Assert.assertEquals;

import org.junit.Test;

import toolgood.algorithm.AlgorithmEngineHelper;
import toolgood.algorithm.DiyNameInfo;

public class AlgorithmEngineHelperTest {

    @Test
    public void Test() throws Exception {
        DiyNameInfo p = AlgorithmEngineHelper.GetDiyNames("[dd]");
        assertEquals("dd", p.Parameters.get(0));
        p = AlgorithmEngineHelper.GetDiyNames("@dd");
        assertEquals("dd", p.Parameters.get(0));
        p = AlgorithmEngineHelper.GetDiyNames("#dd#");
        assertEquals("dd", p.Parameters.get(0));
        p = AlgorithmEngineHelper.GetDiyNames("dd");
        assertEquals("dd", p.Parameters.get(0));

        // 注，这里的 ddd 是数组内有 ddd
        DiyNameInfo p2 = AlgorithmEngineHelper.GetDiyNames("{ddd}");
        assertEquals("ddd", p2.Parameters.get(0));

        DiyNameInfo p3 = AlgorithmEngineHelper.GetDiyNames("【dd】");
        assertEquals("dd", p3.Parameters.get(0));

        DiyNameInfo p4 = AlgorithmEngineHelper.GetDiyNames("@ddd+2");
        assertEquals("ddd", p4.Parameters.get(0));

        p4 = AlgorithmEngineHelper.GetDiyNames("ddd+2");
        assertEquals("ddd", p4.Parameters.get(0));

        DiyNameInfo p5 = AlgorithmEngineHelper.GetDiyNames("ddd(d1,22)");
        assertEquals("ddd", p5.Functions.get(0));
        assertEquals("d1", p5.Parameters.get(0));

        DiyNameInfo p6 = AlgorithmEngineHelper.GetDiyNames("长");
        assertEquals("长", p6.Parameters.get(0));

        DiyNameInfo p7 = AlgorithmEngineHelper.GetDiyNames("#ddd#+2");
        assertEquals("ddd", p7.Parameters.get(0));

    }

    @Test
    public void Test2() {
        boolean b = AlgorithmEngineHelper.IsKeywords("true");
        assertEquals(true, b);

    }
}
