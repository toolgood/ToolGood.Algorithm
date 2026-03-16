import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';


class Function_TEXTTOBASE64 extends Function_1 {
    get Name() {
        return "TextToBase64";
    }

    constructor(a) {
        super(a);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getText_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        try {
            let buffer = Buffer.from(args1.TextValue, 'utf-8');
            let t = buffer.toString('base64');
            return Operand.Create(t);
        } catch (e) {
            return this.functionError();
        }
    }
}

export { Function_TEXTTOBASE64 };

