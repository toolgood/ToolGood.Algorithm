package toolgood.algorithm.conditiontrees;

import org.junit.Test;
import static org.junit.Assert.*;

import toolgood.algorithm.AlgorithmEngineHelper;
import toolgood.algorithm.enums.ConditionTreeType;
import toolgood.algorithm.internals.ConditionTree;

public class ConditionTreeTest {

    @Test
    public void Test1() {
        String txt = "AA.IsText() = bb";
        ConditionTree t1 = AlgorithmEngineHelper.ParseCondition(txt);
        assertEquals(ConditionTreeType.String, t1.Type);
        assertEquals("AA.IsText() = bb", txt.substring(t1.Start, t1.End + 1));
        assertEquals("AA.IsText() = bb", t1.getText());

        txt = "[bbb]=bb";
        t1 = AlgorithmEngineHelper.ParseCondition(txt);
        assertEquals(ConditionTreeType.String, t1.Type);
        assertEquals(txt, txt.substring(t1.Start, t1.End + 1));
    }

    @Test
    public void Test2() {
        String txt = "AA.IsText()=bb && dd=ss";
        ConditionTree tree = AlgorithmEngineHelper.ParseCondition(txt);

        assertEquals(ConditionTreeType.And, tree.Type);
        ConditionTree t1 = tree.Nodes.get(0);
        ConditionTree t2 = tree.Nodes.get(1);
        assertEquals("AA.IsText()=bb", txt.substring(t1.Start, t1.End + 1));
        assertEquals("dd=ss", txt.substring(t2.Start, t2.End + 1));
    }

    @Test
    public void Test3() {
        String txt = "AA.IsText()=bb || dd=ss";
        ConditionTree tree = AlgorithmEngineHelper.ParseCondition(txt);

        assertEquals(ConditionTreeType.Or, tree.Type);
        ConditionTree t1 = tree.Nodes.get(0);
        ConditionTree t2 = tree.Nodes.get(1);
        assertEquals("AA.IsText()=bb", txt.substring(t1.Start, t1.End + 1));
        assertEquals("dd=ss", txt.substring(t2.Start, t2.End + 1));
    }

    @Test
    public void Test4() {
        String txt = "AA.IsText()=bb || (dd=ss && tt=22)";
        ConditionTree tree = AlgorithmEngineHelper.ParseCondition(txt);

        assertEquals(ConditionTreeType.Or, tree.Type);
        ConditionTree t1 = tree.Nodes.get(0);
        ConditionTree t2 = tree.Nodes.get(1);
        ConditionTree t3 = t2.Nodes.get(0);
        ConditionTree t4 = t2.Nodes.get(1);
        assertEquals("AA.IsText()=bb", txt.substring(t1.Start, t1.End + 1));
        assertEquals("dd=ss", txt.substring(t3.Start, t3.End + 1));
        assertEquals("tt=22", txt.substring(t4.Start, t4.End + 1));
    }

    @Test
    public void Test5() {
        String txt = "AA.IsText()=bb || AND(dd=ss , tt=22)";
        ConditionTree tree = AlgorithmEngineHelper.ParseCondition(txt);

        assertEquals(ConditionTreeType.Or, tree.Type);
        ConditionTree t1 = tree.Nodes.get(0);
        ConditionTree t2 = tree.Nodes.get(1);
        assertEquals("AA.IsText()=bb", txt.substring(t1.Start, t1.End + 1));
        assertEquals("AND(dd=ss , tt=22)", txt.substring(t2.Start, t2.End + 1));
    }

    @Test
    public void Test6() {
        String txt = "AA.IsText()==bb && (dd=ss || tt=22)";
        ConditionTree tree = AlgorithmEngineHelper.ParseCondition(txt);

        assertEquals(ConditionTreeType.And, tree.Type);
        ConditionTree t1 = tree.Nodes.get(0);
        ConditionTree t2 = tree.Nodes.get(1);
        assertEquals(ConditionTreeType.Or, t2.Type);
        ConditionTree t3 = t2.Nodes.get(0);
        ConditionTree t4 = t2.Nodes.get(1);

        assertEquals("AA.IsText()==bb", txt.substring(t1.Start, t1.End + 1));
        assertEquals("dd=ss", txt.substring(t3.Start, t3.End + 1));
        assertEquals("tt=22", txt.substring(t4.Start, t4.End + 1));
    }

    @Test
    public void Test7() {
        String txt = "AA.IsText()==bb ? 1:2";
        ConditionTree t1 = AlgorithmEngineHelper.ParseCondition(txt);
        assertEquals(ConditionTreeType.String, t1.Type);
        assertEquals(txt, txt.substring(t1.Start, t1.End + 1));
    }

    @Test
    public void Test8() {
        String txt = "AA.IsText()==bb && (dd=ss || [tt]=22)";
        ConditionTree tree = AlgorithmEngineHelper.ParseCondition(txt);

        assertEquals(ConditionTreeType.And, tree.Type);
        ConditionTree t1 = tree.Nodes.get(0);
        ConditionTree t2 = tree.Nodes.get(1);
        assertEquals(ConditionTreeType.Or, t2.Type);
        ConditionTree t3 = t2.Nodes.get(0);
        ConditionTree t4 = t2.Nodes.get(1);

        assertEquals("AA.IsText()==bb", txt.substring(t1.Start, t1.End + 1));
        assertEquals("dd=ss", txt.substring(t3.Start, t3.End + 1));
        assertEquals("[tt]=22", txt.substring(t4.Start, t4.End + 1));
    }

