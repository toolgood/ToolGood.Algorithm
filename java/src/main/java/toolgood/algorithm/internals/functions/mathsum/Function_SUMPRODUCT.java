package toolgood.algorithm.internals.functions.mathsum;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_SUMPRODUCT extends Function_N {
    public Function_SUMPRODUCT(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "SUMPRODUCT";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        if (funcs.length < 2) return ParameterError(1);

        List<List<BigDecimal>> arrays = new ArrayList<>();
        for (int i = 0; i < funcs.length; i++) {
            Operand arg = GetArray(engine, tempParameter, i);
            if (arg.IsErrorOrNone()) return arg;
            List<BigDecimal> list = new ArrayList<>();
            for (Operand item : arg.ArrayValue()) {
                if (item.IsNumber()) {
                    list.add(item.NumberValue());
                }
            }
            arrays.add(list);
        }

        int minLength = arrays.get(0).size();
        for (int i = 1; i < arrays.size(); i++) {
            if (arrays.get(i).size() < minLength) {
                minLength = arrays.get(i).size();
            }
        }

        if (minLength == 0) {
            return Operand.Zero;
        }

        BigDecimal result = BigDecimal.ZERO;
        for (int i = 0; i < minLength; i++) {
            BigDecimal product = BigDecimal.ONE;
            for (int j = 0; j < arrays.size(); j++) {
                product = product.multiply(arrays.get(j).get(i));
            }
            result = result.add(product);
        }

        return Operand.Create(result);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        for (int i = 0; i < funcs.length; i++) {
            funcs[i].GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        }
    }
}
