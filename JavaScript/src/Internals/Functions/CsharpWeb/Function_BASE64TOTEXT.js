import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';
import * as iconv from 'iconv-lite';

class Function_BASE64TOTEXT extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText(StringCache.Function_parameter_error, "Base64ToText", 1);
            if (args1.IsError) return args1;
        }
        try {
            let encoding;
            if (this.func2 === null) {
                encoding = 'utf-8';
            } else {
                let args2 = this.func2.Evaluate(engine, tempParameter);
                if (args2.IsNotText) {
                    args2 = args2.ToText(StringCache.Function_parameter_error, "Base64ToText", 2);
                    if (args2.IsError) return args2;
                }
                encoding = args2.TextValue;
            }
            let buffer = Buffer.from(args1.TextValue, 'base64');
            let t;
            if (iconv.encodingExists(encoding)) {
                t = iconv.decode(buffer, encoding);
            } else {
                // 如果编码不支持，默认使用utf-8
                t = buffer.toString('utf-8');
            }
            return Operand.Create(t);
        } catch (e) {
            // Ignore errors
        }
        return Operand.Error(StringCache.Function_error, "Base64ToText");
    }
}

export { Function_BASE64TOTEXT };

