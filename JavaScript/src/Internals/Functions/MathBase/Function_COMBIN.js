import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_COMBIN extends Function_2 {
    get Name() {
        return "Combin";
    }

    constructor(z) {
    super(z);
  }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        if (args1.IsNone) { return args1; }

        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsError) { return args2; }
        if (args2.IsNone) { return args2; }

        let total = args1.IntValue;
        let count = args2.IntValue;
        if (total < 0) {
            return this.parameterError(1);
        }
        if (count < 0) {
            return this.parameterError(2);
        }
        if (total < count) {
            return this.parameterError(2);
        }
        let sum = 1;
        let sum2 = 1;
        for (let i = 0; i < count; i++) {
            sum *= (total - i);
            sum2 *= (i + 1);
        }
        return Operand.Create(sum / sum2);
    }
}

export { Function_COMBIN };

