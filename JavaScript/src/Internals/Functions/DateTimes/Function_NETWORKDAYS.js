import { Function_N } from '../Function_N';
import { MyDate } from '../../MyDate';

class Function_NETWORKDAYS extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        let args1 = this._args[0].evaluate(engine, tempParameter);
        if (args1.IsNotDate) {
            args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "NetWorkdays", 1);
            if (args1.IsError) { return args1; }
        }
        let args2 = this._args[1].evaluate(engine, tempParameter);
        if (args2.IsNotDate) {
            args2 = args2.ToMyDate("Function '{0}' parameter {1} is error!", "NetWorkdays", 2);
            if (args2.IsError) { return args2; }
        }

        let startMyDate = new Date(args1.DateValue.getTime());
        const endMyDate = new Date(args2.DateValue.getTime());

        const list = new Set();
        for (let i = 2; i < this._args.length; i++) {
            const ar = this._args[i].evaluate(engine, tempParameter);
            if (ar.IsNotDate) {
                const arDate = ar.ToMyDate("Function '{0}' parameter {1} is error!", "NetWorkdays", i + 1);
                if (arDate.IsError) { return arDate; }
                // 将日期转换为YYYY-MM-DD格式以确保Set能够正确比较
                const dateStr = arDate.DateValue.toISOString().split('T')[0];
                list.add(dateStr);
            } else {
                // 将日期转换为YYYY-MM-DD格式以确保Set能够正确比较
                const dateStr = ar.DateValue.toISOString().split('T')[0];
                list.add(dateStr);
            }
        }

        let days = 0;
        while (startMyDate <= endMyDate) {
            const dayOfWeek = startMyDate.getDay();
            if (dayOfWeek !== 0 && dayOfWeek !== 6) { // 0是周日，6是周六
                // 将当前日期转换为YYYY-MM-DD格式以确保Set能够正确比较
                const currentDateStr = startMyDate.toISOString().split('T')[0];
                if (!list.has(currentDateStr)) {
                    days++;
                }
            }
            startMyDate.setDate(startMyDate.getDate() + 1);
        }
        return engine.createOperand(days);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "NetWorkdays");
    }
}

export { Function_NETWORKDAYS };
