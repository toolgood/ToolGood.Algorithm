package toolgood.algorithm.internals.functions.financial;

import java.math.BigDecimal;
import java.math.MathContext;
import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_5;

final class Function_PV extends Function_5 {
    public Function_PV(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 3 || funcs.length > 5) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires 3 to 5 parameters.");
        }
    }

    @Override
    public String Name() {
        return "PV";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand rateArg = GetNumber_1(engine, tempParameter);
        if (rateArg.IsErrorOrNone()) return rateArg;
        BigDecimal rate = rateArg.NumberValue();

        Operand nperArg = GetNumber_2(engine, tempParameter);
        if (nperArg.IsErrorOrNone()) return nperArg;
        BigDecimal nper = nperArg.NumberValue();

        Operand pmtArg = GetNumber_3(engine, tempParameter);
        if (pmtArg.IsErrorOrNone()) return pmtArg;
        BigDecimal pmt = pmtArg.NumberValue();

        BigDecimal fv = BigDecimal.ZERO;
        if (func4 != null) {
            Operand fvArg = GetNumber_4(engine, tempParameter);
            if (fvArg.IsErrorOrNone()) return fvArg;
            fv = fvArg.NumberValue();
        }

        int type = 0;
        if (func5 != null) {
            Operand typeArg = GetNumber_5(engine, tempParameter);
            if (typeArg.IsErrorOrNone()) return typeArg;
            type = typeArg.IntValue();
            if (type != 0 && type != 1) {
                return ParameterError(5);
            }
        }

        if (rate.compareTo(BigDecimal.ZERO) == 0) {
            return Operand.Create(pmt.multiply(nper).negate().subtract(fv));
        }

        BigDecimal factor = BigDecimal.valueOf(Math.pow(BigDecimal.ONE.add(rate).doubleValue(), nper.doubleValue()));
        BigDecimal pv = fv.add(pmt.multiply(factor.subtract(BigDecimal.ONE)).divide(rate, MathContext.DECIMAL128))
                .negate()
                .divide(factor, MathContext.DECIMAL128);
        if (type == 1) {
            pv = fv.add(pmt.multiply(BigDecimal.ONE.add(rate)).multiply(factor.subtract(BigDecimal.ONE)).divide(rate, MathContext.DECIMAL128))
                    .negate()
                    .divide(factor, MathContext.DECIMAL128);
        }

        return Operand.Create(pv);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        if (func4 != null) func4.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        if (func5 != null) func5.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
