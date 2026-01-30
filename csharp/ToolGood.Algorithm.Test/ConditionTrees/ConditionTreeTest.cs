using PetaTest;
using ToolGood.Algorithm;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm2.Test.ConditionTrees
{
    [TestFixture]
    public class ConditionTreeTest
    {
        [Test]
        public void Test1()
        {
            string txt = "AA.IsText() = bb";
            var t1 = AlgorithmEngineHelper.ParseCondition(txt);
            Assert.AreEqual(t1.Type, ConditionTreeType.String);
            Assert.AreEqual("AA.IsText() = bb", txt.Substring(t1.Start, t1.End - t1.Start + 1));
            Assert.AreEqual("AA.IsText()=bb", t1.ConditionString);

            txt = "[bbb]=bb";
            t1 = AlgorithmEngineHelper.ParseCondition(txt);
            Assert.AreEqual(t1.Type, ConditionTreeType.String);
            Assert.AreEqual(txt, txt.Substring(t1.Start, t1.End - t1.Start + 1));
        }

        [Test]
        public void Test2()
        {
            string txt = "AA.IsText()=bb && dd=ss";
            var tree = AlgorithmEngineHelper.ParseCondition(txt);

            Assert.AreEqual(tree.Type, ConditionTreeType.And);
            var t1 = tree.Nodes[0];
            var t2 = tree.Nodes[1];
            Assert.AreEqual("AA.IsText()=bb", txt.Substring(t1.Start, t1.End - t1.Start + 1));
            Assert.AreEqual("dd=ss", txt.Substring(t2.Start, t2.End - t2.Start + 1));
        }

        [Test]
        public void Test3()
        {
            string txt = "AA.IsText()=bb || dd=ss";
            var tree = AlgorithmEngineHelper.ParseCondition(txt);

            Assert.AreEqual(tree.Type, ConditionTreeType.Or);
            var t1 = tree.Nodes[0];
            var t2 = tree.Nodes[1];
            Assert.AreEqual("AA.IsText()=bb", txt.Substring(t1.Start, t1.End - t1.Start + 1));
            Assert.AreEqual("dd=ss", txt.Substring(t2.Start, t2.End - t2.Start + 1));
        }

        [Test]
        public void Test4()
        {
            string txt = "AA.IsText()=bb || (dd=ss && tt=22)";
            var tree = AlgorithmEngineHelper.ParseCondition(txt);

            Assert.AreEqual(tree.Type, ConditionTreeType.Or);
            var t1 = tree.Nodes[0];
            var t2 = tree.Nodes[1];
            var t3 = t2.Nodes[0];
            var t4 = t2.Nodes[1];
            Assert.AreEqual("AA.IsText()=bb", txt.Substring(t1.Start, t1.End - t1.Start + 1));
            Assert.AreEqual("dd=ss", txt.Substring(t3.Start, t3.End - t3.Start + 1));
            Assert.AreEqual("tt=22", txt.Substring(t4.Start, t4.End - t4.Start + 1));
        }

        [Test]
        public void Test5()
        {
            string txt = "AA.IsText()=bb || AND(dd=ss , tt=22)";
            var tree = AlgorithmEngineHelper.ParseCondition(txt);

            Assert.AreEqual(tree.Type, ConditionTreeType.Or);
            var t1 = tree.Nodes[0];
            var t2 = tree.Nodes[1];
            Assert.AreEqual("AA.IsText()=bb", txt.Substring(t1.Start, t1.End - t1.Start + 1));
            Assert.AreEqual("AND(dd=ss , tt=22)", txt.Substring(t2.Start, t2.End - t2.Start + 1));
        }

        [Test]
        public void Test6()
        {
            string txt = "AA.IsText()==bb && (dd=ss || tt=22)";
            var tree = AlgorithmEngineHelper.ParseCondition(txt);

            Assert.AreEqual(tree.Type, ConditionTreeType.And);
            var t1 = tree.Nodes[0];
            var t2 = tree.Nodes[1];
            Assert.AreEqual(t2.Type, ConditionTreeType.Or);
            var t3 = t2.Nodes[0];
            var t4 = t2.Nodes[1];

            Assert.AreEqual("AA.IsText()==bb", txt.Substring(t1.Start, t1.End - t1.Start + 1));
            Assert.AreEqual("dd=ss", txt.Substring(t3.Start, t3.End - t3.Start + 1));
            Assert.AreEqual("tt=22", txt.Substring(t4.Start, t4.End - t4.Start + 1));
        }

        [Test]
        public void Test7()
        {
            string txt = "AA.IsText()==bb ? 1:2";
            var t1 = AlgorithmEngineHelper.ParseCondition(txt);
            Assert.AreEqual(t1.Type, ConditionTreeType.String);
            Assert.AreEqual(txt, txt.Substring(t1.Start, t1.End - t1.Start + 1));
        }

        [Test]
        public void Test8()
        {
            string txt = "AA.IsText()==bb && (dd=ss || [tt]=22)";
            var tree = AlgorithmEngineHelper.ParseCondition(txt);

            Assert.AreEqual(tree.Type, ConditionTreeType.And);
            var t1 = tree.Nodes[0];
            var t2 = tree.Nodes[1];
            Assert.AreEqual(t2.Type, ConditionTreeType.Or);
            var t3 = t2.Nodes[0];
            var t4 = t2.Nodes[1];

            Assert.AreEqual("AA.IsText()==bb", txt.Substring(t1.Start, t1.End - t1.Start + 1));
            Assert.AreEqual("dd=ss", txt.Substring(t3.Start, t3.End - t3.Start + 1));
            Assert.AreEqual("[tt]=22", txt.Substring(t4.Start, t4.End - t4.Start + 1));
        }

        [Test]
        public void Test9()
        {
            string txt = "AA.IsText()==bb && (dd=ss || [tt]==22)";
            var tree = AlgorithmEngineHelper.ParseCondition(txt);

            Assert.AreEqual(tree.Type, ConditionTreeType.And);
            var t1 = tree.Nodes[0];
            var t2 = tree.Nodes[1];
            Assert.AreEqual(t2.Type, ConditionTreeType.Or);
            var t3 = t2.Nodes[0];
            var t4 = t2.Nodes[1];

            Assert.AreEqual("AA.IsText()==bb", txt.Substring(t1.Start, t1.End - t1.Start + 1));
            Assert.AreEqual("dd=ss", txt.Substring(t3.Start, t3.End - t3.Start + 1));
            Assert.AreEqual("[tt]==22", txt.Substring(t4.Start, t4.End - t4.Start + 1));
        }


		[Test]
		public void Test10()
		{
			string txt = "AA && (dd=ss || [tt]==22)";
			var tree = AlgorithmEngineHelper.ParseCondition(txt);

			Assert.AreEqual(tree.Type, ConditionTreeType.And);
			var t1 = tree.Nodes[0];
			var t2 = tree.Nodes[1];
			Assert.AreEqual(t2.Type, ConditionTreeType.Or);
			var t3 = t2.Nodes[0];
			var t4 = t2.Nodes[1];

			Assert.AreEqual("AA", txt.Substring(t1.Start, t1.End - t1.Start + 1));
			Assert.AreEqual("dd=ss", txt.Substring(t3.Start, t3.End - t3.Start + 1));
			Assert.AreEqual("[tt]==22", txt.Substring(t4.Start, t4.End - t4.Start + 1));
		}


		[Test]
		public void Test11()
		{
			string txt = "1 && (dd=ss || [tt]==22)";
			var tree = AlgorithmEngineHelper.ParseCondition(txt);

			Assert.AreEqual(tree.Type, ConditionTreeType.And);
			var t1 = tree.Nodes[0];
			var t2 = tree.Nodes[1];
			Assert.AreEqual(t2.Type, ConditionTreeType.Or);
			var t3 = t2.Nodes[0];
			var t4 = t2.Nodes[1];

			Assert.AreEqual("1", txt.Substring(t1.Start, t1.End - t1.Start + 1));
			Assert.AreEqual("dd=ss", txt.Substring(t3.Start, t3.End - t3.Start + 1));
			Assert.AreEqual("[tt]==22", txt.Substring(t4.Start, t4.End - t4.Start + 1));
		}
	}
}