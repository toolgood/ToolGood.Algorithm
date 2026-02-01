import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_TEXTTOBASE64 extends Function_1 {
    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            args1 = args1.ToText(StringCache.Function_parameter_error, "TextToBase64", 1);
            if (args1.IsError) return args1;
        try {
            let buffer = Buffer.from(args1.TextValue, 'utf-8');
            let t = buffer.toString('base64');
            return Operand.Create(t);
        } catch (e) {
            // Ignore errors
        }
        return Operand.Error(StringCache.Function_error, "TextToBase64");
    }
}

export { Function_TEXTTOBASE64 };

