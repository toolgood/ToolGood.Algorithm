class UnitNotSupportedException extends Error {
    constructor(unit) {
        super(`The Unit '${unit}' is not supported by this converter`);
        this.name = 'UnitNotSupportedException';
    }
}

export { UnitNotSupportedException };