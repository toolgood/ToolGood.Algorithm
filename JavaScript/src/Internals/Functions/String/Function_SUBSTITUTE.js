import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';

class Function_SUBSTITUTE extends Function_4 {
    constructor(func1, func2, func3, func4) {
        super(func1, func2, func3, func4);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1.ToText('Function {0} parameter {1} is error!', 'Substitute', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotText) {
            args2.ToText('Function {0} parameter {1} is error!', 'Substitute', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        const args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.IsNotText) {
            args3.ToText('Function {0} parameter {1} is error!', 'Substitute', 3);
            if (args3.IsError) {
                return args3;
            }
        }
        if (this.func4 === null) {
            return Operand.Create(args1.TextValue.replace(new RegExp(args2.TextValue.replace(/[.*+?^${}()|[\]\\]/g, '\\$&'), 'g'), args3.TextValue));
        }
        const args4 = this.func4.Evaluate(engine, tempParameter);
        if (args4.IsNotNumber) {
            args4.ToNumber('Function {0} parameter {1} is error!', 'Substitute', 4);
            if (args4.IsError) {
                return args4;
            }
        }
        const text = args1.TextValue;
        const oldtext = args2.TextValue;
        const newtext = args3.TextValue;
        const index = args4.IntValue;

        let index2 = 0;
        let result = '';
        for (let i = 0; i < text.length; i++) {
            let b = true;
            for (let j = 0; j < oldtext.length; j++) {
                if (i + j >= text.length) {
                    b = false;
                    break;
                }
                const t = text[i + j];
                const t2 = oldtext[j];
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

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Substitute');
    }
}

export { Function_SUBSTITUTE };
