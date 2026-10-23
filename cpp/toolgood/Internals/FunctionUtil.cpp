#include "FunctionUtil.h"

#include <algorithm>
#include <cctype>
#include <deque>
#include <map>
#include <mutex>

namespace toolgood {

// ---------------- RegexHelper ----------------

bool RegexIsHex(const std::string& value) {
    if (value.empty()) return false;
    for (char c : value) {
        bool ok = (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f');
        if (!ok) return false;
    }
    return true;
}

bool RegexIsOct(const std::string& value) {
    if (value.empty()) return false;
    for (char c : value) {
        if (c < '0' || c > '7') return false;
    }
    return true;
}

bool RegexIsBin(const std::string& value) {
    if (value.empty()) return false;
    for (char c : value) {
        if (c != '0' && c != '1') return false;
    }
    return true;
}

namespace {
std::mutex g_regexMutex;
std::map<std::string, std::regex> g_regexCache;
}  // namespace

const std::regex& GetRegex(const std::string& pattern) {
    std::lock_guard<std::mutex> lock(g_regexMutex);
    auto it = g_regexCache.find(pattern);
    if (it != g_regexCache.end()) return it->second;
    if (g_regexCache.size() >= 128) g_regexCache.clear();
    auto res = g_regexCache.emplace(pattern, std::regex(pattern, std::regex::ECMAScript));
    return res.first->second;
}

// ---------------- 值比较 ----------------

bool JsonValueEquals(const JsonData* v, const std::string& text) {
    if (v == nullptr) return false;
    if (v->IsString()) return v->StringValue() == text;
    if (v->IsDouble()) return v->NumberValue().ToString() == text;
    if (v->IsBoolean()) {
        std::string b = v->BooleanValue() ? "true" : "false";
        std::string t = text;
        std::transform(t.begin(), t.end(), t.begin(),
                       [](unsigned char c) { return static_cast<char>(std::tolower(c)); });
        return b == t;
    }
    return false;
}

bool OperandValueEquals(const Operand& op, const std::string& text) {
    if (op.IsText()) return op.TextValue() == text;
    if (op.IsNumber()) return op.NumberValue().ToString() == text;
    if (op.IsBoolean()) {
        std::string b = op.BooleanValue() ? "true" : "false";
        std::string t = text;
        std::transform(t.begin(), t.end(), t.begin(),
                       [](unsigned char c) { return static_cast<char>(std::tolower(c)); });
        return b == t;
    }
    return false;
}

bool TryParseBoolean(const std::string& text, bool& boolValue) {
    const size_t len = text.size();
    if (len == 1) {
        char c = text[0];
        if (c == '1') {
            boolValue = true;
            return true;
        }
        if (c == '0') {
            boolValue = false;
            return true;
        }
        // 中文：是 / 有 / 否 / 无（UTF-8 各 3 字节，长度 3）
        return false;
    }
    std::string lower = text;
    std::transform(lower.begin(), lower.end(), lower.begin(),
                   [](unsigned char c) { return static_cast<char>(std::tolower(c)); });
    if (lower == "no") {
        boolValue = false;
        return true;
    }
    if (text == "不是" || text == "没有") {
        boolValue = false;
        return true;
    }
    if (lower == "yes") {
        boolValue = true;
        return true;
    }
    if (lower == "true") {
        boolValue = true;
        return true;
    }
    if (lower == "false") {
        boolValue = false;
        return true;
    }
    // 单字符中文（3 字节）
    if (text == "是" || text == "有") {
        boolValue = true;
        return true;
    }
    if (text == "否" || text == "无") {
        boolValue = false;
        return true;
    }
    boolValue = false;
    return false;
}

// ---------------- 展开 ----------------

int EstimateCount(const std::vector<Operand>& args) {
    int count = 0;
    for (const auto& item : args) {
        if (item.IsArray()) count += static_cast<int>(item.ArrayValue().size());
        else if (item.IsJson()) {
            const JsonData* j = item.JsonValue();
            count += j ? j->Count() : 0;
        } else {
            count++;
        }
    }
    return count;
}

bool FlattenToList(const std::vector<Operand>& args, std::vector<Operand>& list) {
    std::deque<Operand> queue(args.begin(), args.end());
    while (!queue.empty()) {
        Operand item = queue.front();
        queue.pop_front();
        if (item.IsArray()) {
            for (const auto& v : item.ArrayValue()) queue.push_back(v);
        } else if (item.IsJson()) {
            Operand arr = item.ToArray();
            if (arr.IsError()) return false;
            for (const auto& v : arr.ArrayValue()) queue.push_back(v);
        } else {
            list.push_back(item);
        }
    }
    return true;
}

bool FlattenToNumberList(const std::vector<Operand>& args, std::vector<Decimal>& list) {
    std::deque<Operand> queue(args.begin(), args.end());
    while (!queue.empty()) {
        Operand item = queue.front();
        queue.pop_front();
        if (item.IsArray()) {
            for (const auto& v : item.ArrayValue()) queue.push_back(v);
        } else if (item.IsJson()) {
            Operand arr = item.ToArray();
            if (arr.IsError()) return false;
            for (const auto& v : arr.ArrayValue()) queue.push_back(v);
        } else {
            if (item.IsNumber()) {
                list.push_back(item.NumberValue());
            } else {
                Operand converted = item.ToNumber();
                if (converted.IsError()) return false;
                list.push_back(converted.NumberValue());
            }
        }
    }
    return true;
}

bool FlattenToNumberList(const Operand& arg, std::vector<Decimal>& list) {
    if (arg.IsError()) return false;
    if (arg.IsArray()) {
        std::vector<Operand> arr = arg.ArrayValue();
        return FlattenToNumberList(arr, list);
    }
    if (arg.IsJson()) {
        Operand arr = arg.ToArray();
        if (arr.IsError()) return false;
        std::vector<Operand> v = arr.ArrayValue();
        return FlattenToNumberList(v, list);
    }
    if (arg.IsNumber()) {
        list.push_back(arg.NumberValue());
    } else {
        Operand converted = arg.ToNumber();
        if (converted.IsError()) return false;
        list.push_back(converted.NumberValue());
    }
    return true;
}

bool FlattenToStringList(const Operand& arg, std::vector<std::string>& list) {
    if (arg.IsError()) return false;
    if (arg.IsArray()) {
        for (const auto& item : arg.ArrayValue()) {
            if (item.IsArray() || item.IsJson()) {
                if (!FlattenToStringList(item, list)) return false;
            } else {
                Operand converted = item.ToText();
                if (converted.IsError()) return false;
                list.push_back(converted.TextValue());
            }
        }
        return true;
    }
    if (arg.IsJson()) {
        Operand arr = arg.ToArray();
        if (arr.IsError()) return false;
        return FlattenToStringList(arr, list);
    }
    Operand converted = arg.ToText();
    if (converted.IsError()) return false;
    list.push_back(converted.TextValue());
    return true;
}

// ---------------- 条件统计 ----------------

int GetCountIf(const std::vector<Decimal>& dbs, const Decimal& d) {
    int count = 0;
    for (const auto& item : dbs) {
        if (item.Equal(d)) count++;
    }
    return count;
}

int GetCountIf(const std::vector<Decimal>& dbs, const std::string& op, const Decimal& d) {
    int count = 0;
    for (const auto& item : dbs) {
        if (CompareValues(item, d, op)) count++;
    }
    return count;
}

Decimal GetSumIf(const std::vector<Decimal>& dbs, const Decimal& d,
                 const std::vector<Decimal>& sumDbs) {
    Decimal sum;
    size_t n = std::min(dbs.size(), sumDbs.size());
    for (size_t i = 0; i < n; i++) {
        if (dbs[i].Equal(d)) sum = sum.Add(sumDbs[i]);
    }
    return sum;
}

Decimal GetSumIf(const std::vector<Decimal>& dbs, const std::string& op, const Decimal& d,
                 const std::vector<Decimal>& sumDbs) {
    Decimal sum;
    size_t n = std::min(dbs.size(), sumDbs.size());
    for (size_t i = 0; i < n; i++) {
        if (CompareValues(dbs[i], d, op)) sum = sum.Add(sumDbs[i]);
    }
    return sum;
}

bool CompareValues(const Decimal& a, const Decimal& b, const std::string& ss) {
    if (ss == "<") return a.Less(b);
    if (ss == "<=") return a.LessOrEqual(b);
    if (ss == ">") return a.Greater(b);
    if (ss == ">=") return a.GreaterOrEqual(b);
    if (ss == "=" || ss == "==" || ss == "===") return a.Equal(b);
    return !a.Equal(b);
}

// ---------------- 数论 ----------------

Decimal GetGCD(const std::vector<Decimal>& list) {
    if (list.empty()) return Decimal::One;
    Decimal g = list[0].Trunc();
    for (size_t i = 1; i < list.size(); i++) {
        g = GetGCD(g, list[i].Trunc());
        if (g.Equal(Decimal::One)) break;
    }
    return g;
}

Decimal GetGCD(const Decimal& a, const Decimal& b) {
    Decimal x = a;
    Decimal y = b;
    while (!y.IsZero()) {
        Decimal t = y;
        y = x.Mod(y);
        x = t;
    }
    return x;
}

Decimal GetLCM(const std::vector<Decimal>& list) {
    if (list.empty()) return Decimal::One;
    Decimal a = list[0].Trunc();
    for (size_t i = 1; i < list.size(); i++) {
        Decimal b = list[i].Trunc();
        if (a.IsZero() || b.IsZero()) {
            a = Decimal::Zero;
            continue;
        }
        Decimal g = b.Greater(a) ? GetGCD(b, a) : GetGCD(a, b);
        a = a.Div(g).Mul(b);
    }
    return a;
}

Decimal GetFactorial(const Decimal& a) {
    if (a.LessOrEqual(Decimal::Zero)) return Decimal::One;
    Decimal n = a.Trunc();
    Decimal r = Decimal::One;
    for (Decimal i = n; i.Greater(Decimal::Zero); i = i.Sub(Decimal::One)) {
        r = r.Mul(i);
    }
    return r;
}

bool ParseSumIfMatch(const std::string& s, std::string& op, Decimal& value) {
    if (s.empty()) return false;
    // 全角符号归一
    auto norm = [](const std::string& in) {
        std::string out = in;
        // '＝' '＞' '＜' '！' 为 3 字节 UTF-8
        const std::pair<const char*, const char*> map[] = {
            {"＝", "="}, {"＞", ">"}, {"＜", "<"}, {"！", "!"}};
        for (const auto& m : map) {
            size_t pos;
            while ((pos = out.find(m.first)) != std::string::npos) {
                out.replace(pos, std::string(m.first).size(), m.second);
            }
        }
        return out;
    };
    std::string t = norm(s);
    auto tryParse = [&](size_t from, const char* opStr) {
        Decimal d;
        if (Decimal::TryParse(t.substr(from), d)) {
            op = opStr;
            value = d;
            return true;
        }
        return false;
    };
    char c = t[0];
    if (c == '>') {
        if (t.size() > 1 && t[1] == '=') return tryParse(2, ">=");
        return tryParse(1, ">");
    }
    if (c == '<') {
        if (t.size() > 1 && t[1] == '=') return tryParse(2, "<=");
        if (t.size() > 1 && t[1] == '>') return tryParse(2, "!=");
        return tryParse(1, "<");
    }
    if (c == '=') {
        size_t idx = 1;
        if (t.size() > 1 && t[1] == '=') {
            idx = 2;
            if (t.size() > 2 && t[2] == '=') idx = 3;
        }
        return tryParse(idx, "=");
    }
    if (c == '!') {
        size_t idx = 1;
        if (t.size() > 1 && t[1] == '=') {
            idx = 2;
            if (t.size() > 2 && t[2] == '=') idx = 3;
        }
        return tryParse(idx, "!=");
    }
    return false;
}

Decimal QuickSelect(std::vector<Decimal> list, int k, bool largest) {
    if (list.size() == 1) return list[0];
    int targetIndex = largest ? static_cast<int>(list.size()) - 1 - k : k;
    int left = 0, right = static_cast<int>(list.size()) - 1;
    while (left < right) {
        int mid = left + (right - left) / 2;
        Decimal a = list[left], b = list[mid], c = list[right];
        int pivotIndex;
        if (a.Less(b)) {
            if (b.Less(c)) pivotIndex = mid;
            else if (a.Less(c)) pivotIndex = right;
            else pivotIndex = left;
        } else {
            if (a.Less(c)) pivotIndex = left;
            else if (b.Less(c)) pivotIndex = right;
            else pivotIndex = mid;
        }
        // partition
        Decimal pivot = list[pivotIndex];
        std::swap(list[pivotIndex], list[right]);
        int storeIndex = left;
        for (int i = left; i < right; i++) {
            if (list[i].Less(pivot)) {
                std::swap(list[storeIndex], list[i]);
                storeIndex++;
            }
        }
        std::swap(list[storeIndex], list[right]);
        pivotIndex = storeIndex;
        if (targetIndex == pivotIndex) return list[targetIndex];
        if (targetIndex < pivotIndex) right = pivotIndex - 1;
        else left = pivotIndex + 1;
    }
    return list[left];
}

int GetRank(const std::vector<Decimal>& values, const Decimal& num, bool descending) {
    int rank = 1;
    int count = 0;
    for (const auto& v : values) {
        if (v.Equal(num)) {
            count++;
        } else if ((descending && v.Greater(num)) || (!descending && v.Less(num))) {
            rank++;
        }
    }
    return count > 0 ? rank : 0;
}

}  // namespace toolgood
