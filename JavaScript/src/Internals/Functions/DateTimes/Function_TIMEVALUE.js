import { Function_1 } from '../Function_1';
import { MyDate } from '../../MyDate';

class Function_TIMEVALUE extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    evaluate(engine, tempParameter) {
        let args1 = this._arg1.evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText("Function '{0}' parameter is error!", "TimeValue");
            if (args1.IsError) { return args1; }
        }

        try {
            // 尝试解析时间字符串
            const timeStr = args1.TextValue;
            const parts = timeStr.split(':');
            
            if (parts.length >= 2) {
                let hours = parseInt(parts[0], 10);
                let minutes = parseInt(parts[1], 10);
                let seconds = 0;
                
                if (parts.length >= 3) {
                    seconds = parseInt(parts[2], 10);
                }
                
                // 创建一个时间对象（日期部分设为0）
                const timeDate = new MyDate(0, 0, 0, hours, minutes, seconds);
                return engine.createOperand(timeDate);
            }
        } catch (e) {
            // 解析失败
        }
        return engine.createErrorOperand("Function '{0}' parameter is error!", "TimeValue");
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "TimeValue");
    }
}

export { Function_TIMEVALUE };
