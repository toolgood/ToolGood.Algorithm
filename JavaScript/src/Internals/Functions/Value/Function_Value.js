import { FunctionBase } from '../FunctionBase';

class Function_Value extends FunctionBase {
    constructor(value, showName) {
        super();
        this._value = value;
        this._showName = showName;
    }

    evaluate(engine, tempParameter) {
        return this._value;
    }

    toString(stringBuilder, addBrackets) {
        if (this._showName !== undefined && this._showName !== null) {
            stringBuilder.append(this._showName);
            return;
        }
        if (this._value.isText) {
            stringBuilder.append('"');
            let stringValue = this._value.textValue;
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
        } else if (this._value.isDate) {
            stringBuilder.append('"');
            stringBuilder.append(this._value.dateValue.toString());
            stringBuilder.append('"');
        } else if (this._value.isBoolean) {
            stringBuilder.append(this._value.booleanValue ? 'true' : 'false');
        } else {
            stringBuilder.append(this._value.toString());
        }
    }
}

export { Function_Value };
