#!/bin/bash
# cpp/tests/build.sh - 编译 C++ 单元测试并运行
#
# 用法:
#   bash cpp/tests/build.sh                  # 全量：编译 + 运行
#   bash cpp/tests/build.sh --suite=MathBase # 只跑某个套件
#   bash cpp/tests/build.sh --filter=round   # 按用例名过滤
#   bash cpp/tests/build.sh --list           # 列出全部用例
#
# 说明:
#   - 测试源码在 cpp/tests/（随仓库提交），**编译产物全部落到 temp/**，
#     避免安全软件反复扫描仓库目录下的 exe（temp/ 已在 .gitignore 中）。
#   - 不要加 -static：静态大 exe 会被安全软件拦截/删除；产物名也避开 test.exe。
#   - 编译器默认用 temp/w64devkit（便携 MinGW），可用 CXX 覆盖。
set -u
cd "$(dirname "$0")/../.." || exit 1

CXX="${CXX:-/d/git/ToolGood.Algorithm2/temp/w64devkit/bin/g++}"
[ -x "$CXX" ] || CXX="g++"

OUT=temp/cpp-test/build   # 库目标文件
BIN=temp/cpp-test/unit    # 测试目标文件 + 可执行文件
mkdir -p "$OUT" "$BIN"

# 1) 编译库（增量：仅当 .cpp 比 .o 新时重编）
FAIL=0
OBJS=""
for f in $(find cpp/toolgood -name '*.cpp' | sort); do
  o="$OUT/$(echo "$f" | sed 's|cpp/toolgood/||; s|/|_|g; s|\.cpp$|.o|')"
  if [ ! -f "$o" ] || [ "$f" -nt "$o" ]; then
    if ! "$CXX" -std=c++17 -O1 -Wall -Wextra -Wno-unused-parameter -Icpp -c "$f" -o "$o" 2>"$o.err"; then
      echo "=== COMPILE FAIL(lib): $f"
      head -20 "$o.err"
      FAIL=1
    elif [ -s "$o.err" ]; then
      echo "=== WARN(lib): $f"; head -6 "$o.err"
    fi
  fi
  OBJS="$OBJS $o"
done
[ "$FAIL" != "0" ] && exit 2

# 2) 编译测试（cpp/tests/*.cpp + main.cpp，不含 tools/ 下的独立诊断程序）
TOBJS=""
for f in cpp/tests/main.cpp $(find cpp/tests -maxdepth 1 -name '*.cpp' ! -name 'main.cpp' | sort); do
  o="$BIN/$(basename "$f" .cpp).o"
  if ! "$CXX" -std=c++17 -O1 -Wall -Wextra -Wno-unused-parameter -Icpp -Icpp/tests -c "$f" -o "$o" 2>"$o.err"; then
    echo "=== COMPILE FAIL(test): $f"
    head -25 "$o.err"
    FAIL=1
  elif [ -s "$o.err" ]; then
    echo "=== WARN(test): $f"; head -6 "$o.err"
  fi
  TOBJS="$TOBJS $o"
done
[ "$FAIL" != "0" ] && exit 3

# 3) 链接
EXE="$BIN/tg_tests.exe"
"$CXX" -std=c++17 -O2 -s $TOBJS $OBJS -o "$EXE" 2>"$BIN/link.err"
if [ $? -ne 0 ]; then
  echo "=== LINK FAIL"; head -40 "$BIN/link.err"; exit 4
fi

# 4) 运行
"$EXE" "$@"
echo "--- runner exit=$?"
