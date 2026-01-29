import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_HEX2DEC extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(work, tempParameter) {
        const args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.IsNotText) {
            args1.ToText('Function \'{0}\' parameter is error!', 'HEX2DEC');
            if (args1.IsError) {
                return args1;
            }
        }

        if (!RegexHelper.HexRegex.test(args1.TextValue)) {
            return Operand.error('Function \'{0}\' parameter is error!', 'HEX2DEC');
        }
        const num = parseInt(args1.TextValue, 16);
        return Operand.Create(num);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'HEX2DEC');
    }
}

const RegexHelper = {
    HexRegex: /^[0-9A-Fa-f]+$/
};

export { Function_HEX2DEC };
