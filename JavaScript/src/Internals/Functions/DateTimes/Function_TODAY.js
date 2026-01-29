import { FunctionBase } from '../FunctionBase';
import { MyDate } from '../../MyDate';

class Function_TODAY extends FunctionBase {
    evaluate(engine, tempParameter) {
        let now;
        if (engine.UseLocalTime) {
            now = new Date();
        } else {
            now = new Date(Date.UTC(new Date().getFullYear(), new Date().getMonth(), new Date().getDate()));
        }
        return engine.createOperand(new MyDate(now.getFullYear(), now.getMonth() + 1, now.getDate(), 0, 0, 0));
    }

    toString(stringBuilder, addBrackets) {
        stringBuilder.append("Today()");
    }
}

export { Function_TODAY };
