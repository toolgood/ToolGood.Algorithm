import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_SLN extends Function_3 {
    get Name() {
        return "SLN";
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

        if (life == 0) return this.div0Error();

        return Operand.Create((cost - salvage) / life);
    }
}

export { Function_SLN };