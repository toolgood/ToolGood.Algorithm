import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';


class Function_DAY extends Function_1 {
    get Name() {
        return "Day";
    }

    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.GetDate_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        if (args1.DateValue.Day == null) {
            return this.FunctionError();
        }
        return Operand.Create(args1.DateValue.Day);
    }
}

export { Function_DAY };

