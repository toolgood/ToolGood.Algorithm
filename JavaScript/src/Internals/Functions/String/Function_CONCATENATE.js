import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_CONCATENATE extends Function_N {
    get Name() {
        return "Concatenate";
    }

    constructor(z) {
        super(z);
    }

    evaluate(work, tempParameter) {
        if (this.z.length === 0) {
            return Operand.Create('');
        }
        if (this.z.length === 1) {
            let a = this.z[0].evaluate(work, tempParameter);
                a.ToText(this.functionError, 'Concatenate', 1);
                if (a.IsError) {
                    return a;
                }
            return a;
        }
        let result = '';
        for (let i = 0; i < this.z.length; i++) {
            let a = this.z[i].evaluate(work, tempParameter);
                a.ToText(this.functionError, 'Concatenate', i + 1);
                if (a.IsError) {
                    return a;
                }
            result += a.TextValue;
        }
        return Operand.Create(result);
    }
}

export { Function_CONCATENATE };

