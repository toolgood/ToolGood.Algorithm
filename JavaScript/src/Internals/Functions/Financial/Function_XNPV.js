import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { MyDate } from '../../MyDate.js';

class Function_XNPV extends Function_N {
    get Name() {
        return "XNPV";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        if (this.z.length < 3) return this.parameterError(1);

        const rateArg = this.getNumber(engine, tempParameter, 0);
        if (rateArg.IsError) return rateArg;
        const rate = rateArg.NumberValue;

        const valuesArg = this.getArray(engine, tempParameter, 1);
        if (valuesArg.IsError) return valuesArg;
        const values = valuesArg.ArrayValue;

        const datesArg = this.getArray(engine, tempParameter, 2);
        if (datesArg.IsError) return datesArg;
        const dates = datesArg.ArrayValue;

        if (values.length !== dates.length) return this.functionError();
        if (values.length === 0) return this.functionError();

        const dateList = [];
        for (const d of dates) {
            if (d.IsDate) {
                dateList.push(d.DateValue.ToDateTime());
            } else if (d.IsText) {
                const myDate = MyDate.Parse(d.TextValue);
                if (myDate == null) return this.functionError();
                dateList.push(myDate.ToDateTime());
            } else {
                return this.functionError();
            }
        }

        const baseDate = dateList[0];
        let xnpv = 0;

        for (let i = 0; i < values.length; i++) {
            const days = (dateList[i] - baseDate) / (1000 * 60 * 60 * 24);
            xnpv += values[i].NumberValue / Math.pow(1 + rate, days / 365);
        }

        return Operand.Create(xnpv);
    }
}

export { Function_XNPV };
