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

            DiyNameInfo p2 = AlgorithmEngineHelper.GetDiyNames("{ddd}");
            Assert.AreEqual("ddd", p2.Parameters[0]);

            DiyNameInfo p3 = AlgorithmEngineHelper.GetDiyNames("【dd】");
            Assert.AreEqual("dd", p3.Parameters[0]);

            DiyNameInfo p4 = AlgorithmEngineHelper.GetDiyNames("@ddd+2");
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



    }
}
