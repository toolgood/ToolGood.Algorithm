import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_GEOMEAN extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    Evaluate(engine, tempParameter) {
        let args = [];
        for (let item of this.funcs) {
            let aa = item.Evaluate(engine, tempParameter);
            if (aa.IsError) {
                return aa;
            }
            args.push(aa);
        }

        let list = [];
        let o = FunctionUtil.F_base_GetList(args, list);
        if (!o) {
            return Operand.error('Function {0} parameter is error!', 'GeoMean');
        }
        if (list.length === 0) {
            return Operand.error('Function {0} parameter is error!', 'GeoMean');
        }
        console.log('GEOMEAN 输入列表:', list);
        console.log('GEOMEAN 列表长度:', list.length);
        let product = 1.0;
        for (let num of list) {
            if (num <= 0) {
                return Operand.error('Function {0} parameter is error!', 'GeoMean');
            }
            product *= num;
            console.log('GEOMEAN 当前乘积:', product);
        }
        console.log('GEOMEAN 最终乘积:', product);
        let geoMean = Math.pow(product, 1.0 / list.length);
        console.log('GEOMEAN 计算结果:', geoMean);
        return Operand.Create(geoMean);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'GeoMean');
    }
}

export { Function_GEOMEAN };
