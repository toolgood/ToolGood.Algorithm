import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_POWER extends Function_2 {
    get Name() {
        return "Power";
    }

    constructor(z) {
    super(z);
  }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsError) { return args2; }

        let number = args1.NumberValue;
        let power = args2.NumberValue;

        return Operand.Create(Math.pow(number, power));
    }
}

export { Function_POWER };

