import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_LOWER extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1.ToText('Function {0} parameter is error!', 'Lower');
            if (args1.IsError) {
                return args1;
            }
        }
        return Operand.Create(args1.TextValue.toLowerCase());
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'Lower');
    }
}

export { Function_LOWER };
