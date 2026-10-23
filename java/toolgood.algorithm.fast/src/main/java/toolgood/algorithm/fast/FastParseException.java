package toolgood.algorithm.fast;

/**
 * 词法 / 语法分析阶段抛出的异常。
 * <p>与「函数构造期」的 {@link IllegalArgumentException}（参数个数或类型不合法）区分开：
 * {@code AlgorithmEngine.ParseWithoutError} 只把这类错误吞掉并写入 {@code LastError}，
 * 构造期异常则与 ANTLR 版一致地向外抛出。</p>
 */
public class FastParseException extends IllegalArgumentException {
    private static final long serialVersionUID = 1L;

    public FastParseException(String message) {
        super(message);
    }

    public FastParseException(String message, Throwable cause) {
        super(message, cause);
    }
}
