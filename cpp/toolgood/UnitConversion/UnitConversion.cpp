// UnitConversion.cpp - 对应 csharp/ToolGood.Algorithm/UnitConversion/*.cs
//
// 说明（与 C# 的差异）：
//   C# 的换算系数是 decimal 常量，编译期按 28~29 位有效数字计算；
//   C++ 的 Decimal::Div 按 DECIMAL128（34 位）HALF_EVEN 舍入，
//   因此 `1250m / 381` 这类系数的末几位可能不同，相对误差 < 1e-28。
#include "UnitConversion.h"

#include <map>

namespace toolgood {

namespace {

/// 不区分大小写的比较器（对应 C# StringComparer.OrdinalIgnoreCase）。
struct CaseInsensitiveLess {
    bool operator()(const std::string& a, const std::string& b) const {
        const size_t n = a.size() < b.size() ? a.size() : b.size();
        for (size_t i = 0; i < n; i++) {
            unsigned char ca = static_cast<unsigned char>(a[i]);
            unsigned char cb = static_cast<unsigned char>(b[i]);
            if (ca >= 'A' && ca <= 'Z') ca = static_cast<unsigned char>(ca + 32);
            if (cb >= 'A' && cb <= 'Z') cb = static_cast<unsigned char>(cb + 32);
            if (ca != cb) return ca < cb;
        }
        return a.size() < b.size();
    }
};

using UnitMap = std::map<std::string, Decimal, CaseInsensitiveLess>;

/// 公用的换算实现：right = left / l * r
bool TryConvertIn(const UnitMap& units, const std::string& leftSynonym,
                  const std::string& rightSynonym, const Decimal& left, Decimal& right) {
    auto l = units.find(leftSynonym);
    if (l != units.end()) {
        auto r = units.find(rightSynonym);
        if (r != units.end()) {
            right = left.Div(l->second).Mul(r->second);
            return true;
        }
    }
    right = Decimal::Zero;
    return false;
}

/// 整数比（对应 C# 里 `1250m / 381` 这类常量）。
Decimal Ratio(int64_t numerator, int64_t denominator) {
    return Decimal::FromInt(numerator).Div(Decimal::FromInt(denominator));
}

/// 字符串比值（对应 C# 里 `1m / 28.316846592m`）。
Decimal DivByText(const Decimal& value, const std::string& text) {
    return value.Div(Decimal::FromString(text));
}

const UnitMap& DistanceUnits() {
    static const UnitMap units = [] {
        UnitMap m;
        const Decimal one = Decimal::One;
        const Decimal ft = Ratio(1250, 381);
        const Decimal yd = Ratio(1250, 1143);
        const Decimal mile = Ratio(125, 201168);
        const Decimal inch = Ratio(5000, 127);
        const Decimal au = Ratio(1, 149600000000LL);

        for (const char* key : {"m", "metre", "meter", "米"}) m[key] = one;
        for (const char* key : {"km", "kilometre", "kilometer", "千米"})
            m[key] = Decimal::FromString("0.001");
        for (const char* key : {"dm", "decimetre", "decimeter", "分米"}) m[key] = Decimal::FromInt(10);
        for (const char* key : {"cm", "centimetre", "centimeter", "厘米"}) m[key] = Decimal::FromInt(100);
        for (const char* key : {"mm", "millimetre", "millimeter", "毫米"}) m[key] = Decimal::FromInt(1000);
        for (const char* key : {"ft", "foot", "feet", "英尺"}) m[key] = ft;
        for (const char* key : {"yd", "yard", "码"}) m[key] = yd;
        for (const char* key : {"mile", "英里"}) m[key] = mile;
        for (const char* key : {"in", "inch", "英寸"}) m[key] = inch;
        m["au"] = au;
        m["尺"] = Decimal::FromInt(3);
        m["寸"] = Decimal::FromInt(30);
        return m;
    }();
    return units;
}

const UnitMap& AreaUnits() {
    static const UnitMap units = [] {
        UnitMap m;
        const Decimal one = Decimal::One;
        const Decimal ft2 = DivByText(DivByText(one, "0.3048"), "0.3048");
        const Decimal yd2 = DivByText(DivByText(one, "0.9144"), "0.9144");
        const Decimal in2 = DivByText(one, "0.00064516");
        const Decimal mi2 = DivByText(one, "2589988.110336");

        for (const char* key : {"m²", "m2", "square metre", "square meter", "centiare", "平方米",
                                "平方公尺"})
            m[key] = one;
        for (const char* key : {"km²", "km2", "square kilometre", "square kilometer", "平方千米"})
            m[key] = Decimal::FromString("0.000001");
        for (const char* key : {"dm²", "dm2", "square decimetre", "square decimeter", "平方分米"})
            m[key] = Decimal::FromInt(100);
        for (const char* key : {"cm²", "cm2", "square centimetre", "square centimeter", "平方厘米"})
            m[key] = Decimal::FromInt(10000);
        for (const char* key : {"mm²", "mm2", "square millimetre", "square millimeter", "平方毫米"})
            m[key] = Decimal::FromInt(1000000);
        for (const char* key : {"ft²", "ft2", "square foot", "square feet", "sq ft", "平方英尺"})
            m[key] = ft2;
        for (const char* key : {"yd²", "yd2", "sq yd", "square yard", "平方码"}) m[key] = yd2;
        for (const char* key : {"a", "are"}) m[key] = Decimal::FromString("0.01");
        for (const char* key : {"ha", "hectare", "公顷"}) m[key] = Decimal::FromString("0.0001");
        for (const char* key : {"in²", "in2", "sq in", "square inch", "平方英寸"}) m[key] = in2;
        for (const char* key : {"mi²", "mi2", "sq mi", "square mile", "平方英里"}) m[key] = mi2;
        m["亩"] = Ratio(3, 2000);
        return m;
    }();
    return units;
}

const UnitMap& VolumeUnits() {
    static const UnitMap units = [] {
        UnitMap m;
        const Decimal one = Decimal::One;
        const Decimal k3 = Decimal::FromString("0.001");
        const Decimal km3 = k3.Mul(k3).Mul(k3).Mul(k3);  // 0.001m * 0.001m * 0.001m * 0.001m
        const Decimal ft3 = DivByText(one, "28.316846592");
        const Decimal in3 = Ratio(125000000, 2048383);
        const Decimal imperialPint = Ratio(800000, 454609);
        const Decimal imperialGallon = Ratio(100000, 454609);
        const Decimal imperialQuart = Ratio(400000, 454609);
        const Decimal usPint = Ratio(1000000000, 473176473);
        const Decimal usGallon = Ratio(125000000, 473176473);
        const Decimal usQuart = Ratio(1000000000, 946352946);

        for (const char* key : {"l", "lt", "ltr", "liter", "litre"}) m[key] = one;
        for (const char* key : {"dm³", "dm3", "cubic decimetre", "cubic decimeter", "升", "立方分米"})
            m[key] = one;
        for (const char* key : {"m³", "m3", "cubic metre", "cubic meter", "立方米"}) m[key] = k3;
        for (const char* key : {"km³", "km3", "cubic kilometre", "cubic kilometer", "立方千米"})
            m[key] = km3;
        for (const char* key : {"cm³", "cm3", "cubic centimetre", "cubic centimeter", "立方厘米",
                                "毫升", "ml"})
            m[key] = Decimal::FromInt(1000);
        for (const char* key : {"mm³", "mm3", "cubic millimetre", "cubic millimeter", "立方毫米"})
            m[key] = Decimal::FromInt(1000000);
        for (const char* key : {"ft³", "ft3", "cubic foot", "cubic feet", "立方英尺", "cu ft"})
            m[key] = ft3;
        for (const char* key : {"in³", "in3", "cubic in", "cubic inch", "立方英寸"}) m[key] = in3;

        for (const char* key : {"imperial pint", "imperial pt", "imperial p"})
            m[key] = imperialPint;
        for (const char* key : {"imperial gallon", "imperial gal"}) m[key] = imperialGallon;
        for (const char* key : {"imperial quart", "imperial qt"}) m[key] = imperialQuart;
        for (const char* key : {"US pint", "US pt", "US p"}) m[key] = usPint;
        for (const char* key : {"US gallon", "US gal"}) m[key] = usGallon;
        for (const char* key : {"US quart", "US qt"}) m[key] = usQuart;
        return m;
    }();
    return units;
}

const UnitMap& MassUnits() {
    static const UnitMap units = [] {
        UnitMap m;
        const Decimal one = Decimal::One;
        const Decimal ton = Ratio(1, 1000);
        const Decimal pound = Ratio(100000000, 45359237);
        const Decimal stone = Ratio(50000000, 317514659);
        const Decimal ounce = Ratio(1600000000, 45359237);
        const Decimal shortTon = Ratio(50000, 45359237);
        const Decimal longTon = Ratio(312500, 317514659);

        for (const char* key : {"kg", "kilogram", "千克"}) m[key] = one;
        for (const char* key : {"gram", "g", "克"}) m[key] = Decimal::FromInt(1000);
        for (const char* key : {"ton", "t", "吨"}) m[key] = ton;
        for (const char* key : {"lb", "lbs", "pound", "pounds", "磅", "英磅"}) m[key] = pound;
        for (const char* key : {"st", "stone", "石"}) m[key] = stone;
        for (const char* key : {"oz", "ounce", "盎司"}) m[key] = ounce;
        for (const char* key : {"quintal", "英担"}) m[key] = Decimal::FromString("0.01");
        for (const char* key : {"short ton", "net ton", "us ton", "短吨", "美吨"}) m[key] = shortTon;
        for (const char* key : {"long ton", "weight ton", "gross ton", "imperial ton", "长吨", "英吨"})
            m[key] = longTon;
        for (const char* key : {"mg", "毫克"}) m[key] = Decimal::FromInt(1000000);
        m["斤"] = Decimal::FromInt(2);
        m["两"] = Decimal::FromInt(20);
        return m;
    }();
    return units;
}

}  // namespace

bool DistanceConverter::TryConvert(const std::string& leftSynonym, const std::string& rightSynonym,
                                   const Decimal& left, Decimal& right) {
    return TryConvertIn(DistanceUnits(), leftSynonym, rightSynonym, left, right);
}

bool AreaConverter::TryConvert(const std::string& leftSynonym, const std::string& rightSynonym,
                               const Decimal& left, Decimal& right) {
    return TryConvertIn(AreaUnits(), leftSynonym, rightSynonym, left, right);
}

bool VolumeConverter::TryConvert(const std::string& leftSynonym, const std::string& rightSynonym,
                                 const Decimal& left, Decimal& right) {
    return TryConvertIn(VolumeUnits(), leftSynonym, rightSynonym, left, right);
}

bool MassConverter::TryConvert(const std::string& leftSynonym, const std::string& rightSynonym,
                               const Decimal& left, Decimal& right) {
    return TryConvertIn(MassUnits(), leftSynonym, rightSynonym, left, right);
}

}  // namespace toolgood
