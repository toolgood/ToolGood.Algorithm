import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_FORECAST extends Function_N {
    get Name() {
        return "FORECAST";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        if (this.z.length < 3) return this.parameterError(1);

        const xArg = this.getNumber(engine, tempParameter, 0);
        if (xArg.IsError) return xArg;
        const x = xArg.NumberValue;

        const yArrayArg = this.getArray(engine, tempParameter, 1);
        if (yArrayArg.IsError) return yArrayArg;
        const yValues = [];
        for (const item of yArrayArg.ArrayValue) {
            if (item.IsNumber) yValues.push(item.NumberValue);
        }

        const xArrayArg = this.getArray(engine, tempParameter, 2);
        if (xArrayArg.IsError) return xArrayArg;
        const xValues = [];
        for (const item of xArrayArg.ArrayValue) {
            if (item.IsNumber) xValues.push(item.NumberValue);
        }

        if (yValues.length !== xValues.length || yValues.length < 2) return this.functionError();

        let sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0;
        const n = yValues.length;

        for (let i = 0; i < n; i++) {
            sumX += xValues[i];
            sumY += yValues[i];
            sumXY += xValues[i] * yValues[i];
            sumX2 += xValues[i] * xValues[i];
        }

        const meanX = sumX / n;
        const meanY = sumY / n;

        const slope = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
        const intercept = meanY - slope * meanX;

        return Operand.Create(intercept + slope * x);
    }
}

export { Function_FORECAST };
