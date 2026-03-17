package toolgood.algorithm.internals.functions.financial;

import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_6;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_PPMT extends Function_6 {
    public Function_PPMT(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "PPMT";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand rateArg = GetNumber_1(engine, tempParameter);
        if (rateArg.IsErrorOrNone()) return rateArg;
        double rate = rateArg.DoubleValue();

        Operand perArg = GetNumber_2(engine, tempParameter);
        if (perArg.IsErrorOrNone()) return perArg;
        double per = perArg.DoubleValue();

        Operand nperArg = GetNumber_3(engine, tempParameter);
        if (nperArg.IsErrorOrNone()) return nperArg;
        double nper = nperArg.DoubleValue();

        Operand pvArg = GetNumber_4(engine, tempParameter);
        if (pvArg.IsErrorOrNone()) return pvArg;
        double pv = pvArg.DoubleValue();

        double fv = 0;
        if (func5 != null) {
            Operand fvArg = GetNumber_5(engine, tempParameter);
            if (fvArg.IsErrorOrNone()) return fvArg;
            fv = fvArg.DoubleValue();
        }

        int type = 0;
        if (func6 != null) {
            Operand typeArg = GetNumber_6(engine, tempParameter);
            if (typeArg.IsErrorOrNone()) return typeArg;
            type = typeArg.IntValue();
            if (type != 0 && type != 1) return ParameterError(6);
        }

        double pmtResult = calculatePMT(rate, nper, pv, fv, type);
        double ipmtResult = calculateIPMT(rate, per, nper, pv, fv, type);
        return Operand.Create(pmtResult - ipmtResult);
    }

    private double calculatePMT(double rate, double nper, double pv, double fv, int type) {
        if (rate == 0) {
            return -(pv + fv) / nper;
        }
        double factor = Math.pow(1 + rate, nper);
        double pmt = -(pv * factor + fv) * rate / (factor - 1);
        if (type == 1) {
            pmt = pmt / (1 + rate);
        }
        return pmt;
    }

    private double calculateIPMT(double rate, double per, double nper, double pv, double fv, int type) {
        if (rate == 0) {
            return 0;
        }
        double pmt = calculatePMT(rate, nper, pv, fv, type);
        double factor = Math.pow(1 + rate, per - 1);
        double ipmt = -(pv * factor + pmt * (factor - 1) / rate) * rate;
        if (type == 1 && per == 1) {
            ipmt = 0;
        }
        return ipmt;
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
        func4.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        if (func5 != null) func5.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        if (func6 != null) func6.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
