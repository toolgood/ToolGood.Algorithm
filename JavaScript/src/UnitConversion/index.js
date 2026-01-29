import { UnitNotSupportedException } from './UnitNotSupportedException.js';
import { UnitFactors } from './UnitFactors.js';
import { UnitFactorSynonyms } from './UnitFactorSynonyms.js';
import { BaseUnitConverter } from './BaseUnitConverter.js';
import { AreaConverter } from './AreaConverter.js';
import { DistanceConverter } from './DistanceConverter.js';
import { MassConverter } from './MassConverter.js';
import { VolumeConverter } from './VolumeConverter.js';

export {
    UnitNotSupportedException,
    UnitFactors,
    UnitFactorSynonyms,
    BaseUnitConverter,
    AreaConverter,
    DistanceConverter,
    MassConverter,
    VolumeConverter
};

// 为了在浏览器中使用，添加全局变量
if (typeof window !== 'undefined') {
    window.UnitConversion = {
        UnitNotSupportedException,
        UnitFactors,
        UnitFactorSynonyms,
        BaseUnitConverter,
        AreaConverter,
        DistanceConverter,
        MassConverter,
        VolumeConverter
    };
}