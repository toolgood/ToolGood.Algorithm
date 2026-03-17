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

public final class Function_OCT2DEC extends Function_2 {
    public Function_OCT2DEC(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Oct2Dec";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetText_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }

        if (RegexHelper.IsOct(args1.TextValue()) == false) { return ParameterError(1); }
        int num = Integer.parseInt(args1.TextValue(), 8);
        if (func2 != null) {
            Operand args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone()) { return args2; }
            if (args2.IntValue() < 0) {
                return ParameterError(2);
            }
            String n = Integer.toString(num);
            if (n.length() <= args2.IntValue()) {
                return Operand.Create(n);
            }
            return ParameterError(2);
        }
        return Operand.Create(num);
    }

    @Override
    public OperandType GetResultType() {
        if (func2 != null) {
            return OperandType.TEXT;
        }
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
        if (func2 != null) {
            func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        }
    }
}
