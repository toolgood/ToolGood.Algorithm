import { FunctionBase } from '../FunctionBase.js';

class Function_PARAMETER extends FunctionBase {
    constructor(name, a) {
        super();
        if (typeof name === 'string') {
            this.name = name;
        } else {
            this.a = name;
        }
    }

    Evaluate(engine, tempParameter) {
        let txt = this.name;
        if (txt === undefined || txt === null) {
            let args1 = this.a.Evaluate(engine, tempParameter);
                args1 = args1.ToText();
                if (args1.IsError) {
                    return args1;
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

}

export { Function_PARAMETER };
