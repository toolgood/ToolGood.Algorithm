const units2 = {
    "m²": 1, "m2": 1, "square metre": 1, "square meter": 1, "centiare": 1, "平方米": 1, "平方公尺": 1,
    "km²": 0.000001, "km2": 0.000001, "square kilometre": 0.000001, "square kilometer": 0.000001, "平方千米": 0.000001,
    "dm²": 100, "dm2": 100, "square decimetre": 100, "square decimeter": 100, "平方分米": 100,
    "cm²": 10000, "cm2": 10000, "square centimetre": 10000, "square centimeter": 10000, "平方厘米": 10000,
    "mm²": 1000000, "mm2": 1000000, "square millimetre": 1000000, "square millimeter": 1000000, "平方毫米": 1000000,
    "ft²": 1 / 0.3048 / 0.3048, "ft2": 1 / 0.3048 / 0.3048, "square foot": 1 / 0.3048 / 0.3048, "square feet": 1 / 0.3048 / 0.3048, "sq ft": 1 / 0.3048 / 0.3048, "平方英尺": 1 / 0.3048 / 0.3048,
    "yd²": 1 / 0.9144 / 0.9144, "yd2": 1 / 0.9144 / 0.9144, "sq yd": 1 / 0.9144 / 0.9144, "square yard": 1 / 0.9144 / 0.9144, "平方码": 1 / 0.9144 / 0.9144,
    "a": 0.01, "are": 0.01,
    "ha": 0.0001, "hectare": 0.0001, "公顷": 0.0001,
    "in²": 1 / 0.00064516, "in2": 1 / 0.00064516, "sq in": 1 / 0.00064516, "square inch": 1 / 0.00064516, "平方英寸": 1 / 0.00064516,
    "mi²": 1 / 2589988.110336, "mi2": 1 / 2589988.110336, "sq mi": 1 / 2589988.110336, "square mile": 1 / 2589988.110336, "平方英里": 1 / 2589988.110336,
    "亩": 3 / 2000,
};

function getValue(key) {
    return units2[key.toLowerCase()];
}

class AreaConverter {
    constructor(leftSynonym, rightSynonym) {
        this.leftUnit = getValue(leftSynonym);
        this.rightUnit = getValue(rightSynonym);
    }

    static exists(leftSynonym, rightSynonym) {
        if (!leftSynonym || !rightSynonym) { return false; }
        return getValue(leftSynonym) !== undefined && getValue(rightSynonym) !== undefined;
    }

    leftToRight(left) {
        return left / this.leftUnit * this.rightUnit;
    }
}

export { AreaConverter };
