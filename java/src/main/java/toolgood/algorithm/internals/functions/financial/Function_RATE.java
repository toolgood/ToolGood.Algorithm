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

public final class Function_RATE extends Function_6 {
    public Function_RATE(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "RATE";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand nperArg = GetNumber_1(engine, tempParameter);
        if (nperArg.IsErrorOrNone()) return nperArg;
        double nper = nperArg.DoubleValue();

        Operand pmtArg = GetNumber_2(engine, tempParameter);
        if (pmtArg.IsErrorOrNone()) return pmtArg;
        double pmt = pmtArg.DoubleValue();

        Operand pvArg = GetNumber_3(engine, tempParameter);
        if (pvArg.IsErrorOrNone()) return pvArg;
        double pv = pvArg.DoubleValue();

        if (nper <= 0) return ParameterError(1);

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
            if (type != 0 && type != 1) return ParameterError(5);
        }

        double guess = 0.1;
        if (func6 != null) {
            Operand guessArg = GetNumber_6(engine, tempParameter);
            if (guessArg.IsErrorOrNone()) return guessArg;
            guess = guessArg.DoubleValue();
        }

        double rate = newtonRaphson(nper, pmt, pv, fv, type, guess);
        return Operand.Create(rate);
    }

    private double newtonRaphson(double n, double p, double v, double f, double type, double rate) {
        for (int i = 0; i < 100; i++) {
            double factor = Math.pow(1 + rate, n);
            double fn;

            if (type == 1) {
                fn = v * factor + p * (1 + rate) * (factor - 1) / rate + f;
            } else {
                fn = v * factor + p * (factor - 1) / rate + f;
            }

            double dfn;
            if (type == 1) {
                dfn = v * n * Math.pow(1 + rate, n - 1)
                        + p * (1 + rate) * (n * rate * Math.pow(1 + rate, n - 1) - (factor - 1)) / (rate * rate)
                        + p * (factor - 1) / rate;
            } else {
                dfn = v * n * Math.pow(1 + rate, n - 1)
                        + p * (n * rate * Math.pow(1 + rate, n - 1) - (factor - 1)) / (rate * rate);
            }

            if (Math.abs(dfn) < 1e-12) break;
            double newRate = rate - fn / dfn;

            if (Math.abs(newRate - rate) < 1e-10) {
                return newRate;
            }
            rate = newRate;
        }
        return rate;
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
        if (func6 != null) func6.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
