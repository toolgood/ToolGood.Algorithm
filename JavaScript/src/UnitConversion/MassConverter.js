import { BaseUnitConverter } from './BaseUnitConverter.js';
import { UnitFactors } from './UnitFactors.js';
import { UnitFactorSynonyms } from './UnitFactorSynonyms.js';

class MassConverter extends BaseUnitConverter {
    constructor(leftUnit, rightUnit) {
        super();
        this.instantiate(MassConverter.units, leftUnit, rightUnit);
    }

    static exists(leftSynonym, rightSynonym) {
        if (MassConverter.units.findUnit(leftSynonym) != null) {
            return MassConverter.units.findUnit(rightSynonym) != null;
        }
        return false;
    }
}

// 静态单位因子初始化
MassConverter.units = new UnitFactors();
MassConverter.units.set(new UnitFactorSynonyms("kg", "kilogram", "千克"), 1);
MassConverter.units.set(new UnitFactorSynonyms("gram", "g", "克"), 1000);
MassConverter.units.set(new UnitFactorSynonyms("ton", "t", "吨"), 1 / 1000);
MassConverter.units.set(new UnitFactorSynonyms("lb", "lbs", "pound", "pounds", "英镑"), 100000000 / 45359237);
MassConverter.units.set(new UnitFactorSynonyms("st", "stone", "石"), 50000000 / 317514659);
MassConverter.units.set(new UnitFactorSynonyms("oz", "ounce", "盎司"), 1600000000 / 45359237);
MassConverter.units.set(new UnitFactorSynonyms("quintal", "英担"), 0.01);
MassConverter.units.set(new UnitFactorSynonyms("short ton", "net ton", "us ton", "短吨", "美吨"), 0.00110231);
MassConverter.units.set(new UnitFactorSynonyms("long ton", "weight ton", "gross ton", "imperial ton", "长吨", "英吨"), 0.000984207);

export { MassConverter };