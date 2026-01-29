import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';

class Function_REPLACE extends Function_4 {
    constructor(func1, func2, func3, func4) {
        super(func1, func2, func3, func4);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.isNotText) {
            args1.ToText('Function {0} parameter {1} is error!', 'Replace', 1);
            if (args1.isError) {
                return args1;
            }
        }
        const oldtext = args1.textValue;
        if (this.func4 === null) {
            const args22 = this.func2.Evaluate(engine, tempParameter);
            if (args22.isNotText) {
                args22.ToText('Function {0} parameter {1} is error!', 'Replace', 2);
                if (args22.isError) {
                    return args22;
                }
            }
            const args32 = this.func3.Evaluate(engine, tempParameter);
            if (args32.isNotText) {
                args32.ToText('Function {0} parameter {1} is error!', 'Replace', 3);
                if (args32.isError) {
                    return args32;
                }
            }

            const oldStr = args22.textValue;
            const newstr = args32.textValue;
            return Operand.Create(oldtext.replace(new RegExp(oldStr.replace(/[.*+?^${}()|[\]\\]/g, '\\$&'), 'g'), newstr));
        }

        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.isNotNumber) {
            args2.ToNumber('Function {0} parameter {1} is error!', 'Replace', 2);
            if (args2.isError) {
                return args2;
            }
        }
        const args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.isNotNumber) {
            args3.ToNumber('Function {0} parameter {1} is error!', 'Replace', 3);
            if (args3.isError) {
                return args3;
            }
        }
        const args4 = this.func4.Evaluate(engine, tempParameter);
        if (args4.isNotText) {
            args4.ToText('Function {0} parameter {1} is error!', 'Replace', 4);
            if (args4.isError) {
                return args4;
            }
        }

        const start = args2.intValue - engine.excelIndex;
        const length = args3.intValue;
        const newtext = args4.textValue;

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

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Replace');
    }
}

export { Function_REPLACE };
