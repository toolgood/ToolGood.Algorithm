import { FunctionBase } from '../FunctionBase.js';

class Function_Value extends FunctionBase {
    constructor(value, showName) {
        super();
        this._value = value;
        this._showName = showName;
    }

    Evaluate(engine, tempParameter) {
        return this._value;
    }
 
}

export { Function_Value };
