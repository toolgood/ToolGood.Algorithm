package toolgood.algorithm.enums;

/**
 * 体积单位
 */
public enum VolumeUnitType {
    /**
     * 立方毫米
     */
    MM3(21),
    /**
     * 立方厘米
     */
    CM3(22),
    /**
     * 立方分米
     */
    DM3(23),
    /**
     * 立方米
     */
    M3(24),
    /**
     * 立方千米
     */
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
