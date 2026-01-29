import { Function_2 } from '../Function_2';
import { Operand } from '../../Operand';

class Function_PARAM extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    evaluate(engine, tempParameter) {
        const args1 = this.func1.evaluate(engine, tempParameter);
        if (args1.isNotText) {
            args1.toText();
            if (args1.isError) {
                return args1;
            }
        }
        if (tempParameter !== null) {
            const r = tempParameter(engine, args1.textValue);
            if (r !== null) {
                return r;
            }
        }
        const result = engine.getParameter(args1.textValue);
        if (result.isError) {
            if (this.func2 !== null) {
                return this.func2.evaluate(engine, tempParameter);
            }
        }
        return result;
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Param');
    }
}

export { Function_PARAM };
