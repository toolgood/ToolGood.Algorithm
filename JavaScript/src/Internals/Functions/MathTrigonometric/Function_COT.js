import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_COT extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(work, tempParameter) {
        const args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.isNotNumber) {
            args1.ToNumber('Function \'{0}\' parameter is error!', 'Cot');
            if (args1.isError) {
                return args1;
            }
        }
        const d = Math.tan(args1.doubleValue);
        if (d === 0) {
            return Operand.error('Function \'{0}\') div 0 error!', 'Cot');
        }
        return Operand.Create(1.0 / d);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Cot');
    }
}

export { Function_COT };
