package toolgood.algorithm.internals.functions.value;

import java.math.BigDecimal;
import java.util.Map;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.NumberUnitType;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.NumberUnitTypeHelper;
import toolgood.algorithm.internals.functions.Function_0;

public final class Function_Number extends Function_0 {
    private final BigDecimal d;
    private final String unit;

    public Function_Number(BigDecimal func1, String func2) {
        this.d = func1;
        this.unit = func2;
    }

    @Override
    public String Name() {
        return "Num";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Map<String, NumberUnitType> dict = NumberUnitTypeHelper.GetUnitTypedict();
        BigDecimal d2 = NumberUnitTypeHelper.TransformationUnit(d, dict.get(unit), engine.DistanceUnit, engine.AreaUnit, engine.VolumeUnit, engine.MassUnit);
        return Operand.Create(d2);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        stringBuilder.append(d);
        stringBuilder.append(unit);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }
}
