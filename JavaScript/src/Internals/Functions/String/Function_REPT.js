import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_REPT extends Function_2 {
    get Name() {
        return "Rept";
    }

    constructor(z) {
        super(z);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getText_1(work, tempParameter);
        if (args1.IsError) { return args1; }

        let args2 = this.getNumber_2(work, tempParameter);
        if (args2.IsError) { return args2; }

        let newtext = args1.TextValue;
        let length = args2.IntValue;
        if (length < 0) {
            return Operand.Error(this.functionError, 'Rept', 2);
        }
        if (length === 0) {
            return Operand.Create('');
        }
        let result = '';
        for (let i = 0; i < length; i++) {
            result += newtext;
        }
        return Operand.Create(result);
    }
}

export { Function_REPT };

