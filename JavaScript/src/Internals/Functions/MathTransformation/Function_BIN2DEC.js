import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_BIN2DEC extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    evaluate(work, tempParameter) {
        const args1 = this.func1.evaluate(work, tempParameter);
        if (args1.isNotText) {
            args1.toText('Function \'{0}\' parameter is error!', 'BIN2DEC');
            if (args1.isError) {
                return args1;
            }
        }

        if (!RegexHelper.BinRegex.test(args1.textValue)) {
            return Operand.error('Function \'{0}\' parameter is error!', 'BIN2DEC');
        }
        const num = parseInt(args1.textValue, 2);
        return Operand.create(num);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'BIN2DEC');
    }
}

const RegexHelper = {
    BinRegex: /^[01]+$/
};

export { Function_BIN2DEC };
