package toolgood.algorithm.internals.functions.flow;

import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.internals.functions.NoneEngine;

/**
 * IFS 函数：依次检测多个条件，返回第一个为 true 的条件对应的值。
 * 参数格式：IFS(condition1, value1, condition2, value2, ...)
 */
public class Function_IFS extends Function_N {

    public Function_IFS(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work,
            BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        for (int i = 0; i < funcs.length - 1; i += 2) {
            Operand condition = funcs[i].Evaluate(work, tempParameter);
            if (condition.IsNotBoolean()) {
                condition = condition.ToBoolean(
                        "Function '%s' parameter %d is error!", Name(), i + 1);
            }
            if (condition.IsError()) {
                return condition;
            }
            if (condition.BooleanValue()) {
                return funcs[i + 1].Evaluate(work, tempParameter);
            }
        }
        return FunctionError();
    }

    @Override
    public OperandType GetResultType() {
        for (int i = 0; i < funcs.length - 1; i += 2) {
            OperandType t = funcs[i + 1].GetResultType();
            if (t != OperandType.NONE) {
                return t;
            }
        }
        return OperandType.NONE;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result,
            OperandType operandType, String op, String val) {
        for (int i = 0; i < funcs.length - 1; i += 2) {
            funcs[i].GetParameterTypes(noneEngine, result, OperandType.BOOLEAN, op, val);
            funcs[i + 1].GetParameterTypes(noneEngine, result, OperandType.NONE, op, val);
        }
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "IFS");
    }
}
