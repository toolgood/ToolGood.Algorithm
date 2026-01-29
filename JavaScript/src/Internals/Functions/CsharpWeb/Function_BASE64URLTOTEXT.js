import { Function_2 } from '../Function_2';
import { Base64 } from './Base64';

class Function_BASE64URLTOTEXT extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    evaluate(engine, tempParameter) {
        let args1 = this._arg1.evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Base64urlToText", 1);
            if (args1.IsError) return args1;
        }
        try {
            let encoding;
            if (this._arg2 === null) {
                encoding = 'utf-8';
            } else {
                let args2 = this._arg2.evaluate(engine, tempParameter);
                if (args2.IsNotText) {
                    args2 = args2.ToText("Function '{0}' parameter {1} is error!", "Base64urlToText", 2);
                    if (args2.IsError) return args2;
                }
                encoding = args2.TextValue;
            }
            const bytes = Base64.FromBase64ForUrlString(args1.TextValue);
            const decoder = new TextDecoder(encoding);
            const t = decoder.decode(bytes);
            return engine.createOperand(t);
        } catch (e) {
            // Ignore errors
        }
        return engine.createErrorOperand("Function 'Base64urlToText' is error!");
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Base64urlToText");
    }
}

export { Function_BASE64URLTOTEXT };
