import { OperandType } from './Enums/index.js';
import { JsonMapper, JsonData } from './LitJson/index.js';
import { FunctionUtil } from './Internals/Functions/FunctionUtil.js';
import { MyDate } from './Internals/MyDate.js';

/**
 * 操作数
 */
export class Operand {
    /**
     * 版本号
     */
    static Version;

    /**
     * True
     */
    static True;

    /**
     * False
     */
    static False;

    /**
     * One
     */
    static One;

    /**
     * Zero
     */
    static Zero;

    /**
     * 是否为空值
     */
    get IsNull() { return false; }
    /**
     * 是否为非空值
     */
    get IsNotNull() { return true; }

    /**
     * 是否数字
     */
    get IsNumber() { return false; }
    /**
     * 是否非数字
     */
    get IsNotNumber() { return true; }

    /**
     * 是否字符串
     */
    get IsText() { return false; }
    /**
     * 是否非字符串
     */
    get IsNotText() { return true; }

    /**
     * 是否布尔值
     */
    get IsBoolean() { return false; }
    /**
     * 是否非布尔值
     */
    get IsNotBoolean() { return true; }
    /**
     * 是否数组
     */
    get IsArray() { return false; }
    /**
     * 是否非数组
     */
    get IsNotArray() { return true; }
    /**
     * 是否日期
     */
    get IsDate() { return false; }
    /**
     * 是否非日期
     */
    get IsNotDate() { return true; }
    /**
     * 是否Json对象
     */
    get IsJson() { return false; }
    /**
     * 是否非Json对象
     */
    get IsNotJson() { return true; }
    /**
     * 是否Json数组
     */
    get IsArrayJson() { return false; }
    /**
     * 是否非Json数组
     */
    get IsNotArrayJson() { return true; }

    /**
     * 是否出错
     */
    get IsError() { return false; }

    /**
     * 错误信息
     */
    get ErrorMsg() { return null; }

    /**
     * 操作数类型
     */
    get Type() { throw new Error('Not implemented'); }

    /**
     * 数字值
     */
    get NumberValue() { throw new Error('Not implemented'); }
    /**
     * int值
     */
    get IntValue() { throw new Error('Not implemented'); }

    /**
     * 字符串值
     */
    get TextValue() { throw new Error('Not implemented'); }

    /**
     * 布尔值
     */
    get BooleanValue() { throw new Error('Not implemented'); }

    /**
     * 数组值
     */
    get ArrayValue() { throw new Error('Not implemented'); }

    get JsonValue() { throw new Error('Not implemented'); }

    /**
     * 时间值
     */
    get DateValue() { throw new Error('Not implemented'); }

    /**
     * 创建操作数
     */
    static Create(obj) {
        if (obj instanceof Operand) {
            return obj;
        } else if (typeof obj === 'boolean') {
            return obj ? Operand.True : Operand.False;
        } else if (typeof obj === 'number') {
            return new OperandDouble(obj);
        } else if (typeof obj === 'string') {
            if (obj === null) {
                return Operand.CreateNull();
            }
            return new OperandString(obj);
        } else if (obj instanceof MyDate) {
            return new OperandMyDate(obj);
        } else if (obj instanceof JsonData) {
            return new OperandJson(obj);
        } else if (Array.isArray(obj)) {
            let arr=[];
            for (let index = 0; index < obj.length; index++) {
                const element =Operand.Create(obj[index]);
                arr.push(element);
            }
            return new OperandArray(arr);
        }
        return Operand.CreateNull();
    }

    /**
     * 创建操作数
     */
    static CreateJson(txt) {
        if ((txt.startsWith('{') && txt.endsWith('}')) || (txt.startsWith('[') && txt.endsWith(']'))) {
            try {
                const json = JsonMapper.ToObject(txt);
                return Operand.Create(json);
            } catch (e) { }
        }
        return Operand.Error("Convert to json error!");
    }

    /**
     * 创建操作数
     */
    static Error(msg, ...args) {
        if (args.length > 0) {
            msg = msg.replace(/\{\d+\}/g, (match, index) => {
                const i = parseInt(match.substring(1, match.length - 1));
                return args[i] !== undefined ? args[i] : match;
            });
        }
        return new OperandError(msg);
    }

