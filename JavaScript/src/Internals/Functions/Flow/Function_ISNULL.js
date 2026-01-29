import { Function_2 } from '../Function_2.js';

class Function_ISNULL extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    evaluate(engine, tempParameter) {
        const args1 = this.func1.evaluate(engine, tempParameter);
        if (this.func2 !== null) {
            if (args1.IsNull) {
                return this.func2.evaluate(engine, tempParameter);
            }
            if (args1.IsText && args1.TextValue === null) {
                return this.func2.evaluate(engine, tempParameter);
            }
            return args1;
        }
        if (args1.IsNull) {
            return engine.createBooleanOperand(true);
        }
        if (args1.IsText && args1.TextValue === null) {
            return engine.createBooleanOperand(true);
        }
        return engine.createBooleanOperand(false);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "IsNull");
    }
}

export { Function_ISNULL };
