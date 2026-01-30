import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_URLDECODE extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText("Function '{0}' parameter is error!", "UrlDecode");
            if (args1.IsError) {
                return args1;
            }
        }
        let s = args1.TextValue;
        let r = decodeURIComponent(s)
            .replace(/\+/g, ' ');
        return Operand.Create(r);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "UrlDecode");
    }
}

export { Function_URLDECODE };
