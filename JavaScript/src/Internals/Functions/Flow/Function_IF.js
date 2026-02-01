import { Function_3 } from '../Function_3.js';
import { StringCache } from '../../../Internals/StringCache.js';
import { Operand } from '../../../Operand.js';

class Function_IF extends Function_3 {
    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            args1 = args1.ToBoolean(StringCache.Function_parameter_error, "If", 1);
            if (args1.IsError) { return args1; }
        if (args1.BooleanValue) {
            return this.b.Evaluate(engine, tempParameter);
        }
        if (this.c === null) {
            return Operand.Create(false);
        }
        return this.c.Evaluate(engine, tempParameter);
    }
}

export { Function_IF };

