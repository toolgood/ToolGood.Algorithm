import { Function_2 } from '../Function_2';

class Function_ISERROR extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    evaluate(engine, tempParameter) {
        const args1 = this._arg1.evaluate(engine, tempParameter);
        if (this._arg2 !== null) {
            if (args1.IsError) {
                return this._arg2.evaluate(engine, tempParameter);
            }
            return args1;
        }
        if (args1.IsError) {
            return engine.createBooleanOperand(true);
        }
        return engine.createBooleanOperand(false);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "IsError");
    }
}

export { Function_ISERROR };
