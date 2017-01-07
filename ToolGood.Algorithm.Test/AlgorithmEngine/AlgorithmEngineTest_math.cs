using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaTest;

namespace ToolGood.Algorithm
{
    [TestFixture]
    class AlgorithmEngineTest_math
    {
        #region 基础数学
        [Test]
        public void Pi_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("pi()", 0.0);
            Assert.AreEqual(3.141592654, Math.Round(t, 9));
        }




        #endregion


    }
}
