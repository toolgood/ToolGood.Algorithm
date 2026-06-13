package toolgood.algorithm.internals.functions.mathSum;

import java.util.List;
import java.util.ArrayList;
import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.mathNet.ExcelFunctions;

final class Function_PERCENTRANK extends Function_3 {

    Function_PERCENTRANK(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 2 || funcs.length > 3) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires 2 to 3 parameters.");
        }
    }

    @Override
    public String Name() {
        return "PercentRank";
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
        double[] doubleArray = new double[list.size()];
        for (int i = 0; i < list.size(); i++) doubleArray[i] = list.get(i).doubleValue();
        double v = ExcelFunctions.PercentRank(doubleArray, k.doubleValue());
        int d = 3;
        if (func3 != null) {
            Operand args3 = GetNumber_3(engine, tempParameter);
            if (args3.IsErrorOrNone()) { return args3; }
            d = args3.IntValue();
            if (d < 0) {
                return ParameterError(3);
            }
        }
        return Operand.Create(BigDecimal.valueOf(v).setScale(d, RoundingMode.HALF_UP));
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        if (func3 != null) {
            func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        }
    }
}
