import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_FV extends Function_N {
    get Name() {
        return "FV";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        if (this.z.length < 3) return this.parameterError(1);

        const rateArg = this.getNumber(engine, tempParameter, 0);
        if (rateArg.IsError) return rateArg;
        const rate = rateArg.NumberValue;

        const nperArg = this.getNumber(engine, tempParameter, 1);
        if (nperArg.IsError) return nperArg;
        const nper = nperArg.NumberValue;

        const pmtArg = this.getNumber(engine, tempParameter, 2);
        if (pmtArg.IsError) return pmtArg;
        const pmt = pmtArg.NumberValue;

        let pv = 0;
        if (this.z.length > 3) {
            const pvArg = this.getNumber(engine, tempParameter, 3);
            if (pvArg.IsError) return pvArg;
            pv = pvArg.NumberValue;
        }

        let type = 0;
        if (this.z.length > 4) {
            const typeArg = this.getNumber(engine, tempParameter, 4);
            if (typeArg.IsError) return typeArg;
            type = Math.floor(typeArg.NumberValue);
        }

        if (rate === 0) {
            return Operand.Create(-pv - pmt * nper);
        }

        const factor = Math.pow(1 + rate, nper);
        let fv = -pv * factor - pmt * (factor - 1) / rate;
        if (type === 1) {
            fv = fv * (1 + rate);
        }

        return Operand.Create(fv);
    }
}

export { Function_FV };
