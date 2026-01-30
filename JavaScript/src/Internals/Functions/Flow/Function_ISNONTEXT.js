import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_ISNONTEXT extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            return Operand.Create(true);
        }
        return Operand.Create(false);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "IsNontext");
    }
}

export { Function_ISNONTEXT };
