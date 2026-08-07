import { Function_1 } from '../Function_1.js';
import { MyDate } from '../../MyDate.js';

import { Operand } from '../../../Operand.js';

class Function_TIMEVALUE extends Function_1 {
    get Name() {
        return "TimeValue";
    }

    constructor(a) {
        super(a);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getText_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        // 支持 [d.]hh:mm[:ss[.fff]] 及可选负号，分钟/秒须为 0-59（与 C# TimeSpan.TryParse 一致）
        let timeStr = args1.TextValue.trim();
        let match = timeStr.match(/^([+-]?)(?:(\d+)\.)?(\d+):(\d{1,2})(?::(\d{1,2})(?:\.(\d+))?)?$/);
        if (match) {
            let sign = match[1] === '-' ? -1 : 1;
            let days = match[2] ? parseInt(match[2], 10) : 0;
            let hours = parseInt(match[3], 10);
            let minutes = parseInt(match[4], 10);
            let seconds = match[5] ? parseInt(match[5], 10) : 0;
            if (minutes > 59 || seconds > 59) {
                return this.parameterError(1);
            }
            // 小数秒：MyDate 无毫秒字段，按总秒数四舍五入后规范化到 天/时/分/秒
            let frac = match[6] !== undefined ? parseFloat('0.' + match[6]) : 0;
            let totalSeconds = Math.round((((days * 24 + hours) * 60 + minutes) * 60 + seconds) + frac);
            let totalMinutes = Math.trunc(totalSeconds / 60);
            let sec = totalSeconds % 60;
            let totalHours = Math.trunc(totalMinutes / 60);
            let min = totalMinutes % 60;
            let totalDays = Math.trunc(totalHours / 24);
            let hr = totalHours % 24;
            let timeDate = new MyDate(null, null, sign * totalDays, sign * hr, sign * min, sign * sec);
            return Operand.Create(timeDate);
        }
        return this.parameterError(1);
    }
}

export { Function_TIMEVALUE };

