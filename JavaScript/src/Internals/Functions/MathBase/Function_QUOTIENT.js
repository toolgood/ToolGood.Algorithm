import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_QUOTIENT extends Function_2 {
    get Name() {
        return "Quotient";
    }

    constructor(z) {
    super(z);
  }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsError) { return args2; }

        if (args2.NumberValue == 0) {
            return this.div0Error();
        }
        return Operand.Create(Math.trunc(args1.NumberValue / args2.NumberValue));
    }
}

export { Function_QUOTIENT };

