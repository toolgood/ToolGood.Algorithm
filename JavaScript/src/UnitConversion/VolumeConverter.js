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
VolumeConverter.units.set(new UnitFactorSynonyms("cm³", "cm3", "cubic centimetre", "cubic centimeter", "立方厘米", "毫升"), 1000);
VolumeConverter.units.set(new UnitFactorSynonyms("mm³", "mm3", "cubic millimetre", "cubic millimeter", "立方毫米"), 1000000);
VolumeConverter.units.set(new UnitFactorSynonyms("ft³", "ft3", "cubic foot", "cubic feet", "cu ft", "立方英尺"), 0.0353147);
VolumeConverter.units.set(new UnitFactorSynonyms("in³", "in3", "cu in", "cubic inch", "立方英寸"), 61.0237);
VolumeConverter.units.set(new UnitFactorSynonyms("imperial pint", "imperial pt", "imperial p"), 1.75975);
VolumeConverter.units.set(new UnitFactorSynonyms("imperial gallon", "imperial gal"), 0.219969);
VolumeConverter.units.set(new UnitFactorSynonyms("imperial quart", "imperial qt"), 0.879877);
VolumeConverter.units.set(new UnitFactorSynonyms("US pint", "US pt", "US p"), 2.11337643513819);
VolumeConverter.units.set(new UnitFactorSynonyms("US gallon", "US gal"), 0.264172);
VolumeConverter.units.set(new UnitFactorSynonyms("US quart", "US qt"), 2.11338);

export { VolumeConverter };