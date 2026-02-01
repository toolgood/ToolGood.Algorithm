import { Function_N } from '../Function_N.js';
import { MyDate } from '../../MyDate.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_DATE extends Function_N {
    constructor(z) {
        super(z);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.z[0].Evaluate(engine, tempParameter);
            args1 = args1.ToNumber(StringCache.Function_parameter_error, "Date", 1);
            if (args1.IsError) { return args1; }
        let args2 = this.z[1].Evaluate(engine, tempParameter);
            args2 = args2.ToNumber(StringCache.Function_parameter_error, "Date", 2);
            if (args2.IsError) { return args2; }
        let args3 = this.z[2].Evaluate(engine, tempParameter);
            args3 = args3.ToNumber(StringCache.Function_parameter_error, "Date", 3);
            if (args3.IsError) { return args3; }

        let d;
        if (this.z.length == 3) {
            d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, 0, 0, 0);
        } else if (this.z.length == 4) {
            let args4 = this.z[3].Evaluate(engine, tempParameter);
                args4 = args4.ToNumber(StringCache.Function_parameter_error, "Date", 4);
                if (args4.IsError) { return args4; }
            d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, args4.IntValue, 0, 0);
        } else if (this.z.length == 5) {
            let args4 = this.z[3].Evaluate(engine, tempParameter);
                args4 = args4.ToNumber(StringCache.Function_parameter_error, "Date", 4);
                if (args4.IsError) { return args4; }
            let args5 = this.z[4].Evaluate(engine, tempParameter);
                args5 = args5.ToNumber(StringCache.Function_parameter_error, "Date", 5);
                if (args5.IsError) { return args5; }
            d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, args4.IntValue, args5.IntValue, 0);
        } else {
            let args4 = this.z[3].Evaluate(engine, tempParameter);
                args4 = args4.ToNumber(StringCache.Function_parameter_error, "Date", 4);
                if (args4.IsError) { return args4; }
            let args5 = this.z[4].Evaluate(engine, tempParameter);
                args5 = args5.ToNumber(StringCache.Function_parameter_error, "Date", 5);
                if (args5.IsError) { return args5; }
            let args6 = this.z[5].Evaluate(engine, tempParameter);
                args6 = args6.ToNumber(StringCache.Function_parameter_error, "Date", 6);
                if (args6.IsError) { return args6; }
            d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, args4.IntValue, args5.IntValue, args6.IntValue);
        }
        return Operand.Create(d);
    }
}

export { Function_DATE };

