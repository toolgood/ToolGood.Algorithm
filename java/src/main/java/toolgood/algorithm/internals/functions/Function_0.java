package toolgood.algorithm.internals.functions;

import java.util.List;

import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;

/**
 * 零参函数抽象基类
 */
public abstract class Function_0 extends FunctionBase {

    protected Function_0() {
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NONE;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result,
            OperandType operandType, String op, String val) {
        // 零参函数，无参数类型
    }
}
