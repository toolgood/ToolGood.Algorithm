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
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.internals.functions.Function_2;

final class Function_COVARIANCES extends Function_2 {
    public Function_COVARIANCES(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 2) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 2 parameters.");
        }
    }

    @Override
    public String Name() {
        return "Covariances";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }
        Operand args2 = func2.Evaluate(engine, tempParameter);
        if (args2.IsErrorOrNone()) {
            return args2;
        }

        List<BigDecimal> list1 = new ArrayList<>();
        List<BigDecimal> list2 = new ArrayList<>();

        boolean o1 = FunctionUtil.FlattenToNumberList(args1, list1);
        boolean o2 = FunctionUtil.FlattenToNumberList(args2, list2);
        if (o1 == false) {
            return ParameterError(1);
        }
        if (o2 == false) {
            return ParameterError(2);
        }
        if (list1.size() != list2.size()) {
            return Operand.Error("Function '" + Name() + "' parameter's count error!");
        }
        if (list1.size() == 1) {
            return Operand.Error("Function '" + Name() + "' parameter's count error!");
        }

        double mean1 = 0, mean2 = 0, c = 0;
        for (int i = 0; i < list1.size(); i++) {
            double delta1 = list1.get(i).doubleValue() - mean1;
            double delta2 = list2.get(i).doubleValue() - mean2;
            mean1 += delta1 / (i + 1);
            mean2 += delta2 / (i + 1);
            c += delta1 * (list2.get(i).doubleValue() - mean2);
        }
        return Operand.Create(BigDecimal.valueOf(c / (list1.size() - 1)));
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        func2.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
    }
}
