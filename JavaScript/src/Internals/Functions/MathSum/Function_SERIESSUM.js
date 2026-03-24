import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_SERIESSUM extends Function_N {
    get Name() {
        return "SERIESSUM";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        if (this.z.length < 4) return this.parameterError(1);

        let xArg = this.getNumber(engine, tempParameter, 0);
        if (xArg.IsError) return xArg;
        let x = xArg.NumberValue;

        let nArg = this.getNumber(engine, tempParameter, 1);
        if (nArg.IsError) return nArg;
        let n = nArg.NumberValue;

        let mArg = this.getNumber(engine, tempParameter, 2);
        if (mArg.IsError) return mArg;
        let m = mArg.NumberValue;

        let coefficientsArg = this.getArray(engine, tempParameter, 3);
        if (coefficientsArg.IsError) return coefficientsArg;

        let result = 0;
        let i = 0;
        for (let coef of coefficientsArg.ArrayValue) {
            if (coef.IsNumber) {
                let power = n + i * m;
                result += coef.NumberValue * Math.pow(x, power);
                i++;
            }
        }

        return Operand.Create(result);
    }
}

export { Function_SERIESSUM };