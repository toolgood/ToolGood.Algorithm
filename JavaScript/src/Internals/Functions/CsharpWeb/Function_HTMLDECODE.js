import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';


class Function_HTMLDECODE extends Function_1 {
    get Name() {
        return "HtmlDecode";
    }

    constructor(a) {
        super(a);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getText_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let s = args1.TextValue;
        let r = Function_HTMLDECODE.HtmlDecode(s);
        return Operand.Create(r);
    }

    static HtmlDecode(input) {
        if (input == null) return '';
        const entities = {
            'amp': '&', 'lt': '<', 'gt': '>', 'quot': '"', 'apos': "'",
            'nbsp': '\u00A0', 'iexcl': '\u00A1', 'cent': '\u00A2', 'pound': '\u00A3',
            'curren': '\u00A4', 'yen': '\u00A5', 'brvbar': '\u00A6', 'sect': '\u00A7',
            'uml': '\u00A8', 'copy': '\u00A9', 'ordf': '\u00AA', 'laquo': '\u00AB',
            'not': '\u00AC', 'shy': '\u00AD', 'reg': '\u00AE', 'macr': '\u00AF',
            'deg': '\u00B0', 'plusmn': '\u00B1', 'sup2': '\u00B2', 'sup3': '\u00B3',
            'acute': '\u00B4', 'micro': '\u00B5', 'para': '\u00B6', 'middot': '\u00B7',
            'cedil': '\u00B8', 'sup1': '\u00B9', 'ordm': '\u00BA', 'raquo': '\u00BB',
            'frac14': '\u00BC', 'frac12': '\u00BD', 'frac34': '\u00BE', 'iquest': '\u00BF',
            'times': '\u00D7', 'divide': '\u00F7', 'euro': '\u20AC', 'trade': '\u2122'
        };
        return input.toString().replace(/&(?:#\d+|#x[0-9a-fA-F]+|[a-zA-Z][a-zA-Z0-9]*);/g, function (match) {
            let body = match.slice(1, -1);
            if (body[0] === '#') {
                let code;
                if (body[1] === 'x' || body[1] === 'X') {
                    code = parseInt(body.slice(2), 16);
                } else {
                    code = parseInt(body.slice(1), 10);
                }
                if (isNaN(code)) { return match; }
                try {
                    return String.fromCodePoint(code);
                } catch (e) {
                    return match;
                }
            }
            let v = entities[body];
            return v !== undefined ? v : match;
        });
    }
}

export { Function_HTMLDECODE };

