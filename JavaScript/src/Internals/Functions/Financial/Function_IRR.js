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
            if (v.IsNumber) {
                values.push(v.NumberValue);
            } else {
                const v2 = v.ToNumber(`Function '${this.Name}' parameter 1 is error!`);
                if (v2.IsError) return v2;
                values.push(v2.NumberValue);
            }
        }

        if (values.length === 0) return this.parameterError(1);

        // IRR 要求现金流量必须同时包含正负值
        let hasPositive = false;
        let hasNegative = false;
        for (const v of values) {
            if (v > 0) hasPositive = true;
            if (v < 0) hasNegative = true;
        }
        if (!hasPositive || !hasNegative) return this.parameterError(1);

        let guess = 0.1;
        if (this.z.length > 1) {
            const guessArg = this.getNumber(engine, tempParameter, 1);
            if (guessArg.IsError) return guessArg;
            guess = guessArg.NumberValue;
        }

        const irr = this.newtonRaphsonIRR(values, guess);
        if (irr === null) return this.functionError();
        return Operand.Create(irr);
    }

    newtonRaphsonIRR(values, guess) {
        let rate = guess;
        for (let iter = 0; iter < 100; iter++) {
            let npv = 0;
            let dnpv = 0;
            const onePlusRate = 1 + rate;
            let factor = 1;

            for (let i = 0; i < values.length; i++) {
                npv += values[i] / factor;
                dnpv -= i * values[i] / (factor * onePlusRate);
                factor *= onePlusRate;
            }

            if (Math.abs(dnpv) < 1e-12) break;
            const newRate = rate - npv / dnpv;

            if (Math.abs(newRate - rate) < 1e-10) {
                return newRate;
            }
            rate = newRate;
        }
        return null; // 未收敛
    }
}

export { Function_IRR };
