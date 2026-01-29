import { Function_1 } from '../Function_1';

class Function_ISLOGICAL extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    evaluate(engine, tempParameter) {
        const args1 = this._arg1.evaluate(engine, tempParameter);
        if (args1.IsBoolean) {
            return engine.createBooleanOperand(true);
        }
        return engine.createBooleanOperand(false);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "IsLogical");
    }
}

export { Function_ISLOGICAL };
