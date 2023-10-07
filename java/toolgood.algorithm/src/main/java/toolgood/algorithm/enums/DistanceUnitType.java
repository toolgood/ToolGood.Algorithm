package toolgood.algorithm.enums;

public enum DistanceUnitType {
    /// <summary>
    /// 毫米
    /// </summary>
    MM(1),
    /// <summary>
    /// 厘米
    /// </summary>
    CM(2),
    /// <summary>
    /// 分米
    /// </summary>
    DM(3),
    /// <summary>
    /// 米
    /// </summary>
    M(4),
    /// <summary>
    /// 千米
    /// </summary>
    KM(5);

    private final int value;

    DistanceUnitType(int v) {
        value = v;
    }

    public static DistanceUnitType intToEnum(int value) {
        switch (value) {
            case 1:
                return MM;
            case 2:
                return CM;
            case 3:
                return DM;
            case 4:
                return M;
            case 5:
                return KM;
            default:
                return null;
        }
    }

    public int getValue() {
        return value;
    }
}
