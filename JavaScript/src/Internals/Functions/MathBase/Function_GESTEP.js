import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_GESTEP extends Function_2 {
    get Name() {
        return "GeStep";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let number = args1.NumberValue;

        let step = 0;
        if (this.b != null) {
            let args2 = this.getNumber_2(engine, tempParameter);
            if (args2.IsError) { return args2; }
            step = args2.NumberValue;
        }

        return Operand.Create(number >= step ? 1 : 0);
    }
}

export { Function_GESTEP };