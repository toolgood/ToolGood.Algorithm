import { Function_6 } from '../Function_6.js';
import { Operand } from '../../../Operand.js';

class Function_IPMT extends Function_6 {
    get Name() {
        return "IPMT";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        let rateArg = this.getNumber_1(engine, tempParameter);
        if (rateArg.IsError) return rateArg;
        let rate = rateArg.NumberValue;

        let perArg = this.getNumber_2(engine, tempParameter);
        if (perArg.IsError) return perArg;
        let per = perArg.NumberValue;

        let nperArg = this.getNumber_3(engine, tempParameter);
        if (nperArg.IsError) return nperArg;
        let nper = nperArg.NumberValue;

        let pvArg = this.getNumber_4(engine, tempParameter);
        if (pvArg.IsError) return pvArg;
        let pv = pvArg.NumberValue;

        let fv = 0;
        if (this.func5 != null) {
            let fvArg = this.getNumber_5(engine, tempParameter);
            if (fvArg.IsError) return fvArg;
            fv = fvArg.NumberValue;
        }

        let type = 0;
        if (this.func6 != null) {
            let typeArg = this.getNumber_6(engine, tempParameter);
            if (typeArg.IsError) return typeArg;
            type = typeArg.IntValue;
            if (type != 0 && type != 1) {
                return this.parameterError(6);
            }
        }

        if (rate == 0) {
            return Operand.Create(0);
        }

        let pmt = this.calculatePMT(rate, nper, pv, fv, type);
        let factor = Math.pow((1 + rate), (per - 1));
        let ipmt = -(pv * factor + pmt * (factor - 1) / rate) * rate;

        if (type == 1 && per == 1) {
            ipmt = 0;
        }

        return Operand.Create(ipmt);
    }

    calculatePMT(rate, nper, pv, fv, type) {
        let factor = Math.pow((1 + rate), nper);
        let pmt = -(pv * factor + fv) * rate / (factor - 1);
        if (type == 1) {
            pmt = pmt / (1 + rate);
        }
        return pmt;
    }
}

export { Function_IPMT };