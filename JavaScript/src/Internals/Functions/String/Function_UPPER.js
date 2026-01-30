import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_UPPER extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText('Function {0} parameter is error!', 'Upper');
            if (args1.IsError) {
                return args1;
            }
        }
        return Operand.Create(args1.TextValue.toUpperCase());
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'Upper');
    }
}

export { Function_UPPER };
