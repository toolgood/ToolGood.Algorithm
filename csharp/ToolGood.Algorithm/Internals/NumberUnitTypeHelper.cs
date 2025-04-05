using System;
using System.Collections.Generic;
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
                unitTypedict = new Dictionary<string, NumberUnitType>(StringComparer.OrdinalIgnoreCase) {
                    ["KM"] = NumberUnitType.KM,
                    ["M"] = NumberUnitType.M,
                    ["DM"] = NumberUnitType.DM,
                    ["CM"] = NumberUnitType.CM,
                    ["MM"] = NumberUnitType.MM,

                    ["KM2"] = NumberUnitType.KM2,
                    ["M2"] = NumberUnitType.M2,
                    ["DM2"] = NumberUnitType.DM2,
                    ["CM2"] = NumberUnitType.CM2,
                    ["MM2"] = NumberUnitType.MM2,

                    ["M3"] = NumberUnitType.M3,
                    ["DM3"] = NumberUnitType.DM3,
                    ["CM3"] = NumberUnitType.CM3,
                    ["MM3"] = NumberUnitType.MM3,
                    ["KM3"] = NumberUnitType.KM3,

                    ["L"] = NumberUnitType.DM3,
                    ["ML"] = NumberUnitType.CM3,

                    ["T"] = NumberUnitType.T,
                    ["KG"] = NumberUnitType.KG,
                    ["G"] = NumberUnitType.G
                };
            }
            return unitTypedict;
        }

        internal static decimal TransformationUnit(decimal src, NumberUnitType type1, DistanceUnitType distance, AreaUnitType area, VolumeUnitType volume, MassUnitType mass)
        {
            if (type1 == NumberUnitType.MM) {
                return distance switch {
                    DistanceUnitType.MM => src,
                    DistanceUnitType.CM => src * 0.1m,
                    DistanceUnitType.DM => src * 0.01m,
                    DistanceUnitType.M => src * 0.001m,
                    DistanceUnitType.KM => src * 0.000001m,
                    _ => throw new Exception("Number unit is error!"),
                };
            } else if (type1 == NumberUnitType.CM) {
                return distance switch {
                    DistanceUnitType.MM => src * 10,
                    DistanceUnitType.CM => src,
                    DistanceUnitType.DM => src * 0.1m,
                    DistanceUnitType.M => src * 0.01m,
                    DistanceUnitType.KM => src * 0.00001m,
                    _ => throw new Exception("Number unit is error!"),
                };
            } else if (type1 == NumberUnitType.DM) {
                return distance switch {
                    DistanceUnitType.MM => src * 100,
                    DistanceUnitType.CM => src * 10,
                    DistanceUnitType.DM => src,
                    DistanceUnitType.M => src * 0.1m,
                    DistanceUnitType.KM => src * 0.0001m,
                    _ => throw new Exception("Number unit is error!"),
                };
            } else if (type1 == NumberUnitType.M) {
                return distance switch {
                    DistanceUnitType.MM => src * 1000,
                    DistanceUnitType.CM => src * 100,
                    DistanceUnitType.DM => src * 10,
                    DistanceUnitType.M => src,
                    DistanceUnitType.KM => src * 0.001m,
                    _ => throw new Exception("Number unit is error!"),
                };
            } else if (type1 == NumberUnitType.KM) {
                return distance switch {
                    DistanceUnitType.MM => src * 1000000,
                    DistanceUnitType.CM => src * 100000,
                    DistanceUnitType.DM => src * 10000,
                    DistanceUnitType.M => src * 1000,
                    DistanceUnitType.KM => src,
                    _ => throw new Exception("Number unit is error!"),
                };
            } else if (type1 == NumberUnitType.MM2) {
                return area switch {
                    AreaUnitType.MM2 => src,
                    AreaUnitType.CM2 => src * 0.01m,
                    AreaUnitType.DM2 => src * 0.0001m,
                    AreaUnitType.M2 => src * 0.000001m,
                    AreaUnitType.KM2 => src * 0.000000000001m,
                    _ => throw new Exception("Number unit is error!"),
                };
            } else if (type1 == NumberUnitType.CM2) {
                return area switch {
                    AreaUnitType.MM2 => src * 100,
                    AreaUnitType.CM2 => src,
                    AreaUnitType.DM2 => src * 0.01m,
                    AreaUnitType.M2 => src * 0.0001m,
                    AreaUnitType.KM2 => src * 0.0000000001m,
                    _ => throw new Exception("Number unit is error!"),
                };
            } else if (type1 == NumberUnitType.DM2) {
                return area switch {
                    AreaUnitType.MM2 => src * 10000,
                    AreaUnitType.CM2 => src * 100,
                    AreaUnitType.DM2 => src,
                    AreaUnitType.M2 => src * 0.01m,
                    AreaUnitType.KM2 => src * 0.00000001m,
                    _ => throw new Exception("Number unit is error!"),
                };
            } else if (type1 == NumberUnitType.M2) {
                return area switch {
                    AreaUnitType.MM2 => src * 1000000,
                    AreaUnitType.CM2 => src * 10000,
                    AreaUnitType.DM2 => src * 100,
                    AreaUnitType.M2 => src,
                    AreaUnitType.KM2 => src * 0.000001m,
                    _ => throw new Exception("Number unit is error!"),
                };
            } else if (type1 == NumberUnitType.KM2) {
                return area switch {
                    AreaUnitType.MM2 => src * 1000000000000,
                    AreaUnitType.CM2 => src * 10000000000,
                    AreaUnitType.DM2 => src * 100000000,
                    AreaUnitType.M2 => src * 1000000,
                    AreaUnitType.KM2 => src,
                    _ => throw new Exception("Number unit is error!"),
                };
            } else if (type1 == NumberUnitType.MM3) {
                return volume switch {
                    VolumeUnitType.MM3 => src,
                    VolumeUnitType.CM3 => src * 0.001m,
                    VolumeUnitType.DM3 => src * 0.000001m,
                    VolumeUnitType.M3 => src * 0.000000001m,
                    VolumeUnitType.KM3 => src * 0.000000000000000001m,
                    _ => throw new Exception("Number unit is error!"),
                };
            } else if (type1 == NumberUnitType.CM3) {
                return volume switch {
                    VolumeUnitType.MM3 => src * 1000,
                    VolumeUnitType.CM3 => src,
                    VolumeUnitType.DM3 => src * 0.001m,
                    VolumeUnitType.M3 => src * 0.000001m,
                    VolumeUnitType.KM3 => src * 0.000000000000001m,
                    _ => throw new Exception("Number unit is error!"),
                };
            } else if (type1 == NumberUnitType.DM3) {
                return volume switch {
                    VolumeUnitType.MM3 => src * 1000000,
                    VolumeUnitType.CM3 => src * 1000,
                    VolumeUnitType.DM3 => src,
                    VolumeUnitType.M3 => src * 0.001m,
                    VolumeUnitType.KM3 => src * 0.000000000001m,
                    _ => throw new Exception("Number unit is error!"),
                };
            } else if (type1 == NumberUnitType.M3) {
                return volume switch {
                    VolumeUnitType.MM3 => src * 1000000000,
                    VolumeUnitType.CM3 => src * 1000000,
                    VolumeUnitType.DM3 => src * 1000,
                    VolumeUnitType.M3 => src,
                    VolumeUnitType.KM3 => src * 0.000000001m,
                    _ => throw new Exception("Number unit is error!"),
                };
            } else if (type1 == NumberUnitType.KM3) {
                return volume switch {
                    VolumeUnitType.MM3 => src * 1000000000000000000,
                    VolumeUnitType.CM3 => src * 1000000000000000,
                    VolumeUnitType.DM3 => src * 1000000000000,
                    VolumeUnitType.M3 => src * 1000000000,
                    VolumeUnitType.KM3 => src,
                    _ => throw new Exception("Number unit is error!"),
                };
            } else if (type1 == NumberUnitType.G) {
                return mass switch {
                    MassUnitType.G => src,
                    MassUnitType.KG => src * 0.001m,
                    MassUnitType.T => src * 0.000001m,
                    _ => throw new Exception("Number unit is error!"),
                };
            } else if (type1 == NumberUnitType.KG) {
                return mass switch {
                    MassUnitType.G => src * 1000,
                    MassUnitType.KG => src,
                    MassUnitType.T => src * 0.001m,
                    _ => throw new Exception("Number unit is error!"),
                };
            } else if (type1 == NumberUnitType.T) {
                return mass switch {
                    MassUnitType.G => src * 1000000,
                    MassUnitType.KG => src * 1000,
                    MassUnitType.T => src,
                    _ => throw new Exception("Number unit is error!"),
                };
            }
            throw new Exception("Number unit is error!");
        }
    }
}