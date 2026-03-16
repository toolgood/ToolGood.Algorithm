import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';


class Function_BASE64TOTEXT extends Function_1 {
    get Name() {
        return "Base64ToText";
    }

    constructor(a) {
        super(a);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getText_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        try {
            let buffer = Buffer.from(args1.TextValue, 'base64');
            let t = buffer.toString('utf-8');
            return Operand.Create(t);
        } catch (e) {
            return this.functionError();
        }
    }
}

export { Function_BASE64TOTEXT };

