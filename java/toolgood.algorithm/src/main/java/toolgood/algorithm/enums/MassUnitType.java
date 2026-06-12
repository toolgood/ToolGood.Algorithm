package toolgood.algorithm.enums;

/**
 * 重量单位
 */
public enum MassUnitType {
    /**
     * 克
     */
    G(31),
    /**
     * 千克
     */
    KG(32),
    /**
     * 吨
     */
    T(33);

    private final int value;

    MassUnitType(int v) {
        value = v;
    }

    public static MassUnitType intToEnum(int value) {
        switch (value) {
            case 31:
                return G;
            case 32:
                return KG;
            case 33:
                return T;
            default:
                return null;
        }
    }

    public int getValue() {
        return value;
    }
}
