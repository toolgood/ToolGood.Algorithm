import { Function_2 } from '../Function_2.js';
import { MyDate } from '../../MyDate.js';
import { Operand } from '../../../Operand.js';


class Function_ADDYEARS extends Function_2 {
    get Name() {
        return "AddYears";
    }

    constructor(z) {
    super(z);
  }

    evaluate(engine, tempParameter) {
        let args1 = this.getDate_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsError) { return args2; }
        let startDate = args1.DateValue.ToDateTime();
        // 与 C# AddYears 一致：闰日 2-29 在平年保留日号会超限，取月末（2-28）
        let date = new Date(startDate.getFullYear() + args2.IntValue, startDate.getMonth(), 1);
        let daysInMonth = new Date(date.getFullYear(), date.getMonth() + 1, 0).getDate();
        let day = Math.min(startDate.getDate(), daysInMonth);
        date.setDate(day);
        return Operand.Create(new MyDate(date));
    }
}

export { Function_ADDYEARS };

