import { Function_5 } from '../Function_5.js';
import { Operand } from '../../../Operand.js';

class Function_DDB extends Function_5 {
    get Name() {
        return "DDB";
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

        let factor = 2;
        if (this.func5 != null) {
            let factorArg = this.getNumber_5(engine, tempParameter);
            if (factorArg.IsError) return factorArg;
            factor = factorArg.NumberValue;
        }

        if (life == 0 || factor == 0) return this.div0Error();
        if (period < 1 || period > life) {
            return this.parameterError(4);
        }
        if (life < 1) {
            return this.parameterError(3);
        }

        let depreciation = 0;
        let remainingCost = cost;

        for (let i = 1; i <= period; i++) {
            let ddb = remainingCost * factor / life;
            let maxDepreciation = remainingCost - salvage;
            if (ddb > maxDepreciation) {
                ddb = maxDepreciation;
            }
            if (i == period) {
                depreciation = ddb;
            }
            remainingCost -= ddb;
            if (remainingCost <= salvage) {
                if (i == period) {
                    depreciation = remainingCost + ddb - salvage;
                }
                break;
            }
        }

        return Operand.Create(depreciation);
    }
}

export { Function_DDB };