import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';


class Function_URLDECODE extends Function_1 {
    get Name() {
        return "UrlDecode";
    }

    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.GetText_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let s = args1.TextValue;
        let r = decodeURIComponent(s)
            .replace(/\+/g, ' ');
        return Operand.Create(r);
    }
}

export { Function_URLDECODE };

