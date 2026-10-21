"""Conformance runner: replays the Java JUnit assertions against the Python port.

The Java tests under ``java/toolgood.algorithm/src/test/java`` follow a very
regular style::

    int t = engine.TryEvaluate("1+2", 0);
    assertEquals(3, t);

This script extracts those statements and executes them with the Python engine,
printing a per-file summary plus any failures.  Tests that use loops, custom
engines or parameters are skipped (they are covered by ``tests/test_*.py``).
"""
from __future__ import annotations

import ast
import os
import re
import sys
from decimal import Decimal

sys.path.insert(0, os.path.join(os.path.dirname(__file__), ".."))

from toolgood_algorithm import AlgorithmEngine, AlgorithmEngineEx, MyDate, Operand  # noqa: E402

TEST_ROOT = os.path.abspath(
    os.path.join(os.path.dirname(__file__), "..", "..", "java", "toolgood.algorithm", "src", "test", "java")
)

_DECL = re.compile(
    r"^\s*(int|double|String|boolean|BigDecimal|MyDate|DateTime|long|float|Object)\s+(\w+)\s*=\s*(.+?);\s*$"
)
_ASSIGN_CALL = re.compile(
    r"^\s*(\w+)\s*=\s*(\w+)\.(TryEvaluate|TryEvaluate_MyDate)\((.*)\);\s*$"
)
_DECL_CALL = re.compile(
    r"^\s*(int|double|String|boolean|BigDecimal|MyDate|DateTime|long|float|Object)\s+(\w+)\s*=\s*(\w+)\.(TryEvaluate|TryEvaluate_MyDate)\((.*)\);\s*$"
)
_ASSERT_EQ = re.compile(r"^\s*assertEquals\((.*)\);\s*$")
_ASSERT_TRUE = re.compile(r"^\s*assertTrue\((.*)\);\s*$")
_ASSERT_FALSE = re.compile(r"^\s*assertFalse\((.*)\);\s*$")
_ASSERT_NULL = re.compile(r"^\s*assertNull\((.*)\);\s*$")
_CONFIG = re.compile(r"^\s*(\w+)\.(\w+)\s*=\s*(.+?);\s*$")
_CALL_CONFIG = re.compile(r"^\s*(\w+)\.(SetUseExcelIndex)\((true|false)\);\s*$")
_ADD_PARAM = re.compile(r'^\s*(\w+)\.AddParameter\(\s*"([^"]*)"\s*,\s*(.+?)\);\s*$')
_CUSTOM_ENGINE = re.compile(r"^\s*\w+\s+(\w+)\s*=\s*new\s+Cylinder")
_NEW_ENGINE = re.compile(r"^\s*(\w+)\s+(\w+)\s*=\s*new\s+(AlgorithmEngineEx|AlgorithmEngine)\s*\(([^)]*)\)")
_UNIT_CONFIG = re.compile(r"^\s*(\w+)\.(DistanceUnit|AreaUnit|VolumeUnit|MassUnit)\s*=\s*(\w+)\.(\w+);")


def _split_args(text: str):
    """Split on commas that are not inside quotes / parentheses."""
    args, depth, quote, buf = [], 0, None, []
    i = 0
    while i < len(text):
        ch = text[i]
        if quote:
            buf.append(ch)
            if ch == "\\" and i + 1 < len(text):
                buf.append(text[i + 1])
                i += 2
                continue
            if ch == quote:
                quote = None
        elif ch in ("'", '"'):
            quote = ch
            buf.append(ch)
        elif ch in "([{":
            depth += 1
            buf.append(ch)
        elif ch in ")]}":
            depth -= 1
            buf.append(ch)
        elif ch == "," and depth == 0:
            args.append("".join(buf).strip())
            buf = []
        else:
            buf.append(ch)
        i += 1
    if buf:
        args.append("".join(buf).strip())
    return args


_CAST = re.compile(r"^\((\w+)\)\s*(.*)$")


def _unwrap(value):
    if isinstance(value, Operand):
        if value.is_number():
            return value.number_value()
        if value.is_text():
            return value.text_value()
        if value.is_boolean():
            return value.boolean_value()
        if value.is_date():
            return str(value.date_value())
        if value.is_null():
            return None
        return str(value)
    return value


def _java_literal(text: str):
    text = text.strip()
    if text.startswith("(") and ")" in text and text.endswith(")"):
        # casts like (String)null
        inner = text[text.index(")") + 1:].strip()
        if inner:
            text = inner
    if text in ("null", "NULL"):
        return None
    if text in ("true", "false"):
        return text == "true"
    if text.startswith('"') and text.endswith('"'):
        try:
            return ast.literal_eval(text)
        except Exception:  # noqa: BLE001
            return text[1:-1]
    try:
        return int(text)
    except ValueError:
        pass
    try:
        return float(text)
    except ValueError:
        pass
    return ("EXPR", text)


