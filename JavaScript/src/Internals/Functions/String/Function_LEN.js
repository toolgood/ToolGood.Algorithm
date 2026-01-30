import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_LEN extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText('Function {0} parameter is error!', 'Len');
            if (args1.IsError) {
                return args1;
            }
        }
        return Operand.Create(args1.TextValue.length);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'Len');
    }
}

export { Function_LEN };
