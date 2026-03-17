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

public final class Function_FV extends Function_5 {
    public Function_FV(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "FV";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand rateArg = GetNumber_1(engine, tempParameter);
        if (rateArg.IsErrorOrNone()) return rateArg;
        double rate = rateArg.DoubleValue();

        Operand nperArg = GetNumber_2(engine, tempParameter);
        if (nperArg.IsErrorOrNone()) return nperArg;
        double nper = nperArg.DoubleValue();

        Operand pmtArg = GetNumber_3(engine, tempParameter);
        if (pmtArg.IsErrorOrNone()) return pmtArg;
        double pmt = pmtArg.DoubleValue();

        double pv = 0;
        if (func4 != null) {
            Operand pvArg = GetNumber_4(engine, tempParameter);
            if (pvArg.IsErrorOrNone()) return pvArg;
            pv = pvArg.DoubleValue();
        }

        int type = 0;
        if (func5 != null) {
            Operand typeArg = GetNumber_5(engine, tempParameter);
            if (typeArg.IsErrorOrNone()) return typeArg;
            type = typeArg.IntValue();
            if (type != 0 && type != 1) return ParameterError(5);
        }

        if (rate == 0) {
            return Operand.Create(-pmt * nper - pv);
        }

        double factor = Math.pow(1 + rate, nper);
        double fv = -pv * factor - pmt * (factor - 1) / rate;
        if (type == 1) {
            fv = -pv * factor - pmt * (1 + rate) * (factor - 1) / rate;
        }

        return Operand.Create(fv);
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
