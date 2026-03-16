import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_ROUND extends Function_2 {
    get Name() {
        return "Round";
    }

    constructor(z) {
    super(z);
  }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        if (this.b === null) {
            return Operand.Create(Math.round(args1.NumberValue));
        }
        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsError) { return args2; }
        let decimalPlaces = args2.IntValue;
        let factor = Math.pow(10, decimalPlaces);
        return Operand.Create(Math.round(args1.NumberValue * factor) / factor);
    }
}

export { Function_ROUND };

