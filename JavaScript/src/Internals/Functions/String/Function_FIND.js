import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_FIND extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    evaluate(work, tempParameter) {
        const args1 = this.func1.evaluate(work, tempParameter);
        if (args1.isNotText) {
            args1.toText('Function \'{0}\' parameter {1} is error!', 'Find', 1);
            if (args1.isError) {
                return args1;
            }
        }
        const args2 = this.func2.evaluate(work, tempParameter);
        if (args2.isNotText) {
            args2.toText('Function \'{0}\' parameter {1} is error!', 'Find', 2);
            if (args2.isError) {
                return args2;
            }
        }
        if (this.func3 === null) {
            const p = args2.textValue.indexOf(args1.textValue) + work.excelIndex;
            return Operand.create(p);
        }
        const count = this.func3.evaluate(work, tempParameter);
        if (count.isNotNumber) {
            count.toNumber('Function \'{0}\' parameter {1} is error!', 'Find', 3);
            if (count.isError) {
                return count;
            }
        }
        const p2 = args2.textValue.indexOf(args1.textValue, count.intValue) + count.intValue + work.excelIndex;
        return Operand.create(p2);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Find');
    }
}

export { Function_FIND };
