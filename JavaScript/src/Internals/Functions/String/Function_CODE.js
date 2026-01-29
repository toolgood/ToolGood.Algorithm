import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_CODE extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(work, tempParameter) {
        const args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.isNotText) {
            args1.ToText('Function \'{0}\' parameter is error!', 'CODE');
            if (args1.isError) {
                return args1;
            }
        }
        if (!args1.textValue) {
            return Operand.error('Function \'{0}\' parameter is error!', 'CODE');
        }
        const c = args1.textValue[0];
        return Operand.Create(c.charCodeAt(0));
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'CODE');
    }
}

export { Function_CODE };
