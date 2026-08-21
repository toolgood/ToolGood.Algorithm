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

        const xArg = this.getNumber(engine, tempParameter, 0);
        if (xArg.IsError) return xArg;
        const x = xArg.NumberValue;

        const nArg = this.getNumber(engine, tempParameter, 1);
        if (nArg.IsError) return nArg;
        const n = nArg.NumberValue;

        const mArg = this.getNumber(engine, tempParameter, 2);
        if (mArg.IsError) return mArg;
        const m = mArg.NumberValue;

        const coefficientsArg = this.getArray(engine, tempParameter, 3);
        if (coefficientsArg.IsError) return coefficientsArg;

        let result = 0;
        const coefArray = coefficientsArg.ArrayValue;
        for (let i = 0; i < coefArray.length; i++) {
            const coef = coefArray[i];
            if (coef.IsNumber) {
                const power = n + i * m;
                const term = coef.NumberValue * Math.pow(x, power);
                // 负底数非整数幂产生 NaN、数值溢出产生 Infinity,对应 C# 捕获异常返回错误
                if (!isFinite(term)) return this.functionError();
                result += term;
            }
        }

        return Operand.Create(result);
    }
}

export { Function_SERIESSUM };
