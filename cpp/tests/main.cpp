// temp/cpp-test/main.cpp - 测试入口（对应 C# 的 PetaTest.Runner.RunMain）
#include <cstring>
#include <string>

#include "cpptest.h"

int main(int argc, char** argv) {
    const char* suite = nullptr;
    const char* name = nullptr;
    for (int i = 1; i < argc; i++) {
        const char* a = argv[i];
        if (std::strncmp(a, "--suite=", 8) == 0) {
            suite = a + 8;
        } else if (std::strncmp(a, "--filter=", 9) == 0) {
            name = a + 9;
        } else if (std::strcmp(a, "--list") == 0) {
            for (const auto& tc : cpptest::Cases()) {
                std::printf("%s.%s\n", tc.suite.c_str(), tc.name.c_str());
            }
            return 0;
        } else if (std::strcmp(a, "--help") == 0 || std::strcmp(a, "-h") == 0) {
            std::printf("用法: runner [--suite=名称] [--filter=子串] [--list]\n");
            return 0;
        }
    }
    return cpptest::RunAll(suite, name) == 0 ? 0 : 1;
}
