import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_ISNULLOREMPTY extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNull) {
            return Operand.Create(true);
        }
        let textArgs;
        if (args1.IsNotText) {
            textArgs = args1.ToText("Function {0} parameter {1} is error!", "IsNullOrEmpty", 1);
            if (textArgs.IsError) { return textArgs; }
        } else {
            textArgs = args1;
        }
        return Operand.Create(textArgs.TextValue === null || textArgs.TextValue === "");
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "IsNullOrEmpty");
    }
}

export { Function_ISNULLOREMPTY };
