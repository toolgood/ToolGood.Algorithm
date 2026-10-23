#include "AlgorithmEngine.h"

#include <stdexcept>

#include "Internals/Registry.h"
#include "Parser.h"

namespace toolgood {

namespace {
std::string Trim(const std::string& s) {
    size_t b = 0, e = s.size();
    while (b < e && (s[b] == ' ' || s[b] == '\t' || s[b] == '\r' || s[b] == '\n' || s[b] == '\f')) b++;
    while (e > b && (s[e - 1] == ' ' || s[e - 1] == '\t' || s[e - 1] == '\r' || s[e - 1] == '\n' ||
                     s[e - 1] == '\f'))
        e--;
    return s.substr(b, e - b);
}
}  // namespace

Operand AlgorithmEngine::GetParameter(const std::string& parameter) {
    return Operand::Error("Parameter [" + parameter + "] is missing.");
}

Operand AlgorithmEngine::ExecuteDiyFunction(const std::string& parameter,
                                            const std::vector<Operand>& args) {
    (void)args;
    return Operand::Error("DiyFunction [" + parameter + "] is missing.");
}

Operand AlgorithmEngine::GetParameterValue(AlgorithmEngine& engine, const TempParameter* tp,
                                           const std::string& name) {
    if (tp != nullptr && *tp) return (*tp)(engine, name);
    return engine.GetParameter(name);
}

Function AlgorithmEngine::Parse(const std::string& exp) {
    LastError.clear();
    if (Trim(exp).empty()) {
        LastError = "Parameter exp invalid !";
        throw std::runtime_error(LastError);
    }
    std::string e = Trim(exp);
    if (UseParseCache) {
        auto it = parseCache_.find(e);
        if (it != parseCache_.end()) return it->second;
        Function r = ParseInternal(e, true);
        parseCache_[e] = r;
        return r;
    }
    return ParseInternal(e, true);
}

Function AlgorithmEngine::ParseWithoutError(const std::string& exp) {
    LastError.clear();
    if (Trim(exp).empty()) {
        LastError = "Parameter exp invalid !";
        return nullptr;
    }
    std::string e = Trim(exp);
    if (UseParseCache) {
        auto it = parseCache_.find(e);
        if (it != parseCache_.end()) return it->second;
        Function r = ParseInternal(e, false);
        if (r) parseCache_[e] = r;
        return r;
    }
    return ParseInternal(e, false);
}

Function AlgorithmEngine::ParseInternal(const std::string& exp, bool throwOnError) {
    try {
        return Parser::ParseFunction(exp);
    } catch (const std::exception& ex) {
        LastError = ex.what();
        if (throwOnError) throw;
        return nullptr;
    } catch (...) {
        LastError = "unknown parse error";
        if (throwOnError) throw;
        return nullptr;
    }
}

Operand AlgorithmEngine::Evaluate(const Function& function) {
    if (!function) return Operand::Error("Function is null.");
    try {
        return function->Evaluate(*this);
    } catch (const std::exception& ex) {
        LastError = ex.what();
        return Operand::Error(ex.what());
    }
}

Operand AlgorithmEngine::Calculate(const std::string& exp) {
    return Evaluate(Parse(exp));
}

int AlgorithmEngine::TryEvaluate(const std::string& exp, int def) {
    Function f = ParseWithoutError(exp);
    if (!f) return def;
    Operand o = Evaluate(f);
    if (!o.IsNumber()) o = o.ToNumber("It can't be converted to number!");
    if (o.IsError()) {
        LastError = o.ErrorMsg();
        return def;
    }
    return o.IntValue();
}

int64_t AlgorithmEngine::TryEvaluate(const std::string& exp, int64_t def) {
    Function f = ParseWithoutError(exp);
    if (!f) return def;
    Operand o = Evaluate(f);
    if (!o.IsNumber()) o = o.ToNumber("It can't be converted to number!");
    if (o.IsError()) {
        LastError = o.ErrorMsg();
        return def;
    }
    return o.LongValue();
}

double AlgorithmEngine::TryEvaluate(const std::string& exp, double def) {
    Function f = ParseWithoutError(exp);
    if (!f) return def;
    Operand o = Evaluate(f);
    if (!o.IsNumber()) o = o.ToNumber("It can't be converted to number!");
    if (o.IsError()) {
        LastError = o.ErrorMsg();
        return def;
    }
    return o.DoubleValue();
}

Decimal AlgorithmEngine::TryEvaluate(const std::string& exp, const Decimal& def) {
    Function f = ParseWithoutError(exp);
    if (!f) return def;
    Operand o = Evaluate(f);
    if (!o.IsNumber()) o = o.ToNumber("It can't be converted to number!");
    if (o.IsError()) {
        LastError = o.ErrorMsg();
        return def;
    }
    return o.NumberValue();
}

std::string AlgorithmEngine::TryEvaluate(const std::string& exp, const std::string& def) {
    Function f = ParseWithoutError(exp);
    if (!f) return def;
    Operand o = Evaluate(f);
    if (!o.IsText()) o = o.ToText("It can't be converted to string!");
    if (o.IsError()) {
        LastError = o.ErrorMsg();
        return def;
    }
    return o.TextValue();
}

std::string AlgorithmEngine::TryEvaluate(const std::string& exp, const char* def) {
    return TryEvaluate(exp, def != nullptr ? std::string(def) : std::string());
}

bool AlgorithmEngine::TryEvaluate(const std::string& exp, bool def) {
    Function f = ParseWithoutError(exp);
    if (!f) return def;
    Operand o = Evaluate(f);
    if (!o.IsBoolean()) o = o.ToBoolean("It can't be converted to bool!");
    if (o.IsError()) {
        LastError = o.ErrorMsg();
        return def;
    }
    return o.BooleanValue();
}

MyDate AlgorithmEngine::TryEvaluate(const std::string& exp, const MyDate& def) {
    Function f = ParseWithoutError(exp);
    if (!f) return def;
    Operand o = Evaluate(f);
    if (!o.IsDate()) o = o.ToMyDate("It can't be converted to DateTime!");
    if (o.IsError()) {
        LastError = o.ErrorMsg();
        return def;
    }
    return o.DateValue();
}

}  // namespace toolgood
