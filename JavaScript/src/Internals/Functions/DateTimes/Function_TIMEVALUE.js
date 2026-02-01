import { Function_1 } from '../Function_1.js';
import { MyDate } from '../../MyDate.js';
import { StringCache } from '../../../Internals/StringCache.js';
import { Operand } from '../../../Operand.js';

class Function_TIMEVALUE extends Function_1 {
    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            args1 = args1.ToText(StringCache.Function_parameter_error, "TimeValue");
            if (args1.IsError) { return args1; }

        try {
            // 尝试解析时间字符�?
            let timeStr = args1.TextValue;
            let parts = timeStr.split(':');
            
            if (parts.length >= 2) {
                let hours = parseInt(parts[0], 10);
                let minutes = parseInt(parts[1], 10);
                let seconds = 0;
                
                if (parts.length >= 3) {
                    seconds = parseInt(parts[2], 10);
                }
                
                // 创建一个时间对象（日期部分设为0�?
                let timeDate = new MyDate(0, 0, 0, hours, minutes, seconds);
                return Operand.Create(timeDate);
            }
        } catch (e) {
            // 解析失败
        }
        return Operand.Error(StringCache.Function_parameter_error, "TimeValue");
    }
}

export { Function_TIMEVALUE };

