import { Function_2 } from '../Function_2';
import { Operand } from '../../Operand';
import { FunctionUtil } from '../FunctionUtil';

class Function_LARGE extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    evaluate(engine, tempParameter) {
        const args1 = this.func1.evaluate(engine, tempParameter);
        if (!args1.isArray) {
            args1.toArray('Function {0} parameter {1} is error!', 'Large', 1);
            if (args1.isError) {
                return args1;
            }
        }
        const args2 = this.func2.evaluate(engine, tempParameter);
        if (args2.isNotNumber) {
            args2.toNumber('Function {0} parameter {1} is error!', 'Large', 2);
            if (args2.isError) {
                return args2;
            }
        }
        const list = [];
        const o = FunctionUtil.F_base_GetList(args1, list);
        if (!o) {
            return Operand.error('Function {0} parameter {1} is error!', 'Large', 1);
        }

        list.sort((a, b) => b - a); // 降序排序
        const k = Math.round(args2.doubleValue);
        if (k < 1 - engine.excelIndex || k > list.length - engine.excelIndex) {
            return Operand.error('Function {0} parameter {1} is error!', 'Large', 2);
        }
        return Operand.create(list[k - engine.excelIndex]);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Large');
    }
}

export { Function_LARGE };
