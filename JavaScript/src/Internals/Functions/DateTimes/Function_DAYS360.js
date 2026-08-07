import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';


class Function_DAYS360 extends Function_3 {
    get Name() {
        return "Days360";
    }

    constructor(z) {
    super(z);
  }

    evaluate(engine, tempParameter) {
        let args1 = this.getDate_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let args2 = this.getDate_2(engine, tempParameter);
        if (args2.IsError) { return args2; }

        let startMyDate = args1.DateValue;  // MyDate对象
        let endMyDate = args2.DateValue;    // MyDate对象
        let startDate = startMyDate.ToDateTime();  // Date对象
        let endDate = endMyDate.ToDateTime();      // Date对象

        let method = false;
        if (this.c !== null) {
            let args3 = this.getBoolean_3(engine, tempParameter);
            if (args3.IsError) { return args3; }
            method = args3.BooleanValue;
        }

        let days = endDate.getFullYear() * 360 + (endDate.getMonth()) * 30
                    - startDate.getFullYear() * 360 - (startDate.getMonth()) * 30;

        if (method) {
            if (endDate.getDate() == 31) days += 30;
            if (startDate.getDate() == 31) days -= 30;
        } else {
            // US (NASD) 方法: start 若为月末(含 2 月最后一天)则调整为 30 日,
            // end 若为月末, 依据调整后的 startDay 决定按 31 日(下月1日)或 30 日计算
            let startDay = startDate.getDate();
            let endDay = endDate.getDate();
            if (startDate.getMonth() == 11) {
                if (startDay == new Date(startDate.getFullYear() + 1, 0, 0).getDate()) {
                    startDay = 30;
                }
            } else {
                if (startDay == new Date(startDate.getFullYear(), startDate.getMonth() + 1, 0).getDate()) {
                    startDay = 30;
                }
            }
            if (endDate.getMonth() == 11) {
                if (endDay == new Date(endDate.getFullYear() + 1, 0, 0).getDate()) {
                    endDay = startDay < 30 ? 31 : 30;
                }
            } else {
                if (endDay == new Date(endDate.getFullYear(), endDate.getMonth() + 1, 0).getDate()) {
                    endDay = startDay < 30 ? 31 : 30;
                }
            }
            days += endDay - startDay;
        }
        return Operand.Create(days);
    }
}

export { Function_DAYS360 };