    /**
     * 创建操作数
     */
    static CreateNull() {
        return new OperandNull();
    }

    /**
     * 转数值类型
     */
    ToNumber(errorMessage = null, ...args) {
        if (args.length > 0) {
            errorMessage = errorMessage.replace(/\{\d+\}/g, (match, index) => {
                const i = parseInt(match.substring(1, match.length - 1));
                return args[i] !== undefined ? args[i] : match;
            });
        }
        return Operand.Error(errorMessage ?? "Convert to number error!");
    }

    /**
     * 转bool类型
     */
    ToBoolean(errorMessage = null, ...args) {
        if (args.length > 0) {
            errorMessage = errorMessage.replace(/\{\d+\}/g, (match, index) => {
                const i = parseInt(match.substring(1, match.length - 1));
                return args[i] !== undefined ? args[i] : match;
            });
        }
        return Operand.Error(errorMessage ?? "Convert to bool error!");
    }

    /**
     * 转string类型
     */
    ToText(errorMessage = null, ...args) {
        if (args.length > 0) {
            errorMessage = errorMessage.replace(/\{\d+\}/g, (match, index) => {
                const i = parseInt(match.substring(1, match.length - 1));
                return args[i] !== undefined ? args[i] : match;
            });
        }
        return Operand.Error(errorMessage ?? "Convert to string error!");
    }

    /**
     * 转MyDate类型
     */
    ToMyDate(errorMessage = null, ...args) {
        if (args.length > 0) {
            errorMessage = errorMessage.replace(/\{\d+\}/g, (match, index) => {
                const i = parseInt(match.substring(1, match.length - 1));
                return args[i] !== undefined ? args[i] : match;
            });
        }
        return Operand.Error(errorMessage ?? "Convert to date error!");
    }

    /**
     * 转Array类型
     */
    ToArray(errorMessage = null, ...args) {
        if (args.length > 0) {
            errorMessage = errorMessage.replace(/\{\d+\}/g, (match, index) => {
                const i = parseInt(match.substring(1, match.length - 1));
                return args[i] !== undefined ? args[i] : match;
            });
        }
        return Operand.Error(errorMessage ?? "Convert to array error!");
    }

    /**
     * 转Json类型
     */
    ToJson(errorMessage = null) {
        return Operand.Error(errorMessage ?? "Convert to json error!");
    }
}

class OperandDouble extends Operand {
    constructor(obj) {
        super();
        this._value = obj;
    }
    get IsNumber() { return true; }
    get IsNotNumber() { return false; }
    get Type() { return OperandType.NUMBER; }
    get IntValue() { return Math.floor(this._value); }
    get NumberValue() { return this._value; }

    ToNumber(errorMessage) { return this; }
    ToNumber(errorMessage, ...args) { return this; }

    ToBoolean(errorMessage) { return this.NumberValue !== 0 ? Operand.True : Operand.False; }
    ToBoolean(errorMessage, ...args) { return this.NumberValue !== 0 ? Operand.True : Operand.False; }

    ToText(errorMessage) { return Operand.Create(this.NumberValue.toString()); }
    ToText(errorMessage, ...args) { return Operand.Create(this.NumberValue.toString()); }

    ToMyDate(errorMessage) { return Operand.Create(new MyDate(this.NumberValue)); }
    ToMyDate(errorMessage, ...args) { return Operand.Create(new MyDate(this.NumberValue)); }

    toString() { return this.NumberValue.toString(); }
}

class OperandBoolean extends Operand {
    constructor(obj) {
        super();
        this._value = obj;
    }
    get IsBoolean() { return true; }
    get IsNotBoolean() { return false; }
    get Type() { return OperandType.BOOLEAN; }
    get BooleanValue() { return this._value; }

    ToNumber(errorMessage) { return this._value ? Operand.One : Operand.Zero; }
    ToNumber(errorMessage, ...args) { 
        return this._value ? Operand.One : Operand.Zero; 
    }

    ToBoolean(errorMessage) { return this; }
    ToBoolean(errorMessage, ...args) { return this; }

