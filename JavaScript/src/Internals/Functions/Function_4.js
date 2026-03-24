import { Function_3 } from './Function_3.js';

export class Function_4 extends Function_3 {
    constructor(func1, func2, func3, func4) {
        if (Array.isArray(func1)) {
            super(func1);
            this.d = func1.length >= 4 ? func1[3] : null;
        } else {
            super(func1, func2, func3);
            this.d = func4 || null;
        }
    }

    getNumber_4(work, tempParameter) {
        if (this.d == null) {
            return this.parameterError(4);
        }
        let args4 = this.d.evaluate(work, tempParameter);
        if (args4.IsNumber) return args4;
        return this.convertToNumber(args4, 4);
    }

    getBoolean_4(work, tempParameter) {
        if (this.d == null) {
            return this.parameterError(4);
        }
        let args4 = this.d.evaluate(work, tempParameter);
        if (args4.IsBoolean) return args4;
        return this.convertToBoolean(args4, 4);
    }

    getDate_4(work, tempParameter) {
        if (this.d == null) {
            return this.parameterError(4);
        }
        let args4 = this.d.evaluate(work, tempParameter);
        if (args4.IsDate) return args4;
        return this.convertToDate(args4, 4);
    }

    getText_4(work, tempParameter) {
        if (this.d == null) {
            return this.parameterError(4);
        }
        let args4 = this.d.evaluate(work, tempParameter);
        if (args4.IsText) return args4;
        return this.convertToText(args4, 4);
    }

    getArray_4(work, tempParameter) {
        if (this.d == null) {
            return this.parameterError(4);
        }
        let args4 = this.d.evaluate(work, tempParameter);
        if (args4.IsArray) return args4;
        return this.convertToArray(args4, 4);
    }
}