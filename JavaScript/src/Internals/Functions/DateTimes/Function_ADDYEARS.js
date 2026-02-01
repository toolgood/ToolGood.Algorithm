import { Function_2 } from '../Function_2.js';
import { MyDate } from '../../MyDate.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_ADDYEARS extends Function_2 {
    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            args1 = args1.ToMyDate(StringCache.Function_parameter_error, "AddYears", 1);
            if (args1.IsError) {
                return args1;
            }
        let args2 = this.b.Evaluate(engine, tempParameter);
            args2 = args2.ToNumber(StringCache.Function_parameter_error, "AddYears", 2);
            if (args2.IsError) {
                return args2;
            }
        let date = new Date(args1.DateValue.ToDateTime().getTime());
        date.setFullYear(date.getFullYear() + args2.IntValue);
        return Operand.Create(new MyDate(date));
    }
}

export { Function_ADDYEARS };

