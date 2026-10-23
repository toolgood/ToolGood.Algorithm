package toolgood.algorithm.enums;

/**
 * 面积单位
 */
public enum AreaUnitType {
    /**
     * 平方毫米
     */
    MM2(11),
    /**
     * 平方厘米
     */
    CM2(12),
    /**
     * 平方分米
     */
    DM2(13),
    /**
     * 平方米
     */
    M2(14),
    /**
     * 平方千米
     */
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
