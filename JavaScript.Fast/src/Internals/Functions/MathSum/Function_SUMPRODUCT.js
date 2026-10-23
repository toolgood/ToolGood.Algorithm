import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_SUMPRODUCT extends Function_N {
    get Name() {
        return "SUMPRODUCT";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        if (this.z.length < 2) return this.parameterError(1);

        const arrays = [];
        for (let i = 0; i < this.z.length; i++) {
            const arg = this.getArray(engine, tempParameter, i);
            if (arg.IsError) return arg;
            const arr = [];
            for (const item of arg.ArrayValue) {
                if (item.IsNumber) {
                    arr.push(item.NumberValue);
                }
            }
            arrays.push(arr);
        }

        const minLength = Math.min(...arrays.map(a => a.length));
        let result = 0;

        for (let i = 0; i < minLength; i++) {
            let product = 1;
            for (const arr of arrays) {
                product *= arr[i];
            }
            result += product;
        }

        return Operand.Create(result);
    }
}

export { Function_SUMPRODUCT };
