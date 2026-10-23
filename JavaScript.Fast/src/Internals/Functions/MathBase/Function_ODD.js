import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_ODD extends Function_1 {
    get Name() {
        return "Odd";
    }

    constructor(a) {
        super(a);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let z = args1.NumberValue;
        if (z == 0) {
            return Operand.Create(1);
        }

        if (z > 0) {
            if (z % 2 == 1 || z % 2 == -1) { return args1; }
            z = Math.ceil(z);
            if (z % 2 != 0) { return Operand.Create(z); }
            z++;
            return Operand.Create(z);
        } else {
            if (z % 2 == 1 || z % 2 == -1) { return args1; }
            z = Math.floor(z);
            if (z % 2 != 0) { return Operand.Create(z); }
            z--;
            return Operand.Create(z);
        }
    }
}

export { Function_ODD };

