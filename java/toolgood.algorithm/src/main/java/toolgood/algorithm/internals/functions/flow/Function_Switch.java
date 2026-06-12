package toolgood.algorithm.internals.functions.flow;

import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.Enums.OperandType;
import toolgood.algorithm.internals.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;

final class Function_SWITCH extends Function_N {

    public Function_SWITCH(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 3) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires at least 3 parameters.");
        }
    }

    @Override
    public String Name() {
        return "Switch";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand exprValue = funcs[0].Evaluate(engine, tempParameter);
        if (exprValue.IsErrorOrNone()) {
            return exprValue;
        }

        int i = 1;
        while (i < funcs.length - 1) {
            Operand compareValue = funcs[i].Evaluate(engine, tempParameter);
            if (compareValue.IsErrorOrNone()) {
                return compareValue;
            }

            if (equalsOperand(exprValue, compareValue)) {
                return funcs[i + 1].Evaluate(engine, tempParameter);
            }
            i += 2;
        }

        // 参数数量为偶数时，最后一个参数是默认值
        if (funcs.length % 2 == 0) {
            return funcs[funcs.length - 1].Evaluate(engine, tempParameter);
        }

        return FunctionError();
    }

    private boolean equalsOperand(Operand a, Operand b) {
        if (a.IsNumber() && b.IsNumber()) {
            return a.NumberValue().compareTo(b.NumberValue()) == 0;
        }
        if (a.IsText() && b.IsText()) {
            return a.TextValue().equals(b.TextValue());
        }
        if (a.IsBoolean() && b.IsBoolean()) {
            return a.BooleanValue() == b.BooleanValue();
        }
        if (a.IsNull() && b.IsNull()) {
            return true;
        }
        return false;
    }

    @Override
    public OperandType GetResultType() {
        int i = 1;
        while (i < funcs.length - 1) {
            OperandType t = funcs[i + 1].GetResultType();
            if (t != OperandType.NONE) {
                return t;
            }
            i += 2;
        }
        // 检查默认值的类型
        if (funcs.length % 2 == 0) {
            OperandType t = funcs[funcs.length - 1].GetResultType();
            if (t != OperandType.NONE) {
                return t;
            }
        }
        return OperandType.NONE;
    }

    @Override
    void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        funcs[0].GetParameterTypes(noneEngine, result, OperandType.NONE);
        int i = 1;
        while (i < funcs.length - 1) {
            funcs[i].GetParameterTypes(noneEngine, result, OperandType.NONE);
            funcs[i + 1].GetParameterTypes(noneEngine, result, OperandType.NONE);
            i += 2;
        }
        // 默认值参数
        if (funcs.length % 2 == 0) {
            funcs[funcs.length - 1].GetParameterTypes(noneEngine, result, OperandType.NONE);
        }
    }

}
