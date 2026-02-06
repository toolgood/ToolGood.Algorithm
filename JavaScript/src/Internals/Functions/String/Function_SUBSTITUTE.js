import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';

class Function_SUBSTITUTE extends Function_4 {
    get Name() {
        return "Substitute";
    }

    constructor(z) {
        super(z);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.GetText_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        let args2 = this.GetText_2(work, tempParameter);
        if (args2.IsError) { return args2; }
        let args3 = this.GetText_3(work, tempParameter);
        if (args3.IsError) { return args3; }
        if (this.d === null) {
            return Operand.Create(args1.TextValue.replace(new RegExp(args2.TextValue.replace(/[.*+?^${}()|[\]\\]/g, '\\$&'), 'g'), args3.TextValue));
        }
        let args4 = this.GetNumber_4(work, tempParameter);
        if (args4.IsError) { return args4; }
        let text = args1.TextValue;
        let oldtext = args2.TextValue;
        let newtext = args3.TextValue;
        let index = args4.IntValue;

        let index2 = 0;
        let result = '';
        for (let i = 0; i < text.length; i++) {
            let b = true;
            for (let j = 0; j < oldtext.length; j++) {
                if (i + j >= text.length) {
                    b = false;
                    break;
                }
                let t = text[i + j];
                let t2 = oldtext[j];
                if (t !== t2) {
                    b = false;
                    break;
                }
            }
            if (b) {
                index2++;
            }
            if (b && index2 === index) {
                result += newtext;
                i += oldtext.length - 1;
            } else {
                result += text[i];
            }
        }
        return Operand.Create(result);
    }
}

export { Function_SUBSTITUTE };

