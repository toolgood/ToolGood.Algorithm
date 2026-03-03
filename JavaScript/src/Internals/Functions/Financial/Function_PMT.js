import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_PMT extends Function_N {
    get Name() {
        return "PMT";
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

        const pvArg = this.getNumber(engine, tempParameter, 2);
        if (pvArg.IsError) return pvArg;
        const pv = pvArg.NumberValue;

        let fv = 0;
        if (this.z.length > 3) {
            const fvArg = this.getNumber(engine, tempParameter, 3);
            if (fvArg.IsError) return fvArg;
            fv = fvArg.NumberValue;
        }

        let type = 0;
        if (this.z.length > 4) {
            const typeArg = this.getNumber(engine, tempParameter, 4);
            if (typeArg.IsError) return typeArg;
            type = Math.floor(typeArg.NumberValue);
        }

        if (rate === 0) {
            return Operand.Create(-(pv + fv) / nper);
        }

        const factor = Math.pow(1 + rate, nper);
        let pmt = -(pv * factor + fv) * rate / (factor - 1);
        if (type === 1) {
            pmt = pmt / (1 + rate);
        }

        return Operand.Create(pmt);
    }
}

export { Function_PMT };
