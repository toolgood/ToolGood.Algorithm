/**
 * 体积单位
 */
package toolgood.algorithm.enums;

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
     * 立方�?
     */
    M3(24),

    /**
     * 立方千米
     */
    KM3(25);

    private final byte value;

    VolumeUnitType(byte value) {
        this.value = value;
    }

    public byte getValue() {
        return value;
    }
}
