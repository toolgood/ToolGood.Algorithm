package toolgood.algorithm.enums;

/**
 * 条件树类型
 */
public enum ConditionTreeType {
    /**
     * 文本
     */
    String(0),
    /**
     * 并
     */
    And(1),
    /**
     * 或
     */
    Or(2),
    /**
     * 错误
     */
    Error(3);

    private final int value;

    ConditionTreeType(int v) {
        value = v;
    }

    public static ConditionTreeType intToEnum(int value) {
        switch (value) {
            case 0:
                return String;
            case 1:
                return And;
            case 2:
                return Or;
            case 3:
                return Error;
            default:
                return null;
        }
    }

    public int getValue() {
        return value;
    }
}
