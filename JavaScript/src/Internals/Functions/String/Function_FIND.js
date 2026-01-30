import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_FIND extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText('Function \'{0}\' parameter {1} is error!', 'Find', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.func2.Evaluate(work, tempParameter);
        if (args2.IsNotText) {
            args2 = args2.ToText('Function \'{0}\' parameter {1} is error!', 'Find', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        if (this.func3 === null) {
            let p = args2.TextValue.indexOf(args1.TextValue) + work.ExcelIndex;
            return Operand.Create(p);
        }
        let count = this.func3.Evaluate(work, tempParameter);
        if (count.IsNotNumber) {
            count.ToNumber('Function \'{0}\' parameter {1} is error!', 'Find', 3);
            if (count.IsError) {
                return count;
            }
        }
        let p2 = args2.TextValue.indexOf(args1.TextValue, count.IntValue) + count.IntValue + work.ExcelIndex;
        return Operand.Create(p2);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'Find');
    }
}

export { Function_FIND };
