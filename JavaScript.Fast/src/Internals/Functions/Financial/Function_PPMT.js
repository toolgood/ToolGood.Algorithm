import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_PPMT extends Function_N {
    get Name() {
        return "PPMT";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        if (this.z.length < 4) return this.parameterError(1);

        const rateArg = this.getNumber(engine, tempParameter, 0);
        if (rateArg.IsError) return rateArg;
        const rate = rateArg.NumberValue;

        const perArg = this.getNumber(engine, tempParameter, 1);
        if (perArg.IsError) return perArg;
        const per = perArg.NumberValue;

        const nperArg = this.getNumber(engine, tempParameter, 2);
        if (nperArg.IsError) return nperArg;
        const nper = nperArg.NumberValue;

        const pvArg = this.getNumber(engine, tempParameter, 3);
        if (pvArg.IsError) return pvArg;
        const pv = pvArg.NumberValue;

        let fv = 0;
        if (this.z.length > 4) {
            const fvArg = this.getNumber(engine, tempParameter, 4);
            if (fvArg.IsError) return fvArg;
            fv = fvArg.NumberValue;
        }

        let type = 0;
        if (this.z.length > 5) {
            const typeArg = this.getNumber(engine, tempParameter, 5);
            if (typeArg.IsError) return typeArg;
            type = typeArg.IntValue;
            if (type !== 0 && type !== 1) return this.parameterError(6);
        }

        if (per < 1 || per > nper) return this.parameterError(2);

        const pmt = this.calculatePMT(rate, nper, pv, fv, type);
        const ipmt = this.calculateIPMT(rate, per, nper, pv, fv, type, pmt);
        const ppmt = pmt - ipmt;

        return Operand.Create(ppmt);
    }

    calculatePMT(rate, nper, pv, fv, type) {
        if (rate === 0) {
            return -(pv + fv) / nper;
        }
        const factor = Math.pow(1 + rate, nper);
        let pmt = -(pv * factor + fv) * rate / (factor - 1);
        if (type === 1) {
            pmt = pmt / (1 + rate);
        }
        return pmt;
    }

    calculateIPMT(rate, per, nper, pv, fv, type, pmt) {
        if (rate === 0) {
            return 0;
        }
        const factorPer = Math.pow(1 + rate, per - 1);
        const ipmt = -(pv * factorPer + pmt * (factorPer - 1) / rate) * rate;
        if (type === 1 && per === 1) {
            return 0;
        }
        return ipmt;
    }
}

export { Function_PPMT };
