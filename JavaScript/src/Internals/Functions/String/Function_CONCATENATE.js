import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_CONCATENATE extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    Evaluate(work, tempParameter) {
        if (this.funcs.length === 0) {
            return Operand.create('');
        }
        if (this.funcs.length === 1) {
            const a = this.funcs[0].Evaluate(work, tempParameter);
            if (a.isNotText) {
                a.toText('Function \'{0}\' parameter {1} is error!', 'Concatenate', 1);
                if (a.isError) {
                    return a;
                }
            }
            return a;
        }
        let result = '';
        for (let i = 0; i < this.funcs.length; i++) {
            const a = this.funcs[i].Evaluate(work, tempParameter);
            if (a.isNotText) {
                a.toText('Function \'{0}\' parameter {1} is error!', 'Concatenate', i + 1);
                if (a.isError) {
                    return a;
                }
            }
            result += a.textValue;
        }
        return Operand.create(result);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Concatenate');
    }
}

export { Function_CONCATENATE };
