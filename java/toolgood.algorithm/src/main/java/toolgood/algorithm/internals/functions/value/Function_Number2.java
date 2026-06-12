package toolgood.algorithm.internals.functions.value;

import java.math.BigDecimal;
import java.util.HashMap;
import java.util.Map;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.AreaUnitType;
import toolgood.algorithm.enums.DistanceUnitType;
import toolgood.algorithm.enums.MassUnitType;
import toolgood.algorithm.enums.NumberUnitType;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.enums.VolumeUnitType;
import toolgood.algorithm.internals.functions.Function_0;

public final class Function_Number2 extends Function_0 {
    private final BigDecimal d;
    private final String unit;

    public Function_Number2(BigDecimal func1, String func2) {
        d = func1;
        unit = func2;
    }

    @Override
    public String Name() {
        return "Num";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Map<String, NumberUnitType> dict = GetUnitTypedict();
        BigDecimal d2 = TransformationUnit(d, dict.get(unit.toUpperCase()), engine.DistanceUnit, engine.AreaUnit, engine.VolumeUnit, engine.MassUnit);
        return Operand.Create(d2);
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        stringBuilder.append(d);
        stringBuilder.append(unit);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    private static final Map<String, NumberUnitType> unitTypedict = new HashMap<>();

    static {
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

    static Map<String, NumberUnitType> GetUnitTypedict() {
        return unitTypedict;
    }

    static BigDecimal TransformationUnit(BigDecimal src, NumberUnitType type1, DistanceUnitType distance, AreaUnitType area, VolumeUnitType volume, MassUnitType mass) {
        if (type1 == NumberUnitType.MM) {
            if (distance == DistanceUnitType.MM) return src;
            if (distance == DistanceUnitType.CM) return src.multiply(new BigDecimal("0.1"));
            if (distance == DistanceUnitType.DM) return src.multiply(new BigDecimal("0.01"));
            if (distance == DistanceUnitType.M) return src.multiply(new BigDecimal("0.001"));
            if (distance == DistanceUnitType.KM) return src.multiply(new BigDecimal("0.000001"));
        } else if (type1 == NumberUnitType.CM) {
            if (distance == DistanceUnitType.MM) return src.multiply(BigDecimal.TEN);
            if (distance == DistanceUnitType.CM) return src;
            if (distance == DistanceUnitType.DM) return src.multiply(new BigDecimal("0.1"));
            if (distance == DistanceUnitType.M) return src.multiply(new BigDecimal("0.01"));
            if (distance == DistanceUnitType.KM) return src.multiply(new BigDecimal("0.00001"));
        } else if (type1 == NumberUnitType.DM) {
            if (distance == DistanceUnitType.MM) return src.multiply(new BigDecimal("100"));
            if (distance == DistanceUnitType.CM) return src.multiply(BigDecimal.TEN);
            if (distance == DistanceUnitType.DM) return src;
            if (distance == DistanceUnitType.M) return src.multiply(new BigDecimal("0.1"));
            if (distance == DistanceUnitType.KM) return src.multiply(new BigDecimal("0.0001"));
        } else if (type1 == NumberUnitType.M) {
            if (distance == DistanceUnitType.MM) return src.multiply(new BigDecimal("1000"));
            if (distance == DistanceUnitType.CM) return src.multiply(new BigDecimal("100"));
            if (distance == DistanceUnitType.DM) return src.multiply(BigDecimal.TEN);
            if (distance == DistanceUnitType.M) return src;
            if (distance == DistanceUnitType.KM) return src.multiply(new BigDecimal("0.001"));
        } else if (type1 == NumberUnitType.KM) {
            if (distance == DistanceUnitType.MM) return src.multiply(new BigDecimal("1000000"));
            if (distance == DistanceUnitType.CM) return src.multiply(new BigDecimal("100000"));
            if (distance == DistanceUnitType.DM) return src.multiply(new BigDecimal("10000"));
            if (distance == DistanceUnitType.M) return src.multiply(new BigDecimal("1000"));
            if (distance == DistanceUnitType.KM) return src;
        } else if (type1 == NumberUnitType.MM2) {
            if (area == AreaUnitType.MM2) return src;
            if (area == AreaUnitType.CM2) return src.multiply(new BigDecimal("0.01"));
            if (area == AreaUnitType.DM2) return src.multiply(new BigDecimal("0.0001"));
            if (area == AreaUnitType.M2) return src.multiply(new BigDecimal("0.000001"));
            if (area == AreaUnitType.KM2) return src.multiply(new BigDecimal("0.000000000001"));
        } else if (type1 == NumberUnitType.CM2) {
            if (area == AreaUnitType.MM2) return src.multiply(new BigDecimal("100"));
            if (area == AreaUnitType.CM2) return src;
            if (area == AreaUnitType.DM2) return src.multiply(new BigDecimal("0.01"));
            if (area == AreaUnitType.M2) return src.multiply(new BigDecimal("0.0001"));
            if (area == AreaUnitType.KM2) return src.multiply(new BigDecimal("0.0000000001"));
        } else if (type1 == NumberUnitType.DM2) {
            if (area == AreaUnitType.MM2) return src.multiply(new BigDecimal("10000"));
            if (area == AreaUnitType.CM2) return src.multiply(new BigDecimal("100"));
            if (area == AreaUnitType.DM2) return src;
            if (area == AreaUnitType.M2) return src.multiply(new BigDecimal("0.01"));
            if (area == AreaUnitType.KM2) return src.multiply(new BigDecimal("0.00000001"));
        } else if (type1 == NumberUnitType.M2) {
            if (area == AreaUnitType.MM2) return src.multiply(new BigDecimal("1000000"));
            if (area == AreaUnitType.CM2) return src.multiply(new BigDecimal("10000"));
            if (area == AreaUnitType.DM2) return src.multiply(new BigDecimal("100"));
            if (area == AreaUnitType.M2) return src;
            if (area == AreaUnitType.KM2) return src.multiply(new BigDecimal("0.000001"));
        } else if (type1 == NumberUnitType.KM2) {
            if (area == AreaUnitType.MM2) return src.multiply(new BigDecimal("1000000000000"));
            if (area == AreaUnitType.CM2) return src.multiply(new BigDecimal("10000000000"));
            if (area == AreaUnitType.DM2) return src.multiply(new BigDecimal("100000000"));
            if (area == AreaUnitType.M2) return src.multiply(new BigDecimal("1000000"));
            if (area == AreaUnitType.KM2) return src;
        } else if (type1 == NumberUnitType.MM3) {
            if (volume == VolumeUnitType.MM3) return src;
            if (volume == VolumeUnitType.CM3) return src.multiply(new BigDecimal("0.001"));
            if (volume == VolumeUnitType.DM3) return src.multiply(new BigDecimal("0.000001"));
            if (volume == VolumeUnitType.M3) return src.multiply(new BigDecimal("0.000000001"));
            if (volume == VolumeUnitType.KM3) return src.multiply(new BigDecimal("0.000000000000000001"));
        } else if (type1 == NumberUnitType.CM3) {
            if (volume == VolumeUnitType.MM3) return src.multiply(new BigDecimal("1000"));
            if (volume == VolumeUnitType.CM3) return src;
            if (volume == VolumeUnitType.DM3) return src.multiply(new BigDecimal("0.001"));
            if (volume == VolumeUnitType.M3) return src.multiply(new BigDecimal("0.000001"));
            if (volume == VolumeUnitType.KM3) return src.multiply(new BigDecimal("0.000000000000001"));
        } else if (type1 == NumberUnitType.DM3) {
            if (volume == VolumeUnitType.MM3) return src.multiply(new BigDecimal("1000000"));
            if (volume == VolumeUnitType.CM3) return src.multiply(new BigDecimal("1000"));
            if (volume == VolumeUnitType.DM3) return src;
            if (volume == VolumeUnitType.M3) return src.multiply(new BigDecimal("0.001"));
            if (volume == VolumeUnitType.KM3) return src.multiply(new BigDecimal("0.000000000001"));
        } else if (type1 == NumberUnitType.M3) {
            if (volume == VolumeUnitType.MM3) return src.multiply(new BigDecimal("1000000000"));
            if (volume == VolumeUnitType.CM3) return src.multiply(new BigDecimal("1000000"));
            if (volume == VolumeUnitType.DM3) return src.multiply(new BigDecimal("1000"));
            if (volume == VolumeUnitType.M3) return src;
            if (volume == VolumeUnitType.KM3) return src.multiply(new BigDecimal("0.000000001"));
        } else if (type1 == NumberUnitType.KM3) {
            if (volume == VolumeUnitType.MM3) return src.multiply(new BigDecimal("1000000000000000000"));
            if (volume == VolumeUnitType.CM3) return src.multiply(new BigDecimal("1000000000000000"));
            if (volume == VolumeUnitType.DM3) return src.multiply(new BigDecimal("1000000000000"));
            if (volume == VolumeUnitType.M3) return src.multiply(new BigDecimal("1000000000"));
            if (volume == VolumeUnitType.KM3) return src;
        } else if (type1 == NumberUnitType.G) {
            if (mass == MassUnitType.G) return src;
            if (mass == MassUnitType.KG) return src.multiply(new BigDecimal("0.001"));
            if (mass == MassUnitType.T) return src.multiply(new BigDecimal("0.000001"));
        } else if (type1 == NumberUnitType.KG) {
            if (mass == MassUnitType.G) return src.multiply(new BigDecimal("1000"));
            if (mass == MassUnitType.KG) return src;
            if (mass == MassUnitType.T) return src.multiply(new BigDecimal("0.001"));
        } else if (type1 == NumberUnitType.T) {
            if (mass == MassUnitType.G) return src.multiply(new BigDecimal("1000000"));
            if (mass == MassUnitType.KG) return src.multiply(new BigDecimal("1000"));
            if (mass == MassUnitType.T) return src;
        }
        throw new IllegalArgumentException("Number unit is error!");
    }
}
