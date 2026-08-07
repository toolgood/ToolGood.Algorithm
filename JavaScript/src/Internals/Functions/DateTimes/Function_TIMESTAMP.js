import { Function_2 } from '../Function_2.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { Operand } from '../../../Operand.js';

class Function_TIMESTAMP extends Function_2 {
    get Name() {
        return "Timestamp";
    }

    constructor(z) {
        super(z);
    }

    evaluate(work, tempParameter) {
        let type = 0; // 毫秒
        if(this.b != null) {
            let args2 = this.getNumber_2(work, tempParameter);
            if(args2.IsError) { return args2; }
            type = args2.IntValue;
        }
        let args0 = this.getDate_1(work, tempParameter);
        if(args0.IsError) { return args0; }
      
        let ms;
        if (work.UseLocalTime) {
            // 日期按本地时间解释后转 UTC 纪元毫秒（Date.valueOf() 即 UTC 毫秒）
            ms = args0.DateValue.ToDateTime().valueOf();
        } else {
            // 日期直接按 UTC 解释
            ms = args0.DateValue.ToDateTime(0).valueOf();
        }
        if (type === 0) return Operand.Create(ms);
        if (type === 1) return Operand.Create(ms / 1000);
        return this.parameterError(2);
    }
}

export { Function_TIMESTAMP };
