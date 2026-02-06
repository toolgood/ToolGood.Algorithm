import { FunctionBase } from '../FunctionBase.js';

class Function_PARAMETER extends FunctionBase {
    get Name() {
        return "Parameter";
    }

    constructor(name, a) {
        super();
        if (typeof name === 'string') {
            this.name = name;
        } else {
            this.func1 = name;
        }
    }

    Evaluate(work, tempParameter) {
        let txt = this.name;
        if (txt === undefined || txt === null) {
            let args1 = this.func1.Evaluate(work, tempParameter);
                args1 = args1.ToText();
                if (args1.IsError) {
                    return args1;
                }
            txt = args1.TextValue;
        }
        if (tempParameter && tempParameter !== null) {
            let r = tempParameter(work, txt);
            if (r !== null) {
                return r;
            }
        }
        return work.GetParameter(txt);
    }

    ToString(stringBuilder, addBrackets) {
        if (this.name === undefined || this.name === null) {
            this.func1.ToString(stringBuilder, false);
        } else {
            stringBuilder.append(this.name);
        }
    }

}

export { Function_PARAMETER };
