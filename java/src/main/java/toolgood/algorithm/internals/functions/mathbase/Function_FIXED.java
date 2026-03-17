package toolgood.algorithm.internals.functions.mathbase;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.text.DecimalFormat;
import java.text.DecimalFormatSymbols;
import java.util.List;
import java.util.Locale;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_FIXED extends Function_3 {
    public Function_FIXED(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Fixed";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        int num = 2;
        if (func2 != null) {
            Operand args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone()) {
                return args2;
            }
            num = args2.IntValue();
        }

        boolean noCommas = false;
        if (func3 != null) {
            Operand args3 = GetBoolean_3(engine, tempParameter);
            if (args3.IsErrorOrNone()) {
                return args3;
            }
            noCommas = args3.BooleanValue();
        }

        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }

        BigDecimal value = args1.NumberValue();
        value = value.setScale(num, RoundingMode.HALF_UP);

        if (noCommas) {
            return Operand.Create(value.toPlainString());
        }

        DecimalFormatSymbols symbols = new DecimalFormatSymbols(Locale.US);
        DecimalFormat df = new DecimalFormat("#,##0." + (num > 0 ? new String(new char[num]).replace('\0', '0') : ""), symbols);
        return Operand.Create(df.format(value));
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.TEXT;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        if (func2 != null) {
            func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        }
        if (func3 != null) {
            func3.GetParameterTypes(noneEngine, result, OperandType.BOOLEAN);
        }
    }
}
