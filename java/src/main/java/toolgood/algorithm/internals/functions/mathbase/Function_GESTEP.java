package toolgood.algorithm.internals.functions.mathbase;

import java.math.BigDecimal;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;

/**
 * GESTEP 函数：若 number >= step 返回 1，否则返�?0。第二参数省略时 step 默认�?0�?
 * 参数：GESTEP(number [, step])
 */
public class Function_GESTEP extends Function_2 {

    public Function_GESTEP(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work,
            BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        if (func1 == null) {
            return ParameterError(1);
        }
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = ConvertToNumber(args1, 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        BigDecimal number = args1.NumberValue();

        BigDecimal step = BigDecimal.ZERO;
        if (func2 != null) {
            Operand args2 = func2.Evaluate(work, tempParameter);
            if (args2.IsNotNumber()) {
                args2 = ConvertToNumber(args2, 2);
                if (args2.IsError()) {
                    return args2;
                }
            }
            step = args2.NumberValue();
        }

        return Operand.Create(number.compareTo(step) >= 0 ? 1 : 0);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result,
            OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER, op, val);
        if (func2 != null) {
            func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER, op, val);
        }
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "GESTEP");
    }
}
