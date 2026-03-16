/**
 * 操作数类�?
 */
package toolgood.algorithm.enums;

public enum OperandType {
    /**
     * NONE（用于参数类型推断）
     */
    NONE,

    /**
     * NULL
     */
    NULL,

    /**
     * 错误
     */
    ERROR,

    /**
     * 日期
     */
    DATE,

    /**
     * 数组
     */
    ARRAY,

    /**
     * 数字
     */
    NUMBER,

    /**
     * 布尔
     */
    BOOLEAN,

    /**
     * 字符�?
     */
    TEXT,

    /**
     * JSON格式
     */
    JSON,

    /**
     * JSON格式
     */
    ARRAYJSON;

    private final byte value;

    OperandType() {
        this.value = (byte) ordinal();
    }

    public byte getValue() {
        return value;
    }
}
