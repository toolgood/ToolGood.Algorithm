import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { ExcelFunctions } from '../../../MathNet/ExcelFunctions.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_QUARTILE extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotArray) {
            let converted1 = args1.ToArray(StringCache.Function_parameter_error2, "Quartile", 1);
            if (converted1.IsError) return converted1;
            args1 = converted1;
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            let converted2 = args2.ToNumber(StringCache.Function_parameter_error2, "Quartile", 2);
            if (converted2.IsError) return converted2;
            args2 = converted2;
        }

        let list = [];
        // 调试代码
        console.log('args1:', args1);
        console.log('args1.ArrayValue:', args1.ArrayValue);
        console.log('args1.ArrayValue.length:', args1.ArrayValue.length);
        
        let o = FunctionUtil.F_base_GetList(args1.ArrayValue, list);
        if (o == false) {
            return Operand.Error(StringCache.Function_parameter_error2, "Quartile", 1);
        }

        let quant = args2.IntValue;
        if (quant < 0 || quant > 4) {
            return Operand.Error(StringCache.Function_parameter_error2, "Quartile", 2);
        }
        
        // 调试代码
        console.log('QUARTILE 输入数组:', list);
        console.log('QUARTILE 分位数:', quant);
        
        let result = ExcelFunctions.Quartile(list, quant);
        console.log('QUARTILE 结果:', result);
        
        return Operand.Create(result);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Quartile");
    }
}

export { Function_QUARTILE };