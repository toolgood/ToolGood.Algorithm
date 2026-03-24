import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_MROUND extends Function_2 {
    get Name() {
        return "Mround";
    }

    constructor(z) {
    super(z);
  }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsError) { return args2; }

        let multiple = args2.NumberValue;
        if (multiple == 0) {
            return this.parameterError(2);
        }

        let number = args1.NumberValue;

        if ((number > 0 && multiple < 0) || (number < 0 && multiple > 0)) {
            return this.parameterError(2);
        }

        let r = Math.round(number / multiple) * multiple;
        return Operand.Create(r);
    }
}

export { Function_MROUND };

