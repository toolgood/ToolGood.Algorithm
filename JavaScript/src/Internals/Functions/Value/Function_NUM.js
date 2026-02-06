import { FunctionBase } from '../FunctionBase.js';
import { Operand } from '../../../Operand.js';
import { NumberUnitTypeHelper } from '../../NumberUnitTypeHelper.js';

class Function_NUM extends FunctionBase {
    get Name() {
        return "Num";
    }

    constructor(a, b) {
        super();
        this.d = a;
        this.unit = b;
    }

    Evaluate(work, tempParameter) {
        let dict = NumberUnitTypeHelper.getUnitTypedict();
        let unitKey = this.unit.toUpperCase();
        let d2 = NumberUnitTypeHelper.transformationUnit(this.d, dict[unitKey], work.DistanceUnit, work.AreaUnit, work.VolumeUnit, work.MassUnit);
        return Operand.Create(d2);
    }

    ToString(stringBuilder, addBrackets) {
        stringBuilder.append(this.d);
        stringBuilder.append(this.unit);
    }


}

export { Function_NUM };
