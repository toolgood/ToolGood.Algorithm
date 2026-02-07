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
        let result = list[0];
        if (result === value) { return args1; }

        for (let i = 1; i < list.length; i++) {
            let val = list[i];
            if (val < value) {
                result = val;
            } else if (val === value) {
                return args1;
            } else {
                break;
            }
        }
        return Operand.Create(result);
    }
    

}

