import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_JSON extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.isError) {
            return args1;
        }
        if (args1.isJson) {
            return args1;
        }
        if (args1.isArrayJson) {
            args1 = args1.toText();
        }
        if (args1.isNotText) {
            return Operand.error('Function {0} parameter is error!', 'Json');
        }
        const txt = args1.textValue;
        if ((txt.startsWith('{') && txt.endsWith('}')) || (txt.startsWith('[') && txt.endsWith(']'))) {
            try {
                const json = JSON.parse(txt);
                return Operand.create(json);
            } catch (e) {
            }
        }
        return Operand.error('Function {0} parameter is error!', 'Json');
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Json');
    }
}

export { Function_JSON };
