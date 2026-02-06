import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_FIXED extends Function_3 {
    get Name() {
        return "Fixed";
    }

    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let num = 2;
        if (this.b !== null) {
            let args2 = this.GetNumber_2(engine, tempParameter);
            if (args2.IsError) { return args2; }
            num = args2.IntValue;
        }
        let args1 = this.GetNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        // 四舍五入到指定小数位
        let s = Math.round(args1.NumberValue * Math.pow(10, num)) / Math.pow(10, num);
        let no = false;
        if (this.c !== null) {
            let args3 = this.GetBoolean_3(engine, tempParameter);
            if (args3.IsError) { return args3; }
            no = args3.BooleanValue;
        }
        if (no === false) {
            // 格式化数字，保留指定小数位数并添加千位分隔符
            let formatted = s.toFixed(num);
            // 添加千位分隔符
            let parts = formatted.split('.');
            parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
            return Operand.Create(parts.join('.'));
        }
        return Operand.Create(s.toString());
    }
}

export { Function_FIXED };

