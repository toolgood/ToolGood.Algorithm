import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_FIXED extends Function_3 {
    get Name() {
        return "Fixed";
    }

    constructor(z) {
    super(z);
  }

    evaluate(engine, tempParameter) {
        let num = 2;
        if (this.b !== null && this.b !== undefined) {
            let args2 = this.getNumber_2(engine, tempParameter);
            if (args2.IsError) { return args2; }
            num = args2.IntValue;
            if (num < 0 || num > 15) {
                return this.parameterError(2);
            }
        }

        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let s = Math.round(args1.NumberValue * Math.pow(10, num)) / Math.pow(10, num);
        let no = false;
        if (this.c !== null && this.c !== undefined) {
            let args3 = this.getBoolean_3(engine, tempParameter);
            if (args3.IsError) { return args3; }
            no = args3.BooleanValue;
        }
        if (no === false) {
            let formatted = s.toFixed(num);
            let parts = formatted.split('.');
            parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
            return Operand.Create(parts.join('.'));
        }
        return Operand.Create(s.toString());
    }
}

export { Function_FIXED };

