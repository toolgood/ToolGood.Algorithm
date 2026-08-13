const units2 = {
    "l": 1, "lt": 1, "ltr": 1, "liter": 1, "litre": 1,
    "dm³": 1, "dm3": 1, "cubic decimetre": 1, "cubic decimeter": 1, "升": 1, "立方分米": 1,
    "m³": 0.001, "m3": 0.001, "cubic metre": 0.001, "cubic meter": 0.001, "立方米": 0.001,
    "km³": 1e-12, "km3": 1e-12, "cubic kilometre": 1e-12, "cubic kilometer": 1e-12, "立方千米": 1e-12,
    "cm³": 1000, "cm3": 1000, "cubic centimetre": 1000, "cubic centimeter": 1000, "立方厘米": 1000, "毫升": 1000, "ml": 1000,
    "mm³": 1000000, "mm3": 1000000, "cubic millimetre": 1000000, "cubic millimeter": 1000000, "立方毫米": 1000000,
    "ft³": 1 / 28.316846592, "ft3": 1 / 28.316846592, "cubic foot": 1 / 28.316846592, "cubic feet": 1 / 28.316846592, "立方英尺": 1 / 28.316846592, "cu ft": 1 / 28.316846592,
    "in³": 125000000 / 2048383, "in3": 125000000 / 2048383, "cubic in": 125000000 / 2048383, "cubic inch": 125000000 / 2048383, "立方英寸": 125000000 / 2048383,
    "imperial pint": 800000 / 454609, "imperial pt": 800000 / 454609, "imperial p": 800000 / 454609,
    "imperial gallon": 100000 / 454609, "imperial gal": 100000 / 454609,
    "imperial quart": 400000 / 454609, "imperial qt": 400000 / 454609,
    "us pint": 1000000000 / 473176473, "us pt": 1000000000 / 473176473, "us p": 1000000000 / 473176473,
    "us gallon": 125000000 / 473176473, "us gal": 125000000 / 473176473,
    "us quart": 1000000000 / 946352946, "us qt": 1000000000 / 946352946,
};

function getValue(key) {
    return units2[key.toLowerCase()];
}

class VolumeConverter {
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

export { VolumeConverter };
