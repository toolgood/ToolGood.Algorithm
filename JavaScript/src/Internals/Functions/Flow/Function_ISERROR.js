import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_ISERROR extends Function_2 {
    get Name() {
        return "IsError";
    }

    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
        if (this.b != null) {
            if (args1.IsError) { return this.b.Evaluate(engine, tempParameter); }
            return args1;
        }
        if (args1.IsError) { return Operand.True; }
        return Operand.False;
    }
}

export { Function_ISERROR };

