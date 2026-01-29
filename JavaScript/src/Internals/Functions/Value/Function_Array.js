import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_Array extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        const args = [];
        for (const item of this.funcs) {
            const aa = item.evaluate(engine, tempParameter);
            if (aa.isError) {
                return aa;
            }
            args.push(aa);
        }
        return Operand.create(args);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Array');
    }
}

export { Function_Array };
