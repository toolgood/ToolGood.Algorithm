package toolgood.algorithm.internals.functions.mathtransformation;

import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.RegexHelper;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_OCT2HEX extends Function_2 {
    public Function_OCT2HEX(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Oct2Hex";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetText_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }

        if (RegexHelper.IsOct(args1.TextValue()) == false) { return ParameterError(1); }
        String num = Integer.toHexString(Integer.parseInt(args1.TextValue(), 8)).toUpperCase();
        if (func2 != null) {
            Operand args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone()) { return args2; }
            if (args2.IntValue() < 0) {
                return ParameterError(2);
            }
            if (num.length() <= args2.IntValue()) {
                return Operand.Create(num);
            }
            return ParameterError(2);
        }
        return Operand.Create(num);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.TEXT;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
        if (func2 != null) {
            func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        }
    }
}
