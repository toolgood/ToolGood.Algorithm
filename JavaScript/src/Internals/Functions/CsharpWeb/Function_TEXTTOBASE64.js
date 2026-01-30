import { Function_2 } from '../Function_2.js';
import { Base64 } from './Base64.js';
import { Operand } from '../../../Operand.js';

class Function_TEXTTOBASE64 extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "TextToBase64", 1);
            if (args1.IsError) return args1;
        }
        try {
            let encoding;
            if (this.func2 === null) {
                encoding = 'utf-8';
            } else {
                let args2 = this.func2.Evaluate(engine, tempParameter);
                if (args2.IsNotText) {
                    args2 = args2.ToText("Function '{0}' parameter {1} is error!", "TextToBase64", 2);
                    if (args2.IsError) return args2;
                }
                encoding = args2.TextValue;
            }
            let encoder = new TextEncoder(encoding);
            let bytes = encoder.encode(args1.TextValue);
            let t = Base64.ToBase64String(bytes);
            return Operand.Create(t);
        } catch (e) {
            // Ignore errors
        }
        return Operand.Error("Function 'TextToBase64' is error!");
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "TextToBase64");
    }
}

export { Function_TEXTTOBASE64 };
