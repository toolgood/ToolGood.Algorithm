import { Function_N } from '../Function_N.js';

class Function_AVEDEV extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    Evaluate(engine, tempParameter) {
        const args = [];
        for (const item of this._args) {
            const aa = item.Evaluate(engine, tempParameter);
            if (aa.IsError) { return aa; }
            args.push(aa);
        }

        const list = [];
        for (const arg of args) {
            if (arg.IsNotNumber) {
                return engine.createErrorOperand("Function '{0}' parameter is error!", "AveDev");
            }
            list.push(arg.NumberValue);
        }

        if (list.length === 0) {
            return engine.createOperand(0);
        }

        const avg = list.reduce((sum, value) => sum + value, 0) / list.length;
        let sum = 0;
        for (const item of list) {
            sum += Math.abs(item - avg);
        }
        return engine.createOperand(sum / list.length);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "AveDev");
    }
}

export { Function_AVEDEV };
