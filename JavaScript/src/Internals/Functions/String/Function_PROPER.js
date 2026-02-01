import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_PROPER extends Function_1 {
    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            args1 = args1.ToText(StringCache.Function_parameter_1_error, 'Proper');
            if (args1.IsError) {
                return args1;
            }

        let text = args1.TextValue;
        if (text.length === 0) {
            return Operand.Create(text);
        }
        let needModify = false;
        let isFirst = true;
        for (let i = 0; i < text.length; i++) {
            let t = text[i];
            if (t === ' ' || t === '\r' || t === '\n' || t === '\t' || t === '.') {
                isFirst = true;
            } else if (isFirst) {
                if (t === t.toLowerCase() && t !== t.toUpperCase()) {
                    needModify = true;
                    break;
                }
                isFirst = false;
            }
        }
        if (!needModify) {
            return args1; // no change
        }
        let chars = text.split('');
        isFirst = true;
        for (let i = 0; i < chars.length; i++) {
            let t = chars[i];
            if (t === ' ' || t === '\r' || t === '\n' || t === '\t' || t === '.') {
                isFirst = true;
            } else if (isFirst) {
                chars[i] = t.toUpperCase();
                isFirst = false;
            }
        }
        return Operand.Create(chars.join(''));
    }
}

export { Function_PROPER };

