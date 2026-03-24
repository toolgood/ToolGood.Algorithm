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

        let t = args1.DateValue.ToDateTime().getDay();
        if (type == 1 || type == 17) {
            return Operand.Create(t + 1);
        } else if (type == 2 || type == 11) {
            if (t == 0) return Operand.Create(7);
            return Operand.Create(t);
        } else if (type == 3) {
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
        } else if (type == 16) {
            let mapping = [2, 3, 4, 5, 6, 7, 1];
            return Operand.Create(mapping[t]);
        }
        return this.parameterError(2);
    }
}

export { Function_WEEKDAY };

