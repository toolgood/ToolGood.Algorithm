import { FunctionBase } from '../FunctionBase.js';
import { Operand } from '../../../Operand.js';
import { NumberUnitTypeHelper } from '../../NumberUnitTypeHelper.js';

class Function_NUM extends FunctionBase {
    constructor(func1, func2) {
        super();
        this.d = func1;
        this.unit = func2;
    }

    Evaluate(engine, tempParameter) {
        let dict = NumberUnitTypeHelper.getUnitTypedict();
        let d2 = NumberUnitTypeHelper.transformationUnit(this.d, dict[this.unit], engine.DistanceUnit, engine.AreaUnit, engine.VolumeUnit, engine.MassUnit);
        return Operand.Create(d2);
    }

    toString(stringBuilder, addBrackets) {
        stringBuilder.append(this.d);
        stringBuilder.append(this.unit);
    }
}

export { Function_NUM };
