import { Function_2 } from '../Function_2.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_LOOKFLOOR
 */
export class Function_LOOKFLOOR extends Function_2 {
    /**
     * @param {FunctionBase[]} funcs
     */
    constructor(funcs) {
        super(funcs);
    }
    
    get Name() {
        return "LookFloor";
    }
    
    /**
     * @param {AlgorithmEngine} engine
     * @returns {Operand}
     */
    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let args2 = this.getArray_2(engine, tempParameter);
        if (args2.IsError) { return args2; }

        let list = [];
        FunctionUtil.F_base_GetList(args2.ArrayValue, list);
        if (list.length === 0) { return this.parameterError(2); }

        list.sort((a, b) => a - b);
        let value = args1.NumberValue;

        // 二分查找第一个 >= value 的位置
        let lo = 0, hi = list.length;
        while (lo < hi) {
            const mid = (lo + hi) >> 1;
            if (list[mid] < value) {
                lo = mid + 1;
            } else {
                hi = mid;
            }
        }
        if (lo < list.length && list[lo] === value) { return args1; }
        if (lo === 0) {
            // 所有元素都大于 value，返回最小值
            return Operand.Create(list[0]);
        }
        return Operand.Create(list[lo - 1]);
    }
    

}

