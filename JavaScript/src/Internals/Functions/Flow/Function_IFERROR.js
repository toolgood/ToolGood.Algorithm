import { Function_3 } from '../Function_3.js';

class Function_IFERROR extends Function_3 {
    get Name() {
        return "IfError";
    }

    constructor(z) {
    super(z);
  }

    evaluate(engine, tempParameter) {
        let args1 = this.a.evaluate(engine, tempParameter);
        if (args1.IsError) { return this.b.evaluate(engine, tempParameter); }
        if (this.c === null) { return args1; }
        return this.c.evaluate(engine, tempParameter);
    }
}

export { Function_IFERROR };

