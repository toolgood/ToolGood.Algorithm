"""value: shared helpers and base classes (kept out of the per-class files)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import AreaUnitType, DistanceUnitType, MassUnitType, NumberUnitType, VolumeUnitType


class _Distance:
    FACTORS = {
        NumberUnitType.MM: Decimal("0.001"),
        NumberUnitType.CM: Decimal("0.01"),
        NumberUnitType.DM: Decimal("0.1"),
        NumberUnitType.M: Decimal("1"),
        NumberUnitType.KM: Decimal("1000"),
    }

_AREA_FACTORS = {
    NumberUnitType.MM2: Decimal("0.000001"),
    NumberUnitType.CM2: Decimal("0.0001"),
    NumberUnitType.DM2: Decimal("0.01"),
    NumberUnitType.M2: Decimal("1"),
    NumberUnitType.KM2: Decimal("1000000"),
}

_VOLUME_FACTORS = {
    NumberUnitType.MM3: Decimal("0.000000001"),
    NumberUnitType.CM3: Decimal("0.000001"),
    NumberUnitType.DM3: Decimal("0.001"),
    NumberUnitType.M3: Decimal("1"),
    NumberUnitType.KM3: Decimal("1000000000"),
}

_MASS_FACTORS = {
    NumberUnitType.G: Decimal("1"),
    NumberUnitType.KG: Decimal("1000"),
    NumberUnitType.T: Decimal("1000000"),
}

_UNIT_DICT = {
    "KM": NumberUnitType.KM,
    "M": NumberUnitType.M,
    "DM": NumberUnitType.DM,
    "CM": NumberUnitType.CM,
    "MM": NumberUnitType.MM,
    "KM2": NumberUnitType.KM2,
    "M2": NumberUnitType.M2,
    "DM2": NumberUnitType.DM2,
    "CM2": NumberUnitType.CM2,
    "MM2": NumberUnitType.MM2,
    "M3": NumberUnitType.M3,
    "DM3": NumberUnitType.DM3,
    "CM3": NumberUnitType.CM3,
    "MM3": NumberUnitType.MM3,
    "KM3": NumberUnitType.KM3,
    "L": NumberUnitType.DM3,
    "ML": NumberUnitType.CM3,
    "T": NumberUnitType.T,
    "KG": NumberUnitType.KG,
    "G": NumberUnitType.G,
}

_DISTANCE_ALIASES = {
    1: DistanceUnitType.MM,
    2: DistanceUnitType.CM,
    3: DistanceUnitType.DM,
    4: DistanceUnitType.M,
    5: DistanceUnitType.KM,
}

_AREA_ALIASES = {
    11: AreaUnitType.MM2,
    12: AreaUnitType.CM2,
    13: AreaUnitType.DM2,
    14: AreaUnitType.M2,
    15: AreaUnitType.KM2,
}

_VOLUME_ALIASES = {
    21: VolumeUnitType.MM3,
    22: VolumeUnitType.CM3,
    23: VolumeUnitType.DM3,
    24: VolumeUnitType.M3,
    25: VolumeUnitType.KM3,
}

_MASS_ALIASES = {31: MassUnitType.G, 32: MassUnitType.KG, 33: MassUnitType.T}


def _as_enum(value, mapping):
    if isinstance(value, int):
        return mapping[int(value)]
    return value


def transformation_unit(src: Decimal, type1, distance, area, volume, mass) -> Decimal:
    """Convert ``src`` expressed in ``type1`` into the engine configured unit."""
    if type1 is None:
        raise ValueError("Number unit is error!")
    if int(type1) in (1, 2, 3, 4, 5):
        target = _as_enum(distance, _DISTANCE_ALIASES)
        return src * _Distance.FACTORS[type1] / _Distance.FACTORS[target]
    if 11 <= int(type1) <= 15:
        target = _as_enum(area, _AREA_ALIASES)
        return src * _AREA_FACTORS[type1] / _AREA_FACTORS[target]
    if 21 <= int(type1) <= 25:
        target = _as_enum(volume, _VOLUME_ALIASES)
        return src * _VOLUME_FACTORS[type1] / _VOLUME_FACTORS[target]
    if 31 <= int(type1) <= 33:
        target = _as_enum(mass, _MASS_ALIASES)
        return src * _MASS_FACTORS[type1] / _MASS_FACTORS[target]
    raise ValueError("Number unit is error!")
