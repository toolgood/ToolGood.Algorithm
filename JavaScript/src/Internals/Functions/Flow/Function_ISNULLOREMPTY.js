import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_ISNULLOREMPTY extends Function_1 {
    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
        if (args1.IsNull) {
            return Operand.Create(true);
        }
        let textArgs;
        if (args1.IsNotText) {
            textArgs = args1.ToText(StringCache.Function_parameter_error, "IsNullOrEmpty", 1);
            if (textArgs.IsError) { return textArgs; }
        } else {
            textArgs = args1;
        }
        return Operand.Create(textArgs.TextValue === null || textArgs.TextValue === "");
    }
}

export { Function_ISNULLOREMPTY };

