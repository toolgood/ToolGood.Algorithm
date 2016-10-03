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
            addFunc("DATEDIF ", DATEDIF);//返回两个日期之间的隔数　 天数
            addFunc("DAY", DAY);//将序列号转换为月份中的日 
            addFunc("DAYS360", DAYS360);//以一年 360 天为基准计算两个日期间的天数 
            addFunc("EDATE", EDATE);//返回用于表示开始日期之前或之后月数的日期的序列号 
            addFunc("EOMONTH", EOMONTH);//返回指定月数之前或之后的月份的最后一天的序列号 
            addFunc("HOUR", HOUR);//将序列号转换为小时 
            addFunc("MINUTE", MINUTE);//将序列号转换为分钟 
            addFunc("MONTH", MONTH);//将序列号转换为月 
            //addFunc("NETWORKDAYS", NETWORKDAYS);//返回两个日期之间的全部工作日数 
            addFunc("NOW", NOW);//返回当前日期和时间的序列号 
            addFunc("SECOND", SECOND);//将序列号转换为秒 
            addFunc("TIME", TIME);//返回特定时间的序列号 
            addFunc("TIMEVALUE", TIMEVALUE);//将文本格式的时间转换为序列号 
            addFunc("TODAY", TODAY);//返回今天日期的序列号 
            addFunc("WEEKDAY", WEEKDAY);//将序列号转换为星期几 
            //addFunc("WEEKNUM", WEEKNUM);//将序列号转换为一年中相应的周数 
            //addFunc("WORKDAY", WORKDAY);//返回指定的若干个工作日之前或之后的日期的序列号 
            addFunc("YEAR", YEAR);//将序列号转换为年 
            //addFunc("YEARFRAC", YEARFRAC);//返回代表 start_date 和 end_date 之间整天天数的年分数 
        }

        private Operand EOMONTH(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("DATEDIF中参数不足", new List<Operand>());
            var dt = ((DateTime)arg[0].DateValue).AddMonths(arg[1].IntValue+1);
            dt = new DateTime(dt.Year, dt.Month, 1).AddDays(-1);
            return new Operand(OperandType.DATE, (Date)dt);
        }

        private Operand EDATE(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("DATEDIF中参数不足", new List<Operand>());
            return new Operand(OperandType.DATE, (Date)(((DateTime)arg[0].DateValue).AddMonths(arg[1].IntValue)));
        }

        private Operand TIMEVALUE(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("DATEDIF中参数不足", new List<Operand>());
            return new Operand(OperandType.DATE, new Date(TimeSpan.Parse(arg[0].StringValue)));
        }

        private Operand DAYS360(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("DATEDIF中参数不足", new List<Operand>());
            var startDate = arg[0].DateValue;
            var endDate = arg[1].DateValue;

            var days = startDate.Year * 360 + startDate.Month * 30 + startDate.Day
                 - endDate.Year * 360 - endDate.Month * 30 - endDate.Day;
            return new Operand(OperandType.NUMBER, days);
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
            return new Operand(OperandType.NUMBER, ((DateTime)arg[0].DateValue).DayOfWeek + 1);
        }

        private Operand YEAR(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("YEAR中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, arg[0].DateValue.Year);
        }

        private Operand SECOND(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("SECOND中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, arg[0].DateValue.Second);
        }

        private Operand MONTH(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("MONTH中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, arg[0].DateValue.Month);
        }

        private Operand MINUTE(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("MINUTE中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, arg[0].DateValue.Minute);
        }

        private Operand HOUR(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("HOUR中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, arg[0].DateValue.Hour);
        }

        private Operand DAY(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("DAY中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, arg[0].DateValue.Day);
        }

        private Operand DATEDIF(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("DATEDIF中参数不足", new List<Operand>());
            var startDate = arg[0].DateValue;
            var endDate = arg[1].DateValue;
            return new Operand(OperandType.NUMBER, (int)((double)startDate - (double)endDate));


            ////var t = arg[2].StringValue;
            //if (t == "y") {
            //    return new Operand(OperandType.NUMBER, (int)(double)(startDate - endDate) / 365);
            //} else if (t == "m") {
            //    return new Operand(OperandType.NUMBER, (int)(double)(startDate - endDate) / 30);
            //} else if (t == "d") {
            //    return new Operand(OperandType.NUMBER, (int)(double)(startDate - endDate));
            //}
            //return throwError("DATE比较类型错误", new List<Operand>());
        }

        private Operand DATEVALUE(List<Operand> arg)
        {
            if (arg.Count < 3) return throwError("DATE中参数不足", new List<Operand>());
            var d = new Date(arg[0].StringValue);
            return new Operand(OperandType.DATE, d);
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
