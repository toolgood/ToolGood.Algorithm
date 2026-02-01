import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_SEARCH extends Function_3 {
    constructor(funcs) {
    super(funcs);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText(StringCache.Function_parameter_error, 'Search', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotText) {
            args2 = args2.ToText(StringCache.Function_parameter_error, 'Search', 2);
            if (args2.IsError) {
                return args2;
            }
        }

        if (this.func3 === null) {
            let p = args2.TextValue.toLowerCase().indexOf(args1.TextValue.toLowerCase()) + engine.ExcelIndex;
            return Operand.Create(p);
        }
        let args3 = this.func3.Evaluate(engine, tempParameter);
        if (args3.IsNotNumber) {
            args3 = args3.ToNumber(StringCache.Function_parameter_error, 'Search', 3);
            if (args3.IsError) {
                return args3;
            }
        }
        let startIndex = args3.IntValue - engine.ExcelIndex;
        let p2 = args2.TextValue.toLowerCase().indexOf(args1.TextValue.toLowerCase(), startIndex) + engine.ExcelIndex;
        return Operand.Create(p2);
    }
}

export { Function_SEARCH };

