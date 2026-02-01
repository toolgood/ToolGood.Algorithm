import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_BASE64URLTOTEXT extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText(StringCache.Function_parameter_error, "Base64UrlToText", 1);
            if (args1.IsError) return args1;
        }
        try {
            let base64Url = args1.TextValue
                .replace(/-/g, '+')
                .replace(/_/g, '/');
            let padLength = base64Url.length % 4;
            if (padLength > 0) {
                base64Url += '='.repeat(4 - padLength);
            }
            let buffer = Buffer.from(base64Url, 'base64');
            let t = buffer.toString('utf-8');
            return Operand.Create(t);
        } catch (e) {
            // Ignore errors
        }
        return Operand.Error(StringCache.Function_error, "Base64UrlToText");
    }
}

export { Function_BASE64URLTOTEXT };

