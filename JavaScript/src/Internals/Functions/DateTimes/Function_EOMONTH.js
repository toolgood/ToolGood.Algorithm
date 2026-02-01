import { Function_2 } from '../Function_2.js';
import { MyDate } from '../../MyDate.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_EOMONTH extends Function_2 {
    constructor(funcs) {
    super(funcs);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotDate) {
            args1 = args1.ToMyDate(StringCache.Function_parameter_error, "EoMonth", 1);
            if (args1.IsError) { return args1; }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber(StringCache.Function_parameter_error, "EoMonth", 2);
            if (args2.IsError) { return args2; }
        }
        let dt = new Date(args1.DateValue.ToDateTime().getTime());
        dt.setMonth(dt.getMonth() + args2.IntValue + 1);
        dt.setDate(0); // 设置为当月的最后一�?
        return Operand.Create(new MyDate(dt));
    }
}

export { Function_EOMONTH };

