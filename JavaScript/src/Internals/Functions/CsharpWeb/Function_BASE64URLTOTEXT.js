import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_BASE64URLTOTEXT extends Function_1 {
    get Name() {
        return "Base64UrlToText";
    }

    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.GetText_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
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
            return this.FunctionError();
        }
    }
}

export { Function_BASE64URLTOTEXT };

