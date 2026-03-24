import { Function_4 } from './Function_4.js';

export class Function_5 extends Function_4 {
    constructor(func1, func2, func3, func4, func5) {
        if (Array.isArray(func1)) {
            super(func1);
            this.func5 = func1.length >= 5 ? func1[4] : null;
        } else {
            super(func1, func2, func3, func4);
            this.func5 = func5 || null;
        }
    }

    getNumber_5(work, tempParameter) {
        if (this.func5 == null) {
            return this.parameterError(5);
        }
        let args5 = this.func5.evaluate(work, tempParameter);
        if (args5.IsNumber) return args5;
        return this.convertToNumber(args5, 5);
    }

    getBoolean_5(work, tempParameter) {
        if (this.func5 == null) {
            return this.parameterError(5);
        }
        let args5 = this.func5.evaluate(work, tempParameter);
        if (args5.IsBoolean) return args5;
        return this.convertToBoolean(args5, 5);
    }

    getDate_5(work, tempParameter) {
        if (this.func5 == null) {
            return this.parameterError(5);
        }
        let args5 = this.func5.evaluate(work, tempParameter);
        if (args5.IsDate) return args5;
        return this.convertToDate(args5, 5);
    }

    getText_5(work, tempParameter) {
        if (this.func5 == null) {
            return this.parameterError(5);
        }
        let args5 = this.func5.evaluate(work, tempParameter);
        if (args5.IsText) return args5;
        return this.convertToText(args5, 5);
    }

    getArray_5(work, tempParameter) {
        if (this.func5 == null) {
            return this.parameterError(5);
        }
        let args5 = this.func5.evaluate(work, tempParameter);
        if (args5.IsArray) return args5;
        return this.convertToArray(args5, 5);
    }
}