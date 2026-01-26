using PetaTest;

namespace ToolGood.Algorithm
{
	[TestFixture]
	public class AlgorithmEngineTest_vlookup
	{
		[Test]
		public void LookFloor_test()
		{
			AlgorithmEngine engine = new AlgorithmEngine();

			var num = engine.TryEvaluate("LookFloor(2,[0,1,2,3,4])", 0);
			Assert.AreEqual(num, 2);

			num = engine.TryEvaluate("LookFloor(2.1,[0,1,2,3,4])", 0);
			Assert.AreEqual(num, 2);

			num = engine.TryEvaluate("LookFloor(-2.1,[0,1,2,3,4])", 0);
			Assert.AreEqual(num, 0);

			num = engine.TryEvaluate("LookFloor(5,[0,1,2,3,4])", 0);
			Assert.AreEqual(num, 4);
		}

		[Test]
		public void LookCeiling_test()
		{
			AlgorithmEngine engine = new AlgorithmEngine();

			var num = engine.TryEvaluate("LookCeiling(2,[0,1,2,3,4])", 0);
			Assert.AreEqual(num, 2);

			num = engine.TryEvaluate("LookCeiling(2.1,[0,1,2,3,4])", 0);
			Assert.AreEqual(num, 3);

			num = engine.TryEvaluate("LookCeiling(-2.1,[0,1,2,3,4])", 0);
			Assert.AreEqual(num, 0);

			num = engine.TryEvaluate("LookCeiling(5,[0,1,2,3,4])", 0);
			Assert.AreEqual(num, 4);
		}

	}
}