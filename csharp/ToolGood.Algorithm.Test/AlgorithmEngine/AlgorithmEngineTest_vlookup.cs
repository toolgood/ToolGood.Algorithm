using PetaTest;

namespace ToolGood.Algorithm
{
    [TestFixture]
    public class AlgorithmEngineTest_vlookup
    {
        [Test]
        public void vlookup_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var num = engine.TryEvaluate("vlookup(array(array(1,2,3),array(2,3,4),array(3,4,5),array(4,5,6)),3,2)", 0);
            Assert.AreEqual(num, 4);

            num = engine.TryEvaluate("vlookup(array(array(1,'2',3),array(2,3,4),array(3,'4',5),array(4,'5',6)),3,2)", 0);
            Assert.AreEqual(num, 4);

            num = engine.TryEvaluate("vlookup(array(array(1,'2',3),array(2,3,4),array(3.1,'4',5),array(4,'5',6)),3.1,2)", 0);
            Assert.AreEqual(num, 4);

            num = engine.TryEvaluate("vlookup(array(array(1,'2',3),array(2,3,4),array('3a','4',5),array(4,'5',6)),'3a',2)", 0);
            Assert.AreEqual(num, 4);

            num = engine.TryEvaluate("vlookup(array(array(1,'2',3),array(2,3,4),array(5,'4',5),array(6,'5',6)),'3',2,0)", 0);
            Assert.AreEqual(num, 0);
        }
 
    }
}