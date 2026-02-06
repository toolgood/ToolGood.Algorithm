import { FunctionBase } from '../FunctionBase.js';

class Function_Value extends FunctionBase {
    get Name() {
        return "Value";
    }

    constructor(value, showName) {
        super();
        this._value = value;
        this._showName = showName;
    }

    Evaluate(work, tempParameter) {
        return this._value;
    }

    ToString(stringBuilder, addBrackets) {
        if (this._showName != null && this._showName !== '') {
            stringBuilder.append(this._showName);
            return;
        }
        if (this._value.IsText) {
            stringBuilder.append('"');
            let stringValue = this._value.TextValue;
            stringValue = stringValue.replace(/\\/g, '\\\\');
            stringValue = stringValue.replace(/\r/g, '\\r');
            stringValue = stringValue.replace(/\n/g, '\\n');
            stringValue = stringValue.replace(/\t/g, '\\t');
            stringValue = stringValue.replace(/\0/g, '\\0');
            stringValue = stringValue.replace(/\v/g, '\\v');
            stringValue = stringValue.replace(/\a/g, '\\a');
            stringValue = stringValue.replace(/\b/g, '\\b');
            stringValue = stringValue.replace(/\f/g, '\\f');
            stringValue = stringValue.replace(/"/g, '\\"');
            stringBuilder.append(stringValue);
            stringBuilder.append('"');
        } else if (this._value.IsDate) {
            stringBuilder.append('"');
            stringBuilder.append(this._value.DateValue.toString());
            stringBuilder.append('"');
        } else if (this._value.IsBoolean) {
            stringBuilder.append(this._value.BooleanValue ? "true" : "false");
        } else {
            stringBuilder.append(this._value.toString());
        }
    }
}

export { Function_Value };
