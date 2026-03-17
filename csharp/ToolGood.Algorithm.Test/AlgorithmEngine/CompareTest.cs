using PetaTest;

namespace ToolGood.Algorithm.Test.Compare
{
    [TestFixture]
    internal class CompareTest
    {
        [Test]
        public void base_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var b = engine.TryEvaluate("1=1", false);
            Assert.AreEqual(true, b);

            b = engine.TryEvaluate("1=2", true);
            Assert.AreEqual(false, b);

            b = engine.TryEvaluate("1<>2", false);
            Assert.AreEqual(true, b);

            b = engine.TryEvaluate("1!=2", false);
            Assert.AreEqual(true, b);

            b = engine.TryEvaluate("1>2", true);
            Assert.AreEqual(false, b);

            b = engine.TryEvaluate("1<2", false);
            Assert.AreEqual(true, b);

            b = engine.TryEvaluate("1<=2", false);
            Assert.AreEqual(true, b);

            b = engine.TryEvaluate("1>=2", true);
            Assert.AreEqual(false, b);

            b = engine.TryEvaluate("'1'='1'", false);
            Assert.AreEqual(true, b);
            b = engine.TryEvaluate("'e'='e'", false);
            Assert.AreEqual(true, b);
            b = engine.TryEvaluate("'1'='2'", true);
            Assert.AreEqual(false, b);
            b = engine.TryEvaluate("'1'!='2'", false);
            Assert.AreEqual(true, b);
        }

        [Test]
        public void strict_equality_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var b = engine.TryEvaluate("1===1", false);
            Assert.AreEqual(true, b);

            b = engine.TryEvaluate("1===2", true);
            Assert.AreEqual(false, b);

            b = engine.TryEvaluate("'1'==='1'", false);
            Assert.AreEqual(true, b);

            b = engine.TryEvaluate("'1'==='2'", true);
            Assert.AreEqual(false, b);

            b = engine.TryEvaluate("1!==2", false);
            Assert.AreEqual(true, b);

            b = engine.TryEvaluate("1!==1", true);
            Assert.AreEqual(false, b);

            b = engine.TryEvaluate("'1'!=='2'", false);
            Assert.AreEqual(true, b);

            b = engine.TryEvaluate("'1'!=='1'", true);
            Assert.AreEqual(false, b);
        }

        [Test]
        public void null_compare_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();

            var bb2 = engine.TryEvaluate("1>null", true);
            Assert.AreEqual(bb2, false);

            bb2 = engine.TryEvaluate("1>=null", true);
            Assert.AreEqual(bb2, false);

            bb2 = engine.TryEvaluate("1<=null", true);
            Assert.AreEqual(bb2, false);

            bb2 = engine.TryEvaluate("1<null", true);
            Assert.AreEqual(bb2, false);

            bb2 = engine.TryEvaluate("1==null", true);
            Assert.AreEqual(bb2, false);

            bb2 = engine.TryEvaluate("1!=null", false);
            Assert.AreEqual(bb2, true);

            bb2 = engine.TryEvaluate("null=null", false);
            Assert.AreEqual(bb2, true);

            bb2 = engine.TryEvaluate("null!=null", true);
            Assert.AreEqual(bb2, false);

            bb2 = engine.TryEvaluate("'111'=null", true);
            Assert.AreEqual(bb2, false);

            bb2 = engine.TryEvaluate("'111'!=null", false);
            Assert.AreEqual(bb2, true);
        }

        [Test]
        public void negative_compare_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var value = engine.TryEvaluate("1 > (-2)", false);
            Assert.AreEqual(value, true);

            value = engine.TryEvaluate("(-1) > (-2）", false);
            Assert.AreEqual(value, true);

            value = engine.TryEvaluate("-1 > (-2)", false);
            Assert.AreEqual(value, true);

            value = engine.TryEvaluate("-1 > -2", false);
            Assert.AreEqual(value, true);

            var value2 = engine.TryEvaluate("-1 > -2", false);
            Assert.AreEqual(value2, true);

            var value3 = engine.TryEvaluate("-7 < -2", false);
            Assert.AreEqual(value3, true);

            value3 = engine.TryEvaluate("-7*Yes < -2", false);
            Assert.AreEqual(value3, true);

            value3 = engine.TryEvaluate("-7*No > -2", false);
            Assert.AreEqual(value3, true);
        }
    }
}
