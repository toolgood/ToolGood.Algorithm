import { Function_2 } from '../Function_2.js';
import { MyDate } from '../../MyDate.js';
import { Operand } from '../../../Operand.js';


class Function_EOMONTH extends Function_2 {
    get Name() {
        return "EOMonth";
    }

    constructor(z) {
    super(z);
  }

    evaluate(engine, tempParameter) {
        let args1 = this.getDate_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsError) { return args2; }
        let dt = new Date(args1.DateValue.ToDateTime().getTime());
        dt.setDate(1); // 先归位到当月 1 号，避免 setMonth 溢出
        dt.setMonth(dt.getMonth() + args2.IntValue + 1);
        dt.setDate(0); // 设置为当月的最后一天
        return Operand.Create(new MyDate(dt));
    }
}

export { Function_EOMONTH };

