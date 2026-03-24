import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_RANK extends Function_N {
    get Name() {
        return "RANK";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        if (this.z.length < 2) return this.parameterError(1);

        let numArg = this.getNumber(engine, tempParameter, 0);
        if (numArg.IsError) return numArg;
        let num = numArg.NumberValue;

        let arrayArg = this.getArray(engine, tempParameter, 1);
        if (arrayArg.IsError) return arrayArg;

        let order = 0;
        if (this.z.length > 2) {
            let orderArg = this.getNumber(engine, tempParameter, 2);
            if (orderArg.IsError) return orderArg;
            order = orderArg.IntValue;
        }

        let values = [];
        for (let item of arrayArg.ArrayValue) {
            if (item.IsNumber) {
                values.push(item.NumberValue);
            }
        }

        if (values.length == 0) {
            return this.parameterError(2);
        }

        let descending = (order == 0);
        let rank = FunctionUtil.GetRank(values, num, descending);

        if (rank == 0) {
            return this.parameterError(1);
        }

        return Operand.Create(rank);
    }
}

export { Function_RANK };