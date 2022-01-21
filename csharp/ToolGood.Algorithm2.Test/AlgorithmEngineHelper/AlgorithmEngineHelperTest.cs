using PetaTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolGood.Algorithm;

namespace ToolGood.Algorithm2.Test.AlgorithmEngineHelper
{
    [TestFixture]
    public class AlgorithmEngineHelperTest
    {
        [Test]
        public void Test()
        {
            var helper=new ToolGood.Algorithm.AlgorithmEngineHelper();
            DiyNameInfo p = helper.GetDiyNames("[dd]");
            Assert.AreEqual("dd", p.Parameters[0]);

            DiyNameInfo p2 = helper.GetDiyNames("{ddd}");
            Assert.AreEqual("ddd", p2.Parameters[0]);

            DiyNameInfo p3 = helper.GetDiyNames("【dd】");
            Assert.AreEqual("dd", p3.Parameters[0]);

            DiyNameInfo p4 = helper.GetDiyNames("@ddd+2");
            Assert.AreEqual("ddd", p4.Parameters[0]);

            DiyNameInfo p5 = helper.GetDiyNames("ddd(d1,22)");
            Assert.AreEqual("ddd", p5.Functions[0]);
            Assert.AreEqual("d1", p5.Parameters[0]);

            DiyNameInfo p6 = helper.GetDiyNames("长");
            Assert.AreEqual("长", p6.Parameters[0]);

            DiyNameInfo p7 = helper.GetDiyNames("#ddd#+2");
            Assert.AreEqual("ddd", p7.Parameters[0]);

        }

        [Test]
        public void Test2()
        {
            var helper = new ToolGood.Algorithm.AlgorithmEngineHelper();
            var b = helper.IsKeywords("true");
            Assert.IsTrue(b);


        }



    }
}
