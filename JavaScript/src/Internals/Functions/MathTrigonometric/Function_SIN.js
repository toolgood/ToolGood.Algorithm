import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_SIN extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(work, tempParameter) {
        const args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.isNotNumber) {
            args1.toNumber('Function \'{0}\' parameter is error!', 'Sin');
            if (args1.isError) {
                return args1;
            }
        }
        return Operand.create(Math.sin(args1.doubleValue));
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Sin');
    }
}

export { Function_SIN };
