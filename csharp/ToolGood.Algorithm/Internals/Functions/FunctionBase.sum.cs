using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions
{
    #region sum

    internal class Function_MAX : Function_N
    {
        public Function_MAX(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Calculate(work).ToNumber("Function MAX parameter {0} is error!", index++); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function MAX parameter error!"); }

            return Operand.Create(list.Max());
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Max");
        }
    }

    internal class Function_MIN : Function_N
    {
        public Function_MIN(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Calculate(work).ToNumber("Function MIN parameter {0} is error!", index++); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function MIN parameter error!"); }
            return Operand.Create(list.Min());
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Min");
        }
    }

    internal class Function_SUM : Function_N
    {
        public Function_SUM(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Calculate(work).ToNumber("Function SUM parameter {0} is error!", index++); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function SUM parameter error!"); }
            return Operand.Create(list.Sum());
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Sum");
        }
    }

    internal class Function_SUMIF : Function_N
    {
        public Function_SUMIF(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>();
            foreach (var item in funcs) { var aa = item.Calculate(work); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args[0], list);
            if (o == false) { return Operand.Error("Function SUMIF parameter 1 error!"); }

            List<decimal> sumdbs;
            if (args.Count == 3) {
                sumdbs = new List<decimal>();
                var o2 = FunctionUtil.F_base_GetList(args[2], sumdbs);
                if (o2 == false) { return Operand.Error("Function SUMIF parameter 3 error!"); }
            } else {
                sumdbs = list;
            }

            decimal sum;
            if (args[1].Type == OperandType.NUMBER) {
                sum = FunctionUtil.F_base_countif(list, args[1].NumberValue) * args[1].NumberValue;
            } else {
                if (decimal.TryParse(args[1].TextValue.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    sum = FunctionUtil.F_base_sumif(list, d, sumdbs);
                } else {
                    var sunif = args[1].TextValue.Trim();
                    var m2 = FunctionUtil.sumifMatch(sunif);
                    if (m2 != null) {
                        sum = FunctionUtil.F_base_sumif(list, m2.Item1, m2.Item2, sumdbs);
                    } else {
                        return Operand.Error("Function SUMIF parameter 2 error!");
                    }
                }
            }
            return Operand.Create(sum);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "SumIf");
        }
    }

    internal class Function_AVEDEV : Function_N
    {
        public Function_AVEDEV(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Calculate(work).ToNumber("Function AVEDEV parameter {0} is error!", index++); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function AVEDEV parameter error!"); }
            if (list.Count == 0) { return Operand.Zero; }
            var avg = list.Average();
            decimal sum = 0;
            foreach (var item in list) {
                sum += Math.Abs(item - avg);
            }
            return Operand.Create(sum / list.Count);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "AveDev");
        }
    }

    internal class Function_AVERAGE : Function_N
    {
        public Function_AVERAGE(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Calculate(work).ToNumber("Function AVERAGE parameter {0} is error!", index++); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function AVERAGE parameter error!"); }
            if (list.Count == 0) { return Operand.Zero; }
            return Operand.Create(list.Average());
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Average");
        }
    }

    internal class Function_AVERAGEIF : Function_N
    {
        public Function_AVERAGEIF(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(funcs.Length);
            for (int i = 0; i < funcs.Length; i++) { var aa = funcs[i].Calculate(work); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args[0], list);
            if (o == false) { return Operand.Error("Function AVERAGEIF parameter 1 error!"); }

            List<decimal> sumdbs;
            if (args.Count == 3) {
                sumdbs = new List<decimal>();
                var o2 = FunctionUtil.F_base_GetList(args[2], sumdbs);
                if (o2 == false) { return Operand.Error("Function AVERAGEIF parameter 3 error!"); }
            } else {
                sumdbs = list;
            }

            decimal sum;
            int count;
            if (args[1].Type == OperandType.NUMBER) {
                count = FunctionUtil.F_base_countif(list, args[1].NumberValue);
                sum = count * args[1].NumberValue;
            } else {
                if (decimal.TryParse(args[1].TextValue.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    count = FunctionUtil.F_base_countif(list, d);
                    sum = FunctionUtil.F_base_sumif(list, d, sumdbs);
                } else {
                    var sunif = args[1].TextValue.Trim();
                    var m2 = FunctionUtil.sumifMatch(sunif);
                    if (m2 != null) {
                        count = FunctionUtil.F_base_countif(list, m2.Item1, m2.Item2);
                        sum = FunctionUtil.F_base_sumif(list, m2.Item1, m2.Item2, sumdbs);
                    } else {
                        return Operand.Error("Function AVERAGEIF parameter 2 error!");
                    }
                }
            }
            if (count == 0) {
                return Operand.Error("Function AVERAGEIF divide by zero error!");
            }
            return Operand.Create(sum / count);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "AverageIf");
        }
    }

    internal class Function_COUNT : Function_N
    {
        public Function_COUNT(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>();
            foreach (var item in funcs) { var aa = item.Calculate(work); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function COUNT parameter error!"); }
            return Operand.Create(list.Count);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Count");
        }
    }

    internal class Function_COUNTIF : Function_2
    {
        public Function_COUNTIF(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.ARRARY) { args1 = args1.ToArray("Function COUNTIF parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.IsError) { return args2; }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function COUNTIF parameter 1 error!"); }
            int count;
            if (args2.Type == OperandType.NUMBER) {
                count = FunctionUtil.F_base_countif(list, args2.NumberValue);
            } else {
                if (decimal.TryParse(args2.TextValue.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    count = FunctionUtil.F_base_countif(list, d);
                } else {
                    var sunif = args2.TextValue.Trim();
                    var m2 = FunctionUtil.sumifMatch(sunif);
                    if (m2 != null) {
                        count = FunctionUtil.F_base_countif(list, m2.Item1, m2.Item2);
                    } else {
                        return Operand.Error("Function COUNTIF parameter 2 error!");
                    }
                }
            }
            return Operand.Create((decimal)count);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "CountIf");
        }
    }

    internal class Function_MEDIAN : Function_N
    {
        public Function_MEDIAN(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Calculate(work).ToNumber("Function MEDIAN parameter {0} is error!", index++); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);

            if (o == false) { return Operand.Error("Function MEDIAN parameter error!"); }
            if (list.Count == 0) { return Operand.Error("Function MEDIAN parameter error!"); }

            list = list.OrderBy(q => q).ToList();
            return Operand.Create(list[list.Count / 2]);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Median");
        }
    }

    internal class Function_MODE : Function_N
    {
        public Function_MODE(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Calculate(work).ToNumber("Function MODE parameter {0} is error!", index++); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function MODE parameter error!"); }

            Dictionary<decimal, int> dict = new Dictionary<decimal, int>();
            foreach (var item in list) {
                if (dict.ContainsKey(item)) {
                    dict[item] += 1;
                } else {
                    dict[item] = 1;
                }
            }
            return Operand.Create(dict.OrderByDescending(q => q.Value).First().Key);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Mode");
        }
    }

    internal class Function_LARGE : Function_2
    {
        public Function_LARGE(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.ARRARY) { args1 = args1.ToArray("Function LARGE parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function LARGE parameter 2 is error!"); if (args2.IsError) { return args2; } }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function LARGE parameter 1 error!"); }

            list = list.OrderByDescending(q => q).ToList();
            int k = args2.IntValue;
            if (k < 1 - work.ExcelIndex || k > list.Count - work.ExcelIndex) {
                return Operand.Error("Function LARGE parameter 2 is error!");
            }
            return Operand.Create(list[k - work.ExcelIndex]);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Large");
        }
    }

    internal class Function_SMALL : Function_2
    {
        public Function_SMALL(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.ARRARY) { args1 = args1.ToArray("Function SMALL parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function SMALL parameter 2 is error!"); if (args2.IsError) { return args2; } }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function SMALL parameter 1 error!"); }
            list = list.OrderBy(q => q).ToList();
            int k = args2.IntValue;
            if (k < 1 - work.ExcelIndex || k > list.Count - work.ExcelIndex) {
                return Operand.Error("Function SMALL parameter 2 is error!");
            }
            return Operand.Create(list[k - work.ExcelIndex]);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Small");
        }
    }

    internal class Function_PERCENTILE : Function_2
    {
        public Function_PERCENTILE(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.ARRARY) { args1 = args1.ToArray("Function PERCENTILE parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function PERCENTILE parameter 2 is error!"); if (args2.IsError) { return args2; } }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function PERCENTILE parameter 1 error!"); }
            var k = args2.NumberValue;
            return Operand.Create(ExcelFunctions.Percentile(list.Select(q => (double)q).ToArray(), (double)k));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Percentile");
        }
    }

    internal class Function_GEOMEAN : Function_N
    {
        public Function_GEOMEAN(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Calculate(work).ToNumber("Function GEOMEAN parameter {0} is error!", index++); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function GEOMEAN parameter error!"); }
            if (list.Count == 0) { return Operand.Error("Function GEOMEAN parameter error!"); }
            double product = 1.0;
            foreach (var num in list) {
                if (num <= 0) {
                    return Operand.Error("Function GEOMEAN parameter error!");
                }
                product *= (double)num;
            }
            double geoMean = Math.Pow(product, 1.0 / list.Count);
            return Operand.Create((decimal)geoMean);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "GeoMean");
        }
    }

    internal class Function_HARMEAN : Function_N
    {
        public Function_HARMEAN(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Calculate(work).ToNumber("Function HARMEAN parameter {0} is error!", index++); if (aa.IsError) { return aa; } args.Add(aa); }
            if (args.Count == 1) return args[0];

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function HARMEAN parameter error!"); }

            decimal sum = 0;
            foreach (var db in list) {
                if (db == 0) {
                    return Operand.Error("Function HARMEAN parameter error!");
                }
                sum += 1 / db;
            }
            if (sum == 0) {
                return Operand.Error("Function HARMEAN parameter error!");
            }
            return Operand.Create(list.Count / sum);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "HarMean");
        }
    }

    internal class Function_PERCENTRANK : Function_N
    {
        public Function_PERCENTRANK(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>();
            foreach (var item in funcs) { var aa = item.Calculate(work); if (aa.IsError) { return aa; } args.Add(aa); }

            var args1 = args[0].ToArray("Function PERCENTRANK parameter 1 is error!");
            if (args1.IsError) { return args1; }
            var args2 = args[1].ToNumber("Function PERCENTRANK parameter 2 is error!");
            if (args2.IsError) { return args2; }

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function PERCENTRANK parameter error!"); }

            var k = args2.NumberValue;
            var v = ExcelFunctions.PercentRank(list.Select(q => (double)q).ToArray(), (double)k);
            var d = 3;
            if (args.Count == 3) {
                var args3 = args[2].ToNumber("Function PERCENTRANK parameter 3 is error!");
                if (args3.IsError) { return args3; }
                d = args3.IntValue;
            }
            return Operand.Create(Math.Round(v, d, MidpointRounding.AwayFromZero));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "PercentRank");
        }
    }

    internal class Function_RANGE : Function_2
    {
        public Function_RANGE(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function RANGE parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function RANGE parameter 2 is error!"); if (args2.IsError) { return args2; } }
            return Operand.Create(args2.NumberValue - args1.NumberValue);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Range");
        }
    }

    internal class Function_VARIANCE : Function_N
    {
        public Function_VARIANCE(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Calculate(work).ToNumber("Function VARIANCE parameter {0} is error!", index++); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function VARIANCE parameter error!"); }
            if (list.Count == 0) { return Operand.Error("Function VARIANCE parameter error!"); }
            var avg = list.Average();
            var variance = list.Sum(d => (d - avg) * (d - avg)) / list.Count;
            return Operand.Create(variance);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Variance");
        }
    }

    internal class Function_STDEV : Function_N
    {
        public Function_STDEV(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Calculate(work).ToNumber("Function STDEV parameter {0} is error!", index++); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function STDEV parameter error!"); }
            if (list.Count == 0) { return Operand.Error("Function STDEV parameter error!"); }

            var avg = list.Average();
            decimal sum = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += (list[i] - avg) * (list[i] - avg);
            }
            return Operand.Create(Math.Sqrt((double)sum / (list.Count - 1)));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Stdev");
        }
    }

    internal class Function_STDEVP : Function_N
    {
        public Function_STDEVP(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Calculate(work).ToNumber("Function STDEVP parameter {0} is error!", index++); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function STDEVP parameter error!"); }
            if (list.Count == 0) { return Operand.Error("Function STDEVP parameter error!"); }
            var avg = list.Average();
            decimal sum = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += (list[i] - avg) * (list[i] - avg);
            }
            return Operand.Create(Math.Sqrt((double)sum / list.Count));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "StdevP");
        }
    }

    internal class Function_DEVSQ : Function_N
    {
        public Function_DEVSQ(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Calculate(work).ToNumber("Function DEVSQ parameter {0} is error!", index++); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function DEVSQ parameter error!"); }
            if (list.Count == 0) { return Operand.Error("Function DEVSQ parameter error!"); }
            var avg = list.Average();
            decimal sum = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += (list[i] - avg) * (list[i] - avg);
            }
            return Operand.Create(sum);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "DevSQ");
        }
    }

    internal class Function_VAR : Function_N
    {
        public Function_VAR(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Calculate(work).ToNumber("Function VAR parameter {0} is error!", index++); if (aa.IsError) { return aa; } args.Add(aa); }
            if (args.Count == 1) {
                return Operand.Error("Function VAR parameter only one error!");
            }

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function VAR parameter error!"); }
            if (list.Count <= 1) { return Operand.Error("Function VAR parameter error!"); }
            decimal sum = 0;
            decimal sum2 = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += list[i] * list[i];
                sum2 += list[i];
            }
            return Operand.Create((list.Count * sum - sum2 * sum2) / list.Count / (list.Count - 1));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Var");
        }
    }

    internal class Function_VARP : Function_N
    {
        public Function_VARP(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Calculate(work).ToNumber("Function VARP parameter {0} is error!", index++); if (aa.IsError) { return aa; } args.Add(aa); }
            if (args.Count == 1) {
                return Operand.Error("Function VARP parameter only one error!");
            }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function VARP parameter error!"); }
            if (list.Count == 0) { return Operand.Error("Function VARP parameter error!"); }
            if (list.Count == 1) { return Operand.Zero; }

            decimal sum = 0;
            decimal avg = list.Average();
            for (int i = 0; i < list.Count; i++) {
                sum += (avg - list[i]) * (avg - list[i]);
            }
            return Operand.Create(sum / list.Count);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "VarP");
        }
    }

    internal class Function_NORMDIST : Function_4
    {
        public Function_NORMDIST(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) : base(func1, func2, func3, func4)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function NORMDIST parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function NORMDIST parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = func3.Calculate(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function NORMDIST parameter 3 error!"); if (args3.IsError) return args3; }
            var args4 = func4.Calculate(work); if (args4.Type != OperandType.BOOLEAN) { args4 = args4.ToBoolean("Function NORMDIST parameter 4 error!"); if (args4.IsError) return args4; }

            var num = args1.NumberValue;
            var avg = args2.NumberValue;
            var STDEV = args3.NumberValue;
            var b = args4.BooleanValue;
            return Operand.Create(ExcelFunctions.NormDist((double)num, (double)avg, (double)STDEV, b));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "NormDist");
        }
    }

    internal class Function_NORMINV : Function_3
    {
        public Function_NORMINV(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function NORMINV parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function NORMINV parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = func3.Calculate(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function NORMINV parameter 3 error!"); if (args3.IsError) return args3; }
            var p = args1.NumberValue;
            var avg = args2.NumberValue;
            var STDEV = args3.NumberValue;
            return Operand.Create(ExcelFunctions.NormInv((double)p, (double)avg, (double)STDEV));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "NormInv");
        }
    }

    internal class Function_NORMSDIST : Function_1
    {
        public Function_NORMSDIST(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function NORMSDIST parameter error!"); if (args1.IsError) return args1; }
            var num = args1.NumberValue;
            return Operand.Create(ExcelFunctions.NormSDist((double)num));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "NormSDist");
        }
    }

    internal class Function_NORMSINV : Function_1
    {
        public Function_NORMSINV(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function NORMSINV parameter error!"); if (args1.IsError) return args1; }
            var p = args1.NumberValue;
            return Operand.Create(ExcelFunctions.NormSInv((double)p));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "NormSInv");
        }
    }

    internal class Function_BETADIST : Function_3
    {
        public Function_BETADIST(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function BETADIST parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function BETADIST parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = func3.Calculate(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function BETADIST parameter 3 error!"); if (args3.IsError) return args3; }
            var x = args1.NumberValue;
            var alpha = args2.NumberValue;
            var beta = args3.NumberValue;

            if (alpha < 0.0m || beta < 0.0m) {
                return Operand.Error("Function BETADIST parameter error!");
            }
            return Operand.Create(ExcelFunctions.BetaDist((double)x, (double)alpha, (double)beta));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "BetaDist");
        }

    }

    internal class Function_BETAINV : Function_3
    {
        public Function_BETAINV(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function BETAINV parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function BETAINV parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = func3.Calculate(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function BETAINV parameter 3 error!"); if (args3.IsError) return args3; }
            var p = args1.NumberValue;
            var alpha = args2.NumberValue;
            var beta = args3.NumberValue;
            if (alpha < 0.0m || beta < 0.0m || p < 0.0m || p > 1.0m) {
                return Operand.Error("Function BETAINV parameter error!");
            }
            return Operand.Create(ExcelFunctions.BetaInv((double)p, (double)alpha, (double)beta));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "BetaInv");
        }
    }

    internal class Function_BINOMDIST : Function_4
    {
        public Function_BINOMDIST(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) : base(func1, func2, func3, func4)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function BINOMDIST parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function BINOMDIST parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = func3.Calculate(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function BINOMDIST parameter 3 error!"); if (args3.IsError) return args3; }
            var args4 = func4.Calculate(work); if (args4.Type != OperandType.BOOLEAN) { args4 = args4.ToBoolean("Function BINOMDIST parameter 4 error!"); if (args4.IsError) return args4; }

            if (!(args3.NumberValue >= 0.0m && args3.NumberValue <= 1.0m && args2.NumberValue >= 0)) {
                return Operand.Error("Function BINOMDIST parameter error!");
            }
            return Operand.Create(ExcelFunctions.BinomDist(args1.IntValue, args2.IntValue, (double)args3.NumberValue, args4.BooleanValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "BinomDist");
        }
    }

    internal class Function_EXPONDIST : Function_3
    {
        public Function_EXPONDIST(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function EXPONDIST parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function EXPONDIST parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = func3.Calculate(work); if (args3.Type != OperandType.BOOLEAN) { args3 = args3.ToBoolean("Function EXPONDIST parameter 3 error!"); if (args3.IsError) return args3; }

            if (args1.NumberValue < 0.0m) {
                return Operand.Error("Function EXPONDIST parameter error!");
            }
            return Operand.Create(ExcelFunctions.ExponDist((double)args1.NumberValue, (double)args2.NumberValue, args3.BooleanValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "ExponDist");
        }
    }

    internal class Function_FDIST : Function_3
    {
        public Function_FDIST(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function FDIST parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function FDIST parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = func3.Calculate(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function FDIST parameter 3 error!"); if (args3.IsError) return args3; }

            var x = args1.NumberValue;
            var degreesFreedom = args2.IntValue;
            var degreesFreedom2 = args3.IntValue;
            if (degreesFreedom <= 0.0m || degreesFreedom2 <= 0.0m) {
                return Operand.Error("Function FDIST parameter error!");
            }
            return Operand.Create(ExcelFunctions.FDist((double)x, degreesFreedom, degreesFreedom2));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "FDist");
        }
    }

    internal class Function_FINV : Function_3
    {
        public Function_FINV(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function FINV parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function FINV parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = func3.Calculate(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function FINV parameter 3 error!"); if (args3.IsError) return args3; }
            var p = args1.NumberValue;
            var degreesFreedom = args2.IntValue;
            var degreesFreedom2 = args3.IntValue;
            if (degreesFreedom <= 0.0m || degreesFreedom2 <= 0.0m || p < 0.0m || p > 1.0m) {
                return Operand.Error("Function FINV parameter error!");
            }
            return Operand.Create(ExcelFunctions.FInv((double)p, degreesFreedom, degreesFreedom2));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "FInv");
        }
    }

    internal class Function_FISHER : Function_1
    {
        public Function_FISHER(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function FISHER parameter error!"); if (args1.IsError) { return args1; } }
            var x = args1.NumberValue;
            if (x >= 1 || x <= -1) {
                return Operand.Error("Function FISHER parameter error!");
            }
            var n = 0.5 * Math.Log((double)((1 + x) / (1 - x)));
            return Operand.Create(n);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Fisher");
        }
    }

    internal class Function_FISHERINV : Function_1
    {
        public Function_FISHERINV(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function FISHERINV parameter error!"); if (args1.IsError) { return args1; } }
            var x = (double)args1.NumberValue;
            var n = (Math.Exp((2 * x)) - 1) / (Math.Exp((2 * x)) + 1);
            return Operand.Create(n);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "FisherInv");
        }
    }

    internal class Function_GAMMADIST : Function_4
    {
        public Function_GAMMADIST(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) : base(func1, func2, func3, func4)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function GAMMADIST parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function GAMMADIST parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = func3.Calculate(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function GAMMADIST parameter 3 error!"); if (args3.IsError) return args3; }
            var args4 = func4.Calculate(work); if (args4.Type != OperandType.BOOLEAN) { args4 = args4.ToBoolean("Function GAMMADIST parameter 4 error!"); if (args4.IsError) return args4; }
            var x = args1.NumberValue;
            var alpha = args2.NumberValue;
            var beta = args3.NumberValue;
            var cumulative = args4.BooleanValue;
            if (alpha < 0.0m || beta < 0.0m) {
                return Operand.Error("Function GAMMADIST parameter error!");
            }
            return Operand.Create(ExcelFunctions.GammaDist((double)x, (double)alpha, (double)beta, cumulative));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "GammaDist");
        }
    }

    internal class Function_GAMMAINV : Function_3
    {
        public Function_GAMMAINV(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function GAMMAINV parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function GAMMAINV parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = func3.Calculate(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function GAMMAINV parameter 3 error!"); if (args3.IsError) return args3; }
            var probability = args1.NumberValue;
            var alpha = args2.NumberValue;
            var beta = args3.NumberValue;
            if (alpha < 0.0m || beta < 0.0m || probability < 0 || probability > 1.0m) {
                return Operand.Error("Function GAMMAINV parameter error!");
            }
            return Operand.Create(ExcelFunctions.GammaInv((double)probability, (double)alpha, (double)beta));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "GammaInv");
        }
    }

    internal class Function_GAMMALN : Function_1
    {
        public Function_GAMMALN(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function GAMMALN parameter error!"); if (args1.IsError) { return args1; } }
            return Operand.Create(ExcelFunctions.GAMMALN((double)args1.NumberValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "GammaLn");
        }
    }

    internal class Function_HYPGEOMDIST : Function_4
    {
        public Function_HYPGEOMDIST(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) : base(func1, func2, func3, func4)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function HYPGEOMDIST parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function HYPGEOMDIST parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = func3.Calculate(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function HYPGEOMDIST parameter 3 error!"); if (args3.IsError) return args3; }
            var args4 = func4.Calculate(work); if (args4.Type != OperandType.NUMBER) { args4 = args4.ToNumber("Function HYPGEOMDIST parameter 4 error!"); if (args4.IsError) return args4; }
            int k = args1.IntValue;
            int draws = args2.IntValue;
            int success = args3.IntValue;
            int population = args4.IntValue;
            if (!(population >= 0 && success >= 0 && draws >= 0 && success <= population && draws <= population)) {
                return Operand.Error("Function HYPGEOMDIST parameter error!");
            }
            return Operand.Create(ExcelFunctions.HypgeomDist(k, draws, success, population));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "HypgeomDist");
        }
    }

    internal class Function_LOGINV : Function_3
    {
        public Function_LOGINV(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function LOGINV parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function LOGINV parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = func3.Calculate(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function LOGINV parameter 3 error!"); if (args3.IsError) return args3; }
            if (args3.NumberValue < 0.0m) {
                return Operand.Error("Function LOGINV parameter error!");
            }
            return Operand.Create(ExcelFunctions.LogInv((double)args1.NumberValue, (double)args2.NumberValue, (double)args3.NumberValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "LogInv");
        }
    }

    internal class Function_LOGNORMDIST : Function_3
    {
        public Function_LOGNORMDIST(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function LOGNORMDIST parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function LOGNORMDIST parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = func3.Calculate(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function LOGNORMDIST parameter 3 error!"); if (args3.IsError) return args3; }
            if (args3.NumberValue < 0.0m) {
                return Operand.Error("Function LOGNORMDIST parameter error!");
            }
            return Operand.Create(ExcelFunctions.LognormDist((double)args1.NumberValue, (double)args2.NumberValue, (double)args3.NumberValue));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "LognormDist");
        }
    }

    internal class Function_NEGBINOMDIST : Function_3
    {
        public Function_NEGBINOMDIST(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function NEGBINOMDIST parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function NEGBINOMDIST parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = func3.Calculate(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function NEGBINOMDIST parameter 3 error!"); if (args3.IsError) return args3; }
            int k = args1.IntValue;
            var r = args2.NumberValue;
            var p = args3.NumberValue;

            if (!(r >= 0.0m && p >= 0.0m && p <= 1.0m)) {
                return Operand.Error("Function NEGBINOMDIST parameter error!");
            }
            return Operand.Create(ExcelFunctions.NegbinomDist(k, (double)r, (double)p));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "NegbinomDist");
        }
    }

    internal class Function_POISSON : Function_3
    {
        public Function_POISSON(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function POISSON parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function POISSON parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = func3.Calculate(work); if (args3.Type != OperandType.BOOLEAN) { args3 = args3.ToBoolean("Function POISSON parameter 3 error!"); if (args3.IsError) return args3; }
            int k = args1.IntValue;
            var lambda = args2.NumberValue;
            bool state = args3.BooleanValue;
            if (!(lambda > 0.0m)) {
                return Operand.Error("Function POISSON parameter error!");
            }
            return Operand.Create(ExcelFunctions.POISSON(k, (double)lambda, state));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Poisson");
        }
    }

    internal class Function_TDIST : Function_3
    {
        public Function_TDIST(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function TDIST parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function TDIST parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = func3.Calculate(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function TDIST parameter 3 error!"); if (args3.IsError) return args3; }
            var x = args1.NumberValue;
            var degreesFreedom = args2.IntValue;
            var tails = args3.IntValue;
            if (degreesFreedom <= 0.0m || tails < 1 || tails > 2) {
                return Operand.Error("Function TDIST parameter error!");
            }
            return Operand.Create(ExcelFunctions.TDist((double)x, degreesFreedom, tails));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "TDist");
        }
    }

    internal class Function_TINV : Function_2
    {
        public Function_TINV(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function TINV parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function TINV parameter 2 error!"); if (args2.IsError) return args2; }
            var p = args1.NumberValue;
            var degreesFreedom = args2.IntValue;
            if (degreesFreedom <= 0.0m || p < 0.0m || p > 1.0m) {
                return Operand.Error("Function TINV parameter error!");
            }
            return Operand.Create(ExcelFunctions.TInv((double)p, degreesFreedom));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "TInv");
        }
    }

    internal class Function_WEIBULL : Function_4
    {
        public Function_WEIBULL(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) : base(func1, func2, func3, func4)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function WEIBULL parameter 1 error!"); if (args1.IsError) return args1; }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function WEIBULL parameter 2 error!"); if (args2.IsError) return args2; }
            var args3 = func3.Calculate(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function WEIBULL parameter 3 error!"); if (args3.IsError) return args3; }
            var args4 = func4.Calculate(work); if (args4.Type != OperandType.BOOLEAN) { args4 = args4.ToBoolean("Function WEIBULL parameter 4 error!"); if (args4.IsError) return args4; }
            var x = args1.NumberValue;
            var shape = args2.NumberValue;
            var scale = args3.NumberValue;
            var state = args4.BooleanValue;
            if (shape <= 0.0m || scale <= 0.0m) {
                return Operand.Error("Function WEIBULL parameter error!");
            }

            return Operand.Create(ExcelFunctions.WEIBULL((double)x, (double)shape, (double)scale, state));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Weibull");
        }
    }

    internal class Function_QUARTILE : Function_2
    {
        public Function_QUARTILE(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.ARRARY) { args1 = args1.ToArray("Function QUARTILE parameter 1 is error!"); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function QUARTILE parameter 2 is error!"); if (args2.IsError) { return args2; } }

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function QUARTILE parameter 1 error!"); }

            var quant = args2.IntValue;
            if (quant < 0 || quant > 4) {
                return Operand.Error("Function QUARTILE parameter 2 is error!");
            }
            return Operand.Create(ExcelFunctions.Quartile(list.Select(q => (double)q).ToArray(), quant));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Quartile");
        }
    }

    #endregion sum
}
