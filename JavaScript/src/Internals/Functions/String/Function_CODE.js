import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_CODE extends Function_1 {
    get Name() {
        return "CODE";
    }

    constructor(a) {
        super(a);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.GetText_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        if (!args1.TextValue) {
            return Operand.Error(this.FunctionError, 'CODE');
        }
        let c = args1.TextValue[0];
        return Operand.Create(c.charCodeAt(0));
    }
}

export { Function_CODE };

