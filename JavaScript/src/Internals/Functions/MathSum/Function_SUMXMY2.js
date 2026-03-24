import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_SUMXMY2 extends Function_N {
    get Name() {
        return "SUMXMY2";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        if (this.z.length < 2) return this.parameterError(1);

        let arrayXArg = this.getArray(engine, tempParameter, 0);
        if (arrayXArg.IsError) return arrayXArg;
        let arrayX = [];
        for (let item of arrayXArg.ArrayValue) {
            if (item.IsNumber) {
                arrayX.push(item.NumberValue);
            }
        }

        let arrayYArg = this.getArray(engine, tempParameter, 1);
        if (arrayYArg.IsError) return arrayYArg;
        let arrayY = [];
        for (let item of arrayYArg.ArrayValue) {
            if (item.IsNumber) {
                arrayY.push(item.NumberValue);
            }
        }

        let minLength = arrayX.length < arrayY.length ? arrayX.length : arrayY.length;

        let result = 0;
        for (let i = 0; i < minLength; i++) {
            let diff = arrayX[i] - arrayY[i];
            result += diff * diff;
        }

        return Operand.Create(result);
    }
}

export { Function_SUMXMY2 };