import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { JsonMapper } from '../../../LitJson/index.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_JSON extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsError) {
            return args1;
        }
        if (args1.IsJson) {
            return args1;
        }
        if (args1.IsArrayJson) {
            args1 = args1.ToText();
        }
        if (args1.IsNotText) {
            return Operand.Error(StringCache.Function_parameter_1_error, 'Json');
        }
        let txt = args1.TextValue;
        if ((txt.startsWith('{') && txt.endsWith('}')) || (txt.startsWith('[') && txt.endsWith(']'))) {
            try {
                let json = JsonMapper.ToObject(txt);
                return Operand.Create(json);
            } catch (e) {
            }
        }
        return Operand.Error(StringCache.Function_parameter_1_error, 'Json');
    }


}

export { Function_JSON };
