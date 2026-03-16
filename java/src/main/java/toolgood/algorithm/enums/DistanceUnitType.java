/**
 * 长度单位
 */
package toolgood.algorithm.enums;

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
     * �?
     */
    M(4),

    /**
     * 千米
     */
    KM(5);

    private final byte value;

    DistanceUnitType(byte value) {
        this.value = value;
    }

    public byte getValue() {
        return value;
    }
}
