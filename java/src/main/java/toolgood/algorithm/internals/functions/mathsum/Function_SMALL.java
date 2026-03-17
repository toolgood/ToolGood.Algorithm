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

public final class Function_SMALL extends Function_2 {
    public Function_SMALL(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Small";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(engine, tempParameter);
        args1 = ConvertToArray(args1, 1);
        if (args1.IsErrorOrNone()) {
            return args1;
        }

        Operand args2 = func2.Evaluate(engine, tempParameter);
        args2 = ConvertToNumber(args2, 2);
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

        int k = args2.IntValue() - engine.ExcelIndex;
        if (k < 0 || k >= list.size()) {
            return ParameterError(2);
        }
        return Operand.Create(FunctionUtil.QuickSelect(list, k, false));
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
