import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_SEARCH extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    evaluate(engine, tempParameter) {
        const args1 = this.func1.evaluate(engine, tempParameter);
        if (args1.isNotText) {
            args1.toText('Function {0} parameter {1} is error!', 'Search', 1);
            if (args1.isError) {
                return args1;
            }
        }
        const args2 = this.func2.evaluate(engine, tempParameter);
        if (args2.isNotText) {
            args2.toText('Function {0} parameter {1} is error!', 'Search', 2);
            if (args2.isError) {
                return args2;
            }
        }

        if (this.func3 === null) {
            const p = args2.textValue.toLowerCase().indexOf(args1.textValue.toLowerCase()) + engine.excelIndex;
            return Operand.create(p);
        }
        const args3 = this.func3.evaluate(engine, tempParameter);
        if (args3.isNotNumber) {
            args3.toNumber('Function {0} parameter {1} is error!', 'Search', 3);
            if (args3.isError) {
                return args3;
            }
        }
        const startIndex = args3.intValue - engine.excelIndex;
        const p2 = args2.textValue.toLowerCase().indexOf(args1.textValue.toLowerCase(), startIndex) + engine.excelIndex;
        return Operand.create(p2);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Search');
    }
}

export { Function_SEARCH };
