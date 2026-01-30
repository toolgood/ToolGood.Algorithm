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
        let product = 1.0;
        for (let num of list) {
            if (num <= 0) {
                return Operand.error('Function {0} parameter is error!', 'GeoMean');
            }
            product *= num;
        }
        let geoMean = Math.pow(product, 1.0 / list.length);
        return Operand.Create(geoMean);
    }
}

export { Function_GEOMEAN };

