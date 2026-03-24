import { Function_2 } from '../Function_2.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { Operand } from '../../../Operand.js';

class Function_TIMESTAMP extends Function_2 {
    get Name() {
        return "Timestamp";
    }

    constructor(z) {
        super(z);
    }

    evaluate(engine, tempParameter) {
        let type = 0;
        if (this.b != null) {
            let args2 = this.getNumber_2(engine, tempParameter);
            if (args2.IsError) { return args2; }
            type = args2.IntValue;
        }
        let args0 = this.getDate_1(engine, tempParameter);
        if (args0.IsError) { return args0; }

        let startDate = new Date(Date.UTC(1970, 0, 1, 0, 0, 0, 0));
        let date;
        if (engine.UseLocalTime) {
            date = new Date(args0.DateValue.ToDateTime().getTime() + args0.DateValue.ToDateTime().getTimezoneOffset() * 60000);
        } else {
            date = args0.DateValue.ToDateTime();
        }

        if (type === 0) {
            let ms = (date - startDate);
            return Operand.Create(ms);
        } else if (type === 1) {
            let s = (date - startDate) / 1000;
            return Operand.Create(s);
        }
        return this.parameterError(2);
    }
}

export { Function_TIMESTAMP };
