import { Function_5 } from '../Function_5.js';
import { Operand } from '../../../Operand.js';

class Function_FV extends Function_5 {
    get Name() {
        return "FV";
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

        let pmtArg = this.getNumber_3(engine, tempParameter);
        if (pmtArg.IsError) return pmtArg;
        let pmt = pmtArg.NumberValue;

        let pv = 0;
        if (this.func4 != null) {
            let pvArg = this.getNumber_4(engine, tempParameter);
            if (pvArg.IsError) return pvArg;
            pv = pvArg.NumberValue;
        }

        let type = 0;
        if (this.func5 != null) {
            let typeArg = this.getNumber_5(engine, tempParameter);
            if (typeArg.IsError) return typeArg;
            type = typeArg.IntValue;
            if (type != 0 && type != 1) {
                return this.parameterError(5);
            }
        }

        if (rate == 0) {
            return Operand.Create(-pmt * nper - pv);
        }

        let factor = Math.pow((1 + rate), nper);
        let fv = -pv * factor - pmt * (factor - 1) / rate;
        if (type == 1) {
            fv = -pv * factor - pmt * (1 + rate) * (factor - 1) / rate;
        }

        return Operand.Create(fv);
    }
}

export { Function_FV };