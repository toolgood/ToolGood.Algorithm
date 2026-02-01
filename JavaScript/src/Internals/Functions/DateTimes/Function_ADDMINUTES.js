import { Function_2 } from '../Function_2.js';
import { MyDate } from '../../MyDate.js';
import { StringCache } from '../../../Internals/StringCache.js';
import { Operand } from '../../../Operand.js';

class Function_ADDMINUTES extends Function_2 {
    constructor(funcs) {
    super(funcs);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotDate) {
            args1 = args1.ToMyDate(StringCache.Function_parameter_error, "AddMinutes", 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber(StringCache.Function_parameter_error, "AddMinutes", 2);
            if (args2.IsError) {
                return args2;
            }
        }
        let date = new Date(args1.DateValue.getTime());
        date.setMinutes(date.getMinutes() + args2.IntValue);
        return Operand.Create(new MyDate(date));
    }
}

export { Function_ADDMINUTES };

