import { Function_3 } from '../Function_3.js';
import { MyDate } from '../../MyDate.js';
import { Operand } from '../../../Operand.js';

class Function_DAYS360 extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotDate) {
            args1 = args1.ToMyDate("Function {0} parameter {1} is error!", "Days360", 1);
            if (args1.IsError) { return args1; }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotDate) {
            args2 = args2.ToMyDate("Function {0} parameter {1} is error!", "Days360", 2);
            if (args2.IsError) { return args2; }
        }

        let startMyDate = args1.DateValue;  // MyDate对象
        let endMyDate = args2.DateValue;    // MyDate对象
        let startDate = startMyDate.ToDateTime();  // Date对象
        let endDate = endMyDate.ToDateTime();      // Date对象

        let method = false;
        if (this.func3 !== null) {
            let args3 = this.func3.Evaluate(engine, tempParameter);
            if (args3.IsNotBoolean) {
                args3 = args3.ToBoolean("Function {0} parameter {1} is error!", "Days360", 3);
                if (args3.IsError) { return args3; }
            }
            method = args3.BooleanValue;
        }

        let days = endDate.getFullYear() * 360 + (endDate.getMonth()) * 30
                    - startDate.getFullYear() * 360 - (startDate.getMonth()) * 30;

        if (method) {
            if (endDate.getDate() == 31) days += 30;
            if (startDate.getDate() == 31) days -= 30;
        } else {
            if (startDate.getMonth() == 11) {
                if (startDate.getDate() == new Date(startDate.getFullYear() + 1, 0, 0).getDate()) {
                    days -= 30;
                } else {
                    days -= startDate.getDate();
                }
            } else {
                if (startDate.getDate() == new Date(startDate.getFullYear(), startDate.getMonth() + 1, 0).getDate()) {
                    days -= 30;
                } else {
                    days -= startDate.getDate();
                }
            }
            if (endDate.getMonth() == 11) {
                if (endDate.getDate() == new Date(endDate.getFullYear() + 1, 0, 0).getDate()) {
                    if (startDate.getDate() < 30) {
                        days += 31;
                    } else {
                        days += 30;
                    }
                } else {
                    days += endDate.getDate();
                }
            } else {
                if (endDate.getDate() == new Date(endDate.getFullYear(), endDate.getMonth() + 1, 0).getDate()) {
                    if (startDate.getDate() < 30) {
                        days += 31;
                    } else {
                        days += 30;
                    }
                } else {
                    days += endDate.getDate();
                }
            }
        }
        return Operand.Create(days);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Days360");
    }
}

export { Function_DAYS360 };
