import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_RATE extends Function_N {
    get Name() {
        return "RATE";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        if (this.z.length < 3) return this.parameterError(1);

        const nperArg = this.getNumber(engine, tempParameter, 0);
        if (nperArg.IsError) return nperArg;
        const nper = nperArg.NumberValue;

        const pmtArg = this.getNumber(engine, tempParameter, 1);
        if (pmtArg.IsError) return pmtArg;
        const pmt = pmtArg.NumberValue;

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

        let guess = 0.1;
        if (this.z.length > 5) {
            const guessArg = this.getNumber(engine, tempParameter, 5);
            if (guessArg.IsError) return guessArg;
            guess = guessArg.NumberValue;
        }

        const rate = this.newtonRaphson(nper, pmt, pv, fv, type, guess);
        return Operand.Create(rate);
    }

    newtonRaphson(nper, pmt, pv, fv, type, guess) {
        let rate = guess;
        for (let iter = 0; iter < 100; iter++) {
            const factor = Math.pow(1 + rate, nper);
            let fn;
            if (type === 1) {
                fn = pv * factor + pmt * (1 + rate) * (factor - 1) / rate + fv;
            } else {
                fn = pv * factor + pmt * (factor - 1) / rate + fv;
            }

            let dfn;
            if (type === 1) {
                dfn = pv * nper * Math.pow(1 + rate, nper - 1) +
                    pmt * (1 + rate) * (nper * rate * Math.pow(1 + rate, nper - 1) - (factor - 1)) / (rate * rate) +
                    pmt * (factor - 1) / rate;
            } else {
                dfn = pv * nper * Math.pow(1 + rate, nper - 1) +
                    pmt * (nper * rate * Math.pow(1 + rate, nper - 1) - (factor - 1)) / (rate * rate);
            }

            if (Math.abs(dfn) < 1e-12) break;
            const newRate = rate - fn / dfn;

            if (Math.abs(newRate - rate) < 1e-10) {
                return newRate;
            }
            rate = newRate;
        }
        return rate;
    }
}

export { Function_RATE };
