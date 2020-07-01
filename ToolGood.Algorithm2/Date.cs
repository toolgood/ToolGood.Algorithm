using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm
{
    public class Date
    {
        public Date(DateTime dt)
        {
            Year = dt.Year;
            Month = dt.Month;
            Day = dt.Day;
            Hour = dt.Hour;
            Minute = dt.Minute;
            Second = dt.Second;
            Millisecond = dt.Millisecond;
        }
        public Date(TimeSpan dt)
        {
            Day = dt.Days;
            Hour = dt.Hours;
            Minute = dt.Minutes;
            Second = dt.Seconds;
            Millisecond = dt.Milliseconds;
        }

        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
        public int Millisecond { get; set; }



        public override string ToString()
        {
            if (Year > 0 && Hour > 0 && Second > 0) {
                ((DateTime)this).ToString("yyyy-MM-dd HH:mm:ss");
            } else if (Year > 0 && Hour > 0) {
                ((DateTime)this).ToString("yyyy-MM-dd HH:mm");
            } else if (Year > 0) {
                ((DateTime)this).ToString("yyyy-MM-dd");
            } else if (Hour > 0 && Second > 0) {
                ((DateTime)this).ToString("HH:mm:ss");
            } else {
                ((DateTime)this).ToString("HH:mm:ss");
            }
        }

        public string ToString(string f)
        {
            return ((DateTime)this).ToString(f);
        }

        #region operator
        public static implicit operator Date(DateTime date)
        {
            return new Date(date);
        }
        public static implicit operator DateTime(Date date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
        }
        public static implicit operator TimeSpan(Date date)
        {
            return new TimeSpan(date.Hour, date.Minute, date.Second);
        }

        public static implicit operator Date(double days)
        {
            var dt = DateTime.MinValue.AddDays((int)days);
            dt = dt.AddSeconds((days - (int)days) * 24 * 60 * 60);
            return new Date(dt);
        }
        public static implicit operator double(Date date)
        {

            if (date.Year > 1900) {
                var dt = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
                double days = (double)(dt - DateTime.MinValue).TotalDays;
                days += (date.Hour + (date.Minute + date.Second / 60.0) / 60) / 24;
                return days;
            }
            return date.Day + (date.Hour + (date.Minute + date.Second / 60.0) / 60) / 24;
        }



        public static Date operator +(Date date, Date num)
        {
            return (Date)((double)date + (double)num);
        }
        public static Date operator -(Date date, Date num)
        {
            var dt = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
            var dt2 = new DateTime(num.Year, num.Month, num.Day, num.Hour, num.Minute, num.Second);

            return date;
        }



        public static Date operator +(Date date, double num)
        {
            return (Date)((double)date + (double)num);
        }
        public static Date operator -(Date date, double num)
        {
            return (Date)((double)date - (double)num);
        }
        public static Date operator *(Date date, double num)
        {
            return (Date)((double)date * (double)num);
        }
        public static Date operator /(Date date, double num)
        {
            return (Date)((double)date / (double)num);
        }
        public static Date operator %(Date date, double num)
        {
            return (Date)((double)date % (double)num);
        }





        public static double operator +(double num, Date date)
        {
            return num + (double)date;
        }
        public static double operator -(double num, Date date)
        {
            return num - (double)date;
        }
        public static double operator *(double num, Date date)
        {
            return num * (double)date;
        }
        public static double operator /(double num, Date date)
        {
            return num / (double)date;
        }
        public static double operator %(double num, Date date)
        {
            return num % (double)date;
        }

        public static Date operator +(Date date, bool num)
        {
            return (Date)((double)date + (num ? 1 : 0));
        }
        public static Date operator -(Date date, bool num)
        {
            return (Date)((double)date - (num ? 1 : 0));
        }
        public static Date operator *(Date date, bool num)
        {
            return (Date)((double)date * (num ? 1 : 0));
        }


        #endregion
    }
}
