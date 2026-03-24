import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_ACOT extends Function_1 {
    get Name() {
        return "Acot";
    }

    constructor(func1) {
        super(func1);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        return Operand.Create(Math.PI / 2 - Math.atan(args1.NumberValue));
    }
}

export { Function_ACOT };