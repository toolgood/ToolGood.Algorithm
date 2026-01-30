import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_HTMLENCODE extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText("Function {0} parameter is error!", "HtmlEncode");
            if (args1.IsError) {
                return args1;
            }
        }
        let s = args1.TextValue;
        let r = Function_HTMLENCODE.HtmlEncode(s);
        return Operand.Create(r);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "HtmlEncode");
    }

    static HtmlEncode(input) {
        if (input == null) return '';
        return input.toString().replace(/[&<>'"]/g, function (match) {
            switch (match) {
                case '&': return '&amp;';
                case '<': return '&lt;';
                case '>': return '&gt;';
                case '"': return '&quot;';
                case "'": return '&#39;';
                default: return match;
            }
        });
    }
}

export { Function_HTMLENCODE };
