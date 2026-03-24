import { Function_6 } from '../Function_6.js';
import { Operand } from '../../../Operand.js';

class Function_RATE extends Function_6 {
    get Name() {
        return "RATE";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        let nperArg = this.getNumber_1(engine, tempParameter);
        if (nperArg.IsError) return nperArg;
        let nper = nperArg.NumberValue;

        let pmtArg = this.getNumber_2(engine, tempParameter);
        if (pmtArg.IsError) return pmtArg;
        let pmt = pmtArg.NumberValue;

        let pvArg = this.getNumber_3(engine, tempParameter);
        if (pvArg.IsError) return pvArg;
        let pv = pvArg.NumberValue;

        if (nper <= 0) {
            return this.parameterError(1);
        }

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

        let guess = 0.1;
        if (this.func6 != null) {
            let guessArg = this.getNumber_6(engine, tempParameter);
            if (guessArg.IsError) return guessArg;
            guess = guessArg.NumberValue;
        }

        let rate = this.newtonRaphson(nper, pmt, pv, fv, type, guess);
        return Operand.Create(rate);
    }

    newtonRaphson(n, p, v, f, type, rate) {
        for (let i = 0; i < 100; i++) {
            let factor = Math.pow(1 + rate, n);
            let fn;

            if (type == 1) {
                fn = v * factor + p * (1 + rate) * (factor - 1) / rate + f;
            } else {
                fn = v * factor + p * (factor - 1) / rate + f;
            }

            let dfn;
            if (type == 1) {
                dfn = v * n * Math.pow(1 + rate, n - 1) +
                    p * (1 + rate) * (n * rate * Math.pow(1 + rate, n - 1) - (factor - 1)) / (rate * rate) +
                    p * (factor - 1) / rate;
            } else {
                dfn = v * n * Math.pow(1 + rate, n - 1) +
                    p * (n * rate * Math.pow(1 + rate, n - 1) - (factor - 1)) / (rate * rate);
            }

            if (Math.abs(dfn) < 1e-12) break;
            let newRate = rate - fn / dfn;

            if (Math.abs(newRate - rate) < 1e-10) {
                return newRate;
            }
            rate = newRate;
        }
        return rate;
    }
}

export { Function_RATE };