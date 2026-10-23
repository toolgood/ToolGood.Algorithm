import { NumberUnitType } from '../Enums/NumberUnitType.js';
import { DistanceUnitType } from '../Enums/DistanceUnitType.js';
import { AreaUnitType } from '../Enums/AreaUnitType.js';
import { VolumeUnitType } from '../Enums/VolumeUnitType.js';
import { MassUnitType } from '../Enums/MassUnitType.js';

class NumberUnitTypeHelper {
    static unitTypedict = null;

    static getUnitTypedict() {
        if (!this.unitTypedict) {
            this.unitTypedict = {
                "KM": NumberUnitType.KM,
                "M": NumberUnitType.M,
                "DM": NumberUnitType.DM,
                "CM": NumberUnitType.CM,
                "MM": NumberUnitType.MM,
                
                "KM2": NumberUnitType.KM2,
                "M2": NumberUnitType.M2,
                "DM2": NumberUnitType.DM2,
                "CM2": NumberUnitType.CM2,
                "MM2": NumberUnitType.MM2,
                
                "M3": NumberUnitType.M3,
                "DM3": NumberUnitType.DM3,
                "CM3": NumberUnitType.CM3,
                "MM3": NumberUnitType.MM3,
                "KM3": NumberUnitType.KM3,
                
                "L": NumberUnitType.DM3,
                "ML": NumberUnitType.CM3,
                
                "T": NumberUnitType.T,
                "KG": NumberUnitType.KG,
                "G": NumberUnitType.G
            };
        }
        return this.unitTypedict;
    }

