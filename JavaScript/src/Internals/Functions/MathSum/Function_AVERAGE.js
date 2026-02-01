import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_AVERAGE extends Function_N {
    constructor(z) {
        super(z);
    }

    Evaluate(engine, tempParameter) {
        let args = [];
        for (let item of this.z) {
            let aa = item.Evaluate(engine, tempParameter);
            if (aa.IsError) { return aa; }
            args.push(aa);
        }

        let list = [];
        for (let arg of args) {
            if (arg.IsNotNumber) {
                return Operand.Error(StringCache.Function_parameter_error, "Average");
            }
            list.push(arg.NumberValue);
        }

        if (list.length === 0) {
            return Operand.Create(0);
        }

        let average = list.reduce((sum, value) => sum + value, 0) / list.length;
        return Operand.Create(average);
    }
}

export { Function_AVERAGE };

