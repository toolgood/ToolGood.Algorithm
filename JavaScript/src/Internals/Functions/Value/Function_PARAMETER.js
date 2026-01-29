import { FunctionBase } from '../FunctionBase';

class Function_PARAMETER extends FunctionBase {
    constructor(name, func1) {
        super();
        if (typeof name === 'string') {
            this.name = name;
        } else {
            this.func1 = name;
        }
    }

    evaluate(engine, tempParameter) {
        let txt = this.name;
        if (txt === undefined || txt === null) {
            const args1 = this.func1.evaluate(engine, tempParameter);
            if (args1.isNotText) {
                args1.toText();
                if (args1.isError) {
                    return args1;
                }
            }
            txt = args1.textValue;
        }
        if (tempParameter !== null) {
            const r = tempParameter(engine, txt);
            if (r !== null) {
                return r;
            }
        }
        return engine.getParameter(txt);
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
