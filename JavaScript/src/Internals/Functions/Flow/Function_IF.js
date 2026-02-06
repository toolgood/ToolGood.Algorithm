import { Function_3 } from '../Function_3.js';
import { StringCache } from '../../../Internals/StringCache.js';
import { Operand } from '../../../Operand.js';

class Function_IF extends Function_3 {
    get Name() {
        return "If";
    }

    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.GetBoolean_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        if (args1.BooleanValue) {
            return this.b.Evaluate(engine, tempParameter);
        }
        if (this.c === null) {
            return Operand.False;
        }
        return this.c.Evaluate(engine, tempParameter);
    }
}

export { Function_IF };

