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
            addFunc("abs", Func_Abs);//返回数字的绝对值 
            addFunc("acos", Func_Acos);//返回数字的反余弦值 
            addFunc("acosh", Func_Acosh);//返回数字的反双曲余弦值 
            addFunc("asin", Func_Asin);//返回数字的反正弦值 
            addFunc("asinh", Func_Asinh);//返回数字的反正弦值 
            addFunc("atan", Func_Atan);//返回数字的反正切值
            addFunc("atanh", Func_Atanh);//返回数字的反正切值


            addFunc("atan2", Func_Atan2);//从 X 和 Y 坐标返回反正切
            addFunc("ceiling", Func_Ceiling);//将数字舍入为最接近的整数，或最接近的有效数字的倍数
            addFunc("combin", Func_Combin);//计算从给定数目的对象集合中提取若干对象的组合数
            addFunc("cos", Func_Cos);//返回数字的余弦值
            addFunc("cosh", Func_Cosh);//返回数字的双曲余弦值
            addFunc("degrees", Func_Degrees);//将弧度转换为度
            addFunc("even", Func_Even);//将数字向上舍入为最接近的偶型整数
            addFunc("exp", Func_Exp);//返回 e 的指定数乘幂
            addFunc("fact", Func_Fact);//返回数字的阶乘
            addFunc("factdouble", Func_Factdouble);//返回数字的双倍阶乘
            addFunc("floor", Func_Floor);//返回数字的双倍阶乘
            addFunc("GCD", Func_Gcd);//返回最大公约数
            addFunc("INT", Func_Int);//返回最大公约数
            addFunc("LCM", Func_Lcm);//返回整数参数的最小公倍数。 
            addFunc("ln", Func_Ln);//返回数字的自然对数
            addFunc("log", Func_Log);//返回数字的自然对数
            addFunc("LOG10", Func_Log10);//返回数字的常用对数
            addFunc("MULTINOMIAL", Func_Multinomial);//返回参数和的阶乘与各参数阶乘乘积的比值
            //addFunc("MINVERSE", minverse);//返回数组矩阵的逆距阵
            //addFunc("MDETERM", mdeterm);//返回一个数组的矩阵行列式的值
            //addFunc("MMULT", MMULT);//返回两个数组的矩阵乘积
            addFunc("MOD", Func_Mod);//返回两数相除的余数
            addFunc("MROUND", Func_Mround);//返回一个舍入到所需倍数的数字
            addFunc("ODD", Func_Odd);//将数字向上舍入为最接近的奇型整数
            addFunc("PI", Func_Pi);//返回 PI 值
            addFunc("POWER", Func_Power);//返回数的乘幂结果
            addFunc("PRODUCT", Func_Product);//将所有以参数形式给出的数字相乘
            addFunc("QUOTIENT", Func_Quotient);//返回商的整数部分，该函数可用于舍掉商的小数部分。 
            addFunc("RADIANS", Func_Radians);//将度转换为弧度 
            addFunc("RAND", Func_Rand);//返回 0 到 1 之间的随机数 
            addFunc("RANDBETWEEN", Func_Randbetween);//返回指定数字之间的随机数
            //addFunc("ROMAN", ROMAN);//将阿拉伯数字转换为文本形式的罗马数字
            addFunc("ROUND", Func_Round);//将数字舍入到指定位数
            addFunc("ROUNDDOWN", Func_RoundDown);//将数字朝零的方向舍入
            addFunc("ROUNDUP", Func_RoundUp);//将数朝远离零的方向舍入
            //addFunc("SERIESSUM", SERIESSUM);//返回基于公式的幂级数的和
            addFunc("SIGN", Func_Sign);//返回数字的符号
            addFunc("SIN", Func_Sin);//返回给定角度的正弦值
            addFunc("SINH", Func_Sinh);//返回数字的双曲正弦值
            addFunc("SQRTPI", Func_SqrtPi);//返回某数与 PI 的乘积的平方根
            addFunc("SQRT", Func_Sqrt);//返回正平方根
            //addFunc("SUBTOTAL", SUBTOTAL);//返回数据库清单或数据库中的分类汇总
            addFunc("SUM", Func_Sum);//将参数求和
            //addFunc("SUMIF", SUMIF);//按给定条件将指定单元格求和
            //addFunc("SUMPRODUCT", SUMPRODUCT);//返回相对应的数组部分的乘积和
            addFunc("SUMSQ", Func_SumSq);//返回参数的平方和
            //addFunc("SUMX2MY2", SUMX2MY2);//返回两数组中对应值平方差之和
            //addFunc("SUMX2PY2", SUMX2PY2);//返回两数组中对应值的平方和之和
            //addFunc("SUMXMY2", SUMXMY2);//返回两数组中对应值的平方和之和
            addFunc("TAN", Func_Tan);//返回数字的正切值
            addFunc("TANH", Func_Tanh);//返回数字的双曲正切值
            addFunc("TRUNC", Func_Trunc);//将数字截尾取整
            addFunc("PERMUT", Func_Permut);//返回从给定数目的对象集合中选取的若干对象的排列数


        }

        private Operand Func_Permut(List<Operand> arg)
        {
            if (arg.Count < 2) return ThrowError("TRUNC中参数不足", new List<Operand>());
            var total = arg[0].IntValue;
            var count = arg[1].IntValue;
            double sum = 1;
            for (int i = 0; i < count; i++) {
                sum *= (total - i);
            }
            return new Operand(OperandType.NUMBER, sum);
        }

        private Operand Func_Combin(List<Operand> arg)
        {
            if (arg.Count < 2) return ThrowError("TRUNC中参数不足", new List<Operand>());
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

        private Operand Func_Trunc(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("TRUNC中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, (double)(int)(arg[0].NumberValue));
        }


        private Operand Func_Tanh(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("TANH中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, Math.Tanh(arg[0].NumberValue));
        }

        private Operand Func_Tan(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("TAN中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, Math.Tan(arg[0].NumberValue));
        }

        private Operand Func_SumSq(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("SUMSQ中参数不足", new List<Operand>());
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

        private Operand Func_Sum(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("SUM中参数不足", new List<Operand>());
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

        private Operand Func_Sqrt(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("SQRT中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, Math.Sqrt(arg[0].NumberValue));
        }

        private Operand Func_SqrtPi(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("SQRTPI中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, Math.Sqrt(arg[0].NumberValue * Math.PI));
        }

        private Operand Func_Sinh(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("SINH中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, Math.Sinh(arg[0].NumberValue));
        }

        private Operand Func_Sin(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("SIN中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, Math.Sin(arg[0].NumberValue));
        }

        private Operand Func_Sign(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("SIGN中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, (double)Math.Sign(arg[0].NumberValue));
        }

        private Operand Func_RoundUp(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("ROUNDUP中参数不足", new List<Operand>());

            var a = Math.Pow(10, (int)arg[1].NumberValue);
            var b = arg[0].NumberValue;

            var t = (Math.Ceiling(Math.Abs(b) * a)) / a;
            if (b > 0) return new Operand(OperandType.NUMBER, t);
            return new Operand(OperandType.NUMBER, -t);
        }

        private Operand Func_RoundDown(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("ROUNDDOW中参数不足", new List<Operand>());
            var a = Math.Pow(10, (int)arg[1].NumberValue);
            var b = arg[0].NumberValue;

            b = ((double)(int)(b * a)) / a;
            return new Operand(OperandType.NUMBER, b);
        }

        private Operand Func_Round(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("ROUND中参数不足", new List<Operand>());
            if (arg.Count == 1) {
                return new Operand(OperandType.NUMBER, Math.Round(arg[0].NumberValue));
            }
            return new Operand(OperandType.NUMBER, Math.Round(arg[0].NumberValue, (int)arg[1].NumberValue));
        }

        private Operand Func_Randbetween(List<Operand> arg)
        {
            if (arg.Count < 2) return ThrowError("RANDBETWEEN中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, rand.NextDouble() * (arg[1].NumberValue - arg[0].NumberValue) + arg[0].NumberValue);

        }

        private Operand Func_Rand(List<Operand> arg)
        {
            return new Operand(OperandType.NUMBER, rand.NextDouble());
        }

        private Operand Func_Radians(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("MROUND中参数不足", new List<Operand>());
            var r = arg[0].NumberValue / 180 * Math.PI;
            return new Operand(OperandType.NUMBER, r);
        }

        private Operand Func_Quotient(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("MROUND中参数不足", new List<Operand>());
            var r = (int)(arg[0].NumberValue / arg[1].NumberValue);
            return new Operand(OperandType.NUMBER, (double)r);
        }

        private Operand Func_Product(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("MROUND中参数不足", new List<Operand>());
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

        private Operand Func_Power(List<Operand> arg)
        {
            if (arg.Count < 2) return ThrowError("MROUND中参数不足", new List<Operand>());
            return new Operand(OperandType.NUMBER, Math.Pow(arg[0].NumberValue, arg[1].NumberValue));
        }

        private Operand Func_Pi(List<Operand> arg)
        {
            return new Operand(OperandType.NUMBER, Math.PI);
        }

        private Operand Func_Odd(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("ODD中参数不足", new List<Operand>());
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

        private Operand Func_Mround(List<Operand> arg)
        {
            if (arg.Count < 2) return ThrowError("MROUND中参数不足", new List<Operand>());
            var a = arg[1].NumberValue;
            var b = arg[0].NumberValue;
            var r = Math.Round(b / a, 0) * a;

            return new Operand(OperandType.NUMBER, r);
        }

        private Operand Func_Multinomial(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("multinomial中没有参数", new List<Operand>());
            int sum = 0;
            int n = 1;
            foreach (var item in arg) {
                if (item.Type == OperandType.NUMBER) {
                    n *= Factorial(item.IntValue);
                    sum += item.IntValue;
                } else if (item.Type == OperandType.ARRARY) {
                    var ls = item.GetNumberList();
                    foreach (var d in ls) {
                        n *= Factorial((int)d);
                        sum += (int)d;
                    }
                }
            }
            var r = Factorial(sum) / n;
            return new Operand(OperandType.NUMBER, r);
        }

        private int Factorial(int a)
        {
            int r = 1;
            for (int i = a; i > 0; i--) {
                r *= i;
            }
            return r;
        }

        private Operand Func_Log10(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("log10中没有参数", new List<Operand>());

            return new Operand(OperandType.NUMBER, Math.Log(arg[0].NumberValue, 10));
        }

        private Operand Func_Log(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("log中没有参数", new List<Operand>());
            if (arg.Count > 1) {
                return new Operand(OperandType.NUMBER, Math.Log(arg[0].NumberValue, arg[1].NumberValue));
            }
            return new Operand(OperandType.NUMBER, Math.Log(arg[0].NumberValue, 10));
        }

        private Operand Func_Ln(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("ln中没有参数", new List<Operand>());

            return new Operand(OperandType.NUMBER, Math.Log(arg[0].NumberValue));
        }

        private Operand Func_Lcm(List<Operand> arg)
        {
            if (arg.Count < 2) return ThrowError("lgm中没有参数不足", new List<Operand>());
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

        private Operand Func_Int(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("int中没有参数不足", new List<Operand>());
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

        private Operand Func_Gcd(List<Operand> arg)
        {
            if (arg.Count < 2) return ThrowError("gcd中没有参数不足", new List<Operand>());
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


        private Operand Func_Floor(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("exp中没有参数", new List<Operand>());
            if (arg.Count == 1) return new Operand(OperandType.NUMBER, Math.Floor(arg[0].NumberValue));
            var a = arg[0].NumberValue;
            var b = arg[1].NumberValue;
            var d = Math.Floor(a / b) * b;
            return new Operand(OperandType.NUMBER, d);
        }

        private Operand Func_Factdouble(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("factdouble中没有参数", new List<Operand>());
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

        private Operand Func_Fact(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("fact中没有参数", new List<Operand>());
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

        private Operand Func_Exp(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("exp中没有参数", new List<Operand>());

            return new Operand(OperandType.NUMBER, Math.Exp(arg[0].NumberValue));
        }

        private Operand Func_Even(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("even中没有参数", new List<Operand>());

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

        private Operand Func_Degrees(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("degrees中没有参数", new List<Operand>());

            var z = arg[0].NumberValue;
            var r = (z / Math.PI * 180);


            return new Operand(OperandType.NUMBER, r);
        }

        private Operand Func_Cosh(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("cosh中没有参数", new List<Operand>());

            return new Operand(OperandType.NUMBER, Math.Cosh(arg[0].NumberValue));
        }

        private Operand Func_Cos(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("cos中没有参数", new List<Operand>());

            return new Operand(OperandType.NUMBER, Math.Cos(arg[0].NumberValue));
        }

        private Operand Func_Ceiling(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("ceiling中没有参数", new List<Operand>());
            if (arg.Count == 1) return new Operand(OperandType.NUMBER, Math.Ceiling(arg[0].NumberValue));
            var a = arg[0].NumberValue;
            var b = arg[1].NumberValue;
            var d = Math.Ceiling(a / b) * b;
            return new Operand(OperandType.NUMBER, d);
        }

        private Operand Func_Atan2(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("atan2中没有参数", new List<Operand>());

            return new Operand(OperandType.NUMBER, Math.Atan2(arg[1].NumberValue, arg[0].NumberValue));
        }
        private Operand Func_Atanh(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("atanh中没有参数", new List<Operand>());
            var x = arg[0].NumberValue;
            var d = Math.Log((1 + x) / (1 - x)) / 2;
            return new Operand(OperandType.NUMBER, d);
        }
        private Operand Func_Atan(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("atan中没有参数", new List<Operand>());

            return new Operand(OperandType.NUMBER, Math.Atan(arg[0].NumberValue));
        }
        private Operand Func_Asinh(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("asinh中没有参数", new List<Operand>());
            var x = arg[0].NumberValue;
            var d = Math.Log(x + Math.Sqrt(x * x + 1));
            return new Operand(OperandType.NUMBER, d);
        }

        private Operand Func_Asin(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("asin中没有参数", new List<Operand>());

            return new Operand(OperandType.NUMBER, Math.Asin(arg[0].NumberValue));
        }

        private Operand Func_Acosh(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("acosh中没有参数", new List<Operand>());
            var z = arg[0].NumberValue;
            if (z < 1) {
                if (arg.Count == 0) return ThrowError("acosh中参数小于1", new List<Operand>());
            }
            var r = Math.Log(z + Math.Pow(z * z - 1, 0.5));

            return new Operand(OperandType.NUMBER, r);
        }

        private Operand Func_Acos(List<Operand> arg)
        {
            if (arg.Count == 0) return ThrowError("acos中没有参数", new List<Operand>());

            return new Operand(OperandType.NUMBER, Math.Acos(arg[0].NumberValue));
        }

        private Operand Func_Abs(List<Operand> ops)
        {
            if (ops.Count == 0) return ThrowError("abs中没有参数", new List<Operand>());

            return new Operand(OperandType.NUMBER, Math.Abs(ops[0].NumberValue));
        }


    }
}
