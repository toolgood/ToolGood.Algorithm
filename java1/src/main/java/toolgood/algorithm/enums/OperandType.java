/**
 * 操作数类型
 */
package toolgood.algorithm.enums;

public enum OperandType {
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
    ARRARY,

    /**
     * 数字
     */
    NUMBER,

    /**
     * 布尔
     */
    BOOLEAN,

    /**
     * 字符串
     */
    TEXT,

    /**
     * JSON格式
     */
    JSON,

    /**
     * JSON格式
     */
    ARRARYJSON;

    private final byte value;

    OperandType() {
        this.value = (byte) ordinal();
    }

    public byte getValue() {
        return value;
    }
}
