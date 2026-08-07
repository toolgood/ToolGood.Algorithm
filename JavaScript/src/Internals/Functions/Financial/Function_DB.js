import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_DB extends Function_N {
    get Name() {
        return "DB";
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

        let month = 12;
        if (this.z.length > 4) {
            const monthArg = this.getNumber(engine, tempParameter, 4);
            if (monthArg.IsError) return monthArg;
            month = Math.floor(monthArg.NumberValue);
            if (month < 1 || month > 12) return this.parameterError(5);
        }

        if (life === 0 || cost === 0) return this.div0Error();

        // Excel: month<12 时折旧跨越 life+1 个期间(第1年部分月 + life-1 个整年 + 最后部分月)
        const totalPeriods = (month === 12) ? Math.floor(life) : Math.floor(life) + 1;
        if (per < 1 || per > totalPeriods) return this.parameterError(4);
        if (life < 1) return this.parameterError(3);

        // Excel 的 DB 折旧率保留 3 位小数
        const rate = Math.round((1 - Math.pow(salvage / cost, 1 / life)) * 1000) / 1000;
        let db = 0;
        let remainingValue = cost;

        for (let i = 1; i <= per; i++) {
            if (i === 1) {
                db = cost * rate * month / 12;
            } else if (i === totalPeriods && month !== 12) {
                db = remainingValue * rate * (12 - month) / 12;
            } else {
                db = remainingValue * rate;
            }
            remainingValue -= db;
        }

        return Operand.Create(db);
    }
}

export { Function_DB };
