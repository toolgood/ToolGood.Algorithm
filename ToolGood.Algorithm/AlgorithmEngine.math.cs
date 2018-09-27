using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm
{
    partial class AlgorithmEngine
    {

        private void AddMathFunction()
        {
            addFunc("abs", F_Abs);//返回数字的绝对值 
            addFunc("acos", F_Acos);//返回数字的反余弦值 
            addFunc("acosh", F_Acosh);//返回数字的反双曲余弦值 
            addFunc("asin", F_Asin);//返回数字的反正弦值 
            addFunc("asinh", F_Asinh);//返回数字的反正弦值 
            addFunc("atan", F_Atan);//返回数字的反正切值
            addFunc("atanh", F_Atanh);//返回数字的反正切值


            addFunc("atan2", F_Atan2);//从 X 和 Y 坐标返回反正切
            addFunc("ceiling", F_Ceiling);//将数字舍入为最接近的整数，或最接近的有效数字的倍数
            addFunc("combin", F_Combin);//计算从给定数目的对象集合中提取若干对象的组合数
            addFunc("cos", F_Cos);//返回数字的余弦值
            addFunc("cosh", F_Cosh);//返回数字的双曲余弦值
            addFunc("degrees", F_Degrees);//将弧度转换为度
            addFunc("even", F_Even);//将数字向上舍入为最接近的偶型整数
            addFunc("exp", F_Exp);//返回 e 的指定数乘幂
            addFunc("fact", F_Fact);//返回数字的阶乘
            addFunc("factdouble", F_Factdouble);//返回数字的双倍阶乘
            addFunc("floor", F_Floor);//返回数字的双倍阶乘
            addFunc("GCD", F_Gcd);//返回最大公约数
            addFunc("INT", F_Int);//返回最大公约数
            addFunc("LCM", F_Lcm);//返回整数参数的最小公倍数。 
            addFunc("ln", F_Ln);//返回数字的自然对数
            addFunc("log", F_Log);//返回数字的自然对数
            addFunc("LOG10", F_Log10);//返回数字的常用对数
            addFunc("MULTINOMIAL", F_Multinomial);//返回参数和的阶乘与各参数阶乘乘积的比值
            //addFunc("MINVERSE", minverse);//返回数组矩阵的逆距阵
            //addFunc("MDETERM", mdeterm);//返回一个数组的矩阵行列式的值
            //addFunc("MMULT", MMULT);//返回两个数组的矩阵乘积
            addFunc("MOD", F_Mod);//返回两数相除的余数
            addFunc("MROUND", F_Mround);//返回一个舍入到所需倍数的数字
            addFunc("ODD", F_Odd);//将数字向上舍入为最接近的奇型整数
            addFunc("PI", F_Pi);//返回 PI 值
            addFunc("POWER", F_Power);//返回数的乘幂结果
            addFunc("PRODUCT", F_Product);//将所有以参数形式给出的数字相乘
            addFunc("QUOTIENT", F_Quotient);//返回商的整数部分，该函数可用于舍掉商的小数部分。 
            addFunc("RADIANS", F_Radians);//将度转换为弧度 
            addFunc("RAND", F_Rand);//返回 0 到 1 之间的随机数 
            addFunc("RANDBETWEEN", F_Randbetween);//返回指定数字之间的随机数
            //addFunc("ROMAN", ROMAN);//将阿拉伯数字转换为文本形式的罗马数字
            addFunc("ROUND", F_Round);//将数字舍入到指定位数
            addFunc("ROUNDDOWN", F_RoundDown);//将数字朝零的方向舍入
            addFunc("ROUNDUP", F_RoundUp);//将数朝远离零的方向舍入
            //addFunc("SERIESSUM", SERIESSUM);//返回基于公式的幂级数的和
            addFunc("SIGN", F_Sign);//返回数字的符号
            addFunc("SIN", F_Sin);//返回给定角度的正弦值
            addFunc("SINH", F_Sinh);//返回数字的双曲正弦值
            addFunc("SQRTPI", F_SqrtPi);//返回某数与 PI 的乘积的平方根
            addFunc("SQRT", F_Sqrt);//返回正平方根
            //addFunc("SUBTOTAL", SUBTOTAL);//返回数据库清单或数据库中的分类汇总
            addFunc("SUM", F_Sum);//将参数求和
            //addFunc("SUMIF", SUMIF);//按给定条件将指定单元格求和
            //addFunc("SUMPRODUCT", SUMPRODUCT);//返回相对应的数组部分的乘积和
            addFunc("SUMSQ", F_SumSq);//返回参数的平方和
            //addFunc("SUMX2MY2", SUMX2MY2);//返回两数组中对应值平方差之和
            //addFunc("SUMX2PY2", SUMX2PY2);//返回两数组中对应值的平方和之和
            //addFunc("SUMXMY2", SUMXMY2);//返回两数组中对应值的平方和之和
            addFunc("TAN", F_Tan);//返回数字的正切值
            addFunc("TANH", F_Tanh);//返回数字的双曲正切值
            addFunc("TRUNC", F_Trunc);//将数字截尾取整
            addFunc("PERMUT", F_Permut);//返回从给定数目的对象集合中选取的若干对象的排列数


        }

        private Operand F_Permut(List<Operand> arg)
        {
            CheckArgsCount("Permut", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER, OperandType.NUMBER },
                 });

            var total = arg[0].IntValue;
            var count = arg[1].IntValue;
            double sum = 1;
            for (int i = 0; i < count; i++) {
                sum *= (total - i);
            }
            return new Operand(OperandType.NUMBER, sum);
        }

        private Operand F_Combin(List<Operand> arg)
        {
            CheckArgsCount("Combin", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER, OperandType.NUMBER },
                 });

            var total = arg[0].IntValue;
            var count = arg[1].IntValue;
            double sum = 1;
            double sum2 = 1;
            for (int i = 0; i < count; i++) {
                sum *= (total - i);
                sum2 *= (i + 1);
            }
            return new Operand(OperandType.NUMBER, sum / sum2);
        }

        private Operand F_Trunc(List<Operand> arg)
        {
            CheckArgsCount("TRUNC", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER },
                 });

            return new Operand(OperandType.NUMBER, (double)(int)(arg[0].NumberValue));
        }


        private Operand F_Tanh(List<Operand> arg)
        {
            CheckArgsCount("TANH", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER },
                 });

            return new Operand(OperandType.NUMBER, Math.Tanh(arg[0].NumberValue));
        }

        private Operand F_Tan(List<Operand> arg)
        {
            CheckArgsCount("TAN", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER },
                 });

            return new Operand(OperandType.NUMBER, Math.Tan(arg[0].NumberValue));
        }

        private Operand F_SumSq(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("SUMSQ 中参数不足", new List<Operand>());
            double d = 0;
            for (int i = 0; i < arg.Count; i++) {
                if (arg[i].Type == OperandType.NUMBER) {
                    var c = arg[i].NumberValue;
                    d += c * c;
                } else if (arg[i].Type == OperandType.ARRARY) {
                    var ls = arg[i].GetNumberList();
                    foreach (var item in ls) {
                        d += item * item;
                    }
                }
            }
            return new Operand(OperandType.NUMBER, d);
        }

        private Operand F_Sum(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("SUM 中参数不足", new List<Operand>());
            List<double> list = new List<double>();
            for (int i = 0; i < arg.Count; i++) {
                if (arg[i].Type == OperandType.NUMBER) {
                    list.Add(arg[i].NumberValue);
                } else if (arg[i].Type == OperandType.ARRARY) {
                    var ls = arg[i].GetNumberList();
                    foreach (var item in ls) {
                        list.Add(item);
                    }
                }
            }
            return new Operand(OperandType.NUMBER, list.Sum());
        }

        private Operand F_Sqrt(List<Operand> arg)
        {
            CheckArgsCount("SQRT", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER },
                 });

            return new Operand(OperandType.NUMBER, Math.Sqrt(arg[0].NumberValue));
        }

        private Operand F_SqrtPi(List<Operand> arg)
        {
            CheckArgsCount("SQRTPI", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER },
                 });

            return new Operand(OperandType.NUMBER, Math.Sqrt(arg[0].NumberValue * Math.PI));
        }

        private Operand F_Sinh(List<Operand> arg)
        {
            CheckArgsCount("SINH", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER },
                 });

            return new Operand(OperandType.NUMBER, Math.Sinh(arg[0].NumberValue));
        }

        private Operand F_Sin(List<Operand> arg)
        {
            CheckArgsCount("SIN", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER },
                 });

            return new Operand(OperandType.NUMBER, Math.Sin(arg[0].NumberValue));
        }

        private Operand F_Sign(List<Operand> arg)
        {
            CheckArgsCount("SIGN", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER },
                 });

            return new Operand(OperandType.NUMBER, (double)Math.Sign(arg[0].NumberValue));
        }

        private Operand F_RoundUp(List<Operand> arg)
        {
            CheckArgsCount("ROUNDUP", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER, OperandType.NUMBER },
                 });

            var a = Math.Pow(10, (int)arg[1].NumberValue);
            var b = arg[0].NumberValue;

            var t = (Math.Ceiling(Math.Abs(b) * a)) / a;
            if (b > 0) return new Operand(OperandType.NUMBER, t);
            return new Operand(OperandType.NUMBER, -t);
        }

        private Operand F_RoundDown(List<Operand> arg)
        {
            CheckArgsCount("ROUNDDOW", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER, OperandType.NUMBER },
                 });

            var a = Math.Pow(10, (int)arg[1].NumberValue);
            var b = arg[0].NumberValue;

            b = ((double)(int)(b * a)) / a;
            return new Operand(OperandType.NUMBER, b);
        }

        private Operand F_Round(List<Operand> arg)
        {
            CheckArgsCount("ROUND", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER },
                new OperandType[] { OperandType.NUMBER, OperandType.NUMBER },
                 });

            if (arg.Count == 1) {
                return new Operand(OperandType.NUMBER, Math.Round(arg[0].NumberValue, MidpointRounding.AwayFromZero));
            }
            return new Operand(OperandType.NUMBER, (double)Math.Round((decimal)arg[0].NumberValue, (int)arg[1].NumberValue, MidpointRounding.AwayFromZero));
        }

        private Operand F_Randbetween(List<Operand> arg)
        {
            CheckArgsCount("Randbetween", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER, OperandType.NUMBER },
                 });

            return new Operand(OperandType.NUMBER, rand.NextDouble() * (arg[1].NumberValue - arg[0].NumberValue) + arg[0].NumberValue);

        }

        private Operand F_Rand(List<Operand> arg)
        {
            return new Operand(OperandType.NUMBER, rand.NextDouble());
        }

        private Operand F_Radians(List<Operand> arg)
        {
            CheckArgsCount("Radians", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER },
                 });

            var r = arg[0].NumberValue / 180 * Math.PI;
            return new Operand(OperandType.NUMBER, r);
        }

        private Operand F_Quotient(List<Operand> arg)
        {
            CheckArgsCount("Quotient", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER, OperandType.NUMBER },
                 });

            var r = (int)(arg[0].NumberValue / arg[1].NumberValue);
            return new Operand(OperandType.NUMBER, (double)r);
        }

        private Operand F_Product(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("Product 参数不足", new List<Operand>());
            double d = 1;
            for (int i = 0; i < arg.Count; i++) {
                if (arg[i].Type == OperandType.NUMBER) {
                    d *= arg[i].NumberValue;
                } else if (arg[i].Type == OperandType.ARRARY) {
                    var ls = arg[i].GetNumberList();
                    foreach (var item in ls) {
                        d *= item;
                    }
                }
            }
            return new Operand(OperandType.NUMBER, d);
        }

        private Operand F_Power(List<Operand> arg)
        {
            CheckArgsCount("Quotient", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER, OperandType.NUMBER },
                 });
            return new Operand(OperandType.NUMBER, Math.Pow(arg[0].NumberValue, arg[1].NumberValue));
        }

        private Operand F_Pi(List<Operand> arg)
        {
            return new Operand(OperandType.NUMBER, Math.PI);
        }

        private Operand F_Odd(List<Operand> arg)
        {
            CheckArgsCount("ODD", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER},
                 });

            var z = arg[0].NumberValue;
            if (z % 2 == 1) {
                return arg[0];
            }
            z = Math.Ceiling(z);
            if (z % 2 == 1) {
                return new Operand(OperandType.NUMBER, z);
            }
            z = z + 1;
            return new Operand(OperandType.NUMBER, z);
        }

        private Operand F_Mround(List<Operand> arg)
        {
            CheckArgsCount("MROUND", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER, OperandType.NUMBER},
                 });

            var a = arg[1].NumberValue;
            var b = arg[0].NumberValue;
            var r = Math.Round(b / a, 0) * a;

            return new Operand(OperandType.NUMBER, r);
        }

        private Operand F_Multinomial(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("multinomial 中没有参数", new List<Operand>());
            int sum = 0;
            int n = 1;
            foreach (var item in arg) {
                if (item.Type == OperandType.NUMBER) {
                    n *= F_base_Factorial(item.IntValue);
                    sum += item.IntValue;
                } else if (item.Type == OperandType.ARRARY) {
                    var ls = item.GetNumberList();
                    foreach (var d in ls) {
                        n *= F_base_Factorial((int)d);
                        sum += (int)d;
                    }
                }
            }
            var r = F_base_Factorial(sum) / n;
            return new Operand(OperandType.NUMBER, r);
        }

 

        private Operand F_Log10(List<Operand> arg)
        {
            CheckArgsCount("log10", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER},
                 });

            return new Operand(OperandType.NUMBER, Math.Log(arg[0].NumberValue, 10));
        }

        private Operand F_Log(List<Operand> arg)
        {
            CheckArgsCount("log", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER},
                new OperandType[] { OperandType.NUMBER, OperandType.NUMBER},
                 });

            if (arg.Count > 1) {
                return new Operand(OperandType.NUMBER, Math.Log(arg[0].NumberValue, arg[1].NumberValue));
            }
            return new Operand(OperandType.NUMBER, Math.Log(arg[0].NumberValue, 10));
        }

        private Operand F_Ln(List<Operand> arg)
        {
            CheckArgsCount("ln", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER},
                 });


            return new Operand(OperandType.NUMBER, Math.Log(arg[0].NumberValue));
        }

        private Operand F_Lcm(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("Lcm 中没有参数不足", new List<Operand>());
            List<int> list = new List<int>();
            foreach (var item in arg) {
                if (item.Type == OperandType.NUMBER) {
                    list.Add(item.IntValue);
                } else if (item.Type == OperandType.ARRARY) {
                    var ls = item.GetNumberList();
                    foreach (var d in ls) {
                        list.Add((int)d);
                    }
                }
            }
            return new Operand(OperandType.NUMBER, (double)F_base_lgm(list));
        }

        private Operand F_Int(List<Operand> arg)
        {
            CheckArgsCount("int", arg, new OperandType[][] {
                new OperandType[] { OperandType.ANY},
                 });

            if (arg[0].Type == OperandType.DATE) {
                return new Operand(OperandType.NUMBER, (double)(int)arg[0].NumberValue);
            } else if (arg[0].Type == OperandType.NUMBER) {
                return new Operand(OperandType.NUMBER, (double)(int)arg[0].NumberValue);
            } else if (arg[0].Type == OperandType.BOOLEAN) {
                return new Operand(OperandType.NUMBER, arg[0].BooleanValue ? 1 : 0);
            }
            if (double.TryParse(arg[0].StringValue, out double d)) {
                return new Operand(OperandType.NUMBER, (double)(int)d);
            }
            return ThrowError("无法转成整数", arg);
        }

        private Operand F_Gcd(List<Operand> arg)
        {
            if (arg.Count < 2) return ThrowError("gcd 中没有参数不足", new List<Operand>());
            List<int> list = new List<int>();
            foreach (var item in arg) {
                if (item.Type == OperandType.NUMBER) {
                    list.Add(item.IntValue);
                } else if (item.Type == OperandType.ARRARY) {
                    var ls = item.GetNumberList();
                    foreach (var d in ls) {
                        list.Add((int)d);
                    }
                }
            }
            return new Operand(OperandType.NUMBER, (double)F_base_gcd(list));
        }


        private Operand F_Floor(List<Operand> arg)
        {
            CheckArgsCount("Floor", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER},
                new OperandType[] { OperandType.NUMBER, OperandType.NUMBER},
                 });

            if (arg.Count == 0) return ThrowError("exp 中没有参数", new List<Operand>());
            if (arg.Count == 1) return new Operand(OperandType.NUMBER, Math.Floor(arg[0].NumberValue));
            var a = arg[0].NumberValue;
            var b = arg[1].NumberValue;
            var d = Math.Floor(a / b) * b;
            return new Operand(OperandType.NUMBER, d);
        }

        private Operand F_Factdouble(List<Operand> arg)
        {
            CheckArgsCount("factdouble", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER},
                 });

            var z = (int)arg[0].NumberValue;
            if (z < 0) {
                if (arg.Count == 0) return ThrowError("factdouble中参数小于0", new List<Operand>());
            }
            double d = 1;
            for (int i = z; i > 0; i -= 2) {
                d *= i;
            }
            return new Operand(OperandType.NUMBER, d);
        }

        private Operand F_Fact(List<Operand> arg)
        {
            CheckArgsCount("fact", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER},
                 });

            var z = (int)arg[0].NumberValue;
            if (z < 0) {
                if (arg.Count == 0) return ThrowError("fact中参数小于0", new List<Operand>());
            }
            double d = 1;
            for (int i = 1; i <= z; i++) {
                d *= i;
            }
            return new Operand(OperandType.NUMBER, d);
        }

        private Operand F_Exp(List<Operand> arg)
        {
            CheckArgsCount("exp", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER},
                 });

            return new Operand(OperandType.NUMBER, Math.Exp(arg[0].NumberValue));
        }

        private Operand F_Even(List<Operand> arg)
        {
            CheckArgsCount("even", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER},
                 });

            var z = arg[0].NumberValue;
            if (z % 2 == 0) {
                return arg[0];
            }
            z = Math.Ceiling(z);
            if (z % 2 == 0) {
                return new Operand(OperandType.NUMBER, z);
            }
            z = z + 1;
            return new Operand(OperandType.NUMBER, z);
        }

        private Operand F_Degrees(List<Operand> arg)
        {
            CheckArgsCount("degrees", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER},
                 });

            var z = arg[0].NumberValue;
            var r = (z / Math.PI * 180);
            return new Operand(OperandType.NUMBER, r);
        }

        private Operand F_Cosh(List<Operand> arg)
        {
            CheckArgsCount("cosh", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER},
                 });

            return new Operand(OperandType.NUMBER, Math.Cosh(arg[0].NumberValue));
        }

        private Operand F_Cos(List<Operand> arg)
        {
            CheckArgsCount("cos", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER},
                 });

            return new Operand(OperandType.NUMBER, Math.Cos(arg[0].NumberValue));
        }

        private Operand F_Ceiling(List<Operand> arg)
        {
            CheckArgsCount("ceiling", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER},
                new OperandType[] { OperandType.NUMBER, OperandType.NUMBER},
                 });

            if (arg.Count == 1) return new Operand(OperandType.NUMBER, Math.Ceiling(arg[0].NumberValue));
            var a = arg[0].NumberValue;
            var b = arg[1].NumberValue;
            var d = Math.Ceiling(a / b) * b;
            return new Operand(OperandType.NUMBER, d);
        }

        private Operand F_Atan2(List<Operand> arg)
        {
            CheckArgsCount("atan2", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER, OperandType.NUMBER},
                 });
            return new Operand(OperandType.NUMBER, Math.Atan2(arg[1].NumberValue, arg[0].NumberValue));
        }
        private Operand F_Atanh(List<Operand> arg)
        {
            CheckArgsCount("atanh", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER },
                 });

            if (arg.Count == 0) return ThrowError("atanh 中没有参数", new List<Operand>());
            var x = arg[0].NumberValue;
            var d = Math.Log((1 + x) / (1 - x)) / 2;
            return new Operand(OperandType.NUMBER, d);
        }
        private Operand F_Atan(List<Operand> arg)
        {
            CheckArgsCount("atan", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER },
                 });

            return new Operand(OperandType.NUMBER, Math.Atan(arg[0].NumberValue));
        }
        private Operand F_Asinh(List<Operand> arg)
        {
            CheckArgsCount("asinh", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER },
                 });

            var x = arg[0].NumberValue;
            var d = Math.Log(x + Math.Sqrt(x * x + 1));
            return new Operand(OperandType.NUMBER, d);
        }

        private Operand F_Asin(List<Operand> arg)
        {
            CheckArgsCount("asin", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER },
                 });

            return new Operand(OperandType.NUMBER, Math.Asin(arg[0].NumberValue));
        }

        private Operand F_Acosh(List<Operand> arg)
        {
            CheckArgsCount("acosh", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER },
                 });

            var z = arg[0].NumberValue;
            if (z < 1) {
                if (arg.Count == 0) return ThrowError("acosh中参数小于1", new List<Operand>());
            }
            var r = Math.Log(z + Math.Pow(z * z - 1, 0.5));

            return new Operand(OperandType.NUMBER, r);
        }

        private Operand F_Acos(List<Operand> arg)
        {
            CheckArgsCount("acos", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER },
                 });


            return new Operand(OperandType.NUMBER, Math.Acos(arg[0].NumberValue));
        }

        private Operand F_Abs(List<Operand> arg)
        {
            CheckArgsCount("abs", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER },
                 });

            return new Operand(OperandType.NUMBER, Math.Abs(arg[0].NumberValue));
        }


    }
}
