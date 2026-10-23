package toolgood.algorithm.enums;

/**
 * 长度单位
 */
public enum DistanceUnitType {
    /**
     * 毫米
     */
    MM(1),
    /**
     * 厘米
     */
    CM(2),
    /**
     * 分米
     */
    DM(3),
    /**
     * 米
     */
    M(4),
    /**
     * 千米
     */
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
