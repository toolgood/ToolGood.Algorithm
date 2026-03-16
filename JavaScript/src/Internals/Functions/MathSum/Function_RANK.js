import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_RANK extends Function_N {
    get Name() {
        return "RANK";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        if (this.z.length < 2) return this.parameterError(1);

        const numArg = this.getNumber(engine, tempParameter, 0);
        if (numArg.IsError) return numArg;
        const num = numArg.NumberValue;

        const arrayArg = this.getArray(engine, tempParameter, 1);
        if (arrayArg.IsError) return arrayArg;

        let order = 0;
        if (this.z.length > 2) {
            const orderArg = this.getNumber(engine, tempParameter, 2);
            if (orderArg.IsError) return orderArg;
            order = Math.floor(orderArg.NumberValue);
        }

        const values = [];
        for (const item of arrayArg.ArrayValue) {
            if (item.IsNumber) {
                values.push(item.NumberValue);
            }
        }

        values.sort((a, b) => a - b);
        let rank;
        if (order === 0) {
            values.reverse();
            rank = values.indexOf(num) + 1;
        } else {
            rank = values.indexOf(num) + 1;
        }

        return Operand.Create(rank);
    }
}

export { Function_RANK };
