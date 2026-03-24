import { Function_0 } from '../Function_0.js';
import { Operand } from '../../../Operand.js';

class Function_ValueText extends Function_0 {
    constructor(value, showName = null) {
        super();
        this._value = value;
        this._showName = showName || null;
    }

    get Name() {
        return "Value";
    }

    evaluate(engine, tempParameter) {
        return this._value;
    }

    toString2(stringBuilder, addBrackets) {
        if (this._showName != null) {
            stringBuilder.push(this._showName);
            return;
        }
        let stringValue = this._value.TextValue;
        stringValue = stringValue.replace(/\\/g, "\\\\");
        stringValue = stringValue.replace(/\r/g, "\\r");
        stringValue = stringValue.replace(/\n/g, "\\n");
        stringValue = stringValue.replace(/\t/g, "\\t");
        stringValue = stringValue.replace(/\0/g, "\\0");
        stringValue = stringValue.replace(/\v/g, "\\v");
        stringValue = stringValue.replace(/\a/g, "\\a");
        stringValue = stringValue.replace(/\b/g, "\\b");
        stringValue = stringValue.replace(/\f/g, "\\f");
        stringValue = stringValue.replace(/"/g, "\\\"");
        stringBuilder.push('"');
        stringBuilder.push(stringValue);
        stringBuilder.push('"');
    }
}

export { Function_ValueText };