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
        if (life <= 0) return this.parameterError(3);
        if (per < 1 || per > life) return this.parameterError(4);

        let depreciation = 0;
        let remainingCost = cost;

        // 累计 per 之前各期的折旧
        for (let i = 1; i < per; i++) {
            let ddb = remainingCost * factor / life;
            const maxDepreciation = remainingCost - salvage;
            if (ddb > maxDepreciation) {
                ddb = maxDepreciation;
            }
            remainingCost -= ddb;
            if (remainingCost <= salvage) {
                break;
            }
        }

        // 计算当前期间(per)的折旧, per 可为小数
        depreciation = remainingCost * factor / life;
        const maxDep = remainingCost - salvage;
        if (depreciation > maxDep) {
            depreciation = maxDep;
        }

        return Operand.Create(depreciation);
    }
}

export { Function_DDB };
