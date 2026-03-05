using PetaTest;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Test.ConditionTrees
{
    [TestFixture]
    public class CalculateTreeTest
    {
        [Test]
        public void Test1()
        {
            string txt = "A1+1";
            var t1 = AlgorithmEngineHelper.ParseCalculate(txt);
            Assert.AreEqual(t1.Type, CalculateTreeType.Add);
            Assert.AreEqual("A1+1", txt.Substring(t1.Start, t1.End - t1.Start + 1));
            Assert.AreEqual("A1+1", t1.Text);

            txt = "A1-(1+1)";
            t1 = AlgorithmEngineHelper.ParseCalculate(txt);
            Assert.AreEqual(t1.Type, CalculateTreeType.Sub);
            Assert.AreEqual("1+1", t1.Nodes[1].Text);

            txt = "A1*(1+1)";
            t1 = AlgorithmEngineHelper.ParseCalculate(txt);
            Assert.AreEqual(t1.Type, CalculateTreeType.Mul);

            txt = "A1/(1+1)";
            t1 = AlgorithmEngineHelper.ParseCalculate(txt);
            Assert.AreEqual(t1.Type, CalculateTreeType.Div);

            txt = "A1%(1+1)";
            t1 = AlgorithmEngineHelper.ParseCalculate(txt);
            Assert.AreEqual(t1.Type, CalculateTreeType.Mod);

            txt = "A1&(1+1)";
            t1 = AlgorithmEngineHelper.ParseCalculate(txt);
            Assert.AreEqual(t1.Type, CalculateTreeType.Connect);

            txt = "A1(1+1)";
            t1 = AlgorithmEngineHelper.ParseCalculate(txt);
            Assert.AreEqual(t1.Type, CalculateTreeType.String);

            txt = "A1(1+1)-";
            t1 = AlgorithmEngineHelper.ParseCalculate(txt);
            Assert.AreEqual(t1.Type, CalculateTreeType.Error);

            txt = "-1+(1+1)";
            t1 = AlgorithmEngineHelper.ParseCalculate(txt);
            Assert.AreEqual(t1.Type, CalculateTreeType.Add);

            txt = "-1";
            t1 = AlgorithmEngineHelper.ParseCalculate(txt);
            Assert.AreEqual(t1.Type, CalculateTreeType.String);
        }

        [Test]
        public void TestError()
        {
            string txt = "";
            var tree = AlgorithmEngineHelper.ParseCalculate(txt);
            Assert.AreEqual(tree.Type, CalculateTreeType.Error);
            Assert.AreEqual("exp is null", tree.ErrorMessage);
        }

        [Test]
        public void TestComplexExpressions()
        {
            string txt = "A1+B1*C1";
            var tree = AlgorithmEngineHelper.ParseCalculate(txt);
            Assert.AreEqual(tree.Type, CalculateTreeType.Add);

            txt = "A1*(B1+C1)/D1";
            tree = AlgorithmEngineHelper.ParseCalculate(txt);
            Assert.AreEqual(tree.Type, CalculateTreeType.Div);

            txt = "A1+B1-C1*D1/E1";
            tree = AlgorithmEngineHelper.ParseCalculate(txt);
            Assert.AreEqual(tree.Type, CalculateTreeType.Sub);
        }
    }
}
