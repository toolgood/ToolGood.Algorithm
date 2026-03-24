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

        let hour = args1.IntValue;
        let minute = args2.IntValue;
        if (hour < 0 || hour > 23) {
            return this.parameterError(1);
        }
        if (minute < 0 || minute > 59) {
            return this.parameterError(2);
        }

        let d;
        if (this.c !== null) {
            let args3 = this.getNumber_3(engine, tempParameter);
            if (args3.IsError) { return args3; }
            let second = args3.IntValue;
            if (second < 0 || second > 59) {
                return this.parameterError(3);
            }
            d = new MyDate(0, 0, 0, hour, minute, second);
        } else {
            d = new MyDate(0, 0, 0, hour, minute, 0);
        }
        return Operand.Create(d);
    }
}

export { Function_TIME };

