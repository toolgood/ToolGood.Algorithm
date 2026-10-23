#include "Registry.h"

#include "FunctionNames.h"

#include <mutex>
#include <unordered_map>

namespace toolgood {

namespace {
std::unordered_map<std::string, FunctionFactory>& Store() {
    static std::unordered_map<std::string, FunctionFactory> store;
    return store;
}
std::mutex& StoreMutex() {
    static std::mutex m;
    return m;
}
}  // namespace

void RegisterFunction(const std::string& key, FunctionFactory f) {
    std::lock_guard<std::mutex> lock(StoreMutex());
    auto& s = Store();
    if (s.find(key) == s.end()) s.emplace(key, std::move(f));
}

bool HasFunction(const std::string& key) {
    RegisterAllFunctions();
    const std::string name = CanonicalFunctionName(key);
    std::lock_guard<std::mutex> lock(StoreMutex());
    return Store().find(name) != Store().end();
}

Function GetFunction(const std::string& key, const std::vector<Function>& args) {
    RegisterAllFunctions();
    FunctionFactory f;
    const std::string name = CanonicalFunctionName(key);
    {
        std::lock_guard<std::mutex> lock(StoreMutex());
        auto it = Store().find(name);
        if (it == Store().end()) return nullptr;
        f = it->second;
    }
    return f(args);
}

}  // namespace toolgood
