package toolgood.algorithm.internals.functions.mathSum;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;

public final class Function_SUMPRODUCT extends Function_N {
    public Function_SUMPRODUCT(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 1) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires at least 1 parameter.");
        }
    }

    @Override
    public String Name() {
        return "SUMPRODUCT";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        if (funcs.length < 2)
            return ParameterError(1);

        List<List<BigDecimal>> arrays = new ArrayList<>();
        for (int i = 0; i < funcs.length; i++) {
            Operand arg = GetArray(engine, tempParameter, i);
            if (arg.IsErrorOrNone())
                return arg;
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

        double result = 0;
        for (int i = 0; i < minLength; i++) {
            double product = 1;
            for (int j = 0; j < arrays.size(); j++) {
                product *= arrays.get(j).get(i).doubleValue();
            }
            result += product;
        }

        return Operand.Create(BigDecimal.valueOf(result));
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
