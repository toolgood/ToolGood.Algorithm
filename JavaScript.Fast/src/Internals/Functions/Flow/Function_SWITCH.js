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
        // 参数数量为偶数时，最后一个参数是默认值
        if (this.z.length % 2 === 0) {
            return this.z[this.z.length - 1].evaluate(engine, tempParameter);
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
        if (a.IsDate && b.IsDate) {
            return a.DateValue.ToLong() === b.DateValue.ToLong();
        }
        if (a.IsNull && b.IsNull) {
            return true;
        }
        if (a.IsNull || b.IsNull) {
            return false;
        }
        // 跨类型标量比较：与 == 运算符保持一致，统一转数字后再比较。
        // 日期/数组/Json 不参与跨类型比较（== 对这类组合返回 CompareError，此处视为不匹配）；
        // 无法转成数字的跨类型组合同样视为不匹配。
        if (a.IsDate || b.IsDate || a.IsArray || b.IsArray || a.IsJson || b.IsJson || a.IsArrayJson || b.IsArrayJson) {
            return false;
        }
        let numberA = a.ToNumber(null);
        if (numberA.IsErrorOrNone) { return false; }
        let numberB = b.ToNumber(null);
        if (numberB.IsErrorOrNone) { return false; }
        return numberA.NumberValue === numberB.NumberValue;
    }
}

export { Function_SWITCH };
