import { Function_N } from '../Function_N.js';

class Function_AVERAGE extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    Evaluate(engine, tempParameter) {
        let args = [];
        for (let item of this._args) {
            let aa = item.Evaluate(engine, tempParameter);
            if (aa.IsError) { return aa; }
            args.push(aa);
        }

        let list = [];
        for (let arg of args) {
            if (arg.IsNotNumber) {
                return Operand.Error("Function '{0}' parameter is error!", "Average");
            }
            list.push(arg.NumberValue);
        }

        if (list.length === 0) {
            return Operand.Create(0);
        }

        let average = list.reduce((sum, value) => sum + value, 0) / list.length;
        return Operand.Create(average);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Average");
    }
}

export { Function_AVERAGE };
