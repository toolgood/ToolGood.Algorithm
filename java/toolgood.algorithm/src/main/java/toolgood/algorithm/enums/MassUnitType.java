package toolgood.algorithm.enums;

public enum MassUnitType {
    /// <summary>
    /// 克
    /// </summary>
    G(31),
    /// <summary>
    /// 千克
    /// </summary>
    KG(32),
    /// <summary>
    /// 吨
    /// </summary>
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
