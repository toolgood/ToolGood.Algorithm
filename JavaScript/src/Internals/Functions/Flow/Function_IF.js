import { Function_3 } from '../Function_3.js';

import { Operand } from '../../../Operand.js';

class Function_IF extends Function_3 {
    get Name() {
        return "If";
    }

    constructor(z) {
    super(z);
  }

    evaluate(engine, tempParameter) {
        let args1 = this.getBoolean_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        if (args1.BooleanValue) {
            return this.b.evaluate(engine, tempParameter);
        }
        if (this.c === null) {
            return Operand.False;
        }
        return this.c.evaluate(engine, tempParameter);
    }
}

export { Function_IF };