    @Test
    public void Test9() {
        String txt = "AA.IsText()==bb && (dd=ss || [tt]==22)";
        ConditionTree tree = AlgorithmEngineHelper.ParseCondition(txt);

        assertEquals(ConditionTreeType.And, tree.Type);
        ConditionTree t1 = tree.Nodes.get(0);
        ConditionTree t2 = tree.Nodes.get(1);
        assertEquals(ConditionTreeType.Or, t2.Type);
        ConditionTree t3 = t2.Nodes.get(0);
        ConditionTree t4 = t2.Nodes.get(1);

        assertEquals("AA.IsText()==bb", txt.substring(t1.Start, t1.End + 1));
        assertEquals("dd=ss", txt.substring(t3.Start, t3.End + 1));
        assertEquals("[tt]==22", txt.substring(t4.Start, t4.End + 1));
    }

    @Test
    public void Test10() {
        String txt = "AA && (dd=ss || [tt]==22)";
        ConditionTree tree = AlgorithmEngineHelper.ParseCondition(txt);

        assertEquals(ConditionTreeType.And, tree.Type);
        ConditionTree t1 = tree.Nodes.get(0);
        ConditionTree t2 = tree.Nodes.get(1);
        assertEquals(ConditionTreeType.Or, t2.Type);
        ConditionTree t3 = t2.Nodes.get(0);
        ConditionTree t4 = t2.Nodes.get(1);

        assertEquals("AA", txt.substring(t1.Start, t1.End + 1));
        assertEquals("dd=ss", txt.substring(t3.Start, t3.End + 1));
        assertEquals("[tt]==22", txt.substring(t4.Start, t4.End + 1));
    }

    @Test
    public void Test11() {
        String txt = "1 && (dd=ss || [tt]==22)";
        ConditionTree tree = AlgorithmEngineHelper.ParseCondition(txt);

        assertEquals(ConditionTreeType.And, tree.Type);
        ConditionTree t1 = tree.Nodes.get(0);
        ConditionTree t2 = tree.Nodes.get(1);
        assertEquals(ConditionTreeType.Or, t2.Type);
        ConditionTree t3 = t2.Nodes.get(0);
        ConditionTree t4 = t2.Nodes.get(1);

        assertEquals("1", txt.substring(t1.Start, t1.End + 1));
        assertEquals("dd=ss", txt.substring(t3.Start, t3.End + 1));
        assertEquals("[tt]==22", txt.substring(t4.Start, t4.End + 1));
    }

    @Test
    public void TestError() {
        String txt = "";
        ConditionTree tree = AlgorithmEngineHelper.ParseCondition(txt);
        assertEquals(ConditionTreeType.Error, tree.Type);
        assertEquals("condition is null", tree.ErrorMessage);
    }

    @Test
    public void HasBracket_Test() {
        // 无括号时，二元节点的 HasBracket 均为 false
        ConditionTree tree = AlgorithmEngineHelper.ParseCondition("a || b && c");
        assertEquals(ConditionTreeType.Or, tree.Type);
        assertFalse(tree.HasBracket);
        assertFalse(tree.Nodes.get(0).HasBracket);
        assertFalse(tree.Nodes.get(1).HasBracket); // And 节点

        // 括号包裹叶子后，后续兄弟节点不应被误标记为 HasBracket（Bug 修复验证）
        tree = AlgorithmEngineHelper.ParseCondition("(a) || b && c");
        assertEquals(ConditionTreeType.Or, tree.Type);
        assertTrue(tree.Nodes.get(0).HasBracket);  // (a) 叶子
        assertFalse(tree.Nodes.get(1).HasBracket); // And 节点，无括号

        // 括号包裹二元节点时，该节点 HasBracket = true
        tree = AlgorithmEngineHelper.ParseCondition("(a || b) && c");
        assertEquals(ConditionTreeType.And, tree.Type);
        assertTrue(tree.Nodes.get(0).HasBracket); // (a || b) Or 节点

        tree = AlgorithmEngineHelper.ParseCondition("a && (b || c)");
        assertEquals(ConditionTreeType.And, tree.Type);
        assertTrue(tree.Nodes.get(1).HasBracket); // (b || c) Or 节点

        // 括号包裹叶子与二元节点混合
        tree = AlgorithmEngineHelper.ParseCondition("(a) && (b || c)");
        assertEquals(ConditionTreeType.And, tree.Type);
        assertTrue(tree.Nodes.get(0).HasBracket); // (a) 叶子
        assertTrue(tree.Nodes.get(1).HasBracket); // (b || c) Or 节点

        tree = AlgorithmEngineHelper.ParseCondition("(a) || (b)");
        assertEquals(ConditionTreeType.Or, tree.Type);
        assertTrue(tree.Nodes.get(0).HasBracket); // (a) 叶子
        assertTrue(tree.Nodes.get(1).HasBracket); // (b) 叶子

        // 多重括号
        tree = AlgorithmEngineHelper.ParseCondition("((a)) && b");
        assertEquals(ConditionTreeType.And, tree.Type);
        assertTrue(tree.Nodes.get(0).HasBracket);  // ((a)) 叶子
        assertFalse(tree.Nodes.get(1).HasBracket); // b 叶子
    }
}
