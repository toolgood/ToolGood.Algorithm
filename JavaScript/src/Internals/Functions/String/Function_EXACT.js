import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_EXACT extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(work, tempParameter) {
        const args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.isNotText) {
            args1.ToText('Function \'{0}\' parameter {1} is error!', 'EXACT', 1);
            if (args1.isError) {
                return args1;
            }
        }
        const args2 = this.func2.Evaluate(work, tempParameter);
        if (args2.isNotText) {
            args2.ToText('Function \'{0}\' parameter {1} is error!', 'EXACT', 2);
            if (args2.isError) {
                return args2;
            }
        }
        return Operand.Create(args1.textValue === args2.textValue);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Exact');
    }
}

export { Function_EXACT };
