import { Function_2 } from './Function_2.js';

export class Function_3 extends Function_2 {
    constructor(func1, func2, func3) {
        if (Array.isArray(func1)) {
            super(func1);
            this.c = func1.length >= 3 ? func1[2] : null;
        } else {
            super(func1, func2);
            this.c = func3 || null;
        }
    }

    getNumber_3(work, tempParameter) {
        if (this.c == null) {
            return this.parameterError(3);
        }
        let args3 = this.c.evaluate(work, tempParameter);
        if (args3.IsNumber) return args3;
        return this.convertToNumber(args3, 3);
    }

    getBoolean_3(work, tempParameter) {
        if (this.c == null) {
            return this.parameterError(3);
        }
        let args3 = this.c.evaluate(work, tempParameter);
        if (args3.IsBoolean) return args3;
        return this.convertToBoolean(args3, 3);
    }

    getDate_3(work, tempParameter) {
        if (this.c == null) {
            return this.parameterError(3);
        }
        let args3 = this.c.evaluate(work, tempParameter);
        if (args3.IsDate) return args3;
        return this.convertToDate(args3, 3);
    }

    getText_3(work, tempParameter) {
        if (this.c == null) {
            return this.parameterError(3);
        }
        let args3 = this.c.evaluate(work, tempParameter);
        if (args3.IsText) return args3;
        return this.convertToText(args3, 3);
    }

    getArray_3(work, tempParameter) {
        if (this.c == null) {
            return this.parameterError(3);
        }
        let args3 = this.c.evaluate(work, tempParameter);
        if (args3.IsArray) return args3;
        return this.convertToArray(args3, 3);
    }
}