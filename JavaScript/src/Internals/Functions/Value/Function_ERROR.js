import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_ERROR extends Function_1 {
    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText();
            if (args1.IsError) {
                return args1;
            }
        }
        return Operand.Error(args1.TextValue);
    }

}

export { Function_ERROR };
