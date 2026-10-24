import os
import re
import collections

CSHARP = r"D:/git/ToolGood.Algorithm2/csharp/ToolGood.Algorithm"
OUT = r"D:/git/ToolGood.Algorithm2/cpp/toolgood"
FUNCS_ROOT = os.path.join(CSHARP, "Internals/Functions")

# 1) 扫描函数类 -> 组目录
class_to_group = {}
group_files = collections.defaultdict(list)
for root, dirs, files in os.walk(FUNCS_ROOT):
    for f in files:
        if not f.endswith(".cs"):
            continue
        path = os.path.join(root, f)
        rel = os.path.relpath(root, FUNCS_ROOT).replace("\\", "/")
        group = "Root" if rel == "." else rel.split("/")[0]
        with open(path, encoding="utf-8-sig", errors="ignore") as fp:
            text = fp.read()
        for m in re.finditer(r"(?:internal|public)\s+(?:sealed\s+|abstract\s+|static\s+)?class\s+(\w+)", text):
            cls = m.group(1)
            class_to_group[cls] = group
            group_files[group].append((cls, path, text))

# 2) 解析访问器 switch（Function_fun 内）
visitor = os.path.join(CSHARP, "Internals/Visitors/MathFunctionVisitor.cs")
with open(visitor, encoding="utf-8-sig") as fp:
    vtext = fp.read()

start = vtext.index("public FunctionBase VisitFunction_fun")
end = len(vtext)
body = vtext[start:end]
pairs = re.findall(r"case mathLexer\.(\w+):\s*\n?\s*return new (\w+)\(", body)
print("Function_fun 条目:", len(pairs))

# 3) 检查哪些类没有源文件（Operator 组里的类命名不同）
missing = [c for _, c in pairs if c not in class_to_group]
print("缺少源文件映射的类:", missing)

# 4) 生成组头文件与注册表
order = ["Compare", "Csharp", "CsharpSecurity", "CsharpWeb", "DateTimes", "Financial",
         "Flow", "MathBase", "MathSum", "MathSum2", "MathTransformation",
         "MathTrigonometric", "Operator", "String", "Value", "Root"]
order = [g for g in order if g in group_files and g != "Root"]

GROUP_STD = {
    "Compare": "compare", "Csharp": "csharp", "CsharpSecurity": "csharpsecurity",
    "CsharpWeb": "csharpweb", "DateTimes": "datetimes", "Financial": "financial",
    "Flow": "flow", "MathBase": "mathbase", "MathSum": "mathsum", "MathSum2": "mathsum2",
    "MathTransformation": "mathtransformation", "MathTrigonometric": "mathtrigonometric",
    "Operator": "op", "String": "str", "Value": "value", "Root": "root",
}

os.makedirs(os.path.join(OUT, "Internals/Functions"), exist_ok=True)
for g in order:
    d = os.path.join(OUT, "Internals/Functions", g)
    os.makedirs(d, exist_ok=True)
    newfns = sorted({c for c, _, _ in group_files[g] if c.startswith("Function_")})
    lines = [f"// {g}.h - 对应 csharp/.../Internals/Functions/{g}/",
             "#pragma once", "", '#include <vector>', "",
             '#include "../../FunctionBase.h"', "", "namespace toolgood {", ""]
    for c in newfns:
        lines.append(f"Function New{c}(std::vector<Function> args);")
    lines.append("")
    lines.append(f"/// 注册 {g} 组全部函数。")
    lines.append(f"void Register{GROUP_STD[g]}();")
    lines.append("")
    lines.append("}  // namespace toolgood")
    lines.append("")
    with open(os.path.join(d, f"{g}.h"), "w", encoding="utf-8", newline="\n") as fp:
        fp.write("\n".join(lines))

# Registry.cpp
reg = ["// Registry.cpp - 函数注册表（由 csharp MathFunctionVisitor 的分派表自动生成）",
       '#include "Registry.h"', ""]
for g in order:
    reg.append(f'#include "Functions/{g}/{g}.h"')
reg += ["", "namespace toolgood {", ""]

for g in order:
    reg.append(f"static void RegisterGroup_{GROUP_STD[g]}() {{")
    seen = set()
    for token, cls in pairs:
        if class_to_group.get(cls) != g:
            continue
        if token in seen:
            continue
        seen.add(token)
        reg.append(f'    RegisterFunction("{token}", [](const std::vector<Function>& a) {{ return New{cls}(a); }});')
    reg.append("}")
    reg.append("")

reg += ["void RegisterAllFunctions() {",
        "    static bool done = false;",
        "    if (done) return;",
        "    done = true;"]
for g in order:
    reg.append(f"    RegisterGroup_{GROUP_STD[g]}();")
reg += ["}", "", "}  // namespace toolgood", ""]

with open(os.path.join(OUT, "Internals/Registry.cpp"), "w", encoding="utf-8", newline="\n") as fp:
    fp.write("\n".join(reg))

# 5) 输出分组清单，便于分工
for g in order:
    fns = sorted({c for c, _, _ in group_files[g] if c.startswith("Function_")})
    print(f"{g}: {len(fns)} -> {GROUP_STD[g]}")
with open(r"D:/git/ToolGood.Algorithm2/temp/cpp-test/groups.txt", "w", encoding="utf-8") as fp:
    for g in order:
        fns = sorted({c for c, _, _ in group_files[g] if c.startswith("Function_")})
        fp.write(f"== {g} ==\n")
        for c in fns:
            fp.write(f"  {c}\n")
print("total function classes:", sum(len({c for c, _, _ in group_files[g] if c.startswith('Function_')}) for g in order))
