import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_COUNT extends Function_N {
    get Name() {
        return "Count";
    }

    constructor(z) {
        super(z);
    }

    evaluate(engine, tempParameter) {
        let args = [];
        let error = this.tryEvaluateAll(engine, tempParameter, args);
        if (error != null) {
            return error;
        }

        // 对齐 C# FlattenToList(List<Operand>, List<Operand>):不过滤类型,文本/null 也计数
        let count = 0;
        function flatten(item) {
            if (item.IsArray) {
                for (const sub of item.ArrayValue) {
                    flatten(sub);
                }
            } else if (item.IsJson) {
                let array = item.ToArray(null);
                if (array.IsError) {
                    count = -1;
                    return;
                }
                for (const sub of array.ArrayValue) {
                    flatten(sub);
                }
            } else {
                count++;
            }
        }
        for (let item of args) {
            flatten(item);
        }
        if (count < 0) {
            return this.functionError();
        }
        return Operand.Create(count);
    }
}

export { Function_COUNT };