    ToText(errorMessage) { return Operand.Create(this._value ? "TRUE" : "FALSE"); }
    ToText(errorMessage, ...args) { return Operand.Create(this._value ? "TRUE" : "FALSE"); }

    toString() { return this._value ? "true" : "false"; }
}

class OperandString extends Operand {
    constructor(obj) {
        super();
        this._value = obj;
    }

    get IsText() { return true; }
    get IsNotText() { return false; }
    get Type() { return OperandType.TEXT; }
    get TextValue() { return this._value; }

    ToNumber(errorMessage) {
        if (this.TextValue.indexOf('.') === -1) {
            const num = parseInt(this.TextValue);
            if (!isNaN(num)) {
                return Operand.Create(num);
            }
        }
        const d = parseFloat(this.TextValue);
        if (!isNaN(d)) {
            return Operand.Create(d);
        }
        if (errorMessage == null) {
            return Operand.Error("Convert to number error!");
        }
        return Operand.Error(errorMessage);
    }
    ToNumber(errorMessage, ...args) {
        if (this.TextValue.indexOf('.') === -1) {
            const num = parseInt(this.TextValue);
            if (!isNaN(num)) {
                return Operand.Create(num);
            }
        }
        const d = parseFloat(this.TextValue);
        if (!isNaN(d)) {
            return Operand.Create(d);
        }
        if (errorMessage == null) {
            return Operand.Error("Convert to number error!");
        }
        return Operand.Error(errorMessage);
    }

    ToText(errorMessage) { return this; }
    ToText(errorMessage, ...args) { return this; }

    ToBoolean(errorMessage) {
        const b = FunctionUtil.TryParseBoolean(this.TextValue);
        if (b !== null) {
            return b ? Operand.True : Operand.False;
        }
        if (errorMessage == null) {
            return Operand.Error("Convert to bool error!");
        }
        return Operand.Error(errorMessage);
    }
    ToBoolean(errorMessage, ...args) {
        const b = FunctionUtil.TryParseBoolean(this.TextValue);
        if (b !== null) {
            return b ? Operand.True : Operand.False;
        }
        if (errorMessage == null) {
            return Operand.Error("Convert to bool error!");
        }
        return Operand.Error(errorMessage);
    }

    ToMyDate(errorMessage) {
        try {
            const date = new Date(this.TextValue);
            if (!isNaN(date.getTime())) {
                return Operand.Create(new MyDate(date));
            }
        } catch (e) { }
        if (errorMessage == null) {
            return Operand.Error("Convert to date error!");
        }
        return Operand.Error(errorMessage);
    }
    ToMyDate(errorMessage, ...args) {
        try {
            const date = new Date(this.TextValue);
            if (!isNaN(date.getTime())) {
                return Operand.Create(new MyDate(date));
            }
        } catch (e) { }
        if (errorMessage == null) {
            return Operand.Error("Convert to date error!");
        }
        return Operand.Error(errorMessage);
    }

    ToArray(errorMessage) {
        return Operand.Error(errorMessage ?? "Convert to array error!");
    }
    ToJson(errorMessage = null) {
        const txt = this.TextValue.trim();
        if ((txt.startsWith('{') && txt.endsWith('}')) || (txt.startsWith('[') && txt.endsWith(']'))) {
            try {
                const json = JsonMapper.ToObject(txt);
                return Operand.Create(json);
            } catch (e) { }
        }
        return Operand.Error(errorMessage ?? "Convert to json error!");
    }

    toString() {
        let result = '"';
        for (const c of this._value) {
            switch (c) {
                case '"': result += '\\"'; break;
                case '\n': result += '\\n'; break;
                case '\r': result += '\\r'; break;
                case '\t': result += '\\t'; break;
                case '\0': result += '\\0'; break;
                case '\v': result += '\\v'; break;
                case '\a': result += '\\a'; break;
                case '\b': result += '\\b'; break;
                case '\f': result += '\\f'; break;
                default: result += c; break;
            }
        }
        result += '"';
        return result;
    }
}

class OperandMyDate extends Operand {
    constructor(obj) {
        super();
        this._value = obj;
    }
    get IsDate() { return true; }
    get IsNotDate() { return false; }
    get Type() { return OperandType.DATE; }
    get DateValue() { return this._value; }

