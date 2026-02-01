import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_HTMLDECODE extends Function_1 {
    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            args1 = args1.ToText(StringCache.Function_parameter_error, "HtmlDecode");
            if (args1.IsError) {
                return args1;
            }
        let s = args1.TextValue;
        let r = Function_HTMLDECODE.HtmlDecode(s);
        return Operand.Create(r);
    }

    static HtmlDecode(input) {
        if (input == null) return '';
        return input.toString()
            .replace(/&amp;/g, '&')
            .replace(/&lt;/g, '<')
            .replace(/&gt;/g, '>')
            .replace(/&quot;/g, '"')
            .replace(/&#39;/g, "'")
            .replace(/&#([0-9]+);/g, function (match, dec) {
                return String.fromCharCode(dec);
            });
    }
}

export { Function_HTMLDECODE };

