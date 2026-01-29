import { Function_1 } from '../Function_1';

class Function_ISODD extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    evaluate(engine, tempParameter) {
        const args1 = this._arg1.evaluate(engine, tempParameter);
        if (args1.IsNumber) {
            if (args1.IntValue % 2 == 1) {
                return engine.createBooleanOperand(true);
            }
        }
        return engine.createBooleanOperand(false);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "IsOdd");
    }
}

export { Function_ISODD };
