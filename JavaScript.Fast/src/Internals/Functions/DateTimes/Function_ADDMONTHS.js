import { Function_2 } from '../Function_2.js';
import { MyDate } from '../../MyDate.js';
import { Operand } from '../../../Operand.js';


class Function_ADDMONTHS extends Function_2 {
    get Name() {
        return "AddMonths";
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
        // 与 C# AddMonths 一致：先归位到 1 号再添加月份，末日后保留日号（超限取月末）
        let date = new Date(startDate.getFullYear(), startDate.getMonth(), 1);
        date.setMonth(date.getMonth() + args2.IntValue);
        let daysInMonth = new Date(date.getFullYear(), date.getMonth() + 1, 0).getDate();
        let day = Math.min(startDate.getDate(), daysInMonth);
        date.setDate(day);
        return Operand.Create(new MyDate(date));
    }
}

export { Function_ADDMONTHS };

