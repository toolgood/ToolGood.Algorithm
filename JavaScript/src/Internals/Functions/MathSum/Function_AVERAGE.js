import { Function_N } from '../Function_N';

class Function_AVERAGE extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        const args = [];
        for (const item of this._args) {
            const aa = item.evaluate(engine, tempParameter);
            if (aa.IsError) { return aa; }
            args.push(aa);
        }

        const list = [];
        for (const arg of args) {
            if (arg.IsNotNumber) {
                return engine.createErrorOperand("Function '{0}' parameter is error!", "Average");
            }
            list.push(arg.NumberValue);
        }

        if (list.length === 0) {
            return engine.createOperand(0);
        }

        const average = list.reduce((sum, value) => sum + value, 0) / list.length;
        return engine.createOperand(average);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Average");
    }
}

export { Function_AVERAGE };
