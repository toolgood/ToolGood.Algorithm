package toolgood.algorithm;

import org.junit.Test;
import static org.junit.Assert.*;

import toolgood.algorithm.internals.functions.FunctionBase;

public class FunctionCacheTest {

    @Test
    public void ParseWithCache_FormulaExpression_ReturnsFunctionBase() {
        FunctionCache cache = new FunctionCache();
        FunctionBase result = cache.ParseWithCache("abs(1)");
        assertNotNull(result);
    }

    @Test
    public void ParseWithCache_SameFormula_ReturnsSameInstance() {
        FunctionCache cache = new FunctionCache();
        FunctionBase result1 = cache.ParseWithCache("abs(1)");
        FunctionBase result2 = cache.ParseWithCache("abs(1)");
        assertSame(result1, result2);
    }

    @Test
    public void ParseWithCache_DifferentFormulas_ReturnsDifferentInstances() {
        FunctionCache cache = new FunctionCache();
        FunctionBase result1 = cache.ParseWithCache("abs(1)");
        FunctionBase result2 = cache.ParseWithCache("abs(2)");
        assertNotSame(result1, result2);
    }

    @Test
    public void ParseWithCache_MultipleCalls_SameFormula_ReturnsConsistentInstance() {
        FunctionCache cache = new FunctionCache();
        FunctionBase r1 = cache.ParseWithCache("if(true,1,2)");
        FunctionBase r2 = cache.ParseWithCache("if(true,1,2)");
        FunctionBase r3 = cache.ParseWithCache("if(true,1,2)");
        assertSame(r1, r2);
        assertSame(r2, r3);
        assertNotNull(r1);
    }

    @Test
    public void ParseWithCache_DiyExpression_ReturnsFunctionBase() {
        FunctionCache cache = new FunctionCache();
        FunctionBase result = cache.ParseWithCache("myVar");
        assertNotNull(result);
    }

    @Test
    public void ParseWithCache_NestedFunction_ReturnsFunctionBase() {
        FunctionCache cache = new FunctionCache();
        FunctionBase result = cache.ParseWithCache("max(1,2,3)");
        assertNotNull(result);
    }

    @Test
    public void ParseWithCache_FormulaCorrectName() {
        FunctionCache cache = new FunctionCache();
        FunctionBase result = cache.ParseWithCache("abs(1)");
        assertNotNull(result);
        assertNotNull(result.Name());
    }

    @Test
    public void ParseConditionWithCache_SimpleCondition_ReturnsFunctionBase() {
        FunctionCache cache = new FunctionCache();
        FunctionBase result = cache.ParseConditionWithCache("1=1");
        assertNotNull(result);
    }

    @Test
    public void ParseConditionWithCache_AndCondition_ReturnsFunctionBase() {
        FunctionCache cache = new FunctionCache();
        FunctionBase result = cache.ParseConditionWithCache("1=1 && 2=2");
        assertNotNull(result);
    }

    @Test
    public void ParseConditionWithCache_OrCondition_ReturnsFunctionBase() {
        FunctionCache cache = new FunctionCache();
        FunctionBase result = cache.ParseConditionWithCache("1=1 || 2=2");
        assertNotNull(result);
    }

    @Test
    public void ParseConditionWithCache_SameCondition_ReturnsSameInstance() {
        FunctionCache cache = new FunctionCache();
        FunctionBase result1 = cache.ParseConditionWithCache("1=1");
        FunctionBase result2 = cache.ParseConditionWithCache("1=1");
        assertSame(result1, result2);
    }

    @Test
    public void ParseConditionWithCache_DifferentConditions_ReturnsDifferentInstances() {
        FunctionCache cache = new FunctionCache();
        FunctionBase result1 = cache.ParseConditionWithCache("1=1");
        FunctionBase result2 = cache.ParseConditionWithCache("2=2");
        assertNotSame(result1, result2);
    }

    @Test
    public void ParseConditionWithCache_ComplexCondition_ReturnsFunctionBase() {
        FunctionCache cache = new FunctionCache();
        FunctionBase result = cache.ParseConditionWithCache("(1=1 || 2=2) && 3=3");
        assertNotNull(result);
    }

    @Test
    public void ParseConditionWithCache_StringCondition_DelegatesToParseWithCache() {
        FunctionCache cache = new FunctionCache();
        FunctionBase result = cache.ParseConditionWithCache("abs(1) > 0");
        assertNotNull(result);
    }

    @Test
    public void CrossCheck_ParseWithCache_And_ParseConditionWithCache_Independent() {
        FunctionCache cache = new FunctionCache();
        FunctionBase f1 = cache.ParseWithCache("abs(1)");
        FunctionBase f2 = cache.ParseConditionWithCache("1=1");
        assertNotNull(f1);
        assertNotNull(f2);
        assertNotSame(f1, f2);
    }
}
