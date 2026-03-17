package toolgood.algorithm.internals.functions.value;

import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_1;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.litJson.JsonData;
import toolgood.algorithm.litJson.JsonMapper;

public final class Function_JSON extends Function_1 {
    public Function_JSON(FunctionBase func1) {
        super(func1);
    }

    @Override
    public String Name() {
        return "Json";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = func1.Evaluate(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }
        if (args1.IsJson()) {
            return args1;
        }
        if (args1.IsArrayJson()) {
            args1 = args1.ToText("");
        }
        if (args1.IsText() == false) {
            return ParameterError(1);
        }
        String txt = args1.TextValue();
        if ((txt.startsWith("{") && txt.endsWith("}")) || (txt.startsWith("[") && txt.endsWith("]"))) {
            try {
                JsonData json = JsonMapper.ToObject(txt);
                return Operand.Create(json);
            } catch (Exception e) {
            }
        }
        return ParameterError(1);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.JSON;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NONE);
    }
}
