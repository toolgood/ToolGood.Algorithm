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
     * 平方米
     */
    M2(14),

    /**
     * 平方千米
     */
    KM2(15);

    private final byte value;

    AreaUnitType(byte value) {
        this.value = value;
    }

    public byte getValue() {
        return value;
    }
}
