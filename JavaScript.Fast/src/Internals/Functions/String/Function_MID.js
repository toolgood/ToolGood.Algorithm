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

        let text = args1.TextValue;
        let startIndex = args2.IntValue - work.ExcelIndex;
        let length = args3.IntValue;

        if (startIndex < 0) {
            return this.parameterError(2);
        }
        if (length < 0) {
            return this.parameterError(3);
        }
        if (startIndex === 0 && length >= text.length) {
            return args1;
        }
        if (startIndex >= text.length) {
            return Operand.Create('');
        }
        if (startIndex + length > text.length) {
            length = text.length - startIndex;
        }
        return Operand.Create(text.substring(startIndex, startIndex + length));
    }
}

export { Function_MID };

