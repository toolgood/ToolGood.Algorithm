import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_TRIM extends Function_1 {
    get Name() {
        return "Trim";
    }

    constructor(a) {
        super(a);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getText_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        let text = args1.TextValue.trim();
        // 与 C# 一致:连续的多个空格折叠为单个空格(Excel TRIM 语义)
        text = text.replace(/ +/g, ' ');
        if (text === args1.TextValue) {
            return args1;
        }
        return Operand.Create(text);
    }
}

export { Function_TRIM };

