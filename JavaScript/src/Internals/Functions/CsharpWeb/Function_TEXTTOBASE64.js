import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_TEXTTOBASE64 extends Function_1 {
    get Name() {
        return "TextToBase64";
    }

    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.GetText_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        try {
            let buffer = Buffer.from(args1.TextValue, 'utf-8');
            let t = buffer.toString('base64');
            return Operand.Create(t);
        } catch (e) {
            return this.FunctionError();
        }
    }
}

export { Function_TEXTTOBASE64 };

