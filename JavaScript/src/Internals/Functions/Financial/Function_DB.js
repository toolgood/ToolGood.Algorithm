import { Function_5 } from '../Function_5.js';
import { Operand } from '../../../Operand.js';

class Function_DB extends Function_5 {
    get Name() {
        return "DB";
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

        let month = 12;
        if (this.func5 != null) {
            let monthArg = this.getNumber_5(engine, tempParameter);
            if (monthArg.IsError) return monthArg;
            month = monthArg.IntValue;
            if (month < 1 || month > 12) {
                return this.parameterError(5);
            }
        }

        if (life == 0 || cost == 0) return this.div0Error();
        if (period < 1 || period > life) {
            return this.parameterError(4);
        }
        if (life < 1) {
            return this.parameterError(3);
        }

        let rate = 1 - Math.pow((salvage / cost), 1.0 / life);
        rate = Math.round(rate * 1000) / 1000;

        let depreciation = 0;
        if (period == 1) {
            depreciation = cost * rate * month / 12;
        } else if (Math.floor(period) == Math.floor(life)) {
            let remainingCost = cost;
            for (let i = 1; i < life; i++) {
                remainingCost -= depreciation;
                if (i == 1) {
                    depreciation = cost * rate * month / 12;
                } else if (i < life) {
                    depreciation = remainingCost * rate;
                }
            }
            remainingCost -= depreciation;
            depreciation = remainingCost * rate * (12 - month) / 12;
        } else {
            let remainingCost = cost;
            for (let i = 1; i <= period; i++) {
                if (i == 1) {
                    depreciation = cost * rate * month / 12;
                } else {
                    remainingCost -= depreciation;
                    depreciation = remainingCost * rate;
                }
            }
        }

        return Operand.Create(depreciation);
    }
}

export { Function_DB };