package toolgood.algorithm.unitConversion;

class UnitNotSupportedException extends Exception {
    public UnitNotSupportedException() {
    }

    public UnitNotSupportedException(String unit) {
        super("The Unit '{" + unit + "}' is not supported by this converter");
    }
}
