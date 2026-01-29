import { Function_1 } from '../Function_1';

class Function_NOT extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    evaluate(engine, tempParameter) {
        let args1 = this._arg1.evaluate(engine, tempParameter);
        if (args1.IsNotBoolean) {
            args1 = args1.ToBoolean("Function '{0}' parameter is error!", "Not");
            if (args1.IsError) { return args1; }
        }
        return engine.createBooleanOperand(!args1.BooleanValue);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Not");
    }
}

export { Function_NOT };
