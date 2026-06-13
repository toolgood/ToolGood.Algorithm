package toolgood.algorithm.conditiontrees;

import org.junit.Test;
import static org.junit.Assert.*;

import toolgood.algorithm.AlgorithmEngineHelper;
import toolgood.algorithm.enums.CalculateTreeType;
import toolgood.algorithm.internals.CalculateTree;

public class CalculateTreeTest {

    @Test
    public void Test1() {
        String txt = "A1+1";
        CalculateTree t1 = AlgorithmEngineHelper.ParseCalculate(txt);
        assertEquals(CalculateTreeType.Add, t1.Type);
        assertEquals("A1+1", txt.substring(t1.Start, t1.End + 1));
        assertEquals("A1+1", t1.Text);

        txt = "A1-(1+1)";
        t1 = AlgorithmEngineHelper.ParseCalculate(txt);
        assertEquals(CalculateTreeType.Sub, t1.Type);
        assertEquals("1+1", t1.Nodes.get(1).Text);

        txt = "A1*(1+1)";
        t1 = AlgorithmEngineHelper.ParseCalculate(txt);
        assertEquals(CalculateTreeType.Mul, t1.Type);

        txt = "A1/(1+1)";
        t1 = AlgorithmEngineHelper.ParseCalculate(txt);
        assertEquals(CalculateTreeType.Div, t1.Type);

        txt = "A1%(1+1)";
        t1 = AlgorithmEngineHelper.ParseCalculate(txt);
        assertEquals(CalculateTreeType.Mod, t1.Type);

        txt = "A1&(1+1)";
        t1 = AlgorithmEngineHelper.ParseCalculate(txt);
        assertEquals(CalculateTreeType.Connect, t1.Type);

        txt = "A1(1+1)";
        t1 = AlgorithmEngineHelper.ParseCalculate(txt);
        assertEquals(CalculateTreeType.String, t1.Type);

        txt = "A1(1+1)-";
        t1 = AlgorithmEngineHelper.ParseCalculate(txt);
        assertEquals(CalculateTreeType.Error, t1.Type);

        txt = "-1+(1+1)";
        t1 = AlgorithmEngineHelper.ParseCalculate(txt);
        assertEquals(CalculateTreeType.Add, t1.Type);

        txt = "-1";
        t1 = AlgorithmEngineHelper.ParseCalculate(txt);
        assertEquals(CalculateTreeType.String, t1.Type);
    }

    @Test
    public void TestError() {
        String txt = "";
        CalculateTree tree = AlgorithmEngineHelper.ParseCalculate(txt);
        assertEquals(CalculateTreeType.Error, tree.Type);
        assertEquals("exp is null", tree.ErrorMessage);
    }

    @Test
    public void TestComplexExpressions() {
        String txt = "A1+B1*C1";
        CalculateTree tree = AlgorithmEngineHelper.ParseCalculate(txt);
        assertEquals(CalculateTreeType.Add, tree.Type);

        txt = "A1*(B1+C1)/D1";
        tree = AlgorithmEngineHelper.ParseCalculate(txt);
        assertEquals(CalculateTreeType.Div, tree.Type);

        txt = "A1+B1-C1*D1/E1";
        tree = AlgorithmEngineHelper.ParseCalculate(txt);
        assertEquals(CalculateTreeType.Sub, tree.Type);
    }
}
