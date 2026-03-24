import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_IRR extends Function_2 {
    get Name() {
        return "IRR";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        let valuesArg = this.getArray_1(engine, tempParameter);
        if (valuesArg.IsError) return valuesArg;
        let values = [];
        for (let v of valuesArg.ArrayValue) {
            if (v.IsNumber) {
                values.push(v.NumberValue);
            } else {
                let v2 = v.toNumber();
                if (v2.IsError) return v2;
                values.push(v2.NumberValue);
            }
        }

        if (values.length == 0) {
            return this.parameterError(1);
        }

        let hasPositive = false;
        let hasNegative = false;
        for (let v of values) {
            if (v > 0) hasPositive = true;
            if (v < 0) hasNegative = true;
        }
        if (!hasPositive || !hasNegative) {
            return this.parameterError(1);
        }

        let guess = 0.1;
        if (this.b != null) {
            let guessArg = this.getNumber_2(engine, tempParameter);
            if (guessArg.IsError) return guessArg;
            guess = guessArg.NumberValue;
        }

        try {
            let irr = this.newtonRaphsonIRR(values, guess);
            return Operand.Create(irr);
        } catch (e) {
            return this.functionError();
        }
    }

    newtonRaphsonIRR(values, guess) {
        let rate = guess;
        for (let iter = 0; iter < 100; iter++) {
            let npv = 0;
            let dnpv = 0;

            for (let i = 0; i < values.length; i++) {
                let factor = Math.pow((1 + rate), i);
                npv += values[i] / factor;
                dnpv -= i * values[i] / (factor * (1 + rate));
            }

            if (Math.abs(dnpv) < 1e-12) break;
            let newRate = rate - npv / dnpv;

            if (Math.abs(newRate - rate) < 1e-10) {
                return newRate;
            }
            rate = newRate;
        }
        throw new Error("IRR calculation did not converge");
    }
}

export { Function_IRR };