import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_PRODUCT extends Function_N {
    constructor(z) {
        super(z);
    }

    Evaluate(engine, tempParameter) {
        let args = [];
        for (let i = 0; i < this.z.length; i++) {
            let aa = this.z[i].Evaluate(engine, tempParameter);
            if (aa.IsError) { return aa; }
            args.push(aa);
        }

        let list = [];
        for (let arg of args) {
            if (arg.IsNotNumber) {
                return Operand.Error(StringCache.Function_parameter_error, "Product");
            }
            list.push(arg.NumberValue);
        }

        if (list.length === 0) {
            return Operand.Error(StringCache.Function_parameter_error, "Product");
        }

        let d = 1;
        for (let a of list) {
            d *= a;
        }
        return Operand.Create(d);
    }
}

export { Function_PRODUCT };

