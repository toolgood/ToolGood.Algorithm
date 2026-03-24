import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_PERMUT extends Function_2 {
    get Name() {
        return "Permut";
    }

    constructor(z) {
    super(z);
  }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsError) { return args2; }

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

        if (count == 0) {
            return Operand.Create(1);
        }

        let sum = 1;
        for (let i = 0; i < count; i++) {
            sum *= (total - i);
        }
        return Operand.Create(sum);
    }
}

export { Function_PERMUT };

