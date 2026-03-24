import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

export class Function_SPLIT extends Function_2 {
    constructor(z) {
        super(z);
    }

    get Name() {
        return "Split";
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getText_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let args2 = this.getText_2(engine, tempParameter);
        if (args2.IsError) { return args2; }

        if (args2.TextValue.length === 0) {
            return this.parameterError(2);
        }

        return Operand.Create(args1.TextValue.split(args2.TextValue));
    }
}

