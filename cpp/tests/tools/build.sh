#!/bin/bash
# cpp/tests/tools/build.sh - 编译诊断工具（冒烟测试 + 差分执行器）
#
# 用法:
#   bash cpp/tests/tools/build.sh smoke   # 端到端冒烟（60 条）
#   bash cpp/tests/tools/build.sh diff    # 生成 C++ 侧差分输出到 temp/cpp-test/cc_out.txt
#   bash cpp/tests/tools/build.sh all     # 两个都跑（默认）
#
# 产物统一落在 temp/cpp-test/，不写仓库目录（temp/ 已 gitignore，避免杀软反复告警）。
set -u
cd "$(dirname "$0")/../../.." || exit 1

CXX="${CXX:-/d/git/ToolGood.Algorithm2/temp/w64devkit/bin/g++}"
[ -x "$CXX" ] || CXX="g++"

OUT=temp/cpp-test/build
BIN=temp/cpp-test/tools
mkdir -p "$OUT" "$BIN"

# 1) 增量编译库
FAIL=0
OBJS=""
for f in $(find cpp/toolgood -name '*.cpp' | sort); do
  o="$OUT/$(echo "$f" | sed 's|cpp/toolgood/||; s|/|_|g; s|\.cpp$|.o|')"
  if [ ! -f "$o" ] || [ "$f" -nt "$o" ]; then
    if ! "$CXX" -std=c++17 -O1 -Wall -Wextra -Wno-unused-parameter -Icpp -c "$f" -o "$o" 2>"$o.err"; then
      echo "=== COMPILE FAIL(lib): $f"; head -20 "$o.err"; FAIL=1
    fi
  fi
  OBJS="$OBJS $o"
done
[ "$FAIL" != "0" ] && exit 2

run_smoke() {
  "$CXX" -std=c++17 -O2 -s -Icpp cpp/tests/tools/smoke.cpp $OBJS -o "$BIN/tg_smoke.exe" || return 1
  "$BIN/tg_smoke.exe"
}

run_diff() {
  "$CXX" -std=c++17 -O2 -s -Icpp cpp/tests/tools/ccdiff.cpp $OBJS -o "$BIN/tg_ccdiff.exe" || return 1
  "$BIN/tg_ccdiff.exe" cpp/tests/tools/exprs.txt > temp/cpp-test/cc_out.txt
  echo "--- 已写出 temp/cpp-test/cc_out.txt（与 temp/cpp-test/cs_out.txt 比对）"
}

case "${1:-all}" in
  smoke) run_smoke ;;
  diff)  run_diff ;;
  all)   run_smoke; run_diff ;;
  *) echo "usage: $0 [smoke|diff|all]"; exit 1 ;;
esac
