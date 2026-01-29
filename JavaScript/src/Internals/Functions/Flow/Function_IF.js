import { Function_3 } from '../Function_3.js';

class Function_IF extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.func1.evaluate(engine, tempParameter);
        if (args1.IsNotBoolean) {
            args1 = args1.ToBoolean("Function '{0}' parameter {1} is error!", "If", 1);
            if (args1.IsError) { return args1; }
        }
        if (args1.BooleanValue) {
            return this.func2.evaluate(engine, tempParameter);
        }
        if (this.func3 === null) {
            return engine.createBooleanOperand(false);
        }
        return this.func3.evaluate(engine, tempParameter);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "IF");
    }
}

export { Function_IF };
