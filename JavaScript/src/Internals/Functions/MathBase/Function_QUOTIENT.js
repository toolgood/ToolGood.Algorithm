import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_QUOTIENT extends Function_2 {
    get Name() {
        return "Quotient";
    }

    constructor(z) {
    super(z);
  }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsError) { return args2; }

        if (args2.NumberValue == 0) {
            return this.div0Error();
        }
        // 商超 int 范围时返回错误,避免异常穿透
        let result = args1.NumberValue / args2.NumberValue;
        if (result < -2147483648 || result > 2147483647) {
            return this.functionError();
        }
        return Operand.Create(Math.trunc(result));
    }
}

export { Function_QUOTIENT };

