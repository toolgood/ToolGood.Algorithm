import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_ROUNDUP extends Function_2 {
    get Name() {
        return "RoundUp";
    }

    constructor(z) {
    super(z);
  }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsError) { return args2; }
        if (args1.NumberValue == 0.0) { return args1; }
        let a = Math.pow(10, args2.IntValue);
        let b = args1.NumberValue;

        let t = (Math.ceil(Math.abs(b) * a)) / a;
        if (b > 0) return Operand.Create(t);
        return Operand.Create(-t);
    }
}

export { Function_ROUNDUP };

