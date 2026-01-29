import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_OCT2DEC extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(work, tempParameter) {
        const args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.IsNotText) {
            args1.ToText('Function \'{0}\' parameter is error!', 'OCT2DEC');
            if (args1.IsError) {
                return args1;
            }
        }

        if (!RegexHelper.OctRegex.test(args1.TextValue)) {
            return Operand.error('Function \'{0}\' parameter is error!', 'OCT2DEC');
        }
        const num = parseInt(args1.TextValue, 8);
        return Operand.Create(num);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'OCT2DEC');
    }
}

const RegexHelper = {
    OctRegex: /^[0-7]+$/
};

export { Function_OCT2DEC };
