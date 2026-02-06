import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_BASE64TOTEXT extends Function_1 {
    get Name() {
        return "Base64ToText";
    }

    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.GetText_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        try {
            let buffer = Buffer.from(args1.TextValue, 'base64');
            let t = buffer.toString('utf-8');
            return Operand.Create(t);
        } catch (e) {
            return this.FunctionError();
        }
    }
}

export { Function_BASE64TOTEXT };

