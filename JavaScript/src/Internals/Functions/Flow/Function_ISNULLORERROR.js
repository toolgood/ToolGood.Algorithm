import { Function_2 } from '../Function_2.js';

class Function_ISNULLORERROR extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (this.func2 !== null) {
            if (args1.IsNull || args1.IsError) {
                return this.func2.Evaluate(engine, tempParameter);
            }
            if (args1.IsText && args1.TextValue === null) {
                return this.func2.Evaluate(engine, tempParameter);
            }
            return args1;
        }
        if (args1.IsNull || args1.IsError) {
            return engine.createBooleanOperand(true);
        }
        if (args1.IsText && args1.TextValue === null) {
            return engine.createBooleanOperand(true);
        }
        return engine.createBooleanOperand(false);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "IsNullOrError");
    }
}

export { Function_ISNULLORERROR };
