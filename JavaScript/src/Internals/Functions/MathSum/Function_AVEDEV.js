import { Function_N } from '../Function_N.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_AVEDEV extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    Evaluate(engine, tempParameter) {
        let args = [];
        for (let item of this.funcs) {
            let aa = item.Evaluate(engine, tempParameter);
            if (aa.IsError) { return aa; }
            args.push(aa);
        }

        let list = [];
        for (let arg of args) {
            if (arg.IsNotNumber) {
                return Operand.Error(StringCache.Function_parameter_error2, "AveDev");
            }
            list.push(arg.NumberValue);
        }

        if (list.length === 0) {
            return Operand.Create(0);
        }

        let avg = list.reduce((sum, value) => sum + value, 0) / list.length;
        let sum = 0;
        for (let item of list) {
            sum += Math.abs(item - avg);
        }
        return Operand.Create(sum / list.length);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "AveDev");
    }
}

export { Function_AVEDEV };
