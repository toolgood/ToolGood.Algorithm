import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_GESTEP extends Function_N {
    get Name() {
        return 'GeStep';
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber(engine, tempParameter, 0);
        if (args1.IsError) { return args1; }
        let number = args1.NumberValue;

        let step = 0;
        if (this.z.length >= 2) {
            let args2 = this.getNumber(engine, tempParameter, 1);
            if (args2.IsError) { return args2; }
            step = args2.NumberValue;
        }

        return Operand.Create(number >= step ? 1 : 0);
    }
}

export { Function_GESTEP };
