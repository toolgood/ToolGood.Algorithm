import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_SYD extends Function_N {
    get Name() {
        return "SYD";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        if (this.z.length < 4) return this.parameterError(1);

        const costArg = this.getNumber(engine, tempParameter, 0);
        if (costArg.IsError) return costArg;
        const cost = costArg.NumberValue;

        const salvageArg = this.getNumber(engine, tempParameter, 1);
        if (salvageArg.IsError) return salvageArg;
        const salvage = salvageArg.NumberValue;

        const lifeArg = this.getNumber(engine, tempParameter, 2);
        if (lifeArg.IsError) return lifeArg;
        const life = lifeArg.NumberValue;

        const perArg = this.getNumber(engine, tempParameter, 3);
        if (perArg.IsError) return perArg;
        const per = perArg.NumberValue;

        const syd = (cost - salvage) * (life - per + 1) * 2 / (life * (life + 1));
        return Operand.Create(syd);
    }
}

export { Function_SYD };
