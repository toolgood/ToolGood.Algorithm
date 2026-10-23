package toolgood.algorithm.enums;

/**
 * 计算树类型
 */
public enum CalculateTreeType {
    /**
     * 文本
     */
    String(0),
    /**
     * 加
     */
    Add(1),
    /**
     * 减
     */
    Sub(2),
    /**
     * 乘
     */
    Mul(3),
    /**
     * 除
     */
    Div(4),
    /**
     * 求模
     */
    Mod(5),
    /**
     * 连接
     */
    Connect(6),
    /**
     * 并
     */
    And(7),
    /**
     * 或
     */
    Or(8),
    /**
     * 大于
     */
    OpGt(9),
    /**
     * 小于
     */
    OpLt(10),
    /**
     * 大于等于
     */
    OpGe(11),
    /**
     * 小于等于
     */
    OpLe(12),
    /**
     * 等于
     */
    OpEq(13),
    /**
     * 不等于
     */
    OpNe(14),
    /**
     * 错误
     */
    Error(15);

    private final int value;

    CalculateTreeType(int v) {
        value = v;
    }

    public static CalculateTreeType intToEnum(int value) {
        switch (value) {
            case 0:
                return String;
            case 1:
                return Add;
            case 2:
                return Sub;
            case 3:
                return Mul;
            case 4:
                return Div;
            case 5:
                return Mod;
            case 6:
                return Connect;
            case 7:
                return And;
            case 8:
                return Or;
            case 9:
                return OpGt;
            case 10:
                return OpLt;
            case 11:
                return OpGe;
            case 12:
                return OpLe;
            case 13:
                return OpEq;
            case 14:
                return OpNe;
            case 15:
                return Error;
            default:
                return null;
        }
    }

    public int getValue() {
        return value;
    }
}
