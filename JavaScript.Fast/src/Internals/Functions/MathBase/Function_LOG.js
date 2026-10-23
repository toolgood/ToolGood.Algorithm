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

        let z = args1.NumberValue;
        if (z <= 0) {
            return this.parameterError(1);
        }

        if (this.b === null || this.b === undefined) {
            return Operand.Create(Math.log10(z));
        }

        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsError) { return args2; }
        let baseValue = args2.NumberValue;
        if (baseValue <= 0 || baseValue == 1) {
            return this.parameterError(2);
        }
        return Operand.Create(Math.log(z) / Math.log(baseValue));
    }
}

export { Function_LOG };

