import { Function_0 } from '../Function_0.js';
import { Operand } from '../../../Operand.js';

class Function_ValueNumber extends Function_0 {
    constructor(value, showName) {
        super();
        this._value = value;
        this._showName = showName;
    }

    get Name() {
        return this._showName;
    }

    evaluate(engine, tempParameter = null) {
        return this._value;
    }

    toString2(stringBuilder, addBrackets) {
        stringBuilder.push(this._showName);
    }
}

export { Function_ValueNumber };