package test

import (
	"testing"

	alg "github.com/toolgood/ToolGood.Algorithm/go/algorithm"
)

// TestConditionTreeTest 移植自
// csharp/ToolGood.Algorithm.Test/ConditionTrees/ConditionTreeTest.cs。
func TestConditionTreeTest(t *testing.T) {
	t.Run("Test1", func(t *testing.T) {
		txt := "AA.IsText() = bb"
		t1 := alg.ParseCondition(txt)
		assertTrue(t, t1.Type == alg.ConditionTreeTypeString, "应为 String")
		assertText(t, "AA.IsText() = bb", treeSubstr(txt, t1.Start, t1.End))
		assertText(t, "AA.IsText() = bb", t1.Text())

		txt = "[bbb]=bb"
		t1 = alg.ParseCondition(txt)
		assertTrue(t, t1.Type == alg.ConditionTreeTypeString, "应为 String")
		assertText(t, txt, treeSubstr(txt, t1.Start, t1.End))
	})

	t.Run("Test2", func(t *testing.T) {
		txt := "AA.IsText()=bb && dd=ss"
		tree := alg.ParseCondition(txt)

		assertTrue(t, tree.Type == alg.ConditionTreeTypeAnd, "应为 And")
		t1 := tree.Nodes[0]
		t2 := tree.Nodes[1]
		assertText(t, "AA.IsText()=bb", treeSubstr(txt, t1.Start, t1.End))
		assertText(t, "dd=ss", treeSubstr(txt, t2.Start, t2.End))
	})

	t.Run("Test3", func(t *testing.T) {
		txt := "AA.IsText()=bb || dd=ss"
		tree := alg.ParseCondition(txt)

		assertTrue(t, tree.Type == alg.ConditionTreeTypeOr, "应为 Or")
		t1 := tree.Nodes[0]
		t2 := tree.Nodes[1]
		assertText(t, "AA.IsText()=bb", treeSubstr(txt, t1.Start, t1.End))
		assertText(t, "dd=ss", treeSubstr(txt, t2.Start, t2.End))
	})

	t.Run("Test4", func(t *testing.T) {
		txt := "AA.IsText()=bb || (dd=ss && tt=22)"
		tree := alg.ParseCondition(txt)

		assertTrue(t, tree.Type == alg.ConditionTreeTypeOr, "应为 Or")
		t1 := tree.Nodes[0]
		t2 := tree.Nodes[1]
		t3 := t2.Nodes[0]
		t4 := t2.Nodes[1]
		assertText(t, "AA.IsText()=bb", treeSubstr(txt, t1.Start, t1.End))
		assertText(t, "dd=ss", treeSubstr(txt, t3.Start, t3.End))
		assertText(t, "tt=22", treeSubstr(txt, t4.Start, t4.End))
	})

	t.Run("Test5", func(t *testing.T) {
		txt := "AA.IsText()=bb || AND(dd=ss , tt=22)"
		tree := alg.ParseCondition(txt)

		assertTrue(t, tree.Type == alg.ConditionTreeTypeOr, "应为 Or")
		t1 := tree.Nodes[0]
		t2 := tree.Nodes[1]
		assertText(t, "AA.IsText()=bb", treeSubstr(txt, t1.Start, t1.End))
		assertText(t, "AND(dd=ss , tt=22)", treeSubstr(txt, t2.Start, t2.End))
	})

	t.Run("Test6", func(t *testing.T) {
		txt := "AA.IsText()==bb && (dd=ss || tt=22)"
		tree := alg.ParseCondition(txt)

		assertTrue(t, tree.Type == alg.ConditionTreeTypeAnd, "应为 And")
		t1 := tree.Nodes[0]
		t2 := tree.Nodes[1]
		assertTrue(t, t2.Type == alg.ConditionTreeTypeOr, "t2 应为 Or")
		t3 := t2.Nodes[0]
		t4 := t2.Nodes[1]

		assertText(t, "AA.IsText()==bb", treeSubstr(txt, t1.Start, t1.End))
		assertText(t, "dd=ss", treeSubstr(txt, t3.Start, t3.End))
		assertText(t, "tt=22", treeSubstr(txt, t4.Start, t4.End))
	})

	t.Run("Test7", func(t *testing.T) {
		txt := "AA.IsText()==bb ? 1:2"
		t1 := alg.ParseCondition(txt)
		assertTrue(t, t1.Type == alg.ConditionTreeTypeString, "应为 String")
		assertText(t, txt, treeSubstr(txt, t1.Start, t1.End))
	})

	t.Run("Test8", func(t *testing.T) {
		txt := "AA.IsText()==bb && (dd=ss || [tt]=22)"
		tree := alg.ParseCondition(txt)

		assertTrue(t, tree.Type == alg.ConditionTreeTypeAnd, "应为 And")
		t1 := tree.Nodes[0]
		t2 := tree.Nodes[1]
		assertTrue(t, t2.Type == alg.ConditionTreeTypeOr, "t2 应为 Or")
		t3 := t2.Nodes[0]
		t4 := t2.Nodes[1]

		assertText(t, "AA.IsText()==bb", treeSubstr(txt, t1.Start, t1.End))
		assertText(t, "dd=ss", treeSubstr(txt, t3.Start, t3.End))
		assertText(t, "[tt]=22", treeSubstr(txt, t4.Start, t4.End))
	})

	t.Run("Test9", func(t *testing.T) {
		txt := "AA.IsText()==bb && (dd=ss || [tt]==22)"
		tree := alg.ParseCondition(txt)

		assertTrue(t, tree.Type == alg.ConditionTreeTypeAnd, "应为 And")
		t1 := tree.Nodes[0]
		t2 := tree.Nodes[1]
		assertTrue(t, t2.Type == alg.ConditionTreeTypeOr, "t2 应为 Or")
		t3 := t2.Nodes[0]
		t4 := t2.Nodes[1]

		assertText(t, "AA.IsText()==bb", treeSubstr(txt, t1.Start, t1.End))
		assertText(t, "dd=ss", treeSubstr(txt, t3.Start, t3.End))
		assertText(t, "[tt]==22", treeSubstr(txt, t4.Start, t4.End))
	})

	t.Run("Test10", func(t *testing.T) {
		txt := "AA && (dd=ss || [tt]==22)"
		tree := alg.ParseCondition(txt)

		assertTrue(t, tree.Type == alg.ConditionTreeTypeAnd, "应为 And")
		t1 := tree.Nodes[0]
		t2 := tree.Nodes[1]
		assertTrue(t, t2.Type == alg.ConditionTreeTypeOr, "t2 应为 Or")
		t3 := t2.Nodes[0]
		t4 := t2.Nodes[1]

		assertText(t, "AA", treeSubstr(txt, t1.Start, t1.End))
		assertText(t, "dd=ss", treeSubstr(txt, t3.Start, t3.End))
		assertText(t, "[tt]==22", treeSubstr(txt, t4.Start, t4.End))
	})

	t.Run("Test11", func(t *testing.T) {
		txt := "1 && (dd=ss || [tt]==22)"
		tree := alg.ParseCondition(txt)

		assertTrue(t, tree.Type == alg.ConditionTreeTypeAnd, "应为 And")
		t1 := tree.Nodes[0]
		t2 := tree.Nodes[1]
		assertTrue(t, t2.Type == alg.ConditionTreeTypeOr, "t2 应为 Or")
		t3 := t2.Nodes[0]
		t4 := t2.Nodes[1]

		assertText(t, "1", treeSubstr(txt, t1.Start, t1.End))
		assertText(t, "dd=ss", treeSubstr(txt, t3.Start, t3.End))
		assertText(t, "[tt]==22", treeSubstr(txt, t4.Start, t4.End))
	})

	t.Run("TestError", func(t *testing.T) {
		tree := alg.ParseCondition("")
		assertTrue(t, tree.Type == alg.ConditionTreeTypeError, "空条件应为 Error")
		assertText(t, "condition is null", tree.ErrorMessage)
	})

	t.Run("HasBracket_Test", func(t *testing.T) {
		// 无括号时，二元节点的 HasBracket 均为 false
		tree := alg.ParseCondition("a || b && c")
		assertTrue(t, tree.Type == alg.ConditionTreeTypeOr, "应为 Or")
		assertFalse(t, tree.HasBracket, "根节点无括号")
		assertFalse(t, tree.Nodes[0].HasBracket, "a 无括号")
		assertFalse(t, tree.Nodes[1].HasBracket, "And 节点，无括号")

		// 括号包裹叶子后，后续兄弟节点不应被误标记为 HasBracket（Bug 修复验证）
		tree = alg.ParseCondition("(a) || b && c")
		assertTrue(t, tree.Type == alg.ConditionTreeTypeOr, "应为 Or")
		assertTrue(t, tree.Nodes[0].HasBracket, "(a) 叶子有括号")
		assertFalse(t, tree.Nodes[1].HasBracket, "And 节点，无括号")

		// 括号包裹二元节点时，该节点 HasBracket = true
		tree = alg.ParseCondition("(a || b) && c")
		assertTrue(t, tree.Type == alg.ConditionTreeTypeAnd, "应为 And")
		assertTrue(t, tree.Nodes[0].HasBracket, "(a || b) Or 节点有括号")

		tree = alg.ParseCondition("a && (b || c)")
		assertTrue(t, tree.Type == alg.ConditionTreeTypeAnd, "应为 And")
		assertTrue(t, tree.Nodes[1].HasBracket, "(b || c) Or 节点有括号")

		// 括号包裹叶子与二元节点混合
		tree = alg.ParseCondition("(a) && (b || c)")
		assertTrue(t, tree.Type == alg.ConditionTreeTypeAnd, "应为 And")
		assertTrue(t, tree.Nodes[0].HasBracket, "(a) 叶子有括号")
		assertTrue(t, tree.Nodes[1].HasBracket, "(b || c) Or 节点有括号")

		tree = alg.ParseCondition("(a) || (b)")
		assertTrue(t, tree.Type == alg.ConditionTreeTypeOr, "应为 Or")
		assertTrue(t, tree.Nodes[0].HasBracket, "(a) 叶子有括号")
		assertTrue(t, tree.Nodes[1].HasBracket, "(b) 叶子有括号")

		// 多重括号
		tree = alg.ParseCondition("((a)) && b")
		assertTrue(t, tree.Type == alg.ConditionTreeTypeAnd, "应为 And")
		assertTrue(t, tree.Nodes[0].HasBracket, "((a)) 叶子有括号")
		assertFalse(t, tree.Nodes[1].HasBracket, "b 叶子无括号")
	})
}
