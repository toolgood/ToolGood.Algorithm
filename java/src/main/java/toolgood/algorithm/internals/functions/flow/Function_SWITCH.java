package toolgood.algorithm.internals.functions.flow;

import java.math.BigDecimal;
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
 * SWITCH 函数：对表达式求值，与后续候选值逐一比较，返回第一个匹配的结果�?
 * 参数格式：SWITCH(expr, val1, result1, val2, result2, ...[, default])
 */
public class Function_SWITCH extends Function_N {

    public Function_SWITCH(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work,
            BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand exprValue = funcs[0].Evaluate(work, tempParameter);
        if (exprValue.IsError()) {
            return exprValue;
        }

        int i = 1;
        while (i < funcs.length - 1) {
            Operand compareValue = funcs[i].Evaluate(work, tempParameter);
            if (compareValue.IsError()) {
                return compareValue;
            }
            if (equalsOperand(exprValue, compareValue)) {
                return funcs[i + 1].Evaluate(work, tempParameter);
            }
            i += 2;
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
        return OperandType.NONE;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result,
            OperandType operandType, String op, String val) {
        funcs[0].GetParameterTypes(noneEngine, result, OperandType.NONE, op, val);
        int i = 1;
        while (i < funcs.length - 1) {
            funcs[i].GetParameterTypes(noneEngine, result, OperandType.NONE, op, val);
            funcs[i + 1].GetParameterTypes(noneEngine, result, OperandType.NONE, op, val);
            i += 2;
        }
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "SWITCH");
    }
}
