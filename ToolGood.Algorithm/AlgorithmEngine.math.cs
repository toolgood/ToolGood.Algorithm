using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm
{
    partial class AlgorithmEngine
    {
        Random rand = new Random();

        private void AddMathFunction()
        {
            addFunc("abs", abs);//返回数字的绝对值 
            addFunc("acos", acos);//返回数字的反余弦值 
            addFunc("acosh", acosh);//返回数字的反双曲余弦值 
            addFunc("asin", asin);//返回数字的反正弦值 
            addFunc("asinh", asinh);//返回数字的反正弦值 
            addFunc("atan", atan);//返回数字的反正切值
            addFunc("atanh", atanh);//返回数字的反正切值


            addFunc("atan2", atan2);//从 X 和 Y 坐标返回反正切
            addFunc("ceiling", ceiling);//将数字舍入为最接近的整数，或最接近的有效数字的倍数
            addFunc("combin", combin);//计算从给定数目的对象集合中提取若干对象的组合数
            addFunc("cos", cos);//返回数字的余弦值
            addFunc("cosh", cosh);//返回数字的双曲余弦值
            addFunc("degrees", degrees);//将弧度转换为度
            addFunc("even", even);//将数字向上舍入为最接近的偶型整数
            addFunc("exp", exp);//返回 e 的指定数乘幂
            addFunc("fact", fact);//返回数字的阶乘
            addFunc("factdouble", factdouble);//返回数字的双倍阶乘
            addFunc("floor", floor);//返回数字的双倍阶乘
            addFunc("GCD", gcd);//返回最大公约数
            addFunc("INT", @int);//返回最大公约数
            addFunc("LCM", LCM);//返回整数参数的最小公倍数。 
            addFunc("ln", ln);//返回数字的自然对数
            addFunc("log", log);//返回数字的自然对数
            addFunc("LOG10", log10);//返回数字的常用对数
            addFunc("MULTINOMIAL", multinomial);//返回参数和的阶乘与各参数阶乘乘积的比值
            //addFunc("MINVERSE", minverse);//返回数组矩阵的逆距阵
            //addFunc("MDETERM", mdeterm);//返回一个数组的矩阵行列式的值
            //addFunc("MMULT", MMULT);//返回两个数组的矩阵乘积
            addFunc("MOD", mod);//返回两数相除的余数
            addFunc("MROUND", MROUND);//返回一个舍入到所需倍数的数字
            addFunc("ODD", ODD);//将数字向上舍入为最接近的奇型整数
            addFunc("PI", PI);//返回 PI 值
            addFunc("POWER", POWER);//返回数的乘幂结果
            addFunc("PRODUCT", PRODUCT);//将所有以参数形式给出的数字相乘
            addFunc("QUOTIENT", QUOTIENT);//返回商的整数部分，该函数可用于舍掉商的小数部分。 
            addFunc("RADIANS", RADIANS);//将度转换为弧度 
            addFunc("RAND", RAND);//返回 0 到 1 之间的随机数 
            addFunc("RANDBETWEEN", RANDBETWEEN);//返回指定数字之间的随机数
            //addFunc("ROMAN", ROMAN);//将阿拉伯数字转换为文本形式的罗马数字
            addFunc("ROUND", ROUND);//将数字舍入到指定位数
            addFunc("ROUNDDOWN", ROUNDDOWN);//将数字朝零的方向舍入
            addFunc("ROUNDUP", ROUNDUP);//将数朝远离零的方向舍入
            //addFunc("SERIESSUM", SERIESSUM);//返回基于公式的幂级数的和
            addFunc("SIGN", SIGN);//返回数字的符号
            addFunc("SIN", SIN);//返回给定角度的正弦值
            addFunc("SINH", SINH);//返回数字的双曲正弦值
            addFunc("SQRTPI", SQRTPI);//返回某数与 PI 的乘积的平方根
            addFunc("SQRT", SQRT);//返回正平方根
            //addFunc("SUBTOTAL", SUBTOTAL);//返回数据库清单或数据库中的分类汇总
            addFunc("SUM", SUM);//将参数求和
            //addFunc("SUMIF", SUMIF);//按给定条件将指定单元格求和
            //addFunc("SUMPRODUCT", SUMPRODUCT);//返回相对应的数组部分的乘积和
            addFunc("SUMSQ", SUMSQ);//返回参数的平方和
            //addFunc("SUMX2MY2", SUMX2MY2);//返回两数组中对应值平方差之和
            //addFunc("SUMX2PY2", SUMX2PY2);//返回两数组中对应值的平方和之和
            //addFunc("SUMXMY2", SUMXMY2);//返回两数组中对应值的平方和之和
            addFunc("TAN", TAN);//返回数字的正切值
            addFunc("TANH", TANH);//返回数字的双曲正切值
            addFunc("TRUNC", TRUNC);//将数字截尾取整
            addFunc("PERMUT", PERMUT);//返回从给定数目的对象集合中选取的若干对象的排列数


        }

        private Operand PERMUT(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("TRUNC中参数不足", new List<Operand>());
            var total = arg[0].IntValue;
            var count = arg[1].IntValue;
            double sum = 1;
            for (int i = 0; i < count; i++) {
                sum *= (total - i);
            }
            return new Operand(OperandType.NUMBER, sum);
        }

        private Operand combin(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("TRUNC中参数不足", new List<Operand>());
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

        private Operand TRUNC(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("TRUNC中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, (double)(int)(arg[0].NumberValue));
        }


        private Operand TANH(List<Operand> arg)
        {
            if (arg.Count == 0) return throwError("TANH中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, Math.Tanh(arg[0].NumberValue));
        }

        private Operand TAN(List<Operand> arg)
        {
            if (arg.Count == 0) return throwError("TAN中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, Math.Tan(arg[0].NumberValue));
        }

        private Operand SUMSQ(List<Operand> arg)
        {
            if (arg.Count == 0) return throwError("SUMSQ中参数不足", new List<Operand>());
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

        private Operand SUM(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("SUM中参数不足", new List<Operand>());
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

        private Operand SQRT(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("SQRT中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, Math.Sqrt(arg[0].NumberValue));
        }

        private Operand SQRTPI(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("SQRTPI中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, Math.Sqrt(arg[0].NumberValue * Math.PI));
        }

        private Operand SINH(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("SINH中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, Math.Sinh(arg[0].NumberValue));
        }

        private Operand SIN(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("SIN中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, Math.Sin(arg[0].NumberValue));
        }

        private Operand SIGN(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("SIGN中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, (double)Math.Sign(arg[0].NumberValue));
        }

        private Operand ROUNDUP(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("ROUNDUP中参数不足", new List<Operand>());

            var a = Math.Pow(10, (int)arg[1].NumberValue);
            var b = arg[0].NumberValue;

            var t = (Math.Ceiling(Math.Abs(b) * a)) / a;
            if (b > 0) return new Operand(OperandType.NUMBER, t);
            return new Operand(OperandType.NUMBER, -t);
        }

        private Operand ROUNDDOWN(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("ROUNDDOW中参数不足", new List<Operand>());
            var a = Math.Pow(10, (int)arg[1].NumberValue);
            var b = arg[0].NumberValue;

            b = ((double)(int)(b * a)) / a;
            return new Operand(OperandType.NUMBER, b);
        }

        private Operand ROUND(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("ROUND中参数不足", new List<Operand>());
            if (arg.Count == 1) {
                return new Operand(OperandType.NUMBER, Math.Round(arg[0].NumberValue));
            }
            return new Operand(OperandType.NUMBER, Math.Round(arg[0].NumberValue, (int)arg[1].NumberValue));
        }

        private Operand RANDBETWEEN(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("RANDBETWEEN中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, rand.NextDouble() * (arg[1].NumberValue - arg[0].NumberValue) + arg[0].NumberValue);

        }

        private Operand RAND(List<Operand> arg)
        {
            return new Operand(OperandType.NUMBER, rand.NextDouble());
        }

        private Operand RADIANS(List<Operand> arg)
        {
            if (arg.Count == 0) return throwError("MROUND中参数不足", new List<Operand>());
            var r = arg[0].NumberValue / 180 * Math.PI;
            return new Operand(OperandType.NUMBER, r);
        }

        private Operand QUOTIENT(List<Operand> arg)
        {
            if (arg.Count == 0) return throwError("MROUND中参数不足", new List<Operand>());
            var r = (int)(arg[0].NumberValue / arg[1].NumberValue);
            return new Operand(OperandType.NUMBER, (double)r);
        }

        private Operand PRODUCT(List<Operand> arg)
        {
            if (arg.Count == 0) return throwError("MROUND中参数不足", new List<Operand>());
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

        private Operand POWER(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("MROUND中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, Math.Pow(arg[0].NumberValue, arg[1].NumberValue));
        }

        private Operand PI(List<Operand> arg)
        {
            return new Operand(OperandType.NUMBER, Math.PI);
        }

        private Operand ODD(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("ODD中参数不足", new List<Operand>());
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

        private Operand MROUND(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("MROUND中参数不足", new List<Operand>());
            var a = arg[1].NumberValue;
            var b = arg[0].NumberValue;
            var r = Math.Round(b / a, 0) * a;

            return new Operand(OperandType.NUMBER, r);
        }

        private Operand multinomial(List<Operand> arg)
        {
            if (arg.Count == 0) return throwError("multinomial中没有参数", new List<Operand>());
            int sum = 0;
            int n = 1;
            foreach (var item in arg) {
                if (item.Type == OperandType.NUMBER) {
                    n *= factorial(item.IntValue);
                    sum += item.IntValue;
                } else if (item.Type == OperandType.ARRARY) {
                    var ls = item.GetNumberList();
                    foreach (var d in ls) {
                        n *= factorial((int)d);
                        sum += (int)d;
                    }
                }
            }
            var r = factorial(sum) / n;
            return new Operand(OperandType.NUMBER, r);
        }

        private int factorial(int a)
        {
            int r = 1;
            for (int i = a; i > 0; i--) {
                r *= i;
            }
            return r;
        }

        private Operand log10(List<Operand> arg)
        {
            if (arg.Count == 0) return throwError("log10中没有参数", new List<Operand>());

            return new Operand(OperandType.NUMBER, Math.Log(arg[0].NumberValue, 10));
        }

        private Operand log(List<Operand> arg)
        {
            if (arg.Count == 0) return throwError("log中没有参数", new List<Operand>());
            if (arg.Count > 1) {
                return new Operand(OperandType.NUMBER, Math.Log(arg[0].NumberValue, arg[1].NumberValue));
            }
            return new Operand(OperandType.NUMBER, Math.Log(arg[0].NumberValue, 10));
        }

        private Operand ln(List<Operand> arg)
        {
            if (arg.Count == 0) return throwError("ln中没有参数", new List<Operand>());

            return new Operand(OperandType.NUMBER, Math.Log(arg[0].NumberValue));
        }

        private Operand LCM(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("lgm中没有参数不足", new List<Operand>());
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
            return new Operand(OperandType.NUMBER, (double)lgm(list));
        }

        private Operand @int(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("int中没有参数不足", new List<Operand>());
            if (arg[0].Type == OperandType.DATE) {
                return new Operand(OperandType.NUMBER, (double)(int)arg[0].NumberValue);
            } else if (arg[0].Type == OperandType.NUMBER) {
                return new Operand(OperandType.NUMBER, (double)(int)arg[0].NumberValue);
            } else if (arg[0].Type == OperandType.BOOLEAN) {
                return new Operand(OperandType.NUMBER, arg[0].BooleanValue ? 1 : 0);
            }
            double d;
            if (double.TryParse(arg[0].StringValue, out d)) {
                return new Operand(OperandType.NUMBER, (double)(int)d);
            }
            return throwError("无法转成整数", arg);
        }

        private Operand gcd(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("gcd中没有参数不足", new List<Operand>());
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
            return new Operand(OperandType.NUMBER, (double)gcd(list));
        }


        private Operand floor(List<Operand> arg)
        {
            if (arg.Count == 0) return throwError("exp中没有参数", new List<Operand>());
            if (arg.Count == 1) return new Operand(OperandType.NUMBER, Math.Floor(arg[0].NumberValue));
            var a = arg[0].NumberValue;
            var b = arg[1].NumberValue;
            var d = Math.Floor(a / b) * b;
            return new Operand(OperandType.NUMBER, d);
        }

        private Operand factdouble(List<Operand> arg)
        {
            if (arg.Count == 0) return throwError("factdouble中没有参数", new List<Operand>());
            var z = (int)arg[0].NumberValue;
            if (z < 0) {
                if (arg.Count == 0) return throwError("factdouble中参数小于0", new List<Operand>());
            }
            double d = 1;
            for (int i = z; i > 0; i -= 2) {
                d *= i;
            }
            return new Operand(OperandType.NUMBER, d);
        }

        private Operand fact(List<Operand> arg)
        {
            if (arg.Count == 0) return throwError("fact中没有参数", new List<Operand>());
            var z = (int)arg[0].NumberValue;
            if (z < 0) {
                if (arg.Count == 0) return throwError("fact中参数小于0", new List<Operand>());
            }
            double d = 1;
            for (int i = 1; i <= z; i++) {
                d *= i;
            }
            return new Operand(OperandType.NUMBER, d);
        }

        private Operand exp(List<Operand> arg)
        {
            if (arg.Count == 0) return throwError("exp中没有参数", new List<Operand>());

            return new Operand(OperandType.NUMBER, Math.Exp(arg[0].NumberValue));
        }

        private Operand even(List<Operand> arg)
        {
            if (arg.Count == 0) return throwError("even中没有参数", new List<Operand>());

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

        private Operand degrees(List<Operand> arg)
        {
            if (arg.Count == 0) return throwError("degrees中没有参数", new List<Operand>());

            var z = arg[0].NumberValue;
            var r = (z / Math.PI * 180);


            return new Operand(OperandType.NUMBER, r);
        }

        private Operand cosh(List<Operand> arg)
        {
            if (arg.Count == 0) return throwError("cosh中没有参数", new List<Operand>());

            return new Operand(OperandType.NUMBER, Math.Cosh(arg[0].NumberValue));
        }

        private Operand cos(List<Operand> arg)
        {
            if (arg.Count == 0) return throwError("cos中没有参数", new List<Operand>());

            return new Operand(OperandType.NUMBER, Math.Cos(arg[0].NumberValue));
        }

        private Operand ceiling(List<Operand> arg)
        {
            if (arg.Count == 0) return throwError("ceiling中没有参数", new List<Operand>());
            if (arg.Count == 1) return new Operand(OperandType.NUMBER, Math.Ceiling(arg[0].NumberValue));
            var a = arg[0].NumberValue;
            var b = arg[1].NumberValue;
            var d = Math.Ceiling(a / b) * b;
            return new Operand(OperandType.NUMBER, d);
        }

        private Operand atan2(List<Operand> arg)
        {
            if (arg.Count == 0) return throwError("atan2中没有参数", new List<Operand>());

            return new Operand(OperandType.NUMBER, Math.Atan2(arg[1].NumberValue, arg[0].NumberValue));
        }
        private Operand atanh(List<Operand> arg)
        {
            if (arg.Count == 0) return throwError("atanh中没有参数", new List<Operand>());
            var x = arg[0].NumberValue;
            var d = Math.Log((1 + x) / (1 - x)) / 2;
            return new Operand(OperandType.NUMBER, d);
        }
        private Operand atan(List<Operand> arg)
        {
            if (arg.Count == 0) return throwError("atan中没有参数", new List<Operand>());

            return new Operand(OperandType.NUMBER, Math.Atan(arg[0].NumberValue));
        }
        private Operand asinh(List<Operand> arg)
        {
            if (arg.Count == 0) return throwError("asinh中没有参数", new List<Operand>());
            var x = arg[0].NumberValue;
            var d = Math.Log(x + Math.Sqrt(x * x + 1));
            return new Operand(OperandType.NUMBER, d);
        }

        private Operand asin(List<Operand> arg)
        {
            if (arg.Count == 0) return throwError("asin中没有参数", new List<Operand>());

            return new Operand(OperandType.NUMBER, Math.Asin(arg[0].NumberValue));
        }

        private Operand acosh(List<Operand> arg)
        {
            if (arg.Count == 0) return throwError("acosh中没有参数", new List<Operand>());
            var z = arg[0].NumberValue;
            if (z < 1) {
                if (arg.Count == 0) return throwError("acosh中参数小于1", new List<Operand>());
            }
            var r = Math.Log(z + Math.Pow(z * z - 1, 0.5));

            return new Operand(OperandType.NUMBER, r);
        }

        private Operand acos(List<Operand> arg)
        {
            if (arg.Count == 0) return throwError("acos中没有参数", new List<Operand>());

            return new Operand(OperandType.NUMBER, Math.Acos(arg[0].NumberValue));
        }

        private Operand abs(List<Operand> ops)
        {
            if (ops.Count == 0) return throwError("abs中没有参数", new List<Operand>());

            return new Operand(OperandType.NUMBER, Math.Abs(ops[0].NumberValue));
        }


    }
}
