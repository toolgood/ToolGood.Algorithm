import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_FISHERINV extends Function_1 {
    get Name() {
        return "FisherInv";
    }

    constructor(a) {
        super(a);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let x = args1.DoubleValue;
        let exp = Math.exp(2 * x);
        if (!isFinite(exp)) {
            // 对齐 C# decimal 溢出处理:大正数返回 1
            return Operand.Create(1);
        }
        let n = (exp - 1) / (exp + 1);
        return Operand.Create(n);
    }
}

export { Function_FISHERINV };

