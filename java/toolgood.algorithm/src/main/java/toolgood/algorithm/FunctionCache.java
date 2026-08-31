package toolgood.algorithm;

import toolgood.algorithm.enums.CalculateTreeType;
import toolgood.algorithm.enums.CombineCalculateType;
import toolgood.algorithm.enums.ConditionTreeType;
import toolgood.algorithm.internals.CalculateTree;
import toolgood.algorithm.internals.ConditionTree;
import toolgood.algorithm.internals.DiyNameInfo;
import toolgood.algorithm.internals.functions.FunctionBase;

import java.util.concurrent.ConcurrentHashMap;

public class FunctionCache implements IFunctionCache {
    private final ConcurrentHashMap<String, FunctionBase> calculateCache = new ConcurrentHashMap<>();
    private final ConcurrentHashMap<String, FunctionBase> conditionCache = new ConcurrentHashMap<>();
    private final ConcurrentHashMap<String, DiyNameInfo> diyNameCache = new ConcurrentHashMap<>();

    /**
     * 获取自定义名称信息，并使用缓存来提高性能。对于相同的表达式，将从缓存中返回之前解析的结果，而不是重新解析。
     */
    public DiyNameInfo GetDiyNamesWithCache(String exp) {
        DiyNameInfo result = diyNameCache.get(exp);
        if (result != null) return result;
        try {
            DiyNameInfo diyNameInfo = AlgorithmEngineHelper.GetDiyNames(exp);
            diyNameCache.put(exp, diyNameInfo);
            return diyNameInfo;
        } catch (Exception e) {
            throw new RuntimeException(e);
        }
    }

    public FunctionBase ParseWithCache(String funExp) {
        FunctionBase result = calculateCache.get(funExp);
        if (result != null) return result;
        CalculateTree tree = AlgorithmEngineHelper.ParseCalculate(funExp);
        return CreateCalculate(tree, funExp);
    }

    private FunctionBase CreateCalculate(CalculateTree tree, String exp) {
        if (tree.Type == CalculateTreeType.Error) {
            throw new RuntimeException(tree.ErrorMessage);
        }
        String key = exp.substring(tree.Start, tree.End + 1);
        if (calculateCache.containsKey(key)) {
            return calculateCache.get(key);
        }
        if (tree.Type == CalculateTreeType.String) {
            try {
                FunctionBase fun = AlgorithmEngineHelper.ParseFormula(key);
                calculateCache.put(key, fun);
                return fun;
            } catch (Exception e) {
                throw new RuntimeException(e);
            }
        }

        FunctionBase leftFunc = CreateCalculate(tree.Nodes.get(0), exp);
        FunctionBase rightFunc = CreateCalculate(tree.Nodes.get(1), exp);
        CombineCalculateType combineType = CombineCalculateType.intToEnum(tree.Type.getValue());
        FunctionBase fun = AlgorithmEngineHelper.CombineCalculate(leftFunc, combineType, rightFunc);
        calculateCache.put(key, fun);
        return fun;
    }

    public FunctionBase ParseConditionWithCache(String funExp) {
        FunctionBase result = conditionCache.get(funExp);
        if (result != null) return result;
        ConditionTree tree = AlgorithmEngineHelper.ParseCondition(funExp);
        return CreateCondition(tree, funExp);
    }

    private FunctionBase CreateCondition(ConditionTree tree, String exp) {
        if (tree.Type == ConditionTreeType.Error) {
            throw new RuntimeException(tree.ErrorMessage);
        }
        String key = exp.substring(tree.Start, tree.End + 1);
        if (conditionCache.containsKey(key)) {
            return conditionCache.get(key);
        }
        if (tree.Type == ConditionTreeType.String) {
            return ParseWithCache(key);
        }

        FunctionBase leftFunc = CreateCondition(tree.Nodes.get(0), exp);
        FunctionBase rightFunc = CreateCondition(tree.Nodes.get(1), exp);
        if (tree.Type == ConditionTreeType.And) {
            FunctionBase fun = AlgorithmEngineHelper.Condition_And(leftFunc, rightFunc);
            conditionCache.put(key, fun);
            return fun;
        } else if (tree.Type == ConditionTreeType.Or) {
            FunctionBase fun = AlgorithmEngineHelper.Condition_Or(leftFunc, rightFunc);
            conditionCache.put(key, fun);
            return fun;
        }
        throw new RuntimeException(tree.ErrorMessage);
    }
}
