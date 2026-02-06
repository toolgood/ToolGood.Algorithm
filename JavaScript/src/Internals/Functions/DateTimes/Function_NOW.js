import { FunctionBase } from '../FunctionBase.js';
import { MyDate } from '../../MyDate.js';
import { Operand } from '../../../Operand.js';

class Function_NOW extends FunctionBase {
    get Name() {
        return "Now";
    }

    Evaluate(engine, tempParameter) {
        if (engine.UseLocalTime) {
            return Operand.Create(new MyDate(new Date()));
        } else {
            return Operand.Create(new MyDate(new Date(Date.UTC(new Date().getFullYear(), new Date().getMonth(), new Date().getDate(), new Date().getHours(), new Date().getMinutes(), new Date().getSeconds()))));
        }
    }

    ToString(stringBuilder, addBrackets) {
        stringBuilder.push("Now()");
    }
}

export { Function_NOW };

