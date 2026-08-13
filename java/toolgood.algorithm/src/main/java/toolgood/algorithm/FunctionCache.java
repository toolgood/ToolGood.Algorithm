package toolgood.algorithm;

import toolgood.algorithm.enums.CalculateTreeType;
import toolgood.algorithm.enums.CombineCalculateType;
import toolgood.algorithm.enums.ConditionTreeType;
import toolgood.algorithm.internals.CalculateTree;
import toolgood.algorithm.internals.ConditionTree;
import toolgood.algorithm.internals.functions.FunctionBase;

import java.util.concurrent.ConcurrentHashMap;

public class FunctionCache {
    private final ConcurrentHashMap<String, FunctionBase> calculateCache = new ConcurrentHashMap<>();
    private final ConcurrentHashMap<String, FunctionBase> conditionCache = new ConcurrentHashMap<>();

    public FunctionBase ParseWithCache(String funExp) {
        FunctionBase result = calculateCache.get(funExp);
        if (result != null) return result;
        CalculateTree tree = AlgorithmEngineHelper.ParseCalculate(funExp);
        return CreateCalculate(tree);
    }

    private FunctionBase CreateCalculate(CalculateTree tree) {
        if (calculateCache.containsKey(tree.Text)) {
            return calculateCache.get(tree.Text);
        }
        if (tree.Type == CalculateTreeType.String) {
            try {
                FunctionBase fun = AlgorithmEngineHelper.ParseFormula(tree.Text);
                calculateCache.put(tree.Text, fun);
                return fun;
            } catch (Exception e) {
                throw new RuntimeException(e);
            }
        }
        if (tree.Type == CalculateTreeType.Error) {
            throw new RuntimeException(tree.ErrorMessage);
        }

        FunctionBase leftFunc = CreateCalculate(tree.Nodes.get(0));
        FunctionBase rightFunc = CreateCalculate(tree.Nodes.get(1));
        CombineCalculateType combineType = CombineCalculateType.intToEnum(tree.Type.getValue());
        FunctionBase fun = AlgorithmEngineHelper.CombineCalculate(leftFunc, combineType, rightFunc);
        calculateCache.put(tree.Text, fun);
        return fun;
    }

    public FunctionBase ParseConditionWithCache(String funExp) {
        FunctionBase result = conditionCache.get(funExp);
        if (result != null) return result;
        ConditionTree tree = AlgorithmEngineHelper.ParseCondition(funExp);
        return CreateCondition(tree);
    }

    private FunctionBase CreateCondition(ConditionTree tree) {
        if (conditionCache.containsKey(tree.Text)) {
            return conditionCache.get(tree.Text);
        }
        if (tree.Type == ConditionTreeType.String) {
            return ParseWithCache(tree.Text);
        }

        FunctionBase leftFunc = CreateCondition(tree.Nodes.get(0));
        FunctionBase rightFunc = CreateCondition(tree.Nodes.get(1));
        if (tree.Type == ConditionTreeType.And) {
            FunctionBase fun = AlgorithmEngineHelper.Condition_And(leftFunc, rightFunc);
            conditionCache.put(tree.Text, fun);
            return fun;
        } else if (tree.Type == ConditionTreeType.Or) {
            FunctionBase fun = AlgorithmEngineHelper.Condition_Or(leftFunc, rightFunc);
            conditionCache.put(tree.Text, fun);
            return fun;
        }
        throw new RuntimeException(tree.ErrorMessage);
    }
}
