import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_TIMESTAMP extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        let args0 = this.func1.Evaluate(engine, tempParameter);
        if (args0.IsError) { return args0; }

        let Type = 0; // 毫秒
        if (this.func2 !== null) {
            let args2 = this.func2.Evaluate(engine, tempParameter);
            if (args2.IsNotNumber) {
                args2 = args2.ToNumber(StringCache.Function_parameter_error, "TimeStamp", 2);
                if (args2.IsError) { return args2; }
            }
            Type = args2.IntValue;
        }

        let milliseconds;
        if (args0.IsText) {
            // 直接解析字符串为Date对象
            let dateStr = args0.TextValue;
            // 处理不同格式的日期字符串
            let date;
            
            // 尝试解析为YYYY-MM-DD格式
            if (/^\d{4}-\d{2}-\d{2}$/.test(dateStr)) {
                let parts = dateStr.split('-');
                let year = parseInt(parts[0]);
                let month = parseInt(parts[1]) - 1; // 月份�?开�?
                let day = parseInt(parts[2]);
                // 创建本地时间
                date = new Date(year, month, day, 0, 0, 0, 0);
            }
            // 尝试解析为YYYY/MM/DD格式
            else if (/^\d{4}\/\d{2}\/\d{2}$/.test(dateStr)) {
                let parts = dateStr.split('/');
                let year = parseInt(parts[0]);
                let month = parseInt(parts[1]) - 1; // 月份�?开�?
                let day = parseInt(parts[2]);
                // 创建本地时间
                date = new Date(year, month, day, 0, 0, 0, 0);
            }
            // 尝试解析为其他格�?
            else {
                date = new Date(dateStr);
            }
            
            // 检查是否解析成�?
            if (isNaN(date.getTime())) {
                // 解析失败
                return Operand.Error(StringCache.Function_parameter_error, "TimeStamp", 1);
            }
            
            milliseconds = date.getTime();
        } else if (args0.IsDate) {
            // 使用MyDate的ToDateTime方法获取Date对象
            let date = args0.DateValue.ToDateTime();
            milliseconds = date.getTime();
        } else {
            // 尝试转换为MyDate
            let args1 = args0.ToMyDate(StringCache.Function_parameter_error, "TimeStamp", 1);
            if (args1.IsError) { return args1; }
            let date = args1.DateValue.ToDateTime();
            milliseconds = date.getTime();
        }

        if (Type == 0) {
            // 毫秒时间�?
            return Operand.Create(milliseconds);
        } else if (Type == 1) {
            // 秒时间戳
            return Operand.Create(Math.floor(milliseconds / 1000));
        }
        return Operand.Error(StringCache.Function_parameter_error, "TimeStamp");
    }
}

export { Function_TIMESTAMP };

