import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_TRUNC extends Function_2 {
    get Name() {
        return "Trunc";
    }

    constructor(z) {
        super(z);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let digits = 0;
        if (this.b != null) {
            let args2 = this.getNumber_2(engine, tempParameter);
            if (args2.IsError) { return args2; }
            digits = args2.IntValue;
        }

        if (args1.NumberValue == 0) {
            return args1;
        }

        let num = args1.NumberValue;
        let factor = Math.pow(10, digits);

        let result;
        if (num > 0) {
            result = Math.floor(num * factor) / factor;
        } else {
            result = Math.ceil(num * factor) / factor;
        }
        return Operand.Create(result);
    }
}

export { Function_TRUNC };