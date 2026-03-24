import { Function_N } from '../Function_N.js';
import { MyDate } from '../../MyDate.js';
import { Operand } from '../../../Operand.js';


class Function_DATE extends Function_N {
    get Name() {
        return "Date";
    }

    constructor(z) {
        super(z);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber(engine, tempParameter, 0);
        if (args1.IsError) { return args1; }

        let args2 = this.getNumber(engine, tempParameter, 1);
        if (args2.IsError) { return args2; }

        let args3 = this.getNumber(engine, tempParameter, 2);
        if (args3.IsError) { return args3; }

        let year = args1.IntValue;
        let month = args2.IntValue;
        let day = args3.IntValue;

        let baseDate;
        try {
            baseDate = new Date(year, 0, 1);
            baseDate.setMonth(baseDate.getMonth() + month - 1);
            baseDate.setDate(baseDate.getDate() + day - 1);
        } catch {
            return this.parameterError(1);
        }

        let hour = 0, minute = 0, second = 0;
        if (this.z.length > 3) {
            let args4 = this.getNumber(engine, tempParameter, 3);
            if (args4.IsError) { return args4; }
            hour = args4.IntValue;
            if (hour < 0 || hour > 23) {
                return this.parameterError(4);
            }
        }
        if (this.z.length > 4) {
            let args5 = this.getNumber(engine, tempParameter, 4);
            if (args5.IsError) { return args5; }
            minute = args5.IntValue;
            if (minute < 0 || minute > 59) {
                return this.parameterError(5);
            }
        }
        if (this.z.length > 5) {
            let args6 = this.getNumber(engine, tempParameter, 5);
            if (args6.IsError) { return args6; }
            second = args6.IntValue;
            if (second < 0 || second > 59) {
                return this.parameterError(6);
            }
        }

        let d = new MyDate(baseDate.getFullYear(), baseDate.getMonth() + 1, baseDate.getDate(), hour, minute, second);
        return Operand.Create(d);
    }
}

export { Function_DATE };

