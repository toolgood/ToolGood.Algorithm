// cpp/tests/tools/ccdiff.cpp - C++ 侧的差分测试执行器（输出格式与 csdiff 一致）
#include <cstdio>
#include <fstream>
#include <iostream>
#include <string>
#include <vector>

#include "toolgood/AlgorithmEngine.h"

using namespace toolgood;

static std::string Norm(const Operand& r) {
    if (r.IsError()) return "ERROR:" + r.ErrorMsg();
    if (r.IsNumber()) {
        std::string s = r.NumberValue().ToString();
        if (s.find('.') != std::string::npos) {
            size_t p = s.find_last_not_of('0');
            if (p != std::string::npos && s[p] == '.') p--;
            s = s.substr(0, p + 1);
        }
        if (s == "-0" || s.empty()) s = "0";
        return s;
    }
    if (r.IsBoolean()) return r.BooleanValue() ? "true" : "false";
    if (r.IsText()) return r.TextValue();
    if (r.IsDate()) return r.DateValue().ToString();
    return r.ToString();
}

static std::string Escape(const std::string& s) {
    std::string out;
    for (char c : s) {
        if (c == '\n') out += "\\n";
        else if (c == '\r') out += "\\r";
        else if (c == '\t') out += "\\t";
        else out += c;
    }
    return out;
}

int main(int argc, char** argv) {
    std::string path = argc > 1 ? argv[1] : "temp/cpp-test/exprs.txt";
    std::ifstream in(path);
    if (!in) {
        std::cerr << "cannot open " << path << "\n";
        return 1;
    }
    AlgorithmEngine engine;
    std::string line;
    while (std::getline(in, line)) {
        // trim
        size_t b = line.find_first_not_of(" \t\r\n");
        if (b == std::string::npos) continue;
        size_t e = line.find_last_not_of(" \t\r\n");
        std::string exp = line.substr(b, e - b + 1);
        std::string outp;
        try {
            Function f = engine.Parse(exp);
            outp = Norm(f->Evaluate(engine));
        } catch (const std::exception& ex) {
            outp = std::string("EX:") + ex.what();
        }
        std::cout << exp << "\t" << Escape(outp) << "\n";
    }
    return 0;
}
