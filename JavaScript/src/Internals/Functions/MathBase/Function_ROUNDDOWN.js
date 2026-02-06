import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_ROUNDDOWN extends Function_2 {
    get Name() {
        return "RoundDown";
    }

    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.GetNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let args2 = this.GetNumber_2(engine, tempParameter);
        if (args2.IsError) { return args2; }
        if (args1.NumberValue == 0.0) {
            return args1;
        }
        let a = Math.pow(10, args2.IntValue);
        let b = args1.NumberValue;

        let result = b >= 0 ? Math.floor(b * a) / a : Math.ceil(b * a) / a;
        return Operand.Create(result);
    }
}

export { Function_ROUNDDOWN };

