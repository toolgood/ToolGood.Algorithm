import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_POWER extends Function_2 {
    get Name() {
        return "Power";
    }

    constructor(z) {
    super(z);
  }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsError) { return args2; }

        let baseValue = args1.NumberValue;
        let exponent = args2.NumberValue;

        if (baseValue == 0 && exponent < 0) {
            return this.div0Error();
        }
        if (baseValue < 0 && exponent % 1 != 0) {
            return this.parameterError(1);
        }
        let result = Math.pow(baseValue, exponent);
        if (!isFinite(result)) {
            return this.functionError();
        }
        return Operand.Create(result);
    }
}

export { Function_POWER };

