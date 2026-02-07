import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_LOG extends Function_2 {
    get Name() {
        return "Log";
    }

    constructor(z) {
    super(z);
  }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        if (this.b === null || this.b === undefined) {
            return Operand.Create(Math.log10(args1.NumberValue));
        }

        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsError) { return args2; }
        return Operand.Create(Math.log(args1.NumberValue) / Math.log(args2.NumberValue));
    }
}

export { Function_LOG };

