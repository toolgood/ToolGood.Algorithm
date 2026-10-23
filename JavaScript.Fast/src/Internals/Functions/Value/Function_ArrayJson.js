import { Function_N } from '../Function_N.js';
import { OperandKeyValueList, OperandKeyValue } from '../../../Operand.js';

class Function_ArrayJson extends Function_N {
    get Name() {
        return "ArrayJson";
    }

    constructor(z) {
        super(z);
    }

    evaluate(work, tempParameter) {
        let result = new OperandKeyValueList();
        for (let item of this.z) {
            let o = item.evaluate(work, tempParameter);
            // 与 C# 一致:错误或空值直接传播
            if (o.IsError || o.IsNull) {
                return o;
            }
            // 与 C# 一致:子项必须是键值对,否则参数错误
            if (!(o instanceof OperandKeyValue)) {
                return this.parameterError(1);
            }
            result.AddValue(o._value);
        }
        return result;
    }

    toString2(stringBuilder, addBrackets) {
        stringBuilder.append('{');
        for (let i = 0; i < this.z.length; i++) {
            if (i > 0) {
                stringBuilder.append(", ");
            }
            this.z[i].toString2(stringBuilder, false);
        }
        stringBuilder.append('}');
    }

}

export { Function_ArrayJson };
