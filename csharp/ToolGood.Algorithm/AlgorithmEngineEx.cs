using System;
using System.Collections.Generic;
using System.Data.Common;
using ToolGood.Algorithm.LitJson;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm
{
    /// <summary>
    /// AlgorithmEngine 扩展类
    /// 增加自定义参数缓存功能
    /// </summary>
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

		/// <summary>
		/// AlgorithmEngineEx 请重写 GetParameterEx
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public override Operand GetParameter(string parameter)
        {
            if (_tempdict.TryGetValue(parameter, out Operand operand)) {
                return operand;
            }
            var result = GetParameterEx(parameter);
            if (UseTempDict) {
                _tempdict[parameter] = result;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public virtual Operand GetParameterEx(string parameter)
        {
			return Operand.Error($"Parameter [{parameter}] is missing.");
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
            _tempdict[key] = Operand.Create(obj);
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
            _tempdict[key] = Operand.Create(obj);
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, ushort obj)
        {
            _tempdict[key] = Operand.Create(obj);
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, uint obj)
        {
            _tempdict[key] = Operand.Create(obj);
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, ulong obj)
        {
            _tempdict[key] = Operand.Create(obj);
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void AddParameter(string key, float obj)
        {
            _tempdict[key] = Operand.Create(obj);
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
            _tempdict[key] = Operand.Create(obj);
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
            if (json.StartsWith('{') && json.EndsWith('}')) {
                var jo = JsonMapper.ToObject(json);
                if (jo.IsObject) {
                    foreach (var item in jo.inst_object) {
                        var v = item.Value;
                        _tempdict[item.Key] = v switch
                        {
                            _ when v.IsString => Operand.Create(v.StringValue),
                            _ when v.IsBoolean => Operand.Create(v.BooleanValue),
                            _ when v.IsDouble => Operand.Create(v.NumberValue),
                            _ when v.IsObject => Operand.Create(v),
                            _ when v.IsArray => Operand.Create(v),
                            _ when v.IsNull => Operand.Null,
                            _ => Operand.Create(v)
                        };
                    }
                    return;
                }
            }
            throw new Exception("Parameter is not json string.");
        }

        #endregion Parameter
    }
}