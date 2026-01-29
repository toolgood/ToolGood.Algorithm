import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_PARAM extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1.ToText();
            if (args1.IsError) {
                return args1;
            }
        }
        if (tempParameter !== null) {
            let r = tempParameter(engine, args1.TextValue);
            if (r !== null) {
                return r;
            }
        }
        let result = engine.getParameter(args1.TextValue);
        if (result.IsError) {
            if (this.func2 !== null) {
                return this.func2.Evaluate(engine, tempParameter);
            }
        }
        return result;
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'Param');
    }
}

export { Function_PARAM };
