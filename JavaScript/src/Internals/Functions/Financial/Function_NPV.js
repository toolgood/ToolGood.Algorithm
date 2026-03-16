import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_NPV extends Function_N {
    get Name() {
        return "NPV";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        if (this.z.length < 2) return this.parameterError(1);

        const rateArg = this.getNumber(engine, tempParameter, 0);
        if (rateArg.IsError) return rateArg;
        const rate = rateArg.NumberValue;

        const values = [];
        for (let i = 1; i < this.z.length; i++) {
            const arg = this.getNumber(engine, tempParameter, i);
            if (arg.IsError) return arg;
            values.push(arg.NumberValue);
        }

        let npv = 0;
        for (let i = 0; i < values.length; i++) {
            npv += values[i] / Math.pow(1 + rate, i + 1);
        }

        return Operand.Create(npv);
    }
}

export { Function_NPV };
