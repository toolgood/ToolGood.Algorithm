package toolgood.algorithm.internals.functions.mathBase;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.List;
import java.util.Locale;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;

public final class Function_FIXED extends Function_3 {
    public Function_FIXED(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 1 || funcs.length > 3) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires 1 to 3 parameters.");
        }
    }

    @Override
    public String Name() {
        return "Fixed";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        int num = 2;
        if (func2 != null) {
            Operand args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone()) {
                return args2;
            }
            num = args2.IntValue();
            // Excel 支持负数 decimals(向左取整),如 FIXED(1234.567,-1)="1,230",范围与 ROUND 一致
            if (num < -15 || num > 15) {
                return ParameterError(2);
            }
        }
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }
        BigDecimal s = args1.NumberValue().setScale(num, RoundingMode.HALF_UP);
        boolean no = false;
        if (func3 != null) {
            Operand args3 = GetBoolean_3(engine, tempParameter);
            if (args3.IsErrorOrNone()) {
                return args3;
            }
            no = args3.BooleanValue();
        }
        if (no == false) {
            if (num < 0) {
                // 负数位数取整后无小数位,precision 不能为负,用 0 位保持千分位
                return Operand.Create(String.format(Locale.US, "%,.0f", s));
            }
            return Operand.Create(String.format(Locale.US, "%,." + num + "f", s));
        }
        return Operand.Create(s.toPlainString());
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.TEXT;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        if (func2 != null) {
            func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        }
        if (func3 != null) {
            func3.GetParameterTypes(noneEngine, result, OperandType.BOOLEAN);
        }
    }
}
