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
            let converted = a.ToText("Function '{0}' parameter {1} is error!", this.Name, 1);
            if (converted.IsError) {
                return converted;
            }
            // 与 C# 一致:单参数返回转换后的文本(而非原类型 operand)
            return converted;
        }
        let result = '';
        for (let i = 0; i < this.z.length; i++) {
            let a = this.z[i].evaluate(work, tempParameter);
            let converted = a.ToText("Function '{0}' parameter {1} is error!", this.Name, i + 1);
            if (converted.IsError) {
                return converted;
            }
            result += converted.TextValue;
        }
        return Operand.Create(result);
    }
}

export { Function_CONCATENATE };

