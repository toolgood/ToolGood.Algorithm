import { Function_1 } from '../Function_1';

class Function_ISNUMBER extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    evaluate(engine, tempParameter) {
        const args1 = this._arg1.evaluate(engine, tempParameter);
        if (args1.IsNumber) {
            return engine.createBooleanOperand(true);
        }
        return engine.createBooleanOperand(false);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "IsNumber");
    }
}

export { Function_ISNUMBER };
