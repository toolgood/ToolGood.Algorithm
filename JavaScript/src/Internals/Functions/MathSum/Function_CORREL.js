import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_CORREL extends Function_N {
    get Name() {
        return "CORREL";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        if (this.z.length < 2) return this.parameterError(1);

        let array1Arg = this.getArray(engine, tempParameter, 0);
        if (array1Arg.IsError) return array1Arg;

        let array2Arg = this.getArray(engine, tempParameter, 1);
        if (array2Arg.IsError) return array2Arg;

        let xValues = [];
        for (let item of array1Arg.ArrayValue) {
            if (item.IsNumber) xValues.push(item.NumberValue);
        }

        let yValues = [];
        for (let item of array2Arg.ArrayValue) {
            if (item.IsNumber) yValues.push(item.NumberValue);
        }

        if (xValues.length != yValues.length || xValues.length < 2) return this.functionError();

        let n = xValues.length;
        let sumX = 0, sumY = 0;

        for (let i = 0; i < n; i++) {
            sumX += xValues[i];
            sumY += yValues[i];
        }

        let meanX = sumX / n;
        let meanY = sumY / n;

        let numerator = 0, denomX = 0, denomY = 0;

        for (let i = 0; i < n; i++) {
            let dx = xValues[i] - meanX;
            let dy = yValues[i] - meanY;
            numerator += dx * dy;
            denomX += dx * dx;
            denomY += dy * dy;
        }

        if (denomX == 0 || denomY == 0) return this.div0Error();

        return Operand.Create(numerator / Math.sqrt((denomX * denomY)));
    }
}

export { Function_CORREL };