import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_SQRTPI extends Function_1 {
    get Name() {
        return "SqrtPi";
    }

    constructor(a) {
        super(a);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        return Operand.Create(Math.sqrt(args1.NumberValue * Math.PI));
    }
}

export { Function_SQRTPI };

