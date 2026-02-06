import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';


class Function_URLENCODE extends Function_1 {
    get Name() {
        return "UrlEncode";
    }

    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.GetText_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let s = args1.TextValue;
        let r = encodeURIComponent(s)
            .replace(/%20/g, '+')
            .toLowerCase();
        return Operand.Create(r);
    }
}

export { Function_URLENCODE };

