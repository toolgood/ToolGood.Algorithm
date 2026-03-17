package toolgood.algorithm.internals.functions.mathbase;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.ArrayList;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_MULTINOMIAL extends Function_N {
    public Function_MULTINOMIAL(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Multinomial";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        List<Operand> args = new ArrayList<>(funcs.length);
        Operand error = TryEvaluateAll(engine, tempParameter, args);
        if (error != null) {
            return error;
        }

        List<BigDecimal> list = new ArrayList<>();
        boolean o = FunctionUtil.FlattenToList(args, list);
        if (o == false) {
            return FunctionError();
        }

        int sum = 0;
        BigDecimal n = BigDecimal.ONE;
        for (int i = 0; i < list.size(); i++) {
            int a = list.get(i).intValue();
            if (a < 0) {
                return ParameterError(i + 1);
            }
            n = n.multiply(BigDecimal.valueOf(FunctionUtil.GetFactorial(a)));
            sum += a;
        }

        BigDecimal r = BigDecimal.valueOf(FunctionUtil.GetFactorial(sum)).divide(n, RoundingMode.HALF_UP);
        return Operand.Create(r);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        for (FunctionBase item : funcs) {
            item.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        }
    }
}
