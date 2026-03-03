import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_IRR extends Function_N {
    get Name() {
        return "IRR";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        if (this.z.length < 1) return this.parameterError(1);

        const valuesArg = this.getArray(engine, tempParameter, 0);
        if (valuesArg.IsError) return valuesArg;
        const values = [];
        for (const v of valuesArg.ArrayValue) {
            values.push(v.NumberValue);
        }

        let guess = 0.1;
        if (this.z.length > 1) {
            const guessArg = this.getNumber(engine, tempParameter, 1);
            if (guessArg.IsError) return guessArg;
            guess = guessArg.NumberValue;
        }

        const irr = this.newtonRaphsonIRR(values, guess);
        return Operand.Create(irr);
    }

    newtonRaphsonIRR(values, guess) {
        let rate = guess;
        for (let iter = 0; iter < 100; iter++) {
            let npv = 0;
            let dnpv = 0;

            for (let i = 0; i < values.length; i++) {
                const factor = Math.pow(1 + rate, i);
                npv += values[i] / factor;
                dnpv -= i * values[i] / (factor * (1 + rate));
            }

            if (Math.abs(dnpv) < 1e-12) break;
            const newRate = rate - npv / dnpv;

            if (Math.abs(newRate - rate) < 1e-10) {
                return newRate;
            }
            rate = newRate;
        }
        return rate;
    }
}

export { Function_IRR };
