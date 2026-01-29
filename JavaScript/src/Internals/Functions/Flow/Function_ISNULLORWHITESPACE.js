import { Function_1 } from '../Function_1.js';

class Function_ISNULLORWHITESPACE extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    evaluate(engine, tempParameter) {
        const args1 = this._arg1.evaluate(engine, tempParameter);
        if (args1.IsNull) {
            return engine.createBooleanOperand(true);
        }
        let textArgs;
        if (args1.IsNotText) {
            textArgs = args1.ToText("Function '{0}' parameter {1} is error!", "IsNullOrWhiteSpace", 1);
            if (textArgs.IsError) { return textArgs; }
        } else {
            textArgs = args1;
        }
        return engine.createBooleanOperand(textArgs.TextValue === null || textArgs.TextValue.trim() === "");
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "IsNullOrWhiteSpace");
    }
}

export { Function_ISNULLORWHITESPACE };
