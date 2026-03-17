package toolgood.algorithm.internals.functions.mathsum;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.ArrayList;
import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.mathNet.ExcelFunctions;

public final class Function_PERCENTRANK extends Function_3 {
    public Function_PERCENTRANK(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "PercentRank";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetArray_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }

        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) {
            return args2;
        }

        List<BigDecimal> list = new ArrayList<>();
        boolean o = FunctionUtil.FlattenToList_Operand_BigDecimal(args1, list);
        if (o == false) {
            return ParameterError(1);
        }
        if (list.size() == 0) {
            return ParameterError(1);
        }

        BigDecimal k = args2.NumberValue();
        BigDecimal v = ExcelFunctions.PercentRank(list.toArray(new BigDecimal[0]), k);
        int d = 3;
        if (func3 != null) {
            Operand args3 = GetNumber_3(engine, tempParameter);
            if (args3.IsErrorOrNone()) {
                return args3;
            }
            d = args3.IntValue();
            if (d < 0) {
                return ParameterError(3);
            }
        }
        return Operand.Create(v.setScale(d, RoundingMode.HALF_UP));
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
