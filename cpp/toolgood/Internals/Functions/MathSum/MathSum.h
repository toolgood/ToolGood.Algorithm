// MathSum.h - 对应 csharp/.../Internals/Functions/MathSum/
#pragma once

#include <vector>

#include "../../FunctionBase.h"

namespace toolgood {

Function NewFunction_AVEDEV(std::vector<Function> args);
Function NewFunction_AVERAGE(std::vector<Function> args);
Function NewFunction_AVERAGEIF(std::vector<Function> args);
Function NewFunction_CORREL(std::vector<Function> args);
Function NewFunction_COUNT(std::vector<Function> args);
Function NewFunction_COUNTIF(std::vector<Function> args);
Function NewFunction_COVAR(std::vector<Function> args);
Function NewFunction_COVARIANCES(std::vector<Function> args);
Function NewFunction_DEVSQ(std::vector<Function> args);
Function NewFunction_FORECAST(std::vector<Function> args);
Function NewFunction_GEOMEAN(std::vector<Function> args);
Function NewFunction_HARMEAN(std::vector<Function> args);
Function NewFunction_INTERCEPT(std::vector<Function> args);
Function NewFunction_LARGE(std::vector<Function> args);
Function NewFunction_MAX(std::vector<Function> args);
Function NewFunction_MEDIAN(std::vector<Function> args);
Function NewFunction_MIN(std::vector<Function> args);
Function NewFunction_MODE(std::vector<Function> args);
Function NewFunction_PEARSON(std::vector<Function> args);
Function NewFunction_PERCENTILE(std::vector<Function> args);
Function NewFunction_PERCENTRANK(std::vector<Function> args);
Function NewFunction_POISSON(std::vector<Function> args);
Function NewFunction_QUARTILE(std::vector<Function> args);
Function NewFunction_RANK(std::vector<Function> args);
Function NewFunction_SERIESSUM(std::vector<Function> args);
Function NewFunction_SLOPE(std::vector<Function> args);
Function NewFunction_SMALL(std::vector<Function> args);
Function NewFunction_STDEV(std::vector<Function> args);
Function NewFunction_STDEVP(std::vector<Function> args);
Function NewFunction_SUM(std::vector<Function> args);
Function NewFunction_SUMIF(std::vector<Function> args);
Function NewFunction_SUMPRODUCT(std::vector<Function> args);
Function NewFunction_SUMSQ(std::vector<Function> args);
Function NewFunction_SUMX2MY2(std::vector<Function> args);
Function NewFunction_SUMX2PY2(std::vector<Function> args);
Function NewFunction_SUMXMY2(std::vector<Function> args);
Function NewFunction_VAR(std::vector<Function> args);
Function NewFunction_VARP(std::vector<Function> args);

/// 注册 MathSum 组全部函数。
void Registermathsum();

}  // namespace toolgood
