import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

export class Function_SUBSTRING extends Function_3 {
    constructor(z) {
        super(z);
    }

    get Name() {
        return "Substring";
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getText_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsError) { return args2; }

        let text = args1.TextValue;
        let startIndex = args2.IntValue - engine.ExcelIndex;

        if (startIndex < 0) {
            return this.parameterError(2);
        }
        if (startIndex >= text.length) {
            return Operand.Create("");
        }

        if (this.c == null) {
            return Operand.Create(text.substring(startIndex));
        }

        let args3 = this.getNumber_3(engine, tempParameter);
        if (args3.IsError) { return args3; }

        let length = args3.IntValue;
        if (length < 0) {
            return this.parameterError(3);
        }
        if (startIndex + length > text.length) {
            length = text.length - startIndex;
        }
        return Operand.Create(text.substring(startIndex, startIndex + length));
    }
}

