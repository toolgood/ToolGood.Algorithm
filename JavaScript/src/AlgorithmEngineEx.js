import { AlgorithmEngine } from './AlgorithmEngine.js';
import { Operand } from './Operand.js';
import { JsonMapper } from './LitJson/JsonMapper.js';

/**
 * AlgorithmEngine 扩展类
 * 增加自定义参数缓存功能
 */
export class AlgorithmEngineEx extends AlgorithmEngine {
    constructor(ignoreCase = false) {
        super();
        this.IgnoreCase = ignoreCase;
        this.UseTempDict = false;
        this._tempdict = {};
    }

    /**
     * 自定义参数 请重写此方法
     */
    GetParameter(parameter) {
        const key = this.IgnoreCase ? parameter.toLowerCase() : parameter;
        if (this._tempdict[key] !== undefined) {
            return this._tempdict[key];
        }
        const result = super.GetParameter ? super.GetParameter(parameter) : Operand.CreateNull();
        if (this.UseTempDict) {
            this._tempdict[key] = result;
        }
        return result;
    }


    /**
     * 清理参数
     */
    ClearParameters() {
        this._tempdict = {};
    }

    /**
     * 添加自定义参数
     */
    AddParameter(key, obj) {
        const paramKey = this.IgnoreCase ? key.toLowerCase() : key;
        if (obj instanceof Operand) {
            this._tempdict[paramKey] = obj;
        } else {
            this._tempdict[paramKey] = Operand.Create(obj);
        }
    }

    /**
     * 添加自定义参数
     */
    AddParameterFromJson(json) {
        if (json.startsWith('{') && json.endsWith('}')) {
            const jo = JsonMapper.ToObject(json);
            if (jo.IsObject) {
                const obj = jo.EnsureDictionary();
                for (const key in obj) {
                    if (obj.hasOwnProperty(key)) {
                        const v = obj[key];
                        const paramKey = this.IgnoreCase ? key.toLowerCase() : key;
                        if (v.IsString) {
                            this._tempdict[paramKey] = Operand.Create(v.StringValue);
                        } else if (v.IsBoolean) {
                            this._tempdict[paramKey] = Operand.Create(v.BooleanValue);
                        } else if (v.IsDouble) {
                            this._tempdict[paramKey] = Operand.Create(v.NumberValue);
                        } else if (v.IsObject) {
                            this._tempdict[paramKey] = Operand.Create(v);
                        } else if (v.IsArray) {
                            this._tempdict[paramKey] = Operand.Create(v);
                        } else if (v.IsNull) {
                            this._tempdict[paramKey] = Operand.CreateNull();
                        }
                    }
                }
                return;
            }
        }
        throw new Error("Parameter is not json string.");
    }

}

// 浏览器支持
if (typeof window !== 'undefined') {
    window.AlgorithmEngineEx = AlgorithmEngineEx;
}