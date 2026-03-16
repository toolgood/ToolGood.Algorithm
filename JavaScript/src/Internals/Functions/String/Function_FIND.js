import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_FIND extends Function_3 {
    get Name() {
        return "Find";
    }

    constructor(z) {
        super(z);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getText_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        let args2 = this.getText_2(work, tempParameter);
        if (args2.IsError) { return args2; }
        if (this.c === null || this.c === undefined) {
            let p = args2.TextValue.indexOf(args1.TextValue) + work.ExcelIndex;
            return Operand.Create(p);
        }
        let count = this.getNumber_3(work, tempParameter);
        if (count.IsError) { return count; }
      	let p2 = args2.TextValue.indexOf(args1.TextValue, count.IntValue) + count.IntValue + work.ExcelIndex;
        return Operand.Create(p2);
    }
}

export { Function_FIND };

