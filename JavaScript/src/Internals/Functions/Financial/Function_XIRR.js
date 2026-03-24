import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { MyDate } from '../../MyDate.js';

class Function_XIRR extends Function_3 {
    get Name() {
        return "XIRR";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        let valuesArg = this.getArray_1(engine, tempParameter);
        if (valuesArg.IsError) return valuesArg;
        let values = [];
        for (let v of valuesArg.ArrayValue) {
            values.push(v.NumberValue);
        }

        let datesArg = this.getArray_2(engine, tempParameter);
        if (datesArg.IsError) return datesArg;
        let dates = [];
        for (let d of datesArg.ArrayValue) {
            if (d.IsDate) {
                dates.push(d.DateValue.toDate());
            } else if (d.IsText) {
                let myDate = MyDate.parse(d.TextValue);
                if (myDate == null) return this.parameterError(2);
                dates.push(myDate.toDate());
            } else {
                return this.parameterError(2);
            }
        }

        if (values.length != dates.length || values.length < 2) return this.functionError();

        let guess = 0.1;
        if (this.c != null) {
            let guessArg = this.getNumber_3(engine, tempParameter);
            if (guessArg.IsError) return guessArg;
            guess = guessArg.NumberValue;
        }

        try {
            let xirr = this.newtonRaphsonXIRR(values, dates, guess);
            return Operand.Create(xirr);
        } catch (e) {
            return this.functionError();
        }
    }

    newtonRaphsonXIRR(values, dates, rate) {
        let baseDate = dates[0];

        for (let iter = 0; iter < 100; iter++) {
            let npv = 0;
            let dnpv = 0;

            for (let i = 0; i < values.length; i++) {
                let days = (dates[i] - baseDate) / (1000 * 60 * 60 * 24);
                let exp = days / 365.0;
                let factor = Math.pow(1 + rate, exp);
                npv += values[i] / factor;
                dnpv -= values[i] * exp / (factor * (1 + rate));
            }

            if (Math.abs(dnpv) < 1e-12) break;
            let newRate = rate - npv / dnpv;

            if (Math.abs(newRate - rate) < 1e-10) {
                return newRate;
            }
            rate = newRate;
        }
        throw new Error("XIRR calculation did not converge");
    }
}

export { Function_XIRR };