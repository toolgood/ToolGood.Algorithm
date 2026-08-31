import { AlgorithmEngineHelper } from './AlgorithmEngineHelper.js';
import { IFunctionCache } from './IFunctionCache.js';
import { CalculateTreeType } from './Enums/CalculateTreeType.js';
import { ConditionTreeType } from './Enums/ConditionTreeType.js';
import { Function_AND } from './Internals/Functions/Operator/Function_AND.js';
import { Function_OR } from './Internals/Functions/Operator/Function_OR.js';
import { Function_Add } from './Internals/Functions/Operator/Function_Add.js';
import { Function_Sub } from './Internals/Functions/Operator/Function_Sub.js';
import { Function_Mul } from './Internals/Functions/Operator/Function_Mul.js';
import { Function_Div } from './Internals/Functions/Operator/Function_Div.js';
import { Function_Mod } from './Internals/Functions/Operator/Function_Mod.js';
import { Function_Connect } from './Internals/Functions/Operator/Function_Connect.js';
import { Function_GT } from './Internals/Functions/Compare/Function_GT.js';
import { Function_LT } from './Internals/Functions/Compare/Function_LT.js';
import { Function_GE } from './Internals/Functions/Compare/Function_GE.js';
import { Function_LE } from './Internals/Functions/Compare/Function_LE.js';
import { Function_EQ } from './Internals/Functions/Compare/Function_EQ.js';
import { Function_NE } from './Internals/Functions/Compare/Function_NE.js';

/**
 * 函数缓存类，使用 Map 实现函数缓存。
 */
export class FunctionCache extends IFunctionCache {
    constructor() {
        super();
        this.calculateCache = new Map();
        this.conditionCache = new Map();
        this.diyNameCache = new Map();
    }

    /**
     * 获取自定义名称信息，并使用缓存来提高性能。对于相同的表达式，将从缓存中返回之前解析的结果，而不是重新解析。
     * @param {string} exp
     * @returns {DiyNameInfo}
     */
    GetDiyNamesWithCache(exp) {
        let result = this.diyNameCache.get(exp);
        if (result != null) return result;
        const diyNameInfo = AlgorithmEngineHelper.GetDiyNames(exp);
        this.diyNameCache.set(exp, diyNameInfo);
        return diyNameInfo;
    }

    /**
     * 解析函数表达式，并使用缓存来提高性能。对于相同的函数表达式，函数将从缓存中返回之前解析的结果，而不是重新解析。
     * @param {string} funExp
     * @returns {FunctionBase}
     */
    ParseWithCache(funExp) {
        let result = this.calculateCache.get(funExp);
        if (result != null) return result;
        const tree = AlgorithmEngineHelper.ParseCalculate(funExp);
        const fun = this.CreateCalculate(tree, funExp);
        this.calculateCache.set(funExp, fun);
        return fun;
    }

    CreateCalculate(tree, exp) {
        if (tree.Type === CalculateTreeType.String) {
            // 仅叶子节点需要字符串内容；中间节点不再以子串作为缓存 key，避免 substring 产生 O(n²) 子串驻留
            const key = exp.substring(tree.start, tree.end + 1);
            if (this.calculateCache.has(key)) {
                return this.calculateCache.get(key);
            }
            const fun = AlgorithmEngineHelper.ParseFormula(key);
            this.calculateCache.set(key, fun);
            return fun;
        }
        if (tree.Type === CalculateTreeType.Error) {
            throw new Error(tree.ErrorMessage);
        }

        const leftFunc = this.CreateCalculate(tree.nodes[0], exp);
        const rightFunc = this.CreateCalculate(tree.nodes[1], exp);
        return this.CombineCalculate(leftFunc, tree.Type, rightFunc);
    }

    CombineCalculate(left, type, right) {
        const funcs = [left, right];
        switch (type) {
            case CalculateTreeType.Add: return new Function_Add(funcs);
            case CalculateTreeType.Sub: return new Function_Sub(funcs);
            case CalculateTreeType.Mul: return new Function_Mul(funcs);
            case CalculateTreeType.Div: return new Function_Div(funcs);
            case CalculateTreeType.Mod: return new Function_Mod(funcs);
            case CalculateTreeType.Connect: return new Function_Connect(funcs);
            case CalculateTreeType.And: return new Function_AND(funcs);
            case CalculateTreeType.Or: return new Function_OR(funcs);
            case CalculateTreeType.OpGt: return new Function_GT(funcs);
            case CalculateTreeType.OpLt: return new Function_LT(funcs);
            case CalculateTreeType.OpGe: return new Function_GE(funcs);
            case CalculateTreeType.OpLe: return new Function_LE(funcs);
            case CalculateTreeType.OpEq: return new Function_EQ(funcs);
            case CalculateTreeType.OpNe:
            default: return new Function_NE(funcs);
        }
    }

    /**
     * 解析条件表达式，并使用缓存来提高性能。对于相同的条件表达式，函数将从缓存中返回之前解析的结果，而不是重新解析。
     * @param {string} funExp
     * @returns {FunctionBase}
     */
    ParseConditionWithCache(funExp) {
        let result = this.conditionCache.get(funExp);
        if (result != null) return result;
        const tree = AlgorithmEngineHelper.ParseCondition(funExp);
        const fun = this.CreateCondition(tree, funExp);
        this.conditionCache.set(funExp, fun);
        return fun;
    }

    CreateCondition(tree, exp) {
        if (tree.Type === ConditionTreeType.String) {
            // 直接走计算树路径，避免对同一 key 重入
            return this.ParseWithCache(exp.substring(tree.start, tree.end + 1));
        }
        if (tree.Type === ConditionTreeType.Error) {
            throw new Error(tree.ErrorMessage);
        }

        const leftFunc = this.CreateCondition(tree.nodes[0], exp);
        const rightFunc = this.CreateCondition(tree.nodes[1], exp);
        if (tree.Type === ConditionTreeType.And) {
            return AlgorithmEngineHelper.Condition_And(leftFunc, rightFunc);
        } else if (tree.Type === ConditionTreeType.Or) {
            return AlgorithmEngineHelper.Condition_Or(leftFunc, rightFunc);
        }
        throw new Error(tree.ErrorMessage);
    }
}

// 浏览器支持
if (typeof window !== 'undefined') {
    window.FunctionCache = FunctionCache;
}