class Runner:
    def __init__(self, path: str):
        self.path = path
        self.engines = {"engine": AlgorithmEngine()}
        self.vars = {}
        self.results = []  # (ok, line_no, description)
        self.skipped = 0
        self.unsupported = set()

    # ------------------------------------------------------------------ setup
    def _engine(self, name: str):
        if name not in self.engines:
            self.engines[name] = AlgorithmEngineEx()
        return self.engines[name]

    def _reset(self):
        self.engines = {"engine": AlgorithmEngine()}
        self.vars = {}

    # ------------------------------------------------------------------ run
    def run(self):
        lines = open(self.path, encoding="utf-8").read().split("\n")
        for index, line in enumerate(lines, 1):
            stripped = line.strip()
            if stripped.startswith("//") or stripped.startswith("*"):
                continue
            try:
                self._handle(index, line)
            except Exception as exc:  # noqa: BLE001
                self.results.append((False, index, f"<runner error> {exc}"))
        return self.results

    def _handle(self, line_no: int, line: str) -> None:
        if "@Test" in line:
            self.engines = {"engine": AlgorithmEngine()}
            self.vars = {}
            self.unsupported = set()
            return
        m = _NEW_ENGINE.match(line)
        if m:
            _type_name, var, engine_type, ctor_args = m.groups()
            ignore_case = "true" in ctor_args.lower()
            self.engines[var] = AlgorithmEngineEx(ignore_case) if engine_type == "AlgorithmEngineEx" else AlgorithmEngine(ignore_case)
            return
        m = _UNIT_CONFIG.match(line)
        if m:
            engine_name, prop, enum_name, enum_value = m.groups()
            from toolgood_algorithm import enums as _enums

            setattr(self._engine(engine_name), prop, getattr(_enums, enum_name)[enum_value])
            return
        if ".ClearParameters()" in line:
            self._engine(line.split(".")[0].strip()).clear_parameters()
            return
        if "AddParameter(" in line and not line.strip().endswith(";"):
            self.unsupported.add(line.split(".")[0].strip())
            return
        m = _DECL_CALL.match(line)
        if m:
            type_name, var, engine_name, method, args = m.groups()
            self._record_call(line_no, type_name, var, engine_name, method, args)
            return
        m = _ASSIGN_CALL.match(line)
        if m:
            var, engine_name, method, args = m.groups()
            type_name = self.vars.get(var, (None, None, None))[0]
            self._record_call(line_no, type_name, var, engine_name, method, args)
            return
        m = _CUSTOM_ENGINE.match(line)
        if m:
            self.unsupported.add(m.group(1))
            return
        if "AddParameterFromJson" in line:
            name = line.split(".")[0].strip()
            self.unsupported.add(name)
            return
        m = _ADD_PARAM.match(line)
        if m:
            engine_name, key, value_text = m.groups()
            value_text = value_text.strip()
            if "Arrays.asList" in value_text or "Operand.Create" in value_text:
                self.unsupported.add(engine_name)
                return
            value = _java_literal(value_text)
            if isinstance(value, tuple):
                self.unsupported.add(engine_name)
                return
            self._engine(engine_name).add_parameter(key, value)
            return
        m = _CALL_CONFIG.match(line)
        if m:
            engine_name, method, value = m.groups()
            self._engine(engine_name).ExcelIndex = 1 if value == "true" else 0
            return
        m = _DECL.match(line)
        if m:
            type_name, var, value = m.groups()
            literal = _java_literal(value)
            if isinstance(literal, tuple):
                self.vars.pop(var, None)
            else:
                self.vars[var] = (type_name, None, literal)
            return
        m = _CONFIG.match(line)
        if m:
            engine_name, prop, value = m.groups()
            if prop in ("IgnoreCase", "UseLocalTime", "UseStrictMode", "UseParseCache", "UseTempDict"):
                setattr(self._engine(engine_name), prop, value.strip() == "true")
            elif prop == "ExcelIndex":
                try:
                    self._engine(engine_name).ExcelIndex = int(value)
                except ValueError:
                    pass
            return
        m = _ASSERT_EQ.match(line)
        if m:
            self._assert_equals(line_no, _split_args(m.group(1)))
            return
        m = _ASSERT_TRUE.match(line)
        if m:
            self._assert_bool(line_no, m.group(1), True)
            return
        m = _ASSERT_FALSE.match(line)
        if m:
            self._assert_bool(line_no, m.group(1), False)
            return
        m = _ASSERT_NULL.match(line)
        if m:
            var = m.group(1).strip()
            entry = self.vars.get(var)
            if entry and entry[1] is not None:
                ok = self._evaluate(entry) is None
                self.results.append((ok, line_no, f"{var} expected null"))
            else:
                self.skipped += 1

    def _record_call(self, line_no, type_name, var, engine_name, method, args_text):
        parts = _split_args(args_text)
        if len(parts) < 2 or not parts[0].startswith('"') or parts[0].rstrip().endswith("+"):
            self.skipped += 1
            return
        expr = _java_literal(parts[0])
        cast = None
        default_text = parts[1].strip()
        mcast = _CAST.match(default_text)
        if mcast:
            cast = mcast.group(1)
            default_text = mcast.group(2).strip()
        default = _java_literal(default_text)
        if isinstance(expr, tuple) or isinstance(default, tuple):
            self.skipped += 1
            return
        self.vars[var] = (type_name, (engine_name, method, expr, default, cast), expr)

    def _evaluate(self, entry):
        type_name, call, _ = entry
        if call is None:
            return entry[2]
        engine_name, method, expr, default, cast = call
        if engine_name in self.unsupported:
            raise RuntimeError("custom engine")
        engine = self._engine(engine_name)
        if method == "TryEvaluate_MyDate":
            return engine.try_evaluate_my_date(expr, None)
        if cast in ("String", "Object"):
            return engine.try_evaluate_text(expr, default)
        if cast in ("MyDate",):
            return engine.try_evaluate_my_date(expr, default)
        if cast in ("BigDecimal", "Decimal", "decimal"):
            return engine.try_evaluate_number(expr, default)
        if cast in ("DateTime",):
            return engine.try_evaluate_datetime(expr, default)
        if isinstance(default, bool):
            return engine.try_evaluate(expr, default) if type_name == "boolean" else engine.try_evaluate(expr, default)
        if isinstance(default, str):
            return engine.try_evaluate(expr, default)
        if isinstance(default, float):
            return engine.try_evaluate(expr, default)
        if isinstance(default, int):
            return engine.try_evaluate(expr, default)
        return engine.try_evaluate(expr, default)

    def _assert_equals(self, line_no, args):
        if len(args) < 2:
            self.skipped += 1
            return
        expected = _java_literal(args[0])
        var = args[1].strip()
        delta = None
        if len(args) >= 3:
            maybe = _java_literal(args[2])
            if isinstance(maybe, (int, float)):
                delta = float(maybe)
        entry = self.vars.get(var)
        if entry is None or entry[1] is None:
            self.skipped += 1
            return
        if isinstance(expected, tuple):
            self.skipped += 1
            return
        try:
            actual = _unwrap(self._evaluate(entry))
        except RuntimeError:
            self.skipped += 1
            return
        ok = False
        detail = ""
        if isinstance(expected, bool):
            ok = bool(actual) == expected
        elif isinstance(expected, str):
            ok = actual == expected
            detail = f"expected {expected!r} got {actual!r}"
        elif expected is None:
            ok = actual is None
        else:
            try:
                if isinstance(actual, MyDate):
                    ok = False
                elif isinstance(actual, Decimal):
                    ok = abs(float(actual) - float(expected)) <= (delta if delta is not None else 1e-10)
                else:
                    ok = abs(float(actual) - float(expected)) <= (delta if delta is not None else 1e-10)
            except (TypeError, ValueError):
                ok = actual == expected
            detail = f"expected {expected} got {actual}"
        self.results.append((ok, line_no, detail))

    def _assert_bool(self, line_no, var, expected):
        var = var.strip()
        entry = self.vars.get(var)
        if entry is None or entry[1] is None:
            self.skipped += 1
            return
        try:
            actual = _unwrap(self._evaluate(entry))
        except RuntimeError:
            self.skipped += 1
            return
        self.results.append((bool(actual) == expected, line_no, f"expected {expected} got {actual}"))


def main():
    files = []
    for dirpath, _dirnames, filenames in os.walk(TEST_ROOT):
        for name in sorted(filenames):
            if name.endswith("Test.java"):
                files.append(os.path.join(dirpath, name))

    total_ok = total_fail = total_skip = 0
    failures = []
    for path in sorted(files):
        runner = Runner(path)
        results = runner.run()
        ok = sum(1 for r in results if r[0])
        fail = [r for r in results if not r[0]]
        total_ok += ok
        total_fail += len(fail)
        total_skip += runner.skipped
        print(f"{os.path.basename(path):32s} pass={ok:4d} fail={len(fail):3d} skip={runner.skipped:3d}")
        for _ok, line, detail in fail:
            failures.append((os.path.basename(path), line, detail))

    print("-" * 70)
    print(f"TOTAL pass={total_ok} fail={total_fail} skip={total_skip}")
    if failures:
        print("\nFailures:")
        for name, line, detail in failures[:80]:
            print(f"  {name}:{line} {detail}")


if __name__ == "__main__":
    main()
