package toolgood.algorithm.Tests4;

import org.junit.Test;
import toolgood.algorithm.internals.ConditionTree;
import toolgood.algorithm.enums.ConditionTreeType;

import static org.junit.Assert.assertEquals;

public class ConditionTreeTest {

    @Test
    public void Test1() {
        String txt = "AA.IsText() = bb";
        ConditionTree t1 = ConditionTree.Parse(txt);

        assertEquals(t1.Type, ConditionTreeType.String);
        assertEquals("AA.IsText() = bb", txt.substring(t1.Start, t1.End + 1));
        assertEquals("AA.IsText()=bb",t1.ConditionString);

        txt = "[bbb]=bb";
        t1 = ConditionTree.Parse(txt);
        assertEquals(t1.Type, ConditionTreeType.String);
        assertEquals("[bbb]=bb", txt.substring(t1.Start, t1.End + 1));
    }

    @Test
    public void Test2() {
        String txt = "AA.IsText()=bb && dd=ss";
        ConditionTree tree = ConditionTree.Parse(txt);

        assertEquals(tree.Type, ConditionTreeType.And);
        ConditionTree t1 = tree.Nodes.get(0);
        ConditionTree t2 = tree.Nodes.get(1);
        assertEquals("AA.IsText()=bb", txt.substring(t1.Start, t1.End + 1));
        assertEquals("dd=ss", txt.substring(t2.Start, t2.End + 1));
    }

    @Test

    public void Test3() {
        String txt = "AA.IsText()=bb || dd=ss";
        ConditionTree tree = ConditionTree.Parse(txt);

        assertEquals(tree.Type, ConditionTreeType.Or);
        ConditionTree t1 = tree.Nodes.get(0);
        ConditionTree t2 = tree.Nodes.get(1);
        assertEquals("AA.IsText()=bb", txt.substring(t1.Start, t1.End + 1));
        assertEquals("dd=ss", txt.substring(t2.Start, t2.End + 1));
    }


    @Test

    public void Test4() {
        String txt = "AA.IsText()=bb || (dd=ss && tt=22)";
        ConditionTree tree = ConditionTree.Parse(txt);

        assertEquals(tree.Type, ConditionTreeType.Or);
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
        ConditionTree tree = ConditionTree.Parse(txt);

        assertEquals(tree.Type, ConditionTreeType.Or);
        ConditionTree t1 = tree.Nodes.get(0);
        ConditionTree t2 = tree.Nodes.get(1);
        assertEquals("AA.IsText()=bb", txt.substring(t1.Start, t1.End + 1));
        assertEquals("AND(dd=ss , tt=22)", txt.substring(t2.Start, t2.End + 1));
    }

    @Test

    public void Test6() {
        String txt = "AA.IsText()==bb && (dd=ss || tt=22)";
        ConditionTree tree = ConditionTree.Parse(txt);

        assertEquals(tree.Type, ConditionTreeType.And);
        ConditionTree t1 = tree.Nodes.get(0);
        ConditionTree t2 = tree.Nodes.get(1);
        assertEquals(t2.Type, ConditionTreeType.Or);
        ConditionTree t3 = t2.Nodes.get(0);
        ConditionTree t4 = t2.Nodes.get(1);

        assertEquals("AA.IsText()==bb", txt.substring(t1.Start, t1.End + 1));
        assertEquals("dd=ss", txt.substring(t3.Start, t3.End + 1));
        assertEquals("tt=22", txt.substring(t4.Start, t4.End + 1));
    }

    @Test
    public void Test7() {
        String txt = "AA.IsText()==bb ? 1:2";
        ConditionTree t1 = ConditionTree.Parse(txt);
        assertEquals(t1.Type, ConditionTreeType.String);
        assertEquals("AA.IsText()==bb ? 1:2", txt.substring(t1.Start, t1.End + 1));
    }

    @Test
    public void Test8() {
        String txt = "AA.IsText()==bb && (dd=ss || {tt}=22)";
        ConditionTree tree = ConditionTree.Parse(txt);

        assertEquals(tree.Type, ConditionTreeType.And);
        ConditionTree t1 = tree.Nodes.get(0);
        ConditionTree t2 = tree.Nodes.get(1);
        assertEquals(t2.Type, ConditionTreeType.Or);
        ConditionTree t3 = t2.Nodes.get(0);
        ConditionTree t4 = t2.Nodes.get(1);

        assertEquals("AA.IsText()==bb", txt.substring(t1.Start, t1.End + 1));
        assertEquals("dd=ss", txt.substring(t3.Start, t3.End + 1));
        assertEquals("{tt}=22", txt.substring(t4.Start, t4.End + 1));
    }

    @Test
    public void Test9() {
        String txt = "AA.IsText()==bb && (dd=ss || [tt]==22)";
        ConditionTree tree = ConditionTree.Parse(txt);

        assertEquals(tree.Type, ConditionTreeType.And);
        ConditionTree t1 = tree.Nodes.get(0);
        ConditionTree t2 = tree.Nodes.get(1);
        assertEquals(t2.Type, ConditionTreeType.Or);
        ConditionTree t3 = t2.Nodes.get(0);
        ConditionTree t4 = t2.Nodes.get(1);

        assertEquals("AA.IsText()==bb", txt.substring(t1.Start, t1.End + 1));
        assertEquals("dd=ss", txt.substring(t3.Start, t3.End + 1));
        assertEquals("[tt]==22", txt.substring(t4.Start, t4.End + 1));
    }
}
