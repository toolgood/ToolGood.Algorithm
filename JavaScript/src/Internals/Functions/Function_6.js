import { Function_5 } from './Function_5.js';

export class Function_6 extends Function_5 {
    constructor(func1, func2, func3, func4, func5, func6) {
        if (Array.isArray(func1)) {
            super(func1);
            this.func6 = func1.length >= 6 ? func1[5] : null;
        } else {
            super(func1, func2, func3, func4, func5);
            this.func6 = func6 || null;
        }
    }

    getNumber_6(work, tempParameter) {
        if (this.func6 == null) {
            return this.parameterError(6);
        }
        let args6 = this.func6.evaluate(work, tempParameter);
        if (args6.IsNumber) return args6;
        return this.convertToNumber(args6, 6);
    }

    getBoolean_6(work, tempParameter) {
        if (this.func6 == null) {
            return this.parameterError(6);
        }
        let args6 = this.func6.evaluate(work, tempParameter);
        if (args6.IsBoolean) return args6;
        return this.convertToBoolean(args6, 6);
    }

    getDate_6(work, tempParameter) {
        if (this.func6 == null) {
            return this.parameterError(6);
        }
        let args6 = this.func6.evaluate(work, tempParameter);
        if (args6.IsDate) return args6;
        return this.convertToDate(args6, 6);
    }

    getText_6(work, tempParameter) {
        if (this.func6 == null) {
            return this.parameterError(6);
        }
        let args6 = this.func6.evaluate(work, tempParameter);
        if (args6.IsText) return args6;
        return this.convertToText(args6, 6);
    }

    getArray_6(work, tempParameter) {
        if (this.func6 == null) {
            return this.parameterError(6);
        }
        let args6 = this.func6.evaluate(work, tempParameter);
        if (args6.IsArray) return args6;
        return this.convertToArray(args6, 6);
    }
}