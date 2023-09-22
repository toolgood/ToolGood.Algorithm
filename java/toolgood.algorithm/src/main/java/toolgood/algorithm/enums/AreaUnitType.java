package toolgood.algorithm.enums;

public enum AreaUnitType {
    /// <summary>
    /// 平方毫米
    /// </summary>
    MM2(11),
    /// <summary>
    /// 平方厘米
    /// </summary>
    CM2(12),
    /// <summary>
    /// 平方分米
    /// </summary>
    DM2(13),
    /// <summary>
    /// 平方米
    /// </summary>
    M2(14),
    /// <summary>
    /// 平方千米
    /// </summary>
    KM2(15);

    private final int value;

    AreaUnitType(int v) {
        value = v;
    }

    public static AreaUnitType intToEnum(int value) {
        switch (value) {
            case 11:
                return MM2;
            case 12:
                return CM2;
            case 13:
                return DM2;
            case 14:
                return M2;
            case 15:
                return KM2;
            default:
                return null;
        }
    }

    public int getValue() {
        return value;
    }
}
