package toolgood.algorithm.internals.functions.csharp;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_LOOKFLOOR extends Function_2 {

    public Function_LOOKFLOOR(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 2) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 2 parameters.");
        }
    }

    @Override
    public String Name() {
        return "LookFloor";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }

        Operand args2 = GetArray_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) {
            return args2;
        }

        List<BigDecimal> list = new ArrayList<BigDecimal>();
        boolean o = FunctionUtil.FlattenToNumberList(args2, list);
        if (o == false) {
            return ParameterError(2);
        }
        if (list.size() == 0) {
            return ParameterError(2);
        }
        Collections.sort(list);

        BigDecimal value = args1.NumberValue();
        int index = Collections.binarySearch(list, value);
        if (index >= 0) {
            return args1;
        }
        index = -index - 1; // 第一个大于 value 的索引
        if (index == 0) {
            // 所有元素都大于 value，返回最小值
            return Operand.Create(list.get(0));
        }
        return Operand.Create(list.get(index - 1));
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func2.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
    }

}
