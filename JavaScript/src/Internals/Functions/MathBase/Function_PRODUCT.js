import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_PRODUCT extends Function_N {
    get Name() {
        return "Product";
    }

    constructor(z) {
        super(z);
    }

    evaluate(engine, tempParameter) {
        let args = [];
        for (let i = 0; i < this.z.length; i++) {
            let aa = this.getNumber(engine, tempParameter, i);
            if (aa.IsError) { return aa; }
            args.push(aa);
        }

        let list = [];
        for (let arg of args) {
            if (!arg.IsNumber) {
                return this.functionError();
            }
            list.push(arg.NumberValue);
        }

        let d = 1;
        for (let i = 0; i < list.length; i++) {
            let a = list[i];
            d *= a;
        }
        return Operand.Create(d);
    }
}

export { Function_PRODUCT };

