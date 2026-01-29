import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';

class Function_SUBSTITUTE extends Function_4 {
    constructor(func1, func2, func3, func4) {
        super(func1, func2, func3, func4);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.isNotText) {
            args1.toText('Function {0} parameter {1} is error!', 'Substitute', 1);
            if (args1.isError) {
                return args1;
            }
        }
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.isNotText) {
            args2.toText('Function {0} parameter {1} is error!', 'Substitute', 2);
            if (args2.isError) {
                return args2;
            }
        }
        const args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.isNotText) {
            args3.toText('Function {0} parameter {1} is error!', 'Substitute', 3);
            if (args3.isError) {
                return args3;
            }
        }
        if (this.func4 === null) {
            return Operand.Create(args1.textValue.replace(new RegExp(args2.textValue.replace(/[.*+?^${}()|[\]\\]/g, '\\$&'), 'g'), args3.textValue));
        }
        const args4 = this.func4.Evaluate(engine, tempParameter);
        if (args4.isNotNumber) {
            args4.ToNumber('Function {0} parameter {1} is error!', 'Substitute', 4);
            if (args4.isError) {
                return args4;
            }
        }
        const text = args1.textValue;
        const oldtext = args2.textValue;
        const newtext = args3.textValue;
        const index = args4.intValue;

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
