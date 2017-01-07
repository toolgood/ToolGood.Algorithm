using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm
{
    partial class AlgorithmEngine
    {
        private void AddDateTimeFunction()
        {
            addFunc("DATE", DATE);//返回特定日期的序列号 
            addFunc("DATEVALUE", DATEVALUE);//将文本格式的日期转换为序列号 
            addFunc("DATEDIF", DATEDIF);//返回两个日期之间的隔数　 天数
            addFunc("DAY", DAY);//将序列号转换为月份中的日 
            addFunc("DAYS360", DAYS360);//以一年 360 天为基准计算两个日期间的天数 
            addFunc("EDATE", EDATE);//返回用于表示开始日期之前或之后月数的日期的序列号 
            addFunc("EOMONTH", EOMONTH);//返回指定月数之前或之后的月份的最后一天的序列号 
            addFunc("HOUR", HOUR);//将序列号转换为小时 
            addFunc("MINUTE", MINUTE);//将序列号转换为分钟 
            addFunc("MONTH", MONTH);//将序列号转换为月 
            addFunc("NETWORKDAYS", NETWORKDAYS);//返回两个日期之间的全部工作日数 
            addFunc("NOW", NOW);//返回当前日期和时间的序列号 
            addFunc("SECOND", SECOND);//将序列号转换为秒 
            addFunc("TIME", TIME);//返回特定时间的序列号 
            addFunc("TIMEVALUE", TIMEVALUE);//将文本格式的时间转换为序列号 
            addFunc("TODAY", TODAY);//返回今天日期的序列号 
            addFunc("WEEKDAY", WEEKDAY);//将序列号转换为星期几 
            addFunc("WEEKNUM", WEEKNUM);//将序列号转换为一年中相应的周数 
            addFunc("WORKDAY", WORKDAY);//返回指定的若干个工作日之前或之后的日期的序列号 
            addFunc("YEAR", YEAR);//将序列号转换为年 
            //addFunc("YEARFRAC", YEARFRAC);//返回代表 start_date 和 end_date 之间整天天数的年分数 
        }

        private Operand WEEKNUM(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("WEEKNUM中参数不足", new List<Operand>());
            var startDate = (DateTime)arg[0].DateValue;

            var days = startDate.DayOfYear + (int)(new DateTime(startDate.Year, 1, 1).DayOfWeek);
            if (arg.Count == 2 && arg[1].IntValue ==2) days--;

            var week = Math.Ceiling(days / 7.0);
            return new Operand(OperandType.DATE, week);
        }


        private Operand WORKDAY(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("WORKDAY中参数不足", new List<Operand>());
            var startDate = (DateTime)arg[0].DateValue;
            var days = arg[1].IntValue;
            List<DateTime> list = new List<DateTime>();
            for (int i = 2; i < arg.Count; i++) {
                list.Add(arg[i].DateValue);
            }
            while (days > 0) {
                startDate = startDate.AddDays(1);
                if (startDate.DayOfWeek == DayOfWeek.Saturday) continue;
                if (startDate.DayOfWeek == DayOfWeek.Sunday) continue;
                if (list.Contains(startDate)) continue;
                days--;
            }
            return new Operand(OperandType.DATE, (Date)startDate);
        }

        private Operand NETWORKDAYS(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("NETWORKDAYS中参数不足", new List<Operand>());
            var startDate = (DateTime)arg[0].DateValue;
            var endDate = (DateTime)arg[1].DateValue;
            List<DateTime> list = new List<DateTime>();
            for (int i = 3; i < arg.Count; i++) {
                list.Add(arg[i].DateValue);
            }
            var days = 0;
            while (startDate <= endDate) {
                if (startDate.DayOfWeek != DayOfWeek.Sunday && startDate.DayOfWeek != DayOfWeek.Saturday) {
                    if (list.Contains(startDate) == false) {
                        days++;
                    }
                }
                startDate = startDate.AddDays(1);
            }
            return new Operand(OperandType.DATE, (double)days);
        }


        private Operand EOMONTH(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("EOMONTH中参数不足", new List<Operand>());
            var dt = ((DateTime)arg[0].DateValue).AddMonths(arg[1].IntValue + 1);
            dt = new DateTime(dt.Year, dt.Month, 1).AddDays(-1);
            return new Operand(OperandType.DATE, (Date)dt);
        }

        private Operand EDATE(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("EDATE中参数不足", new List<Operand>());
            return new Operand(OperandType.DATE, (Date)(((DateTime)arg[0].DateValue).AddMonths(arg[1].IntValue)));
        }

        private Operand TIMEVALUE(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("TIMEVALUE中参数不足", new List<Operand>());
            return new Operand(OperandType.DATE, new Date(TimeSpan.Parse(arg[0].StringValue)));
        }

        private Operand DAYS360(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("DAYS360中参数不足", new List<Operand>());
            var startDate = (DateTime)arg[0].DateValue;
            var endDate = (DateTime)arg[1].DateValue;

            var method = false;
            if (arg.Count == 3) method = arg[2].BooleanValue;
            var days = endDate.Year * 360 + (endDate.Month - 1) * 30
                        - startDate.Year * 360 - (startDate.Month - 1) * 30;
            if (method) {
                if (endDate.Day == 31) days += 30;
                if (startDate.Day == 31) days -= 30;
            } else {
                if (startDate.Day == new DateTime(startDate.Year, startDate.Month + 1, 1).AddDays(-1).Day) {
                    days -= 30;
                } else {
                    days -= startDate.Day;
                }
                if (endDate.Day == new DateTime(endDate.Year, endDate.Month + 1, 1).AddDays(-1).Day) {
                    if (startDate.Day < 30) {
                        days += 31;
                    } else {
                        days += 30;
                    }
                } else {
                    days += endDate.Day;
                }
            }
            return new Operand(OperandType.NUMBER, (double)days);
        }

        private Operand TIME(List<Operand> arg)
        {
            if (arg.Count < 3) return throwError("DATE中参数不足", new List<Operand>());
            Date d = new Date(0, 0, 0, arg[0].IntValue, arg[1].IntValue, arg[2].IntValue);
            return new Operand(OperandType.DATE, d);
        }

        private Operand TODAY(List<Operand> arg)
        {
            return new Operand(OperandType.DATE, new Date(DateTime.Today));
        }

        private Operand NOW(List<Operand> arg)
        {
            return new Operand(OperandType.DATE, new Date(DateTime.Now));
        }


        private Operand WEEKDAY(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("YEAR中参数不足", new List<Operand>());
            var type = 1;
            if (arg.Count == 2) type = arg[1].IntValue;
            var t = ((DateTime)arg[0].DateValue).DayOfWeek;
            if (type == 1) {
                return new Operand(OperandType.NUMBER, (double)(t + 1));
            } else if (type == 2) {
                if (t == 0) return new Operand(OperandType.NUMBER, 7d);
                return new Operand(OperandType.NUMBER, (double)t);
            }
            if (t == 0) {
                return new Operand(OperandType.NUMBER, 6d);
            }
            return new Operand(OperandType.NUMBER, (double)(t - 1));
        }

        private Operand YEAR(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("YEAR中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, (double)arg[0].DateValue.Year);
        }

        private Operand SECOND(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("SECOND中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, (double)arg[0].DateValue.Second);
        }

        private Operand MONTH(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("MONTH中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, (double)arg[0].DateValue.Month);
        }

        private Operand MINUTE(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("MINUTE中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, (double)arg[0].DateValue.Minute);
        }

        private Operand HOUR(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("HOUR中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, (double)arg[0].DateValue.Hour);
        }

        private Operand DAY(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("DAY中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, (double)arg[0].DateValue.Day);
        }

        private Operand DATEDIF(List<Operand> arg)
        {
            if (arg.Count < 3) return throwError("DATEDIF中参数不足", new List<Operand>());
            var startDate = (DateTime)arg[0].DateValue;
            var endDate = (DateTime)arg[1].DateValue;
            var t = arg[2].StringValue.ToLower();

            if (t == "y") {
                #region y
                bool b = false;
                if (startDate.Month < endDate.Month) {
                    b = true;
                } else if (startDate.Month == endDate.Month) {
                    if (startDate.Day <= endDate.Day) b = true;
                }
                if (b) {
                    return new Operand(OperandType.NUMBER, (double)(endDate.Year - startDate.Year));
                } else {
                    return new Operand(OperandType.NUMBER, (double)(endDate.Year - startDate.Year - 1));
                }
                #endregion
            } else if (t == "m") {
                #region m
                bool b = false;
                if (startDate.Day <= endDate.Day) b = true;
                if (b) {
                    return new Operand(OperandType.NUMBER, (double)(endDate.Year * 12 + endDate.Month - startDate.Year * 12 - startDate.Month));
                } else {
                    return new Operand(OperandType.NUMBER, (double)(endDate.Year * 12 + endDate.Month - startDate.Year * 12 - startDate.Month - 1));
                }
                #endregion
            } else if (t == "d") {
                return new Operand(OperandType.NUMBER, (double)(endDate - startDate).Days);
            } else if (t == "yd") {
                #region yd
                var day = endDate.DayOfYear - startDate.DayOfYear;
                if (endDate.Year > startDate.Year && day < 0) {
                    var days = new DateTime(startDate.Year, 12, 31).DayOfYear;
                    day = days + day;
                }
                return new Operand(OperandType.NUMBER, (double)(day));
                #endregion
            } else if (t == "md") {
                #region md
                var mo = endDate.Day - startDate.Day;
                if (mo < 0) {
                    var days = new DateTime(startDate.Year, startDate.Month + 1, 1).AddDays(-1).Day;
                    mo += days;
                }
                return new Operand(OperandType.NUMBER, (double)(mo));
                #endregion
            } else if (t == "ym") {
                #region ym
                var mo = endDate.Month - startDate.Month;
                if (endDate.Day < startDate.Day) mo = mo - 1;
                if (mo < 0) mo += 12;
                return new Operand(OperandType.NUMBER, (double)(mo));
                #endregion
            }
            return throwError("DATE比较类型错误", new List<Operand>());
        }

        private Operand DATEVALUE(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("DATE中参数不足", new List<Operand>());
            var d = DateTime.Parse(arg[0].StringValue).Date;

            return new Operand(OperandType.DATE, (Date)d);
        }

        private Operand DATE(List<Operand> arg)
        {
            if (arg.Count < 3) return throwError("DATE中参数不足", new List<Operand>());
            Date d;
            if (arg.Count == 3) {
                d = new Date(arg[0].IntValue, arg[1].IntValue, arg[2].IntValue);
            } else if (arg.Count == 4) {
                d = new Date(arg[0].IntValue, arg[1].IntValue, arg[2].IntValue, arg[3].IntValue);
            } else if (arg.Count == 5) {
                d = new Date(arg[0].IntValue, arg[1].IntValue, arg[2].IntValue, arg[3].IntValue, arg[4].IntValue);
            } else {
                d = new Date(arg[0].IntValue, arg[1].IntValue, arg[2].IntValue, arg[3].IntValue, arg[4].IntValue, arg[5].IntValue);
            }
            return new Operand(OperandType.DATE, d);
        }
    }
}
