import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';

class Function_REPLACE extends Function_4 {
    get Name() {
        return "Replace";
    }

    constructor(z) {
        super(z);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getText_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        let oldtext = args1.TextValue;
        if (this.d === null || this.d === undefined) {
            let args22 = this.b.evaluate(work, tempParameter);
            let converted22 = args22.ToText("Function '{0}' parameter {1} is error!", this.Name, 2);
            if (converted22.IsError) {
                return converted22;
            }
            let args32 = this.c.evaluate(work, tempParameter);
            let converted32 = args32.ToText("Function '{0}' parameter {1} is error!", this.Name, 3);
            if (converted32.IsError) {
                return converted32;
            }

            let oldStr = converted22.TextValue;
            let newstr = converted32.TextValue;
            return Operand.Create(oldtext.replace(new RegExp(oldStr.replace(/[.*+?^${}()|[\]\\]/g, '\\$&'), 'g'), newstr));
        }

        let args2 = this.getNumber_2(work, tempParameter);
        if (args2.IsError) { return args2; }
        let args3 = this.getNumber_3(work, tempParameter);
        if (args3.IsError) { return args3; }
        let args4 = this.getText_4(work, tempParameter);
        if (args4.IsError) { return args4; }

        let start = args2.IntValue - work.ExcelIndex;
        let length = args3.IntValue;
        let newtext = args4.TextValue;

        let result = '';
        for (let i = 0; i < oldtext.length; i++) {
            if (i < start) {
                result += oldtext[i];
            } else if (i === start) {
                result += newtext;
            } else if (i >= start + length) {
                result += oldtext[i];
            }
        }
        return Operand.Create(result);
    }
}

export { Function_REPLACE };

