import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';


class Function_MONTH extends Function_1 {
    get Name() {
        return "Month";
    }

    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.GetDate_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        if (args1.DateValue.Month == null) {
            return this.FunctionError();
        }
        return Operand.Create(args1.DateValue.Month);
    }
}

export { Function_MONTH };

