package toolgood.algorithm.internals.functions.mathsum;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.internals.functions.NoneEngine;

public class Function_SUMX2PY2 extends Function_N {
    public Function_SUMX2PY2(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "SUMX2PY2";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        if (funcs.length < 2) return ParameterError(1);

        Operand arrayXArg = GetArray(engine, tempParameter, 0);
        if (arrayXArg.IsError()) return arrayXArg;
        List<BigDecimal> arrayX = new ArrayList<>();
        for (Operand item : arrayXArg.ArrayValue()) {
            if (item.IsNumber()) arrayX.add(item.NumberValue());
        }

        Operand arrayYArg = GetArray(engine, tempParameter, 1);
        if (arrayYArg.IsError()) return arrayYArg;
        List<BigDecimal> arrayY = new ArrayList<>();
        for (Operand item : arrayYArg.ArrayValue()) {
            if (item.IsNumber()) arrayY.add(item.NumberValue());
        }

        int minLength = Math.min(arrayX.size(), arrayY.size());

        BigDecimal result = BigDecimal.ZERO;
        for (int i = 0; i < minLength; i++) {
            // X² + Y²
            result = result.add(arrayX.get(i).multiply(arrayX.get(i)))
                           .add(arrayY.get(i).multiply(arrayY.get(i)));
        }

        return Operand.Create(result);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void getParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        funcs[0].getParameterTypes(noneEngine, result, OperandType.ARRAY, null, null);
        funcs[1].getParameterTypes(noneEngine, result, OperandType.ARRAY, null, null);
    }
}
