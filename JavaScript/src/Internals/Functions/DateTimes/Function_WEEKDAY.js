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
            if (type != 1 && type != 2 && type != 3 && (type < 11 || type > 17)) {
                return this.parameterError(2);
            }
        }

        let t = args1.DateValue.ToDateTime().getDay(); // JavaScript中，0表示星期日，6表示星期六
        if (type == 1 || type == 17) {
            // 类型1/17：返回1-7，1表示星期日，7表示星期六
            return Operand.Create(t + 1);
        } else if (type == 2 || type == 11) {
            // 类型2/11：返回1-7，1表示星期一，7表示星期日
            if (t == 0) return Operand.Create(7);
            return Operand.Create(t);
        } else if (type == 3) {
            // 类型3：返回0-6，0表示星期一，6表示星期日
            if (t == 0) return Operand.Create(6);
            return Operand.Create(t - 1);
        } else if (type == 12) {
            let mapping = [6, 7, 1, 2, 3, 4, 5];
            return Operand.Create(mapping[t]);
        } else if (type == 13) {
            let mapping = [5, 6, 7, 1, 2, 3, 4];
            return Operand.Create(mapping[t]);
        } else if (type == 14) {
            let mapping = [4, 5, 6, 7, 1, 2, 3];
            return Operand.Create(mapping[t]);
        } else if (type == 15) {
            let mapping = [3, 4, 5, 6, 7, 1, 2];
            return Operand.Create(mapping[t]);
        } else {
            // 类型16：返回2-7，1，2表示星期二，1表示星期一
            let mapping = [2, 3, 4, 5, 6, 7, 1];
            return Operand.Create(mapping[t]);
        }
    }
}

export { Function_WEEKDAY };

