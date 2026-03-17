package toolgood.algorithm.internals.functions.mathsum;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_SUMIF extends Function_3 {
    public Function_SUMIF(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "SumIf";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetArray_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }
        Operand args2 = func2.Evaluate(engine, tempParameter);
        if (args2.IsErrorOrNone()) {
            return args2;
        }

        List<BigDecimal> list = new ArrayList<>();
        boolean o = FunctionUtil.FlattenToList_Operand_BigDecimal(args1, list);
        if (o == false) {
            return ParameterError(1);
        }

        List<BigDecimal> sumdbs;
        if (func3 != null) {
            Operand args3 = GetArray_3(engine, tempParameter);
            if (args3.IsErrorOrNone()) {
                return args3;
            }
            sumdbs = new ArrayList<>();
            boolean o2 = FunctionUtil.FlattenToList_Operand_BigDecimal(args3, sumdbs);
            if (o2 == false) {
                return ParameterError(3);
            }
        } else {
            sumdbs = list;
        }

        BigDecimal sum;
        if (args2.IsNumber()) {
            sum = new BigDecimal(FunctionUtil.GetCountIf(list, args2.NumberValue())).multiply(args2.NumberValue());
        } else {
            String text = args2.TextValue().trim();
            try {
                BigDecimal d = new BigDecimal(text);
                sum = FunctionUtil.GetSumIf(list, d, sumdbs);
            } catch (NumberFormatException e) {
                Object[] m2 = FunctionUtil.ParseSumIfMatch(text);
                if (m2 != null) {
                    sum = FunctionUtil.GetSumIf(list, (String) m2[0], (BigDecimal) m2[1], sumdbs);
                } else {
                    return ParameterError(2);
                }
            }
        }
        return Operand.Create(sum);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        func2.GetParameterTypes(noneEngine, result, OperandType.NONE);
        if (func3 != null) {
            func3.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        }
    }
}
