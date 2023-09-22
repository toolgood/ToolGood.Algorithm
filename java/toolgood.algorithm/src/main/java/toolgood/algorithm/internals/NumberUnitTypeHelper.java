package toolgood.algorithm.internals;

import toolgood.algorithm.enums.*;

import java.math.BigDecimal;
import java.util.HashMap;
import java.util.Map;

public class NumberUnitTypeHelper {
    private static Map<String, NumberUnitType> unitTypedict;

    /// <summary>
    /// 获取 单位字典
    /// </summary>
    /// <returns></returns>
    static Map<String, NumberUnitType> GetUnitTypedict() {
        if (unitTypedict == null) {
            unitTypedict = new HashMap<>();
            unitTypedict.put("KM", NumberUnitType.KM);
            unitTypedict.put("M", NumberUnitType.M);
            unitTypedict.put("DM", NumberUnitType.DM);
            unitTypedict.put("CM", NumberUnitType.CM);
            unitTypedict.put("MM", NumberUnitType.MM);

            unitTypedict.put("KM2", NumberUnitType.KM2);
            unitTypedict.put("M2", NumberUnitType.M2);
            unitTypedict.put("DM2", NumberUnitType.DM2);
            unitTypedict.put("CM2", NumberUnitType.CM2);
            unitTypedict.put("MM2", NumberUnitType.MM2);

            unitTypedict.put("M3", NumberUnitType.M3);
            unitTypedict.put("DM3", NumberUnitType.DM3);
            unitTypedict.put("CM3", NumberUnitType.CM3);
            unitTypedict.put("MM3", NumberUnitType.MM3);
            unitTypedict.put("KM3", NumberUnitType.KM3);

            unitTypedict.put("L", NumberUnitType.DM3);
            unitTypedict.put("ML", NumberUnitType.CM3);

            unitTypedict.put("T", NumberUnitType.T);
            unitTypedict.put("KG", NumberUnitType.KG);
            unitTypedict.put("G", NumberUnitType.G);
        }
        return unitTypedict;
    }

