import { OperandType } from './Enums/index.js';
import { JsonMapper } from './LitJson/JsonMapper.js';
import { JsonData } from './LitJson/JsonData.js';
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
     * 是否未指定值(对齐 C# OperandNone)
     */
    get IsNone() { return false; }

    /**
     * 是否错误或空值(对齐 C# IsErrorOrNone)
     */
    get IsErrorOrNone() { return this.IsError || this.IsNone; }

    /**
     * 是否数字
     */
    get IsNumber() { return false; }

    /**
     * 是否字符串
     */
    get IsText() { return false; }

    /**
     * 是否布尔值
     */
    get IsBoolean() { return false; }

    /**
     * 是否数组
     */
    get IsArray() { return false; }

    /**
     * 是否日期
     */
    get IsDate() { return false; }

    /**
     * 是否Json对象
     */
    get IsJson() { return false; }

    /**
     * 是否Json数组
     */
    get IsArrayJson() { return false; }

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
    get Type() { throw new Error('FIXME'); }

    /**
     * 数字值
     */
    get NumberValue() { throw new Error('FIXME'); }
    
    /**
     * double值
     */
    get DoubleValue() { throw new Error('FIXME'); }

    /**
     * int值
     */
    get IntValue() { throw new Error('FIXME'); }

    /**
     * long值
     */
    get LongValue() { throw new Error('FIXME'); }

    /**
     * 字符串值
     */
    get TextValue() { throw new Error('FIXME'); }

    /**
     * 布尔值
     */
    get BooleanValue() { throw new Error('FIXME'); }

    /**
     * 数组值
     */
    get ArrayValue() { throw new Error('FIXME'); }

    get JsonValue() { throw new Error('FIXME'); }

    /**
     * 时间值
     */
    get DateValue() { throw new Error('FIXME'); }

    /**
     * 创建操作数
     */
    static Create(obj) {
        if (obj instanceof Operand) {
            return obj;
        } else if (typeof obj === 'boolean') {
            return obj ? Operand.True : Operand.False;
        } else if (typeof obj === 'number') {
            if (Number.isNaN(obj) || !Number.isFinite(obj)) {
                return Operand.Error("Number is NaN or Infinity!");
            }
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
                let element =Operand.Create(obj[index]);
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
                let json = JsonMapper.toObject(txt);
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
                let i = parseInt(match.substring(1, match.length - 1));
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
                let i = parseInt(match.substring(1, match.length - 1));
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
                let i = parseInt(match.substring(1, match.length - 1));
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
                let i = parseInt(match.substring(1, match.length - 1));
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
                let i = parseInt(match.substring(1, match.length - 1));
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
                let i = parseInt(match.substring(1, match.length - 1));
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
    get Type() { return OperandType.NUMBER; }
    get IntValue() { return Math.trunc(this._value); }
    get NumberValue() { return this._value; }
    get LongValue() { return Math.trunc(this._value); }
    get DoubleValue() { return this._value; }

    ToNumber(errorMessage) { return this; }
    ToNumber(errorMessage, ...args) { return this; }

    ToBoolean(errorMessage) {
        if (this._value === 0) {
            return Operand.False;
        } else if (this._value === 1) {
            return Operand.True;
        }
        return super.ToBoolean(errorMessage);
    }
    ToBoolean(errorMessage, ...args) {
        if (this._value === 0) {
            return Operand.False;
        } else if (this._value === 1) {
            return Operand.True;
        }
        return super.ToBoolean(errorMessage, ...args);
    }

    ToText(errorMessage) { return Operand.Create(this.DoubleValue.toString()); }
    ToText(errorMessage, ...args) { return Operand.Create(this.DoubleValue.toString()); }

    toString() { return this.DoubleValue.toString(); }
}

class OperandBoolean extends Operand {
    constructor(obj) {
        super();
        this._value = obj;
    }
    get IsBoolean() { return true; }
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
    get Type() { return OperandType.TEXT; }
    get TextValue() { return this._value; }

    ToNumber(errorMessage) {
        return this.toNumberInternal(errorMessage);
    }
    ToNumber(errorMessage, ...args) {
        return this.toNumberInternal(this.replaceErrorMessage(errorMessage, args));
    }

    // 与 C# decimal.TryParse(NumberStyles.Any, InvariantCulture) 对齐的严格解析
    toNumberInternal(errorMessage) {
        let parsed = this.tryParseNumber(this.TextValue);
        if (parsed === null) {
            if (errorMessage == null) {
                return Operand.Error("Convert to number error!");
            }
            return Operand.Error(errorMessage);
        }
        return Operand.Create(parsed);
    }

    // 严格数字解析:失败返回 null,杜绝 parseInt/parseFloat 的部分解析
    tryParseNumber(text) {
        // AllowLeadingWhite | AllowTrailingWhite
        let t = text.trim();
        if (t.length === 0) {
            return null;
        }
        let negative = false;
        // AllowParentheses:括号表示负数
        if (t.startsWith('(') && t.endsWith(')')) {
            negative = true;
            t = t.substring(1, t.length - 1).trim();
        }
        // AllowCurrencySymbol(InvariantCulture 货币符号 ¤)
        if (t.startsWith('¤')) {
            t = t.substring(1).trim();
        }
        // AllowLeadingSign
        if (t.startsWith('+')) {
            t = t.substring(1);
        } else if (t.startsWith('-')) {
            negative = !negative;
            t = t.substring(1);
        }
        t = t.trim();
        // AllowTrailingSign
        if (t.endsWith('-')) {
            negative = !negative;
            t = t.substring(0, t.length - 1).trim();
        } else if (t.endsWith('+')) {
            t = t.substring(0, t.length - 1).trim();
        }
        // AllowThousands:千分位逗号直接忽略
        t = t.replace(/,/g, '');
        // 严格校验数字格式(AllowDecimalPoint | AllowExponent),避免部分解析
        if (!/^[+-]?(\d+\.?\d*|\.\d+)([eE][+-]?\d+)?$/.test(t)) {
            return null;
        }
        let num = parseFloat(t);
        if (isNaN(num)) {
            return null;
        }
        return negative ? -num : num;
    }

    // 替换 {0} {1} 占位符,对齐 C# string.Format
    replaceErrorMessage(errorMessage, args) {
        if (args.length === 0) {
            return errorMessage;
        }
        return errorMessage.replace(/\{\d+\}/g, (match) => {
            let i = parseInt(match.substring(1, match.length - 1));
            return args[i] !== undefined ? args[i] : match;
        });
    }

    ToText(errorMessage) { return this; }
    ToText(errorMessage, ...args) { return this; }

    ToBoolean(errorMessage) {
        let b = FunctionUtil.TryParseBoolean(this.TextValue);
        if (b !== null) {
            return b ? Operand.True : Operand.False;
        }
        if (errorMessage == null) {
            return Operand.Error("Convert to bool error!");
        }
        return Operand.Error(errorMessage);
    }
    ToBoolean(errorMessage, ...args) {
        let b = FunctionUtil.TryParseBoolean(this.TextValue);
        if (b !== null) {
            return b ? Operand.True : Operand.False;
        }
        if (errorMessage == null) {
            return Operand.Error("Convert to bool error!");
        }
        return Operand.Error(errorMessage);
    }

    ToMyDate(errorMessage) {
        return this.toMyDateInternal(errorMessage);
    }
    ToMyDate(errorMessage, ...args) {
        return this.toMyDateInternal(this.replaceErrorMessage(errorMessage, args));
    }

    toMyDateInternal(errorMessage) {
        // 对齐 C# TimeSpan.TryParse:纯时间或"天.时间"格式 → MyDate(TimeSpan)
        let timeSpan = this.tryParseTimeSpan(this.TextValue);
        if (timeSpan !== null) {
            return Operand.Create(timeSpan);
        }
        try {
            let text = this.TextValue.replaceAll("/", "-");
            let date = new Date(text);
            if (!isNaN(date.getTime())) {
                // ISO 格式（YYYY-MM-DD...）在 JS 中按 UTC 解析，用 UTC 分量还原，避免本地时区偏移
                if (/^\d{4}-\d{2}-\d{2}/.test(text.trim())) {
                    return Operand.Create(new MyDate(date.getUTCFullYear(), date.getUTCMonth() + 1, date.getUTCDate(), date.getUTCHours(), date.getUTCMinutes(), date.getUTCSeconds()));
                }
                return Operand.Create(new MyDate(date));
            }
        } catch (e) { }
        if (errorMessage == null) {
            return Operand.Error("Convert to date error!");
        }
        return Operand.Error(errorMessage);
    }

    // 对齐 C# TimeSpan.TryParse 的时间跨度解析:失败返回 null
    tryParseTimeSpan(text) {
        let t = text.trim();
        // 天.时分秒 / 天.时分
        let dayMatch = t.match(/^(-?\d+)\.(\d{1,2}):(\d{2})(?::(\d{2}))?$/);
        if (dayMatch) {
            return new MyDate({
                days: parseInt(dayMatch[1]),
                hours: parseInt(dayMatch[2]),
                minutes: parseInt(dayMatch[3]),
                seconds: dayMatch[4] ? parseInt(dayMatch[4]) : 0
            });
        }
        // 纯天数
        if (/^-?\d+$/.test(t)) {
            return new MyDate({ days: parseInt(t), hours: 0, minutes: 0, seconds: 0 });
        }
        // 时分秒 / 时分
        let timeMatch = t.match(/^(-?)(\d{1,2}):(\d{2})(?::(\d{2}))?$/);
        if (timeMatch) {
            let negative = timeMatch[1] === '-';
            let hours = parseInt(timeMatch[2]);
            let minutes = parseInt(timeMatch[3]);
            let seconds = timeMatch[4] ? parseInt(timeMatch[4]) : 0;
            if (negative) {
                hours = -hours;
                minutes = -minutes;
                seconds = -seconds;
            }
            return new MyDate({ days: 0, hours: hours, minutes: minutes, seconds: seconds });
        }
        return null;
    }

    ToArray(errorMessage) {
        return Operand.Error(errorMessage ?? "Convert to array error!");
    }
    ToJson(errorMessage = null) {
        let txt = this.TextValue.trim();
        if ((txt.startsWith('{') && txt.endsWith('}')) || (txt.startsWith('[') && txt.endsWith(']'))) {
            try {
                let json = JsonMapper.toObject(txt);
                return Operand.Create(json);
            } catch (e) { }
        }
        return Operand.Error(errorMessage ?? "Convert to json error!");
    }

    toString() {
        let result = '"';
        for (let c of this._value) {
            switch (c) {
                case '"': result += '\\"'; break;
                case '\\': result += '\\\\'; break;
                case '\n': result += '\\n'; break;
                case '\r': result += '\\r'; break;
                case '\t': result += '\\t'; break;
                case '\b': result += '\\b'; break;
                case '\f': result += '\\f'; break;
                default:
                    // 对齐 C#:其他控制字符输出 \uXXXX
                    result += c < ' ' ? '\\u' + c.charCodeAt(0).toString(16).padStart(4, '0') : c;
                    break;
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
    get Type() { return OperandType.JSON; }
    get JsonValue() { return this._value; }
    get IsArrayJson() { return false; }

    ToText(errorMessage = null) {
        return Operand.Create(this._value.toString());
    }
    ToText(errorMessage, ...args) {
        return Operand.Create(this._value.toString());
    }

    ToArray(errorMessage) {
        if (this.JsonValue.IsArray) {
            let list = [];
            for (let v of this.JsonValue) {
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
            let list = [];
            for (let v of this.JsonValue) {
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
            let value = this.JsonValue.inst_object[key];
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
    get Type() { return OperandType.ARRAY; }
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
        let txt = this.toString();
        try {
            let json = JsonMapper.toObject(txt);
            return Operand.Create(json);
        } catch (e) { }
        return Operand.Error(errorMessage ?? "Convert to json error!");
    }

    toString() {
        let elements = this.ArrayValue.map(item => item.toString());
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

class OperandNone extends Operand {
    get IsNone() { return true; }
    get IsErrorOrNone() { return true; }
    get Type() { return OperandType.NONE; }
    toString() { return "none"; }
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
    get Type() { return OperandType.ARRAYJSON; }
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
        let txt = this.toString();
        try {
            let json = JsonMapper.toObject(txt);
            return Operand.Create(json);
        } catch (e) { }
        return Operand.Error(errorMessage ?? "Convert to json error!");
    }

    AddValue(keyValue) {
        this.TextList.push(keyValue);
    }

    TryGetValue(key) {
        for (let item of this.TextList) {
            if (item.key === key.toString()) {
                return item.value;
            }
        }
        return null;
    }

    ContainsKey(value) {
        for (let item of this.TextList) {
            if (item.key === value.TextValue) {
                return true;
            }
        }
        return false;
    }

    ContainsValue(value) {
        for (let item of this.TextList) {
            let op = item.value;
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
        let elements = this.TextList.map(item => new OperandString(item.key).toString() + ':' + item.value.toString());
        return '{' + elements.join(',') + '}';
    }
}

class OperandKeyValue extends Operand {
    constructor(obj) {
        super();
        this._value = obj;
    }
    get IsArrayJson() { return true; }
    get Type() { return OperandType.ARRAYJSON; }
    get Value() { return this._value; }
}

// 导出所有类
export {  OperandDouble,   OperandBoolean, OperandString, OperandMyDate, OperandJson, OperandArray, OperandError, OperandNull, OperandNone, KeyValue, OperandKeyValueList, OperandKeyValue };

// 初始化静态属性
Operand.Version = new OperandString("ToolGood.Algorithm 6.3");
Operand.True = new OperandBoolean(true);
Operand.False = new OperandBoolean(false);
Operand.One = Operand.Create(1);
Operand.Zero = Operand.Create(0);
Operand.None = new OperandNone();

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
    window.OperandNone = OperandNone;
    window.KeyValue = KeyValue;
    window.OperandKeyValueList = OperandKeyValueList;
    window.OperandKeyValue = OperandKeyValue;
}