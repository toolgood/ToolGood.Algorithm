import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';


class Function_SECOND extends Function_1 {
    get Name() {
        return "Second";
    }

    constructor(a) {
        super(a);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getDate_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        return Operand.Create(args1.DateValue.Second);
    }
}

export { Function_SECOND };

