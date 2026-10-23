import { Function_2 } from '../Function_2.js';
import { MyDate } from '../../MyDate.js';

import { Operand } from '../../../Operand.js';

class Function_ADDSECONDS extends Function_2 {
    get Name() {
        return "AddSeconds";
    }

    constructor(z) {
    super(z);
  }

    evaluate(engine, tempParameter) {
        let args1 = this.getDate_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsError) { return args2; }
        let date = new Date(args1.DateValue.ToDateTime().getTime());
        date.setSeconds(date.getSeconds() + args2.IntValue);
        return Operand.Create(new MyDate(date));
    }
}

export { Function_ADDSECONDS };

