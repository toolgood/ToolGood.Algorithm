package toolgood.algorithm.internals.functions.financial;

import java.math.BigDecimal;
import java.math.MathContext;
import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_5;

public final class Function_NPER extends Function_5 {
    public Function_NPER(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 3 || funcs.length > 5) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires 3 to 5 parameters.");
        }
    }

    @Override
    public String Name() {
        return "NPER";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand rateArg = GetNumber_1(engine, tempParameter);
        if (rateArg.IsErrorOrNone()) return rateArg;
        BigDecimal rate = rateArg.NumberValue();

        Operand pmtArg = GetNumber_2(engine, tempParameter);
        if (pmtArg.IsErrorOrNone()) return pmtArg;
        BigDecimal pmt = pmtArg.NumberValue();

        Operand pvArg = GetNumber_3(engine, tempParameter);
        if (pvArg.IsErrorOrNone()) return pvArg;
        BigDecimal pv = pvArg.NumberValue();

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
        }

        if (rate.compareTo(BigDecimal.ZERO) == 0) {
            if (pmt.compareTo(BigDecimal.ZERO) == 0) return Div0Error();
            return Operand.Create(pv.add(fv).negate().divide(pmt, MathContext.DECIMAL128));
        }
        if (rate.compareTo(BigDecimal.ONE.negate()) == 0) {
            return Div0Error();
        }

        BigDecimal factor = pmt;
        if (type == 1) {
            factor = pmt.multiply(BigDecimal.ONE.add(rate));
        }

        double nper = Math.log(
                fv.multiply(rate).negate().add(factor).divide(pv.multiply(rate).add(factor), MathContext.DECIMAL128).doubleValue())
                / Math.log(BigDecimal.ONE.add(rate).doubleValue());
        return Operand.Create(BigDecimal.valueOf(nper));
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        if (func4 != null) func4.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        if (func5 != null) func5.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
