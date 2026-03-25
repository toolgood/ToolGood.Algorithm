import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_INT extends Function_1 {
    get Name() {
        return "Int";
    }

    constructor(a) {
        super(a);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let n = args1.NumberValue;
        return Operand.Create(Math.trunc(n));
    }
}

export { Function_INT };