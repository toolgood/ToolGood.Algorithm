import { Function_2 } from '../Function_2.js';
import { MyDate } from '../../MyDate.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_ADDDAYS extends Function_2 {
    get Name() {
        return "AddDays";
    }

    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.GetDate_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let args2 = this.GetNumber_2(engine, tempParameter);
        if (args2.IsError) { return args2; }
        let date = new Date(args1.DateValue.ToDateTime().getTime());
        date.setDate(date.getDate() + args2.NumberValue);
        return Operand.Create(new MyDate(date));
    }
}

export { Function_ADDDAYS };

