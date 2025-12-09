using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions
{
    internal class Function_MAX : Function_N
    {
        public Function_MAX(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Evaluate(work).ToNumber("Function '{0}' parameter {1} is error!", "Max", index++); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter is error!", "Max"); }

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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Evaluate(work).ToNumber("Function '{0}' parameter {1} is error!", "Min", index++); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter is error!", "Min"); }
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Evaluate(work).ToNumber("Function '{0}' parameter {1} is error!", "Sum", index++); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter is error!", "Sum"); }
            return Operand.Create(list.Sum());
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Sum");
        }
    }

    internal class Function_SUMIF : Function_3
    {
        public Function_SUMIF(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsError) { return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsError) { return args2; }

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "SumIf", 1); }

            List<decimal> sumdbs;
            if (func3 != null) {
                var args3 = func3.Evaluate(work, tempParameter); if (args3.IsError) { return args3; }
                sumdbs = new List<decimal>();
                var o2 = FunctionUtil.F_base_GetList(args3, sumdbs);
                if (o2 == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "SumIf", 3); }
            } else {
                sumdbs = list;
            }

            decimal sum;
            if (args2.IsNumber) {
                sum = FunctionUtil.F_base_countif(list, args2.NumberValue) * args2.NumberValue;
            } else {
                if (decimal.TryParse(args2.TextValue.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    sum = FunctionUtil.F_base_sumif(list, d, sumdbs);
                } else {
                    var sunif = args2.TextValue.Trim();
                    var m2 = FunctionUtil.sumifMatch(sunif);
                    if (m2 != null) {
                        sum = FunctionUtil.F_base_sumif(list, m2.Item1, m2.Item2, sumdbs);
                    } else {
                        return Operand.Error("Function '{0}' parameter {1} is error!", "SumIf", 2);
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Evaluate(work).ToNumber("Function '{0}' parameter {1} is error!", "AveDev", index++); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter is error!", "AveDev"); }
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Evaluate(work).ToNumber("Function '{0}' parameter {1} is error!", "Average", index++); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter is error!", "Average"); }
            if (list.Count == 0) { return Operand.Zero; }
            return Operand.Create(list.Average());
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Average");
        }
    }

    internal class Function_AVERAGEIF : Function_3
    {
        public Function_AVERAGEIF(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsError) { return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsError) { return args2; }

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "AverageIf", 1); }

            List<decimal> sumdbs;
            if (func3 != null) {
                var args3 = func3.Evaluate(work, tempParameter); if (args3.IsError) { return args3; }
                sumdbs = new List<decimal>();
                var o2 = FunctionUtil.F_base_GetList(args3, sumdbs);
                if (o2 == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "AverageIf", 3); }
            } else {
                sumdbs = list;
            }

            decimal sum;
            int count;
            if (args2.IsNumber) {
                count = FunctionUtil.F_base_countif(list, args2.NumberValue);
                sum = count * args2.NumberValue;
            } else {
                if (decimal.TryParse(args2.TextValue.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
                    count = FunctionUtil.F_base_countif(list, d);
                    sum = FunctionUtil.F_base_sumif(list, d, sumdbs);
                } else {
                    var sunif = args2.TextValue.Trim();
                    var m2 = FunctionUtil.sumifMatch(sunif);
                    if (m2 != null) {
                        count = FunctionUtil.F_base_countif(list, m2.Item1, m2.Item2);
                        sum = FunctionUtil.F_base_sumif(list, m2.Item1, m2.Item2, sumdbs);
                    } else {
                        return Operand.Error("Function '{0}' parameter {1} is error!", "AverageIf", 2);
                    }
                }
            }
            if (count == 0) {
                return Operand.Error("Function '{0}' div 0 error!", "AverageIf");
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args = new List<Operand>();
            foreach (var item in funcs) { var aa = item.Evaluate(work, tempParameter); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter is error!", "Count"); }
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotArray) { args1 = args1.ToArray("Function '{0}' parameter {1} is error!", "CountIf", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsError) { return args2; }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "CountIf", 1); }
            int count;
            if (args2.IsNumber) {
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
                        return Operand.Error("Function '{0}' parameter {1} is error!", "CountIf", 2);
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Evaluate(work).ToNumber("Function '{0}' parameter {1} is error!", "Median", index++); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);

            if (o == false) { return Operand.Error("Function '{0}' parameter is error!", "Median"); }
            if (list.Count == 0) { return Operand.Error("Function '{0}' parameter is error!", "Median"); }

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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Evaluate(work).ToNumber("Function '{0}' parameter {1} is error!", "Mode", index++); if (aa.IsError) { return aa; } args.Add(aa); }

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter is error!", "Mode"); }

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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotArray) { args1 = args1.ToArray("Function '{0}' parameter {1} is error!", "Large", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Large", 2); if (args2.IsError) { return args2; } }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "Large", 1); }

            list = list.OrderByDescending(q => q).ToList();
            int k = args2.IntValue;
            if (k < 1 - work.ExcelIndex || k > list.Count - work.ExcelIndex) {
                return Operand.Error("Function '{0}' parameter {1} is error!", "Large", 2);
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotArray) { args1 = args1.ToArray("Function '{0}' parameter {1} is error!", "Small", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Small", 2); if (args2.IsError) { return args2; } }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "Small", 1); }
            list = list.OrderBy(q => q).ToList();
            int k = args2.IntValue;
            if (k < 1 - work.ExcelIndex || k > list.Count - work.ExcelIndex) {
                return Operand.Error("Function '{0}' parameter {1} is error!", "Small", 2);
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotArray) { args1 = args1.ToArray("Function '{0}' parameter {1} is error!", "Percentile", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Percentile", 2); if (args2.IsError) { return args2; } }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "Percentile", 1); }
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Evaluate(work).ToNumber("Function '{0}' parameter {1} is error!", "GeoMean", index++); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter is error!", "GeoMean"); }
            if (list.Count == 0) { return Operand.Error("Function '{0}' parameter is error!", "GeoMean"); }
            double product = 1.0;
            foreach (var num in list) {
                if (num <= 0) {
                    return Operand.Error("Function '{0}' parameter is error!", "GeoMean");
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Evaluate(work).ToNumber("Function '{0}' parameter {1} is error!", "HarMean", index++); if (aa.IsError) { return aa; } args.Add(aa); }
            if (args.Count == 1) return args[0];

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter is error!", "HarMean"); }

            decimal sum = 0;
            foreach (var db in list) {
                if (db == 0) {
                    return Operand.Error("Function '{0}' parameter is error!", "HarMean");
                }
                sum += 1 / db;
            }
            if (sum == 0) {
                return Operand.Error("Function '{0}' parameter is error!", "HarMean");
            }
            return Operand.Create(list.Count / sum);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "HarMean");
        }
    }

    internal class Function_PERCENTRANK : Function_3
    {
        public Function_PERCENTRANK(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotArray) { args1 = args1.ToArray("Function '{0}' parameter {1} is error!", "PercentRank", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "PercentRank", 2); if (args2.IsError) { return args2; } }

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter is error!", "PercentRank"); }

            var k = args2.NumberValue;
            var v = ExcelFunctions.PercentRank(list.Select(q => (double)q).ToArray(), (double)k);
            var d = 3;
            if (func3 != null) {
                var args3 = func2.Evaluate(work, tempParameter); if (args3.IsNotNumber) { args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "PercentRank", 3); if (args3.IsError) { return args3; } }
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Range", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Range", 2); if (args2.IsError) { return args2; } }
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Evaluate(work).ToNumber("Function '{0}' parameter {1} is error!", "Variance", index++); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter is error!", "Variance"); }
            if (list.Count == 0) { return Operand.Error("Function '{0}' parameter is error!", "Variance"); }
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Evaluate(work).ToNumber("Function '{0}' parameter {1} is error!", "Stdev", index++); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter is error!", "Stdev"); }
            if (list.Count == 0) { return Operand.Error("Function '{0}' parameter is error!", "Stdev"); }

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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Evaluate(work).ToNumber("Function '{0}' parameter {1} is error!", "STDEVP", index++); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter is error!", "StdevP"); }
            if (list.Count == 0) { return Operand.Error("Function '{0}' parameter is error!", "StdevP"); }
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Evaluate(work).ToNumber("Function '{0}' parameter {1} is error!", "DevSQ", index++); if (aa.IsError) { return aa; } args.Add(aa); }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter is error!", "DevSQ"); }
            if (list.Count == 0) { return Operand.Error("Function '{0}' parameter is error!", "DevSQ"); }
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Evaluate(work).ToNumber("Function '{0}' parameter {1} is error!", "Var", index++); if (aa.IsError) { return aa; } args.Add(aa); }
            if (args.Count == 1) {
                return Operand.Error("Function '{0}}' parameter only one error!", "Var");
            }

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter is error!", "Var"); }
            if (list.Count <= 1) { return Operand.Error("Function '{0}' parameter is error!", "Var"); }
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args = new List<Operand>(); int index = 1;
            foreach (var item in funcs) { var aa = item.Evaluate(work).ToNumber("Function '{0}' parameter {1} is error!", "VarP", index++); if (aa.IsError) { return aa; } args.Add(aa); }
            if (args.Count == 1) {
                return Operand.Error("Function '{0}}' parameter only one error!", "VarP");
            }
            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter is error!", "VarP"); }
            if (list.Count == 0) { return Operand.Error("Function '{0}' parameter is error!", "VarP"); }
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "NormDist", 1); if (args1.IsError) return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "NormDist", 2); if (args2.IsError) return args2; }
            var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotNumber) { args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "NormDist", 3); if (args3.IsError) return args3; }
            var args4 = func4.Evaluate(work, tempParameter); if (args4.IsNotBoolean) { args4 = args4.ToBoolean("Function '{0}' parameter {1} is error!", "NormDist", 4); if (args4.IsError) return args4; }

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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "NormInv", 1); if (args1.IsError) return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "NormInv", 2); if (args2.IsError) return args2; }
            var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotNumber) { args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "NormInv", 3); if (args3.IsError) return args3; }
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "NormSDist"); if (args1.IsError) return args1; }
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "NormSInv"); if (args1.IsError) return args1; }
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "BetaDist", 1); if (args1.IsError) return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "BetaDist", 2); if (args2.IsError) return args2; }
            var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotNumber) { args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "BetaDist", 3); if (args3.IsError) return args3; }
            var x = args1.NumberValue;
            var alpha = args2.NumberValue;
            var beta = args3.NumberValue;

            if (alpha < 0.0m || beta < 0.0m) {
                return Operand.Error("Function '{0}' parameter is error!", "BetaDist");
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "BetaInv", 1); if (args1.IsError) return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "BetaInv", 2); if (args2.IsError) return args2; }
            var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotNumber) { args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "BetaInv", 3); if (args3.IsError) return args3; }
            var p = args1.NumberValue;
            var alpha = args2.NumberValue;
            var beta = args3.NumberValue;
            if (alpha < 0.0m || beta < 0.0m || p < 0.0m || p > 1.0m) {
                return Operand.Error("Function '{0}' parameter is error!", "BetaInv");
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "BinomDist", 1); if (args1.IsError) return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "BinomDist", 2); if (args2.IsError) return args2; }
            var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotNumber) { args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "BinomDist", 3); if (args3.IsError) return args3; }
            var args4 = func4.Evaluate(work, tempParameter); if (args4.IsNotBoolean) { args4 = args4.ToBoolean("Function '{0}' parameter {1} is error!", "BinomDist", 4); if (args4.IsError) return args4; }

            if (!(args3.NumberValue >= 0.0m && args3.NumberValue <= 1.0m && args2.NumberValue >= 0)) {
                return Operand.Error("Function '{0}' parameter is error!", "BinomDist");
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "ExponDist", 1); if (args1.IsError) return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "ExponDist", 2); if (args2.IsError) return args2; }
            var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotBoolean) { args3 = args3.ToBoolean("Function '{0}' parameter {1} is error!", "ExponDist", 3); if (args3.IsError) return args3; }

            if (args1.NumberValue < 0.0m) {
                return Operand.Error("Function '{0}' parameter is error!", "ExponDist");
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "FDist", 1); if (args1.IsError) return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "FDist", 2); if (args2.IsError) return args2; }
            var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotNumber) { args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "FDist", 3); if (args3.IsError) return args3; }

            var x = args1.NumberValue;
            var degreesFreedom = args2.IntValue;
            var degreesFreedom2 = args3.IntValue;
            if (degreesFreedom <= 0.0m || degreesFreedom2 <= 0.0m) {
                return Operand.Error("Function '{0}' parameter is error!", "FDist");
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "FInv", 1); if (args1.IsError) return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "FInv", 2); if (args2.IsError) return args2; }
            var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotNumber) { args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "FInv", 3); if (args3.IsError) return args3; }
            var p = args1.NumberValue;
            var degreesFreedom = args2.IntValue;
            var degreesFreedom2 = args3.IntValue;
            if (degreesFreedom <= 0.0m || degreesFreedom2 <= 0.0m || p < 0.0m || p > 1.0m) {
                return Operand.Error("Function '{0}' parameter is error!", "FInv");
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "Fisher"); if (args1.IsError) { return args1; } }
            var x = args1.NumberValue;
            if (x >= 1 || x <= -1) {
                return Operand.Error("Function '{0}' parameter is error!", "Fisher");
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "FisherInv"); if (args1.IsError) { return args1; } }
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "GammaDist", 1); if (args1.IsError) return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "GammaDist", 2); if (args2.IsError) return args2; }
            var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotNumber) { args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "GammaDist", 3); if (args3.IsError) return args3; }
            var args4 = func4.Evaluate(work, tempParameter); if (args4.IsNotBoolean) { args4 = args4.ToBoolean("Function '{0}' parameter {1} is error!", "GammaDist", 4); if (args4.IsError) return args4; }
            var x = args1.NumberValue;
            var alpha = args2.NumberValue;
            var beta = args3.NumberValue;
            var cumulative = args4.BooleanValue;
            if (alpha < 0.0m || beta < 0.0m) {
                return Operand.Error("Function '{0}' parameter is error!", "GammaDist");
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "GammaInv", 1); if (args1.IsError) return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "GammaInv", 2); if (args2.IsError) return args2; }
            var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotNumber) { args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "GammaInv", 3); if (args3.IsError) return args3; }
            var probability = args1.NumberValue;
            var alpha = args2.NumberValue;
            var beta = args3.NumberValue;
            if (alpha < 0.0m || beta < 0.0m || probability < 0 || probability > 1.0m) {
                return Operand.Error("Function '{0}' parameter is error!", "GammaInv");
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter is error!", "GammaLn"); if (args1.IsError) { return args1; } }
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "HypgeomDist", 1); if (args1.IsError) return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "HypgeomDist", 2); if (args2.IsError) return args2; }
            var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotNumber) { args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "HypgeomDist", 3); if (args3.IsError) return args3; }
            var args4 = func4.Evaluate(work, tempParameter); if (args4.IsNotNumber) { args4 = args4.ToNumber("Function '{0}' parameter {1} is error!", "HypgeomDist", 4); if (args4.IsError) return args4; }
            int k = args1.IntValue;
            int draws = args2.IntValue;
            int success = args3.IntValue;
            int population = args4.IntValue;
            if (!(population >= 0 && success >= 0 && draws >= 0 && success <= population && draws <= population)) {
                return Operand.Error("Function '{0}' parameter is error!", "HypgeomDist");
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "LogInv", 1); if (args1.IsError) return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "LogInv", 2); if (args2.IsError) return args2; }
            var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotNumber) { args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "LogInv", 3); if (args3.IsError) return args3; }
            if (args3.NumberValue < 0.0m) {
                return Operand.Error("Function '{0}' parameter is error!", "LogInv");
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "LognormDist", 1); if (args1.IsError) return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "LognormDist", 2); if (args2.IsError) return args2; }
            var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotNumber) { args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "LognormDist", 3); if (args3.IsError) return args3; }
            if (args3.NumberValue < 0.0m) {
                return Operand.Error("Function '{0}' parameter is error!", "LognormDist");
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "NegbinomDist", 1); if (args1.IsError) return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "NegbinomDist", 2); if (args2.IsError) return args2; }
            var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotNumber) { args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "NegbinomDist", 3); if (args3.IsError) return args3; }
            int k = args1.IntValue;
            var r = args2.NumberValue;
            var p = args3.NumberValue;

            if (!(r >= 0.0m && p >= 0.0m && p <= 1.0m)) {
                return Operand.Error("Function '{0}' parameter is error!", "NegbinomDist");
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Poisson", 1); if (args1.IsError) return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Poisson", 2); if (args2.IsError) return args2; }
            var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotBoolean) { args3 = args3.ToBoolean("Function '{0}' parameter {1} is error!", "Poisson", 3); if (args3.IsError) return args3; }
            int k = args1.IntValue;
            var lambda = args2.NumberValue;
            bool state = args3.BooleanValue;
            if (!(lambda > 0.0m)) {
                return Operand.Error("Function '{0}' parameter is error!", "Poisson");
            }
            return Operand.Create(ExcelFunctions.Poisson(k, (double)lambda, state));
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "TDist", 1); if (args1.IsError) return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "TDist", 2); if (args2.IsError) return args2; }
            var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotNumber) { args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "TDist", 3); if (args3.IsError) return args3; }
            var x = args1.NumberValue;
            var degreesFreedom = args2.IntValue;
            var tails = args3.IntValue;
            if (degreesFreedom <= 0.0m || tails < 1 || tails > 2) {
                return Operand.Error("Function '{0}' parameter is error!", "TDist");
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "TInv", 1); if (args1.IsError) return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "TInv", 2); if (args2.IsError) return args2; }
            var p = args1.NumberValue;
            var degreesFreedom = args2.IntValue;
            if (degreesFreedom <= 0.0m || p < 0.0m || p > 1.0m) {
                return Operand.Error("Function '{0}' parameter is error!", "TInv");
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Weibull", 1); if (args1.IsError) return args1; }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Weibull", 2); if (args2.IsError) return args2; }
            var args3 = func3.Evaluate(work, tempParameter); if (args3.IsNotNumber) { args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "Weibull", 3); if (args3.IsError) return args3; }
            var args4 = func4.Evaluate(work, tempParameter); if (args4.IsNotBoolean) { args4 = args4.ToBoolean("Function '{0}' parameter {1} is error!", "Weibull", 4); if (args4.IsError) return args4; }
            var x = args1.NumberValue;
            var shape = args2.NumberValue;
            var scale = args3.NumberValue;
            var state = args4.BooleanValue;
            if (shape <= 0.0m || scale <= 0.0m) {
                return Operand.Error("Function '{0}' parameter is error!", "Weibull");
            }

            return Operand.Create(ExcelFunctions.Weibull((double)x, (double)shape, (double)scale, state));
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

        public override Operand Evaluate(AlgorithmEngine work, Func<string, Operand> tempParameter)
        {
            var args1 = func1.Evaluate(work, tempParameter); if (args1.IsNotArray) { args1 = args1.ToArray("Function '{0}' parameter {1} is error!", "Quartile", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Evaluate(work, tempParameter); if (args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Quartile", 2); if (args2.IsError) { return args2; } }

            List<decimal> list = new List<decimal>();
            var o = FunctionUtil.F_base_GetList(args1, list);
            if (o == false) { return Operand.Error("Function '{0}' parameter {1} is error!", "Quartile", 1); }

            var quant = args2.IntValue;
            if (quant < 0 || quant > 4) {
                return Operand.Error("Function '{0}' parameter {1} is error!", "Quartile", 2);
            }
            return Operand.Create(ExcelFunctions.Quartile(list.Select(q => (double)q).ToArray(), quant));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Quartile");
        }
    }

}
