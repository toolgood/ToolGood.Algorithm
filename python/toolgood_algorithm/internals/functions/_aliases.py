"""CamelCase aliases for the ``FunctionBase`` helpers.

Imported once by :mod:`toolgood_algorithm.internals.functions` so that
``function.GetNumber_1(...)`` keeps working for callers coming from C#/Java.
The aliases are wrappers (not direct assignments) so subclass overrides are
honoured.
"""
from __future__ import annotations

from toolgood_algorithm.internals.functions.function_base import FunctionBase


def _make_alias(name):
    def method(self, *args, **kwargs):
        return getattr(self, name)(*args, **kwargs)

    return method


for _camel, _snake in (
    ("GetText_1", "_get_text_1"),
    ("GetNumber_1", "_get_number_1"),
    ("GetDate_1", "_get_date_1"),
    ("GetBoolean_1", "_get_boolean_1"),
    ("GetArray_1", "_get_array_1"),
    ("GetText_2", "_get_text_2"),
    ("GetNumber_2", "_get_number_2"),
    ("GetDate_2", "_get_date_2"),
    ("GetBoolean_2", "_get_boolean_2"),
    ("GetArray_2", "_get_array_2"),
    ("GetNumber_3", "_get_number_3"),
    ("GetText_3", "_get_text_3"),
    ("GetNumber_4", "_get_number_4"),
    ("GetText_4", "_get_text_4"),
    ("GetNumber_5", "_get_number_5"),
    ("GetText", "_get_text"),
    ("GetNumber", "_get_number"),
    ("GetDate", "_get_date"),
    ("GetBoolean", "_get_boolean"),
    ("GetArray", "_get_array"),
    ("TryEvaluateAll", "try_evaluate_all"),
    ("ParameterError", "_parameter_error"),
    ("FunctionError", "_function_error"),
    ("CompareError", "_compare_error"),
    ("Div0Error", "_div0_error"),
    ("NumError", "_num_error"),
):
    if hasattr(FunctionBase, _snake):
        setattr(FunctionBase, _camel, _make_alias(_snake))
