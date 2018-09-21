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
            addFunc("array", F_Array);
            addFunc("NormSDist", F_NormSDist);
            addFunc("AVEDEV", F_Avedev);//返回数据点与其平均值的绝对偏差的平均值
            addFunc("AVERAGE", F_Average);//返回参数的平均值
            addFunc("BETADIST", F_BetaDist);//返回 Beta 累积分布函数
            addFunc("BETAINV", F_BetaInv);//返回指定 Beta 分布的累积分布函数的反函数
            addFunc("BINOMDIST", F_BinomDist);//返回一元二项式分布概率
            //addFunc("CHIDIST", CHIDIST);//返回 chi 平方分布的单尾概率
            //addFunc("CHIINV", CHIINV);//返回 chi 平方分布的反单尾概率
            //addFunc("CHITEST", CHITEST);//返回独立性检验值
            addFunc("COUNT", F_Count);//计算参数列表中数字的个数
            addFunc("COUNTIF", F_CountIf);//计算参数列表中数字的个数



            //addFunc("CORREL", CORREL);//返回两个数据集之间的相关系数。
            //addFunc("COVAR", COVAR);//返回协方差，即成对偏移乘积的平均数。
            //addFunc("CRITBINOM", CRITBINOM);//返回使累积二项式分布小于等于临界值的最小值。
            addFunc("DEVSQ", F_DevSq);//返回偏差的平方和。
            addFunc("EXPONDIST", F_ExponDist);//返回指数分布。
            addFunc("FDIST", F_FDist);//返回 F 概率分布。
            addFunc("FINV", F_FInv);//返回 F 概率分布的反函数。
            addFunc("FISHER", F_Fisher);//返回 Fisher 变换值。
            addFunc("FISHERINV", F_FisherInv);//返回 Fisher 变换的反函数。
            //addFunc("FORECAST", FORECAST);//根据线性趋势返回值。
            //addFunc("FTEST", FTEST);//返回 F 检验的结果。
            //addFunc("FREQUENCY", FREQUENCY);//以垂直数组的形式返回频率分布。
            addFunc("GAMMADIST", F_GammadDist);//返回 γ 分布。
            addFunc("GAMMAINV", F_GammaInv);//返回 γ 累积分布函数的反函数。
            addFunc("GAMMALN", F_Gammaln);//返回 γ 函数的自然对数，Γ(x)。
            addFunc("GEOMEAN", F_Geomean);//返回正数数组或区域的几何平均值
            //addFunc("GROWTH", GROWTH);//根据指数趋势返回值
            addFunc("HARMEAN", F_harMean);//返回数据集合的调和平均值
            addFunc("HYPGEOMDIST", F_hypgeomDist);//返回超几何分布
            //addFunc("INTERCEPT", INTERCEPT);//返回线性回归线截距
            //addFunc("KURT", KURT);//返回数据集的峰值
            addFunc("LARGE", F_Large);//返回数据集中第k个最大值
            //addFunc("LINEST", LINEST);//返回线性趋势的参数

            addFunc("LOGINV", F_LogInv);//返回反对数正态分布
            addFunc("LognormDist", F_LognormDist);//返回反对数正态分布
            //addFunc("LINEST", LINEST);//返回累积对数正态分布函数
            addFunc("MAX", F_Max);//返回参数列表中的最大值
            addFunc("MEDIAN", F_Median);//返回给定数字的中值
            addFunc("MIN", F_Min);//返回参数列表中的最小值
            addFunc("MODE", F_Mode);//返回数据集中出现最多的值间的概率
            //addFunc("PROB", PROB);//返回区域中的数值落在指定区间内的对应概率
            addFunc("NEGBINOMDIST", F_NegbinomDist);//返回负二项式分布
            addFunc("NORMDIST", F_NormDist);//返回正态累积分布
            addFunc("NORMINV", F_NormInv);//返回反正态累积分布
            addFunc("NORMSINV", F_NormsInv);//返回反标准正态累积分布
            //addFunc("PEARSON", PEARSON);//返回 Pearson 乘积矩相关系数
            addFunc("PERCENTILE", F_Percentile);//返回区域中的第 k 个百分位值
            addFunc("PERCENTRANK", F_PercentRank);//返回数据集中值的百分比排位
            addFunc("POISSON", F_Poisson);//返回 Poisson 分布
            //addFunc("PROB", PROB);//返回区域中的数值落在指定区间内的对应概率
            addFunc("QUARTILE", F_Quartile);//返回数据集的四分位数
            //addFunc("RANK", RANK);//返回某数在数字列表中的排位
            //addFunc("RSQ", RSQ);//返回 Pearson 乘积矩相关系数的平方
            //addFunc("SLOPE", SLOPE);//返回线性回归直线的斜率
            addFunc("SMALL", F_Small);//返回数据集中的第k个最小值
            //addFunc("STANDARDIZE", STANDARDIZE);//返回正态化数值
            addFunc("STDEV", F_Stdev);//基于样本估算标准偏差
            addFunc("STDEVP", F_Stdevp);//计算基于整个样本总体的标准偏差
            addFunc("TDIST", F_TDist);//返回学生的 t 分布
            addFunc("TINV", F_TInv);//返回学生的 t 分布的反分布
            //addFunc("TREND", TREND);//返回沿线性趋势的值
            //addFunc("TRIMMEAN", TRIMMEAN);//返回数据集的内部平均值
            //addFunc("TTEST", TTEST);//返回与学生的 t 检验相关的概率
            addFunc("VAR", F_Var);//基于样本估算方差
            addFunc("VARP", F_Varp);//基于整个样本总体计算方差
            addFunc("WEIBULL", F_weibull);//返回 Weibull 分布
            //addFunc("ZTEST", ZTEST);//返回 z 检验的单尾概率值
            addFunc("SUMIF", F_SumIf);//返回 z 检验的单尾概率值
            addFunc("AVERAGEIF", F_AverageIf);//返回参数的平均值

        }

        private Operand F_harMean(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("HARMEAN 中参数不足", new List<Operand>());
            if (arg.Count == 1) return arg[0];
            var dbs = F_base_GetList(arg);
            double sum = 0;
            foreach (var db in dbs) {
                sum += 1 / db;
            }
            return new Operand(OperandType.NUMBER, dbs.Count / sum);
        }

        private Operand F_Geomean(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("GAMMALN 中参数不足", new List<Operand>());
            if (arg.Count == 1) return arg[0];
            var dbs = F_base_GetList(arg);
            double sum = 1;
            foreach (var db in dbs) {
                sum *= db;
            }
            return new Operand(OperandType.NUMBER, Math.Pow(sum, 1.0 / dbs.Count));
        }

        private Operand F_Gammaln(List<Operand> arg)
        {
            CheckArgsCount("Char", arg, new OperandType[][] {
                new OperandType[] {  OperandType.NUMBER  },
                 });

            return new Operand(OperandType.NUMBER, ExcelFunctions.GAMMALN(arg[0].NumberValue));
        }

        private Operand F_Small(List<Operand> arg)
        {
            if (arg.Count < 2) return ThrowError("SMALL 中参数不足", new List<Operand>());
            var list = arg[0].GetNumberList().OrderBy(q => q).ToList();
            return new Operand(OperandType.NUMBER, list[arg[1].IntValue - excelIndex]);
        }

        private Operand F_Large(List<Operand> arg)
        {
            if (arg.Count < 2) return ThrowError("LARGE 中参数不足", new List<Operand>());
            var list = arg[0].GetNumberList().OrderByDescending(q => q).ToList();
            return new Operand(OperandType.NUMBER, list[arg[1].IntValue - excelIndex]);
        }

        private Operand F_FisherInv(List<Operand> arg)
        {
            CheckArgsCount("FisherInv", arg, new OperandType[][] {
                new OperandType[] {  OperandType.NUMBER  },
                 });

            var x = arg[0].NumberValue;
            var n = (Math.Exp(2 * x) - 1) / (Math.Exp(2 * x) + 1);
            return new Operand(OperandType.NUMBER, n);
        }

        private Operand F_Fisher(List<Operand> arg)
        {
            CheckArgsCount("FISHER", arg, new OperandType[][] {
                new OperandType[] {  OperandType.NUMBER  },
                 });

            var x = arg[0].NumberValue;
            var n = 0.5 * Math.Log((1 + x) / (1 - x));
            return new Operand(OperandType.NUMBER, n);
        }

        private Operand F_weibull(List<Operand> arg)
        {
            CheckArgsCount("WEIBULL", arg, new OperandType[][] {
                new OperandType[] {  OperandType.NUMBER,OperandType.NUMBER,OperandType.NUMBER,OperandType.NUMBER  },
                 });

            return new Operand(OperandType.NUMBER, ExcelFunctions.WEIBULL(arg[0].NumberValue, arg[1].NumberValue, arg[2].NumberValue, arg[3].BooleanValue));
        }

        private Operand F_Poisson(List<Operand> arg)
        {
            CheckArgsCount("Poisson", arg, new OperandType[][] {
                new OperandType[] {  OperandType.NUMBER,OperandType.NUMBER,OperandType.BOOLEAN   },
                 });

            return new Operand(OperandType.NUMBER, ExcelFunctions.POISSON(arg[0].IntValue, arg[1].NumberValue, arg[2].BooleanValue));

        }

        private Operand F_BinomDist(List<Operand> arg)
        {
            CheckArgsCount("BINOMDIST", arg, new OperandType[][] {
                new OperandType[] {  OperandType.NUMBER,OperandType.NUMBER, OperandType.NUMBER, OperandType.BOOLEAN   },
                 });

            return new Operand(OperandType.NUMBER, ExcelFunctions.BinomDist(arg[0].IntValue, arg[1].IntValue, arg[2].NumberValue, arg[3].BooleanValue));
        }

        private Operand F_LogInv(List<Operand> arg)
        {
            CheckArgsCount("LOGINV", arg, new OperandType[][] {
                new OperandType[] {  OperandType.NUMBER,OperandType.NUMBER, OperandType.NUMBER   },
                 });

            return new Operand(OperandType.NUMBER, ExcelFunctions.LogInv(arg[0].NumberValue, arg[1].NumberValue, arg[2].NumberValue));
        }

        private Operand F_LognormDist(List<Operand> arg)
        {
            CheckArgsCount("LognormDist", arg, new OperandType[][] {
                new OperandType[] {  OperandType.NUMBER,OperandType.NUMBER, OperandType.NUMBER   },
                 });

            return new Operand(OperandType.NUMBER, ExcelFunctions.LognormDist(arg[0].NumberValue, arg[1].NumberValue, arg[2].NumberValue));

        }

        private Operand F_NegbinomDist(List<Operand> arg)
        {
            CheckArgsCount("NegbinomDist", arg, new OperandType[][] {
                new OperandType[] {  OperandType.NUMBER,OperandType.NUMBER, OperandType.NUMBER   },
                 });

            return new Operand(OperandType.NUMBER, ExcelFunctions.NegbinomDist(arg[0].IntValue, arg[1].NumberValue, arg[2].NumberValue));

        }

        private Operand F_hypgeomDist(List<Operand> arg)
        {
            CheckArgsCount("NegbinomDist", arg, new OperandType[][] {
                new OperandType[] {  OperandType.NUMBER,OperandType.NUMBER, OperandType.NUMBER , OperandType.NUMBER  },
                 });
            if (arg.Count < 4) return ThrowError("HYPGEOMDIST 中参数不足", new List<Operand>());

            return new Operand(OperandType.NUMBER, ExcelFunctions.HypgeomDist(arg[0].IntValue, arg[1].IntValue, arg[2].IntValue, arg[3].IntValue));
        }

        private Operand F_ExponDist(List<Operand> arg)
        {
            CheckArgsCount("NegbinomDist", arg, new OperandType[][] {
                new OperandType[] {  OperandType.NUMBER,OperandType.NUMBER, OperandType.BOOLEAN   },
                 });

            return new Operand(OperandType.NUMBER, ExcelFunctions.ExponDist(arg[0].NumberValue, arg[1].NumberValue, arg[2].BooleanValue));

        }

        #region SUMIF COUNTIF 
        private Operand F_AverageIf(List<Operand> arg)
        {
            if (arg.Count < 2) return ThrowError("SUMIF 中参数不足", new List<Operand>());
            var dbs = arg[0].GetNumberList();
            List<double> sumdbs = dbs;
            if (arg.Count == 3) sumdbs = arg[2].GetNumberList();

            double sum = 0;
            int count = 0;
            if (arg[1].Type == OperandType.NUMBER) {
                count = F_base_countif(dbs, arg[1].NumberValue);
                sum = count * arg[1].NumberValue;
            } else {
                if (double.TryParse(arg[1].StringValue.Trim(), out double d)) {
                    count = F_base_countif(dbs, arg[1].NumberValue);
                    sum = F_base_sumif(dbs, "=" + arg[1].StringValue.Trim(), sumdbs);
                } else {
                    count = F_base_countif(dbs, arg[1].StringValue.Trim());
                    sum = F_base_sumif(dbs, arg[1].StringValue.Trim(), sumdbs);
                }
            }
            return new Operand(OperandType.NUMBER, sum / count);

        }


        private Operand F_SumIf(List<Operand> arg)
        {
            if (arg.Count < 2) return ThrowError("SUMIF 中参数不足", new List<Operand>());
            var dbs = arg[0].GetNumberList();
            var sumdbs = dbs;
            if (arg.Count == 3) sumdbs = arg[2].GetNumberList();
            double sum = 0;
            if (arg[1].Type == OperandType.NUMBER) {
                sum = F_base_countif(dbs, arg[1].NumberValue) * arg[1].NumberValue;
            } else {
                if (double.TryParse(arg[1].StringValue.Trim(), out double d)) {
                    sum = F_base_sumif(dbs, "=" + arg[1].StringValue.Trim(), sumdbs);
                } else {
                    sum = F_base_sumif(dbs, arg[1].StringValue.Trim(), sumdbs);
                }
            }
            return new Operand(OperandType.NUMBER, sum);
        }



        private Operand F_CountIf(List<Operand> arg)
        {
            if (arg.Count < 2) return ThrowError("COUNTIF 中参数不足", new List<Operand>());
            var dbs = arg[0].GetNumberList();
            int count = 0;
            if (arg[1].Type == OperandType.NUMBER) {
                count = F_base_countif(dbs, arg[1].NumberValue);
            } else {
                if (double.TryParse(arg[1].StringValue.Trim(), out double d)) {
                    count = F_base_countif(dbs, arg[1].NumberValue);
                } else {
                    count = F_base_countif(dbs, arg[1].StringValue.Trim());
                }
            }
            return new Operand(OperandType.NUMBER, count);
        }


        #endregion

        private Operand F_Array(List<Operand> ops)
        {
            List<Operand> list = new List<Operand>();
            foreach (var op in ops) {
                F_base_AddArrary(list, op);
            }
            return new Operand(OperandType.ARRARY, list);
        }



        private Operand F_NormsInv(List<Operand> arg)
        {
            CheckArgsCount("NORMSINV", arg, new OperandType[][] {
                new OperandType[] {  OperandType.NUMBER  },
                 });

            var k = arg[0].NumberValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.NormSInv(k));
        }

        private Operand F_NormSDist(List<Operand> arg)
        {
            CheckArgsCount("NormSDist", arg, new OperandType[][] {
                new OperandType[] {  OperandType.NUMBER  },
                 });
            var k = arg[0].NumberValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.NormSDist(k));
        }

        private Operand F_PercentRank(List<Operand> arg)
        {
            CheckArgsCount("PERCENTRANK", arg, new OperandType[][] {
                new OperandType[] {  OperandType.ANY, OperandType.NUMBER  },
                new OperandType[] {  OperandType.ANY, OperandType.NUMBER, OperandType.NUMBER   },
                 });

            var array = (arg[0].GetNumberList()).ToArray();
            var k = arg[1].NumberValue;
            var v = ExcelFunctions.PercentRank(array, k);
            var d = 3;
            if (arg.Count == 3) d = arg[3].IntValue;
            return new Operand(OperandType.NUMBER, Math.Round(v, d));
        }

        private Operand F_Percentile(List<Operand> arg)
        {
            CheckArgsCount("PERCENTILE", arg, new OperandType[][] {
                new OperandType[] {  OperandType.ARRARY, OperandType.NUMBER  },
                 });
            var array = (arg[0].GetNumberList()).ToArray();
            var k = arg[1].NumberValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.Percentile(array, k));
        }

        private Operand F_Quartile(List<Operand> arg)
        {
            CheckArgsCount("QUARTILE", arg, new OperandType[][] {
                new OperandType[] {  OperandType.ARRARY, OperandType.NUMBER  },
                 });
            var array = (arg[0].GetNumberList()).ToArray();
            var quant = arg[1].IntValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.Quartile(array, quant));
        }

        private Operand F_GammaInv(List<Operand> arg)
        {
            CheckArgsCount("GAMMAINV", arg, new OperandType[][] {
                new OperandType[] {  OperandType.NUMBER, OperandType.NUMBER, OperandType.NUMBER   },
                 });

            var probability = arg[0].NumberValue;
            var alpha = arg[1].NumberValue;
            var beta = arg[2].NumberValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.GammaInv(probability, alpha, beta));
        }

        private Operand F_GammadDist(List<Operand> arg)
        {
            CheckArgsCount("GAMMADIST", arg, new OperandType[][] {
                new OperandType[] {  OperandType.NUMBER, OperandType.NUMBER, OperandType.NUMBER, OperandType.BOOLEAN   },
                 });

            var x = arg[0].NumberValue;
            var alpha = arg[1].NumberValue;
            var beta = arg[2].NumberValue;
            var cumulative = arg[3].BooleanValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.GammaDist(x, alpha, beta, cumulative));
        }

        private Operand F_BetaInv(List<Operand> arg)
        {
            CheckArgsCount("BETAINV", arg, new OperandType[][] {
                new OperandType[] {  OperandType.NUMBER, OperandType.NUMBER, OperandType.NUMBER   },
                 });

            var probability = arg[0].NumberValue;
            var alpha = arg[1].NumberValue;
            var beta = arg[2].NumberValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.BetaInv(probability, alpha, beta));
        }

        private Operand F_BetaDist(List<Operand> arg)
        {
            CheckArgsCount("BetaDist", arg, new OperandType[][] {
                new OperandType[] {  OperandType.NUMBER, OperandType.NUMBER, OperandType.NUMBER   },
                 });

            var x = arg[0].NumberValue;
            var alpha = arg[1].NumberValue;
            var beta = arg[2].NumberValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.BetaDist(x, alpha, beta));
        }

        private Operand F_FInv(List<Operand> arg)
        {
            CheckArgsCount("FINV", arg, new OperandType[][] {
                new OperandType[] {  OperandType.NUMBER, OperandType.NUMBER, OperandType.NUMBER   },
                 });
            var probability = arg[0].NumberValue;
            var degreesFreedom = arg[1].IntValue;
            var degreesFreedom2 = arg[2].IntValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.FInv(probability, degreesFreedom, degreesFreedom2));
        }

        private Operand F_FDist(List<Operand> arg)
        {
            CheckArgsCount("FDIST", arg, new OperandType[][] {
                new OperandType[] {  OperandType.NUMBER, OperandType.NUMBER, OperandType.NUMBER   },
                 });

            var x = arg[0].NumberValue;
            var degreesFreedom = arg[1].IntValue;
            var degreesFreedom2 = arg[2].IntValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.FDist(x, degreesFreedom, degreesFreedom2));
        }

        private Operand F_TInv(List<Operand> arg)
        {
            CheckArgsCount("TDIST", arg, new OperandType[][] {
                new OperandType[] {  OperandType.NUMBER, OperandType.NUMBER  },
                 });
            var probability = arg[0].NumberValue;
            var degreesFreedom = arg[1].IntValue;

            return new Operand(OperandType.NUMBER, ExcelFunctions.TInv(probability, degreesFreedom));
        }

        private Operand F_TDist(List<Operand> arg)
        {
            CheckArgsCount("TDIST", arg, new OperandType[][] {
                new OperandType[] {  OperandType.NUMBER, OperandType.NUMBER, OperandType.NUMBER   },
                 });

            var x = arg[0].NumberValue;
            var degreesFreedom = arg[1].IntValue;
            var tails = arg[2].IntValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.TDist(x, degreesFreedom, tails));
        }

        private Operand F_NormInv(List<Operand> arg)
        {
            CheckArgsCount("NORMINV", arg, new OperandType[][] {
                new OperandType[] {  OperandType.NUMBER, OperandType.NUMBER, OperandType.NUMBER   },
                 });

            var num = arg[0].NumberValue;
            var avg = arg[1].NumberValue;
            var STDEV = arg[2].NumberValue;

            return new Operand(OperandType.NUMBER, ExcelFunctions.NormInv(num, avg, STDEV));
        }

        private Operand F_NormDist(List<Operand> arg)
        {
            CheckArgsCount("NORMDIST", arg, new OperandType[][] {
                new OperandType[] {  OperandType.NUMBER, OperandType.NUMBER, OperandType.NUMBER, OperandType.BOOLEAN   },
                 });

            var num = arg[0].NumberValue;
            var avg = arg[1].NumberValue;
            var STDEV = arg[2].NumberValue;
            var b = arg[3].BooleanValue;
            return new Operand(OperandType.NUMBER, ExcelFunctions.NormDist(num, avg, STDEV, b));
        }

        private Operand F_Varp(List<Operand> arg)
        {
            List<double> list = F_base_GetList(arg);
            if (list.Count < 1) return ThrowError("VARP 中参数不足", new List<Operand>());

            double sum = 0;
            double avg = list.Average();
            for (int i = 0; i < list.Count; i++) {
                sum += (avg - list[i]) * (avg - list[i]);
            }
            return new Operand(OperandType.NUMBER, sum / list.Count);
        }

        private Operand F_Var(List<Operand> arg)
        {
            List<double> list = F_base_GetList(arg);
            if (list.Count < 1) return ThrowError("VAR 中参数不足", new List<Operand>());
            double sum = 0;
            double sum2 = 0;
            double avg = list.Average();
            for (int i = 0; i < list.Count; i++) {
                sum += list[i] * list[i];
                sum2 += list[i];
            }
            return new Operand(OperandType.NUMBER, (list.Count * sum - sum2 * sum2) / list.Count / (list.Count - 1));
        }

        private Operand F_Stdevp(List<Operand> arg)
        {
            List<double> list = F_base_GetList(arg);
            if (list.Count < 1) return ThrowError("STDEVP 中参数不足", new List<Operand>());
            double sum = 0;
            double avg = list.Average();

            for (int i = 0; i < list.Count; i++) {
                sum += (list[i] - avg) * (list[i] - avg);
            }
            return new Operand(OperandType.NUMBER, Math.Sqrt(sum / (list.Count)));
        }

        private Operand F_Stdev(List<Operand> arg)
        {
            List<double> list = F_base_GetList(arg);
            if (list.Count < 1) return ThrowError("STDEV 中参数不足", new List<Operand>());
            double avg = list.Average();
            double sum = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += (list[i] - avg) * (list[i] - avg);
            }
            return new Operand(OperandType.NUMBER, Math.Sqrt(sum / (list.Count - 1)));
        }

        private Operand F_DevSq(List<Operand> arg)
        {
            List<double> list = F_base_GetList(arg);
            if (list.Count < 1) return ThrowError("DEVSQ 中参数不足", new List<Operand>());
            double avg = list.Average();
            double sum = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += (list[i] - avg) * (list[i] - avg);
            }
            return new Operand(OperandType.NUMBER, sum);
        }

        private Operand F_Avedev(List<Operand> arg)
        {
            List<double> list = F_base_GetList(arg);
            if (list.Count < 1) return ThrowError("AVEDEV 中参数不足", new List<Operand>());
            double avg = list.Average();
            double sum = 0;
            for (int i = 0; i < list.Count; i++) {
                sum += Math.Abs(list[i] - avg);
            }
            return new Operand(OperandType.NUMBER, sum / list.Count);
        }

        private Operand F_Mode(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("MODE 中参数不足", new List<Operand>());
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

        private Operand F_Min(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("MIN 中参数不足", new List<Operand>());
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

        private Operand F_Max(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("MAX 中参数不足", new List<Operand>());
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

        private Operand F_Median(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("MEDIAN 中参数不足", new List<Operand>());
            List<double> list = F_base_GetList(arg);
            list = list.OrderBy(q => q).ToList();
            return new Operand(OperandType.NUMBER, list[list.Count / 2]);
        }

        private Operand F_Count(List<Operand> arg)
        {
            List<double> list = F_base_GetList(arg);
            return new Operand(OperandType.NUMBER, list.Count);
        }

        private Operand F_Average(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("AVERAGE 中参数不足", new List<Operand>());
            List<double> list = F_base_GetList(arg);
            return new Operand(OperandType.NUMBER, list.Average());
        }


    }
}
