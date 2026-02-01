import { FunctionBase } from '../FunctionBase.js';
import { Operand } from '../../../Operand.js';
import { NumberUnitTypeHelper } from '../../NumberUnitTypeHelper.js';

class Function_NUM extends FunctionBase {
    constructor(a, b) {
        super();
        this.d = a;
        this.unit = b;
    }

    Evaluate(engine, tempParameter) {
        let dict = NumberUnitTypeHelper.getUnitTypedict();
        let unitKey = this.unit.toUpperCase();
        let d2 = NumberUnitTypeHelper.transformationUnit(this.d, dict[unitKey], engine.DistanceUnit, engine.AreaUnit, engine.VolumeUnit, engine.MassUnit);
        return Operand.Create(d2);
    }


}

export { Function_NUM };
