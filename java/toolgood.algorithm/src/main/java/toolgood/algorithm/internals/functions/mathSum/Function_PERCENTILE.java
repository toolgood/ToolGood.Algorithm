package toolgood.algorithm.internals.functions.mathSum;

import java.util.List;
import java.util.ArrayList;
import java.math.BigDecimal;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.mathNet.ExcelFunctions;

final class Function_PERCENTILE extends Function_2 {

    Function_PERCENTILE(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 2) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 2 parameters.");
        }
    }

    @Override
    public String Name() {
        return "Percentile";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetArray_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }

        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) { return args2; }

        List<BigDecimal> list = new ArrayList<>();
        boolean o = FunctionUtil.FlattenToList(args1, list);
        if (o == false) { return ParameterError(1); }
        if (list.size() == 0) { return ParameterError(1); }
        BigDecimal k = args2.NumberValue();
        if (k.compareTo(BigDecimal.ZERO) < 0 || k.compareTo(BigDecimal.ONE) > 0) {
            return ParameterError(2);
        }
        double[] doubleArray = new double[list.size()];
        for (int i = 0; i < list.size(); i++) doubleArray[i] = list.get(i).doubleValue();
        try {
            return Operand.Create(BigDecimal.valueOf(ExcelFunctions.Percentile(doubleArray, k.doubleValue())));
        } catch (Exception e) {
            return ParameterError(2);
        }
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
