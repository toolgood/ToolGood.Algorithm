import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';


class Function_ISNULLORWHITESPACE extends Function_1 {
    get Name() {
        return "IsNullOrWhiteSpace";
    }

    constructor(a) {
        super(a);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.a.evaluate(engine, tempParameter);
        if (args1.IsNull) {
            return Operand.True;
        }
        let textArgs;
        if (args1.IsNotText) {
            textArgs = args1.ToText(StringCache.Function_parameter_error, "IsNullOrWhiteSpace", 1);
            if (textArgs.IsError) { return textArgs; }
        } else {
            textArgs = args1;
        }
        return textArgs.TextValue === null || textArgs.TextValue.trim() === "" ? Operand.True : Operand.False;
    }
}

export { Function_ISNULLORWHITESPACE };

