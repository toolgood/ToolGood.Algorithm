import { Function_2 } from '../Function_2.js';

class Function_ISERROR extends Function_2 {
    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
        if (this.b !== null) {
            if (args1.IsError) {
                return this.b.Evaluate(engine, tempParameter);
            }
            return args1;
        }
        if (args1.IsError) {
            return Operand.Create(true);
        }
        return Operand.Create(false);
    }
}

export { Function_ISERROR };

