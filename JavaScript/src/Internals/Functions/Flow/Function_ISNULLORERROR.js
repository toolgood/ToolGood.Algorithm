import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_ISNULLORERROR extends Function_2 {
    get Name() {
        return "IsNullorError";
    }

    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
        if (this.b !== null) {
            if (args1.IsNull || args1.IsError) {
                return this.b.Evaluate(engine, tempParameter);
            }
            if (args1.IsText && args1.TextValue === null) {
                return this.b.Evaluate(engine, tempParameter);
            }
            return args1;
        }
        if (args1.IsNull || args1.IsError) {
            return Operand.True;
        }
        if (args1.IsText && args1.TextValue === null) {
            return Operand.True;
        }
        return Operand.False;
    }
}

export { Function_ISNULLORERROR };

