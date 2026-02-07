import { IJsonWrapper } from './IJsonWrapper.js';
import { JsonType } from './JsonType.js';

class JsonData extends IJsonWrapper {
    constructor() {
        super();
        this.inst_array = null;
        this.inst_boolean = false;
        this.inst_double = 0;
        this.inst_object = null;
        this.inst_string = null;
        this.type = JsonType.None;
    }

    get Count() {
        if (this.type === JsonType.Array) {
            return this.inst_array.length;
        }
        if (this.type === JsonType.Object) {
            return Object.keys(this.inst_object).length;
        }
        return 0;
    }

    get IsArray() {
        return this.type === JsonType.Array;
    }

    get IsBoolean() {
        return this.type === JsonType.Boolean;
    }

    get IsDouble() {
        return this.type === JsonType.Double;
    }

    get IsObject() {
        return this.type === JsonType.Object;
    }

    get IsString() {
        return this.type === JsonType.String;
    }

    get IsNull() {
        return this.type === JsonType.Null;
    }

    get BooleanValue() {
        return this.inst_boolean;
    }

    get NumberValue() {
        return this.inst_double;
    }

    get StringValue() {
        return this.inst_string;
    }

    getEnumerator() {
        return this.ensureList().values();
    }

    *[Symbol.iterator]() {
        if (this.type === JsonType.Array) {
            for (let item of this.inst_array) {
                yield item;
            }
        }
    }

    ensureDictionary() {
        if (this.type === JsonType.Object) {
            return this.inst_object;
        }
        this.type = JsonType.Object;
        this.inst_object = {};
        return this.inst_object;
    }

    ensureList() {
        if (this.type === JsonType.Array) {
            return this.inst_array;
        }
        this.type = JsonType.Array;
        this.inst_array = [];
        return this.inst_array;
    }

    SetBoolean(val) {
        this.type = JsonType.Boolean;
        this.inst_boolean = val;
    }

    SetDouble(val) {
        this.type = JsonType.Double;
        this.inst_double = val;
    }

    SetString(val) {
        this.type = JsonType.String;
        this.inst_string = val;
    }

    SetNull() {
        this.type = JsonType.Null;
    }

    Add(val) {
        this.ensureList().push(val);
    }

    Set(key, val) {
        this.ensureDictionary()[key] = val;
    }

    SetJsonType(type) {
        if (this.type === type) {
            return;
        }

        switch (type) {
            case JsonType.None:
                break;

            case JsonType.Object:
                this.inst_object = {};
                break;

            case JsonType.Array:
                this.inst_array = [];
                break;

            case JsonType.String:
                this.inst_string = '';
                break;

            case JsonType.Double:
                this.inst_double = 0;
                break;

            case JsonType.Boolean:
                this.inst_boolean = false;
                break;
        }

        this.type = type;
    }

    toString() {
        let stringBuilder = [];
        this.toStringBuilder(stringBuilder);
        return stringBuilder.join('');
    }

    toStringBuilder(stringBuilder) {
        if (this.IsNull) {
            stringBuilder.push('null');
        } else if (this.IsBoolean) {
            stringBuilder.push(this.inst_boolean ? 'true' : 'false');
        } else if (this.IsArray) {
            stringBuilder.push('[');
            for (let i = 0; i < this.inst_array.length; i++) {
                if (i > 0) {
                    stringBuilder.push(',');
                }
                this.inst_array[i].toStringBuilder(stringBuilder);
            }
            stringBuilder.push(']');
        } else if (this.IsObject) {
            stringBuilder.push('{');
            let first = true;
            for (let key in this.inst_object) {
                if (!first) {
                    stringBuilder.push(',');
                }
                first = false;
                stringBuilder.push('"');
                stringBuilder.push(key.replace(/"/g, '\\"').replace(/\n/g, '\\n').replace(/\r/g, '\\r')
                    .replace(/\t/g, '\\t').replace(/\0/g, '\\0').replace(/\v/g, '\\v')
                    .replace(/\a/g, '\\a').replace(/\b/g, '\\b').replace(/\f/g, '\\f'));
                stringBuilder.push('":');
                this.inst_object[key].toStringBuilder(stringBuilder);
            }
            stringBuilder.push('}');
        } else if (this.IsString) {
            stringBuilder.push('"');
            stringBuilder.push(this.inst_string.replace(/"/g, '\\"').replace(/\n/g, '\\n').replace(/\r/g, '\\r')
                    .replace(/\t/g, '\\t').replace(/\0/g, '\\0').replace(/\v/g, '\\v')
                    .replace(/\a/g, '\\a').replace(/\b/g, '\\b').replace(/\f/g, '\\f'));
            stringBuilder.push('"');
        } else if (this.IsDouble) {
            stringBuilder.push(this.inst_double.toString());
        }
    }
}

export { JsonData };