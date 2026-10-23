package toolgood.algorithm;

import toolgood.algorithm.internals.DiyNameInfo;
import toolgood.algorithm.internals.functions.FunctionBase;

/**
 * 函数缓存接口，定义对计算表达式、条件表达式以及自定义名称解析结果的缓存行为。
 */
public interface IFunctionCache {

    /**
     * 获取自定义名称信息，并使用缓存来提高性能。对于相同的表达式，将从缓存中返回之前解析的结果，而不是重新解析。
     *
     * @param exp 表达式字符串
     * @return 自定义名称信息
     */
    DiyNameInfo GetDiyNamesWithCache(String exp);

    /**
     * 解析函数表达式，并使用缓存来提高性能。对于相同的函数表达式，函数将从缓存中返回之前解析的结果，而不是重新解析。
     *
     * @param funExp 函数表达式字符串
     * @return 解析后的函数对象
     */
    FunctionBase ParseWithCache(String funExp);

    /**
     * 解析条件表达式，并使用缓存来提高性能。对于相同的条件表达式，函数将从缓存中返回之前解析的结果，而不是重新解析。
     *
     * @param funExp 条件表达式字符串
     * @return 解析后的函数对象
     */
    FunctionBase ParseConditionWithCache(String funExp);
}
