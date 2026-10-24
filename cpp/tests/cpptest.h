// temp/cpp-test/cpptest.h - C++ 版 PetaTest（用于移植 csharp/ToolGood.Algorithm.Test）
//
// 断言语义与 C# PetaTest 对齐：
//   Assert::AreEqual(a, b)         相等（double 为精确比较，对应 object.Equals）
//   Assert::AreEqual(a, b, within) 浮点容差比较
//   Assert::IsTrue / IsFalse / IsNull / IsNotNull / IsEmpty / IsNotEmpty
//   Assert::Throws([]{...})        期望抛异常
//   Assert::GreaterOrEqual / LessOrEqual / Less / Greater
//
// 用法（建议放在匿名命名空间内，避免多个测试文件重名）：
//   namespace {
//   TEST(MathBase, abs_test) {
//       AlgorithmEngine engine;
//       Assert::AreEqual(1.2, engine.TryEvaluate("abs(-1.2)", 0.0));
//   }
//   }
#pragma once

#include <cmath>
#include <cstdio>
#include <cstring>
#include <exception>
#include <string>
#include <vector>

#include "toolgood/Decimal.h"
#include "toolgood/MyDate.h"
#include "toolgood/Operands/Operand.h"

namespace cpptest {

struct TestCase {
    std::string suite;
    std::string name;
    void (*fn)();
};

/// 调用点信息：作为默认参数注入，避免与 C# 的 within 参数（可能是 int）冲突
struct Src {
    int line;
    const char* file;
};

struct Failure {
    std::string where;
    std::string message;
};

inline std::vector<TestCase>& Cases() {
    static std::vector<TestCase> v;
    return v;
}

inline std::vector<Failure>& CurrentFailures() {
    static std::vector<Failure> v;
    return v;
}

/// 注册测试用例。
inline bool Register(const char* suite, const char* name, void (*fn)()) {
    Cases().push_back(TestCase{suite, name, fn});
    return true;
}

/// 记录断言失败。
inline void Fail(const char* file, int line, const std::string& message) {
    const char* base = std::strrchr(file, '/');
    if (base == nullptr) base = std::strrchr(file, '\\');
    std::string loc = (base != nullptr ? base + 1 : file);
    loc += ":" + std::to_string(line);
    CurrentFailures().push_back(Failure{loc, message});
}

// ---------------- 值显示（对应 PetaTest.FormatValue）----------------

inline std::string Show(bool v) { return v ? "true" : "false"; }
inline std::string Show(int v) { return std::to_string(v); }
inline std::string Show(long v) { return std::to_string(v); }
inline std::string Show(long long v) { return std::to_string(v); }
inline std::string Show(unsigned v) { return std::to_string(v); }
inline std::string Show(size_t v) { return std::to_string(v); }
inline std::string Show(double v) {
    char buf[64];
    std::snprintf(buf, sizeof(buf), "%.17g", v);
    return buf;
}
inline std::string Show(float v) { return Show(static_cast<double>(v)); }
inline std::string Show(const std::string& v) { return "\"" + v + "\""; }
inline std::string Show(const char* v) { return v != nullptr ? "\"" + std::string(v) + "\"" : "<null>"; }
inline std::string Show(const toolgood::Decimal& v) { return v.ToString(); }
inline std::string Show(const toolgood::MyDate& v) { return v.ToString(); }

inline std::string Show(const toolgood::Operand& v) {
    if (v.IsError()) return "<error: " + v.ErrorMsg() + ">";
    if (v.IsNull()) return "<null>";
    if (v.IsNone()) return "<none>";
    if (v.IsNumber()) return v.NumberValue().ToString();
    if (v.IsText()) return "\"" + v.TextValue() + "\"";
    if (v.IsBoolean()) return v.BooleanValue() ? "true" : "false";
    if (v.IsDate()) return v.DateValue().ToString();
    return v.ToString();
}

template <class T>
std::string Show(const std::vector<T>& v) {
    std::string s = "[";
    for (size_t i = 0; i < v.size(); i++) {
        if (i > 0) s += ", ";
        s += Show(v[i]);
    }
    return s + "]";
}

// ---------------- 相等判定 ----------------

namespace detail {

/// 通用相等（要求类型可 ==）
template <class A, class B>
inline bool Equals(const A& a, const B& b) {
    return a == b;
}

/// Decimal 没有 operator==，用数值比较（对应 C# decimal ==）
inline bool Equals(const toolgood::Decimal& a, const toolgood::Decimal& b) { return a.Equal(b); }

/// 日期按 C# 的 DateTime == 语义（精确到秒）
inline bool Equals(const toolgood::MyDate& a, const toolgood::MyDate& b) {
    return a.ToLong() == b.ToLong();
}

}  // namespace detail

/// 断言集合。
class Assert {
public:
    // ---- AreEqual ----
    template <class A, class B>
    static void AreEqual(const A& a, const B& b, Src src = Src{__builtin_LINE(), __builtin_FILE()}) {
        if (!detail::Equals(a, b)) {
            Fail(src.file, src.line, "Objects are not equal\n  lhs: " + Show(a) + "\n  rhs: " + Show(b));
        }
    }

    static void AreEqual(double a, double b, double within, Src src = Src{__builtin_LINE(), __builtin_FILE()}) {
        if (!(std::fabs(a - b) < within)) {
            Fail(src.file, src.line, "Objects are not equal (within " + Show(within) +
                                 ")\n  lhs: " + Show(a) + "\n  rhs: " + Show(b));
        }
    }

