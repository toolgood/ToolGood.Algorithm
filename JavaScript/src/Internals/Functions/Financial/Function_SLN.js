import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_SLN extends Function_N {
    get Name() {
        return "SLN";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        if (this.z.length < 3) return this.parameterError(1);

        const costArg = this.getNumber(engine, tempParameter, 0);
        if (costArg.IsError) return costArg;
        const cost = costArg.NumberValue;

        const salvageArg = this.getNumber(engine, tempParameter, 1);
        if (salvageArg.IsError) return salvageArg;
        const salvage = salvageArg.NumberValue;

        const lifeArg = this.getNumber(engine, tempParameter, 2);
        if (lifeArg.IsError) return lifeArg;
        const life = lifeArg.NumberValue;

        const sln = (cost - salvage) / life;
        return Operand.Create(sln);
    }
}

export { Function_SLN };
