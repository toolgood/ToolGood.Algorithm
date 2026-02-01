import { Function_2 } from '../Function_2.js';
import { MyDate } from '../../MyDate.js';
import { StringCache } from '../../../Internals/StringCache.js';
import { Operand } from '../../../Operand.js';

class Function_ADDSECONDS extends Function_2 {
    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            args1 = args1.ToMyDate(StringCache.Function_parameter_error, "AddSeconds", 1);
            if (args1.IsError) {
                return args1;
            }
        let args2 = this.b.Evaluate(engine, tempParameter);
            args2 = args2.ToNumber(StringCache.Function_parameter_error, "AddSeconds", 2);
            if (args2.IsError) {
                return args2;
            }
        let date = new Date(args1.DateValue.getTime());
        date.setSeconds(date.getSeconds() + args2.IntValue);
        return Operand.Create(new MyDate(date));
    }
}

export { Function_ADDSECONDS };

