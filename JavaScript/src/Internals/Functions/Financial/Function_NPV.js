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
        let rateArg = this.getNumber(engine, tempParameter, 0);
        if (rateArg.IsError) return rateArg;
        let rate = rateArg.NumberValue;
        if (rate == -1) {
            return this.div0Error();
        }

        let values = [];
        for (let i = 1; i < this.z.length; i++) {
            let arg = this.getNumber(engine, tempParameter, i);
            if (arg.IsError) return arg;
            values.push(arg.NumberValue);
        }

        let npv = 0;
        for (let i = 0; i < values.length; i++) {
            npv += values[i] / Math.pow((1 + rate), i + 1);
        }

        return Operand.Create(npv);
    }
}

export { Function_NPV };