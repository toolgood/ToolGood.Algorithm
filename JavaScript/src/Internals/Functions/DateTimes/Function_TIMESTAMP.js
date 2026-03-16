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
      
        let ms=args0.DateValue.ToDateTime().valueOf()
        if(work.UseLocalTime) {
            ms=ms+new Date(1970, 0, 1, 0, 0, 0, 0).valueOf()
        } 
        if(type === 0) return Operand.Create(ms);
        return Operand.Create(ms/1000);
    }
}

export { Function_TIMESTAMP };
