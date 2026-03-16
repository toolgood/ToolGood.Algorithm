package toolgood.algorithm.internals.functions.operator;

import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.NoneEngine;
import toolgood.algorithm.internals.functions.Function_N;

public class Function_XOR extends Function_N {
    public Function_XOR(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String getName() {
        return "XOR";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        int trueCount = 0;
        for (int i = 0; i < funcs.length; i++) {
            Operand a = GetBoolean(engine, tempParameter, i);
            if (a.IsErrorOrNone()) {
                return a;
            }
            if (a.BooleanValue()) {
                trueCount++;
            }
        }
        return (trueCount % 2 == 1) ? Operand.True : Operand.False;
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.BOOLEAN;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        for (int i = 0; i < funcs.length; i++) {
            funcs[i].GetParameterTypes(noneEngine, result, OperandType.BOOLEAN, op, val);
        }
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "XOR");
    }
}
