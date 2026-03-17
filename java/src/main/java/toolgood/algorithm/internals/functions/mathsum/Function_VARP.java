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

public final class Function_VARP extends Function_N {
    public Function_VARP(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "VarP";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        List<Operand> args = new ArrayList<>(funcs.length);
        Operand error = TryEvaluateAll(engine, tempParameter, args);
        if (error != null) {
            return error;
        }

        List<BigDecimal> list = new ArrayList<>();
        boolean o = FunctionUtil.FlattenToList_BigDecimal(args, list);
        if (o == false) {
            return FunctionError();
        }
        if (list.size() == 0) {
            return FunctionError();
        }
        BigDecimal mean = BigDecimal.ZERO, m2 = BigDecimal.ZERO;
        for (int i = 0; i < list.size(); i++) {
            BigDecimal delta = list.get(i).subtract(mean);
            mean = mean.add(delta.divide(new BigDecimal(i + 1), java.math.MathContext.DECIMAL128));
            m2 = m2.add(delta.multiply(list.get(i).subtract(mean)));
        }
        return Operand.Create(m2.divide(new BigDecimal(list.size()), java.math.MathContext.DECIMAL128));
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        if (funcs.length == 1) {
            funcs[0].GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        } else {
            for (int i = 0; i < funcs.length; i++) {
                funcs[i].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
            }
        }
    }
}
