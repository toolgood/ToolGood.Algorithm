using PetaTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolGood.Algorithm;


namespace ToolGood.Algorithm2.Test.AlgorithmEngineHelper2
{
    [TestFixture]
    public class AlgorithmEngineHelperTest
    {
        [Test]
        public void Test()
        {
            DiyNameInfo p = AlgorithmEngineHelper.GetDiyNames("[dd]");
            Assert.AreEqual("dd", p.Parameters[0]);
            p = AlgorithmEngineHelper.GetDiyNames("@dd");
            Assert.AreEqual("dd", p.Parameters[0]);
            p = AlgorithmEngineHelper.GetDiyNames("#dd#");
            Assert.AreEqual("dd", p.Parameters[0]);
            p = AlgorithmEngineHelper.GetDiyNames("dd");
            Assert.AreEqual("dd", p.Parameters[0]);

            // 注，这里的 ddd 是数组内有 ddd
            DiyNameInfo p2 = AlgorithmEngineHelper.GetDiyNames("{ddd}");
            Assert.AreEqual("ddd", p2.Parameters[0]);

            DiyNameInfo p3 = AlgorithmEngineHelper.GetDiyNames("【dd】");
            Assert.AreEqual("dd", p3.Parameters[0]);
            p3 = AlgorithmEngineHelper.GetDiyNames("【dd.1】");
            Assert.AreEqual("dd.1", p3.Parameters[0]);

            DiyNameInfo p4 = AlgorithmEngineHelper.GetDiyNames("@ddd+2");
            Assert.AreEqual("ddd", p4.Parameters[0]);
            p4 = AlgorithmEngineHelper.GetDiyNames("ddd+2");
            Assert.AreEqual("ddd", p4.Parameters[0]);

            DiyNameInfo p5 = AlgorithmEngineHelper.GetDiyNames("ddd(d1,22)");
            Assert.AreEqual("ddd", p5.Functions[0]);
            Assert.AreEqual("d1", p5.Parameters[0]);

            DiyNameInfo p6 = AlgorithmEngineHelper.GetDiyNames("长");
            Assert.AreEqual("长", p6.Parameters[0]);

            DiyNameInfo p7 = AlgorithmEngineHelper.GetDiyNames("#ddd#+2");
            Assert.AreEqual("ddd", p7.Parameters[0]);

        }

        [Test]
        public void Test2()
        {
            var b = AlgorithmEngineHelper.IsKeywords("false");
            Assert.IsTrue(b);


        }

        [Test]
        public void Test3()
        {
            var b = AlgorithmEngineHelper.UnitConversion(1M, "米", "千米", "测试");
            Assert.AreEqual(0.001M, b);
            b = AlgorithmEngineHelper.UnitConversion(1M, "米", "分米", "测试");
            Assert.AreEqual(10M, b);
            b = AlgorithmEngineHelper.UnitConversion(1M, "米", "厘米", "测试");
            Assert.AreEqual(100M, b);
            b = AlgorithmEngineHelper.UnitConversion(1M, "米", "mm", "测试");
            Assert.AreEqual(1000M, b);


            b = AlgorithmEngineHelper.UnitConversion(1M, "m2", "dm2", "测试");
            Assert.AreEqual(100M, b);
            b = AlgorithmEngineHelper.UnitConversion(1M, "m2", "cm2", "测试");
            Assert.AreEqual(10000M, b);
            b = AlgorithmEngineHelper.UnitConversion(1M, "m2", "mm2", "测试");
            Assert.AreEqual(1000000M, b);


            b = AlgorithmEngineHelper.UnitConversion(1M, "m3", "dm3", "测试");
            Assert.AreEqual(1000M, b);
            b = AlgorithmEngineHelper.UnitConversion(1M, "m3", "cm3", "测试");
            Assert.AreEqual(1000000M, b);
            b = AlgorithmEngineHelper.UnitConversion(1M, "m3", "mm3", "测试");
            Assert.AreEqual(1000000000M, b);


            b = AlgorithmEngineHelper.UnitConversion(1M, "t", "kg", "测试");
            Assert.AreEqual(1000M, b);
            b = AlgorithmEngineHelper.UnitConversion(1M, "t", "g", "测试");
            Assert.AreEqual(1000000M, b);

        }
        [Test]
        public void Test4()
        {
            var progContext = AlgorithmEngineHelper.Parse("1+1");
            var operand = AlgorithmEngineHelper.Evaluate(progContext);
            Assert.AreEqual(2, operand.IntValue);
        }



    }
}
