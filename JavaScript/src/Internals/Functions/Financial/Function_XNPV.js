import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { MyDate } from '../../MyDate.js';

class Function_XNPV extends Function_3 {
    get Name() {
        return "XNPV";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        let rateArg = this.getNumber_1(engine, tempParameter);
        if (rateArg.IsError) return rateArg;
        let rate = rateArg.NumberValue;
        if (rate == -1) {
            return this.div0Error();
        }

        let valuesArg = this.getArray_2(engine, tempParameter);
        if (valuesArg.IsError) return valuesArg;
        let values = valuesArg.ArrayValue;

        let datesArg = this.getArray_3(engine, tempParameter);
        if (datesArg.IsError) return datesArg;
        let dates = datesArg.ArrayValue;

        if (values.length != dates.length) return this.functionError();
        if (values.length == 0) return this.parameterError(1);

        let dateList = [];
        for (let d of dates) {
            if (d.IsDate) {
                dateList.push(d.DateValue.toDate());
            } else if (d.IsText) {
                let myDate = MyDate.parse(d.TextValue);
                if (myDate == null) return this.parameterError(3);
                dateList.push(myDate.toDate());
            } else {
                return this.parameterError(3);
            }
        }

        let baseDate = dateList[0];
        let xnpv = 0;

        for (let i = 0; i < values.length; i++) {
            let days = (dateList[i] - baseDate) / (1000 * 60 * 60 * 24);
            xnpv += values[i].NumberValue / Math.pow((1 + rate), days / 365.0);
        }

        return Operand.Create(xnpv);
    }
}

export { Function_XNPV };