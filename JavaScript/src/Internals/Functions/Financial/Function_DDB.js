import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_DDB extends Function_N {
    get Name() {
        return "DDB";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        if (this.z.length < 4) return this.parameterError(1);

        const costArg = this.getNumber(engine, tempParameter, 0);
        if (costArg.IsError) return costArg;
        let cost = costArg.NumberValue;

        const salvageArg = this.getNumber(engine, tempParameter, 1);
        if (salvageArg.IsError) return salvageArg;
        const salvage = salvageArg.NumberValue;

        const lifeArg = this.getNumber(engine, tempParameter, 2);
        if (lifeArg.IsError) return lifeArg;
        const life = lifeArg.NumberValue;

        const perArg = this.getNumber(engine, tempParameter, 3);
        if (perArg.IsError) return perArg;
        const per = perArg.NumberValue;

        let factor = 2;
        if (this.z.length > 4) {
            const factorArg = this.getNumber(engine, tempParameter, 4);
            if (factorArg.IsError) return factorArg;
            factor = factorArg.NumberValue;
        }

        if (life === 0 || factor === 0) return this.div0Error();
        if (per < 1 || per > life) return this.parameterError(4);
        if (life < 1) return this.parameterError(3);

        let depreciation = 0;
        let remainingCost = cost;

        for (let i = 1; i <= per; i++) {
            let ddb = remainingCost * factor / life;
            const maxDepreciation = remainingCost - salvage;
            if (ddb > maxDepreciation) {
                ddb = maxDepreciation;
            }
            if (i === per) {
                depreciation = ddb;
            }
            remainingCost -= ddb;
            if (remainingCost <= salvage) {
                if (i === per) {
                    depreciation = remainingCost + ddb - salvage;
                }
                break;
            }
        }

        return Operand.Create(depreciation);
    }
}

export { Function_DDB };
