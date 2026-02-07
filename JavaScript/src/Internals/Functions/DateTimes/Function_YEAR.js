import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';


class Function_YEAR extends Function_1 {
    get Name() {
        return "Year";
    }

    constructor(a) {
        super(a);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getDate_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        if (args1.DateValue.Year == null) {
            return this.functionError();
        }
        return Operand.Create(args1.DateValue.Year);
    }

    
}

export { Function_YEAR };

