package toolgood.algorithm.internals.functions.financial;

import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_6;

/**
 * IPMT: 返回在给定期数内对投资的利息偿还额
 * IPMT(rate, per, nper, pv, [fv], [type])
 */
public class Function_IPMT extends Function_6 {
    public Function_IPMT(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand rateArg = GetNumber_1(engine, tempParameter);
        if (rateArg.IsError()) return rateArg;
        double rate = rateArg.DoubleValue();

        Operand perArg = GetNumber_2(engine, tempParameter);
        if (perArg.IsError()) return perArg;
        double per = perArg.DoubleValue();

        Operand nperArg = GetNumber_3(engine, tempParameter);
        if (nperArg.IsError()) return nperArg;
        double nper = nperArg.DoubleValue();

        Operand pvArg = GetNumber_4(engine, tempParameter);
        if (pvArg.IsError()) return pvArg;
        double pv = pvArg.DoubleValue();

        double fv = 0;
        if (func5 != null) {
            Operand fvArg = GetNumber_5(engine, tempParameter);
            if (fvArg.IsError()) return fvArg;
            fv = fvArg.DoubleValue();
        }

        int type = 0;
        if (func6 != null) {
            Operand typeArg = GetNumber_6(engine, tempParameter);
            if (typeArg.IsError()) return typeArg;
            type = typeArg.IntValue();
            if (type != 0 && type != 1) return ParameterError(6);
        }

        if (rate == 0) {
            return Operand.Create(0.0);
        }

        double pmt = calculatePMT(rate, nper, pv, fv, type);
        double factor = Math.pow(1 + rate, per - 1);
        double ipmt = -(pv * factor + pmt * (factor - 1) / rate) * rate;

        if (type == 1 && per == 1) {
            ipmt = 0;
        }

        return Operand.Create(ipmt);
    }

    private double calculatePMT(double rate, double nper, double pv, double fv, int type) {
        double factor = Math.pow(1 + rate, nper);
        double pmt = -(pv * factor + fv) * rate / (factor - 1);
        if (type == 1) {
            pmt = pmt / (1 + rate);
        }
        return pmt;
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "IPMT");
    }
}
