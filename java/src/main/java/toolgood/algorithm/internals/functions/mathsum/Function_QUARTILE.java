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
import toolgood.algorithm.mathNet.ExcelFunctions;

public final class Function_QUARTILE extends Function_2 {
    public Function_QUARTILE(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Quartile";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
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

        int quant = args2.IntValue();
        if (quant < 0 || quant > 4) {
            return ParameterError(2);
        }
        return Operand.Create(ExcelFunctions.Quartile(list.toArray(new BigDecimal[0]), quant));
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
