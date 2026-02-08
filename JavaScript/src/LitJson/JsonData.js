import { JsonType } from './JsonType.js';

class JsonData  {
    constructor() {
        this.inst_array = null;
        this.inst_boolean = false;
        this.inst_double = 0;
        this.inst_object = null;
        this.inst_string = null;
        this.type = JsonType.none;
    }

    get Count() {
        if (this.type === JsonType.array) {
            return this.inst_array.length;
        }
        if (this.type === JsonType.object) {
            return Object.keys(this.inst_object).length;
        }
        return 0;
    }

    get IsArray() {
        return this.type === JsonType.array;
    }

    get IsBoolean() {
        return this.type === JsonType.boolean;
    }

    get IsDouble() {
        return this.type === JsonType.double;
    }

    get IsObject() {
        return this.type === JsonType.object;
    }

    get IsString() {
        return this.type === JsonType.string;
    }

    get IsNull() {
        return this.type === JsonType.null;
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
        if (this.type === JsonType.array) {
            for (let item of this.inst_array) {
                yield item;
            }
        }
    }

    ensureDictionary() {
        if (this.type === JsonType.object) {
            return this.inst_object;
        }
        this.type = JsonType.object;
        this.inst_object = {};
        return this.inst_object;
    }

    ensureList() {
        if (this.type === JsonType.array) {
            return this.inst_array;
        }
        this.type = JsonType.array;
        this.inst_array = [];
        return this.inst_array;
    }

    setBoolean(val) {
        this.type = JsonType.boolean;
        this.inst_boolean = val;
    }

    setDouble(val) {
        this.type = JsonType.double;
        this.inst_double = val;
    }

    setString(val) {
        this.type = JsonType.string;
        this.inst_string = val;
    }

    setNull() {
        this.type = JsonType.null;
    }

    add(val) {
        this.ensureList().push(val);
    }

    set(key, val) {
        this.ensureDictionary()[key] = val;
    }

    setJsonType(type) {
        if (this.type === type) {
            return;
        }

        switch (type) {
            case JsonType.none:
                break;

            case JsonType.object:
                this.inst_object = {};
                break;

            case JsonType.array:
                this.inst_array = [];
                break;

            case JsonType.string:
                this.inst_string = '';
                break;

            case JsonType.double:
                this.inst_double = 0;
                break;

            case JsonType.boolean:
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