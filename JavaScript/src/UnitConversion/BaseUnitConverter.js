import { UnitFactors } from './UnitFactors.js';

class BaseUnitConverter {
    // Set Unit conversions and initial Left/Right conversion
    Instantiate(conversionFactors, leftUnit, rightUnit) {
        this.units = conversionFactors;
        this.unitLeft = leftUnit;
        this.unitRight = rightUnit;
    }

    // Convert the Unit on the Left to the Unit on the Right
    LeftToRight(value) {
        const startFactor = this.units.FindFactor(this.unitLeft);
        const endFactor = this.units.FindFactor(this.unitRight);
        const result = (value / startFactor) * endFactor;
        return result;
    }
}

export { BaseUnitConverter };