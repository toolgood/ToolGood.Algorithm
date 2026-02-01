import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_MID extends Function_3 {
    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText(StringCache.Function_parameter_error, 'Mid', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.b.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber(StringCache.Function_parameter_error, 'Mid', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        let args3 = this.c.Evaluate(engine, tempParameter);
        if (args3.IsNotNumber) {
            args3 = args3.ToNumber(StringCache.Function_parameter_error, 'Mid', 3);
            if (args3.IsError) {
                return args3;
            }
        }
        let startIndex = args2.IntValue - engine.ExcelIndex;
        let length = args3.IntValue;
        return Operand.Create(args1.TextValue.substring(startIndex, startIndex + length));
    }
}

export { Function_MID };

