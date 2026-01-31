package toolgood.algorithm.internals.functions.value;

import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.internals.AlgorithmEngine;

public class Function_NUM extends FunctionBase {
    private final double d;
    private final String unit;

    public Function_NUM(double func1, String func2) {
        this.d = func1;
        this.unit = func2;
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        var dict = NumberUnitTypeHelper.GetUnitTypedict();
        var d2 = NumberUnitTypeHelper.TransformationUnit(d, dict.get(unit), work.getDistanceUnit(), work.getAreaUnit(), work.getVolumeUnit(), work.getMassUnit());
        return Operand.Create(d2);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        stringBuilder.append(d);
        stringBuilder.append(unit);
    }
}
