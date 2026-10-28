package toolgood.algorithm.internals.functions;

/**
 * SUMIF/COUNTIF/AVERAGEIF 的比较运算符。
 * 由 ParseSumIfMatch 在解析条件字符串时一次性确定，
 * 之后逐行比较只在循环外做一次分发，行内只剩 BigDecimal.compareTo。
 */
public enum CompareOperator {
    Lt, Le, Gt, Ge, Eq, Ne
}
