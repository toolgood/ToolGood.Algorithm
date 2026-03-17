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

public final class Function_SWITCH extends Function_N {
    public Function_SWITCH(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Switch";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
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

            if (EqualsOperand(exprValue, compareValue)) {
                return funcs[i + 1].Evaluate(engine, tempParameter);
            }
            i += 2;
        }
        return FunctionError();
    }

    private boolean EqualsOperand(Operand a, Operand b) {
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
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        funcs[0].GetParameterTypes(noneEngine, result, OperandType.NONE);
        int i = 1;
        while (i < funcs.length - 1) {
            funcs[i].GetParameterTypes(noneEngine, result, OperandType.NONE);
            funcs[i + 1].GetParameterTypes(noneEngine, result, OperandType.NONE);
            i += 2;
        }
    }
}
