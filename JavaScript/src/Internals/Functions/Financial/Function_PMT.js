import { Function_5 } from '../Function_5.js';
import { Operand } from '../../../Operand.js';

class Function_PMT extends Function_5 {
    get Name() {
        return "PMT";
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        let rateArg = this.getNumber_1(engine, tempParameter);
        if (rateArg.IsError) return rateArg;
        let rate = rateArg.NumberValue;

        let nperArg = this.getNumber_2(engine, tempParameter);
        if (nperArg.IsError) return nperArg;
        let nper = nperArg.NumberValue;

        let pvArg = this.getNumber_3(engine, tempParameter);
        if (pvArg.IsError) return pvArg;
        let pv = pvArg.NumberValue;

        if (nper == 0) {
            return this.div0Error();
        }

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
            return Operand.Create(-(pv + fv) / nper);
        }

        let factor = Math.pow((1 + rate), nper);
        let pmt = -(pv * factor + fv) * rate / (factor - 1);
        if (type == 1) {
            pmt = pmt / (1 + rate);
        }

        return Operand.Create(pmt);
    }
}

export { Function_PMT };