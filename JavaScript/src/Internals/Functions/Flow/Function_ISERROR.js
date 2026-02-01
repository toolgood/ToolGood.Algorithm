import { Function_2 } from '../Function_2.js';

class Function_ISERROR extends Function_2 {
    constructor(funcs) {
    super(funcs);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (this.func2 !== null) {
            if (args1.IsError) {
                return this.func2.Evaluate(engine, tempParameter);
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

