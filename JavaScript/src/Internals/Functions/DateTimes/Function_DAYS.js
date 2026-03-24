import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_DAYS extends Function_2 {
    get Name() {
        return "Days";
    }

    constructor(func1, func2) {
        super(func1, func2);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getDate_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let args2 = this.getDate_2(engine, tempParameter);
        if (args2.IsError) { return args2; }

        let endDate = args1.DateValue.toDate();
        let startDate = args2.DateValue.toDate();

        return Operand.Create((endDate - startDate) / (1000 * 60 * 60 * 24));
    }
}

export { Function_DAYS };