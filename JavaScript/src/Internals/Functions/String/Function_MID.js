import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_MID extends Function_3 {
    get Name() {
        return "Mid";
    }

    constructor(z) {
        super(z);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getText_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        let args2 = this.getNumber_2(work, tempParameter);
        if (args2.IsError) { return args2; }
        let args3 = this.getNumber_3(work, tempParameter);
        if (args3.IsError) { return args3; }
        let startIndex = args2.IntValue - work.ExcelIndex;
        let length = args3.IntValue;
        return Operand.Create(args1.TextValue.substring(startIndex, startIndex + length));
    }
}

export { Function_MID };