    template <class A, class B>
    static void AreNotEqual(const A& a, const B& b, Src src = Src{__builtin_LINE(), __builtin_FILE()}) {
        if (detail::Equals(a, b)) {
            Fail(src.file, src.line, "Objects are equal\n  lhs: " + Show(a) + "\n  rhs: " + Show(b));
        }
    }

    // ---- 布尔 ----
    static void IsTrue(bool test, Src src = Src{__builtin_LINE(), __builtin_FILE()}) {
        if (!test) Fail(src.file, src.line, "Expression is not true");
    }
    static void IsFalse(bool test, Src src = Src{__builtin_LINE(), __builtin_FILE()}) {
        if (test) Fail(src.file, src.line, "Expression is not false");
    }

    // ---- null / 空 ----
    template <class T>
    static void IsNull(T* v, Src src = Src{__builtin_LINE(), __builtin_FILE()}) {
        if (v != nullptr) Fail(src.file, src.line, "Object is not null");
    }
    template <class T>
    static void IsNotNull(T* v, Src src = Src{__builtin_LINE(), __builtin_FILE()}) {
        if (v == nullptr) Fail(src.file, src.line, "Object is null");
    }
    /// C# 的 string null 在移植版中用空串表示（如 AlgorithmEngine.LastError）
    static void IsNull(const std::string& v, Src src = Src{__builtin_LINE(), __builtin_FILE()}) {
        if (!v.empty()) Fail(src.file, src.line, "String is not null: " + Show(v));
    }
    static void IsNotNull(const std::string& v, Src src = Src{__builtin_LINE(), __builtin_FILE()}) {
        if (v.empty()) Fail(src.file, src.line, "String is null");
    }
    static void IsEmpty(const std::string& v, Src src = Src{__builtin_LINE(), __builtin_FILE()}) {
        if (!v.empty()) Fail(src.file, src.line, "String is not empty: " + Show(v));
    }
    static void IsNotEmpty(const std::string& v, Src src = Src{__builtin_LINE(), __builtin_FILE()}) {
        if (v.empty()) Fail(src.file, src.line, "String is empty");
    }

    // ---- 比较 ----
    template <class A, class B>
    static void GreaterOrEqual(const A& a, const B& b, Src src = Src{__builtin_LINE(), __builtin_FILE()}) {
        if (!(a >= b)) Fail(src.file, src.line, "lhs < rhs\n  lhs: " + Show(a) + "\n  rhs: " + Show(b));
    }
    template <class A, class B>
    static void LessOrEqual(const A& a, const B& b, Src src = Src{__builtin_LINE(), __builtin_FILE()}) {
        if (!(a <= b)) Fail(src.file, src.line, "lhs > rhs\n  lhs: " + Show(a) + "\n  rhs: " + Show(b));
    }
    template <class A, class B>
    static void Less(const A& a, const B& b, Src src = Src{__builtin_LINE(), __builtin_FILE()}) {
        if (!(a < b)) Fail(src.file, src.line, "lhs >= rhs\n  lhs: " + Show(a) + "\n  rhs: " + Show(b));
    }
    template <class A, class B>
    static void Greater(const A& a, const B& b, Src src = Src{__builtin_LINE(), __builtin_FILE()}) {
        if (!(a > b)) Fail(src.file, src.line, "lhs <= rhs\n  lhs: " + Show(a) + "\n  rhs: " + Show(b));
    }

    // ---- 异常 ----
    /// 对应 Assert.Throws<T>(() => ...)
    template <class F>
    static void Throws(F f, Src src = Src{__builtin_LINE(), __builtin_FILE()}) {
        bool thrown = false;
        try {
            f();
        } catch (...) {
            thrown = true;
        }
        if (!thrown) Fail(src.file, src.line, "Expected exception but none was thrown");
    }
};

/// 运行全部用例，返回失败用例数（0 表示全通过）。
inline int RunAll(const char* suiteFilter = nullptr, const char* nameFilter = nullptr) {
    int total = 0, failedTests = 0;
    for (const auto& tc : Cases()) {
        if (suiteFilter != nullptr && tc.suite != suiteFilter) continue;
        if (nameFilter != nullptr && tc.name.find(nameFilter) == std::string::npos) continue;
        total++;
        CurrentFailures().clear();
        try {
            tc.fn();
        } catch (const std::exception& ex) {
            CurrentFailures().push_back(Failure{"", std::string("Unhandled exception: ") + ex.what()});
        } catch (...) {
            CurrentFailures().push_back(Failure{"", "Unhandled unknown exception"});
        }
        if (!CurrentFailures().empty()) {
            failedTests++;
            std::printf("[FAIL] %s.%s\n", tc.suite.c_str(), tc.name.c_str());
            for (const auto& f : CurrentFailures()) {
                std::printf("       %s %s\n", f.where.c_str(), f.message.c_str());
            }
        }
    }
    std::printf("\n==== %d/%d tests passed, %d failed ====\n", total - failedTests, total,
                failedTests);
    return failedTests;
}

}  // namespace cpptest

/// 定义一个测试用例（请在匿名命名空间内使用）。
#define TEST(suite, name)                                                \
    static void tg_test_##suite##_##name();                              \
    [[maybe_unused]] static const bool tg_reg_##suite##_##name =         \
        ::cpptest::Register(#suite, #name, &tg_test_##suite##_##name);   \
    static void tg_test_##suite##_##name()

using Assert = cpptest::Assert;
