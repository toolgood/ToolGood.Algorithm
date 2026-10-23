import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_DAYS extends Function_2 {
    get Name() {
        return "Days";
    }

    constructor(a, b) {
        super(a, b);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getDate_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let args2 = this.getDate_2(engine, tempParameter);
        if (args2.IsError) { return args2; }
        
        let endDate = args1.DateValue.ToDateTime();
        let startDate = args2.DateValue.ToDateTime();

        // 与 C# (end.Date - start.Date).Days 一致：先截断到日期部分
        let e = new Date(endDate.getFullYear(), endDate.getMonth(), endDate.getDate());
        let s = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate());

        let diffTime = e.getTime() - s.getTime();
        let diffDays = Math.round(diffTime / (1000 * 60 * 60 * 24));

        return Operand.Create(diffDays);
    }
}

export { Function_DAYS };