    ToNumber(errorMessage) { return Operand.Create(this._value.valueOf()); }
    ToNumber(errorMessage, ...args) { return Operand.Create(this._value.valueOf()); }

    ToBoolean(errorMessage) { return this._value.valueOf() !== 0 ? Operand.True : Operand.False; }
    ToBoolean(errorMessage, ...args) { return this._value.valueOf() !== 0 ? Operand.True : Operand.False; }

    ToText(errorMessage) { return Operand.Create(this._value.toString()); }
    ToText(errorMessage, ...args) { return Operand.Create(this._value.toString()); }

    ToMyDate(errorMessage) { return this; }
    ToMyDate(errorMessage, ...args) { return this; }

    toString() { return '"' + this._value.toString() + '"'; }
}

class OperandJson extends Operand {
    constructor(obj) {
        super();
        this._value = obj;
    }
    get IsJson() { return true; }
    get IsNotJson() { return false; }
    get Type() { return OperandType.JSON; }
    get JsonValue() { return this._value; }
    get IsArrayJson() { return false; }
    get IsNotArrayJson() { return true; }

    ToText(errorMessage = null) {
        return Operand.Create(this._value.toString());
    }
    ToText(errorMessage, ...args) {
        return Operand.Create(this._value.toString());
    }

    ToArray(errorMessage) {
        if (this.JsonValue.IsArray) {
            const list = [];
            for (const v of this.JsonValue) {
                if (v.IsString) {
                    list.push(Operand.Create(v.StringValue));
                } else if (v.IsBoolean) {
                    list.push(Operand.Create(v.BooleanValue));
                } else if (v.IsDouble) {
                    list.push(Operand.Create(v.NumberValue));
                } else if (v.IsNull) {
                    list.push(Operand.CreateNull());
                } else {
                    list.push(Operand.Create(v));
                }
            }
            return Operand.Create(list);
        }
        return Operand.Error(errorMessage ?? "Convert to array error!");
    }
    ToArray(errorMessage, ...args) {
        if (this.JsonValue.IsArray) {
            const list = [];
            for (const v of this.JsonValue) {
                if (v.IsString) {
                    list.push(Operand.Create(v.StringValue));
                } else if (v.IsBoolean) {
                    list.push(Operand.Create(v.BooleanValue));
                } else if (v.IsDouble) {
                    list.push(Operand.Create(v.NumberValue));
                } else if (v.IsNull) {
                    list.push(Operand.CreateNull());
                } else {
                    list.push(Operand.Create(v));
                }
            }
            return Operand.Create(list);
        }
        return Operand.Error(errorMessage);
    }
    ToJson(errorMessage = null) {
        return this;
    }
    
    TryGetValue(key) {
        if (this.JsonValue.IsObject) {
            const value = this.JsonValue.inst_object[key];
            if (value) {
                if (value.IsString) {
                    return Operand.Create(value.StringValue);
                } else if (value.IsBoolean) {
                    return Operand.Create(value.BooleanValue);
                } else if (value.IsDouble) {
                    return Operand.Create(value.NumberValue);
                } else if (value.IsNull) {
                    return Operand.CreateNull();
                } else {
                    return Operand.Create(value);
                }
            }
        }
        return null;
    }
    
    toString() {
        return this._value.toString();
    }
}

class OperandArray extends Operand {
    constructor(obj) {
        super();
        this._value = obj;
    }
    get IsArray() { return true; }
    get IsNotArray() { return false; }
    get Type() { return OperandType.ARRARY; }
    get ArrayValue() { return this._value; }

    ToText(errorMessage = null) {
        return Operand.Create(this.toString());
    }
    ToText(errorMessage, ...args) {
        return Operand.Create(this.toString());
    }

    ToArray(errorMessage) { return this; }
    ToArray(errorMessage, ...args) { return this; }

    ToJson(errorMessage = null) {
        const txt = this.toString();
        try {
            const json = JsonMapper.ToObject(txt);
            return Operand.Create(json);
        } catch (e) { }
        return Operand.Error(errorMessage ?? "Convert to json error!");
    }

    toString() {
        const elements = this.ArrayValue.map(item => item.toString());
        return '[' + elements.join(',') + ']';
    }
}

