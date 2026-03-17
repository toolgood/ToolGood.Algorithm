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

    private final int value;

    MassUnitType(int value) {
        this.value = value;
    }

    public int getValue() {
        return value;
    }
}
