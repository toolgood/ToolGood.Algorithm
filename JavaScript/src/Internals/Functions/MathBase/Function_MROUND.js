import { Function_2 } from '../Function_2.js';

class Function_MROUND extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    evaluate(engine, tempParameter) {
        let args1 = this._arg1.evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "MRound", 1);
            if (args1.IsError) { return args1; }
        }
        let args2 = this._arg2.evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "MRound", 2);
            if (args2.IsError) { return args2; }
        }
        const a = args2.NumberValue;
        if (a <= 0) {
            return engine.createErrorOperand("Function '{0}' parameter {1} is error!", "MRound", 2);
        }

        const b = args1.NumberValue;
        // 四舍五入到最接近的倍数
        const r = Math.round(b / a) * a;
        return engine.createOperand(r);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "MRound");
    }
}

export { Function_MROUND };
