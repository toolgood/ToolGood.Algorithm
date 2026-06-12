package toolgood.algorithm.internals.functions.mathSum;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;

final class Function_SUMXMY2 extends Function_N {
    public Function_SUMXMY2(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 2) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 2 parameters.");
        }
    }

    @Override
    public String Name() {
        return "SUMXMY2";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        if (funcs.length < 2)
            return ParameterError(1);

        Operand arrayXArg = GetArray(engine, tempParameter, 0);
        if (arrayXArg.IsErrorOrNone())
            return arrayXArg;
        List<BigDecimal> arrayX = new ArrayList<>();
        for (Operand item : arrayXArg.ArrayValue()) {
            if (item.IsNumber()) {
                arrayX.add(item.NumberValue());
            }
        }

        Operand arrayYArg = GetArray(engine, tempParameter, 1);
        if (arrayYArg.IsErrorOrNone())
            return arrayYArg;
        List<BigDecimal> arrayY = new ArrayList<>();
        for (Operand item : arrayYArg.ArrayValue()) {
            if (item.IsNumber()) {
                arrayY.add(item.NumberValue());
            }
        }

        int minLength = arrayX.size() < arrayY.size() ? arrayX.size() : arrayY.size();

        double result = 0;
        for (int i = 0; i < minLength; i++) {
            double diff = arrayX.get(i).doubleValue() - arrayY.get(i).doubleValue();
            result += diff * diff;
        }

        return Operand.Create(BigDecimal.valueOf(result));
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        funcs[0].GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        funcs[1].GetParameterTypes(noneEngine, result, OperandType.ARRAY);
    }
}
