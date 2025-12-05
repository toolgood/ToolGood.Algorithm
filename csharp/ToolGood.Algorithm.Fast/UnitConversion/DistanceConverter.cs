namespace ToolGood.Algorithm.UnitConversion
{
    internal sealed class DistanceConverter : BaseUnitConverter
    {
        private static UnitFactors units = new UnitFactors()
        {
            { new UnitFactorSynonyms("m", "metre","米"), 1 },
            { new UnitFactorSynonyms("km", "kilometre","千米"), 0.001m },
            { new UnitFactorSynonyms("dm","decimetre", "分米"), 10 },
            { new UnitFactorSynonyms("cm", "centimetre", "厘米"), 100 },
            { new UnitFactorSynonyms("mm", "millimetre", "毫米"), 1000 },
            { new UnitFactorSynonyms("ft", "foot", "feet","英尺"), 1250m / 381 },
            { new UnitFactorSynonyms("yd", "yard","码"), 1250m / 1143 },
            { new UnitFactorSynonyms("mile","英里"), 125m / 201168 },
            { new UnitFactorSynonyms("in", "inch","英寸"), 5000m / 127 },
            { "au", 1m / 149600000000}
        };

        public DistanceConverter(string leftUnit, string rightUnit)
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