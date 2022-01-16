package toolgood.algorithm.Test2;

import static org.junit.Assert.assertEquals;

import org.junit.Test;

import toolgood.algorithm.ConditionCache;

public class AlgorithmEngineExTest {
    @Test
    public void Test() {
        ConditionCache multiConditionCache = new ConditionCache();
        multiConditionCache.AddFormula("桌面积", "[圆桌]", "[半径]*[半径]*pi()");
        multiConditionCache.AddFormula("桌面积", "[方桌]", "[长]*[宽]");

        multiConditionCache.AddFormula("价格", "[圆桌]&& [半径]<2.5", "[桌面积]*1.3");
        multiConditionCache.AddFormula("价格", "[圆桌]&& [半径]<5", "[桌面积]*1.5");
        multiConditionCache.AddFormula("价格", "[圆桌]&& [半径]<7", "[桌面积]*2");
        multiConditionCache.AddFormula("价格", "[圆桌]", "[桌面积]*2.5");

        multiConditionCache.AddFormula("价格", "[方桌]&& [长]<1.3", "[桌面积]*1.3+[高]*1.1");
        multiConditionCache.AddFormula("价格", "[方桌]&& [长]<2", "[桌面积]*1.5+[高]*1.1");
        multiConditionCache.AddFormula("价格", "[方桌]&& [长]<5", "[桌面积]*2+[高]*1.1");
        multiConditionCache.AddFormula("价格", "[方桌]&& [长]<7", "[桌面积]*2.5");

        multiConditionCache.AddFormula("出错了", "[方桌]&& [长]<11", "[出错]*2.5");

        Desk desk = new Desk();
        desk.IsRoundTable = true;
        desk.Radius = 3;

        PriceAlgorithm priceAlgorithm = new PriceAlgorithm(multiConditionCache, desk);
        Double p1 = priceAlgorithm.TryEvaluate("价格", 0.0);
        assertEquals(3 * 3 * Math.PI * 1.5, p1, 0.0001);

        Desk desk2 = new Desk();
        desk2.IsRoundTable = false;
        desk2.Length = 3;
        desk2.Width = 1.3;
        desk2.Heigth = 1;
        priceAlgorithm = new PriceAlgorithm(multiConditionCache, desk2);
        Double p2 = priceAlgorithm.TryEvaluate("价格", 0.0);
        assertEquals(3 * 1.3 * 2 + 1 * 1.1, p2, 0.0001);

        Desk desk3 = new Desk();
        desk3.IsRoundTable = false;
        desk3.Length = 9;
        desk3.Width = 1.3;
        desk3.Heigth = 1;

        priceAlgorithm = new PriceAlgorithm(multiConditionCache, desk3);
        Double p3 = priceAlgorithm.TryEvaluate("价格", 0.0);
        assertEquals(0, p3, 0.001);
        assertEquals("CategoryName [价格] is missing.", priceAlgorithm.LastError);

        Double p4 = priceAlgorithm.TryEvaluate("出错了", 0.0);
    }

    @Test
    public void Test2() {
        ConditionCache multiConditionCache = new ConditionCache();
        multiConditionCache.LazyLoad = true;
        multiConditionCache.AddFormula("桌面积", "[圆桌]", "[半径]*[半径]*pi()");
        multiConditionCache.AddFormula("桌面积", "[方桌]", "[长]*[宽]");

        multiConditionCache.AddFormula("价格", "[圆桌]&& [半径]<2.5", "[桌面积]*1.3");
        multiConditionCache.AddFormula("价格", "[圆桌]&& [半径]<5", "[桌面积]*1.5");
        multiConditionCache.AddFormula("价格", "[圆桌]&& [半径]<7", "[桌面积]*2");
        multiConditionCache.AddFormula("价格", "[圆桌]", "[桌面积]*2.5");

        multiConditionCache.AddFormula("价格", "[方桌]&& [长]<1.3", "[桌面积]*1.3+[高]*1.1");
        multiConditionCache.AddFormula("价格", "[方桌]&& [长]<2", "[桌面积]*1.5+[高]*1.1");
        multiConditionCache.AddFormula("价格", "[方桌]&& [长]<5", "[桌面积]*2+[高]*1.1");
        multiConditionCache.AddFormula("价格", "[方桌]&& [长]<7", "[桌面积]*2.5");

        multiConditionCache.AddFormula("出错了", "[方桌]&& [长]<11", "[出错]*2.5");

        Desk desk = new Desk();
        desk.IsRoundTable = true;
        desk.Radius = 3;

        PriceAlgorithm priceAlgorithm = new PriceAlgorithm(multiConditionCache, desk);
        Double p1 = priceAlgorithm.TryEvaluate("价格", 0.0);
        assertEquals(3 * 3 * Math.PI * 1.5, p1, 0.0001);

        Desk desk2 = new Desk();
        desk2.IsRoundTable = false;
        desk2.Length = 3;
        desk2.Width = 1.3;
        desk2.Heigth = 1;
        priceAlgorithm = new PriceAlgorithm(multiConditionCache, desk2);
        Double p2 = priceAlgorithm.TryEvaluate("价格", 0.0);
        assertEquals(3 * 1.3 * 2 + 1 * 1.1, p2, 0.0001);

        Desk desk3 = new Desk();
        desk3.IsRoundTable = false;
        desk3.Length = 9;
        desk3.Width = 1.3;
        desk3.Heigth = 1;

        priceAlgorithm = new PriceAlgorithm(multiConditionCache, desk3);
        Double p3 = priceAlgorithm.TryEvaluate("价格", 0.0);
        assertEquals(0, p3, 0.001);
        assertEquals("CategoryName [价格] is missing.", priceAlgorithm.LastError);

        Double p4 = priceAlgorithm.TryEvaluate("出错了", 0.0);

    }

    @Test
    public void Test3() throws Exception {
        ConditionCache multiConditionCache = new ConditionCache();
        multiConditionCache.AddCondition("类型", "[方桌]&& [长]<1.3", "1");
        multiConditionCache.AddCondition("类型", "[方桌]&& [长]<2", "2");
        multiConditionCache.AddCondition("类型", "[方桌]&& [长]<5", "3");
        multiConditionCache.AddCondition("类型", "[方桌]&& [长]<7", "4");

        Desk desk = new Desk();
        desk.IsRoundTable = false;
        desk.Length = 3;
        desk.Width = 1.3;
        desk.Heigth = 1;

        PriceAlgorithm priceAlgorithm = new PriceAlgorithm(multiConditionCache, desk);
        String p1 = priceAlgorithm.SearchRemark("类型");
        assertEquals("3", p1);
    }
}
