package toolgood.algorithm.internals.functions;

public enum StringComparison {
    Ordinal(0),
    OrdinalIgnoreCase(1);

    public final int value;

    StringComparison(int value) {
        this.value = value;
    }
}
