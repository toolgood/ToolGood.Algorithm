import { FunctionBase } from '../FunctionBase.js';

class Function_PARAMETER extends FunctionBase {
    constructor(name, func1) {
        super();
        if (typeof name === 'string') {
            this.name = name;
        } else {
            this.func1 = name;
        }
    }

    Evaluate(engine, tempParameter) {
        let txt = this.name;
        if (txt === undefined || txt === null) {
            let args1 = this.func1.Evaluate(engine, tempParameter);
            if (args1.IsNotText) {
                args1 = args1.ToText();
                if (args1.IsError) {
                    return args1;
                }
            }
            txt = args1.TextValue;
        }
        if (tempParameter &&tempParameter !== null) {
            let r = tempParameter(engine, txt);
            if (r !== null) {
                return r;
            }
        }
        return engine.GetParameter(txt);
    }

    toString(stringBuilder, addBrackets) {
        if (this.name !== undefined && this.name !== null) {
            stringBuilder.append(this.name);
        } else {
            this.func1.toString(stringBuilder, false);
        }
    }
}

export { Function_PARAMETER };
