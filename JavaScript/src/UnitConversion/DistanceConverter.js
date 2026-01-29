import { BaseUnitConverter } from './BaseUnitConverter.js';
import { UnitFactors } from './UnitFactors.js';
import { UnitFactorSynonyms } from './UnitFactorSynonyms.js';

class DistanceConverter extends BaseUnitConverter {
    constructor(leftUnit, rightUnit) {
        super();
        this.Instantiate(DistanceConverter.units, leftUnit, rightUnit);
    }

    static Exists(leftSynonym, rightSynonym) {
        if (DistanceConverter.units.FindUnit(leftSynonym) != null) {
            return DistanceConverter.units.FindUnit(rightSynonym) != null;
        }
        return false;
    }
}

// 静态单位因子初始化
DistanceConverter.units = new UnitFactors();
DistanceConverter.units.set(new UnitFactorSynonyms("m", "metre", "米"), 1);
DistanceConverter.units.set(new UnitFactorSynonyms("km", "kilometre", "千米"), 0.001);
DistanceConverter.units.set(new UnitFactorSynonyms("dm", "decimetre", "分米"), 10);
DistanceConverter.units.set(new UnitFactorSynonyms("cm", "centimetre", "厘米"), 100);
DistanceConverter.units.set(new UnitFactorSynonyms("mm", "millimetre", "毫米"), 1000);
DistanceConverter.units.set(new UnitFactorSynonyms("ft", "foot", "feet", "英尺"), 1250 / 381);
DistanceConverter.units.set(new UnitFactorSynonyms("yd", "yard", "码"), 1250 / 1143);
DistanceConverter.units.set(new UnitFactorSynonyms("mile", "英里"), 125 / 201168);
DistanceConverter.units.set(new UnitFactorSynonyms("in", "inch", "英寸"), 5000 / 127);
DistanceConverter.units.set(new UnitFactorSynonyms("au"), 1 / 149600000000);

export { DistanceConverter };