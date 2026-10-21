"""Unit conversion (``ToolGood.Algorithm/UnitConversion``).

Conversion factors follow the C# / Java converters: the maps store
"1 target unit = X base units", and ``convert`` computes
``value / source_factor * target_factor`` with 30 digits of precision.
"""
from __future__ import annotations

import re
from decimal import Decimal

from .area_converter import AreaConverter
from .distance_converter import DistanceConverter
from .mass_converter import MassConverter
from .volume_converter import VolumeConverter

__all__ = [
    "AreaConverter",
    "DistanceConverter",
    "MassConverter",
    "VolumeConverter",
    "try_convert",
    "unit_conversion",
]

_STRIP = re.compile(r"[\s\(\)（）\[\]<>]")


def _try(src: Decimal, source_unit: str, target_unit: str):
    for converter in (DistanceConverter, MassConverter, AreaConverter, VolumeConverter):
        result = converter.try_convert(source_unit, target_unit, src)
        if result is not None:
            return result
    return None


def try_convert(src: Decimal, source_unit: str, target_unit: str):
    return _try(src, source_unit, target_unit)


def unit_conversion(src: Decimal, old_src_unit: str, old_tar_unit: str, name: str = None) -> Decimal:
    """Convert ``src`` from ``old_src_unit`` to ``old_tar_unit``.

    Raises ``ValueError`` when no converter knows the pair (same behaviour as
    ``AlgorithmEngineHelper.UnitConversion``).
    """
    if not old_src_unit or not old_src_unit.strip() or not old_tar_unit or not old_tar_unit.strip():
        return src
    if old_src_unit == old_tar_unit:
        return src

    result = _try(src, old_src_unit, old_tar_unit)
    if result is not None:
        return result

    src_unit = _STRIP.sub("", old_src_unit)
    tar_unit = _STRIP.sub("", old_tar_unit)
    result = _try(src, src_unit, tar_unit)
    if result is not None:
        return result

    if not name:
        raise ValueError(
            "The input item has different units and cannot be converted from "
            f"[{src_unit}] to [{tar_unit}]"
        )
    raise ValueError(
        f"The input item [{name}] has different units and cannot be converted from "
        f"[{src_unit}] to [{tar_unit}]"
    )
