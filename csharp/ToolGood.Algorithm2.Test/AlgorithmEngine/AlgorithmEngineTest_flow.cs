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
        public void iserror_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("iserror(1/0,1)", 0);
            Assert.AreEqual(1, t);

            t = engine.TryEvaluate("iserror(1-'rrr',1)", 0);
            Assert.AreEqual(1, t);
        }
        [Test]
        public void ifnull_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("isnull(null,1)", 0);
            Assert.AreEqual(1, t);

            t = engine.TryEvaluate("isnull(1,2)", 0);
            Assert.AreEqual(1, t);
        }

        [Test]
        public void isnullorerror_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("isnullorerror(null,1)", 0);
            Assert.AreEqual(1, t);

            t = engine.TryEvaluate("isnullorerror(1/0,1)", 0);
            Assert.AreEqual(1, t);

            t = engine.TryEvaluate("isnullorerror(1,2)", 0);
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
        public void ISNONTEXT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("if(ISNONTEXT(1),1,2)", 0);
            Assert.AreEqual(1, t);

            t = engine.TryEvaluate("if(ISNONTEXT('e'),1,2)", 0);
            Assert.AreEqual(2, t);
            t = engine.TryEvaluate("if(ISNONTEXT('11'),1,2)", 0);
            Assert.AreEqual(2, t);

            t = engine.TryEvaluate("if(ISNONTEXT('2016-1-2'),1,2)", 0);
            Assert.AreEqual(2, t);
        }


        [Test]
        public void ISLOGICAL_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("if(ISLOGICAL(1),1,2)", 0);
            Assert.AreEqual(2, t);

            t = engine.TryEvaluate("if(ISLOGICAL('e'),1,2)", 0);
            Assert.AreEqual(2, t);

            t = engine.TryEvaluate("if(ISLOGICAL(true),1,2)", 0);
            Assert.AreEqual(1, t);

            t = engine.TryEvaluate("if(ISLOGICAL(false),1,2)", 0);
            Assert.AreEqual(1, t);
        }

        [Test]
        public void ISEVEN_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("if(ISEVEN(1),1,2)", 0);
            Assert.AreEqual(2, t);

            t = engine.TryEvaluate("if(ISEVEN(2),1,2)", 0);
            Assert.AreEqual(1, t);

            t = engine.TryEvaluate("if(ISEVEN('e'),1,2)", 0);
            Assert.AreEqual(2, t);
        }

        [Test]
        public void ISODD_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("if(ISODD(1),1,2)", 0);
            Assert.AreEqual(1, t);

            t = engine.TryEvaluate("if(ISODD(2),1,2)", 0);
            Assert.AreEqual(2, t);

            t = engine.TryEvaluate("if(ISODD('e'),1,2)", 0);
            Assert.AreEqual(2, t);
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

        [Test]
        public void andor_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("1=1 && 2==2 and 3=3", false);
            Assert.AreEqual(true, t);

            t = engine.TryEvaluate("1=1 && 2!=2", false);
            Assert.AreEqual(false, t);

            t = engine.TryEvaluate("1=1 && 2!=2", true);
            Assert.AreEqual(false, t);

            t = engine.TryEvaluate("1=1 || 2!=2", false);
            Assert.AreEqual(true, t);


            t = engine.TryEvaluate("1=1 and 2==2", false);
            Assert.AreEqual(true, t);

            t = engine.TryEvaluate("1=1 and 2!=2", false);
            Assert.AreEqual(false, t);

            t = engine.TryEvaluate("1=1 and 2!=2", true);
            Assert.AreEqual(false, t);

            t = engine.TryEvaluate("1=1 or 2!=2", false);
            Assert.AreEqual(true, t);

        }




    }
}
