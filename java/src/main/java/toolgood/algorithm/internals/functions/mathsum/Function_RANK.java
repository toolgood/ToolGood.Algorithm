package toolgood.algorithm.internals.functions.mathsum;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_RANK extends Function_N {
    public Function_RANK(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "RANK";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        if (funcs.length < 2) return ParameterError(1);

        Operand numArg = GetNumber(engine, tempParameter, 0);
        if (numArg.IsErrorOrNone()) return numArg;
        BigDecimal num = numArg.NumberValue();

        Operand arrayArg = GetArray(engine, tempParameter, 1);
        if (arrayArg.IsErrorOrNone()) return arrayArg;

        int order = 0;
        if (funcs.length > 2) {
            Operand orderArg = GetNumber(engine, tempParameter, 2);
            if (orderArg.IsErrorOrNone()) return orderArg;
            order = orderArg.IntValue();
        }

        List<BigDecimal> values = new ArrayList<>();
        for (Operand item : arrayArg.ArrayValue()) {
            if (item.IsNumber()) {
                values.add(item.NumberValue());
            }
        }

        if (values.size() == 0) {
            return ParameterError(2);
        }

        boolean descending = (order == 0);
        int rank = FunctionUtil.GetRank(values, num, descending);

        if (rank == 0) {
            return ParameterError(1);
        }

        return Operand.Create(rank);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        funcs[0].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        funcs[1].GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        if (funcs.length > 2) {
            funcs[2].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        }
    }
}
