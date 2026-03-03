import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_DELTA extends Function_N {
    get Name() {
        return 'Delta';
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        if (this.z.length < 1) {
            return this.functionError();
        }

        let args1 = this.getNumber(engine, tempParameter, 0);
        if (args1.IsError) { return args1; }
        let num1 = args1.NumberValue;

        let num2 = 0;
        if (this.z.length >= 2) {
            let args2 = this.getNumber(engine, tempParameter, 1);
            if (args2.IsError) { return args2; }
            num2 = args2.NumberValue;
        }

        return Operand.Create(num1 === num2 ? 1 : 0);
    }
}

export { Function_DELTA };
