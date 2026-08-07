import { BaseUnitConverter } from './BaseUnitConverter.js';
import { UnitFactors } from './UnitFactors.js';
import { UnitFactorSynonyms } from './UnitFactorSynonyms.js';

class VolumeConverter extends BaseUnitConverter {
    constructor(leftUnit, rightUnit) {
        super();
        this.instantiate(VolumeConverter.units, leftUnit, rightUnit);
    }

    static exists(leftSynonym, rightSynonym) {
        if (VolumeConverter.units.findUnit(leftSynonym) != null) {
            return VolumeConverter.units.findUnit(rightSynonym) != null;
        }
        return false;
    }
}

// 静态单位因子初始化
VolumeConverter.units = new UnitFactors();
VolumeConverter.units.set(new UnitFactorSynonyms("l", "L", "lt", "ltr", "liter", "litre", "dm³", "dm3", "cubic decimetre", "cubic decimeter", "升", "立方分米"), 1);
VolumeConverter.units.set(new UnitFactorSynonyms("m³", "m3", "cubic metre", "cubic meter", "立方米"), 0.001);
VolumeConverter.units.set(new UnitFactorSynonyms("km³", "km3", "cubic kilometre", "cubic kilometer", "立方千米"), 0.001 * 0.001 * 0.001 * 0.001);
VolumeConverter.units.set(new UnitFactorSynonyms("cm³", "cm3", "cubic centimetre", "cubic centimeter", "立方厘米", "毫升", "ml"), 1000);
VolumeConverter.units.set(new UnitFactorSynonyms("mm³", "mm3", "cubic millimetre", "cubic millimeter", "立方毫米"), 1000000);
VolumeConverter.units.set(new UnitFactorSynonyms("ft³", "ft3", "cubic foot", "cubic feet", "cu ft", "立方英尺"), 1 / 28.316846592);
VolumeConverter.units.set(new UnitFactorSynonyms("in³", "in3", "cu in", "cubic inch", "立方英寸"), 125000000 / 2048383);
VolumeConverter.units.set(new UnitFactorSynonyms("imperial pint", "imperial pt", "imperial p"), 800000 / 454609);
VolumeConverter.units.set(new UnitFactorSynonyms("imperial gallon", "imperial gal"), 100000 / 454609);
VolumeConverter.units.set(new UnitFactorSynonyms("imperial quart", "imperial qt"), 400000 / 454609);
VolumeConverter.units.set(new UnitFactorSynonyms("US pint", "US pt", "US p"), 1000000000 / 473176473);
VolumeConverter.units.set(new UnitFactorSynonyms("US gallon", "US gal"), 125000000 / 473176473);
VolumeConverter.units.set(new UnitFactorSynonyms("US quart", "US qt"), 1000000000 / 946352946);

export { VolumeConverter };