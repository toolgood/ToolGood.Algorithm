package toolgood.algorithm;

import toolgood.algorithm.enums.CalculateTreeType;
import toolgood.algorithm.enums.CombineCalculateType;
import toolgood.algorithm.enums.ConditionTreeType;
import toolgood.algorithm.internals.CalculateTree;
import toolgood.algorithm.internals.ConditionTree;
import toolgood.algorithm.internals.functions.FunctionBase;

import java.util.concurrent.ConcurrentHashMap;

public class FunctionCache {
    private final ConcurrentHashMap<String, FunctionBase> functionCache = new ConcurrentHashMap<>();

    public FunctionBase ParseWithCache(String funExp) {
        return functionCache.computeIfAbsent(funExp, key -> {
            CalculateTree tree = AlgorithmEngineHelper.ParseCalculate(key);
            return CreateCalculate(tree);
        });
    }

    private FunctionBase CreateCalculate(CalculateTree tree) {
        if (functionCache.containsKey(tree.Text)) {
            return functionCache.get(tree.Text);
        }
        if (tree.Type == CalculateTreeType.String) {
            try {
                return AlgorithmEngineHelper.ParseFormula(tree.Text);
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
        functionCache.put(tree.Text, fun);
        return fun;
    }

    public FunctionBase ParseConditionWithCache(String funExp) {
        return functionCache.computeIfAbsent(funExp, key -> {
            ConditionTree tree = AlgorithmEngineHelper.ParseCondition(key);
            return CreateCondition(tree);
        });
    }

    private FunctionBase CreateCondition(ConditionTree tree) {
        if (functionCache.containsKey(tree.Text)) {
            return functionCache.get(tree.Text);
        }
        if (tree.Type == ConditionTreeType.String) {
            return ParseWithCache(tree.Text);
        }

        FunctionBase leftFunc = CreateCondition(tree.Nodes.get(0));
        FunctionBase rightFunc = CreateCondition(tree.Nodes.get(1));
        if (tree.Type == ConditionTreeType.And) {
            FunctionBase fun = AlgorithmEngineHelper.Condition_And(leftFunc, rightFunc);
            functionCache.put(tree.Text, fun);
            return fun;
        } else if (tree.Type == ConditionTreeType.Or) {
            FunctionBase fun = AlgorithmEngineHelper.Condition_Or(leftFunc, rightFunc);
            functionCache.put(tree.Text, fun);
            return fun;
        }
        throw new RuntimeException(tree.ErrorMessage);
    }
}
