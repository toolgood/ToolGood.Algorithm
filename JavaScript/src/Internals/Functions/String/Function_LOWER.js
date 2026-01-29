import { Function_1 } from '../Function_1';
import { Operand } from '../../Operand';

class Function_LOWER extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    evaluate(engine, tempParameter) {
        const args1 = this.func1.evaluate(engine, tempParameter);
        if (args1.isNotText) {
            args1.toText('Function {0} parameter is error!', 'Lower');
            if (args1.isError) {
                return args1;
            }
        }
        return Operand.create(args1.textValue.toLowerCase());
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Lower');
    }
}

export { Function_LOWER };
