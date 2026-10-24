// 移植自 csharp/ToolGood.Algorithm.Test/ConditionTrees/CalculateTreeTest.cs
//
// 类型对应：
//   AlgorithmEngineHelper.ParseCalculate(txt) -> AlgorithmEngineHelper::ParseCalculate(txt)
//   t.Type                                    -> t.Type（enum class CalculateTreeType，比较时转 int）
//   t.Text / t.Nodes[i].Text                  -> t.Text() / t.Nodes[i].Text()
//   txt.Substring(...)                        -> txt.substr(...)
#include "cpptest.h"

#include <string>

#include "toolgood/AlgorithmEngineHelper.h"

using namespace toolgood;

namespace {

int Ty(CalculateTreeType t) { return static_cast<int>(t); }

TEST(CalculateTree, Test1) {
    std::string txt = "A1+1";
    CalculateTree t1 = AlgorithmEngineHelper::ParseCalculate(txt);
    Assert::AreEqual(Ty(t1.Type), Ty(CalculateTreeType::Add));
    Assert::AreEqual(txt.substr(t1.Start, t1.End - t1.Start + 1), std::string("A1+1"));
    Assert::AreEqual(t1.Text(), std::string("A1+1"));

    txt = "A1-(1+1)";
    t1 = AlgorithmEngineHelper::ParseCalculate(txt);
    Assert::AreEqual(Ty(t1.Type), Ty(CalculateTreeType::Sub));
    Assert::AreEqual(t1.Nodes[1].Text(), std::string("1+1"));

    txt = "A1*(1+1)";
    t1 = AlgorithmEngineHelper::ParseCalculate(txt);
    Assert::AreEqual(Ty(t1.Type), Ty(CalculateTreeType::Mul));

    txt = "A1/(1+1)";
    t1 = AlgorithmEngineHelper::ParseCalculate(txt);
    Assert::AreEqual(Ty(t1.Type), Ty(CalculateTreeType::Div));

    txt = "A1%(1+1)";
    t1 = AlgorithmEngineHelper::ParseCalculate(txt);
    Assert::AreEqual(Ty(t1.Type), Ty(CalculateTreeType::Mod));

    txt = "A1&(1+1)";
    t1 = AlgorithmEngineHelper::ParseCalculate(txt);
    Assert::AreEqual(Ty(t1.Type), Ty(CalculateTreeType::Connect));

    txt = "A1(1+1)";
    t1 = AlgorithmEngineHelper::ParseCalculate(txt);
    Assert::AreEqual(Ty(t1.Type), Ty(CalculateTreeType::String));

    txt = "A1(1+1)-";
    t1 = AlgorithmEngineHelper::ParseCalculate(txt);
    Assert::AreEqual(Ty(t1.Type), Ty(CalculateTreeType::Error));

    txt = "-1+(1+1)";
    t1 = AlgorithmEngineHelper::ParseCalculate(txt);
    Assert::AreEqual(Ty(t1.Type), Ty(CalculateTreeType::Add));

    txt = "-1";
    t1 = AlgorithmEngineHelper::ParseCalculate(txt);
    Assert::AreEqual(Ty(t1.Type), Ty(CalculateTreeType::String));
}

TEST(CalculateTree, TestError) {
    std::string txt = "";
    CalculateTree tree = AlgorithmEngineHelper::ParseCalculate(txt);
    Assert::AreEqual(Ty(tree.Type), Ty(CalculateTreeType::Error));
    Assert::AreEqual(tree.ErrorMessage, std::string("exp is null"));
}

TEST(CalculateTree, TestComplexExpressions) {
    std::string txt = "A1+B1*C1";
    CalculateTree tree = AlgorithmEngineHelper::ParseCalculate(txt);
    Assert::AreEqual(Ty(tree.Type), Ty(CalculateTreeType::Add));

    txt = "A1*(B1+C1)/D1";
    tree = AlgorithmEngineHelper::ParseCalculate(txt);
    Assert::AreEqual(Ty(tree.Type), Ty(CalculateTreeType::Div));

    txt = "A1+B1-C1*D1/E1";
    tree = AlgorithmEngineHelper::ParseCalculate(txt);
    Assert::AreEqual(Ty(tree.Type), Ty(CalculateTreeType::Sub));
}

TEST(CalculateTree, HasBracket_Test) {
    // 无括号时，二元节点的 HasBracket 均为 false
    CalculateTree tree = AlgorithmEngineHelper::ParseCalculate("a + b * c");
    Assert::AreEqual(Ty(tree.Type), Ty(CalculateTreeType::Add));
    Assert::IsFalse(tree.Nodes[1].HasBracket);  // Mul 节点

    // 括号包裹叶子后，后续兄弟节点不应被误标记为 HasBracket（Bug 修复验证）
    tree = AlgorithmEngineHelper::ParseCalculate("(a) + b * c");
    Assert::AreEqual(Ty(tree.Type), Ty(CalculateTreeType::Add));
    Assert::IsTrue(tree.Nodes[0].HasBracket);   // (a) 叶子
    Assert::IsFalse(tree.Nodes[1].HasBracket);  // Mul 节点，无括号

    // 括号包裹二元节点时，该节点 HasBracket = true
    tree = AlgorithmEngineHelper::ParseCalculate("a + (b * c)");
    Assert::AreEqual(Ty(tree.Type), Ty(CalculateTreeType::Add));
    Assert::IsTrue(tree.Nodes[1].HasBracket);  // (b * c) Mul 节点

    tree = AlgorithmEngineHelper::ParseCalculate("(a + b) * c");
    Assert::AreEqual(Ty(tree.Type), Ty(CalculateTreeType::Mul));
    Assert::IsTrue(tree.Nodes[0].HasBracket);  // (a + b) Add 节点

    // 与条件树同样修复了 || && 的泄漏
    tree = AlgorithmEngineHelper::ParseCalculate("(a) || b && c");
    Assert::AreEqual(Ty(tree.Type), Ty(CalculateTreeType::Or));
    Assert::IsTrue(tree.Nodes[0].HasBracket);   // (a) 叶子
    Assert::IsFalse(tree.Nodes[1].HasBracket);  // And 节点，无括号
}

}  // namespace
