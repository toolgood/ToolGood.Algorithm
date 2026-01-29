import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_PROPER extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.isNotText) {
            args1.toText('Function {0} parameter is error!', 'Proper');
            if (args1.isError) {
                return args1;
            }
        }

        const text = args1.textValue;
        if (text.length === 0) {
            return Operand.create(text);
        }
        let needModify = false;
        let isFirst = true;
        for (let i = 0; i < text.length; i++) {
            const t = text[i];
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
        const chars = text.split('');
        isFirst = true;
        for (let i = 0; i < chars.length; i++) {
            const t = chars[i];
            if (t === ' ' || t === '\r' || t === '\n' || t === '\t' || t === '.') {
                isFirst = true;
            } else if (isFirst) {
                chars[i] = t.toUpperCase();
                isFirst = false;
            }
        }
        return Operand.create(chars.join(''));
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Proper');
    }
}

export { Function_PROPER };
