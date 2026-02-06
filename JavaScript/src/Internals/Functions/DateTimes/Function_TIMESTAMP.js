import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_TIMESTAMP extends Function_2 {
    get Name() {
        return "Timestamp";
    }

    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let type = 0; // 毫秒
        if (this.b != null) {
            let args2 = this.GetNumber_2(engine, tempParameter);
            if (args2.IsError) { return args2; }
            type = args2.IntValue;
        }
        let args0 = this.GetDate_1(engine, tempParameter);
        if (args0.IsError) { return args0; }

        let date = args0.DateValue.ToDateTime();
        let milliseconds = date.getTime();

        if (type == 0) {
            // 毫秒时间戳
            return Operand.Create(milliseconds);
        } else if (type == 1) {
            // 秒时间戳
            return Operand.Create(Math.floor(milliseconds / 1000));
        }
        return this.FunctionError();
    }
}

export { Function_TIMESTAMP };

