#include "FunctionNames.h"

#include <unordered_map>

namespace toolgood {

namespace {

/// 别名 -> 规范 token 名（与 csharp/g4/math.g4 的词法别名一一对应）
const std::unordered_map<std::string, std::string>& AliasMap() {
    static const std::unordered_map<std::string, std::string> m = {
        // 带点号的统计函数
        {"BETA.DIST", "BETADIST"},
        {"BETA.INV", "BETAINV"},
        {"BINOM.DIST", "BINOMDIST"},
        {"COVARIANCE.P", "COVAR"},
        {"COVARIANCE.S", "COVARIANCES"},
        {"EXPON.DIST", "EXPONDIST"},
        {"F.DIST", "FDIST"},
        {"F.INV", "FINV"},
        {"GAMMA.DIST", "GAMMADIST"},
        {"GAMMA.INV", "GAMMAINV"},
        {"GAMMALN.PRECISE", "GAMMALN"},
        {"HYPGEOM.DIST", "HYPGEOMDIST"},
        {"LOGNORM.INV", "LOGINV"},
        {"LOGNORM.DIST", "LOGNORMDIST"},
        {"NEGBINOM.DIST", "NEGBINOMDIST"},
        {"NORM.DIST", "NORMDIST"},
        {"NORM.INV", "NORMINV"},
        {"NORM.S.DIST", "NORMSDIST"},
        {"NORM.S.INV", "NORMSINV"},
        {"PERCENTILE.INC", "PERCENTILE"},
        {"PERCENTRANK.INC", "PERCENTRANK"},
        {"POISSON.DIST", "POISSON"},
        {"STDEV.S", "STDEV"},
        {"STDEV.P", "STDEVP"},
        {"T.DIST", "TDIST"},
        {"T.INV", "TINV"},
        {"VAR.S", "VAR"},
        {"VAR.P", "VARP"},
        // 不带点号的别名
        {"CEIL", "CEILING"},
        {"CONCAT", "CONCATENATE"},
        {"HASKEY", "HAS"},
        {"CONTAINS", "HAS"},
        {"CONTAINSKEY", "HAS"},
        {"CONTAINSVALUE", "HASVALUE"},
        {"ISMATCH", "ISREGEX"},
        {"WIDECHAR", "JIS"},
        {"TOLOWER", "LOWER"},
        {"TOUPPER", "UPPER"},
        {"LTRIM", "TRIMSTART"},
        {"RTRIM", "TRIMEND"},
        {"GETPARAMETER", "PARAM"},
    };
    return m;
}

}  // namespace

bool IsDottedAlias(const std::string& upperName) {
    if (upperName.find('.') == std::string::npos) return false;
    return AliasMap().find(upperName) != AliasMap().end();
}

std::string CanonicalFunctionName(const std::string& upperName) {
    auto it = AliasMap().find(upperName);
    if (it == AliasMap().end()) return upperName;
    return it->second;
}

bool IsKnownFunctionName(const std::string& upperName) {
    return IsDottedAlias(upperName);
}

}  // namespace toolgood
