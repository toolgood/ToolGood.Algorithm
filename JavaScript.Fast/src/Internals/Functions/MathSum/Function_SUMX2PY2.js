import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_SUMX2PY2 extends Function_N {
    get Name() {
        return "SUMX2PY2";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        if (this.z.length < 2) return this.parameterError(1);

        const array1Arg = this.getArray(engine, tempParameter, 0);
        if (array1Arg.IsError) return array1Arg;
        const xValues = [];
        for (const item of array1Arg.ArrayValue) {
            if (item.IsNumber) xValues.push(item.NumberValue);
        }

        const array2Arg = this.getArray(engine, tempParameter, 1);
        if (array2Arg.IsError) return array2Arg;
        const yValues = [];
        for (const item of array2Arg.ArrayValue) {
            if (item.IsNumber) yValues.push(item.NumberValue);
        }

        const minLength = Math.min(xValues.length, yValues.length);
        let result = 0;

        for (let i = 0; i < minLength; i++) {
            result += xValues[i] * xValues[i] + yValues[i] * yValues[i];
        }

        return Operand.Create(result);
    }
}

export { Function_SUMX2PY2 };
