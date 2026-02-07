import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { JsonMapper } from '../../../LitJson/JsonMapper.js';

class Function_JSON extends Function_1 {
    get Name() {
        return "Json";
    }

    constructor(a) {
        super(a);
    }

    evaluate(work, tempParameter) {
        let args1 = this.a.evaluate(work, tempParameter);
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
            return this.functionError();
        }
        let txt = args1.TextValue;
        if ((txt.startsWith('{') && txt.endsWith('}')) || (txt.startsWith('[') && txt.endsWith(']'))) {
            try {
                let json = JsonMapper.ToObject(txt);
                return Operand.Create(json);
            } catch (e) {
            }
        }
        return this.functionError();
    }


}

export { Function_JSON };
