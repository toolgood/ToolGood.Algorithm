import { FunctionBase } from '../FunctionBase.js';
import { MyDate } from '../../MyDate.js';
import { Operand } from '../../../Operand.js';

class Function_NOW extends FunctionBase {
    get Name() {
        return "Now";
    }

    evaluate(engine, tempParameter) {
        let now = new Date();
        if (engine.UseLocalTime) {
            return Operand.Create(new MyDate(now));
        } else {
            // C# DateTime.UtcNow：取 UTC 时间分量
            let utc = new Date(Date.UTC(now.getUTCFullYear(), now.getUTCMonth(), now.getUTCDate(), now.getUTCHours(), now.getUTCMinutes(), now.getUTCSeconds()));
            return Operand.Create(new MyDate(utc));
        }
    }

    toString2(stringBuilder, addBrackets) {
        stringBuilder.push("Now()");
    }
}

export { Function_NOW };

