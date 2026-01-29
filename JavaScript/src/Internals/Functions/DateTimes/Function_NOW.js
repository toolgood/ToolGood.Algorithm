import { FunctionBase } from '../FunctionBase.js';
import { MyDate } from '../../MyDate.js';

class Function_NOW extends FunctionBase {
    evaluate(engine, tempParameter) {
        if (engine.UseLocalTime) {
            return engine.createOperand(new MyDate(new Date()));
        } else {
            return engine.createOperand(new MyDate(new Date(Date.UTC(new Date().getFullYear(), new Date().getMonth(), new Date().getDate(), new Date().getHours(), new Date().getMinutes(), new Date().getSeconds()))));
        }
    }

    toString(stringBuilder, addBrackets) {
        stringBuilder.append("Now()");
    }
}

export { Function_NOW };
