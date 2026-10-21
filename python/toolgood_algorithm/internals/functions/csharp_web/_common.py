"""csharp_web: shared helpers and base classes (kept out of the per-class files)."""
from __future__ import annotations

_HEX = "0123456789abcdef"

_HTML_ENTITIES = {
    "amp": "&",
    "lt": "<",
    "gt": ">",
    "quot": '"',
    "apos": "'",
    # Latin-1 special characters
    "nbsp": "\u00A0",
    "iexcl": "\u00A1",
    "cent": "\u00A2",
    "pound": "\u00A3",
    "curren": "\u00A4",
    "yen": "\u00A5",
    "brvbar": "\u00A6",
    "sect": "\u00A7",
    "uml": "\u00A8",
    "copy": "\u00A9",
    "ordf": "\u00AA",
    "laquo": "\u00AB",
    "not": "\u00AC",
    "shy": "\u00AD",
    "reg": "\u00AE",
    "macr": "\u00AF",
    "deg": "\u00B0",
    "plusmn": "\u00B1",
    "sup2": "\u00B2",
    "sup3": "\u00B3",
    "acute": "\u00B4",
    "micro": "\u00B5",
    "para": "\u00B6",
    "middot": "\u00B7",
    "cedil": "\u00B8",
    "sup1": "\u00B9",
    "ordm": "\u00BA",
    "raquo": "\u00BB",
    "frac14": "\u00BC",
    "frac12": "\u00BD",
    "frac34": "\u00BE",
    "iquest": "\u00BF",
    "Agrave": "\u00C0",
    "Aacute": "\u00C1",
    "Acirc": "\u00C2",
    "Atilde": "\u00C3",
    "Auml": "\u00C4",
    "Aring": "\u00C5",
    "AElig": "\u00C6",
    "Ccedil": "\u00C7",
    "Egrave": "\u00C8",
    "Eacute": "\u00C9",
    "Ecirc": "\u00CA",
    "Euml": "\u00CB",
    "Igrave": "\u00CC",
    "Iacute": "\u00CD",
    "Icirc": "\u00CE",
    "Iuml": "\u00CF",
    "ETH": "\u00D0",
    "Ntilde": "\u00D1",
    "Ograve": "\u00D2",
    "Oacute": "\u00D3",
    "Ocirc": "\u00D4",
    "Otilde": "\u00D5",
    "Ouml": "\u00D6",
    "times": "\u00D7",
    "Oslash": "\u00D8",
    "Ugrave": "\u00D9",
    "Uacute": "\u00DA",
    "Ucirc": "\u00DB",
    "Uuml": "\u00DC",
    "Yacute": "\u00DD",
    "THORN": "\u00DE",
    "szlig": "\u00DF",
    "agrave": "\u00E0",
    "aacute": "\u00E1",
    "acirc": "\u00E2",
    "atilde": "\u00E3",
    "auml": "\u00E4",
    "aring": "\u00E5",
    "aelig": "\u00E6",
    "ccedil": "\u00E7",
    "egrave": "\u00E8",
    "eacute": "\u00E9",
    "ecirc": "\u00EA",
    "euml": "\u00EB",
    "igrave": "\u00EC",
    "iacute": "\u00ED",
    "icirc": "\u00EE",
    "iuml": "\u00EF",
    "eth": "\u00F0",
    "ntilde": "\u00F1",
    "ograve": "\u00F2",
    "oacute": "\u00F3",
    "ocirc": "\u00F4",
    "otilde": "\u00F5",
    "ouml": "\u00F6",
    "divide": "\u00F7",
    "oslash": "\u00F8",
    "ugrave": "\u00F9",
    "uacute": "\u00FA",
    "ucirc": "\u00FB",
    "uuml": "\u00FC",
    "yacute": "\u00FD",
    "thorn": "\u00FE",
    "yuml": "\u00FF",
    # common symbols and special characters
    "OElig": "\u0152",
    "oelig": "\u0153",
    "Scaron": "\u0160",
    "scaron": "\u0161",
    "Yuml": "\u0178",
    "fnof": "\u0192",
    "circ": "\u02C6",
    "tilde": "\u02DC",
    "ensp": "\u2002",
    "emsp": "\u2003",
    "thinsp": "\u2009",
    "zwnj": "\u200C",
    "zwj": "\u200D",
    "lrm": "\u200E",
    "rlm": "\u200F",
    "ndash": "\u2013",
    "mdash": "\u2014",
    "lsquo": "\u2018",
    "rsquo": "\u2019",
    "sbquo": "\u201A",
    "ldquo": "\u201C",
    "rdquo": "\u201D",
    "bdquo": "\u201E",
    "dagger": "\u2020",
    "Dagger": "\u2021",
    "bull": "\u2022",
    "hellip": "\u2026",
    "permil": "\u2030",
    "prime": "\u2032",
    "Prime": "\u2033",
    "lsaquo": "\u2039",
    "rsaquo": "\u203A",
    "oline": "\u203E",
    "frasl": "\u2044",
    "euro": "\u20AC",
    "image": "\u2111",
    "weierp": "\u2118",
    "real": "\u211C",
    "trade": "\u2122",
    "alefsym": "\u2135",
    "larr": "\u2190",
    "uarr": "\u2191",
    "rarr": "\u2192",
    "darr": "\u2193",
    "harr": "\u2194",
    "crarr": "\u21B5",
    "lArr": "\u21D0",
    "uArr": "\u21D1",
    "rArr": "\u21D2",
    "dArr": "\u21D3",
    "hArr": "\u21D4",
    "forall": "\u2200",
    "part": "\u2202",
    "exist": "\u2203",
    "empty": "\u2205",
    "nabla": "\u2207",
    "isin": "\u2208",
    "notin": "\u2209",
    "ni": "\u220B",
    "prod": "\u220F",
    "sum": "\u2211",
    "minus": "\u2212",
    "lowast": "\u2217",
    "radic": "\u221A",
    "prop": "\u221D",
    "infin": "\u221E",
    "ang": "\u2220",
    "and": "\u2227",
    "or": "\u2228",
    "cap": "\u2229",
    "cup": "\u222A",
    "int": "\u222B",
    "there4": "\u2234",
    "sim": "\u223C",
    "cong": "\u2245",
    "asymp": "\u2248",
    "ne": "\u2260",
    "equiv": "\u2261",
    "le": "\u2264",
    "ge": "\u2265",
    "sub": "\u2282",
    "sup": "\u2283",
    "nsub": "\u2284",
    "sube": "\u2286",
    "supe": "\u2287",
    "oplus": "\u2295",
    "otimes": "\u2297",
    "perp": "\u22A5",
    "sdot": "\u22C5",
    "lceil": "\u2308",
    "rceil": "\u2309",
    "lfloor": "\u230A",
    "rfloor": "\u230B",
    "lang": "\u2329",
    "rang": "\u232A",
    "loz": "\u25CA",
    "spades": "\u2660",
    "clubs": "\u2663",
    "hearts": "\u2665",
    "diams": "\u2666",
}


