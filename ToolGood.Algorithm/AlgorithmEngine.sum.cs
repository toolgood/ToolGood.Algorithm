using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm
{
    partial class AlgorithmEngine
    {
        private void AddSumFunction()
        {
            addFunc("array", array);
            addFunc("NormSDist", NormSDist);
            addFunc("AVEDEV", AVEDEV);//返回数据点与其平均值的绝对偏差的平均值
            addFunc("AVERAGE", AVERAGE);//返回参数的平均值
            addFunc("BETADIST", BETADIST);//返回 Beta 累积分布函数
            addFunc("BETAINV", BETAINV);//返回指定 Beta 分布的累积分布函数的反函数
            addFunc("BINOMDIST", BINOMDIST);//返回一元二项式分布概率
            //addFunc("CHIDIST", CHIDIST);//返回 chi 平方分布的单尾概率
            //addFunc("CHIINV", CHIINV);//返回 chi 平方分布的反单尾概率
            //addFunc("CHITEST", CHITEST);//返回独立性检验值
            addFunc("COUNT", COUNT);//计算参数列表中数字的个数
            addFunc("COUNTIF", COUNTIF);//计算参数列表中数字的个数



            //addFunc("CORREL", CORREL);//返回两个数据集之间的相关系数。
            //addFunc("COVAR", COVAR);//返回协方差，即成对偏移乘积的平均数。
            //addFunc("CRITBINOM", CRITBINOM);//返回使累积二项式分布小于等于临界值的最小值。
            addFunc("DEVSQ", DEVSQ);//返回偏差的平方和。
            addFunc("EXPONDIST", EXPONDIST);//返回指数分布。
            addFunc("FDIST", FDIST);//返回 F 概率分布。
            addFunc("FINV", FINV);//返回 F 概率分布的反函数。
            //addFunc("FISHER", FISHER);//返回 Fisher 变换值。
            //addFunc("FISHERINV", FISHERINV);//返回 Fisher 变换的反函数。
            //addFunc("FORECAST", FORECAST);//根据线性趋势返回值。
            //addFunc("FTEST", FTEST);//返回 F 检验的结果。
            //addFunc("FREQUENCY", FREQUENCY);//以垂直数组的形式返回频率分布。
            addFunc("GAMMADIST", GAMMADIST);//返回 γ 分布。
            addFunc("GAMMAINV", GAMMAINV);//返回 γ 累积分布函数的反函数。
            //addFunc("GAMMALN", GAMMALN);//返回 γ 函数的自然对数，Γ(x)。
            //addFunc("GEOMEAN", GEOMEAN);//返回正数数组或区域的几何平均值
            //addFunc("GROWTH", GROWTH);//根据指数趋势返回值
            //addFunc("HARMEAN", HARMEAN);//返回数据集合的调和平均值
            addFunc("HYPGEOMDIST", HYPGEOMDIST);//返回超几何分布
            //addFunc("INTERCEPT", INTERCEPT);//返回线性回归线截距
            //addFunc("KURT", KURT);//返回数据集的峰值
            //addFunc("LARGE", LARGE);//返回数据集中第k个最大值
            //addFunc("LINEST", LINEST);//返回线性趋势的参数

            addFunc("LOGINV", LOGINV);//返回反对数正态分布
            addFunc("LognormDist", LognormDist);//返回反对数正态分布
            //addFunc("LINEST", LINEST);//返回累积对数正态分布函数
            addFunc("MAX", MAX);//返回参数列表中的最大值
            addFunc("MEDIAN", MEDIAN);//返回给定数字的中值
            addFunc("MIN", MIN);//返回参数列表中的最小值
            addFunc("MODE", MODE);//返回数据集中出现最多的值间的概率
            //addFunc("PROB", PROB);//返回区域中的数值落在指定区间内的对应概率
            addFunc("NEGBINOMDIST", NEGBINOMDIST);//返回负二项式分布
            addFunc("NORMDIST", NORMDIST);//返回正态累积分布
            addFunc("NORMINV", NORMINV);//返回反正态累积分布
            addFunc("NORMSINV", NORMSINV);//返回反标准正态累积分布
            //addFunc("PEARSON", PEARSON);//返回 Pearson 乘积矩相关系数
            addFunc("PERCENTILE", PERCENTILE);//返回区域中的第 k 个百分位值
            addFunc("PERCENTRANK", PERCENTRANK);//返回数据集中值的百分比排位
            //addFunc("PERMUT", PERMUT);//返回从给定数目的对象集合中选取的若干对象的排列数
            addFunc("POISSON", POISSON);//返回 Poisson 分布
            //addFunc("PROB", PROB);//返回区域中的数值落在指定区间内的对应概率
            addFunc("QUARTILE", QUARTILE);//返回数据集的四分位数
            //addFunc("RANK", RANK);//返回某数在数字列表中的排位
            //addFunc("RSQ", RSQ);//返回 Pearson 乘积矩相关系数的平方
            //addFunc("SLOPE", SLOPE);//返回线性回归直线的斜率
            //addFunc("SMALL", SMALL);//返回数据集中的第k个最小值
            //addFunc("STANDARDIZE", STANDARDIZE);//返回正态化数值
            addFunc("STDEV", STDEV);//基于样本估算标准偏差
            addFunc("STDEVP", STDEVP);//计算基于整个样本总体的标准偏差
            addFunc("TDIST", TDIST);//返回学生的 t 分布
            addFunc("TINV", TINV);//返回学生的 t 分布的反分布
            //addFunc("TREND", TREND);//返回沿线性趋势的值
            //addFunc("TRIMMEAN", TRIMMEAN);//返回数据集的内部平均值
            //addFunc("TTEST", TTEST);//返回与学生的 t 检验相关的概率
            addFunc("VAR", VAR);//基于样本估算方差
            addFunc("VARP", VARP);//基于整个样本总体计算方差
            addFunc("WEIBULL", WEIBULL);//返回 Weibull 分布
            //addFunc("ZTEST", ZTEST);//返回 z 检验的单尾概率值
            addFunc("SUMIF", SUMIF);//返回 z 检验的单尾概率值
            addFunc("AVERAGEIF", AVERAGEIF);//返回参数的平均值

        }

        private Operand WEIBULL(List<Operand> arg)
        {
            if (arg.Count < 4) return throwError("WEIBULL中参数不足", new List<Operand>());

            return new Operand(OperandType.NUMBER, ExcelFunctions.WEIBULL(arg[0].NumberValue, arg[1].NumberValue, arg[2].NumberValue,arg[3].BooleanValue));
        }

        private Operand POISSON(List<Operand> arg)
        {
            if (arg.Count < 3) return throwError("POISSON中参数不足", new List<Operand>());

            return new Operand(OperandType.NUMBER, ExcelFunctions.POISSON(arg[0].IntValue, arg[1].NumberValue, arg[2].BooleanValue));

        }

        private Operand BINOMDIST(List<Operand> arg)
        {
            if (arg.Count < 4) return throwError("BINOMDIST中参数不足", new List<Operand>());

            return new Operand(OperandType.NUMBER, ExcelFunctions.BinomDist(arg[0].IntValue, arg[1].IntValue, arg[2].NumberValue,arg[3].BooleanValue));
        }

        private Operand LOGINV(List<Operand> arg)
        {
            if (arg.Count < 3) return throwError("LOGINV中参数不足", new List<Operand>());

            return new Operand(OperandType.NUMBER, ExcelFunctions.LogInv(arg[0].NumberValue, arg[1].NumberValue, arg[2].NumberValue));
        }

        private Operand LognormDist(List<Operand> arg)
        {
            if (arg.Count < 3) return throwError("LognormDist中参数不足", new List<Operand>());

            return new Operand(OperandType.NUMBER, ExcelFunctions.LognormDist(arg[0].NumberValue, arg[1].NumberValue, arg[2].NumberValue));

        }

        private Operand NEGBINOMDIST(List<Operand> arg)
        {
            if (arg.Count < 3) return throwError("NEGBINOMDIST中参数不足", new List<Operand>());

            return new Operand(OperandType.NUMBER, ExcelFunctions.NegbinomDist(arg[0].IntValue, arg[1].NumberValue, arg[2].NumberValue));

        }

        private Operand HYPGEOMDIST(List<Operand> arg)
        {
            if (arg.Count < 4) return throwError("HYPGEOMDIST中参数不足", new List<Operand>());

            return new Operand(OperandType.NUMBER, ExcelFunctions.HypgeomDist(arg[0].IntValue, arg[1].IntValue, arg[2].IntValue,arg[3].IntValue));
        }

        private Operand EXPONDIST(List<Operand> arg)
        {
            if (arg.Count < 3) return throwError("EXPONDIST中参数不足", new List<Operand>());

            return new Operand(OperandType.NUMBER, ExcelFunctions.ExponDist(arg[0].NumberValue,arg[1].NumberValue,arg[2].BooleanValue));

        }

        #region SUMIF COUNTIF 
        private Operand AVERAGEIF(List<Operand> arg)
        {
            throw new NotImplementedException();
        }


        private Operand SUMIF(List<Operand> arg)
        {
            throw new NotImplementedException();

            if (arg.Count < 2) return throwError("SUMIF中参数不足", new List<Operand>());
            var dbs = arg[0].GetNumberList();
            double sum = 0;
            if (arg[1].Type == OperandType.NUMBER) {
                sum = countif(dbs, arg[1].NumberValue) * arg[1].NumberValue;
            } else {
                double d;
                if (double.TryParse(arg[1].StringValue.Trim(), out d)) {
                    sum = countif(dbs, arg[1].NumberValue) * arg[1].NumberValue;
                } else {
                    //sum = countif(dbs, arg[1].StringValue.Trim());
                }
            }
            return new Operand(OperandType.NUMBER, sum);
        }
        private double sumif(List<double> dbs, string s)
        {
            Regex re = new Regex(@"(<|<=|>|>=|=|==|!=|<>) *([-+]?\d+(\.(\d+)?)?)");
            if (re.IsMatch(s) == false) {
                return 0;
            }
            var m = re.Match(s);
            var d = double.Parse(m.Groups[2].Value);
            var ss = m.Groups[1].Value;
            double sum = 0;

            foreach (var item in dbs) {
                if (compare(item, d, s)) {
                    sum += item;
                }
            }
            return sum;
        }


        private Operand COUNTIF(List<Operand> arg)
        {
            throw new NotImplementedException();


            if (arg.Count < 2) return throwError("COUNTIF中参数不足", new List<Operand>());
            var dbs = arg[0].GetNumberList();
            int count = 0;
            if (arg[1].Type == OperandType.NUMBER) {
                count = countif(dbs, arg[1].NumberValue);
            } else {
                double d;
                if (double.TryParse(arg[1].StringValue.Trim(), out d)) {
                    count = countif(dbs, arg[1].NumberValue);
                } else {
                    count = countif(dbs, arg[1].StringValue.Trim());
                }
            }
            return new Operand(OperandType.NUMBER, count);
        }

        private bool compare(double a, double b, string ss)
        {
            if (ss == "<") {
                return a < b;
            } else if (ss == "<=") {
                return a <= b;
            } else if (ss == ">") {
                return a > b;
            } else if (ss == ">=") {
                return a >= b;
            } else if (ss == "=" || ss == "==") {
                return a == b;
            }
            return a != b;
        }

        private int countif(List<double> dbs, string s)
        {
            Regex re = new Regex(@"(<|<=|>|>=|=|==|!=|<>) *([-+]?\d+(\.(\d+)?)?)");
            if (re.IsMatch(s) == false) {
                return 0;
            }
            var m = re.Match(s);
            var d = double.Parse(m.Groups[2].Value);
            var ss = m.Groups[1].Value;
            int count = 0;

            foreach (var item in dbs) {
                if (compare(item, d, s)) {
                    count++;
                }
            }
            return count;
        }


        private int countif(List<double> dbs, double d)
        {
            int count = 0;
            d = Math.Round(d, 12);
            foreach (var item in dbs) {
                if (Math.Round(item, 12) == d) {
                    count++;
                }
            }
            return count;
        }

        #endregion

        private Operand array(List<Operand> ops)
        {
            List<Operand> list = new List<Operand>();
            foreach (var op in ops) {
                addArrary(list, op);
            }
            return new Operand(OperandType.ARRARY, list);
        }

        private void addArrary(List<Operand> ops, Operand opd)
        {
            if (opd.Type == OperandType.ARRARY) {
                foreach (var item in opd.Value as List<Operand>) {
                    addArrary(ops, item);
                }
            } else {
                ops.Add(opd);
            }
        }


        private Operand NORMSINV(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("NORMSINV中参数不足", new List<Operand>());
            var k = arg[0].NumberValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.NormSInv(k));
        }

        private Operand NormSDist(List<Operand> arg)
        {
            //public static double NormSDist(double z)
            if (arg.Count < 1) return throwError("NormSDist中参数不足", new List<Operand>());
            var k = arg[0].NumberValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.NormSDist(k));
        }

        private Operand PERCENTRANK(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("PERCENTRANK中参数不足", new List<Operand>());
            var array = (arg[0].GetNumberList()).ToArray();
            var k = arg[1].NumberValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.PercentRank(array, k));
        }

        private Operand PERCENTILE(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("PERCENTILE中参数不足", new List<Operand>());
            var array = (arg[0].GetNumberList()).ToArray();
            var k = arg[1].NumberValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.Percentile(array, k));
        }

        private Operand QUARTILE(List<Operand> arg)
        {
            if (arg.Count < 3) return throwError("QUARTILE中参数不足", new List<Operand>());
            var array = (arg[0].GetNumberList()).ToArray();
            var quant = arg[1].IntValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.Quartile(array, quant));
        }

        private Operand GAMMAINV(List<Operand> arg)
        {
            if (arg.Count < 3) return throwError("GAMMAINV中参数不足", new List<Operand>());
            var probability = arg[0].NumberValue;
            var alpha = arg[1].NumberValue;
            var beta = arg[2].NumberValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.GammaInv(probability, alpha, beta));
        }

        private Operand GAMMADIST(List<Operand> arg)
        {
            if (arg.Count < 4) return throwError("GAMMADIST中参数不足", new List<Operand>());
            var x = arg[0].NumberValue;
            var alpha = arg[1].NumberValue;
            var beta = arg[2].NumberValue;
            var cumulative = arg[3].BooleanValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.GammaDist(x, alpha, beta, cumulative));
        }

        private Operand BETAINV(List<Operand> arg)
        {
            //public static double BetaInv(double probability, double alpha, double beta)
            if (arg.Count < 3) return throwError("BETAINV中参数不足", new List<Operand>());
            var probability = arg[0].NumberValue;
            var alpha = arg[1].NumberValue;
            var beta = arg[2].NumberValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.BetaInv(probability, alpha, beta));
        }

        private Operand BETADIST(List<Operand> arg)
        {
            //public static double BetaDist(double x, double alpha, double beta)
            if (arg.Count < 3) return throwError("BETADIST中参数不足", new List<Operand>());
            var x = arg[0].NumberValue;
            var alpha = arg[1].NumberValue;
            var beta = arg[2].NumberValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.BetaDist(x, alpha, beta));
        }

        private Operand FINV(List<Operand> arg)
        {
            if (arg.Count < 3) return throwError("FINV中参数不足", new List<Operand>());

            var probability = arg[0].NumberValue;
            var degreesFreedom = arg[1].IntValue;
            var degreesFreedom2 = arg[2].IntValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.FInv(probability, degreesFreedom, degreesFreedom2));
        }

        private Operand FDIST(List<Operand> arg)
        {
            if (arg.Count < 3) return throwError("FDIST中参数不足", new List<Operand>());
            var x = arg[0].NumberValue;
            var degreesFreedom = arg[1].IntValue;
            var degreesFreedom2 = arg[2].IntValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.FDist(x, degreesFreedom, degreesFreedom2));
        }

        private Operand TINV(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("TDIST中参数不足", new List<Operand>());
            var probability = arg[0].NumberValue;
            var degreesFreedom = arg[1].IntValue;

            return new Operand(OperandType.NUMBER, ExcelFunctions.TInv(probability, degreesFreedom));
        }

        private Operand TDIST(List<Operand> arg)
        {
            if (arg.Count < 3) return throwError("TDIST中参数不足", new List<Operand>());
            var x = arg[0].NumberValue;
            var degreesFreedom = arg[1].IntValue;
            var tails = arg[2].IntValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.TDist(x, degreesFreedom, tails));
        }

        private Operand NORMINV(List<Operand> arg)
        {
            if (arg.Count < 3) return throwError("NORMINV中参数不足", new List<Operand>());
            var num = arg[0].NumberValue;
            var avg = arg[1].NumberValue;
            var STDEV = arg[2].NumberValue;

            return new Operand(OperandType.NUMBER, ExcelFunctions.NormInv(avg, STDEV, num));
        }

        private Operand NORMDIST(List<Operand> arg)
        {
            if (arg.Count < 4) return throwError("NORMDIST中参数不足", new List<Operand>());
            var num = arg[0].NumberValue;
            var avg = arg[1].NumberValue;
            var STDEV = arg[2].NumberValue;
            var b = arg[3].BooleanValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.NormDist(num, avg, STDEV, b));


            //if (b) {
            //    var d = (num - avg) / STDEV;
            //    return new Operand(OperandType.NUMBER, MathEx.NormDist(d));
            //}
            //return new Operand(OperandType.NUMBER, MathEx.NormDistDensity(num, avg, STDEV));
        }

        private Operand VARP(List<Operand> arg)
        {
            List<double> list = GetList(arg);
            if (list.Count < 1) return throwError("VARP中参数不足", new List<Operand>());

            double sum = 0;
            double avg = list.Average();
            for (int i = 0; i < list.Count; i++) {
                sum += avg * avg - list[i] * list[i];
            }
            return new Operand(OperandType.NUMBER, sum / (list.Count * list.Count));
        }

        private Operand VAR(List<Operand> arg)
        {
            List<double> list = GetList(arg);
            if (list.Count < 1) return throwError("VAR中参数不足", new List<Operand>());
            double sum = 0;
            double avg = list.Average();
            for (int i = 0; i < list.Count; i++) {
                sum += avg * avg - list[i] * list[i];
            }
            return new Operand(OperandType.NUMBER, sum / ((list.Count - 1) * list.Count));
        }

        private Operand STDEVP(List<Operand> arg)
        {
            List<double> list = GetList(arg);
            if (list.Count < 1) return throwError("STDEVP中参数不足", new List<Operand>());
            double sum = 0;
            double avg = list.Average();

            for (int i = 0; i < list.Count; i++) {
                sum += (list[i] - avg) * (list[i] - avg);
            }
            return new Operand(OperandType.NUMBER, Math.Sqrt(sum / (list.Count)));
        }

        private Operand STDEV(List<Operand> arg)
        {
            List<double> list = GetList(arg);
            if (list.Count < 1) return throwError("STDEV中参数不足", new List<Operand>());
            double avg = list.Average();
            double sum = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += (list[i] - avg) * (list[i] - avg);
            }
            return new Operand(OperandType.NUMBER, Math.Sqrt(sum / (list.Count - 1)));
        }

        private Operand DEVSQ(List<Operand> arg)
        {
            List<double> list = GetList(arg);
            if (list.Count < 1) return throwError("DEVSQ中参数不足", new List<Operand>());
            double avg = list.Average();
            double sum = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += (list[i] - avg) * (list[i] - avg);
            }
            return new Operand(OperandType.NUMBER, sum);
        }

        private Operand AVEDEV(List<Operand> arg)
        {
            List<double> list = GetList(arg);
            if (list.Count < 1) return throwError("AVEDEV中参数不足", new List<Operand>());
            double avg = list.Average();
            double sum = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += Math.Abs(list[i] - avg);
            }
            return new Operand(OperandType.NUMBER, sum / list.Count);
        }

        private Operand MODE(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("MODE中参数不足", new List<Operand>());
            Dictionary<double, int> dict = new Dictionary<double, int>();
            foreach (var item in arg) {
                if (item.Type == OperandType.NUMBER) {
                    if (dict.ContainsKey(item.NumberValue)) {
                        dict[item.NumberValue] += 1;
                    } else {
                        dict[item.NumberValue] = 1;
                    }
                } else if (item.Type == OperandType.ARRARY) {
                    var list = item.GetNumberList();
                    if (list == null) continue;
                    foreach (var d in list) {
                        if (dict.ContainsKey(d)) {
                            dict[d] += 1;
                        } else {
                            dict[d] = 1;
                        }
                    }
                }
            }
            return new Operand(OperandType.NUMBER, dict.OrderByDescending(q => q.Value).Select(q => q.Key));
        }

        private Operand MIN(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("MIN中参数不足", new List<Operand>());
            var min = double.MaxValue;
            foreach (var item in arg) {
                if (item.Type == OperandType.NUMBER) {
                    if (item.NumberValue < min) {
                        min = item.NumberValue;
                    }
                } else if (item.Type == OperandType.ARRARY) {
                    var list = item.GetNumberList();
                    foreach (var d in list) {
                        if (d < min) {
                            min = d;
                        }
                    }
                }
            }
            return new Operand(OperandType.NUMBER, min);
        }

        private Operand MAX(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("MAX中参数不足", new List<Operand>());
            var max = double.MinValue;
            foreach (var item in arg) {
                if (item.Type == OperandType.NUMBER) {
                    if (item.NumberValue > max) {
                        max = item.NumberValue;
                    }
                } else if (item.Type == OperandType.ARRARY) {
                    var list = item.GetNumberList();
                    foreach (var d in list) {
                        if (d > max) {
                            max = d;
                        }
                    }
                }
            }
            return new Operand(OperandType.NUMBER, max);
        }

        private Operand MEDIAN(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("MEDIAN中参数不足", new List<Operand>());
            List<double> list = GetList(arg);
            list = list.OrderBy(q => q).ToList();
            return new Operand(OperandType.NUMBER, list[list.Count / 2]);
        }

        private Operand COUNT(List<Operand> arg)
        {
            List<double> list = GetList(arg);
            return new Operand(OperandType.NUMBER, list.Count);
        }

        private Operand AVERAGE(List<Operand> arg)
        {
            //if (arg.Count < 1) return throwError("AVERAGE中参数不足", new List<Operand>());
            List<double> list = GetList(arg);
            return new Operand(OperandType.NUMBER, list.Average());
        }

        private List<double> GetList(List<Operand> arg)
        {
            List<double> list = new List<double>();
            foreach (var item in arg) {
                if (item.Type == OperandType.NUMBER) {
                    list.Add(item.NumberValue);
                } else if (item.Type == OperandType.ARRARY) {
                    var ls = item.GetNumberList();
                    if (ls == null) continue;
                    foreach (var d in ls) {
                        list.Add(d);
                    }
                }
            }
            return list;
        }
    }
}
