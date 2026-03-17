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
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_COVARIANCES extends Function_2 {
    public Function_COVARIANCES(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Covariances";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
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

        boolean o1 = FunctionUtil.FlattenToList_Operand_BigDecimal(args1, list1);
        boolean o2 = FunctionUtil.FlattenToList_Operand_BigDecimal(args2, list2);
        if (o1 == false) {
            return ParameterError(1);
        }
        if (o2 == false) {
            return ParameterError(2);
        }
        if (list1.size() != list2.size()) {
            return Operand.Error("Function '{0}' parameter's count error!", "CovarIanceS");
        }
        if (list1.size() == 1) {
            return Operand.Error("Function '{0}' parameter's count error!", "CovarIanceS");
        }

        BigDecimal mean1 = BigDecimal.ZERO, mean2 = BigDecimal.ZERO, c = BigDecimal.ZERO;
        for (int i = 0; i < list1.size(); i++) {
            BigDecimal delta1 = list1.get(i).subtract(mean1);
            BigDecimal delta2 = list2.get(i).subtract(mean2);
            mean1 = mean1.add(delta1.divide(new BigDecimal(i + 1), java.math.MathContext.DECIMAL128));
            mean2 = mean2.add(delta2.divide(new BigDecimal(i + 1), java.math.MathContext.DECIMAL128));
            c = c.add(delta1.multiply(list2.get(i).subtract(mean2)));
        }
        return Operand.Create(c.divide(new BigDecimal(list1.size() - 1), java.math.MathContext.DECIMAL128));
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        func2.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
    }
}
