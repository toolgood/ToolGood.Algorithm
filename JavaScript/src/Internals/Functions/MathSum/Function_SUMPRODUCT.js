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

        let arrays = [];
        for (let i = 0; i < this.z.length; i++) {
            let arg = this.getArray(engine, tempParameter, i);
            if (arg.IsError) return arg;
            let list = [];
            for (let item of arg.ArrayValue) {
                if (item.IsNumber) {
                    list.push(item.NumberValue);
                }
            }
            arrays.push(list);
        }

        let minLength = arrays[0].length;
        for (let i = 1; i < arrays.length; i++) {
            if (arrays[i].length < minLength) {
                minLength = arrays[i].length;
            }
        }

        if (minLength == 0) {
            return Operand.Create(0);
        }

        let result = 0;
        for (let i = 0; i < minLength; i++) {
            let product = 1;
            for (let j = 0; j < arrays.length; j++) {
                product *= arrays[j][i];
            }
            result += product;
        }

        return Operand.Create(result);
    }
}

export { Function_SUMPRODUCT };