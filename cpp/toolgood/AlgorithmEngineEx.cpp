// AlgorithmEngineEx.cpp - 对应 csharp/ToolGood.Algorithm/AlgorithmEngineEx.cs
#include "AlgorithmEngineEx.h"

#include <stdexcept>
#include <utility>

#include "LitJson/JsonData.h"

namespace toolgood {

bool ParameterKeyLess::operator()(const std::string& a, const std::string& b) const {
    if (IgnoreCase) {
        const size_t n = a.size() < b.size() ? a.size() : b.size();
        for (size_t i = 0; i < n; i++) {
            unsigned char ca = static_cast<unsigned char>(a[i]);
            unsigned char cb = static_cast<unsigned char>(b[i]);
            if (ca >= 'A' && ca <= 'Z') ca = static_cast<unsigned char>(ca + 32);
            if (cb >= 'A' && cb <= 'Z') cb = static_cast<unsigned char>(cb + 32);
            if (ca != cb) return ca < cb;
        }
        return a.size() < b.size();
    }
    return a < b;
}

AlgorithmEngineEx::AlgorithmEngineEx() : tempdict_(ParameterKeyLess{false}) {}

AlgorithmEngineEx::AlgorithmEngineEx(bool ignoreCase)
    : tempdict_(ParameterKeyLess{ignoreCase}), ignoreCase_(ignoreCase) {}

Operand AlgorithmEngineEx::GetParameter(const std::string& parameter) {
    auto it = tempdict_.find(parameter);
    if (it != tempdict_.end()) return it->second;
    Operand result = GetParameterEx(parameter);
    if (UseTempDict) {
        tempdict_.insert(std::make_pair(parameter, result));
    }
    return result;
}

Operand AlgorithmEngineEx::GetParameterEx(const std::string& parameter) {
    return Operand::Error("Parameter [" + parameter + "] is missing.");
}

void AlgorithmEngineEx::ClearParameters() {
    tempdict_.clear();
}

void AlgorithmEngineEx::AddParameter(const std::string& key, const Operand& obj) {
    tempdict_[key] = obj;
}

void AlgorithmEngineEx::AddParameter(const std::string& key, bool obj) {
    tempdict_[key] = Operand::Create(obj);
}

void AlgorithmEngineEx::AddParameter(const std::string& key, int obj) {
    tempdict_[key] = Operand::Create(obj);
}

void AlgorithmEngineEx::AddParameter(const std::string& key, int64_t obj) {
    tempdict_[key] = Operand::Create(obj);
}

void AlgorithmEngineEx::AddParameter(const std::string& key, double obj) {
    tempdict_[key] = Operand::Create(obj);
}

void AlgorithmEngineEx::AddParameter(const std::string& key, const Decimal& obj) {
    tempdict_[key] = Operand::Create(obj);
}

void AlgorithmEngineEx::AddParameter(const std::string& key, const std::string& obj) {
    tempdict_[key] = Operand::Create(obj);
}

void AlgorithmEngineEx::AddParameter(const std::string& key, const char* obj) {
    tempdict_[key] = Operand::Create(std::string(obj == nullptr ? "" : obj));
}

void AlgorithmEngineEx::AddParameter(const std::string& key, const MyDate& obj) {
    tempdict_[key] = Operand::Create(obj);
}

void AlgorithmEngineEx::AddParameter(const std::string& key, const std::vector<Operand>& obj) {
    tempdict_[key] = Operand::Create(obj);
}

void AlgorithmEngineEx::AddParameter(const std::string& key, const std::vector<std::string>& obj) {
    std::vector<Operand> list;
    list.reserve(obj.size());
    for (const auto& item : obj) list.push_back(Operand::Create(item));
    tempdict_[key] = Operand::Create(std::move(list));
}

void AlgorithmEngineEx::AddParameter(const std::string& key, const std::vector<double>& obj) {
    std::vector<Operand> list;
    list.reserve(obj.size());
    for (double item : obj) list.push_back(Operand::Create(item));
    tempdict_[key] = Operand::Create(std::move(list));
}

void AlgorithmEngineEx::AddParameter(const std::string& key, const std::vector<int>& obj) {
    std::vector<Operand> list;
    list.reserve(obj.size());
    for (int item : obj) list.push_back(Operand::Create(item));
    tempdict_[key] = Operand::Create(std::move(list));
}

void AlgorithmEngineEx::AddParameter(const std::string& key, const std::vector<bool>& obj) {
    std::vector<Operand> list;
    list.reserve(obj.size());
    for (bool item : obj) list.push_back(Operand::Create(item));
    tempdict_[key] = Operand::Create(std::move(list));
}

void AlgorithmEngineEx::AddParameterFromJson(const std::string& json) {
    const bool looksLikeObject = json.size() >= 2 && json.front() == '{' && json.back() == '}';
    if (looksLikeObject) {
        JsonData jo;
        if (JsonMapper::TryParse(json, jo) && jo.IsObject()) {
            for (const std::string& key : jo.Keys()) {
                const JsonData* v = jo.GetChild(key);
                if (v == nullptr) continue;
                if (v->IsString()) {
                    tempdict_[key] = Operand::Create(v->StringValue());
                } else if (v->IsBoolean()) {
                    tempdict_[key] = Operand::Create(v->BooleanValue());
                } else if (v->IsDouble()) {
                    tempdict_[key] = Operand::Create(v->NumberValue());
                } else if (v->IsObject()) {
                    tempdict_[key] = Operand::Create(*v);
                } else if (v->IsArray()) {
                    tempdict_[key] = Operand::Create(*v);
                } else if (v->IsNull()) {
                    tempdict_[key] = Operand::Null();
                } else {
                    tempdict_[key] = Operand::Create(*v);
                }
            }
            return;
        }
    }
    throw std::invalid_argument("Parameter is not json string.");
}

}  // namespace toolgood
