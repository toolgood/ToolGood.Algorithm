package toolgood.algorithm.enums;

public enum VolumeUnitType {
    /// <summary>
    /// 立方毫米
    /// </summary>
    MM3(21),
    /// <summary>
    /// 立方厘米
    /// </summary>
    CM3(22),
    /// <summary>
    /// 立方分米
    /// </summary>
    DM3(23),
    /// <summary>
    /// 立方米
    /// </summary>
    M3(24),
    /// <summary>
    /// 立方千米
    /// </summary>
    KM3(25);

    private final int value;

    VolumeUnitType(int v) {
        value = v;
    }

    public static VolumeUnitType intToEnum(int value) {
        switch (value) {
            case 21:
                return MM3;
            case 22:
                return CM3;
            case 23:
                return DM3;
            case 24:
                return M3;
            case 25:
                return KM3;
            default:
                return null;
        }
    }

    public int getValue() {
        return value;
    }
}
