namespace ToolGood.Algorithm.UnitConversion
{
    internal sealed class AreaConverter : BaseUnitConverter
    {
        private static UnitFactors units = new UnitFactors()
        {
            { new UnitFactorSynonyms("m²", "m2", "square metre","square meter", "centiare","平方米","平方公尺"), 1 },
            { new UnitFactorSynonyms("km²", "km2", "square kilometre","square kilometer","平方千米"), 0.000001m },
            { new UnitFactorSynonyms("dm²", "dm2", "square decimetre","square decimeter","平方分米"), 100 },
            { new UnitFactorSynonyms("cm²", "cm2", "square centimetre","square centimeter","平方厘米"), 10000 },
            { new UnitFactorSynonyms("mm²", "mm2", "square millimetre", "square millimeter","平方毫米"), 1000000 },
            { new UnitFactorSynonyms("ft²", "ft2", "square foot", "square feet", "sq ft","平方英尺"), 1m /  0.3048m /  0.3048m },
            { new UnitFactorSynonyms("yd²", "yd2", "sq yd", "square yard","平方码"), 1m /  0.9144m /  0.9144m},
            { new UnitFactorSynonyms("a", "are"), 0.01m },
            { new UnitFactorSynonyms("ha", "hectare","公顷"), 0.0001m },
            { new UnitFactorSynonyms("in²", "in2", "sq in", "square inch","平方英寸"), 1m / 0.00064516m },
            { new UnitFactorSynonyms("mi²", "mi2", "sq mi", "square mile","平方英里"), 1m / 2589988.110336m },
            { new UnitFactorSynonyms( "亩"), 1m / 666.667m },
        };

        public AreaConverter(string leftUnit, string rightUnit)
        {
            Instantiate(units, leftUnit, rightUnit);
        }

        public static bool Exists(string leftSynonym, string rightSynonym)
        {
            if (units.FindUnit(leftSynonym) != null) {
                return units.FindUnit(rightSynonym) != null;
            }
            return false;
        }
    }
}