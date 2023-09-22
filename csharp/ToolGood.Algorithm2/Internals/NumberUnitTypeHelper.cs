using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals
{
    /// <summary>
    /// 单位
    /// </summary>
    internal static class NumberUnitTypeHelper
    {
        private static Dictionary<string, NumberUnitType> unitTypedict;

        /// <summary>
        /// 获取 单位字典
        /// </summary>
        /// <returns></returns>
        internal static Dictionary<string, NumberUnitType> GetUnitTypedict()
        {
            if (unitTypedict == null) {
                unitTypedict = new Dictionary<string, NumberUnitType>(StringComparer.OrdinalIgnoreCase);
                unitTypedict["KM"] = NumberUnitType.KM;
                unitTypedict["M"] = NumberUnitType.M;
                unitTypedict["DM"] = NumberUnitType.DM;
                unitTypedict["CM"] = NumberUnitType.CM;
                unitTypedict["MM"] = NumberUnitType.MM;

                unitTypedict["KM2"] = NumberUnitType.KM2;
                unitTypedict["M2"] = NumberUnitType.M2;
                unitTypedict["DM2"] = NumberUnitType.DM2;
                unitTypedict["CM2"] = NumberUnitType.CM2;
                unitTypedict["MM2"] = NumberUnitType.MM2;

                unitTypedict["M3"] = NumberUnitType.M3;
                unitTypedict["DM3"] = NumberUnitType.DM3;
                unitTypedict["CM3"] = NumberUnitType.CM3;
                unitTypedict["MM3"] = NumberUnitType.MM3;
                unitTypedict["KM3"] = NumberUnitType.KM3;

                unitTypedict["L"] = NumberUnitType.DM3;
                unitTypedict["ML"] = NumberUnitType.CM3;

                unitTypedict["T"] = NumberUnitType.T;
                unitTypedict["KG"] = NumberUnitType.KG;
                unitTypedict["G"] = NumberUnitType.G;
            }
            return unitTypedict;
        }

        internal static decimal TransformationUnit(decimal src, NumberUnitType type1, DistanceUnitType distance, AreaUnitType area, VolumeUnitType volume, MassUnitType mass)
        {
            if (type1 == NumberUnitType.MM) {
                switch (distance) {
                    case DistanceUnitType.MM: return src;
                    case DistanceUnitType.CM: return src * 0.1m;
                    case DistanceUnitType.DM: return src * 0.01m;
                    case DistanceUnitType.M: return src * 0.001m;
                    case DistanceUnitType.KM: return src * 0.000001m;
                    default: throw new Exception("Number unit is error!");
                }
            } else if (type1 == NumberUnitType.CM) {
                switch (distance) {
                    case DistanceUnitType.MM: return src * 10;
                    case DistanceUnitType.CM: return src;
                    case DistanceUnitType.DM: return src * 0.1m;
                    case DistanceUnitType.M: return src * 0.01m;
                    case DistanceUnitType.KM: return src * 0.00001m;
                    default: throw new Exception("Number unit is error!");
                }
            } else if (type1 == NumberUnitType.DM) {
                switch (distance) {
                    case DistanceUnitType.MM: return src * 100;
                    case DistanceUnitType.CM: return src * 10;
                    case DistanceUnitType.DM: return src;
                    case DistanceUnitType.M: return src * 0.1m;
                    case DistanceUnitType.KM: return src * 0.0001m;
                    default: throw new Exception("Number unit is error!");
                }
            } else if (type1 == NumberUnitType.M) {
                switch (distance) {
                    case DistanceUnitType.MM: return src * 1000;
                    case DistanceUnitType.CM: return src * 100;
                    case DistanceUnitType.DM: return src * 10;
                    case DistanceUnitType.M: return src;
                    case DistanceUnitType.KM: return src * 0.001m;
                    default: throw new Exception("Number unit is error!");
                }
            } else if (type1 == NumberUnitType.KM) {
                switch (distance) {
                    case DistanceUnitType.MM: return src * 1000000;
                    case DistanceUnitType.CM: return src * 100000;
                    case DistanceUnitType.DM: return src * 10000;
                    case DistanceUnitType.M: return src * 1000;
                    case DistanceUnitType.KM: return src;
                    default: throw new Exception("Number unit is error!");
                }
            } else if (type1 == NumberUnitType.MM2) {
                switch (area) {
                    case AreaUnitType.MM2: return src;
                    case AreaUnitType.CM2: return src * 0.01m;
                    case AreaUnitType.DM2: return src * 0.0001m;
                    case AreaUnitType.M2: return src * 0.000001m;
                    case AreaUnitType.KM2: return src * 0.000000000001m;
                    default: throw new Exception("Number unit is error!");
                }
            } else if (type1 == NumberUnitType.CM2) {
                switch (area) {
                    case AreaUnitType.MM2: return src * 100;
                    case AreaUnitType.CM2: return src;
                    case AreaUnitType.DM2: return src * 0.01m;
                    case AreaUnitType.M2: return src * 0.0001m;
                    case AreaUnitType.KM2: return src * 0.0000000001m;
                    default: throw new Exception("Number unit is error!");
                }
            } else if (type1 == NumberUnitType.DM2) {
                switch (area) {
                    case AreaUnitType.MM2: return src * 10000;
                    case AreaUnitType.CM2: return src * 100;
                    case AreaUnitType.DM2: return src;
                    case AreaUnitType.M2: return src * 0.01m;
                    case AreaUnitType.KM2: return src * 0.00000001m;
                    default: throw new Exception("Number unit is error!");
                }
            } else if (type1 == NumberUnitType.M2) {
                switch (area) {
                    case AreaUnitType.MM2: return src * 1000000;
                    case AreaUnitType.CM2: return src * 10000;
                    case AreaUnitType.DM2: return src * 100;
                    case AreaUnitType.M2: return src;
                    case AreaUnitType.KM2: return src * 0.000001m;
                    default: throw new Exception("Number unit is error!");
                }
            } else if (type1 == NumberUnitType.KM2) {
                switch (area) {
                    case AreaUnitType.MM2: return src * 1000000000000;
                    case AreaUnitType.CM2: return src * 10000000000;
                    case AreaUnitType.DM2: return src * 100000000;
                    case AreaUnitType.M2: return src * 1000000;
                    case AreaUnitType.KM2: return src;
                    default: throw new Exception("Number unit is error!");
                }
            } else if (type1 == NumberUnitType.MM3) {
                switch (volume) {
                    case VolumeUnitType.MM3: return src;
                    case VolumeUnitType.CM3: return src * 0.001m;
                    case VolumeUnitType.DM3: return src * 0.000001m;
                    case VolumeUnitType.M3: return src * 0.000000001m;
                    case VolumeUnitType.KM3: return src * 0.000000000000000001m;
                    default: throw new Exception("Number unit is error!");
                }
            } else if (type1 == NumberUnitType.CM3) {
                switch (volume) {
                    case VolumeUnitType.MM3: return src * 1000;
                    case VolumeUnitType.CM3: return src;
                    case VolumeUnitType.DM3: return src * 0.001m;
                    case VolumeUnitType.M3: return src * 0.000001m;
                    case VolumeUnitType.KM3: return src * 0.000000000000001m;
                    default: throw new Exception("Number unit is error!");
                }
            } else if (type1 == NumberUnitType.DM3) {
                switch (volume) {
                    case VolumeUnitType.MM3: return src * 1000000;
                    case VolumeUnitType.CM3: return src * 1000;
                    case VolumeUnitType.DM3: return src;
                    case VolumeUnitType.M3: return src * 0.001m;
                    case VolumeUnitType.KM3: return src * 0.000000000001m;
                    default: throw new Exception("Number unit is error!");
                }
            } else if (type1 == NumberUnitType.M3) {
                switch (volume) {
                    case VolumeUnitType.MM3: return src * 1000000000;
                    case VolumeUnitType.CM3: return src * 1000000;
                    case VolumeUnitType.DM3: return src * 1000;
                    case VolumeUnitType.M3: return src;
                    case VolumeUnitType.KM3: return src * 0.000000001m;
                    default: throw new Exception("Number unit is error!");
                }
            } else if (type1 == NumberUnitType.KM3) {
                switch (volume) {
                    case VolumeUnitType.MM3: return src * 1000000000000000000;
                    case VolumeUnitType.CM3: return src * 1000000000000000;
                    case VolumeUnitType.DM3: return src * 1000000000000;
                    case VolumeUnitType.M3: return src * 1000000000;
                    case VolumeUnitType.KM3: return src;
                    default: throw new Exception("Number unit is error!");
                }
            } else if (type1 == NumberUnitType.G) {
                switch (mass) {
                    case MassUnitType.G: return src;
                    case MassUnitType.KG: return src * 1000;
                    case MassUnitType.T: return src * 1000000;
                    default: throw new Exception("Number unit is error!");
                }
            } else if (type1 == NumberUnitType.KG) {
                switch (mass) {
                    case MassUnitType.G: return src * 0.001m;
                    case MassUnitType.KG: return src;
                    case MassUnitType.T: return src * 1000;
                    default: throw new Exception("Number unit is error!");
                }
            } else if (type1 == NumberUnitType.T) {
                switch (mass) {
                    case MassUnitType.G: return src * 0.000001m;
                    case MassUnitType.KG: return src * 0.001m;
                    case MassUnitType.T: return src;
                    default: throw new Exception("Number unit is error!");
                }
            }
            throw new Exception("Number unit is error!");
        }
    }
}
