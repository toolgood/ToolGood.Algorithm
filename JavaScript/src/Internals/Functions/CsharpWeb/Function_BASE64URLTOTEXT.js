import { Function_2 } from '../Function_2.js';
import { Base64 } from './Base64.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';
import * as iconv from 'iconv-lite';

class Function_BASE64URLTOTEXT extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText(StringCache.Function_parameter_error2, "Base64UrlToText", 1);
            if (args1.IsError) return args1;
        }
        try {
            let encoding;
            if (this.func2 === null) {
                encoding = 'utf-8';
            } else {
                let args2 = this.func2.Evaluate(engine, tempParameter);
                if (args2.IsNotText) {
                    args2 = args2.ToText(StringCache.Function_parameter_error2, "Base64UrlToText", 2);
                    if (args2.IsError) return args2;
                }
                encoding = args2.TextValue;
            }
            let base64Url = args1.TextValue
                .replace(/-/g, '+')
                .replace(/_/g, '/');
            let padLength = base64Url.length % 4;
            if (padLength > 0) {
                base64Url += '='.repeat(4 - padLength);
            }
            let buffer = Buffer.from(base64Url, 'base64');
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
        return Operand.Error("Function 'Base64UrlToText' is error!");
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Base64urlToText");
    }
}

export { Function_BASE64URLTOTEXT };
