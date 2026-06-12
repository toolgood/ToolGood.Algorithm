package toolgood.algorithm.internals.functions.mathSum;

import java.util.List;
import java.util.ArrayList;
import java.math.BigDecimal;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.FunctionUtil;

final class Function_SMALL extends Function_2 {

    Function_SMALL(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 2) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 2 parameters.");
        }
    }

    @Override
    public String Name() {
        return "Small";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(engine, tempParameter);
        args1 = ConvertToArray(args1, 1);
        if (args1.IsErrorOrNone()) { return args1; }

        Operand args2 = func2.Evaluate(engine, tempParameter);
        args2 = ConvertToNumber(args2, 2);
        if (args2.IsErrorOrNone()) { return args2; }

        List<BigDecimal> list = new ArrayList<>();
        boolean o = FunctionUtil.FlattenToList(args1, list);
        if (o == false) { return ParameterError(1); }
        if (list.size() == 0) { return ParameterError(1); }

        int excelIndex = engine.UseExcelIndex ? 1 : 0;
        int k = args2.IntValue() - excelIndex;
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
    void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
