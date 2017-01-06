using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaTest;

namespace ToolGood.Algorithm
{
    [TestFixture]
    class AlgorithmEngineTest
    {
        #region flow
        [Test]
        public void flow()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("if(1=1,1,2)", 0);
            Assert.AreEqual(1, t);
             t = engine.TryEvaluate("if(3,1,2)", 0);
            Assert.AreEqual(1, t);

            t = engine.TryEvaluate("if(true(),1,2)", 0);
            Assert.AreEqual(1, t);

            t = engine.TryEvaluate("if(false(),1,2)", 0);
            Assert.AreEqual(2, t);

            t = engine.TryEvaluate("iferror(1/0,1,2)", 0);
            Assert.AreEqual(1, t);

            t = engine.TryEvaluate("ifnumber(4,1,2)", 0);
            Assert.AreEqual(1, t);

            t = engine.TryEvaluate("IFTEXT('11',1,2)", 0);
            Assert.AreEqual(1, t);
            t = engine.TryEvaluate("IFTEXT(11,1,2)", 0);
            Assert.AreEqual(2, t);

            var b = engine.TryEvaluate("ISNUMBER(1)", false);
            Assert.AreEqual(true, b);
            b = engine.TryEvaluate("istext('1')", false);
            Assert.AreEqual(true, b);

            b = engine.TryEvaluate("and(1,2=2)", false);
            Assert.AreEqual(true, b);

            b = engine.TryEvaluate("or(1,2=3)", false);
            Assert.AreEqual(true, b);

            b = engine.TryEvaluate("not(true())", true);
            Assert.AreEqual(false, b);
        }


        #endregion


        [Test]

        public void Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            double t = 0.0;
            if (engine.Parse("1+2")) {
                t = (double)engine.Evaluate();
            }
            Assert.AreEqual(3.0, t);

            var c = engine.TryEvaluate("2+3", 0);
            Assert.AreEqual(5, c);






        }



    }
}
