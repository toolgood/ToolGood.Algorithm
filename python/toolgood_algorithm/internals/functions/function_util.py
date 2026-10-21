"""Helpers shared by the function implementations (ported ``FunctionUtil``)."""
from __future__ import annotations

from decimal import Decimal, ROUND_DOWN
from typing import List, Optional, Tuple

from toolgood_algorithm.internals.visitors.char_util import equals as _str_equals
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.numeric import ZERO, ONE


def estimate_count(args: List[Operand]) -> int:
    count = 0
    for item in args:
        if item.type() == OperandType.ARRAY:
            count += len(item.array_value())
        elif item.type() == OperandType.JSON:
            count += 8
        else:
            count += 1
    return count


def flatten_to_list(args: List[Operand], result: List[Operand]) -> bool:
    stack = list(args)
    while stack:
        item = stack.pop()
        if item.type() == OperandType.ARRAY:
            stack.extend(reversed(item.array_value()))
        elif item.type() == OperandType.JSON:
            converted = item.to_array(None)
            if converted.is_error():
                return False
            stack.extend(reversed(converted.array_value()))
        else:
            result.append(item)
    return True


def flatten_to_big_decimal_list(args: List[Operand], result: List[Decimal]) -> bool:
    stack = list(args)
    while stack:
        item = stack.pop()
        if item.type() == OperandType.ARRAY:
            stack.extend(reversed(item.array_value()))
        elif item.type() == OperandType.JSON:
            converted = item.to_array(None)
            if converted.is_error():
                return False
            stack.extend(reversed(converted.array_value()))
        else:
            if item.type() == OperandType.NUMBER:
                result.append(item.number_value())
            else:
                converted = item.to_number(None)
                if converted.is_error():
                    return False
                result.append(converted.number_value())
    return True


def flatten_to_number_list(arg: Operand, result: List[Decimal]) -> bool:
    if arg.is_error():
        return False
    if arg.type() == OperandType.ARRAY:
        return flatten_to_big_decimal_list(arg.array_value(), result)
    if arg.type() == OperandType.JSON:
        converted = arg.to_array(None)
        if converted.is_error():
            return False
        return flatten_to_big_decimal_list(converted.array_value(), result)
    if arg.type() == OperandType.NUMBER:
        result.append(arg.number_value())
    else:
        converted = arg.to_number(None)
        if converted.is_error():
            return False
        result.append(converted.number_value())
    return True


def flatten_to_string_list(arg: Operand, result: List[str]) -> bool:
    if arg.is_error():
        return False
    if arg.type() == OperandType.ARRAY:
        for item in arg.array_value():
            if item.type() in (OperandType.ARRAY, OperandType.JSON):
                if not flatten_to_string_list(item, result):
                    return False
            else:
                converted = item.to_text(None)
                if converted.is_error():
                    return False
                result.append(converted.text_value())
        return True
    if arg.type() == OperandType.JSON:
        converted = arg.to_array(None)
        if converted.is_error():
            return False
        return flatten_to_string_list(converted, result)
    converted = arg.to_text(None)
    if converted.is_error():
        return False
    result.append(converted.text_value())
    return True


def get_count_if(values: List[Decimal], value: Decimal) -> int:
    return sum(1 for item in values if item == value)


def get_count_if_op(values: List[Decimal], op: str, value: Decimal) -> int:
    return sum(1 for item in values if compare_values(item, value, op))


def get_sum_if(values: List[Decimal], value: Decimal, sums: List[Decimal]) -> Decimal:
    total = ZERO
    for i in range(min(len(values), len(sums))):
        if values[i] == value:
            total += sums[i]
    return total


def get_sum_if_op(values: List[Decimal], op: str, value: Decimal, sums: List[Decimal]) -> Decimal:
    total = ZERO
    for i in range(min(len(values), len(sums))):
        if compare_values(values[i], value, op):
            total += sums[i]
    return total


def compare_values(a: Decimal, b: Decimal, op: str) -> bool:
    if _str_equals(op, "<"):
        return a < b
    if _str_equals(op, "<="):
        return a <= b
    if _str_equals(op, ">"):
        return a > b
    if _str_equals(op, ">="):
        return a >= b
    if _str_equals(op, "=", "==", "==="):
        return a == b
    return a != b


def get_gcd_two(a: Decimal, b: Decimal) -> Decimal:
    while b != ZERO:
        a, b = b, a % b
    return a


