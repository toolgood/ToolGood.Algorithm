import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_PRODUCT extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    Evaluate(engine, tempParameter) {
        let args = [];
        for (let i = 0; i < this.funcs.length; i++) {
            let aa = this.funcs[i].Evaluate(engine, tempParameter);
            if (aa.IsError) { return aa; }
            args.push(aa);
        }

        let list = [];
        for (let arg of args) {
            if (arg.IsNotNumber) {
                return Operand.Error("Function {0} parameter is error!", "Product");
            }
            list.push(arg.NumberValue);
        }

        if (list.length === 0) {
            return Operand.Error("Function {0} parameter is error!", "Product");
        }

        let d = 1;
        for (let a of list) {
            d *= a;
        }
        return Operand.Create(d);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Product");
    }
}

export { Function_PRODUCT };
