import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_RANDBETWEEN extends Function_2 {
    get Name() {
        return "RandBetween";
    }

    constructor(z) {
    super(z);
  }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsError) { return args2; }

        let bottom = args1.NumberValue;
        let top = args2.NumberValue;
        if (bottom > top) {
            return this.parameterError(1);
        }

        return Operand.Create(Math.floor(Math.random() * (top - bottom + 1) + bottom));
    }
}

export { Function_RANDBETWEEN };

