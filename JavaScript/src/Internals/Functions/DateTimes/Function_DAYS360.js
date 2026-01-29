import { Function_3 } from '../Function_3';
import { MyDate } from '../../MyDate';

class Function_DAYS360 extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    evaluate(engine, tempParameter) {
        let args1 = this._arg1.evaluate(engine, tempParameter);
        if (args1.IsNotDate) {
            args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "Days360", 1);
            if (args1.IsError) { return args1; }
        }
        let args2 = this._arg2.evaluate(engine, tempParameter);
        if (args2.IsNotDate) {
            args2 = args2.ToMyDate("Function '{0}' parameter {1} is error!", "Days360", 2);
            if (args2.IsError) { return args2; }
        }

        const startMyDate = args1.DateValue;
        const endMyDate = args2.DateValue;

        let method = false;
        if (this._arg3 !== null) {
            let args3 = this._arg3.evaluate(engine, tempParameter);
            if (args3.IsNotBoolean) {
                args3 = args3.ToBoolean("Function '{0}' parameter {1} is error!", "Days360", 3);
                if (args3.IsError) { return args3; }
            }
            method = args3.BooleanValue;
        }

        let days = endMyDate.getFullYear() * 360 + (endMyDate.getMonth()) * 30
                    - startMyDate.getFullYear() * 360 - (startMyDate.getMonth()) * 30;

        if (method) {
            if (endMyDate.getDate() == 31) days += 30;
            if (startMyDate.getDate() == 31) days -= 30;
        } else {
            if (startMyDate.getMonth() == 11) {
                if (startMyDate.getDate() == new Date(startMyDate.getFullYear() + 1, 0, 0).getDate()) {
                    days -= 30;
                } else {
                    days -= startMyDate.getDate();
                }
            } else {
                if (startMyDate.getDate() == new Date(startMyDate.getFullYear(), startMyDate.getMonth() + 1, 0).getDate()) {
                    days -= 30;
                } else {
                    days -= startMyDate.getDate();
                }
            }
            if (endMyDate.getMonth() == 11) {
                if (endMyDate.getDate() == new Date(endMyDate.getFullYear() + 1, 0, 0).getDate()) {
                    if (startMyDate.getDate() < 30) {
                        days += 31;
                    } else {
                        days += 30;
                    }
                } else {
                    days += endMyDate.getDate();
                }
            } else {
                if (endMyDate.getDate() == new Date(endMyDate.getFullYear(), endMyDate.getMonth() + 1, 0).getDate()) {
                    if (startMyDate.getDate() < 30) {
                        days += 31;
                    } else {
                        days += 30;
                    }
                } else {
                    days += endMyDate.getDate();
                }
            }
        }
        return engine.createOperand(days);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Days360");
    }
}

export { Function_DAYS360 };
