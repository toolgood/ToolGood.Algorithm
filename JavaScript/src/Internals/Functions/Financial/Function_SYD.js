import { Function_4 } from '../Function_4.js';
import { Operand } from '../../../Operand.js';

class Function_SYD extends Function_4 {
    get Name() {
        return "SYD";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        let costArg = this.getNumber_1(engine, tempParameter);
        if (costArg.IsError) return costArg;
        let cost = costArg.NumberValue;

        let salvageArg = this.getNumber_2(engine, tempParameter);
        if (salvageArg.IsError) return salvageArg;
        let salvage = salvageArg.NumberValue;

        let lifeArg = this.getNumber_3(engine, tempParameter);
        if (lifeArg.IsError) return lifeArg;
        let life = lifeArg.NumberValue;

        let periodArg = this.getNumber_4(engine, tempParameter);
        if (periodArg.IsError) return periodArg;
        let period = periodArg.NumberValue;

        if (life == 0) return this.div0Error();
        if (period < 1 || period > life) {
            return this.parameterError(4);
        }
        if (life < 1) {
            return this.parameterError(3);
        }

        let syd = (cost - salvage) * (life - period + 1) * 2 / (life * (life + 1));
        return Operand.Create(syd);
    }
}

export { Function_SYD };