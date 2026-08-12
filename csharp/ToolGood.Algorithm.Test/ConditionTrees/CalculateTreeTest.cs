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

        [Test]
        public void HasBracket_Test()
        {
            // 无括号时，二元节点的 HasBracket 均为 false
            var tree = AlgorithmEngineHelper.ParseCalculate("a + b * c");
            Assert.AreEqual(tree.Type, CalculateTreeType.Add);
            Assert.IsFalse(tree.Nodes[1].HasBracket); // Mul 节点

            // 括号包裹叶子后，后续兄弟节点不应被误标记为 HasBracket（Bug 修复验证）
            tree = AlgorithmEngineHelper.ParseCalculate("(a) + b * c");
            Assert.AreEqual(tree.Type, CalculateTreeType.Add);
            Assert.IsTrue(tree.Nodes[0].HasBracket);  // (a) 叶子
            Assert.IsFalse(tree.Nodes[1].HasBracket); // Mul 节点，无括号

            // 括号包裹二元节点时，该节点 HasBracket = true
            tree = AlgorithmEngineHelper.ParseCalculate("a + (b * c)");
            Assert.AreEqual(tree.Type, CalculateTreeType.Add);
            Assert.IsTrue(tree.Nodes[1].HasBracket); // (b * c) Mul 节点

            tree = AlgorithmEngineHelper.ParseCalculate("(a + b) * c");
            Assert.AreEqual(tree.Type, CalculateTreeType.Mul);
            Assert.IsTrue(tree.Nodes[0].HasBracket); // (a + b) Add 节点

            // 与条件树同样修复了 || && 的泄漏
            tree = AlgorithmEngineHelper.ParseCalculate("(a) || b && c");
            Assert.AreEqual(tree.Type, CalculateTreeType.Or);
            Assert.IsTrue(tree.Nodes[0].HasBracket);  // (a) 叶子
            Assert.IsFalse(tree.Nodes[1].HasBracket); // And 节点，无括号
        }
    }
}
