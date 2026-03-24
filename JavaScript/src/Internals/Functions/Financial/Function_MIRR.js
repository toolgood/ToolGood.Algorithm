import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_MIRR extends Function_3 {
    get Name() {
        return "MIRR";
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

        let financeRateArg = this.getNumber_2(engine, tempParameter);
        if (financeRateArg.IsError) return financeRateArg;
        let financeRate = financeRateArg.NumberValue;

        let reinvestRateArg = this.getNumber_3(engine, tempParameter);
        if (reinvestRateArg.IsError) return reinvestRateArg;
        let reinvestRate = reinvestRateArg.NumberValue;

        let npvNegative = 0;
        let npvPositive = 0;
        let n = values.length;

        if (n == 0) {
            return this.parameterError(1);
        }
        if (n == 1) {
            return this.div0Error();
        }

        for (let i = 0; i < n; i++) {
            if (values[i] < 0) {
                npvNegative += values[i] / Math.pow((1 + financeRate), i);
            } else {
                npvPositive += values[i] * Math.pow((1 + reinvestRate), n - 1 - i);
            }
        }

        if (npvNegative == 0) return this.div0Error();

        let mirr = Math.pow((-npvPositive / npvNegative), 1.0 / (n - 1)) - 1;
        return Operand.Create(mirr);
    }
}

export { Function_MIRR };