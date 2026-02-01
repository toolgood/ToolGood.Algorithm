import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_PARAM extends Function_2 {
    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText();
            if (args1.IsError) {
                return args1;
            }
        }
        if (tempParameter !== null) {
            let r = tempParameter(engine, args1.TextValue);
            if (r !== null) {
                return r;
            }
        }
        let result = engine.GetParameter(args1.TextValue);
        if (result.IsError) {
            if (this.b !== null) {
                return this.b.Evaluate(engine, tempParameter);
            }
        }
        return result;
    }
 
}

export { Function_PARAM };