    static transformationUnit(src, type1, distance, area, volume, mass) {
        if (type1 === NumberUnitType.MM) {
            switch (distance) {
                case DistanceUnitType.MM:
                    return src;
                case DistanceUnitType.CM:
                    return src * 0.1;
                case DistanceUnitType.DM:
                    return src * 0.01;
                case DistanceUnitType.M:
                    return src * 0.001;
                case DistanceUnitType.KM:
                    return src * 0.000001;
                default:
                    throw new Error("Number unit is error!");
            }
        } else if (type1 === NumberUnitType.CM) {
            switch (distance) {
                case DistanceUnitType.MM:
                    return src * 10;
                case DistanceUnitType.CM:
                    return src;
                case DistanceUnitType.DM:
                    return src * 0.1;
                case DistanceUnitType.M:
                    return src * 0.01;
                case DistanceUnitType.KM:
                    return src * 0.00001;
                default:
                    throw new Error("Number unit is error!");
            }
        } else if (type1 === NumberUnitType.DM) {
            switch (distance) {
                case DistanceUnitType.MM:
                    return src * 100;
                case DistanceUnitType.CM:
                    return src * 10;
                case DistanceUnitType.DM:
                    return src;
                case DistanceUnitType.M:
                    return src * 0.1;
                case DistanceUnitType.KM:
                    return src * 0.0001;
                default:
                    throw new Error("Number unit is error!");
            }
        } else if (type1 === NumberUnitType.M) {
            switch (distance) {
                case DistanceUnitType.MM:
                    return src * 1000;
                case DistanceUnitType.CM:
                    return src * 100;
                case DistanceUnitType.DM:
                    return src * 10;
                case DistanceUnitType.M:
                    return src;
                case DistanceUnitType.KM:
                    return src * 0.001;
                default:
                    throw new Error("Number unit is error!");
            }
        } else if (type1 === NumberUnitType.KM) {
            switch (distance) {
                case DistanceUnitType.MM:
                    return src * 1000000;
                case DistanceUnitType.CM:
                    return src * 100000;
                case DistanceUnitType.DM:
                    return src * 10000;
                case DistanceUnitType.M:
                    return src * 1000;
                case DistanceUnitType.KM:
                    return src;
                default:
                    throw new Error("Number unit is error!");
            }
        } else if (type1 === NumberUnitType.MM2) {
            switch (area) {
                case AreaUnitType.MM2:
                    return src;
                case AreaUnitType.CM2:
                    return src * 0.01;
                case AreaUnitType.DM2:
                    return src * 0.0001;
                case AreaUnitType.M2:
                    return src * 0.000001;
                case AreaUnitType.KM2:
                    return src * 0.000000000001;
                default:
                    throw new Error("Number unit is error!");
            }
        } else if (type1 === NumberUnitType.CM2) {
            switch (area) {
                case AreaUnitType.MM2:
                    return src * 100;
                case AreaUnitType.CM2:
                    return src;
                case AreaUnitType.DM2:
                    return src * 0.01;
                case AreaUnitType.M2:
                    return src * 0.0001;
                case AreaUnitType.KM2:
                    return src * 0.0000000001;
                default:
                    throw new Error("Number unit is error!");
            }
        } else if (type1 === NumberUnitType.DM2) {
            switch (area) {
                case AreaUnitType.MM2:
                    return src * 10000;
                case AreaUnitType.CM2:
                    return src * 100;
                case AreaUnitType.DM2:
                    return src;
                case AreaUnitType.M2:
                    return src * 0.01;
                case AreaUnitType.KM2:
                    return src * 0.00000001;
                default:
                    throw new Error("Number unit is error!");
            }
        } else if (type1 === NumberUnitType.M2) {
            switch (area) {
                case AreaUnitType.MM2:
                    return src * 1000000;
                case AreaUnitType.CM2:
                    return src * 10000;
                case AreaUnitType.DM2:
                    return src * 100;
                case AreaUnitType.M2:
                    return src;
                case AreaUnitType.KM2:
                    return src * 0.000001;
                default:
                    throw new Error("Number unit is error!");
            }
        } else if (type1 === NumberUnitType.KM2) {
            switch (area) {
                case AreaUnitType.MM2:
                    return src * 1000000000000;
                case AreaUnitType.CM2:
                    return src * 10000000000;
                case AreaUnitType.DM2:
                    return src * 100000000;
                case AreaUnitType.M2:
                    return src * 1000000;
                case AreaUnitType.KM2:
                    return src;
                default:
                    throw new Error("Number unit is error!");
            }
        } else if (type1 === NumberUnitType.MM3) {
            switch (volume) {
                case VolumeUnitType.MM3:
                    return src;
                case VolumeUnitType.CM3:
                    return src * 0.001;
                case VolumeUnitType.DM3:
                    return src * 0.000001;
                case VolumeUnitType.M3:
                    return src * 0.000000001;
                case VolumeUnitType.KM3:
                    return src * 0.000000000000000001;
                default:
                    throw new Error("Number unit is error!");
            }
        } else if (type1 === NumberUnitType.CM3) {
            switch (volume) {
                case VolumeUnitType.MM3:
                    return src * 1000;
                case VolumeUnitType.CM3:
                    return src;
                case VolumeUnitType.DM3:
                    return src * 0.001;
                case VolumeUnitType.M3:
                    return src * 0.000001;
                case VolumeUnitType.KM3:
                    return src * 0.000000000000001;
                default:
                    throw new Error("Number unit is error!");
            }
        } else if (type1 === NumberUnitType.DM3) {
            switch (volume) {
                case VolumeUnitType.MM3:
                    return src * 1000000;
                case VolumeUnitType.CM3:
                    return src * 1000;
                case VolumeUnitType.DM3:
                    return src;
                case VolumeUnitType.M3:
                    return src * 0.001;
                case VolumeUnitType.KM3:
                    return src * 0.000000000001;
                default:
                    throw new Error("Number unit is error!");
            }
        } else if (type1 === NumberUnitType.M3) {
            switch (volume) {
                case VolumeUnitType.MM3:
                    return src * 1000000000;
                case VolumeUnitType.CM3:
                    return src * 1000000;
                case VolumeUnitType.DM3:
                    return src * 1000;
                case VolumeUnitType.M3:
                    return src;
                case VolumeUnitType.KM3:
                    return src * 0.000000001;
                default:
                    throw new Error("Number unit is error!");
            }
        } else if (type1 === NumberUnitType.KM3) {
            switch (volume) {
                case VolumeUnitType.MM3:
                    return src * 1000000000000000000;
                case VolumeUnitType.CM3:
                    return src * 1000000000000000;
                case VolumeUnitType.DM3:
                    return src * 1000000000000;
                case VolumeUnitType.M3:
                    return src * 1000000000;
                case VolumeUnitType.KM3:
                    return src;
                default:
                    throw new Error("Number unit is error!");
            }
        } else if (type1 === NumberUnitType.G) {
            switch (mass) {
                case MassUnitType.G:
                    return src;
                case MassUnitType.KG:
                    return src * 0.001;
                case MassUnitType.T:
                    return src * 0.000001;
                default:
                    throw new Error("Number unit is error!");
            }
        } else if (type1 === NumberUnitType.KG) {
            switch (mass) {
                case MassUnitType.G:
                    return src * 1000;
                case MassUnitType.KG:
                    return src;
                case MassUnitType.T:
                    return src * 0.001;
                default:
                    throw new Error("Number unit is error!");
            }
        } else if (type1 === NumberUnitType.T) {
            switch (mass) {
                case MassUnitType.G:
                    return src * 1000000;
                case MassUnitType.KG:
                    return src * 1000;
                case MassUnitType.T:
                    return src;
                default:
                    throw new Error("Number unit is error!");
            }
        }
        throw new Error("Number unit is error!");
    }
}

export { NumberUnitTypeHelper };
