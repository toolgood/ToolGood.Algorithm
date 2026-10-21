package test

import (
	"testing"

	alg "github.com/toolgood/ToolGood.Algorithm/go/algorithm"
)

// treeSubstr 等价 C# 的 txt.Substring(start, end-start+1)（按 rune 下标）。
func treeSubstr(txt string, start, end int) string {
	runes := []rune(txt)
	if start < 0 || end < start || end >= len(runes) {
		return ""
	}
	return string(runes[start : end+1])
}

// TestCalculateTreeTest 移植自
// csharp/ToolGood.Algorithm.Test/ConditionTrees/CalculateTreeTest.cs。
func TestCalculateTreeTest(t *testing.T) {
	t.Run("Test1", func(t *testing.T) {
		txt := "A1+1"
		t1 := alg.ParseCalculate(txt)
		assertTrue(t, t1.Type == alg.CalculateTreeTypeAdd, "A1+1 应为 Add")
		assertText(t, "A1+1", treeSubstr(txt, t1.Start, t1.End))
		assertText(t, "A1+1", t1.Text())

		txt = "A1-(1+1)"
		t1 = alg.ParseCalculate(txt)
		assertTrue(t, t1.Type == alg.CalculateTreeTypeSub, "A1-(1+1) 应为 Sub")
		assertText(t, "1+1", t1.Nodes[1].Text())

		txt = "A1*(1+1)"
		t1 = alg.ParseCalculate(txt)
		assertTrue(t, t1.Type == alg.CalculateTreeTypeMul, "A1*(1+1) 应为 Mul")

		txt = "A1/(1+1)"
		t1 = alg.ParseCalculate(txt)
		assertTrue(t, t1.Type == alg.CalculateTreeTypeDiv, "A1/(1+1) 应为 Div")

		txt = "A1%(1+1)"
		t1 = alg.ParseCalculate(txt)
		assertTrue(t, t1.Type == alg.CalculateTreeTypeMod, "取模表达式应为 Mod")

		txt = "A1&(1+1)"
		t1 = alg.ParseCalculate(txt)
		assertTrue(t, t1.Type == alg.CalculateTreeTypeConnect, "A1&(1+1) 应为 Connect")

		txt = "A1(1+1)"
		t1 = alg.ParseCalculate(txt)
		assertTrue(t, t1.Type == alg.CalculateTreeTypeString, "A1(1+1) 应为 String")

		txt = "A1(1+1)-"
		t1 = alg.ParseCalculate(txt)
		assertTrue(t, t1.Type == alg.CalculateTreeTypeError, "A1(1+1)- 应为 Error")

		txt = "-1+(1+1)"
		t1 = alg.ParseCalculate(txt)
		assertTrue(t, t1.Type == alg.CalculateTreeTypeAdd, "-1+(1+1) 应为 Add")

		txt = "-1"
		t1 = alg.ParseCalculate(txt)
		assertTrue(t, t1.Type == alg.CalculateTreeTypeString, "-1 应为 String")
	})

	t.Run("TestError", func(t *testing.T) {
		tree := alg.ParseCalculate("")
		assertTrue(t, tree.Type == alg.CalculateTreeTypeError, "空表达式应为 Error")
		assertText(t, "exp is null", tree.ErrorMessage)
	})

	t.Run("TestComplexExpressions", func(t *testing.T) {
		txt := "A1+B1*C1"
		tree := alg.ParseCalculate(txt)
		assertTrue(t, tree.Type == alg.CalculateTreeTypeAdd, "A1+B1*C1 应为 Add")

		txt = "A1*(B1+C1)/D1"
		tree = alg.ParseCalculate(txt)
		assertTrue(t, tree.Type == alg.CalculateTreeTypeDiv, "A1*(B1+C1)/D1 应为 Div")

		txt = "A1+B1-C1*D1/E1"
		tree = alg.ParseCalculate(txt)
		assertTrue(t, tree.Type == alg.CalculateTreeTypeSub, "A1+B1-C1*D1/E1 应为 Sub")
	})

	t.Run("HasBracket_Test", func(t *testing.T) {
		// 无括号时，二元节点的 HasBracket 均为 false
		tree := alg.ParseCalculate("a + b * c")
		assertTrue(t, tree.Type == alg.CalculateTreeTypeAdd, "应为 Add")
		assertFalse(t, tree.Nodes[1].HasBracket, "Mul 节点无括号")

		// 括号包裹叶子后，后续兄弟节点不应被误标记为 HasBracket（Bug 修复验证）
		tree = alg.ParseCalculate("(a) + b * c")
		assertTrue(t, tree.Type == alg.CalculateTreeTypeAdd, "应为 Add")
		assertTrue(t, tree.Nodes[0].HasBracket, "(a) 叶子有括号")
		assertFalse(t, tree.Nodes[1].HasBracket, "Mul 节点，无括号")

		// 括号包裹二元节点时，该节点 HasBracket = true
		tree = alg.ParseCalculate("a + (b * c)")
		assertTrue(t, tree.Type == alg.CalculateTreeTypeAdd, "应为 Add")
		assertTrue(t, tree.Nodes[1].HasBracket, "(b * c) Mul 节点有括号")

		tree = alg.ParseCalculate("(a + b) * c")
		assertTrue(t, tree.Type == alg.CalculateTreeTypeMul, "应为 Mul")
		assertTrue(t, tree.Nodes[0].HasBracket, "(a + b) Add 节点有括号")

		// 与条件树同样修复了 || && 的泄漏
		tree = alg.ParseCalculate("(a) || b && c")
		assertTrue(t, tree.Type == alg.CalculateTreeTypeOr, "应为 Or")
		assertTrue(t, tree.Nodes[0].HasBracket, "(a) 叶子有括号")
		assertFalse(t, tree.Nodes[1].HasBracket, "And 节点，无括号")
	})
}
