const units2 = {
    "m": 1, "metre": 1, "meter": 1, "米": 1,
    "km": 0.001, "kilometre": 0.001, "kilometer": 0.001, "千米": 0.001,
    "dm": 10, "decimetre": 10, "decimeter": 10, "分米": 10,
    "cm": 100, "centimetre": 100, "centimeter": 100, "厘米": 100,
    "mm": 1000, "millimetre": 1000, "millimeter": 1000, "毫米": 1000,
    "ft": 1250 / 381, "foot": 1250 / 381, "feet": 1250 / 381, "英尺": 1250 / 381,
    "yd": 1250 / 1143, "yard": 1250 / 1143, "码": 1250 / 1143,
    "mile": 125 / 201168, "英里": 125 / 201168,
    "in": 5000 / 127, "inch": 5000 / 127, "英寸": 5000 / 127,
    "au": 1 / 149600000000,
    "尺": 3, "寸": 30,
};

function getValue(key) {
    return units2[key.toLowerCase()];
}

class DistanceConverter {
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

export { DistanceConverter };
