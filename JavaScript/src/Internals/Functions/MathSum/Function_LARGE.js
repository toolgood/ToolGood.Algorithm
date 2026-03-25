import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_LARGE extends Function_2 {
    get Name() {
        return "Large";
    }

    constructor(z) {
        super(z);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.a.evaluate(engine, tempParameter);
        args1 = this.convertToArray(args1, 1);
        if (args1.IsError) { return args1; }

        let args2 = this.b.evaluate(engine, tempParameter);
        args2 = this.convertToNumber(args2, 2);
        if (args2.IsError) { return args2; }

        let list = [];
        let o = FunctionUtil.FlattenToList(args1, list);
        if (o == false) { return this.parameterError(1); }
        if (list.length == 0) { return this.parameterError(1); }

        let k = args2.IntValue - engine.ExcelIndex;
        if (k < 0 || k >= list.length) {
            return this.parameterError(2);
        }
        list.sort((a, b) => b - a);
        return Operand.Create(list[k]);
    }
}

export { Function_LARGE };

