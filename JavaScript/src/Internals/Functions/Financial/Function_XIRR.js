import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { MyDate } from '../../MyDate.js';

class Function_XIRR extends Function_N {
    get Name() {
        return "XIRR";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        if (this.z.length < 2) return this.parameterError(1);

        const valuesArg = this.getArray(engine, tempParameter, 0);
        if (valuesArg.IsError) return valuesArg;
        const values = [];
        for (const v of valuesArg.ArrayValue) {
            values.push(v.NumberValue);
        }

        const datesArg = this.getArray(engine, tempParameter, 1);
        if (datesArg.IsError) return datesArg;
        const dates = [];
        for (const d of datesArg.ArrayValue) {
            if (d.IsDate) {
                dates.push(d.DateValue.ToDateTime());
            } else if (d.IsText) {
                const myDate = MyDate.Parse(d.TextValue);
                if (myDate == null) return this.functionError();
                dates.push(myDate.ToDateTime());
            } else {
                return this.functionError();
            }
        }

        if (values.length !== dates.length || values.length < 2) return this.functionError();

        let guess = 0.1;
        if (this.z.length > 2) {
            const guessArg = this.getNumber(engine, tempParameter, 2);
            if (guessArg.IsError) return guessArg;
            guess = guessArg.NumberValue;
        }

        const xirr = this.newtonRaphsonXIRR(values, dates, guess);
        return Operand.Create(xirr);
    }

    newtonRaphsonXIRR(values, dates, guess) {
        let rate = guess;
        const baseDate = dates[0];

        for (let iter = 0; iter < 100; iter++) {
            let npv = 0;
            let dnpv = 0;

            for (let i = 0; i < values.length; i++) {
                const days = (dates[i] - baseDate) / (1000 * 60 * 60 * 24);
                const exp = days / 365;
                const factor = Math.pow(1 + rate, exp);
                npv += values[i] / factor;
                dnpv -= values[i] * exp / (factor * (1 + rate));
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

export { Function_XIRR };
