import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_ISNULL extends Function_2 {
    get Name() {
        return "IsNull";
    }

    constructor(z) {
    super(z);
  }

    evaluate(engine, tempParameter) {
        let args1 = this.a.evaluate(engine, tempParameter);
        if (this.b != null) {
            if (args1.IsNull) { return this.b.evaluate(engine, tempParameter); }
            if (args1.IsText && args1.TextValue == null) { return this.b.evaluate(engine, tempParameter); }
            return args1;
        }
        if (args1.IsNull) { return Operand.True; }
        if (args1.IsText && args1.TextValue == null) { return Operand.True; }
        return Operand.False;
    }
}

export { Function_ISNULL };

