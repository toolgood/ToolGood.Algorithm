import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_Array extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    Evaluate(engine, tempParameter) {
        let args = [];
        console.log('Function_Array.funcs.length:', this.funcs.length);
        for (let i = 0; i < this.funcs.length; i++) {
            let item = this.funcs[i];
            console.log(`Function_Array.item[${i}]:`, item);
            let aa = item.Evaluate(engine, tempParameter);
            console.log(`Function_Array.aa[${i}]:`, aa);
            if (aa.IsError) {
                return aa;
            }
            args.push(aa);
        }
        console.log('Function_Array.args:', args);
        let result = Operand.Create(args);
        console.log('Function_Array.result:', result);
        return result;
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'Array');
    }
}

export { Function_Array };
