import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';

class Function_REPLACE extends Function_4 {
    constructor(func1, func2, func3, func4) {
        super(func1, func2, func3, func4);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1.ToText('Function {0} parameter {1} is error!', 'Replace', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        const oldtext = args1.TextValue;
        if (this.func4 === null) {
            const args22 = this.func2.Evaluate(engine, tempParameter);
            if (args22.IsNotText) {
                args22.ToText('Function {0} parameter {1} is error!', 'Replace', 2);
                if (args22.IsError) {
                    return args22;
                }
            }
            const args32 = this.func3.Evaluate(engine, tempParameter);
            if (args32.IsNotText) {
                args32.ToText('Function {0} parameter {1} is error!', 'Replace', 3);
                if (args32.IsError) {
                    return args32;
                }
            }

            const oldStr = args22.TextValue;
            const newstr = args32.TextValue;
            return Operand.Create(oldtext.replace(new RegExp(oldStr.replace(/[.*+?^${}()|[\]\\]/g, '\\$&'), 'g'), newstr));
        }

        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2.ToNumber('Function {0} parameter {1} is error!', 'Replace', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        const args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.IsNotNumber) {
            args3.ToNumber('Function {0} parameter {1} is error!', 'Replace', 3);
            if (args3.IsError) {
                return args3;
            }
        }
        const args4 = this.func4.Evaluate(engine, tempParameter);
        if (args4.IsNotText) {
            args4.ToText('Function {0} parameter {1} is error!', 'Replace', 4);
            if (args4.IsError) {
                return args4;
            }
        }

        const start = args2.IntValue - engine.excelIndex;
        const length = args3.IntValue;
        const newtext = args4.TextValue;

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
        this.AddFunction(stringBuilder, 'Replace');
    }
}

export { Function_REPLACE };
