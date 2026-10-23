// MathNet.h - 对应 csharp/ToolGood.Algorithm/MathNet/
// C# 版基于 System.Decimal（28 位有效数字）实现的 MathNet.Numerics 裁剪版，
// 这里改用 double 逐段翻译，算法、迭代次数与收敛阈值保持一致（阈值按 double 精度适配）。
#pragma once

#include <functional>
#include <vector>

namespace toolgood {
namespace mn {

// ---------------------------------------------------------------------------
// 常量（对应 Constants.cs）
// ---------------------------------------------------------------------------
constexpr double LnPi = 1.1447298858494001741434273513530587116472948129153;
constexpr double Sqrt2 = 1.4142135623730950488016887242096980785696718753769;
constexpr double Sqrt2Pi = 2.5066282746310005024157652848110452530069867406099;
constexpr double LogTwoSqrtEOverPi = 0.6207822376352452223455184457816472122518527279025978;
constexpr double TwoSqrtEOverPi = 1.8603827342052657173362492472666631120594218414085755;
/// 圆周率（对应 MathEx.PI）
constexpr double Pi = 3.14159265358979323846264338327950288419716939937510;
/// 自然对数底（对应 MathEx.E）
constexpr double E = 2.7182818284590452353602874713526624977572470936999595749;
/// decimal.MaxValue，C# 中溢出/无界时返回该值
constexpr double DecimalMax = 7.9228162514264337593543950335e28;

// ---------------------------------------------------------------------------
// 精度（对应 Precision.cs）
// ---------------------------------------------------------------------------
struct Precision {
    /// 对应 C# Precision.DecimalPrecision
    static constexpr double DecimalPrecision = 1e-28;
    /// 对应 C# Precision.PositiveDecimalPrecision
    static constexpr double PositiveDecimalPrecision = 2e-28;
    /// 对应 C# Precision.DefaultDecimalAccuracy；double 下取机器精度量级
    static constexpr double DefaultDecimalAccuracy = 1e-16;
    /// 连分式收敛阈值（C# 用 1e-28，double 取机器精度量级）
    static constexpr double ConvergenceEpsilon = 1e-16;

    static bool AlmostEqualNormRelative(double a, double b, double diff, double maximumError);
    static bool AlmostEqualRelative(double a, double b);
    static bool AlmostEqual(double a, double b);
    static bool AlmostEqualNorm(double a, double b, double diff, double maximumAbsoluteError);
};

// ---------------------------------------------------------------------------
// 特殊函数（对应 SpecialFunctions/*.cs）
// ---------------------------------------------------------------------------
struct SpecialFunctions {
    /// 伽玛函数（Lanczos 展开，对应 SpecialFunctions/Gamma.cs）
    static double Gamma(double z);
    /// 伽玛函数对数（对应 SpecialFunctions/Beta.cs 中的 GammaLn）
    static double GammaLn(double z);
    /// 贝塔函数 B(a,b)（C# 无此方法，按 GammaLn 组合实现）
    static double Beta(double a, double b);
    /// 正则化不完全贝塔函数 I_x(a,b)（连分式）
    static double BetaRegularized(double a, double b, double x);
    /// 正则化下不完全伽玛函数 P(a,x)
    static double GammaLowerRegularized(double a, double x);
    /// 正则化下不完全伽玛函数的反函数
    static double GammaLowerRegularizedInv(double a, double y0);

    static double Erf(double x);
    static double Erfc(double x);
    static double ErfInv(double x);
    static double ErfcInv(double x);

    /// ln(x!)；x <= 1 时为 0（对应 Statistics/Factorial.cs）
    static double FactorialLn(int x);
    /// ln(C(n,k))
    static double BinomialLn(int n, int k);

    // 整数阶贝塞尔函数
    static double BesselI(int n, double x);
    static double BesselJ(int n, double x);
    static double BesselK(int n, double x);
    static double BesselY(int n, double x);
};

// ---------------------------------------------------------------------------
// 求根（对应 RootFinding/Brent.cs）
// ---------------------------------------------------------------------------
struct Brent {
    /// 找不到根时抛 std::runtime_error("RootFindingFailed")（对应 C# InvalidOperationException）。
    static double FindRoot(const std::function<double(double)>& f, double lowerBound, double upperBound,
                           double accuracy = 1e-8, int maxIterations = 100);
    static bool TryFindRoot(const std::function<double(double)>& f, double lowerBound, double upperBound,
                            double accuracy, int maxIterations, double& root);
};

// ---------------------------------------------------------------------------
// 分布（对应 Distributions/*.cs）
// ---------------------------------------------------------------------------
namespace dist {

struct Normal {
    static double CDF(double mean, double stddev, double x);
    static double InvCDF(double mean, double stddev, double p);
    static double PDF(double mean, double stddev, double x);
};

struct StudentT {
    static double CDF(double location, double scale, int freedom, double x);
    static double InvCDF(double location, double scale, int freedom, double p);
};

struct FisherSnedecor {
    static double CDF(int d1, int d2, double x);
    static double InvCDF(int d1, int d2, double p);
};

/// 卡方分布（C# 裁剪版未提供，按伽玛分布 k/2, 1/2 实现）
struct ChiSquare {
    static double CDF(int freedom, double x);
    static double PDF(int freedom, double x);
};

struct Gamma {
    static double CDF(double shape, double rate, double x);
    static double PDF(double shape, double rate, double x);
    static double PDFLn(double shape, double rate, double x);
    static double InvCDF(double shape, double rate, double p);
};

struct Beta {
    static double CDF(double a, double b, double x);
    static double InvCDF(double a, double b, double p);
};

struct Binomial {
    static double PMF(double p, int n, int k);
    static double CDF(double p, int n, double x);
};

struct Exponential {
    static double CDF(double rate, double x);
    static double PDF(double rate, double x);
};

struct LogNormal {
    static double CDF(double mu, double sigma, double x);
    static double InvCDF(double mu, double sigma, double p);
};

struct NegativeBinomial {
    static double PMF(double r, double p, int k);
    static double PMFLn(double r, double p, int k);
};

struct Hypergeometric {
    static double PMF(int population, int success, int draws, int k);
};

struct Poisson {
    static double PMF(double lambda, int k);
    static double CDF(double lambda, double x);
};

struct Weibull {
    static double CDF(double shape, double scale, double x);
    static double PDF(double shape, double scale, double x);
};

}  // namespace dist

// ---------------------------------------------------------------------------
// 统计（对应 Statistics/*.cs 中被 ExcelFunctions 调用的部分 + 常用统计量）
// ---------------------------------------------------------------------------
namespace stats {

double Minimum(const std::vector<double>& data);
double Maximum(const std::vector<double>& data);
/// 分位数（对应 ArrayStatistics.QuantileCustomInplace，会打乱入参副本）
double QuantileCustom(std::vector<double> data, double tau);
/// 分位数排名（对应 SortedArrayStatistics.QuantileRank，要求 data 已升序）
double QuantileRank(const std::vector<double>& data, double x);

double Mean(const std::vector<double>& data);
/// 无偏样本方差（n-1 分母）
double Variance(const std::vector<double>& data);
/// 总体方差（n 分母）
double PopulationVariance(const std::vector<double>& data);
/// 样本协方差（n-1 分母）
double Covariance(const std::vector<double>& a, const std::vector<double>& b);
/// 皮尔逊相关系数
double Correlation(const std::vector<double>& a, const std::vector<double>& b);

}  // namespace stats

}  // namespace mn
}  // namespace toolgood
