using PetaTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolGood.Algorithm;

namespace ToolGood.Algorithm2.Test.AlgorithmEngineEx
{
    [TestFixture]
    public class AlgorithmEngineExTest
    {
        [Test]
        public void Test()
        {
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


            Desk desk = new Desk() {
                IsRoundTable = true,
                Radius = 3
            };
            PriceAlgorithm priceAlgorithm = new PriceAlgorithm(multiConditionCache, desk);
            var p1 = priceAlgorithm.TryEvaluate("价格", 0.0);
            Assert.AreEqual(3 * 3 * Math.PI * 1.5, p1, 0.0001);

            Desk desk2 = new Desk() {
                IsRoundTable = false,
                Length = 3,
                Width = 1.3,
                Heigth = 1
            };
            priceAlgorithm = new PriceAlgorithm(multiConditionCache, desk2);
            var p2 = priceAlgorithm.TryEvaluate("价格", 0.0);
            Assert.AreEqual(3 * 1.3 * 2 + 1 * 1.1, p2, 0.0001);


            Desk desk3 = new Desk() {
                IsRoundTable = false,
                Length = 9,
                Width = 1.3,
                Heigth = 1
            };
            priceAlgorithm = new PriceAlgorithm(multiConditionCache, desk3);
            var p3 = priceAlgorithm.TryEvaluate("价格", 0.0);
            Assert.AreEqual(0, p3, 0.001);
            Assert.AreEqual("CategoryName [价格] is missing.", priceAlgorithm.LastError);


            var p4 = priceAlgorithm.TryEvaluate("出错了", 0.0);


            var tt = priceAlgorithm.EvaluateFormula("长-宽-高", '-');
            Assert.AreEqual("9-1.3-1", tt);
        }
        [Test]
        public void Test2()
        {
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


            Desk desk = new Desk() {
                IsRoundTable = true,
                Radius = 3
            };
            PriceAlgorithm priceAlgorithm = new PriceAlgorithm(multiConditionCache, desk);
            var p1 = priceAlgorithm.TryEvaluate("价格", 0.0);
            Assert.AreEqual(3 * 3 * Math.PI * 1.5, p1, 0.0001);

            Desk desk2 = new Desk() {
                IsRoundTable = false,
                Length = 3,
                Width = 1.3,
                Heigth = 1
            };
            priceAlgorithm = new PriceAlgorithm(multiConditionCache, desk2);
            var p2 = priceAlgorithm.TryEvaluate("价格", 0.0);
            Assert.AreEqual(3 * 1.3 * 2 + 1 * 1.1, p2, 0.0001);


            Desk desk3 = new Desk() {
                IsRoundTable = false,
                Length = 9,
                Width = 1.3,
                Heigth = 1
            };
            priceAlgorithm = new PriceAlgorithm(multiConditionCache, desk3);
            var p3 = priceAlgorithm.TryEvaluate("价格", 0.0);
            Assert.AreEqual(0, p3, 0.001);
            Assert.AreEqual("CategoryName [价格] is missing.", priceAlgorithm.LastError);


            var p4 = priceAlgorithm.TryEvaluate("出错了", 0.0);

        }
        [Test]
        public void Test3()
        {
            ConditionCache multiConditionCache = new ConditionCache();
            multiConditionCache.AddCondition("类型", "[方桌]&& [长]<1.3", "1");
            multiConditionCache.AddCondition("类型", "[方桌]&& [长]<2", "2");
            multiConditionCache.AddCondition("类型", "[方桌]&& [长]<5", "3");
            multiConditionCache.AddCondition("类型", "[方桌]&& [长]<7", "4");

            Desk desk = new Desk() {
                IsRoundTable = false,
                Length = 3,
                Width = 1.3,
                Heigth = 1
            };
            PriceAlgorithm priceAlgorithm = new PriceAlgorithm(multiConditionCache, desk);
            var p1 = priceAlgorithm.SearchRemark("类型");
            Assert.AreEqual("3", p1);
        }

        [Test]
        public void Test4()
        {
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

            var algoEngine = new ToolGood.Algorithm.AlgorithmEngineEx(multiConditionCache);
            algoEngine.JumpConditionError = true;
            algoEngine.AddParameter("方桌", true);
            algoEngine.AddParameter("长", 3);
            algoEngine.AddParameter("宽", 1.3);
            algoEngine.AddParameter("高", 1);

            var p2 = algoEngine.TryEvaluate("价格", 0.0);
            Assert.AreEqual(3 * 1.3 * 2 + 1 * 1.1, p2, 0.0001);

        }

    }

}
