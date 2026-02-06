import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_CEILING extends Function_2 {
    get Name() {
        return "Ceiling";
    }

    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.GetNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        if (this.b === null || this.b === undefined) {
            return Operand.Create(Math.ceil(args1.NumberValue));
        }

        let args2 = this.GetNumber_2(engine, tempParameter);
        if (args2.IsError) { return args2; }
        let b = args2.NumberValue;
        if (b == 0) {
            return Operand.Zero;
        }
        if (b < 0) {
            return this.ParameterError(2);
        }

        let a = args1.NumberValue;
        let d = Math.ceil(a / b) * b;
        return Operand.Create(d);
    }
}

export { Function_CEILING };

