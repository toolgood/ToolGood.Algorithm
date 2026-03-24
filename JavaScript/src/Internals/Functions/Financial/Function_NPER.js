import { Function_5 } from '../Function_5.js';
import { Operand } from '../../../Operand.js';

class Function_NPER extends Function_5 {
    get Name() {
        return "NPER";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        let rateArg = this.getNumber_1(engine, tempParameter);
        if (rateArg.IsError) return rateArg;
        let rate = rateArg.NumberValue;

        let pmtArg = this.getNumber_2(engine, tempParameter);
        if (pmtArg.IsError) return pmtArg;
        let pmt = pmtArg.NumberValue;

        let pvArg = this.getNumber_3(engine, tempParameter);
        if (pvArg.IsError) return pvArg;
        let pv = pvArg.NumberValue;

        let fv = 0;
        if (this.func4 != null) {
            let fvArg = this.getNumber_4(engine, tempParameter);
            if (fvArg.IsError) return fvArg;
            fv = fvArg.NumberValue;
        }

        let type = 0;
        if (this.func5 != null) {
            let typeArg = this.getNumber_5(engine, tempParameter);
            if (typeArg.IsError) return typeArg;
            type = typeArg.IntValue;
        }

        if (rate == 0) {
            if (pmt == 0) return this.div0Error();
            return Operand.Create(-(pv + fv) / pmt);
        }
        if (rate == -1) {
            return this.div0Error();
        }

        let factor = pmt;
        if (type == 1) {
            factor = pmt * (1 + rate);
        }

        let nper = Math.log((-fv * rate + factor) / (pv * rate + factor)) / Math.log((1 + rate));
        return Operand.Create(nper);
    }
}

export { Function_NPER };