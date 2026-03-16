/**
 * 重量单位
 */
package toolgood.algorithm.enums;

public enum MassUnitType {
    /**
     * �?
     */
    G(31),

    /**
     * 千克
     */
    KG(32),

    /**
     * �?
     */
    T(33);

    private final byte value;

    MassUnitType(byte value) {
        this.value = value;
    }

    public byte getValue() {
        return value;
    }
}
