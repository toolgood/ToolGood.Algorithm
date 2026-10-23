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
        FunctionBase fun = CreateCalculate(tree, funExp);
        calculateCache.put(funExp, fun);
        return fun;
    }

    private FunctionBase CreateCalculate(CalculateTree tree, String exp) {
        if (tree.Type == CalculateTreeType.String) {
            // 仅叶子节点需要字符串内容；中间节点不再以子串作为缓存 key，避免 substring 产生 O(n²) 子串驻留
            String key = exp.substring(tree.Start, tree.End + 1);
            if (calculateCache.containsKey(key)) {
                return calculateCache.get(key);
            }
            try {
                FunctionBase fun = AlgorithmEngineHelper.ParseFormula(key);
                calculateCache.put(key, fun);
                return fun;
            } catch (Exception e) {
                throw new RuntimeException(e);
            }
        }
        if (tree.Type == CalculateTreeType.Error) {
            throw new RuntimeException(tree.ErrorMessage);
        }

        FunctionBase leftFunc = CreateCalculate(tree.Nodes.get(0), exp);
        FunctionBase rightFunc = CreateCalculate(tree.Nodes.get(1), exp);
        CombineCalculateType combineType = CombineCalculateType.intToEnum(tree.Type.getValue());
        return AlgorithmEngineHelper.CombineCalculate(leftFunc, combineType, rightFunc);
    }

    public FunctionBase ParseConditionWithCache(String funExp) {
        FunctionBase result = conditionCache.get(funExp);
        if (result != null) return result;
        ConditionTree tree = AlgorithmEngineHelper.ParseCondition(funExp);
        FunctionBase fun = CreateCondition(tree, funExp);
        conditionCache.put(funExp, fun);
        return fun;
    }

    private FunctionBase CreateCondition(ConditionTree tree, String exp) {
        if (tree.Type == ConditionTreeType.String) {
            return ParseWithCache(exp.substring(tree.Start, tree.End + 1));
        }
        if (tree.Type == ConditionTreeType.Error) {
            throw new RuntimeException(tree.ErrorMessage);
        }

        FunctionBase leftFunc = CreateCondition(tree.Nodes.get(0), exp);
        FunctionBase rightFunc = CreateCondition(tree.Nodes.get(1), exp);
        if (tree.Type == ConditionTreeType.And) {
            return AlgorithmEngineHelper.Condition_And(leftFunc, rightFunc);
        } else if (tree.Type == ConditionTreeType.Or) {
            return AlgorithmEngineHelper.Condition_Or(leftFunc, rightFunc);
        }
        throw new RuntimeException(tree.ErrorMessage);
    }
}
