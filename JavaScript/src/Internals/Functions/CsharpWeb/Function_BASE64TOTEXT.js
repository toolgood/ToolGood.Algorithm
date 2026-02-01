import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_BASE64TOTEXT extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText(StringCache.Function_parameter_error, "Base64ToText", 1);
            if (args1.IsError) return args1;
        }
        try {
            let buffer = Buffer.from(args1.TextValue, 'base64');
            let t = buffer.toString('utf-8');
            return Operand.Create(t);
        } catch (e) {
            // Ignore errors
        }
        return Operand.Error(StringCache.Function_error, "Base64ToText");
    }
}

export { Function_BASE64TOTEXT };

