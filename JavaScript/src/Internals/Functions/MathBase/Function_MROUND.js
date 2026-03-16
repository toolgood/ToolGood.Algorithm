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
        let a = args2.NumberValue;
        if (a <= 0) { return this.parameterError(2); }

        let b = args1.NumberValue;
        let r = Math.round(b / a) * a;
        return Operand.Create(r);
    }
}

export { Function_MROUND };

