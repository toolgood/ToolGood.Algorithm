import { Function_2 } from '../Function_2.js';
import { MyDate } from '../../MyDate.js';
import { Operand } from '../../../Operand.js';


class Function_ADDDAYS extends Function_2 {
    get Name() {
        return "AddDays";
    }

    constructor(z) {
    super(z);
  }

    evaluate(engine, tempParameter) {
        let args1 = this.getDate_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsError) { return args2; }
        // 与 C# AddDays(args2.IntValue) 一致：整数天，且复用 MyDate.AddDays 以正确处理纯时间
        return Operand.Create(args1.DateValue.AddDays(args2.IntValue));
    }
}

export { Function_ADDDAYS };

