import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_FIXED extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(engine, tempParameter) {
        let num = 2;
        if (this.func2 !== null) {
            let args2 = this.func2.Evaluate(engine, tempParameter);
            if (args2.IsNotNumber) {
                args2 = args2.ToNumber(StringCache.Function_parameter_error, "Fixed", 2);
                if (args2.IsError) { return args2; }
            }
            num = args2.IntValue;
        }
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber(StringCache.Function_parameter_error, "Fixed", 1);
            if (args1.IsError) { return args1; }
        }

        // 四舍五入到指定小数位�?
        let s = Math.round(args1.NumberValue * Math.pow(10, num)) / Math.pow(10, num);
        let no = false;
        if (this.func3 !== null) {
            let args3 = this.func3.Evaluate(engine, tempParameter);
            if (args3.IsNotBoolean) {
                args3 = args3.ToBoolean(StringCache.Function_parameter_error, "Fixed", 3);
                if (args3.IsError) { return args3; }
            }
            no = args3.BooleanValue;
        }
        if (no === false) {
            // 格式化数字，保留指定小数位数并添加千位分隔符
            let formatted = s.toFixed(num);
            // 添加千位分隔�?
            let parts = formatted.split('.');
            parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
            return Operand.Create(parts.join('.'));
        }
        return Operand.Create(s.toString());
    }
}

export { Function_FIXED };

