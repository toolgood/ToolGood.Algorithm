import { Function_N } from '../Function_N.js';

class Function_PRODUCT extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    Evaluate(engine, tempParameter) {
        const args = [];
        for (let i = 0; i < this._args.length; i++) {
            const aa = this._args[i].Evaluate(engine, tempParameter);
            if (aa.IsError) { return aa; }
            args.push(aa);
        }

        const list = [];
        for (const arg of args) {
            if (arg.IsNotNumber) {
                return engine.createErrorOperand("Function '{0}' parameter is error!", "Product");
            }
            list.push(arg.NumberValue);
        }

        if (list.length === 0) {
            return engine.createErrorOperand("Function '{0}' parameter is error!", "Product");
        }

        let d = 1;
        for (const a of list) {
            d *= a;
        }
        return engine.createOperand(d);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Product");
    }
}

export { Function_PRODUCT };
