import { Function_1 } from '../Function_1.js';

class Function_HTMLENCODE extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    evaluate(engine, tempParameter) {
        let args1 = this._arg1.evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText("Function '{0}' parameter is error!", "HtmlEncode");
            if (args1.IsError) {
                return args1;
            }
        }
        const s = args1.TextValue;
        const r = Function_HTMLENCODE.HtmlEncode(s);
        return engine.createOperand(r);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "HtmlEncode");
    }

    static HtmlEncode(input) {
        const temp = document.createElement('div');
        temp.textContent = input;
        return temp.innerHTML;
    }
}

export { Function_HTMLENCODE };
