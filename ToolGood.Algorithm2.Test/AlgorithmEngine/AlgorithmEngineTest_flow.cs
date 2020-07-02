using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaTest;

namespace ToolGood.Algorithm
{
    [TestFixture]
    class AlgorithmEngineTest_flow
    {
        [Test]
        public void If_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("if(1=1,1,2)", 0);
            Assert.AreEqual(1, t);
            t = engine.TryEvaluate("if(1=1,1)", 0);
            Assert.AreEqual(1, t);

            t = engine.TryEvaluate("if(3,1,2)", 0);
            Assert.AreEqual(1, t);
            t = engine.TryEvaluate("if('1',1,2)", 0);
            Assert.AreEqual(1, t);
            t = engine.TryEvaluate("if(0,1,2)", 0);
            Assert.AreEqual(2, t);
        }

        [Test]
        public void iferror_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("iferror(1/0,1,2)", 0);
            Assert.AreEqual(1, t);

            t = engine.TryEvaluate("iferror(1-'rrr',1,2)", 0);
            Assert.AreEqual(1, t);
        }

        [Test]
        public void IFNUMBER_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("IFNUMBER(1,1,2)", 0);
            Assert.AreEqual(1, t);

            t = engine.TryEvaluate("IFNUMBER('e',1,2)", 0);
            Assert.AreEqual(2, t);

            t = engine.TryEvaluate("IFNUMBER('11',1,2)", 0);
            Assert.AreEqual(2, t);

            t = engine.TryEvaluate("IFNUMBER('2016-1-2',1,2)", 0);
            Assert.AreEqual(2, t);
        }

        [Test]
        public void IFTEXT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("IFTEXT(1,1,2)", 0);
            Assert.AreEqual(2, t);

            t = engine.TryEvaluate("IFTEXT('e',1,2)", 0);
            Assert.AreEqual(1, t);
            t = engine.TryEvaluate("IFTEXT('11',1,2)", 0);
            Assert.AreEqual(1, t);

            t = engine.TryEvaluate("IFTEXT('2016-1-2',1,2)", 0);
            Assert.AreEqual(1, t);
        }

        [Test]
        public void ISNUMBER_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("if(ISNUMBER(1),1,2)", 0);
            Assert.AreEqual(1, t);

            t = engine.TryEvaluate("if(ISNUMBER('e'),1,2)", 0);
            Assert.AreEqual(2, t);
            t = engine.TryEvaluate("if(ISNUMBER('11'),1,2)", 0);
            Assert.AreEqual(2, t);

            t = engine.TryEvaluate("if(ISNUMBER('2016-1-2'),1,2)", 0);
            Assert.AreEqual(2, t);
        }

        [Test]
        public void ISTEXT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("if(ISTEXT(1),1,2)", 0);
            Assert.AreEqual(2, t);

            t = engine.TryEvaluate("if(ISTEXT('e'),1,2)", 0);
            Assert.AreEqual(1, t);
            t = engine.TryEvaluate("if(ISTEXT('11'),1,2)", 0);
            Assert.AreEqual(1, t);

            t = engine.TryEvaluate("if(ISTEXT('2016-1-2'),1,2)", 0);
            Assert.AreEqual(1, t);
        }

        [Test]
        public void And_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("and(true(),1=1)", false);
            Assert.AreEqual(true, t);
            t = engine.TryEvaluate("and(true(),1)", false);
            Assert.AreEqual(true, t);

            t = engine.TryEvaluate("and(true(),false(),1=1)", false);
            Assert.AreEqual(false, t);
            t = engine.TryEvaluate("and(true(),false(),1)", false);
            Assert.AreEqual(false, t);

            t = engine.TryEvaluate("and(true(),0)", false);
            Assert.AreEqual(false, t);
        }

        [Test]
        public void Or_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("or(true(),1=1)", false);
            Assert.AreEqual(true, t);
            t = engine.TryEvaluate("or(true(),1)", false);
            Assert.AreEqual(true, t);

            t = engine.TryEvaluate("or(true(),false(),1=1)", false);
            Assert.AreEqual(true, t);
            t = engine.TryEvaluate("or(true(),false(),1)", false);
            Assert.AreEqual(true, t);

            t = engine.TryEvaluate("or(true(),0)", false);
            Assert.AreEqual(true, t);

            t = engine.TryEvaluate("or(false(),1=2)", false);
            Assert.AreEqual(false, t);
        }

        [Test]
        public void Not_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("not(true())", false);
            Assert.AreEqual(false, t);
            t = engine.TryEvaluate("not(false())", false);
            Assert.AreEqual(true, t);
        }

        [Test]
        public void true_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("true()", false);
            Assert.AreEqual(true, t);
        }

        [Test]
        public void false_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("false()", false);
            Assert.AreEqual(false, t);
        }


    }
}
