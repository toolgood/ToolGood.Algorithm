import { Function_3 } from '../Function_3.js';
import { MyDate } from '../../MyDate.js';

import { Operand } from '../../../Operand.js';


class Function_TIME extends Function_3 {
    get Name() {
        return "Time";
    }

    constructor(z) {
    super(z);
  }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsError) { return args2; }

        let d;
        if (this.c !== null) {
            let args3 = this.getNumber_3(engine, tempParameter);
            if (args3.IsError) { return args3; }
            d = new MyDate(0, 0, 0, args1.IntValue, args2.IntValue, args3.IntValue);
        } else {
            d = new MyDate(0, 0, 0, args1.IntValue, args2.IntValue, 0);
        }
        return Operand.Create(d);
    }
}

export { Function_TIME };

