using System;
using System.Collections.Generic;
using ToolGood.Algorithm.LitJson;

namespace ToolGood.Algorithm
{
    public class AlgorithmEngineEx : AlgorithmEngine
    {
        private readonly Dictionary<string, Operand> _tempdict;
        /// <summary>
        /// 是否忽略大小写
        /// </summary>
        public bool IgnoreCase { get; private set; }

        /// <summary>
        /// 保存到临时文档
        /// </summary>
        public bool UseTempDict { get; set; } = false;

        #region 构造函数

        /// <summary>
        /// 默认不带缓存
        /// </summary>
        public AlgorithmEngineEx()
        {
            _tempdict = new Dictionary<string, Operand>();
        }

        /// <summary>
        /// 带缓存关键字大小写参数
        /// </summary>
        /// <param name="ignoreCase"></param>
        public AlgorithmEngineEx(bool ignoreCase)
        {
            IgnoreCase = ignoreCase;
            if (ignoreCase) {
                _tempdict = new Dictionary<string, Operand>(StringComparer.OrdinalIgnoreCase);
            } else {
                _tempdict = new Dictionary<string, Operand>();
            }
        }

        #endregion 构造函数

        public override Operand GetParameter(string parameter)
        {
            if (_tempdict.TryGetValue(parameter, out Operand operand)) {
                return operand;
            }
            var result = GetParameter(parameter);
            if (UseTempDict) {
                _tempdict[parameter] = result;
            }
            return result;
        }

        #region Parameter

        /// <summary>
        /// 清理参数
        /// </summary>
        public void ClearParameters()
        {
            _tempdict.Clear();
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, Operand obj)
        {
            _tempdict[key] = obj;
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, bool obj)
        {
            _tempdict[key] = Operand.Create(obj);
        }

        #region number

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, short obj)
        {
            _tempdict[key] = Operand.Create((int)obj);
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, int obj)
        {
            _tempdict[key] = Operand.Create(obj);
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, long obj)
        {
            _tempdict[key] = Operand.Create((double)obj);
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, ushort obj)
        {
            _tempdict[key] = Operand.Create((int)obj);
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, uint obj)
        {
            _tempdict[key] = Operand.Create((double)obj);
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, ulong obj)
        {
            _tempdict[key] = Operand.Create((double)obj);
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, float obj)
        {
            _tempdict[key] = Operand.Create((double)obj);
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, double obj)
        {
            _tempdict[key] = Operand.Create(obj);
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, decimal obj)
        {
            _tempdict[key] = Operand.Create((double)obj);
        }

        #endregion number

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, string obj)
        {
            _tempdict[key] = Operand.Create(obj);
        }

        #region MyDate

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, MyDate obj)
        {
            _tempdict[key] = Operand.Create(obj);
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, DateTime obj)
        {
            _tempdict[key] = Operand.Create(obj);
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, TimeSpan obj)
        {
            _tempdict[key] = Operand.Create(obj);
        }

        #endregion MyDate

        #region array

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, List<Operand> obj)
        {
            _tempdict[key] = Operand.Create(obj);
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, ICollection<string> obj)
        {
            _tempdict[key] = Operand.Create(obj);
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, ICollection<double> obj)
        {
            _tempdict[key] = Operand.Create(obj);
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, ICollection<int> obj)
        {
            _tempdict[key] = Operand.Create(obj);
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, ICollection<bool> obj)
        {
            _tempdict[key] = Operand.Create(obj);
        }

        #endregion array

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="json"></param>
        public void AddParameterFromJson(string json)
        {
            if (json.StartsWith("{") && json.EndsWith("}")) {
                var jo = JsonMapper.ToObject(json);
                if (jo.IsObject) {
                    foreach (var item in jo.inst_object) {
                        var v = item.Value;
                        if (v.IsString)
                            _tempdict[item.Key] = Operand.Create(v.StringValue);
                        else if (v.IsBoolean)
                            _tempdict[item.Key] = Operand.Create(v.BooleanValue);
                        else if (v.IsDouble)
                            _tempdict[item.Key] = Operand.Create(v.NumberValue);
                        else if (v.IsObject)
                            _tempdict[item.Key] = Operand.Create(v);
                        else if (v.IsArray)
                            _tempdict[item.Key] = Operand.Create(v);
                        else if (v.IsNull)
                            _tempdict[item.Key] = Operand.CreateNull();
                    }
                    return;
                }
            }
            throw new Exception("Parameter is not json string.");
        }

        #endregion Parameter
    }
}
