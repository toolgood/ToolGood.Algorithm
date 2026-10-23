package toolgood.algorithm.fast;

/**
 * 词法单元：类型 + 在原文中的起止下标（End 为闭区间）。
 * <p>可变对象，词法扫描时复用同一个实例，避免为每个 Token 分配对象。</p>
 */
final class FastLexeme {
    int type;
    int start;
    int end;
}
