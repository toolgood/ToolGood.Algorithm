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

    private final int value;

    NumberUnitType(int v) {
        value = v;
    }

    public static NumberUnitType intToEnum(int value) {
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
            case 31:
                return G;
            case 32:
                return KG;
            case 33:
                return T;
            default:
                return null;
        }
    }

    public int getValue() {
        return value;
    }
}
