// MathEx.h - decimal 级数学函数库
// 逐段移植 csharp/ToolGood.Algorithm/System/MathEx.cs：
// 全部以 Decimal（96 位尾数 / 28~29 位有效数字）计算，算法、迭代次数与收敛阈值与 C# 一致。
// 溢出（C# OverflowException）抛 std::overflow_error；
// 定义域非法（C# ArgumentException / InvalidOperationException）抛 std::domain_error。
#pragma once

#include "Decimal.h"

namespace toolgood {
namespace MathEx {

/// 自然对数底 e（对应 MathEx.E）
const Decimal& E();
/// 圆周率（对应 MathEx.PI）
const Decimal& PI();

/// e^x
Decimal Exp(const Decimal& x);
/// 自然对数 ln(x)，x 必须大于 0
Decimal Log(const Decimal& x);
/// 以 newBase 为底的对数
Decimal Log(const Decimal& d, const Decimal& newBase);
/// 常用对数 log10(x)
Decimal Log10(const Decimal& x);
/// value^pow
Decimal Pow(const Decimal& value, const Decimal& pow);
/// value^power（整数次幂，逐次平方）
Decimal PowerN(const Decimal& value, int power);
/// 平方根（复用 Decimal::Sqrt 的 96 位精确开方）
Decimal Sqrt(const Decimal& x);
/// decimal 语义的除法（标度 <= 28、96 位尾数、HALF_EVEN，与 C# 的 `/` 一致）。
/// Decimal::Div 按 34 位有效数字计算后再归一，末位可能与 C# 相差 1，
/// 因此凡 C# 直接用 `/` 参与超越函数计算的地方都应改用本函数。
Decimal Div(const Decimal& a, const Decimal& b);

Decimal Sin(const Decimal& x);
Decimal Cos(const Decimal& x);
Decimal Tan(const Decimal& x);
Decimal Asin(const Decimal& x);
Decimal Acos(const Decimal& x);
Decimal Atan(const Decimal& x);
Decimal Atan2(const Decimal& y, const Decimal& x);
Decimal Sinh(const Decimal& x);
Decimal Cosh(const Decimal& x);
Decimal Tanh(const Decimal& x);
Decimal Asinh(const Decimal& x);
Decimal Acosh(const Decimal& x);
Decimal Atanh(const Decimal& x);

}  // namespace MathEx
}  // namespace toolgood
