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

        return Operand.Create(Math.random() * (args2.NumberValue - args1.NumberValue) + args1.NumberValue);
    }
}

export { Function_RANDBETWEEN };