def get_gcd(values: List[Decimal]) -> Decimal:
    if not values:
        return ONE
    g = values[0].to_integral_value(rounding=ROUND_DOWN)
    for value in values[1:]:
        g = get_gcd_two(g, value.to_integral_value(rounding=ROUND_DOWN))
        if g == ONE:
            break
    return g


def get_lcm(values: List[Decimal]) -> Decimal:
    if not values:
        return ONE
    result = None
    for value in values:
        v = value.to_integral_value(rounding=ROUND_DOWN)
        if v <= ONE:
            continue
        if result is None:
            result = v
            continue
        g = get_gcd_two(v, result) if v > result else get_gcd_two(result, v)
        result = (result / g).to_integral_value(rounding=ROUND_DOWN) * v
    return result if result is not None else ONE


def get_factorial(value: Decimal) -> Decimal:
    if value <= ZERO:
        return ONE
    n = int(value.to_integral_value(rounding=ROUND_DOWN))
    result = ONE
    for i in range(n, 0, -1):
        result *= i
    return result


def _try_parse_decimal(text: str) -> Optional[Decimal]:
    try:
        return Decimal(text)
    except Exception:  # noqa: BLE001
        return None


def parse_sum_if_match(text: str) -> Optional[Tuple[str, Decimal]]:
    if text is None or len(text) == 0:
        return None
    c = text[0]
    if c in (">", "＞"):
        if len(text) > 1 and text[1] in ("=", "＝"):
            d = _try_parse_decimal(text[2:].strip())
            return (">=", d) if d is not None else None
        d = _try_parse_decimal(text[1:].strip())
        return (">", d) if d is not None else None
    if c in ("<", "＜"):
        if len(text) > 1 and text[1] in ("=", "＝"):
            d = _try_parse_decimal(text[2:].strip())
            return ("<=", d) if d is not None else None
        if len(text) > 1 and text[1] in (">", "＞"):
            d = _try_parse_decimal(text[2:].strip())
            return ("!=", d) if d is not None else None
        d = _try_parse_decimal(text[1:].strip())
        return ("<", d) if d is not None else None
    if c in ("=", "＝"):
        index = 1
        if len(text) > 1 and text[1] in ("=", "＝"):
            index = 2
            if len(text) > 2 and text[2] in ("=", "＝"):
                index = 3
        d = _try_parse_decimal(text[index:].strip())
        return ("=", d) if d is not None else None
    if c in ("!", "！"):
        index = 1
        if len(text) > 1 and text[1] in ("=", "＝"):
            index = 2
            if len(text) > 2 and text[2] in ("=", "＝"):
                index = 3
        d = _try_parse_decimal(text[index:].strip())
        return ("!=", d) if d is not None else None
    return None


def try_parse_boolean(text: Optional[str]) -> Optional[bool]:
    if text is None:
        return None
    if len(text) == 1:
        c = text
        if c in ("1", "是", "有"):
            return True
        if c in ("0", "否", "无"):
            return False
    elif len(text) == 2:
        if text.lower() == "no" or text in ("不是", "没有"):
            return False
    elif len(text) == 3:
        if text.lower() == "yes":
            return True
    elif len(text) == 4:
        if text.lower() == "true":
            return True
    elif len(text) == 5:
        if text.lower() == "false":
            return False
    return None


def _select_pivot(values: List[Decimal], left: int, right: int) -> int:
    mid = left + (right - left) // 2
    a, b, c = values[left], values[mid], values[right]
    if a < b:
        if b < c:
            return mid
        if a < c:
            return right
        return left
    if a < c:
        return left
    if b < c:
        return right
    return mid


def _partition(values: List[Decimal], left: int, right: int, pivot_index: int) -> int:
    pivot = values[pivot_index]
    values[pivot_index], values[right] = values[right], pivot
    store = left
    for i in range(left, right):
        if values[i] < pivot:
            values[store], values[i] = values[i], values[store]
            store += 1
    values[right], values[store] = values[store], pivot
    return store


def quick_select(values: List[Decimal], k: int, largest: bool) -> Decimal:
    if len(values) == 1:
        return values[0]
    target = len(values) - 1 - k if largest else k
    left, right = 0, len(values) - 1
    while left < right:
        pivot_index = _partition(values, left, right, _select_pivot(values, left, right))
        if target == pivot_index:
            return values[target]
        if target < pivot_index:
            right = pivot_index - 1
        else:
            left = pivot_index + 1
    return values[left]


def get_rank(values: List[Decimal], num: Decimal, descending: bool) -> int:
    rank = 1
    count = 0
    for item in values:
        if item == num:
            count += 1
        elif (descending and item > num) or (not descending and item < num):
            rank += 1
    return rank if count > 0 else 0
