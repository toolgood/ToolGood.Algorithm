import { BaseUnitConverter } from './BaseUnitConverter.js';
import { UnitFactors } from './UnitFactors.js';
import { UnitFactorSynonyms } from './UnitFactorSynonyms.js';

class AreaConverter extends BaseUnitConverter {
    constructor(leftUnit, rightUnit) {
        super();
        this.Instantiate(AreaConverter.units, leftUnit, rightUnit);
    }

    static Exists(leftSynonym, rightSynonym) {
        if (AreaConverter.units.FindUnit(leftSynonym) != null) {
            return AreaConverter.units.FindUnit(rightSynonym) != null;
        }
        return false;
    }
}

// 静态单位因子初始化
AreaConverter.units = new UnitFactors();
AreaConverter.units.set(new UnitFactorSynonyms("m²", "m2", "square metre", "square meter", "centiare", "平方米", "平方公尺"), 1);
AreaConverter.units.set(new UnitFactorSynonyms("km²", "km2", "square kilometre", "square kilometer", "平方千米"), 0.000001);
AreaConverter.units.set(new UnitFactorSynonyms("dm²", "dm2", "square decimetre", "square decimeter", "平方分米"), 100);
AreaConverter.units.set(new UnitFactorSynonyms("cm²", "cm2", "square centimetre", "square centimeter", "平方厘米"), 10000);
AreaConverter.units.set(new UnitFactorSynonyms("mm²", "mm2", "square millimetre", "square millimeter", "平方毫米"), 1000000);
AreaConverter.units.set(new UnitFactorSynonyms("ft²", "ft2", "square foot", "square feet", "sq ft", "平方英尺"), 1 / 0.3048 / 0.3048);
AreaConverter.units.set(new UnitFactorSynonyms("yd²", "yd2", "sq yd", "square yard", "平方码"), 1 / 0.9144 / 0.9144);
AreaConverter.units.set(new UnitFactorSynonyms("a", "are"), 0.01);
AreaConverter.units.set(new UnitFactorSynonyms("ha", "hectare", "公顷"), 0.0001);
AreaConverter.units.set(new UnitFactorSynonyms("in²", "in2", "sq in", "square inch", "平方英寸"), 1 / 0.00064516);
AreaConverter.units.set(new UnitFactorSynonyms("mi²", "mi2", "sq mi", "square mile", "平方英里"), 1 / 2589988.110336);
AreaConverter.units.set(new UnitFactorSynonyms("亩"), 1 / 666.667);

export { AreaConverter };