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

        let n = args1.NumberValue;
        let k = args2.NumberValue;

        if (n < 0 || k < 0) {
            return this.parameterError(1);
        }
        if (n < k) {
            return this.parameterError(2);
        }

        if (k == 0) {
            return Operand.Create(1);
        }

        let result = 1;
        for (let i = 0; i < k; i++) {
            result *= (n - i);
        }

        return Operand.Create(result);
    }
}

export { Function_PERMUT };

