import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_GEOMEAN extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    Evaluate(engine, tempParameter) {
        const args = [];
        for (const item of this.funcs) {
            const aa = item.Evaluate(engine, tempParameter);
            if (aa.IsError) {
                return aa;
            }
            args.push(aa);
        }

        const list = [];
        const o = FunctionUtil.F_base_GetList(args, list);
        if (!o) {
            return Operand.error('Function {0} parameter is error!', 'GeoMean');
        }
        if (list.length === 0) {
            return Operand.error('Function {0} parameter is error!', 'GeoMean');
        }
        let product = 1.0;
        for (const num of list) {
            if (num <= 0) {
                return Operand.error('Function {0} parameter is error!', 'GeoMean');
            }
            product *= num;
        }
        const geoMean = Math.pow(product, 1.0 / list.length);
        return Operand.Create(geoMean);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'GeoMean');
    }
}

export { Function_GEOMEAN };
