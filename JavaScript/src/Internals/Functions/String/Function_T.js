import { Function_1 } from '../Function_1';
import { Operand } from '../../Operand';

class Function_T extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    evaluate(engine, tempParameter) {
        const args1 = this.func1.evaluate(engine, tempParameter);
        if (args1.isText) {
            return args1;
        }
        return Operand.create('');
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'T');
    }
}

export { Function_T };
