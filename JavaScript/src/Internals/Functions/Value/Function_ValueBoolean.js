import { Function_0 } from '../Function_0.js';
import { Operand } from '../../../Operand.js';

class Function_ValueBoolean extends Function_0 {
    constructor(value) {
        super();
        this._value = value;
    }

    get Name() {
        return this._value ? "True" : "False";
    }

    evaluate(engine, tempParameter = null) {
        return this._value ? Operand.True : Operand.False;
    }

    toString2(stringBuilder, addBrackets) {
        stringBuilder.push(this.Name);
    }
}

export { Function_ValueBoolean };