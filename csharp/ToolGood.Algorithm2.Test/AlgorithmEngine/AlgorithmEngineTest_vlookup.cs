using System;
using System.Collections.Generic;
using System.Text;
using PetaTest;
using ToolGood.Algorithm;

namespace ToolGood.Algorithm
{
    [TestFixture]
    public class AlgorithmEngineTest_vlookup
    {
        [Test]
        public void vlookup_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var num = engine.TryEvaluate("vlookup({{1,2,3},{2,3,4},{3,4,5},{4,5,6}},3,2)", 0);
            Assert.AreEqual(num, 4);


            num = engine.TryEvaluate("vlookup({{1,'2',3},{2,'3',4},{3,'4',5},{4,'5',6}},3,2)", 0);
            Assert.AreEqual(num, 4);

            num = engine.TryEvaluate("vlookup({{1,'2',3},{2,'3',4},{3,'4',5},{4,'5',6}},3.1,2)", 0);
            Assert.AreEqual(num, 4);

            num = engine.TryEvaluate("vlookup({{1,'2',3},{2,'3',4},{'3a','4',5},{4,'5',6}},'3a',2)", 0);
            Assert.AreEqual(num, 4);
        }

    }
}