def _decode_entity(entity: str):
    if len(entity) > 1 and entity[0] == "#":
        try:
            if len(entity) > 2 and entity[1] in ("x", "X"):
                code_point = int(entity[2:], 16)
            else:
                code_point = int(entity[1:], 10)
        except ValueError:
            return None
        if 0 <= code_point <= 0x10FFFF and not 0xD800 <= code_point <= 0xDFFF:
            return chr(code_point)
        return None
    return _HTML_ENTITIES.get(entity)


def _html_decode(text: str) -> str:
    if not text:
        return text
    out: list[str] = []
    i = 0
    while i < len(text):
        char = text[i]
        if char == "&":
            semicolon = text.find(";", i + 1)
            if semicolon > i + 1 and semicolon - i <= 12:
                decoded = _decode_entity(text[i + 1:semicolon])
                if decoded is not None:
                    out.append(decoded)
                    i = semicolon + 1
                    continue
        out.append(char)
        i += 1
    return "".join(out)


def _html_encode(text: str) -> str:
    if not text:
        return text
    return (
        text.replace("&", "&amp;")
        .replace("<", "&lt;")
        .replace(">", "&gt;")
        .replace('"', "&quot;")
        .replace("'", "&#39;")
    )


def _url_encode(text: str) -> str:
    out: list[str] = []
    for value in text.encode("utf-8"):
        if (
            ord("a") <= value <= ord("z")
            or ord("A") <= value <= ord("Z")
            or ord("0") <= value <= ord("9")
            or value in (ord("-"), ord("_"), ord("."), ord("~"))
        ):
            out.append(chr(value))
        elif value == ord(" "):
            out.append("+")
        else:
            out.append("%" + _HEX[value >> 4] + _HEX[value & 0x0F])
    return "".join(out)
