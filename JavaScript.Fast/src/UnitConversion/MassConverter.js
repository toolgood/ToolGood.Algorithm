const units2 = {
    "kg": 1, "kilogram": 1, "千克": 1,
    "gram": 1000, "g": 1000, "克": 1000,
    "ton": 1 / 1000, "t": 1 / 1000, "吨": 1 / 1000,
    "lb": 100000000 / 45359237, "lbs": 100000000 / 45359237, "pound": 100000000 / 45359237, "pounds": 100000000 / 45359237, "磅": 100000000 / 45359237, "英磅": 100000000 / 45359237,
    "st": 50000000 / 317514659, "stone": 50000000 / 317514659, "石": 50000000 / 317514659,
    "oz": 1600000000 / 45359237, "ounce": 1600000000 / 45359237, "盎司": 1600000000 / 45359237,
    "quintal": 0.01, "英担": 0.01,
    "short ton": 50000 / 45359237, "net ton": 50000 / 45359237, "us ton": 50000 / 45359237, "短吨": 50000 / 45359237, "美吨": 50000 / 45359237,
    "long ton": 312500 / 317514659, "weight ton": 312500 / 317514659, "gross ton": 312500 / 317514659, "imperial ton": 312500 / 317514659, "长吨": 312500 / 317514659, "英吨": 312500 / 317514659,
    "mg": 1000000, "毫克": 1000000,
    "斤": 2, "两": 20,
};

function getValue(key) {
    return units2[key.toLowerCase()];
}

class MassConverter {
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

export { MassConverter };
