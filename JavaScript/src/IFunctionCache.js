/**
 * 函数缓存接口。
 * JavaScript 无原生接口，使用抽象基类模拟：子类须重写以下方法，否则调用时抛出未实现错误。
 */
export class IFunctionCache {
    /**
     * 获取自定义名称信息，并使用缓存来提高性能。对于相同的表达式，将从缓存中返回之前解析的结果，而不是重新解析。
     * @param {string} exp
     * @returns {DiyNameInfo}
     */
    GetDiyNamesWithCache(exp) {
        throw new Error('IFunctionCache.GetDiyNamesWithCache() 未实现');
    }

    /**
     * 解析函数表达式，并使用缓存来提高性能。对于相同的函数表达式，函数将从缓存中返回之前解析的结果，而不是重新解析。
     * @param {string} funExp
     * @returns {FunctionBase}
     */
    ParseWithCache(funExp) {
        throw new Error('IFunctionCache.ParseWithCache() 未实现');
    }

    /**
     * 解析条件表达式，并使用缓存来提高性能。对于相同的条件表达式，函数将从缓存中返回之前解析的结果，而不是重新解析。
     * @param {string} funExp
     * @returns {FunctionBase}
     */
    ParseConditionWithCache(funExp) {
        throw new Error('IFunctionCache.ParseConditionWithCache() 未实现');
    }
}

// 浏览器支持
if (typeof window !== 'undefined') {
    window.IFunctionCache = IFunctionCache;
}
