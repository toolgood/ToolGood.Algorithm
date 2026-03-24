import { Function_N } from '../Function_N.js';
import { MyDate } from '../../MyDate.js';
import { Operand } from '../../../Operand.js';


class Function_WORKDAY extends Function_N {
    get Name() {
        return "Workday";
    }

    constructor(z) {
        super(z);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getDate(engine, tempParameter, 0);
        if (args1.IsError) { return args1; }

        let args2 = this.getNumber(engine, tempParameter, 1);
        if (args2.IsError) { return args2; }

        let startMyDate = new Date(args1.DateValue.ToDateTime().getTime());
        let days = args2.IntValue;
        let list = new Set();
        for (let i = 2; i < this.z.length; i++) {
            let ar = this.getDate(engine, tempParameter, i);
            if (ar.IsError) { return ar; }
            let dateStr = ar.DateValue.ToDateTime().toISOString().split('T')[0];
            list.add(dateStr);
        }

        if (days > 0) {
            while (days > 0) {
                startMyDate.setDate(startMyDate.getDate() + 1);
                let dayOfWeek = startMyDate.getDay();
                if (dayOfWeek === 6) continue;
                if (dayOfWeek === 0) continue;
                let currentDateStr = startMyDate.toISOString().split('T')[0];
                if (list.has(currentDateStr)) continue;
                days--;
            }
        } else if (days < 0) {
            while (days < 0) {
                startMyDate.setDate(startMyDate.getDate() - 1);
                let dayOfWeek = startMyDate.getDay();
                if (dayOfWeek === 6) continue;
                if (dayOfWeek === 0) continue;
                let currentDateStr = startMyDate.toISOString().split('T')[0];
                if (list.has(currentDateStr)) continue;
                days++;
            }
        }
        return Operand.Create(new MyDate(startMyDate));
    }
}

export { Function_WORKDAY };

