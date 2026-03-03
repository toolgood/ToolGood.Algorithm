import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_SWITCH extends Function_N {
    get Name() {
        return 'Switch';
    }

    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        let exprValue = this.z[0].evaluate(engine, tempParameter);
        if (exprValue.IsError) { return exprValue; }

        let i = 1;
        while (i < this.z.length - 1) {
            let compareValue = this.z[i].evaluate(engine, tempParameter);
            if (compareValue.IsError) { return compareValue; }

            if (this.equalsOperand(exprValue, compareValue)) {
                return this.z[i + 1].evaluate(engine, tempParameter);
            }
            i += 2;
        }
        return this.functionError();
    }

    equalsOperand(a, b) {
        if (a.IsNumber && b.IsNumber) {
            return a.NumberValue === b.NumberValue;
        }
        if (a.IsText && b.IsText) {
            return a.TextValue === b.TextValue;
        }
        if (a.IsBoolean && b.IsBoolean) {
            return a.BooleanValue === b.BooleanValue;
        }
        if (a.IsNull && b.IsNull) {
            return true;
        }
        return false;
    }
}

export { Function_SWITCH };
