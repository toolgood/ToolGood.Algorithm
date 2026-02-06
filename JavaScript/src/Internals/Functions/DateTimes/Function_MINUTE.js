import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_MINUTE extends Function_1 {
    get Name() {
        return "Minute";
    }

    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.GetDate_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        return Operand.Create(args1.DateValue.Minute);
    }
}

export { Function_MINUTE };

