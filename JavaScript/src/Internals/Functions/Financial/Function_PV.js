import { Function_5 } from '../Function_5.js';
import { Operand } from '../../../Operand.js';

class Function_PV extends Function_5 {
    get Name() {
        return "PV";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        let rateArg = this.getNumber_1(engine, tempParameter);
        if (rateArg.IsError) return rateArg;
        let rate = rateArg.NumberValue;

        let nperArg = this.getNumber_2(engine, tempParameter);
        if (nperArg.IsError) return nperArg;
        let nper = nperArg.NumberValue;

        let pmtArg = this.getNumber_3(engine, tempParameter);
        if (pmtArg.IsError) return pmtArg;
        let pmt = pmtArg.NumberValue;

        let fv = 0;
        if (this.func4 != null) {
            let fvArg = this.getNumber_4(engine, tempParameter);
            if (fvArg.IsError) return fvArg;
            fv = fvArg.NumberValue;
        }

        let type = 0;
        if (this.func5 != null) {
            let typeArg = this.getNumber_5(engine, tempParameter);
            if (typeArg.IsError) return typeArg;
            type = typeArg.IntValue;
            if (type != 0 && type != 1) {
                return this.parameterError(5);
            }
        }

        if (rate == 0) {
            return Operand.Create(-pmt * nper - fv);
        }

        let factor = Math.pow((1 + rate), nper);
        let pv = -(fv + pmt * (factor - 1) / rate) / factor;
        if (type == 1) {
            pv = pv - pmt / (1 + rate) * ((factor - 1) / rate) / factor * (1 + rate);
            pv = -(fv + pmt * (1 + rate) * (factor - 1) / rate) / factor;
        }

        return Operand.Create(pv);
    }
}

export { Function_PV };