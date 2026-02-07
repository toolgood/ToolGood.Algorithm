import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';


class Function_HTMLENCODE extends Function_1 {
    get Name() {
        return "HtmlEncode";
    }

    constructor(a) {
        super(a);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getText_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let s = args1.TextValue;
        let r = Function_HTMLENCODE.HtmlEncode(s);
        return Operand.Create(r);
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

