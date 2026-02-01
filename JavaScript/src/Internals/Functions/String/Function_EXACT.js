import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_EXACT extends Function_2 {
    constructor(z) {
    super(z);
  }

    Evaluate(work, tempParameter) {
        let args1 = this.a.Evaluate(work, tempParameter);
            args1 = args1.ToText(StringCache.Function_parameter_error, 'EXACT', 1);
            if (args1.IsError) {
                return args1;
            }
        let args2 = this.b.Evaluate(work, tempParameter);
            args2 = args2.ToText(StringCache.Function_parameter_error, 'EXACT', 2);
            if (args2.IsError) {
                return args2;
            }
        return Operand.Create(args1.TextValue === args2.TextValue);
    }
}

export { Function_EXACT };

