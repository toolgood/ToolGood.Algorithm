import { Function_1 } from '../Function_1.js';

class Function_ISEVEN extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    evaluate(engine, tempParameter) {
        const args1 = this.func1.evaluate(engine, tempParameter);
        if (args1.IsNumber) {
            if (args1.IntValue % 2 == 0) {
                return engine.createBooleanOperand(true);
            }
        }
        return engine.createBooleanOperand(false);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "IsEven");
    }
}

export { Function_ISEVEN };