    public static BigDecimal TransformationUnit(BigDecimal src, NumberUnitType type1, DistanceUnitType distance, AreaUnitType area, VolumeUnitType volume, MassUnitType mass) throws Exception {
        if (type1.getValue() == NumberUnitType.MM.getValue()) {
            switch (distance) {
                case MM:
                    return src;
                case CM:
                    return src.multiply(new BigDecimal(0.1));
                case DM:
                    return src.multiply(new BigDecimal(0.01));
                case M:
                    return src.multiply(new BigDecimal(0.001));
                case KM:
                    return src.multiply(new BigDecimal(0.000001));
                default:
                    throw new Exception("Number unit is error!");
            }
        } else if (type1.getValue() == NumberUnitType.CM.getValue()) {
            switch (distance) {
                case MM:
                    return src.multiply(new BigDecimal(10));
                case CM:
                    return src;
                case DM:
                    return src.multiply(new BigDecimal(0.1));
                case M:
                    return src.multiply(new BigDecimal(0.01));
                case KM:
                    return src.multiply(new BigDecimal(0.00001));
                default:
                    throw new Exception("Number unit is error!");
            }
        } else if (type1.getValue() == NumberUnitType.DM.getValue()) {
            switch (distance) {
                case MM:
                    return src.multiply(new BigDecimal(100));
                case CM:
                    return src.multiply(new BigDecimal(10));
                case DM:
                    return src;
                case M:
                    return src.multiply(new BigDecimal(0.1));
                case KM:
                    return src.multiply(new BigDecimal(0.0001));
                default:
                    throw new Exception("Number unit is error!");
            }
        } else if (type1.getValue() == NumberUnitType.M.getValue()) {
            switch (distance) {
                case MM:
                    return src.multiply(new BigDecimal(1000));
                case CM:
                    return src.multiply(new BigDecimal(100));
                case DM:
                    return src.multiply(new BigDecimal(10));
                case M:
                    return src;
                case KM:
                    return src.multiply(new BigDecimal(0.001));
                default:
                    throw new Exception("Number unit is error!");
            }
        } else if (type1.getValue() == NumberUnitType.KM.getValue()) {
            switch (distance) {
                case MM:
                    return src.multiply(new BigDecimal(1000000));
                case CM:
                    return src.multiply(new BigDecimal(100000));
                case DM:
                    return src.multiply(new BigDecimal(10000));
                case M:
                    return src.multiply(new BigDecimal(1000));
                case KM:
                    return src;
                default:
                    throw new Exception("Number unit is error!");
            }
        } else if (type1.getValue() == NumberUnitType.MM2.getValue()) {
            switch (area) {
                case MM2:
                    return src;
                case CM2:
                    return src.multiply(new BigDecimal(0.01));
                case DM2:
                    return src.multiply(new BigDecimal(0.0001));
                case M2:
                    return src.multiply(new BigDecimal(0.000001));
                case KM2:
                    return src.multiply(new BigDecimal(0.000000000001));
                default:
                    throw new Exception("Number unit is error!");
            }
        } else if (type1.getValue() == NumberUnitType.CM2.getValue()) {
            switch (area) {
                case MM2:
                    return src.multiply(new BigDecimal(100));
                case CM2:
                    return src;
                case DM2:
                    return src.multiply(new BigDecimal(0.01));
                case M2:
                    return src.multiply(new BigDecimal(0.0001));
                case KM2:
                    return src.multiply(new BigDecimal(0.0000000001));
                default:
                    throw new Exception("Number unit is error!");
            }
        } else if (type1.getValue() == NumberUnitType.DM2.getValue()) {
            switch (area) {
                case MM2:
                    return src.multiply(new BigDecimal(10000));
                case CM2:
                    return src.multiply(new BigDecimal(100));
                case DM2:
                    return src;
                case M2:
                    return src.multiply(new BigDecimal(0.01));
                case KM2:
                    return src.multiply(new BigDecimal(0.00000001));
                default:
                    throw new Exception("Number unit is error!");
            }
        } else if (type1.getValue() == NumberUnitType.M2.getValue()) {
            switch (area) {
                case MM2:
                    return src.multiply(new BigDecimal(1000000));
                case CM2:
                    return src.multiply(new BigDecimal(10000));
                case DM2:
                    return src.multiply(new BigDecimal(100));
                case M2:
                    return src;
                case KM2:
                    return src.multiply(new BigDecimal(0.000001));
                default:
                    throw new Exception("Number unit is error!");
            }
        } else if (type1.getValue() == NumberUnitType.KM2.getValue()) {
            switch (area) {
                case MM2:
                    return src.multiply(new BigDecimal(1000000000000d));
                case CM2:
                    return src.multiply(new BigDecimal(10000000000d));
                case DM2:
                    return src.multiply(new BigDecimal(100000000));
                case M2:
                    return src.multiply(new BigDecimal(1000000));
                case KM2:
                    return src;
                default:
                    throw new Exception("Number unit is error!");
            }
        } else if (type1.getValue() == NumberUnitType.MM3.getValue()) {
            switch (volume) {
                case MM3:
                    return src;
                case CM3:
                    return src.multiply(new BigDecimal(0.001));
                case DM3:
                    return src.multiply(new BigDecimal(0.000001));
                case M3:
                    return src.multiply(new BigDecimal(0.000000001));
                case KM3:
                    return src.multiply(new BigDecimal(0.000000000000000001));
                default:
                    throw new Exception("Number unit is error!");
            }
        } else if (type1.getValue() == NumberUnitType.CM3.getValue()) {
            switch (volume) {
                case MM3:
                    return src.multiply(new BigDecimal(1000));
                case CM3:
                    return src;
                case DM3:
                    return src.multiply(new BigDecimal(0.001));
                case M3:
                    return src.multiply(new BigDecimal(0.000001));
                case KM3:
                    return src.multiply(new BigDecimal(0.000000000000001));
                default:
                    throw new Exception("Number unit is error!");
            }
        } else if (type1.getValue() == NumberUnitType.DM3.getValue()) {
            switch (volume) {
                case MM3:
                    return src.multiply(new BigDecimal(1000000));
                case CM3:
                    return src.multiply(new BigDecimal(1000));
                case DM3:
                    return src;
                case M3:
                    return src.multiply(new BigDecimal(0.001));
                case KM3:
                    return src.multiply(new BigDecimal(0.000000000001));
                default:
                    throw new Exception("Number unit is error!");
            }
        } else if (type1.getValue() == NumberUnitType.M3.getValue()) {
            switch (volume) {
                case MM3:
                    return src.multiply(new BigDecimal(1000000000));
                case CM3:
                    return src.multiply(new BigDecimal(1000000));
                case DM3:
                    return src.multiply(new BigDecimal(1000));
                case M3:
                    return src;
                case KM3:
                    return src.multiply(new BigDecimal(0.000000001));
                default:
                    throw new Exception("Number unit is error!");
            }
        } else if (type1.getValue() == NumberUnitType.KM3.getValue()) {
            switch (volume) {
                case MM3:
                    return src.multiply(new BigDecimal(1000000000000000000d));
                case CM3:
                    return src.multiply(new BigDecimal(1000000000000000d));
                case DM3:
                    return src.multiply(new BigDecimal(1000000000000d));
                case M3:
                    return src.multiply(new BigDecimal(1000000000));
                case KM3:
                    return src;
                default:
                    throw new Exception("Number unit is error!");
            }
        } else if (type1.getValue() == NumberUnitType.G.getValue()) {
            switch (mass) {
                case G:
                    return src;
                case KG:
                    return src.multiply(new BigDecimal(0.001));
                case T:
                    return src.multiply(new BigDecimal(0.000001));
                default:
                    throw new Exception("Number unit is error!");
            }
        } else if (type1.getValue() == NumberUnitType.KG.getValue()) {
            switch (mass) {
                case G:
                    return src.multiply(new BigDecimal(1000));
                case KG:
                    return src;
                case T:
                    return src.multiply(new BigDecimal(0.001));
                default:
                    throw new Exception("Number unit is error!");
            }
        } else if (type1.getValue() == NumberUnitType.T.getValue()) {
            switch (mass) {
                case G:
                    return src.multiply(new BigDecimal(1000000));
                case KG:
                    return src.multiply(new BigDecimal(1000));
                case T:
                    return src;
                default:
                    throw new Exception("Number unit is error!");
            }
        }
        throw new Exception("Number unit is error!");
    }

}
