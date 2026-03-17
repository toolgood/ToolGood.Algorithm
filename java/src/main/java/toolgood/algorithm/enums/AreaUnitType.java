/**
 * 面积单位
 */
package toolgood.algorithm.enums;

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
     * 平方�?
     */
    M2(14),

    /**
     * 平方千米
     */
    KM2(15);

    private final int value;

    AreaUnitType(int value) {
        this.value = value;
    }

    public int getValue() {
        return value;
    }
}
