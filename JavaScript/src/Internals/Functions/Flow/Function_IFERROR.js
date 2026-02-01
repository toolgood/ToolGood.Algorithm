import { Function_3 } from '../Function_3.js';

class Function_IFERROR extends Function_3 {
    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
        if (args1.IsError) {
            return this.b.Evaluate(engine, tempParameter);
        }
        return this.c.Evaluate(engine, tempParameter);
    }
}

export { Function_IFERROR };