class OperandError extends Operand {
    get Type() { return OperandType.ERROR; }
    get IsError() { return true; }
    constructor(msg) {
        super();
        this._errorMsg = msg;
    }
    get ErrorMsg() { return this._errorMsg; }

    ToNumber(errorMessage) { return this; }
    ToNumber(errorMessage, ...args) { return this; }

    ToBoolean(errorMessage) { return this; }
    ToBoolean(errorMessage, ...args) { return this; }

    ToText(errorMessage) { return this; }
    ToText(errorMessage, ...args) { return this; }

    ToArray(errorMessage) { return this; }
    ToArray(errorMessage, ...args) { return this; }

    ToMyDate(errorMessage) { return this; }
    ToMyDate(errorMessage, ...args) { return this; }
}

class OperandNull extends Operand {
    get IsNull() { return true; }
    get IsNotNull() { return false; }
    get Type() { return OperandType.NULL; }
    toString() { return "null"; }
}

class KeyValue {
    constructor(key, value) {
        this.Key = key;
        this.Value = value;
    }
}

class OperandKeyValueList extends Operand {
    constructor() {
        super();
        this.TextList = [];
    }

    get IsArrayJson() { return true; }
    get IsNotArrayJson() { return false; }
    get Type() { return OperandType.ARRARYJSON; }
    get ArrayValue() { return this.TextList.map(q => q.Value); }

    ToText(errorMessage = null) {
        return Operand.Create(this.toString());
    }
    ToText(errorMessage, ...args) {
        return Operand.Create(this.toString());
    }

    ToArray(errorMessage) {
        return Operand.Create(this.ArrayValue);
    }
    ToArray(errorMessage, ...args) {
        return Operand.Create(this.ArrayValue);
    }

    ToJson(errorMessage = null) {
        const txt = this.toString();
        try {
            const json = JsonMapper.ToObject(txt);
            return Operand.Create(json);
        } catch (e) { }
        return Operand.Error(errorMessage ?? "Convert to json error!");
    }

    AddValue(keyValue) {
        this.TextList.push(keyValue);
    }

    TryGetValue(key) {
        for (const item of this.TextList) {
            if (item.key === key.toString()) {
                return item.value;
            }
        }
        return null;
    }

    ContainsKey(value) {
        for (const item of this.TextList) {
            if (value.TextValue === item.value) {
                return true;
            }
        }
        return false;
    }

    ContainsValue(value) {
        for (const item of this.TextList) {
            const op = item.Value;
            if (value.Type !== op.Type) { continue; }
            if (value.IsText) {
                if (value.TextValue === op.TextValue) {
                    return true;
                }
            }
        }
        return false;
    }

    toString() {
        const elements = this.TextList.map(item => '"' + item.Key + '":' + item.Value.toString());
        return '{' + elements.join(',') + '}';
    }
}

class OperandKeyValue extends Operand {
    constructor(obj) {
        super();
        this._value = obj;
    }
    get IsArrayJson() { return true; }
    get IsNotArrayJson() { return false; }
    get Type() { return OperandType.ARRARYJSON; }
    get Value() { return this._value; }
}

// 导出所有类
export {  OperandDouble,  OperandBoolean, OperandString, OperandMyDate, OperandJson, OperandArray, OperandError, OperandNull, KeyValue, OperandKeyValueList, OperandKeyValue };

// 初始化静态属性
Operand.Version = new OperandString("ToolGood.Algorithm 6.1");
Operand.True = new OperandBoolean(true);
Operand.False = new OperandBoolean(false);
Operand.One = Operand.Create(1);
Operand.Zero = Operand.Create(0);

// 浏览器支持
if (typeof window !== 'undefined') {
    window.Operand = Operand;
    window.OperandDouble = OperandDouble;
    window.OperandBoolean = OperandBoolean;
    window.OperandString = OperandString;
    window.OperandMyDate = OperandMyDate;
    window.OperandJson = OperandJson;
    window.OperandArray = OperandArray;
    window.OperandError = OperandError;
    window.OperandNull = OperandNull;
    window.KeyValue = KeyValue;
    window.OperandKeyValueList = OperandKeyValueList;
    window.OperandKeyValue = OperandKeyValue;
}