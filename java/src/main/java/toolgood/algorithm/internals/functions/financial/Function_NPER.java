package toolgood.algorithm.internals.functions.financial;

import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_5;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_NPER extends Function_5 {
    public Function_NPER(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "NPER";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand rateArg = GetNumber_1(engine, tempParameter);
        if (rateArg.IsErrorOrNone()) return rateArg;
        double rate = rateArg.DoubleValue();

        Operand pmtArg = GetNumber_2(engine, tempParameter);
        if (pmtArg.IsErrorOrNone()) return pmtArg;
        double pmt = pmtArg.DoubleValue();

        Operand pvArg = GetNumber_3(engine, tempParameter);
        if (pvArg.IsErrorOrNone()) return pvArg;
        double pv = pvArg.DoubleValue();

        double fv = 0;
        if (func4 != null) {
            Operand fvArg = GetNumber_4(engine, tempParameter);
            if (fvArg.IsErrorOrNone()) return fvArg;
            fv = fvArg.DoubleValue();
        }

        int type = 0;
        if (func5 != null) {
            Operand typeArg = GetNumber_5(engine, tempParameter);
            if (typeArg.IsErrorOrNone()) return typeArg;
            type = typeArg.IntValue();
        }

        if (rate == 0) {
            if (pmt == 0) return Div0Error();
            return Operand.Create(-(pv + fv) / pmt);
        }
        if (rate == -1) {
            return Div0Error();
        }

        double factor = pmt;
        if (type == 1) {
            factor = pmt * (1 + rate);
        }

        double nper = Math.log((-fv * rate + factor) / (pv * rate + factor)) / Math.log(1 + rate);
        return Operand.Create(nper);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        if (func4 != null) func4.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        if (func5 != null) func5.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
