package toolgood.algorithm.internals.functions.value;

import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.Function_0;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_PARAMETER extends Function_0 {
    private final String name;

    public Function_PARAMETER(String name) {
        this.name = name;
    }

    @Override
    public String Name() {
        return "Parameter";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        String txt = name;
        if (tempParameter != null) {
            Operand r = tempParameter.apply(engine, txt);
            if (r != null) {
                return r;
            }
        }
        return engine.GetParameter(txt);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        stringBuilder.append(name);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NONE;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        ParameterType pt = new ParameterType();
        pt.Name = name;
        pt.Type = operandType;
        pt.Operator = op;
        pt.Value = val;
        result.add(pt);
    }
}
