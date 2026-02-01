import { Function_3 } from '../Function_3.js';

class Function_IFERROR extends Function_3 {
    constructor(funcs) {
    super(funcs);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsError) {
            return this.func2.Evaluate(engine, tempParameter);
        }
        return this.func3.Evaluate(engine, tempParameter);
    }
}

export { Function_IFERROR };

