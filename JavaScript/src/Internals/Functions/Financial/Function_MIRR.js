import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_MIRR extends Function_N {
    get Name() {
        return "MIRR";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        if (this.z.length < 3) return this.parameterError(1);

        const valuesArg = this.getArray(engine, tempParameter, 0);
        if (valuesArg.IsError) return valuesArg;
        const values = [];
        for (const v of valuesArg.ArrayValue) {
            values.push(v.NumberValue);
        }

        const financeRateArg = this.getNumber(engine, tempParameter, 1);
        if (financeRateArg.IsError) return financeRateArg;
        const financeRate = financeRateArg.NumberValue;

        const reinvestRateArg = this.getNumber(engine, tempParameter, 2);
        if (reinvestRateArg.IsError) return reinvestRateArg;
        const reinvestRate = reinvestRateArg.NumberValue;

        let npvNegative = 0;
        let npvPositive = 0;
        const n = values.length;

        for (let i = 0; i < n; i++) {
            if (values[i] < 0) {
                npvNegative += values[i] / Math.pow(1 + financeRate, i);
            } else {
                npvPositive += values[i] * Math.pow(1 + reinvestRate, n - 1 - i);
            }
        }

        if (npvNegative === 0 || npvPositive === 0) {
            return this.functionError();
        }

        const mirr = Math.pow(-npvPositive / npvNegative, 1 / (n - 1)) - 1;
        return Operand.Create(mirr);
    }
}

export { Function_MIRR };
