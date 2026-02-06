import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';


class Function_TEXTTOBASE64URL extends Function_1 {
    get Name() {
        return "TextToBase64Url";
    }

    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.GetText_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        try {
            let buffer = Buffer.from(args1.TextValue, 'utf-8');
            let t = buffer.toString('base64')
                .replace(/=/g, '')
                .replace(/\+/g, '-')
                .replace(/\//g, '_');
            return Operand.Create(t);
        } catch (e) {
            return this.FunctionError();
        }
    }
}

export { Function_TEXTTOBASE64URL };

