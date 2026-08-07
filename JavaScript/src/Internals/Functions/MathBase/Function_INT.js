import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_INT extends Function_1 {
    get Name() {
        return "INT";
    }

    constructor(a) {
        super(a);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        // Excel INT 语义:向下取整(Floor)
        return Operand.Create(Math.floor(args1.NumberValue));
    }
}

export { Function_INT };
