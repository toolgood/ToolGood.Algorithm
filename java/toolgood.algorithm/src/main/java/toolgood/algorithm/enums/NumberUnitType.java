package toolgood.algorithm.enums;

public enum NumberUnitType {
    MM(1),
    CM(2),
    DM(3),
    M(4),
    KM(5),

    MM2(11),
    CM2(12),
    DM2(13),
    M2(14),
    KM2(15),

    MM3(21),
    CM3(22),
    DM3(23),
    M3(24),
    KM3(25),

    G(31),
    KG(32),
    T(33);

    private final byte value;

    NumberUnitType(byte value) {
        this.value = value;
    }

    public byte getValue() {
        return value;
    }
}
