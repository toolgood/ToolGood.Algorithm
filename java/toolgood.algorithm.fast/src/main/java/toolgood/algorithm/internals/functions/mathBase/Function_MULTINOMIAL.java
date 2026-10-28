package toolgood.algorithm.internals.functions.mathBase;

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
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.internals.functions.Function_N;

public final class Function_MULTINOMIAL extends Function_N {
    public Function_MULTINOMIAL(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 1) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires at least 1 parameter.");
        }
    }

    @Override
    public String Name() {
        return "Multinomial";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        List<BigDecimal> list = new ArrayList<>();
        for (int i = 0; i < funcs.length; i++) {
            // 按参数逐个求值并展平：展平后的下标与原始参数位置不再一一对应，
            // 逐个展平才能让错误消息给出真实的参数序号
            Operand aa = funcs[i].Evaluate(engine, tempParameter);
            if (aa.IsErrorOrNone()) { return aa; }
            List<BigDecimal> item = new ArrayList<>();
            if (FunctionUtil.FlattenToNumberList(aa, item) == false) { return FunctionError(); }
            for (int j = 0; j < item.size(); j++) {
                BigDecimal value = item.get(j);
                if (value.compareTo(BigDecimal.valueOf(Integer.MAX_VALUE)) > 0 || value.compareTo(BigDecimal.valueOf(Integer.MIN_VALUE)) < 0) {
                    return ParameterError(i + 1);
                }
                if (value.intValue() < 0) {
                    return ParameterError(i + 1);
                }
            }
            list.addAll(item);
        }
        BigDecimal sum = BigDecimal.ZERO;
        BigDecimal n = BigDecimal.ONE;
        for (int i = 0; i < list.size(); i++) {
            BigDecimal value = list.get(i);
            int a = value.intValue();
            n = n.multiply(FunctionUtil.GetFactorial(BigDecimal.valueOf(a)));
            sum = sum.add(BigDecimal.valueOf(a));
        }
        BigDecimal r = FunctionUtil.GetFactorial(sum).divide(n);
        return Operand.Create(r);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        for (FunctionBase item : funcs) {
            item.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        }
    }
}
