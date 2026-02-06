import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_TEXT extends Function_2 {
    get Name() {
        return "Text";
    }

    constructor(z) {
        super(z);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.a.Evaluate(work, tempParameter);
        if (args1.IsError) {
            return args1;
        }
        let args2 = this.GetText_2(work, tempParameter);
        if (args2.IsError) {
            return args2;
        }

        if (args1.IsText) {
            return args1;
        } else if (args1.IsBoolean) {
            return Operand.Create(args1.BooleanValue ? 'TRUE' : 'FALSE');
        } else if (args1.IsNumber) {
            // 实现基本的数字格式化逻辑
            let format = args2.TextValue;
            let value = args1.NumberValue;
            
            // 处理简单的数字格式，如 "0.00"
            if (format.includes('0')) {
                // 计算小数位数
                let decimalIndex = format.indexOf('.');
                if (decimalIndex !== -1) {
                    let decimalPlaces = format.substring(decimalIndex + 1).length;
                    return Operand.Create(value.toFixed(decimalPlaces));
                } else {
                    // 没有小数部分，返回整数
                    return Operand.Create(Math.round(value).toString());
                }
            }
            
            // 如果没有匹配的格式，使用默认的toString
            return Operand.Create(value.toString());
        } else if (args1.IsDate) {
            // 同样，日期格式化可能需要更复杂的处理
            return Operand.Create(args1.DateValue.toString());
        }
        let args1Text = args1.ToText(this.FunctionError, 'Text', 1);
        if (args1Text.IsError) {
            return args1Text;
        }
        return Operand.Create(args1Text.TextValue.toString());
    }
}

export { Function_TEXT };