import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_SEARCH extends Function_3 {
    get Name() {
        return "Search";
    }

    constructor(z) {
        super(z);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.GetText_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        let args2 = this.GetText_2(work, tempParameter);
        if (args2.IsError) { return args2; }

        if (this.c === null) {
            let p = args2.TextValue.toLowerCase().indexOf(args1.TextValue.toLowerCase()) + work.ExcelIndex;
            return Operand.Create(p);
        }
        let args3 = this.GetNumber_3(work, tempParameter);
        if (args3.IsError) { return args3; }
        let startIndex = args3.IntValue - work.ExcelIndex;
        let p2 = args2.TextValue.toLowerCase().indexOf(args1.TextValue.toLowerCase(), startIndex) + work.ExcelIndex;
        return Operand.Create(p2);
    }
}

export { Function_SEARCH };

