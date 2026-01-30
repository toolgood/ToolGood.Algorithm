import { FunctionBase } from '../FunctionBase.js';
import { MyDate } from '../../MyDate.js';
import { Operand } from '../../../Operand.js';

class Function_TODAY extends FunctionBase {
    Evaluate(engine, tempParameter) {
        let now;
        if (engine.UseLocalTime) {
            now = new Date();
        } else {
            now = new Date(Date.UTC(new Date().getFullYear(), new Date().getMonth(), new Date().getDate()));
        }
        return Operand.Create(new MyDate(now.getFullYear(), now.getMonth() + 1, now.getDate(), 0, 0, 0));
    }
}

export { Function_TODAY };

