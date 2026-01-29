import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_MID extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1.ToText('Function {0} parameter {1} is error!', 'Mid', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2.ToNumber('Function {0} parameter {1} is error!', 'Mid', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        const args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.IsNotNumber) {
            args3.ToNumber('Function {0} parameter {1} is error!', 'Mid', 3);
            if (args3.IsError) {
                return args3;
            }
        }
        const startIndex = args2.IntValue - engine.excelIndex;
        const length = args3.IntValue;
        return Operand.Create(args1.TextValue.substring(startIndex, startIndex + length));
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Mid');
    }
}

export { Function_MID };
