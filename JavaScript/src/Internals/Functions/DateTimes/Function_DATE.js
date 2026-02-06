import { Function_N } from '../Function_N.js';
import { MyDate } from '../../MyDate.js';
import { Operand } from '../../../Operand.js';


class Function_DATE extends Function_N {
    get Name() {
        return "Date";
    }

    constructor(z) {
        super(z);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.GetNumber(engine, tempParameter, 0);
        if (args1.IsError) { return args1; }

        let args2 = this.GetNumber(engine, tempParameter, 1);
        if (args2.IsError) { return args2; }

        let args3 = this.GetNumber(engine, tempParameter, 2);
        if (args3.IsError) { return args3; }

        let d;
        if (this.z.length == 3) {
            d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, 0, 0, 0);
        } else if (this.z.length == 4) {
            let args4 = this.GetNumber(engine, tempParameter, 3);
            if (args4.IsError) { return args4; }

            d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, args4.IntValue, 0, 0);
        } else if (this.z.length == 5) {
            let args4 = this.GetNumber(engine, tempParameter, 3);
            if (args4.IsError) { return args4; }

            let args5 = this.GetNumber(engine, tempParameter, 4);
            if (args5.IsError) { return args5; }

            d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, args4.IntValue, args5.IntValue, 0);
        } else {
            let args4 = this.GetNumber(engine, tempParameter, 3);
            if (args4.IsError) { return args4; }

            let args5 = this.GetNumber(engine, tempParameter, 4);
            if (args5.IsError) { return args5; }

            let args6 = this.GetNumber(engine, tempParameter, 5);
            if (args6.IsError) { return args6; }

            d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, args4.IntValue, args5.IntValue, args6.IntValue);
        }
        return Operand.Create(d);
    }
}

export { Function_DATE };

