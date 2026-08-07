import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';


class Function_URLDECODE extends Function_1 {
    get Name() {
        return "UrlDecode";
    }

    constructor(a) {
        super(a);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getText_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let s = args1.TextValue;
        let s2 = s.replace(/\+/g, ' ');
        try {
            let r = decodeURIComponent(s2);
            return Operand.Create(r);
        } catch (e) {
            // C# HttpUtility.UrlDecode 对无效编码不抛异常，原样返回
            return Operand.Create(s2);
        }
    }
}

export { Function_URLDECODE };

