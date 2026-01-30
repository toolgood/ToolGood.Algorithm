import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

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
            return Operand.Create(true);
        }
        if (args1.IsText && args1.TextValue === null) {
            return Operand.Create(true);
        }
        return Operand.Create(false);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "IsNullOrError");
    }
}

export { Function_ISNULLORERROR };
