// 移植自 csharp/ToolGood.Algorithm.Test/ConditionTrees/ConditionTreeTest.cs
//
// 类型对应：
//   AlgorithmEngineHelper.ParseCondition(txt) -> AlgorithmEngineHelper::ParseCondition(txt)
//   t.Type                                    -> t.Type（enum class ConditionTreeType，比较时转 int）
//   t.Text                                    -> t.Text()
//   t.Nodes[i]                                -> t.Nodes[i]（std::vector<ConditionTree>）
//   txt.Substring(t.Start, t.End - t.Start + 1) -> txt.substr(t.Start, t.End - t.Start + 1)
#include "cpptest.h"

#include <string>

#include "toolgood/AlgorithmEngineHelper.h"

using namespace toolgood;

namespace {

int Ty(ConditionTreeType t) { return static_cast<int>(t); }

TEST(ConditionTree, Test1) {
    std::string txt = "AA.IsText() = bb";
    ConditionTree t1 = AlgorithmEngineHelper::ParseCondition(txt);
    Assert::AreEqual(Ty(t1.Type), Ty(ConditionTreeType::String));
    Assert::AreEqual(txt.substr(t1.Start, t1.End - t1.Start + 1),
                     std::string("AA.IsText() = bb"));
    Assert::AreEqual(t1.Text(), std::string("AA.IsText() = bb"));

    txt = "[bbb]=bb";
    t1 = AlgorithmEngineHelper::ParseCondition(txt);
    Assert::AreEqual(Ty(t1.Type), Ty(ConditionTreeType::String));
    Assert::AreEqual(txt.substr(t1.Start, t1.End - t1.Start + 1), txt);
}

TEST(ConditionTree, Test2) {
    std::string txt = "AA.IsText()=bb && dd=ss";
    ConditionTree tree = AlgorithmEngineHelper::ParseCondition(txt);

    Assert::AreEqual(Ty(tree.Type), Ty(ConditionTreeType::And));
    const ConditionTree& t1 = tree.Nodes[0];
    const ConditionTree& t2 = tree.Nodes[1];
    Assert::AreEqual(txt.substr(t1.Start, t1.End - t1.Start + 1), std::string("AA.IsText()=bb"));
    Assert::AreEqual(txt.substr(t2.Start, t2.End - t2.Start + 1), std::string("dd=ss"));
}

TEST(ConditionTree, Test3) {
    std::string txt = "AA.IsText()=bb || dd=ss";
    ConditionTree tree = AlgorithmEngineHelper::ParseCondition(txt);

    Assert::AreEqual(Ty(tree.Type), Ty(ConditionTreeType::Or));
    const ConditionTree& t1 = tree.Nodes[0];
    const ConditionTree& t2 = tree.Nodes[1];
    Assert::AreEqual(txt.substr(t1.Start, t1.End - t1.Start + 1), std::string("AA.IsText()=bb"));
    Assert::AreEqual(txt.substr(t2.Start, t2.End - t2.Start + 1), std::string("dd=ss"));
}

TEST(ConditionTree, Test4) {
    std::string txt = "AA.IsText()=bb || (dd=ss && tt=22)";
    ConditionTree tree = AlgorithmEngineHelper::ParseCondition(txt);

    Assert::AreEqual(Ty(tree.Type), Ty(ConditionTreeType::Or));
    const ConditionTree& t1 = tree.Nodes[0];
    const ConditionTree& t2 = tree.Nodes[1];
    const ConditionTree& t3 = t2.Nodes[0];
    const ConditionTree& t4 = t2.Nodes[1];
    Assert::AreEqual(txt.substr(t1.Start, t1.End - t1.Start + 1), std::string("AA.IsText()=bb"));
    Assert::AreEqual(txt.substr(t3.Start, t3.End - t3.Start + 1), std::string("dd=ss"));
    Assert::AreEqual(txt.substr(t4.Start, t4.End - t4.Start + 1), std::string("tt=22"));
}

TEST(ConditionTree, Test5) {
    std::string txt = "AA.IsText()=bb || AND(dd=ss , tt=22)";
    ConditionTree tree = AlgorithmEngineHelper::ParseCondition(txt);

    Assert::AreEqual(Ty(tree.Type), Ty(ConditionTreeType::Or));
    const ConditionTree& t1 = tree.Nodes[0];
    const ConditionTree& t2 = tree.Nodes[1];
    Assert::AreEqual(txt.substr(t1.Start, t1.End - t1.Start + 1), std::string("AA.IsText()=bb"));
    Assert::AreEqual(txt.substr(t2.Start, t2.End - t2.Start + 1),
                     std::string("AND(dd=ss , tt=22)"));
}

TEST(ConditionTree, Test6) {
    std::string txt = "AA.IsText()==bb && (dd=ss || tt=22)";
    ConditionTree tree = AlgorithmEngineHelper::ParseCondition(txt);

    Assert::AreEqual(Ty(tree.Type), Ty(ConditionTreeType::And));
    const ConditionTree& t1 = tree.Nodes[0];
    const ConditionTree& t2 = tree.Nodes[1];
    Assert::AreEqual(Ty(t2.Type), Ty(ConditionTreeType::Or));
    const ConditionTree& t3 = t2.Nodes[0];
    const ConditionTree& t4 = t2.Nodes[1];

    Assert::AreEqual(txt.substr(t1.Start, t1.End - t1.Start + 1), std::string("AA.IsText()==bb"));
    Assert::AreEqual(txt.substr(t3.Start, t3.End - t3.Start + 1), std::string("dd=ss"));
    Assert::AreEqual(txt.substr(t4.Start, t4.End - t4.Start + 1), std::string("tt=22"));
}

TEST(ConditionTree, Test7) {
    std::string txt = "AA.IsText()==bb ? 1:2";
    ConditionTree t1 = AlgorithmEngineHelper::ParseCondition(txt);
    Assert::AreEqual(Ty(t1.Type), Ty(ConditionTreeType::String));
    Assert::AreEqual(txt.substr(t1.Start, t1.End - t1.Start + 1), txt);
}

TEST(ConditionTree, Test8) {
    std::string txt = "AA.IsText()==bb && (dd=ss || [tt]=22)";
    ConditionTree tree = AlgorithmEngineHelper::ParseCondition(txt);

    Assert::AreEqual(Ty(tree.Type), Ty(ConditionTreeType::And));
    const ConditionTree& t1 = tree.Nodes[0];
    const ConditionTree& t2 = tree.Nodes[1];
    Assert::AreEqual(Ty(t2.Type), Ty(ConditionTreeType::Or));
    const ConditionTree& t3 = t2.Nodes[0];
    const ConditionTree& t4 = t2.Nodes[1];

    Assert::AreEqual(txt.substr(t1.Start, t1.End - t1.Start + 1), std::string("AA.IsText()==bb"));
    Assert::AreEqual(txt.substr(t3.Start, t3.End - t3.Start + 1), std::string("dd=ss"));
    Assert::AreEqual(txt.substr(t4.Start, t4.End - t4.Start + 1), std::string("[tt]=22"));
}

TEST(ConditionTree, Test9) {
    std::string txt = "AA.IsText()==bb && (dd=ss || [tt]==22)";
    ConditionTree tree = AlgorithmEngineHelper::ParseCondition(txt);

    Assert::AreEqual(Ty(tree.Type), Ty(ConditionTreeType::And));
    const ConditionTree& t1 = tree.Nodes[0];
    const ConditionTree& t2 = tree.Nodes[1];
    Assert::AreEqual(Ty(t2.Type), Ty(ConditionTreeType::Or));
    const ConditionTree& t3 = t2.Nodes[0];
    const ConditionTree& t4 = t2.Nodes[1];

    Assert::AreEqual(txt.substr(t1.Start, t1.End - t1.Start + 1), std::string("AA.IsText()==bb"));
    Assert::AreEqual(txt.substr(t3.Start, t3.End - t3.Start + 1), std::string("dd=ss"));
    Assert::AreEqual(txt.substr(t4.Start, t4.End - t4.Start + 1), std::string("[tt]==22"));
}

TEST(ConditionTree, Test10) {
    std::string txt = "AA && (dd=ss || [tt]==22)";
    ConditionTree tree = AlgorithmEngineHelper::ParseCondition(txt);

    Assert::AreEqual(Ty(tree.Type), Ty(ConditionTreeType::And));
    const ConditionTree& t1 = tree.Nodes[0];
    const ConditionTree& t2 = tree.Nodes[1];
    Assert::AreEqual(Ty(t2.Type), Ty(ConditionTreeType::Or));
    const ConditionTree& t3 = t2.Nodes[0];
    const ConditionTree& t4 = t2.Nodes[1];

    Assert::AreEqual(txt.substr(t1.Start, t1.End - t1.Start + 1), std::string("AA"));
    Assert::AreEqual(txt.substr(t3.Start, t3.End - t3.Start + 1), std::string("dd=ss"));
    Assert::AreEqual(txt.substr(t4.Start, t4.End - t4.Start + 1), std::string("[tt]==22"));
}

TEST(ConditionTree, Test11) {
    std::string txt = "1 && (dd=ss || [tt]==22)";
    ConditionTree tree = AlgorithmEngineHelper::ParseCondition(txt);

    Assert::AreEqual(Ty(tree.Type), Ty(ConditionTreeType::And));
    const ConditionTree& t1 = tree.Nodes[0];
    const ConditionTree& t2 = tree.Nodes[1];
    Assert::AreEqual(Ty(t2.Type), Ty(ConditionTreeType::Or));
    const ConditionTree& t3 = t2.Nodes[0];
    const ConditionTree& t4 = t2.Nodes[1];

    Assert::AreEqual(txt.substr(t1.Start, t1.End - t1.Start + 1), std::string("1"));
    Assert::AreEqual(txt.substr(t3.Start, t3.End - t3.Start + 1), std::string("dd=ss"));
    Assert::AreEqual(txt.substr(t4.Start, t4.End - t4.Start + 1), std::string("[tt]==22"));
}

TEST(ConditionTree, TestError) {
    std::string txt = "";
    ConditionTree tree = AlgorithmEngineHelper::ParseCondition(txt);
    Assert::AreEqual(Ty(tree.Type), Ty(ConditionTreeType::Error));
    Assert::AreEqual(tree.ErrorMessage, std::string("condition is null"));
}

TEST(ConditionTree, HasBracket_Test) {
    // 无括号时，二元节点的 HasBracket 均为 false
    ConditionTree tree = AlgorithmEngineHelper::ParseCondition("a || b && c");
    Assert::AreEqual(Ty(tree.Type), Ty(ConditionTreeType::Or));
    Assert::IsFalse(tree.HasBracket);
    Assert::IsFalse(tree.Nodes[0].HasBracket);
    Assert::IsFalse(tree.Nodes[1].HasBracket);  // And 节点

    // 括号包裹叶子后，后续兄弟节点不应被误标记为 HasBracket（Bug 修复验证）
    tree = AlgorithmEngineHelper::ParseCondition("(a) || b && c");
    Assert::AreEqual(Ty(tree.Type), Ty(ConditionTreeType::Or));
    Assert::IsTrue(tree.Nodes[0].HasBracket);   // (a) 叶子
    Assert::IsFalse(tree.Nodes[1].HasBracket);  // And 节点，无括号

    // 括号包裹二元节点时，该节点 HasBracket = true
    tree = AlgorithmEngineHelper::ParseCondition("(a || b) && c");
    Assert::AreEqual(Ty(tree.Type), Ty(ConditionTreeType::And));
    Assert::IsTrue(tree.Nodes[0].HasBracket);  // (a || b) Or 节点

    tree = AlgorithmEngineHelper::ParseCondition("a && (b || c)");
    Assert::AreEqual(Ty(tree.Type), Ty(ConditionTreeType::And));
    Assert::IsTrue(tree.Nodes[1].HasBracket);  // (b || c) Or 节点

    // 括号包裹叶子与二元节点混合
    tree = AlgorithmEngineHelper::ParseCondition("(a) && (b || c)");
    Assert::AreEqual(Ty(tree.Type), Ty(ConditionTreeType::And));
    Assert::IsTrue(tree.Nodes[0].HasBracket);  // (a) 叶子
    Assert::IsTrue(tree.Nodes[1].HasBracket);  // (b || c) Or 节点

    tree = AlgorithmEngineHelper::ParseCondition("(a) || (b)");
    Assert::AreEqual(Ty(tree.Type), Ty(ConditionTreeType::Or));
    Assert::IsTrue(tree.Nodes[0].HasBracket);  // (a) 叶子
    Assert::IsTrue(tree.Nodes[1].HasBracket);  // (b) 叶子

    // 多重括号
    tree = AlgorithmEngineHelper::ParseCondition("((a)) && b");
    Assert::AreEqual(Ty(tree.Type), Ty(ConditionTreeType::And));
    Assert::IsTrue(tree.Nodes[0].HasBracket);   // ((a)) 叶子
    Assert::IsFalse(tree.Nodes[1].HasBracket);  // b 叶子
}

}  // namespace
