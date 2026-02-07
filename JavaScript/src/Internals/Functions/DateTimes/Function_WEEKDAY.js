import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';


class Function_WEEKDAY extends Function_2 {
    get Name() {
        return "Weekday";
    }

    constructor(z) {
    super(z);
  }

    evaluate(engine, tempParameter) {
        let args1 = this.getDate_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let type = 1;
        if (this.b != null) {
            let args2 = this.getNumber_2(engine, tempParameter);
            if (args2.IsError) { return args2; }
            type = args2.IntValue;
        }

        let t = args1.DateValue.ToDateTime().getDay(); // JavaScript中，0表示星期日，6表示星期六
        if (type == 1) {
            // 类型1：返回1-7，1表示星期日，7表示星期六
            return Operand.Create(t + 1);
        } else if (type == 2) {
            // 类型2：返回1-7，1表示星期一，7表示星期日
            if (t == 0) return Operand.Create(7);
            return Operand.Create(t);
        }
        // 其他类型：返回0-6，0表示星期一，6表示星期日
        if (t == 0) {
            return Operand.Create(6);
        }
        return Operand.Create(t - 1);
    }
}

export { Function_WEEKDAY };

