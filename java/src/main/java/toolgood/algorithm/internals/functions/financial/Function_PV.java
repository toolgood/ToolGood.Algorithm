package toolgood.algorithm.internals.functions.financial;

import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_5;

/**
 * PV: 返回投资的现值
 * PV(rate, nper, pmt, [fv], [type])
 */
public class Function_PV extends Function_5 {
    public Function_PV(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand rateArg = GetNumber_1(engine, tempParameter);
        if (rateArg.IsError()) return rateArg;
        double rate = rateArg.DoubleValue();

        Operand nperArg = GetNumber_2(engine, tempParameter);
        if (nperArg.IsError()) return nperArg;
        double nper = nperArg.DoubleValue();

        Operand pmtArg = GetNumber_3(engine, tempParameter);
        if (pmtArg.IsError()) return pmtArg;
        double pmt = pmtArg.DoubleValue();

        double fv = 0;
        if (func4 != null) {
            Operand fvArg = GetNumber_4(engine, tempParameter);
            if (fvArg.IsError()) return fvArg;
            fv = fvArg.DoubleValue();
        }

        int type = 0;
        if (func5 != null) {
            Operand typeArg = GetNumber_5(engine, tempParameter);
            if (typeArg.IsError()) return typeArg;
            type = typeArg.IntValue();
            if (type != 0 && type != 1) return ParameterError(5);
        }

        if (rate == 0) {
            return Operand.Create(-pmt * nper - fv);
        }

        double factor = Math.pow(1 + rate, nper);
        double pv;
        if (type == 1) {
            pv = -(fv + pmt * (1 + rate) * (factor - 1) / rate) / factor;
        } else {
            pv = -(fv + pmt * (factor - 1) / rate) / factor;
        }

        return Operand.Create(pv);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "PV");
    }
}
