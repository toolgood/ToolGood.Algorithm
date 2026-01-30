import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_TEXT extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsError) {
            return args1;
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotText) {
            args2 = args2.ToText(StringCache.Function_parameter_error, 'Text', 2);
            if (args2.IsError) {
                return args2;
            }
        }

        if (args1.IsText) {
            return args1;
        } else if (args1.IsBoolean) {
            return Operand.Create(args1.BooleanValue ? 'TRUE' : 'FALSE');
        } else if (args1.IsNumber) {
            // 实现基本的数字格式化逻辑
            const format = args2.TextValue;
            const value = args1.NumberValue;
            
            // 处理简单的数字格式，如 "0.00"
            if (format.includes('0')) {
                // 计算小数位数
                const decimalIndex = format.indexOf('.');
                if (decimalIndex !== -1) {
                    const decimalPlaces = format.substring(decimalIndex + 1).length;
                    return Operand.Create(value.toFixed(decimalPlaces));
                } else {
                    // 没有小数部分，返回整�?
                    return Operand.Create(Math.round(value).toString());
                }
            }
            
            // 如果没有匹配的格式，使用默认的toString
            return Operand.Create(value.toString());
        } else if (args1.IsDate) {
            // 同样，日期格式化可能需要更复杂的处�?
            return Operand.Create(args1.DateValue.toString());
        }
        let args1Text = args1.ToText(StringCache.Function_parameter_error, 'Text', 1);
        if (args1Text.IsError) {
            return args1Text;
        }
        return Operand.Create(args1Text.TextValue.toString());
    }
}

export { Function_TEXT };

