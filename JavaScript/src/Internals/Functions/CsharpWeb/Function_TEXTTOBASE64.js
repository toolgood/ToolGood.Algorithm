import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import * as iconv from 'iconv-lite';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_TEXTTOBASE64 extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText(StringCache.Function_parameter_error2, "TextToBase64", 1);
            if (args1.IsError) return args1;
        }
        try {
            let encoding;
            if (this.func2 === null) {
                encoding = 'utf-8';
            } else {
                let args2 = this.func2.Evaluate(engine, tempParameter);
                if (args2.IsNotText) {
                    args2 = args2.ToText(StringCache.Function_parameter_error2, "TextToBase64", 2);
                    if (args2.IsError) return args2;
                }
                encoding = args2.TextValue;
            }
            // 使用iconv-lite处理不同编码
            let buffer;
            if (iconv.encodingExists(encoding)) {
                buffer = iconv.encode(args1.TextValue, encoding);
            } else {
                // 如果编码不支持，默认使用utf-8
                buffer = Buffer.from(args1.TextValue, 'utf-8');
            }
            let t = buffer.toString('base64');
            return Operand.Create(t);
        } catch (e) {
            // Ignore errors
        }
        return Operand.Error(StringCache.Function_error1, "TextToBase64");
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "TextToBase64");
    }
}

export { Function_TEXTTOBASE64 };
