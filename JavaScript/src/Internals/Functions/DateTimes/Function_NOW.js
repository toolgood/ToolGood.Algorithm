import { FunctionBase } from '../FunctionBase.js';
import { MyDate } from '../../MyDate.js';
import { Operand } from '../../../Operand.js';

class Function_NOW extends FunctionBase {
    Evaluate(engine, tempParameter) {
        if (engine.UseLocalTime) {
            return Operand.Create(new MyDate(new Date()));
        } else {
            return Operand.Create(new MyDate(new Date(Date.UTC(new Date().getFullYear(), new Date().getMonth(), new Date().getDate(), new Date().getHours(), new Date().getMinutes(), new Date().getSeconds()))));
        }
    }
}

export { Function_NOW };

