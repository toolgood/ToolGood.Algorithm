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
            addFunc("array", Func_Array);
            addFunc("NormSDist", Func_NormSDist);
            addFunc("AVEDEV", Func_Avedev);//返回数据点与其平均值的绝对偏差的平均值
            addFunc("AVERAGE", Func_Average);//返回参数的平均值
            addFunc("BETADIST", Func_BetaDist);//返回 Beta 累积分布函数
            addFunc("BETAINV", Func_BetaInv);//返回指定 Beta 分布的累积分布函数的反函数
            addFunc("BINOMDIST", Func_BinomDist);//返回一元二项式分布概率
            //addFunc("CHIDIST", CHIDIST);//返回 chi 平方分布的单尾概率
            //addFunc("CHIINV", CHIINV);//返回 chi 平方分布的反单尾概率
            //addFunc("CHITEST", CHITEST);//返回独立性检验值
            addFunc("COUNT", Func_Count);//计算参数列表中数字的个数
            addFunc("COUNTIF", Func_CountIf);//计算参数列表中数字的个数



            //addFunc("CORREL", CORREL);//返回两个数据集之间的相关系数。
            //addFunc("COVAR", COVAR);//返回协方差，即成对偏移乘积的平均数。
            //addFunc("CRITBINOM", CRITBINOM);//返回使累积二项式分布小于等于临界值的最小值。
            addFunc("DEVSQ", Func_DevSq);//返回偏差的平方和。
            addFunc("EXPONDIST", Func_ExponDist);//返回指数分布。
            addFunc("FDIST", Func_FDist);//返回 F 概率分布。
            addFunc("FINV", Func_FInv);//返回 F 概率分布的反函数。
            addFunc("FISHER", Func_Fisher);//返回 Fisher 变换值。
            addFunc("FISHERINV", Func_FisherInv);//返回 Fisher 变换的反函数。
            //addFunc("FORECAST", FORECAST);//根据线性趋势返回值。
            //addFunc("FTEST", FTEST);//返回 F 检验的结果。
            //addFunc("FREQUENCY", FREQUENCY);//以垂直数组的形式返回频率分布。
            addFunc("GAMMADIST", Func_GammadDist);//返回 γ 分布。
            addFunc("GAMMAINV", Func_GammaInv);//返回 γ 累积分布函数的反函数。
            addFunc("GAMMALN", Func_Gammaln);//返回 γ 函数的自然对数，Γ(x)。
            addFunc("GEOMEAN", Func_Geomean);//返回正数数组或区域的几何平均值
            //addFunc("GROWTH", GROWTH);//根据指数趋势返回值
            addFunc("HARMEAN", Func_harMean);//返回数据集合的调和平均值
            addFunc("HYPGEOMDIST", Func_hypgeomDist);//返回超几何分布
            //addFunc("INTERCEPT", INTERCEPT);//返回线性回归线截距
            //addFunc("KURT", KURT);//返回数据集的峰值
            addFunc("LARGE", Func_Large);//返回数据集中第k个最大值
            //addFunc("LINEST", LINEST);//返回线性趋势的参数

            addFunc("LOGINV", Func_LogInv);//返回反对数正态分布
            addFunc("LognormDist", Func_LognormDist);//返回反对数正态分布
            //addFunc("LINEST", LINEST);//返回累积对数正态分布函数
            addFunc("MAX", Func_Max);//返回参数列表中的最大值
            addFunc("MEDIAN", Func_Median);//返回给定数字的中值
            addFunc("MIN", Func_Min);//返回参数列表中的最小值
            addFunc("MODE", Func_Mode);//返回数据集中出现最多的值间的概率
            //addFunc("PROB", PROB);//返回区域中的数值落在指定区间内的对应概率
            addFunc("NEGBINOMDIST", Func_negbinomDist);//返回负二项式分布
            addFunc("NORMDIST", Func_normDist);//返回正态累积分布
            addFunc("NORMINV", Func_normInv);//返回反正态累积分布
            addFunc("NORMSINV", Func_normsInv);//返回反标准正态累积分布
            //addFunc("PEARSON", PEARSON);//返回 Pearson 乘积矩相关系数
            addFunc("PERCENTILE", Func_Percentile);//返回区域中的第 k 个百分位值
            addFunc("PERCENTRANK", Func_PercentRank);//返回数据集中值的百分比排位
            addFunc("POISSON", Func_Poisson);//返回 Poisson 分布
            //addFunc("PROB", PROB);//返回区域中的数值落在指定区间内的对应概率
            addFunc("QUARTILE", Func_Quartile);//返回数据集的四分位数
            //addFunc("RANK", RANK);//返回某数在数字列表中的排位
            //addFunc("RSQ", RSQ);//返回 Pearson 乘积矩相关系数的平方
            //addFunc("SLOPE", SLOPE);//返回线性回归直线的斜率
            addFunc("SMALL", Func_Small);//返回数据集中的第k个最小值
            //addFunc("STANDARDIZE", STANDARDIZE);//返回正态化数值
            addFunc("STDEV", Func_Stdev);//基于样本估算标准偏差
            addFunc("STDEVP", Func_Stdevp);//计算基于整个样本总体的标准偏差
            addFunc("TDIST", Func_TDist);//返回学生的 t 分布
            addFunc("TINV", Func_TInv);//返回学生的 t 分布的反分布
            //addFunc("TREND", TREND);//返回沿线性趋势的值
            //addFunc("TRIMMEAN", TRIMMEAN);//返回数据集的内部平均值
            //addFunc("TTEST", TTEST);//返回与学生的 t 检验相关的概率
            addFunc("VAR", Func_var);//基于样本估算方差
            addFunc("VARP", Func_Varp);//基于整个样本总体计算方差
            addFunc("WEIBULL", Func_weibull);//返回 Weibull 分布
            //addFunc("ZTEST", ZTEST);//返回 z 检验的单尾概率值
            addFunc("SUMIF", Func_SumIf);//返回 z 检验的单尾概率值
            addFunc("AVERAGEIF", Func_AverageIf);//返回参数的平均值

        }

        private Operand Func_harMean(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("HARMEAN中参数不足", new List<Operand>());
            if (arg.Count == 1) return arg[0];
            var dbs = GetList(arg);
            double sum = 0;
            foreach (var db in dbs) {
                sum += 1 / db;
            }
            return new Operand(OperandType.NUMBER, dbs.Count / sum);
        }

        private Operand Func_Geomean(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("GAMMALN中参数不足", new List<Operand>());
            if (arg.Count == 1) return arg[0];
            var dbs = GetList(arg);
            double sum = 1;
            foreach (var db in dbs) {
                sum *= db;
            }
            return new Operand(OperandType.NUMBER, Math.Pow(sum, 1.0 / dbs.Count));
        }

        private Operand Func_Gammaln(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("GAMMALN中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, ExcelFunctions.GAMMALN(arg[0].NumberValue));
        }

        private Operand Func_Small(List<Operand> arg)
        {
            if (arg.Count < 2) return ThrowError("SMALL中参数不足", new List<Operand>());
            var list = arg[0].GetNumberList().OrderBy(q => q).ToList();
            return new Operand(OperandType.NUMBER, list[arg[1].IntValue - 1]);
        }

        private Operand Func_Large(List<Operand> arg)
        {
            if (arg.Count < 2) return ThrowError("LARGE中参数不足", new List<Operand>());
            var list = arg[0].GetNumberList().OrderByDescending(q => q).ToList();
            return new Operand(OperandType.NUMBER, list[arg[1].IntValue - 1]);
        }

        private Operand Func_FisherInv(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("FISHER中参数不足", new List<Operand>());
            var x = arg[0].NumberValue;
            var n = (Math.Exp(2 * x) - 1) / (Math.Exp(2 * x) + 1);
            return new Operand(OperandType.NUMBER, n);
        }

        private Operand Func_Fisher(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("FISHER中参数不足", new List<Operand>());
            var x = arg[0].NumberValue;
            var n = 0.5 * Math.Log((1 + x) / (1 - x));
            return new Operand(OperandType.NUMBER, n);
        }

        private Operand Func_weibull(List<Operand> arg)
        {
            if (arg.Count < 4) return ThrowError("WEIBULL中参数不足", new List<Operand>());

            return new Operand(OperandType.NUMBER, ExcelFunctions.WEIBULL(arg[0].NumberValue, arg[1].NumberValue, arg[2].NumberValue, arg[3].BooleanValue));
        }

        private Operand Func_Poisson(List<Operand> arg)
        {
            if (arg.Count < 3) return ThrowError("POISSON中参数不足", new List<Operand>());

            return new Operand(OperandType.NUMBER, ExcelFunctions.POISSON(arg[0].IntValue, arg[1].NumberValue, arg[2].BooleanValue));

        }

        private Operand Func_BinomDist(List<Operand> arg)
        {
            if (arg.Count < 4) return ThrowError("BINOMDIST中参数不足", new List<Operand>());

            return new Operand(OperandType.NUMBER, ExcelFunctions.BinomDist(arg[0].IntValue, arg[1].IntValue, arg[2].NumberValue, arg[3].BooleanValue));
        }

        private Operand Func_LogInv(List<Operand> arg)
        {
            if (arg.Count < 3) return ThrowError("LOGINV中参数不足", new List<Operand>());

            return new Operand(OperandType.NUMBER, ExcelFunctions.LogInv(arg[0].NumberValue, arg[1].NumberValue, arg[2].NumberValue));
        }

        private Operand Func_LognormDist(List<Operand> arg)
        {
            if (arg.Count < 3) return ThrowError("LognormDist中参数不足", new List<Operand>());

            return new Operand(OperandType.NUMBER, ExcelFunctions.LognormDist(arg[0].NumberValue, arg[1].NumberValue, arg[2].NumberValue));

        }

        private Operand Func_negbinomDist(List<Operand> arg)
        {
            if (arg.Count < 3) return ThrowError("NEGBINOMDIST中参数不足", new List<Operand>());

            return new Operand(OperandType.NUMBER, ExcelFunctions.NegbinomDist(arg[0].IntValue, arg[1].NumberValue, arg[2].NumberValue));

        }

        private Operand Func_hypgeomDist(List<Operand> arg)
        {
            if (arg.Count < 4) return ThrowError("HYPGEOMDIST中参数不足", new List<Operand>());

            return new Operand(OperandType.NUMBER, ExcelFunctions.HypgeomDist(arg[0].IntValue, arg[1].IntValue, arg[2].IntValue, arg[3].IntValue));
        }

        private Operand Func_ExponDist(List<Operand> arg)
        {
            if (arg.Count < 3) return ThrowError("EXPONDIST中参数不足", new List<Operand>());

            return new Operand(OperandType.NUMBER, ExcelFunctions.ExponDist(arg[0].NumberValue, arg[1].NumberValue, arg[2].BooleanValue));

        }

        #region SUMIF COUNTIF 
        private Operand Func_AverageIf(List<Operand> arg)
        {
            if (arg.Count < 2) return ThrowError("SUMIF中参数不足", new List<Operand>());
            var dbs = arg[0].GetNumberList();
            List<double> sumdbs = dbs;
            if (arg.Count == 3) sumdbs = arg[2].GetNumberList();

            double sum = 0;
            int count = 0;
            if (arg[1].Type == OperandType.NUMBER) {
                count = countif(dbs, arg[1].NumberValue);
                sum = count * arg[1].NumberValue;
            } else {
                if (double.TryParse(arg[1].StringValue.Trim(), out double d)) {
                    count = countif(dbs, arg[1].NumberValue);
                    sum = sumif(dbs, "=" + arg[1].StringValue.Trim(), sumdbs);
                } else {
                    count = countif(dbs, arg[1].StringValue.Trim());
                    sum = sumif(dbs, arg[1].StringValue.Trim(), sumdbs);
                }
            }
            return new Operand(OperandType.NUMBER, sum / count);

        }


        private Operand Func_SumIf(List<Operand> arg)
        {
            if (arg.Count < 2) return ThrowError("SUMIF中参数不足", new List<Operand>());
            var dbs = arg[0].GetNumberList();
            var sumdbs = dbs;
            if (arg.Count == 3) sumdbs = arg[2].GetNumberList();
            double sum = 0;
            if (arg[1].Type == OperandType.NUMBER) {
                sum = countif(dbs, arg[1].NumberValue) * arg[1].NumberValue;
            } else {
                if (double.TryParse(arg[1].StringValue.Trim(), out double d)) {
                    sum = sumif(dbs, "=" + arg[1].StringValue.Trim(), sumdbs);
                } else {
                    sum = sumif(dbs, arg[1].StringValue.Trim(), sumdbs);
                }
            }
            return new Operand(OperandType.NUMBER, sum);
        }



        private Operand Func_CountIf(List<Operand> arg)
        {
            if (arg.Count < 2) return ThrowError("COUNTIF中参数不足", new List<Operand>());
            var dbs = arg[0].GetNumberList();
            int count = 0;
            if (arg[1].Type == OperandType.NUMBER) {
                count = countif(dbs, arg[1].NumberValue);
            } else {
                if (double.TryParse(arg[1].StringValue.Trim(), out double d)) {
                    count = countif(dbs, arg[1].NumberValue);
                } else {
                    count = countif(dbs, arg[1].StringValue.Trim());
                }
            }
            return new Operand(OperandType.NUMBER, count);
        }


        #endregion

        private Operand Func_Array(List<Operand> ops)
        {
            List<Operand> list = new List<Operand>();
            foreach (var op in ops) {
                AddArrary(list, op);
            }
            return new Operand(OperandType.ARRARY, list);
        }

        private void AddArrary(List<Operand> ops, Operand opd)
        {
            if (opd.Type == OperandType.ARRARY) {
                foreach (var item in opd.Value as List<Operand>) {
                    AddArrary(ops, item);
                }
            } else {
                ops.Add(opd);
            }
        }


        private Operand Func_normsInv(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("NORMSINV中参数不足", new List<Operand>());
            var k = arg[0].NumberValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.NormSInv(k));
        }

        private Operand Func_NormSDist(List<Operand> arg)
        {
            //public static double NormSDist(double z)
            if (arg.Count < 1) return ThrowError("NormSDist中参数不足", new List<Operand>());
            var k = arg[0].NumberValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.NormSDist(k));
        }

        private Operand Func_PercentRank(List<Operand> arg)
        {
            if (arg.Count < 2) return ThrowError("PERCENTRANK中参数不足", new List<Operand>());
            var array = (arg[0].GetNumberList()).ToArray();
            var k = arg[1].NumberValue;
            var v = ExcelFunctions.PercentRank(array, k);
            var d = 3;
            if (arg.Count == 3) d = arg[3].IntValue;
            return new Operand(OperandType.NUMBER, Math.Round(v, d));
        }

        private Operand Func_Percentile(List<Operand> arg)
        {
            if (arg.Count < 2) return ThrowError("PERCENTILE中参数不足", new List<Operand>());
            var array = (arg[0].GetNumberList()).ToArray();
            var k = arg[1].NumberValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.Percentile(array, k));
        }

        private Operand Func_Quartile(List<Operand> arg)
        {
            if (arg.Count < 2) return ThrowError("QUARTILE中参数不足", new List<Operand>());
            var array = (arg[0].GetNumberList()).ToArray();
            var quant = arg[1].IntValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.Quartile(array, quant));
        }

        private Operand Func_GammaInv(List<Operand> arg)
        {
            if (arg.Count < 3) return ThrowError("GAMMAINV中参数不足", new List<Operand>());
            var probability = arg[0].NumberValue;
            var alpha = arg[1].NumberValue;
            var beta = arg[2].NumberValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.GammaInv(probability, alpha, beta));
        }

        private Operand Func_GammadDist(List<Operand> arg)
        {
            if (arg.Count < 4) return ThrowError("GAMMADIST中参数不足", new List<Operand>());
            var x = arg[0].NumberValue;
            var alpha = arg[1].NumberValue;
            var beta = arg[2].NumberValue;
            var cumulative = arg[3].BooleanValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.GammaDist(x, alpha, beta, cumulative));
        }

        private Operand Func_BetaInv(List<Operand> arg)
        {
            //public static double BetaInv(double probability, double alpha, double beta)
            if (arg.Count < 3) return ThrowError("BETAINV中参数不足", new List<Operand>());
            var probability = arg[0].NumberValue;
            var alpha = arg[1].NumberValue;
            var beta = arg[2].NumberValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.BetaInv(probability, alpha, beta));
        }

        private Operand Func_BetaDist(List<Operand> arg)
        {
            //public static double BetaDist(double x, double alpha, double beta)
            if (arg.Count < 3) return ThrowError("BETADIST中参数不足", new List<Operand>());
            var x = arg[0].NumberValue;
            var alpha = arg[1].NumberValue;
            var beta = arg[2].NumberValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.BetaDist(x, alpha, beta));
        }

        private Operand Func_FInv(List<Operand> arg)
        {
            if (arg.Count < 3) return ThrowError("FINV中参数不足", new List<Operand>());

            var probability = arg[0].NumberValue;
            var degreesFreedom = arg[1].IntValue;
            var degreesFreedom2 = arg[2].IntValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.FInv(probability, degreesFreedom, degreesFreedom2));
        }

        private Operand Func_FDist(List<Operand> arg)
        {
            if (arg.Count < 3) return ThrowError("FDIST中参数不足", new List<Operand>());
            var x = arg[0].NumberValue;
            var degreesFreedom = arg[1].IntValue;
            var degreesFreedom2 = arg[2].IntValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.FDist(x, degreesFreedom, degreesFreedom2));
        }

        private Operand Func_TInv(List<Operand> arg)
        {
            if (arg.Count < 2) return ThrowError("TDIST中参数不足", new List<Operand>());
            var probability = arg[0].NumberValue;
            var degreesFreedom = arg[1].IntValue;

            return new Operand(OperandType.NUMBER, ExcelFunctions.TInv(probability, degreesFreedom));
        }

        private Operand Func_TDist(List<Operand> arg)
        {
            if (arg.Count < 3) return ThrowError("TDIST中参数不足", new List<Operand>());
            var x = arg[0].NumberValue;
            var degreesFreedom = arg[1].IntValue;
            var tails = arg[2].IntValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.TDist(x, degreesFreedom, tails));
        }

        private Operand Func_normInv(List<Operand> arg)
        {
            if (arg.Count < 3) return ThrowError("NORMINV中参数不足", new List<Operand>());
            var num = arg[0].NumberValue;
            var avg = arg[1].NumberValue;
            var STDEV = arg[2].NumberValue;

            return new Operand(OperandType.NUMBER, ExcelFunctions.NormInv(num, avg, STDEV));
        }

        private Operand Func_normDist(List<Operand> arg)
        {
            if (arg.Count < 4) return ThrowError("NORMDIST中参数不足", new List<Operand>());
            var num = arg[0].NumberValue;
            var avg = arg[1].NumberValue;
            var STDEV = arg[2].NumberValue;
            var b = arg[3].BooleanValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.NormDist(num, avg, STDEV, b));
        }

        private Operand Func_Varp(List<Operand> arg)
        {
            List<double> list = GetList(arg);
            if (list.Count < 1) return ThrowError("VARP中参数不足", new List<Operand>());

            double sum = 0;
            double avg = list.Average();
            for (int i = 0; i < list.Count; i++) {
                sum += (avg - list[i]) * (avg - list[i]);
            }
            return new Operand(OperandType.NUMBER, sum / list.Count);
        }

        private Operand Func_var(List<Operand> arg)
        {
            List<double> list = GetList(arg);
            if (list.Count < 1) return ThrowError("VAR中参数不足", new List<Operand>());
            double sum = 0;
            double sum2 = 0;
            double avg = list.Average();
            for (int i = 0; i < list.Count; i++) {
                sum += list[i] * list[i];
                sum2 += list[i];
            }
            return new Operand(OperandType.NUMBER, (list.Count * sum - sum2 * sum2) / list.Count / (list.Count - 1));
        }

        private Operand Func_Stdevp(List<Operand> arg)
        {
            List<double> list = GetList(arg);
            if (list.Count < 1) return ThrowError("STDEVP中参数不足", new List<Operand>());
            double sum = 0;
            double avg = list.Average();

            for (int i = 0; i < list.Count; i++) {
                sum += (list[i] - avg) * (list[i] - avg);
            }
            return new Operand(OperandType.NUMBER, Math.Sqrt(sum / (list.Count)));
        }

        private Operand Func_Stdev(List<Operand> arg)
        {
            List<double> list = GetList(arg);
            if (list.Count < 1) return ThrowError("STDEV中参数不足", new List<Operand>());
            double avg = list.Average();
            double sum = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += (list[i] - avg) * (list[i] - avg);
            }
            return new Operand(OperandType.NUMBER, Math.Sqrt(sum / (list.Count - 1)));
        }

        private Operand Func_DevSq(List<Operand> arg)
        {
            List<double> list = GetList(arg);
            if (list.Count < 1) return ThrowError("DEVSQ中参数不足", new List<Operand>());
            double avg = list.Average();
            double sum = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += (list[i] - avg) * (list[i] - avg);
            }
            return new Operand(OperandType.NUMBER, sum);
        }

        private Operand Func_Avedev(List<Operand> arg)
        {
            List<double> list = GetList(arg);
            if (list.Count < 1) return ThrowError("AVEDEV中参数不足", new List<Operand>());
            double avg = list.Average();
            double sum = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += Math.Abs(list[i] - avg);
            }
            return new Operand(OperandType.NUMBER, sum / list.Count);
        }

        private Operand Func_Mode(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("MODE中参数不足", new List<Operand>());
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
            return new Operand(OperandType.NUMBER, dict.OrderByDescending(q => q.Value).First().Key);
        }

        private Operand Func_Min(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("MIN中参数不足", new List<Operand>());
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

        private Operand Func_Max(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("MAX中参数不足", new List<Operand>());
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

        private Operand Func_Median(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("MEDIAN中参数不足", new List<Operand>());
            List<double> list = GetList(arg);
            list = list.OrderBy(q => q).ToList();
            return new Operand(OperandType.NUMBER, list[list.Count / 2]);
        }

        private Operand Func_Count(List<Operand> arg)
        {
            List<double> list = GetList(arg);
            return new Operand(OperandType.NUMBER, list.Count);
        }

        private Operand Func_Average(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("AVERAGE中参数不足", new List<Operand>());
            List<double> list = GetList(arg);
            return new Operand(OperandType.NUMBER, list.Average());
        }


    }
}
