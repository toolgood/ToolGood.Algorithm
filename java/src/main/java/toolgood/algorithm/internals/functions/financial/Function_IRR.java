package toolgood.algorithm.internals.functions.financial;

import java.util.ArrayList;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_IRR extends Function_2 {
    public Function_IRR(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "IRR";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand valuesArg = GetArray_1(engine, tempParameter);
        if (valuesArg.IsErrorOrNone()) return valuesArg;

        List<Double> values = new ArrayList<>();
        for (Operand v : valuesArg.ArrayValue()) {
            if (v.IsNumber()) {
                values.add(v.DoubleValue());
            } else {
                Operand v2 = v.ToNumber("Function 'IRR' parameter 1 is error!");
                if (v2.IsErrorOrNone()) return v2;
                values.add(v2.DoubleValue());
            }
        }

        if (values.isEmpty()) return ParameterError(1);

        boolean hasPositive = false;
        boolean hasNegative = false;
        for (double v : values) {
            if (v > 0) hasPositive = true;
            if (v < 0) hasNegative = true;
        }
        if (!hasPositive || !hasNegative) return ParameterError(1);

        double guess = 0.1;
        if (func2 != null) {
            Operand guessArg = GetNumber_2(engine, tempParameter);
            if (guessArg.IsErrorOrNone()) return guessArg;
            guess = guessArg.DoubleValue();
        }

        double irr = newtonRaphsonIRR(values, guess);
        return Operand.Create(irr);
    }

    private double newtonRaphsonIRR(List<Double> values, double guess) {
        double rate = guess;
        for (int iter = 0; iter < 100; iter++) {
            double npv = 0;
            double dnpv = 0;

            for (int i = 0; i < values.size(); i++) {
                double factor = Math.pow(1 + rate, i);
                npv += values.get(i) / factor;
                dnpv -= i * values.get(i) / (factor * (1 + rate));
            }

            if (Math.abs(dnpv) < 1e-12) break;
            double newRate = rate - npv / dnpv;

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
        func1.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        if (func2 != null) func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
