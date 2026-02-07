import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_SMALL extends Function_2 {
    get Name() {
        return "Small";
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
        let o = FunctionUtil.F_base_GetList(args1, list);
        if (o == false) { return this.parameterError(1); }
        list.sort((a, b) => a - b);
        let k = args2.IntValue;
        if (k < 1 - engine.ExcelIndex || k > list.length - engine.ExcelIndex) {
            return this.parameterError(2);
        }
        return Operand.Create(list[k - engine.ExcelIndex]);
    }
}

export { Function_SMALL };
