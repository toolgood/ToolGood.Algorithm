using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm
{
    /// <summary>
    /// 操作数
    /// </summary>
    public class Operand
    {
        #region Constructed Function
        public Operand(OperandType type, object value)
        {
            this.Type = type;

            this.Value = value;
        }
        public Operand(OperandType type, float value)
        {
            this.Type = type;
            this.Value = (double) value;
        }
        public Operand(OperandType type, double value)
        {
            this.Type = type;
            this.Value = (double) value;
        }
        internal Operand(OperandType type, Date value)
        {
            this.Type = type;
            this.Value = (Date) value;
        }
        public Operand(OperandType type, string value)
        {
            this.Type = type;
            this.Value = value;
        }
        public Operand(OperandType type, bool value)
        {
            this.Type = type;
            this.Value = value;
        }
        public Operand(List<Operand> value)
        {
            this.Type = OperandType.ARRARY;
            this.Value = value;
        }

        #endregion

        #region Variable &　Property



        /// <summary>
        /// 操作数类型
        /// </summary>
        public OperandType Type { get; set; }

        /// <summary>
        /// 操作数值
        /// </summary>
        internal object Value { get; private set; }

        public double NumberValue
        {
            get
            {
                if (Type == OperandType.BOOLEAN)
                {
                    return (bool) Value ? 1 : 0;
                }
                if (Value is string)
                {
                    if (double.TryParse(Value.ToString(), NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"), out double d))
                    {
                        return d;
                    }
                }
                return (double) Value;
            }
        }
        public string StringValue { get { return Value.ToString(); } }
        public bool BooleanValue
        {
            get
            {
                if (Type == OperandType.NUMBER)
                {
                    if (Value is double)
                    {
                        return (double) Value != 0;
                    }
                    return decimal.Parse(Value.ToString()) != 0;
                }
                return (bool) Value;
            }
        }
        internal Date DateValue { get { return (Date) Value; } }
        public int IntValue { get { return (int) (double) Value; } }

        public List<Operand> GetValueList()
        {
            List<Operand> list = new List<Operand>();
            if (Value is List<Operand>)
            {
                var ls = Value as List<Operand>;
                foreach (var item in ls)
                {
                    if (item.Type == OperandType.ARRARY)
                    {
                        list.AddRange(item.GetValueList());
                    }
                    else
                    {
                        list.Add(item);
                    }
                }
            }
            else
            {
                list.Add(this);
            }
            return list;
        }

        #endregion


        public Operand ToNumber(string title)
        {
            if (Type == OperandType.ERROR)
            {
                return this;
            }
            if (Type == OperandType.NUMBER)
            {
                return this;
            }
            if (Type == OperandType.BOOLEAN)
            {
                Type = OperandType.NUMBER;
                Value = (bool) Value ? 1.0 : 0.0;
                return this;
            }
            if (Type == OperandType.DATE)
            {
                Type = OperandType.NUMBER;
                Value = (double) (Date) Value;
                return this;
            }
            if (Type == OperandType.STRING)
            {
                if (double.TryParse(Value.ToString(), NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"), out double d))
                {
                    Type = OperandType.NUMBER;
                    Value = d;
                    return this;
                }
            }
            return new Operand(OperandType.ERROR, title + "无法转成数字！");
        }



    }
}
