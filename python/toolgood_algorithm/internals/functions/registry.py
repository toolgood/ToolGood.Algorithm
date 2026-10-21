"""Function registry: maps upper-case keyword -> implementation class."""
from __future__ import annotations

from typing import Dict, Type

from toolgood_algorithm.internals.functions.function_base import FunctionBase

FUNCTIONS: Dict[str, Type[FunctionBase]] = {}


def register(*names: str):
    """Class decorator registering a function implementation under one or more keys."""

    def decorator(cls: Type[FunctionBase]):
        for name in names:
            FUNCTIONS[name.upper()] = cls
        return cls

    return decorator


def register_class(name: str, cls: Type[FunctionBase]) -> None:
    FUNCTIONS[name.upper()] = cls


def get(name: str) -> Type[FunctionBase]:
    return FUNCTIONS[name.upper()]


def has(name: str) -> bool:
    return name.upper() in FUNCTIONS
